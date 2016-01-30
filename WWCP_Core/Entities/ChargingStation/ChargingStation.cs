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
using System.Diagnostics;
using System.Threading.Tasks;
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
        public const UInt16 DefaultMaxStationStatusListSize         = 15;

        /// <summary>
        /// The default max size of the charging station admin status list.
        /// </summary>
        public const UInt16 DefaultMaxStationAdminStatusListSize    = 15;

        #endregion

        #region Properties

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

                if (_Name == null || !_Name.Any())
                    return _ChargingPool.Name;

                return _Name;

            }

            set
            {

                if (_Name != value && _Name != _ChargingPool.Name)
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

        #region Brand

        private Brand _Brand;

        /// <summary>
        /// A brand for this charging station
        /// is this is different from the EVSE operator.
        /// </summary>
        [Optional]
        public Brand Brand
        {

            get
            {
                return _Brand;
            }

            set
            {

                if (_Brand != value)
                    SetProperty<Brand>(ref _Brand, value);

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

        #region OpeningTimes

        internal OpeningTimes _OpeningTimes;

        /// <summary>
        /// The opening times of this charging station.
        /// </summary>
        public OpeningTimes OpeningTimes
        {

            get
            {

                return _OpeningTimes != null
                    ? _OpeningTimes
                    : ChargingPool.OpeningTimes;

            }

            set
            {

                if (value == ChargingPool.OpeningTimes)
                    return;

                if (OpeningTimes != value)
                {

                    if (value == null)
                        DeleteProperty(ref _OpeningTimes);

                    else
                        SetProperty(ref _OpeningTimes, value);

                }

            }

        }

        #endregion

        #region AuthenticationModes

        internal ReactiveSet<AuthenticationMode> _AuthenticationModes;

        public ReactiveSet<AuthenticationMode> AuthenticationModes
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
                        _AuthenticationModes = new ReactiveSet<AuthenticationMode>();

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

        #endregion

        #region Links

        #region RemoteChargingStation

        private IRemoteChargingStation _RemoteChargingStation;

        /// <summary>
        /// An optional remote charging station.
        /// </summary>
        public IRemoteChargingStation RemoteChargingStation
        {

            get
            {
                return _RemoteChargingStation;
            }

            internal set
            {
                _RemoteChargingStation = value;
            }

        }

        #endregion

        #region ChargingPool

        private readonly ChargingPool _ChargingPool;

        /// <summary>
        /// The charging pool.
        /// </summary>
        [InternalUseOnly]
        public ChargingPool ChargingPool
        {
            get
            {
                return _ChargingPool;
            }
        }

        #endregion

        #region Operator

        /// <summary>
        /// The EVSE operator of this EVSE.
        /// </summary>
        [InternalUseOnly]
        public EVSEOperator Operator
        {
            get
            {
                return ChargingPool.Operator;
            }
        }

        #endregion

        #endregion

        #region Events

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

            this._ChargingPool               = ChargingPool;

            this._EVSEs                      = new HashSet<EVSE>();

            this.Name                        = new I18NString();
            this.Description                 = new I18NString();

            this._UserComment                = new I18NString();
            this._ServiceProviderComment     = new I18NString();
            //this.GeoLocation                 = new GeoCoordinate();

            this._PaymentOptions             = new ReactiveSet<PaymentOptions>();

            this._StatusSchedule             = new StatusSchedule<ChargingStationStatusType>(MaxStationStatusListSize);
            this._StatusSchedule.Insert(ChargingStationStatusType.Unspecified);

            this._AdminStatusSchedule        = new StatusSchedule<ChargingStationAdminStatusType>(MaxStationAdminStatusListSize);
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


        #region EVSEs...

        #region EVSEs

        private readonly HashSet<EVSE> _EVSEs;

        /// <summary>
        /// All Electric Vehicle Supply Equipments (EVSE) present
        /// within this charging station.
        /// </summary>
        public IEnumerable<EVSE> EVSEs
        {
            get
            {
                return _EVSEs;
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
                return _EVSEs.Select(evse => evse.Id);
            }
        }

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
                throw new ArgumentNullException(nameof(EVSEId), "The given EVSE identification must not be null!");

            if (_EVSEs.Any(evse => evse.Id == EVSEId))
            {
                if (OnError == null)
                    throw new EVSEAlreadyExistsInStation(EVSEId, this.Id);
                else
                    OnError.FailSafeInvoke(this, EVSEId);
            }

            #endregion

            var Now   = DateTime.Now;
            var _EVSE = new EVSE(EVSEId, this);

            if (EVSEAddition.SendVoting(Now, this, _EVSE))
            {
                if (_EVSEs.Add(_EVSE))
                {

                    _EVSE.OnPropertyChanged       += (Timestamp, Sender, PropertyName, OldValue, NewValue)
                                                      => UpdateEVSEData       (Timestamp, Sender as EVSE, PropertyName, OldValue, NewValue);

                    _EVSE.OnStatusChanged         += (Timestamp, EVSE, OldEVSEStatus, NewEVSEStatus)
                                                      => UpdateEVSEStatus     (Timestamp, EVSE, OldEVSEStatus, NewEVSEStatus);

                    _EVSE.OnAdminStatusChanged    += (Timestamp, EVSE, OldEVSEStatus, NewEVSEStatus)
                                                      => UpdateEVSEAdminStatus(Timestamp, EVSE, OldEVSEStatus, NewEVSEStatus);

                    _EVSE.OnNewReservation        += SendNewReservation;
                    _EVSE.OnNewChargingSession    += SendNewChargingSession;
                    _EVSE.OnNewChargeDetailRecord += SendNewChargeDetailRecord;

                    OnSuccess.FailSafeInvoke(_EVSE);
                    EVSEAddition.SendNotification(Now, this, _EVSE);
                    UpdateEVSEStatus(Now, _EVSE, new Timestamped<EVSEStatusType>(Now, EVSEStatusType.Unspecified), _EVSE.Status);

                    if (_RemoteChargingStation != null)
                    {

                        _EVSE.RemoteEVSE = _RemoteChargingStation.CreateNewEVSE(EVSEId);

                        _EVSE.OnStatusChanged                    += (Timestamp, RemoteEVSE, OldStatus, NewStatus) => _EVSE.RemoteEVSE.Status           = NewStatus;
                        _EVSE.OnAdminStatusChanged               += (Timestamp, RemoteEVSE, OldStatus, NewStatus) => _EVSE.RemoteEVSE.AdminStatus      = NewStatus;
                        _EVSE.OnNewReservation                   += (Timestamp, RemoteEVSE, Reservation)          => _EVSE.RemoteEVSE.Reservation      = Reservation;
                        _EVSE.OnNewChargingSession               += (Timestamp, RemoteEVSE, ChargingSession)      => _EVSE.RemoteEVSE.ChargingSession  = ChargingSession;

                        _EVSE.RemoteEVSE.OnStatusChanged         += (Timestamp, RemoteEVSE, OldStatus, NewStatus) => _EVSE.Status                      = NewStatus;
                        _EVSE.RemoteEVSE.OnAdminStatusChanged    += (Timestamp, RemoteEVSE, OldStatus, NewStatus) => _EVSE.AdminStatus                 = NewStatus;
                        _EVSE.RemoteEVSE.OnNewReservation        += (Timestamp, RemoteEVSE, Reservation)          => _EVSE.Reservation                 = Reservation;
                        _EVSE.RemoteEVSE.OnNewChargingSession    += (Timestamp, RemoteEVSE, ChargingSession)      => _EVSE.ChargingSession             = ChargingSession;
                        _EVSE.RemoteEVSE.OnNewChargeDetailRecord += (Timestamp, RemoteEVSE, ChargeDetailRecord)   => _EVSE.SendNewChargeDetailRecord(Timestamp, RemoteEVSE, ChargeDetailRecord);

                    }

                    Configurator.FailSafeInvoke(_EVSE);

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
            return _EVSEs.Any(evse => evse.Id == EVSE.Id);
        }

        #endregion

        #region ContainsEVSE(EVSEId)

        /// <summary>
        /// Check if the given EVSE identification is already present within the charging station.
        /// </summary>
        /// <param name="EVSEId">The unique identification of an EVSE.</param>
        public Boolean ContainsEVSE(EVSE_Id EVSEId)
        {
            return _EVSEs.Any(evse => evse.Id == EVSEId);
        }

        #endregion

        #region GetEVSEbyId(EVSEId)

        public EVSE GetEVSEbyId(EVSE_Id EVSEId)
        {
            return _EVSEs.Where(evse => evse.Id == EVSEId).FirstOrDefault();
        }

        #endregion

        #region TryGetEVSEbyId(EVSEId, out EVSE)

        public Boolean TryGetEVSEbyId(EVSE_Id EVSEId, out EVSE EVSE)
        {

            EVSE = GetEVSEbyId(EVSEId);

            return EVSE != null;

        }

        #endregion

        #region RemoveEVSE(EVSEId)

        public EVSE RemoveEVSE(EVSE_Id EVSEId)
        {

            var _EVSE = GetEVSEbyId(EVSEId);

            if (_EVSE != null && _EVSEs.Remove(_EVSE))
                return _EVSE;

            return null;

        }

        #endregion

        #region TryRemoveEVSE(EVSEId, out EVSE)

        public Boolean TryRemoveEVSE(EVSE_Id EVSEId, out EVSE EVSE)
        {

            EVSE = GetEVSEbyId(EVSEId);

            if (EVSE == null)
                return false;

            return _EVSEs.Remove(EVSE);

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
                                       StatusAggregationDelegate(new EVSEStatusReport(_EVSEs)));

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
            return _EVSEs.GetEnumerator();
        }

        public IEnumerator<EVSE> GetEnumerator()
        {
            return _EVSEs.GetEnumerator();
        }

        #endregion

        #endregion


        #region Reservations...

        /// <summary>
        /// The maximum time span for a reservation.
        /// </summary>
        public static readonly TimeSpan MaxReservationDuration = TimeSpan.FromMinutes(30);

        #region ChargingReservations

        /// <summary>
        /// Return all current charging reservations.
        /// </summary>
        public IEnumerable<ChargingReservation> ChargingReservations
        {
            get
            {

                return _EVSEs.
                           Select(evse        => evse.Reservation).
                           Where (reservation => reservation != null);

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

            ReservationResult result = null;

            if (EventTrackingId == null)
                EventTrackingId = EventTracking_Id.New;

            #endregion

            #region Send OnReserveEVSE event

            var OnReserveEVSELocal = OnReserveEVSE;
            if (OnReserveEVSELocal != null)
                OnReserveEVSELocal(this,
                                   Timestamp,
                                   EventTrackingId,
                                   _ChargingPool.Operator.RoamingNetwork.Id,
                                   ReservationId,
                                   EVSEId,
                                   StartTime,
                                   Duration,
                                   ProviderId,
                                   ChargingProductId,
                                   AuthTokens,
                                   eMAIds,
                                   PINs);

            #endregion


            var _EVSE = _EVSEs.Where(evse => evse.Id == EVSEId).
                               FirstOrDefault();

            if (_EVSE != null)
            {

                result = await _EVSE.Reserve(Timestamp,
                                             CancellationToken,
                                             EventTrackingId,
                                             StartTime,
                                             Duration,
                                             ReservationId,
                                             ProviderId,
                                             ChargingProductId,
                                             AuthTokens,
                                             eMAIds,
                                             PINs,
                                             QueryTimeout);

            }

            else
                result = ReservationResult.UnknownEVSE;


            #region Send OnEVSEReserved event

            var OnEVSEReservedLocal = OnEVSEReserved;
            if (OnEVSEReservedLocal != null)
                OnEVSEReservedLocal(this,
                                    Timestamp,
                                    EventTrackingId,
                                    _ChargingPool.Operator.RoamingNetwork.Id,
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

            #endregion

            return result;

        }

        #endregion

        #region Reserve(...StartTime, Duration, ReservationId = null, ProviderId = null, ...)

        /// <summary>
        /// Reserve the possibility to charge.
        /// </summary>
        /// <param name="Timestamp">The timestamp of this request.</param>
        /// <param name="CancellationToken">A token to cancel this request.</param>
        /// <param name="EventTrackingId">An unique event tracking identification for correlating this request with other events.</param>
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

            ReservationResult result = null;

            if (EventTrackingId == null)
                EventTrackingId = EventTracking_Id.New;

            #endregion

            #region Send OnReserveChargingStation event

            var OnReserveChargingStationLocal = OnReserveChargingStation;
            if (OnReserveChargingStationLocal != null)
                OnReserveChargingStationLocal(this,
                                              Timestamp,
                                              EventTrackingId,
                                              _ChargingPool.Operator.RoamingNetwork.Id,
                                              Id,
                                              StartTime,
                                              Duration,
                                              ReservationId,
                                              ProviderId,
                                              ChargingProductId,
                                              AuthTokens,
                                              eMAIds,
                                              PINs);

            #endregion


            if (_RemoteChargingStation != null)
            {

                result = await _RemoteChargingStation.
                                   Reserve(Timestamp,
                                           CancellationToken,
                                           EventTrackingId,
                                           StartTime,
                                           Duration,
                                           ReservationId,
                                           ProviderId,
                                           ChargingProductId,
                                           AuthTokens,
                                           eMAIds,
                                           PINs,
                                           QueryTimeout);

            }

            else
                result = ReservationResult.Offline;


            #region Send OnChargingStationReserved event

            var OnChargingStationReservedLocal = OnChargingStationReserved;
            if (OnChargingStationReservedLocal != null)
                OnChargingStationReservedLocal(this,
                                               Timestamp,
                                               EventTrackingId,
                                               _ChargingPool.Operator.RoamingNetwork.Id,
                                               Id,
                                               StartTime,
                                               Duration,
                                               ReservationId,
                                               ProviderId,
                                               ChargingProductId,
                                               AuthTokens,
                                               eMAIds,
                                               PINs,
                                               result);

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

            Reservation = _EVSEs.Where (evse => evse.Reservation    != null &&
                                                evse.Reservation.Id == ReservationId).
                                 Select(evse => evse.Reservation).
                                 FirstOrDefault();

            return Reservation != null;

        }

        #endregion


        #region OnReservationDeleted

        /// <summary>
        /// An event fired whenever a charging reservation was deleted.
        /// </summary>
        public event OnReservationCancelledDelegate OnReservationDeleted;

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

            #region Initial checks

            if (ReservationId == null)
                throw new ArgumentNullException(nameof(ReservationId), "The given charging reservation identification must not be null!");

            #endregion


            return await _EVSEs.Where   (evse => evse.Reservation    != null &&
                                                 evse.Reservation.Id == ReservationId).
                                MapFirst(evse => evse.CancelReservation(ReservationId, ReservationCancellation),
                                         Task.FromResult(false));

        }

        #endregion

        #endregion

        #region RemoteStart/-Stop

        #region ChargingSessions

        /// <summary>
        /// Return all current charging sessions.
        /// </summary>
        public IEnumerable<ChargingSession> ChargingSessions
        {
            get
            {

                return _EVSEs.
                           Select(evse    => evse.ChargingSession).
                           Where (session => session != null);

            }
        }

        #endregion

        #region OnRemote...Start / OnRemote...Started / OnNewChargingSession

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

        /// <summary>
        /// An event fired whenever a new charging session was created.
        /// </summary>
        public event OnNewChargingSessionDelegate            OnNewChargingSession;

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
                throw new ArgumentNullException(nameof(EVSEId), "The given EVSE identification must not be null!");

            RemoteStartEVSEResult result = null;

            if (EventTrackingId == null)
                EventTrackingId = EventTracking_Id.New;

            #endregion

            #region Send OnRemoteEVSEStart event

            var OnRemoteEVSEStartLocal = OnRemoteEVSEStart;
            if (OnRemoteEVSEStartLocal != null)
                OnRemoteEVSEStartLocal(Timestamp,
                                       this,
                                       EventTrackingId,
                                       _ChargingPool.Operator.RoamingNetwork.Id,
                                       EVSEId,
                                       ChargingProductId,
                                       ReservationId,
                                       SessionId,
                                       ProviderId,
                                       eMAId,
                                       QueryTimeout.Value);

            #endregion


            var _EVSE = _EVSEs.Where(evse => evse.Id == EVSEId).
                               FirstOrDefault();

            if (_EVSE != null)
            {

                result = await _EVSE.RemoteStart(Timestamp,
                                                 CancellationToken,
                                                 EventTrackingId,
                                                 ChargingProductId,
                                                 ReservationId,
                                                 SessionId,
                                                 ProviderId,
                                                 eMAId,
                                                 QueryTimeout);

                if (result.Result == RemoteStartEVSEResultType.Success)
                    result.Session.ChargingStation = this;

            }

            else
                result = RemoteStartEVSEResult.UnknownEVSE;


            #region Send OnRemoteEVSEStarted event

            var OnRemoteEVSEStartedLocal = OnRemoteEVSEStarted;
            if (OnRemoteEVSEStartedLocal != null)
                OnRemoteEVSEStartedLocal(Timestamp,
                                         this,
                                         EventTrackingId,
                                         _ChargingPool.Operator.RoamingNetwork.Id,
                                         EVSEId,
                                         ChargingProductId,
                                         ReservationId,
                                         SessionId,
                                         ProviderId,
                                         eMAId,
                                         QueryTimeout,
                                         result);

            #endregion

            return result;

        }

        #endregion

        #region RemoteStart(...ChargingProductId = null, ReservationId = null, SessionId = null, ProviderId = null, eMAId = null, ...)

        /// <summary>
        /// Start a charging session at the given charging station.
        /// </summary>
        /// <param name="Timestamp">The timestamp of the request.</param>
        /// <param name="CancellationToken">A token to cancel this request.</param>
        /// <param name="EventTrackingId">An unique event tracking identification for correlating this request with other events.</param>
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
                        ChargingProduct_Id      ChargingProductId  = null,
                        ChargingReservation_Id  ReservationId      = null,
                        ChargingSession_Id      SessionId          = null,
                        EVSP_Id                 ProviderId         = null,
                        eMA_Id                  eMAId              = null,
                        TimeSpan?               QueryTimeout       = null)

        {

            #region Initial checks

            if (EventTrackingId == null)
                EventTrackingId = EventTracking_Id.New;

            #endregion

            #region Send OnRemoteChargingStationStart event

            var OnRemoteChargingStationStartLocal = OnRemoteChargingStationStart;
            if (OnRemoteChargingStationStartLocal != null)
                OnRemoteChargingStationStartLocal(Timestamp,
                                                  this,
                                                  EventTrackingId,
                                                  ChargingPool.Operator.RoamingNetwork.Id,
                                                  Id,
                                                  ChargingProductId,
                                                  ReservationId,
                                                  SessionId,
                                                  ProviderId,
                                                  eMAId,
                                                  QueryTimeout.Value);

            #endregion


            RemoteStartChargingStationResult result = null;


            if (_RemoteChargingStation != null)
                result = await _RemoteChargingStation.
                                   RemoteStart(Timestamp,
                                               CancellationToken,
                                               EventTrackingId,
                                               ChargingProductId,
                                               ReservationId,
                                               SessionId,
                                               ProviderId,
                                               eMAId);

            //if (result == null)
            //{

            //    var OnRemoteStartChargingStationLocal = OnRemoteStartChargingStation;
            //    if (OnRemoteStartChargingStationLocal == null)
            //        return RemoteStartChargingStationResult.Error("");

            //    var results = await Task.WhenAll(OnRemoteStartChargingStationLocal.
            //                                         GetInvocationList().
            //                                         Select(subscriber => (subscriber as OnRemoteStartChargingStationDelegate)
            //                                             (Timestamp,
            //                                              this,
            //                                              CancellationToken,
            //                                              EventTrackingId,
            //                                              Id,
            //                                              ChargingProductId,
            //                                              ReservationId,
            //                                              SessionId,
            //                                              ProviderId,
            //                                              eMAId,
            //                                              QueryTimeout)));

            //    result = results.
            //                 Where(result2 => result2.Result != RemoteStartChargingStationResultType.Unspecified).
            //                 First();

            //}

            //if (result == null)
            //{

            //    var _AvailableEVSE = _EVSEs.Where(evse => evse.Status.Value == EVSEStatusType.Available).
            //                                FirstOrDefault();

            //    if (_AvailableEVSE != null)
            //    {

            //        RemoteStartEVSEResult EVSEStartResult = null;

            //        EVSEStartResult = await RemoteStart(Timestamp,
            //                                            CancellationToken,
            //                                            EventTrackingId,
            //                                            _AvailableEVSE.Id,
            //                                            ChargingProductId,
            //                                            ReservationId,
            //                                            SessionId,
            //                                            ProviderId,
            //                                            eMAId);

            //        switch (EVSEStartResult.Result)
            //        {

            //            case RemoteStartEVSEResultType.Error:
            //                result = RemoteStartChargingStationResult.Error(EVSEStartResult.ErrorMessage);
            //                break;



            //        }

            //    }

            //}


            #region Send OnRemoteChargingStationStarted event

            var OnRemoteChargingStationStartedLocal = OnRemoteChargingStationStarted;
            if (OnRemoteChargingStationStartedLocal != null)
                OnRemoteChargingStationStartedLocal(Timestamp,
                                                    this,
                                                    EventTrackingId,
                                                    ChargingPool.Operator.RoamingNetwork.Id,
                                                    Id,
                                                    ChargingProductId,
                                                    ReservationId,
                                                    SessionId,
                                                    ProviderId,
                                                    eMAId,
                                                    QueryTimeout,
                                                    result);

            #endregion

            return result;

        }

        #endregion

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


        #region OnRemote...Stop / OnRemote...Stopped / OnNewChargeDetailRecord

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

        /// <summary>
        /// An event fired whenever a new charge detail record was created.
        /// </summary>
        public event OnNewChargeDetailRecordDelegate         OnNewChargeDetailRecord;

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

            if (EventTrackingId == null)
                EventTrackingId = EventTracking_Id.New;

            #endregion

            #region Send OnRemoteStop event

            var OnRemoteStopLocal = OnRemoteStop;
            if (OnRemoteStopLocal != null)
                OnRemoteStopLocal(this,
                                  Timestamp,
                                  EventTrackingId,
                                  _ChargingPool.Operator.RoamingNetwork.Id,
                                  SessionId,
                                  ReservationHandling,
                                  ProviderId,
                                  QueryTimeout.Value);

            #endregion


            var result = RemoteStopResult.Error(SessionId);

            if (_RemoteChargingStation != null)
                result = await _RemoteChargingStation.
                                   RemoteStop(Timestamp,
                                              CancellationToken,
                                              EventTrackingId,
                                              SessionId,
                                              ReservationHandling,
                                              ProviderId);


            var ChargingSession = ChargingSessions.
                                      Where(session => session.Id == SessionId).
                                      FirstOrDefault();

            if (ChargingSession == null)
                result = RemoteStopResult.InvalidSessionId(SessionId);

            if (result == null && ChargingSession.EVSE == null)
                result = RemoteStopResult.Error(SessionId, "Unkown EVSE for the given charging session identification!");

            if (result == null)
            {

                var result2 = await ChargingSession.
                                        EVSE.
                                        RemoteStop(Timestamp,
                                                   CancellationToken,
                                                   EventTrackingId,
                                                   SessionId,
                                                   ReservationHandling,
                                                   ProviderId);

                switch (result2.Result)
                {

                    case RemoteStopEVSEResultType.Error:
                        result = RemoteStopResult.Error(SessionId, result2.ErrorMessage);
                        break;


                }

            }


            #region Send OnRemoteStopped event

            var OnRemoteStoppedLocal = OnRemoteStopped;
            if (OnRemoteStoppedLocal != null)
                OnRemoteStoppedLocal(this,
                                     Timestamp,
                                     EventTrackingId,
                                     _ChargingPool.Operator.RoamingNetwork.Id,
                                     SessionId,
                                     ReservationHandling,
                                     ProviderId,
                                     QueryTimeout,
                                     result);

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

            RemoteStopEVSEResult result = null;

            if (EventTrackingId == null)
                EventTrackingId = EventTracking_Id.New;

            #endregion

            #region Send OnRemoteEVSEStop event

            var OnRemoteEVSEStopLocal = OnRemoteEVSEStop;
            if (OnRemoteEVSEStopLocal != null)
                OnRemoteEVSEStopLocal(this,
                                      Timestamp,
                                      EventTrackingId,
                                      _ChargingPool.Operator.RoamingNetwork.Id,
                                      EVSEId,
                                      SessionId,
                                      ReservationHandling,
                                      ProviderId,
                                      QueryTimeout.Value);

            #endregion


            var _EVSE = _EVSEs.Where(evse => evse.Id == EVSEId).
                               FirstOrDefault();

            if (_EVSE != null)
            {

                result = await _EVSE.RemoteStop(Timestamp,
                                                CancellationToken,
                                                EventTrackingId,
                                                SessionId,
                                                ReservationHandling,
                                                ProviderId,
                                                QueryTimeout);

            }

            else
                result = RemoteStopEVSEResult.UnknownEVSE(SessionId);


            #region Send OnRemoteEVSEStopped event

            var OnRemoteEVSEStoppedLocal = OnRemoteEVSEStopped;
            if (OnRemoteEVSEStoppedLocal != null)
                OnRemoteEVSEStoppedLocal(this,
                                         Timestamp,
                                         EventTrackingId,
                                         _ChargingPool.Operator.RoamingNetwork.Id,
                                         EVSEId,
                                         SessionId,
                                         ReservationHandling,
                                         ProviderId,
                                         QueryTimeout,
                                         result);

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

            if (EventTrackingId == null)
                EventTrackingId = EventTracking_Id.New;

            #endregion

            #region Send OnRemoteChargingStationStop event

            var OnRemoteChargingStationStopLocal = OnRemoteChargingStationStop;
            if (OnRemoteChargingStationStopLocal != null)
                OnRemoteChargingStationStopLocal(this,
                                                 Timestamp,
                                                 EventTrackingId,
                                                 _ChargingPool.Operator.RoamingNetwork.Id,
                                                 ChargingStationId,
                                                 SessionId,
                                                 ReservationHandling,
                                                 ProviderId,
                                                 QueryTimeout.Value);

            #endregion


            var result = RemoteStopChargingStationResult.Error(SessionId);

            var ChargingSession = ChargingSessions.
                                      Where(session => session.Id == SessionId).
                                      FirstOrDefault();

            if (ChargingSession == null)
                result = RemoteStopChargingStationResult.InvalidSessionId(SessionId);

            if (result == null && ChargingSession.EVSE == null)
                result = RemoteStopChargingStationResult.UnknownOperator(SessionId);

            //if (result == null)
            //    result = await _ChargingSession.
            //                       EVSE.
            //                       RemoteStop(Timestamp,
            //                                  CancellationToken,
            //                                  ChargingStationId,
            //                                  SessionId,
            //                                  ReservationHandling,
            //                                  ProviderId);


            #region Send OnRemoteChargingStationStopped event

            var OnRemoteChargingStationStoppedLocal = OnRemoteChargingStationStopped;
            if (OnRemoteChargingStationStoppedLocal != null)
                OnRemoteChargingStationStoppedLocal(this,
                                                    Timestamp,
                                                    EventTrackingId,
                                                    _ChargingPool.Operator.RoamingNetwork.Id,
                                                    ChargingStationId,
                                                    SessionId,
                                                    ReservationHandling,
                                                    ProviderId,
                                                    QueryTimeout,
                                                    result);

            #endregion

            return result;

        }

        #endregion

        #region (internal) SendNewChargeDetailRecord(Timestamp, Sender, ChargeDetailRecord)

        internal void SendNewChargeDetailRecord(DateTime            Timestamp,
                                                Object              Sender,
                                                ChargeDetailRecord  ChargeDetailRecord)
        {

            var OnNewChargeDetailRecordLocal = OnNewChargeDetailRecord;
            if (OnNewChargeDetailRecordLocal != null)
                OnNewChargeDetailRecordLocal(Timestamp, Sender, ChargeDetailRecord);

        }

        #endregion

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
