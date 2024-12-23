/*
 * Copyright (c) 2014-2024 GraphDefined GmbH <achim.friedland@graphdefined.com>
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

using System;
using System.Linq;
using System.Threading;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Collections.Generic;

using org.GraphDefined.Vanaheimr.Illias;
using org.GraphDefined.Vanaheimr.Illias.Votes;
using org.GraphDefined.Vanaheimr.Styx.Arrows;
using org.GraphDefined.Vanaheimr.Aegir;
using org.GraphDefined.Vanaheimr.Hermod;

#endregion

namespace cloud.charging.open.protocols.WWCP
{

    public interface IeMobilityStation : IEquatable<eMobilityStation>, IComparable<eMobilityStation>, IComparable,
                                         IEntity<eMobilityStation_Id>
    {

        /// <summary>
        /// The unique identification of this charging pool.
        /// </summary>
        eMobilityStation_Id     Id                    { get; }

        ///// <summary>
        ///// The roaming network of this charging pool.
        ///// </summary>
        //IRoamingNetwork         RoamingNetwork        { get; }



        I18NString Name         { get; }
        I18NString Description  { get; }

    }


    /// <summary>
    /// A e-mobility station for hosting electric vehicles.
    /// </summary>
    public class eMobilityStation : AEMobilityEntity<eMobilityStation_Id,
                                                     eMobilityStationAdminStatusTypes,
                                                     eMobilityStationStatusTypes>,
                                    IeMobilityStation

    {

        #region Data

        /// <summary>
        /// The default max size of the e-mobility station (aggregated EVSE) status list.
        /// </summary>
        public const UInt16 DefaultMaxStatusScheduleSize         = 15;

        /// <summary>
        /// The default max size of the e-mobility station admin status list.
        /// </summary>
        public const UInt16 DefaultMaxAdminStatusScheduleSize    = 15;

        #endregion

        #region Properties

        #region ServiceIdentification

        private String _ServiceIdentification;

        /// <summary>
        /// The internal service identification of the e-mobility station maintained by the e-mobility station Operator.
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

        #region Name

        private I18NString _Name;

        /// <summary>
        /// The offical (multi-language) name of this e-mobility station.
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
                    SetProperty(ref _Name, value);

            }

        }

        #endregion

        #region Description

        internal I18NString _Description;

        /// <summary>
        /// An optional (multi-language) description of this e-mobility station.
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

                if (Description != value)
                    SetProperty(ref _Description, value);

            }

        }

        #endregion

        #region Brand

        private Brand _Brand;

        /// <summary>
        /// A brand for this e-mobility station
        /// is this is different from the e-mobility station Operator.
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
                    SetProperty(ref _Brand, value);

            }

        }

        #endregion

        #region Address

        internal Address _Address;

        /// <summary>
        /// The address of this e-mobility station.
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

        #region OSM_NodeId

        private String _OSM_NodeId;

        /// <summary>
        /// OSM Node Id.
        /// </summary>
        [Optional]
        public String OSM_NodeId
        {

            get
            {
                return _OSM_NodeId;
            }

            set
            {
                SetProperty(ref _OSM_NodeId, value);
            }

        }

        #endregion

        #region GeoLocation

        internal GeoCoordinate _GeoLocation;

        /// <summary>
        /// The geographical location of this e-mobility station.
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

        #region ParkingSpaces

        private List<ParkingSpace> _ParkingSpaces;

        /// <summary>
        /// parking spaces reachable from this e-mobility station.
        /// </summary>
        [Optional]
        public IEnumerable<ParkingSpace> ParkingSpaces

            => _ParkingSpaces;

        #endregion

        #region OpeningTimes

        internal OpeningTimes _OpeningTimes;

        /// <summary>
        /// The opening times of this e-mobility station.
        /// </summary>
        public OpeningTimes OpeningTimes
        {

            get
            {
                return _OpeningTimes;
            }

            set
            {

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

        internal ReactiveSet<AuthenticationModes> _AuthenticationModes;

        public ReactiveSet<AuthenticationModes> AuthenticationModes
        {

            get
            {
                return _AuthenticationModes;
            }

            set
            {

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
                return _PaymentOptions;
            }

            set
            {

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

        internal AccessibilityType _Accessibility;

        [Optional]
        public AccessibilityType Accessibility
        {

            get
            {

                return _Accessibility;

            }

            set
            {

                if (Accessibility != value)
                {

                    SetProperty(ref _Accessibility, value);

                }

            }

        }

        #endregion

        #region HotlinePhoneNumber

        internal PhoneNumber? _HotlinePhoneNumber;

        /// <summary>
        /// The telephone number of the e-mobility station Operator hotline.
        /// </summary>
        [Optional]
        public PhoneNumber? HotlinePhoneNumber
        {

            get
            {
                return _HotlinePhoneNumber ?? Provider.HotlinePhoneNumber;
            }

            set
            {

                if (value == Provider.HotlinePhoneNumber)
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

        #region PhotoURIs

        private ReactiveSet<String> _PhotoURIs;

        /// <summary>
        /// URIs of photos of this e-mobility station.
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

        #endregion

        #region Links

        /// <summary>
        /// An optional remote e-mobility station.
        /// </summary>
        public IRemoteEMobilityStation  RemoteeMobilityStation    { get; }


        /// <summary>
        /// The e-mobility station Operator of this EVSE.
        /// </summary>
        [InternalUseOnly]
        public EMobilityProvider Provider { get; }

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new e-mobility station having the given identification.
        /// </summary>
        /// <param name="Id">The unique identification of the e-mobility station pool.</param>
        /// <param name="MaxAdminStatusScheduleSize">The default size of the admin status list.</param>
        internal eMobilityStation(eMobilityStation_Id                    Id,
                                  EMobilityProvider                      Provider,
                                  Action<eMobilityStation>               Configurator                   = null,
                                  RemoteEMobilityStationCreatorDelegate  RemoteeMobilityStationCreator  = null,
                                  eMobilityStationAdminStatusTypes        AdminStatus                    = eMobilityStationAdminStatusTypes.Operational,
                                  UInt16                                 MaxAdminStatusScheduleSize         = DefaultMaxAdminStatusScheduleSize)

            : base(Id)

        {

            #region Initial checks

            if (Provider == null)
                throw new ArgumentNullException(nameof(Provider),  "The e-mobility provider must not be null!");

            #endregion

            #region Init data and properties

            this.Provider                    = Provider;

            this.Name                        = new I18NString();
            this.Description                 = new I18NString();

            this._UserComment                = new I18NString();
            this._ServiceProviderComment     = new I18NString();
            //this.GeoLocation                 = new GeoCoordinate();

            this._ParkingSpaces               = new List<ParkingSpace>();

            this._PaymentOptions             = new ReactiveSet<PaymentOptions>();

            #endregion

            #region Init events


            #endregion

            #region Link events

            this.adminStatusSchedule.OnStatusChanged += (timestamp, eventTrackingId, statusSchedule, newStatus, oldStatus, dataSource)
                                                          => UpdateAdminStatus(timestamp, eventTrackingId, newStatus, oldStatus, dataSource);

            //// eMobilityStation events
            //this.OnEVSEAddition.           OnVoting       += (timestamp, station, evse, vote)      => ChargingPool.EVSEAddition.           SendVoting      (timestamp, station, evse, vote);
            //this.OnEVSEAddition.           OnNotification += (timestamp, station, evse)            => ChargingPool.EVSEAddition.           SendNotification(timestamp, station, evse);

            //this.OnEVSERemoval.            OnVoting       += (timestamp, station, evse, vote)      => ChargingPool.EVSERemoval .           SendVoting      (timestamp, station, evse, vote);
            //this.OnEVSERemoval.            OnNotification += (timestamp, station, evse)            => ChargingPool.EVSERemoval .           SendNotification(timestamp, station, evse);

            //// EVSE events
            //this.SocketOutletAddition.     OnVoting       += (timestamp, evse, outlet, vote)       => ChargingPool.SocketOutletAddition.   SendVoting      (timestamp, evse, outlet, vote);
            //this.SocketOutletAddition.     OnNotification += (timestamp, evse, outlet)             => ChargingPool.SocketOutletAddition.   SendNotification(timestamp, evse, outlet);

            //this.SocketOutletRemoval.      OnVoting       += (timestamp, evse, outlet, vote)       => ChargingPool.SocketOutletRemoval.    SendVoting      (timestamp, evse, outlet, vote);
            //this.SocketOutletRemoval.      OnNotification += (timestamp, evse, outlet)             => ChargingPool.SocketOutletRemoval.    SendNotification(timestamp, evse, outlet);

            #endregion

            this.OnPropertyChanged += UpdateData;

            Configurator?.Invoke(this);

            this.RemoteeMobilityStation = RemoteeMobilityStationCreator?.Invoke(this);

        }

        #endregion


        #region Data/AdminStatus management

        #region OnData/(Admin)StatusChanged

        /// <summary>
        /// An event fired whenever the static data changed.
        /// </summary>
        public event OnEMobilityStationDataChangedDelegate         OnDataChanged;

        /// <summary>
        /// An event fired whenever the admin status changed.
        /// </summary>
        public event OnEMobilityStationAdminStatusChangedDelegate  OnAdminStatusChanged;

        #endregion


        #region (internal) UpdateData       (Timestamp, EventTrackingId, Sender, PropertyName, OldValue, NewValue)

        /// <summary>
        /// Update the static data.
        /// </summary>
        /// <param name="Timestamp">The timestamp when this change was detected.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="Sender">The changed e-mobility station.</param>
        /// <param name="PropertyName">The name of the changed property.</param>
        /// <param name="OldValue">The old value of the changed property.</param>
        /// <param name="NewValue">The new value of the changed property.</param>
        internal async Task UpdateData(DateTime          Timestamp,
                                       EventTracking_Id  EventTrackingId,
                                       Object            Sender,
                                       String            PropertyName,
                                       Object?           NewValue,
                                       Object?           OldValue     = null,
                                       Context?          DataSource   = null)
        {

            var onDataChanged = OnDataChanged;
            if (onDataChanged is not null)
                await onDataChanged(Timestamp,
                                    EventTrackingId,
                                    Sender as eMobilityStation,
                                    PropertyName,
                                    NewValue,
                                    OldValue,
                                    DataSource);

        }

        #endregion

        #region (internal) UpdateAdminStatus(Timestamp, EventTrackingId, OldStatus, NewStatus)

        /// <summary>
        /// Update the current admin status.
        /// </summary>
        /// <param name="Timestamp">The timestamp when this change was detected.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="OldStatus">The old e-mobility station admin status.</param>
        /// <param name="NewStatus">The new e-mobility station admin status.</param>
        internal async Task UpdateAdminStatus(DateTime                                        Timestamp,
                                              EventTracking_Id                                EventTrackingId,
                                              Timestamped<eMobilityStationAdminStatusTypes>   NewStatus,
                                              Timestamped<eMobilityStationAdminStatusTypes>?  OldStatus    = null,
                                              Context?                                        DataSource   = null)
        {

            await OnAdminStatusChanged?.Invoke(Timestamp,
                                               EventTrackingId,
                                               this,
                                               NewStatus,
                                               OldStatus,
                                               DataSource);

        }

        #endregion

        #endregion

        #region eVehicles

        #region eVehicleAddition

        internal readonly IVotingNotificator<DateTime, eMobilityStation, EVehicle, Boolean> eVehicleAddition;

        /// <summary>
        /// Called whenever an electric vehicle will be or was added.
        /// </summary>
        public IVotingSender<DateTime, eMobilityStation, EVehicle, Boolean> OnEVehicleAddition

            => eVehicleAddition;

        #endregion

        #region eVehicleRemoval

        internal readonly IVotingNotificator<DateTime, eMobilityStation, EVehicle, Boolean> eVehicleRemoval;

        /// <summary>
        /// Called whenever an electric vehicle will be or was removed.
        /// </summary>
        public IVotingSender<DateTime, eMobilityStation, EVehicle, Boolean> OnEVehicleRemoval

            => eVehicleRemoval;

        #endregion


        #region eVehicles

        private EntityHashSet<ChargingStationOperator, EVehicle_Id, EVehicle> _eVehicles;

        public IEnumerable<EVehicle> eVehicles

            => _eVehicles;

        #endregion

        #region eVehicleAdminStatus

        public IEnumerable<KeyValuePair<EVehicle_Id, eVehicleAdminStatusTypes>> eVehicleAdminStatus

            => _eVehicles.
                   OrderBy(vehicle => vehicle.Id).
                   Select (vehicle => new KeyValuePair<EVehicle_Id, eVehicleAdminStatusTypes>(vehicle.Id, vehicle.AdminStatus.Value));

        #endregion

        #region eVehicleStatus

        public IEnumerable<KeyValuePair<EVehicle_Id, eVehicleStatusTypes>> eVehicleStatus

            => _eVehicles.
                   OrderBy(vehicle => vehicle.Id).
                   Select (vehicle => new KeyValuePair<EVehicle_Id, eVehicleStatusTypes>(vehicle.Id, vehicle.Status.Value));

        #endregion


        #region CreateNeweVehicle(eVehicleId = null, Configurator = null, OnSuccess = null, OnError = null)

        /// <summary>
        /// Create and register a new eVehicle having the given
        /// unique eVehicle identification.
        /// </summary>
        /// <param name="eVehicleId">The unique identification of the new eVehicle.</param>
        /// <param name="Configurator">An optional delegate to configure the new eVehicle before its successful creation.</param>
        /// <param name="OnSuccess">An optional delegate to configure the new eVehicle after its successful creation.</param>
        /// <param name="OnError">An optional delegate to be called whenever the creation of the eVehicle failed.</param>
        public EVehicle CreateNewEVehicle(EVehicle_Id                            eVehicleId             = null,
                                          Action<EVehicle>                       Configurator           = null,
                                          RemoteEVehicleCreatorDelegate          RemoteeVehicleCreator  = null,
                                          eVehicleAdminStatusTypes                AdminStatus            = eVehicleAdminStatusTypes.Operational,
                                          eVehicleStatusTypes                     Status                 = eVehicleStatusTypes.Available,
                                          Action<EVehicle>                       OnSuccess              = null,
                                          Action<eMobilityStation, EVehicle_Id>  OnError                = null)

        {

            #region Initial checks

            if (eVehicleId == null)
                eVehicleId = EVehicle_Id.Random(Provider.Id);

            // Do not throw an exception when an OnError delegate was given!
            if (_eVehicles.Any(pool => pool.Id == eVehicleId))
            {
                if (OnError == null)
                    throw new eVehicleAlreadyExistsInStation(this, eVehicleId);
                else
                    OnError?.Invoke(this, eVehicleId);
            }

            #endregion

            var eVehicle = new EVehicle(eVehicleId,
                                        Provider,
                                        Configurator,
                                        RemoteeVehicleCreator,
                                        AdminStatus,
                                        Status);


            if (_eVehicles.TryAdd(eVehicle,
                                  EventTracking_Id.New,
                                  null).Result == CommandResult.Success)
            {

                eVehicle.OnDataChanged                        += UpdateEVehicleData;
                eVehicle.OnStatusChanged                      += UpdateEVehicleStatus;
                eVehicle.OnAdminStatusChanged                 += UpdateEVehicleAdminStatus;

                //_eVehicle.OnNewReservation                     += SendNewReservation;
                //_eVehicle.OnCancelReservationResponse               += SendOnCancelReservationResponse;
                //_eVehicle.OnNewChargingSession                 += SendNewChargingSession;
                //_eVehicle.OnNewChargeDetailRecord              += SendNewChargeDetailRecord;

                OnSuccess?.Invoke(eVehicle);

                return eVehicle;

            }

            return null;

        }

        #endregion


        #region ContainseVehicle(eVehicle)

        /// <summary>
        /// Check if the given eVehicle is already present within the Charging Station Operator.
        /// </summary>
        /// <param name="eVehicle">A eVehicle.</param>
        public Boolean ContainseVehicle(EVehicle eVehicle)

            => _eVehicles.Contains(eVehicle);

        #endregion

        #region ContainseVehicle(eVehicleId)

        /// <summary>
        /// Check if the given eVehicle identification is already present within the Charging Station Operator.
        /// </summary>
        /// <param name="eVehicleId">The unique identification of the eVehicle.</param>
        public Boolean ContainseVehicle(EVehicle_Id eVehicleId)

            => _eVehicles.ContainsId(eVehicleId);

        #endregion

        #region GetEVehicleById(eVehicleId)

        public EVehicle GetEVehicleById(EVehicle_Id eVehicleId)

            => _eVehicles.GetById(eVehicleId);

        #endregion

        #region TryGetEVehicleById(eVehicleId, out eVehicle)

        public Boolean TryGetEVehicleById(EVehicle_Id eVehicleId, out EVehicle eVehicle)

            => _eVehicles.TryGet(eVehicleId, out eVehicle);

        #endregion

        #region RemoveEVehicle(eVehicleId)

        public EVehicle RemoveEVehicle(EVehicle_Id eVehicleId)
        {

            EVehicle _eVehicle = null;

            if (TryGetEVehicleById(eVehicleId, out _eVehicle))
            {

                if (eVehicleRemoval.SendVoting(EventTracking_Id.New, Timestamp.Now, this, _eVehicle))
                {

                    if (_eVehicles.TryRemove(eVehicleId,
                                             out _eVehicle,
                                             EventTracking_Id.New,
                                             null))
                    {

                        eVehicleRemoval.SendNotification(EventTracking_Id.New, Timestamp.Now, this, _eVehicle);

                        return _eVehicle;

                    }

                }

            }

            return null;

        }

        #endregion

        #region TryRemoveEVehicle(eVehicleId, out eVehicle)

        public Boolean TryRemoveEVehicle(EVehicle_Id eVehicleId, out EVehicle? eVehicle)
        {

            if (TryGetEVehicleById(eVehicleId, out eVehicle))
            {

                if (eVehicleRemoval.SendVoting(EventTracking_Id.New, Timestamp.Now,
                                               this,
                                               eVehicle))
                {

                    if (_eVehicles.TryRemove(eVehicleId,
                                             out _,
                                             EventTracking_Id.New,
                                             null))
                    {

                        eVehicleRemoval.SendNotification(EventTracking_Id.New, Timestamp.Now,
                                                         this,
                                                         eVehicle);

                        return true;

                    }

                }

                return false;

            }

            return true;

        }

        #endregion

        #region SeteVehicleAdminStatus(eVehicleId, NewStatus)

        public void SeteVehicleAdminStatus(EVehicle_Id                            eVehicleId,
                                           Timestamped<eVehicleAdminStatusTypes>  NewStatus,
                                           Boolean                                SendUpstream = false)
        {

            if (TryGetEVehicleById(eVehicleId, out var eVehicle) &&
                eVehicle is not null)
            {
                eVehicle.AdminStatus = NewStatus;
            }

        }

        #endregion

        #region SetEVehicleAdminStatus(eVehicleId, NewStatus, Timestamp)

        public void SetEVehicleAdminStatus(EVehicle_Id               eVehicleId,
                                           eVehicleAdminStatusTypes  NewStatus,
                                           DateTime                  Timestamp)
        {

            if (TryGetEVehicleById(eVehicleId, out var eVehicle) &&
                eVehicle is not null)
            {
                eVehicle.AdminStatus = new Timestamped<eVehicleAdminStatusTypes>(Timestamp, NewStatus);
            }

        }

        #endregion

        #region SetEVehicleAdminStatus(eVehicleId, StatusList, ChangeMethod = ChangeMethods.Replace)

        public void SetEVehicleAdminStatus(EVehicle_Id                                        eVehicleId,
                                           IEnumerable<Timestamped<eVehicleAdminStatusTypes>>  StatusList,
                                           ChangeMethods                                      ChangeMethod  = ChangeMethods.Replace)
        {

            EVehicle _eVehicle  = null;
            if (TryGetEVehicleById(eVehicleId, out _eVehicle))
                _eVehicle.SetAdminStatus(StatusList, ChangeMethod);

            //if (SendUpstream)
            //{
            //
            //    RoamingNetwork.
            //        SendeVehicleAdminStatusDiff(new eVehicleAdminStatusDiff(Timestamp.Now,
            //                                               ChargingStationOperatorId:    Id,
            //                                               ChargingStationOperatorName:  Name,
            //                                               NewStatus:         new List<KeyValuePair<eVehicle_Id, eVehicleAdminStatusType>>(),
            //                                               ChangedStatus:     new List<KeyValuePair<eVehicle_Id, eVehicleAdminStatusType>>() {
            //                                                                          new KeyValuePair<eVehicle_Id, eVehicleAdminStatusType>(eVehicleId, NewStatus.Value)
            //                                                                      },
            //                                               RemovedIds:        new List<eVehicle_Id>()));
            //
            //}

        }

        #endregion


        #region OnEVehicleData/(Admin)StatusChanged

        /// <summary>
        /// An event fired whenever the static data of any subordinated eVehicle changed.
        /// </summary>
        public event OnEVehicleDataChangedDelegate         OnEVehicleDataChanged;

        /// <summary>
        /// An event fired whenever the aggregated dynamic status of any subordinated eVehicle changed.
        /// </summary>
        public event OnEVehicleStatusChangedDelegate       OnEVehicleStatusChanged;

        /// <summary>
        /// An event fired whenever the aggregated dynamic status of any subordinated eVehicle changed.
        /// </summary>
        public event OnEVehicleAdminStatusChangedDelegate  OnEVehicleAdminStatusChanged;

        #endregion

        #region OnEVehicleGeoLocationChanged

        /// <summary>
        /// An event fired whenever the geo coordinate changed.
        /// </summary>
        public event OnEVehicleGeoLocationChangedDelegate OnEVehicleGeoLocationChanged;

        #endregion


        #region (internal) UpdateEVehicleData(Timestamp, EventTrackingId, eVehicle, OldStatus, NewStatus)

        /// <summary>
        /// Update the data of an eVehicle.
        /// </summary>
        /// <param name="Timestamp">The timestamp when this change was detected.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="eVehicle">The changed eVehicle.</param>
        /// <param name="PropertyName">The name of the changed property.</param>
        /// <param name="OldValue">The old value of the changed property.</param>
        /// <param name="NewValue">The new value of the changed property.</param>
        internal async Task UpdateEVehicleData(DateTime          Timestamp,
                                               EventTracking_Id  EventTrackingId,
                                               EVehicle          eVehicle,
                                               String            PropertyName,
                                               Object?           NewValue,
                                               Object?           OldValue     = null,
                                               Context?          DataSource   = null)
        {

            var onEVehicleDataChanged = OnEVehicleDataChanged;
            if (onEVehicleDataChanged is not null)
                await onEVehicleDataChanged(Timestamp,
                                                 EventTrackingId,
                                                 eVehicle,
                                                 PropertyName,
                                                 NewValue,
                                                 OldValue,
                                                 DataSource);

        }

        #endregion

        #region (internal) UpdateEVehicleAdminStatus(Timestamp, EventTrackingId, eVehicle, OldStatus, NewStatus)

        /// <summary>
        /// Update the current eVehicle admin status.
        /// </summary>
        /// <param name="Timestamp">The timestamp when this change was detected.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="eVehicle">The updated eVehicle.</param>
        /// <param name="OldStatus">The old aggreagted charging station status.</param>
        /// <param name="NewStatus">The new aggreagted charging station status.</param>
        internal async Task UpdateEVehicleAdminStatus(DateTime                                Timestamp,
                                                      EventTracking_Id                        EventTrackingId,
                                                      EVehicle                                eVehicle,
                                                      Timestamped<eVehicleAdminStatusTypes>   NewStatus,
                                                      Timestamped<eVehicleAdminStatusTypes>?  OldStatus    = null,
                                                      Context?                                DataSource   = null)
        {

            var OnEVehicleAdminStatusChangedLocal = OnEVehicleAdminStatusChanged;
            if (OnEVehicleAdminStatusChangedLocal is not null)
                await OnEVehicleAdminStatusChangedLocal(Timestamp,
                                                        EventTrackingId,
                                                        eVehicle,
                                                        NewStatus,
                                                        OldStatus,
                                                        DataSource);

        }

        #endregion

        #region (internal) UpdateEVehicleStatus(Timestamp, EventTrackingId, eVehicle, OldStatus, NewStatus)

        /// <summary>
        /// Update the current eVehicle status.
        /// </summary>
        /// <param name="Timestamp">The timestamp when this change was detected.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="eVehicle">The updated eVehicle.</param>
        /// <param name="OldStatus">The old aggreagted charging station status.</param>
        /// <param name="NewStatus">The new aggreagted charging station status.</param>
        internal async Task UpdateEVehicleStatus(DateTime                           Timestamp,
                                                 EventTracking_Id                   EventTrackingId,
                                                 EVehicle                           eVehicle,
                                                 Timestamped<eVehicleStatusTypes>   NewStatus,
                                                 Timestamped<eVehicleStatusTypes>?  OldStatus    = null,
                                                 Context?                           DataSource   = null)
        {

            var OnEVehicleStatusChangedLocal = OnEVehicleStatusChanged;
            if (OnEVehicleStatusChangedLocal is not null)
                await OnEVehicleStatusChangedLocal(Timestamp,
                                                   EventTrackingId,
                                                   eVehicle,
                                                   NewStatus,
                                                   OldStatus,
                                                   DataSource);

        }

        #endregion

        #region (internal) UpdateEVehicleGeoLocation(Timestamp, EventTrackingId, eVehicle, OldGeoCoordinate, NewGeoCoordinate)

        /// <summary>
        /// Update the current electric vehicle geo location.
        /// </summary>
        /// <param name="Timestamp">The timestamp when this change was detected.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="eVehicle">The updated eVehicle.</param>
        /// <param name="OldGeoCoordinate">The old aggreagted charging station status.</param>
        /// <param name="NewGeoCoordinate">The new aggreagted charging station status.</param>
        internal async Task UpdateEVehicleGeoLocation(DateTime                     Timestamp,
                                                      EventTracking_Id             EventTrackingId,
                                                      EVehicle                     eVehicle,
                                                      Timestamped<GeoCoordinate>   NewGeoCoordinate,
                                                      Timestamped<GeoCoordinate>?  OldGeoCoordinate   = null,
                                                      Context?                     DataSource         = null)
        {

            var OnEVehicleGeoLocationChangedLocal = OnEVehicleGeoLocationChanged;
            if (OnEVehicleGeoLocationChangedLocal is not null)
                await OnEVehicleGeoLocationChangedLocal(Timestamp,
                                                        EventTrackingId,
                                                        eVehicle,
                                                        NewGeoCoordinate,
                                                        OldGeoCoordinate,
                                                        DataSource);

        }

        #endregion

        #endregion


        public void AddParkingSpaces(params ParkingSpace[] ParkingSpaces)
        {

            if (ParkingSpaces is not null)
                _ParkingSpaces.AddRange(ParkingSpaces);

        }


        #region IComparable<eMobilityStation> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        public override Int32 CompareTo(Object Object)
        {

            if (Object == null)
                throw new ArgumentNullException("The given object must not be null!");

            // Check if the given object is a e-mobility station.
            var eMobilityStation = Object as eMobilityStation;
            if ((Object) eMobilityStation == null)
                throw new ArgumentException("The given object is not a e-mobility station!");

            return CompareTo(eMobilityStation);

        }

        #endregion

        #region CompareTo(eMobilityStation)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="eMobilityStation">A e-mobility station object to compare with.</param>
        public Int32 CompareTo(eMobilityStation eMobilityStation)
        {

            if ((Object) eMobilityStation == null)
                throw new ArgumentNullException("The given e-mobility station must not be null!");

            return Id.CompareTo(eMobilityStation.Id);

        }

        #endregion

        #endregion

        #region IEquatable<eMobilityStation> Members

        #region Equals(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public override Boolean Equals(Object Object)
        {

            if (Object == null)
                return false;

            // Check if the given object is a e-mobility station.
            var eMobilityStation = Object as eMobilityStation;
            if ((Object) eMobilityStation == null)
                return false;

            return this.Equals(eMobilityStation);

        }

        #endregion

        #region Equals(eMobilityStation)

        /// <summary>
        /// Compares two e-mobility stations for equality.
        /// </summary>
        /// <param name="eMobilityStation">A e-mobility station to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public Boolean Equals(eMobilityStation eMobilityStation)
        {

            if ((Object) eMobilityStation == null)
                return false;

            return Id.Equals(eMobilityStation.Id);

        }

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
            => Id.ToString();

        #endregion

    }

}
