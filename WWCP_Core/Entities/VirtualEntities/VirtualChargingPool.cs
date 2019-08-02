/*
 * Copyright (c) 2014-2018 GraphDefined GmbH <achim.friedland@graphdefined.com>
 * This file is part of WWCP Cloud <https://github.com/GraphDefined/WWCP_Cloud>
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

using org.GraphDefined.Vanaheimr.Illias;

using org.GraphDefined.WWCP.ChargingStations;
using org.GraphDefined.Vanaheimr.Hermod;
using Org.BouncyCastle.Bcpg.OpenPgp;

#endregion

namespace org.GraphDefined.WWCP.ChargingPools
{

    /// <summary>
    /// A demo implementation of a virtual WWCP charging pool.
    /// </summary>
    public class VirtualChargingPool : AEMobilityEntity<ChargingPool_Id>,
                                       IEquatable<VirtualChargingPool>, IComparable<VirtualChargingPool>, IComparable,
                                       IStatus<ChargingPoolStatusTypes>,
                                       IRemoteChargingPool
    {

        #region Data

        /// <summary>
        /// The default max size of the status history.
        /// </summary>
        public const UInt16 DefaultMaxStatusListSize = 50;

        /// <summary>
        /// The default max size of the admin status history.
        /// </summary>
        public const UInt16 DefaultMaxAdminStatusListSize = 50;

        /// <summary>
        /// The maximum time span for a reservation.
        /// </summary>
        public static readonly TimeSpan MaxReservationDuration = TimeSpan.FromMinutes(15);

        #endregion

        #region Properties

        #region Id

        private ChargingPool_Id _Id;

        /// <summary>
        /// The unique identification of this virtual charging pool.
        /// </summary>
        public ChargingPool_Id Id
        {
            get
            {
                return _Id;
            }
        }

        #endregion

        #region Description

        internal I18NString _Description;

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

                if (value == _Description)
                    return;

                _Description = value;

            }

        }

        #endregion


        #region AdminStatus

        /// <summary>
        /// The current charging station admin status.
        /// </summary>
        [InternalUseOnly]
        public Timestamped<ChargingPoolAdminStatusTypes> AdminStatus
        {

            get
            {
                return _AdminStatusSchedule.CurrentStatus;
            }

            set
            {

                if (value == null)
                    return;

                if (_AdminStatusSchedule.CurrentValue != value.Value)
                    SetAdminStatus(value);

            }

        }

        #endregion

        #region AdminStatusSchedule

        private StatusSchedule<ChargingPoolAdminStatusTypes> _AdminStatusSchedule;

        /// <summary>
        /// The charging station admin status schedule.
        /// </summary>
        public IEnumerable<Timestamped<ChargingPoolAdminStatusTypes>> AdminStatusSchedule
        {
            get
            {
                return _AdminStatusSchedule;
            }
        }

        #endregion

        #region Status

        /// <summary>
        /// The current charging station status.
        /// </summary>
        [InternalUseOnly]
        public Timestamped<ChargingPoolStatusTypes> Status
        {

            get
            {
                return _StatusSchedule.CurrentStatus;
            }

            set
            {

                if (value == null)
                    return;

                if (_StatusSchedule.CurrentValue != value.Value)
                    SetStatus(value);

            }

        }

        #endregion

        #region StatusSchedule

        private StatusSchedule<ChargingPoolStatusTypes> _StatusSchedule;

        /// <summary>
        /// The charging station status schedule.
        /// </summary>
        public IEnumerable<Timestamped<ChargingPoolStatusTypes>> StatusSchedule
        {
            get
            {
                return _StatusSchedule;
            }
        }

        #endregion

        #endregion

        #region Links

        #region ChargingPool

        private readonly ChargingPool _ChargingPool;

        public ChargingPool ChargingPool
        {
            get
            {
                return _ChargingPool;
            }
        }

        #endregion

        #endregion

        #region Events



        #endregion

        #region Constructor(s)

        /// <summary>
        /// A virtual WWCP charging pool.
        /// </summary>
        public VirtualChargingPool(ChargingPool_Id               Id,
                                   ChargingPoolAdminStatusTypes  InitialAdminStatus       = ChargingPoolAdminStatusTypes.Operational,
                                   ChargingPoolStatusTypes       InitialStatus            = ChargingPoolStatusTypes.Available,
                                   UInt16                        MaxAdminStatusListSize   = DefaultMaxAdminStatusListSize,
                                   UInt16                        MaxStatusListSize        = DefaultMaxStatusListSize)

            : base(Id)

        {

            #region Init data and properties

            this._Stations             = new HashSet<IRemoteChargingStation>();

            this._AdminStatusSchedule  = new StatusSchedule<ChargingPoolAdminStatusTypes>(MaxAdminStatusListSize);
            this._AdminStatusSchedule.Insert(InitialAdminStatus);

            this._StatusSchedule       = new StatusSchedule<ChargingPoolStatusTypes>(MaxStatusListSize);
            this._StatusSchedule.Insert(InitialStatus);

            #endregion

            #region Link events

            this._AdminStatusSchedule.OnStatusChanged += (Timestamp, EventTrackingId, StatusSchedule, OldStatus, NewStatus)
                                                          => UpdateAdminStatus(Timestamp, EventTrackingId, OldStatus, NewStatus);

            this._StatusSchedule.     OnStatusChanged += (Timestamp, EventTrackingId, StatusSchedule, OldStatus, NewStatus)
                                                          => UpdateStatus(Timestamp, EventTrackingId, OldStatus, NewStatus);

            #endregion

        }

        #endregion


        #region Charging stations...

        #region Stations

        private readonly HashSet<IRemoteChargingStation> _Stations;

        /// <summary>
        /// All registered charging stations.
        /// </summary>
        public IEnumerable<IRemoteChargingStation> Stations
            => _Stations;

        #endregion

        #region CreateVirtualStation(ChargingStation, Configurator = null, OnSuccess = null, OnError = null)

        /// <summary>
        /// Create and register a new charging station having the given
        /// unique charging station identification.
        /// </summary>
        /// <param name="ChargingStation">A charging station.</param>
        /// <param name="Configurator">An optional delegate to configure the new charging station after its creation.</param>
        /// <param name="OnSuccess">An optional delegate called after successful creation of the charging station.</param>
        /// <param name="OnError">An optional delegate for signaling errors.</param>
        public VirtualChargingStation CreateVirtualStation(ChargingStation_Id                               ChargingStationId,
                                                           ChargingStationAdminStatusTypes                  InitialAdminStatus       = ChargingStationAdminStatusTypes.Operational,
                                                           ChargingStationStatusTypes                       InitialStatus            = ChargingStationStatusTypes.Available,
                                                           UInt16                                           MaxAdminStatusListSize   = DefaultMaxAdminStatusListSize,
                                                           UInt16                                           MaxStatusListSize        = DefaultMaxStatusListSize,
                                                           TimeSpan?                                        SelfCheckTimeSpan        = null,
                                                           Action<VirtualChargingStation>                   Configurator             = null,
                                                           Action<VirtualChargingStation>                   OnSuccess                = null,
                                                           Action<VirtualChargingStation, ChargingStation_Id>  OnError                  = null)
        {

            #region Initial checks

            if (_Stations.Any(station => station.Id == ChargingStationId))
            {
                throw new Exception("StationAlreadyExistsInPool");
                //if (OnError == null)
                //    throw new ChargingStationAlreadyExistsInStation(this.ChargingStation, ChargingStation.Id);
                //else
                //    OnError?.Invoke(this, ChargingStation.Id);
            }

            #endregion

            var Now           = DateTime.UtcNow;
            var _VirtualStation  = new VirtualChargingStation(ChargingStationId,
                                                              this,
                                                              null,
                                                              null,
                                                              InitialAdminStatus,
                                                              InitialStatus,
                                                              MaxAdminStatusListSize,
                                                              MaxStatusListSize,
                                                              SelfCheckTimeSpan);

            Configurator?.Invoke(_VirtualStation);

            if (_Stations.Add(_VirtualStation))
            {

                //_VirtualEVSE.OnPropertyChanged        += (Timestamp, Sender, PropertyName, OldValue, NewValue)
                //                                           => UpdateEVSEData(Timestamp, Sender as VirtualEVSE, PropertyName, OldValue, NewValue);
                //
                //_VirtualEVSE.OnStatusChanged          += UpdateEVSEStatus;
                //_VirtualEVSE.OnAdminStatusChanged     += UpdateEVSEAdminStatus;
                //_VirtualEVSE.OnNewReservation         += SendNewReservation;
                //_VirtualEVSE.OnNewChargingSession     += SendNewChargingSession;
                //_VirtualEVSE.OnNewChargeDetailRecord  += SendNewChargeDetailRecord;

                OnSuccess?.Invoke(_VirtualStation);

                return _VirtualStation;

            }

            return null;

        }

        #endregion

        #region CreateVirtualStation(ChargingStation, Configurator = null, OnSuccess = null, OnError = null)

        /// <summary>
        /// Create and register a new charging station having the given
        /// unique charging station identification.
        /// </summary>
        /// <param name="ChargingStation">A charging station.</param>
        /// <param name="Configurator">An optional delegate to configure the new charging station after its creation.</param>
        /// <param name="OnSuccess">An optional delegate called after successful creation of the charging station.</param>
        /// <param name="OnError">An optional delegate for signaling errors.</param>
        public VirtualChargingStation CreateVirtualStation(ChargingStation_Id                               ChargingStationId,
                                                           PgpSecretKeyRing                                 SecretKeyRing            = null,
                                                           PgpPublicKeyRing                                 PublicKeyRing            = null,
                                                           ChargingStationAdminStatusTypes                  InitialAdminStatus       = ChargingStationAdminStatusTypes.Operational,
                                                           ChargingStationStatusTypes                       InitialStatus            = ChargingStationStatusTypes.Available,
                                                           UInt16                                           MaxAdminStatusListSize   = DefaultMaxAdminStatusListSize,
                                                           UInt16                                           MaxStatusListSize        = DefaultMaxStatusListSize,
                                                           TimeSpan?                                        SelfCheckTimeSpan        = null,
                                                           Action<VirtualChargingStation>                   Configurator             = null,
                                                           Action<VirtualChargingStation>                   OnSuccess                = null,
                                                           Action<VirtualChargingStation, ChargingStation_Id>  OnError                  = null)
        {

            #region Initial checks

            if (_Stations.Any(station => station.Id == ChargingStationId))
            {
                throw new Exception("StationAlreadyExistsInPool");
                //if (OnError == null)
                //    throw new ChargingStationAlreadyExistsInStation(this.ChargingStation, ChargingStation.Id);
                //else
                //    OnError?.Invoke(this, ChargingStation.Id);
            }

            #endregion

            var Now              = DateTime.UtcNow;
            var _VirtualStation  = new VirtualChargingStation(ChargingStationId,
                                                              this,
                                                              SecretKeyRing,
                                                              PublicKeyRing,
                                                              InitialAdminStatus,
                                                              InitialStatus,
                                                              MaxAdminStatusListSize,
                                                              MaxStatusListSize,
                                                              SelfCheckTimeSpan);

            Configurator?.Invoke(_VirtualStation);

            if (_Stations.Add(_VirtualStation))
            {

                //_VirtualEVSE.OnPropertyChanged        += (Timestamp, Sender, PropertyName, OldValue, NewValue)
                //                                           => UpdateEVSEData(Timestamp, Sender as VirtualEVSE, PropertyName, OldValue, NewValue);
                //
                //_VirtualEVSE.OnStatusChanged          += UpdateEVSEStatus;
                //_VirtualEVSE.OnAdminStatusChanged     += UpdateEVSEAdminStatus;
                //_VirtualEVSE.OnNewReservation         += SendNewReservation;
                //_VirtualEVSE.OnNewChargingSession     += SendNewChargingSession;
                //_VirtualEVSE.OnNewChargeDetailRecord  += SendNewChargeDetailRecord;

                OnSuccess?.Invoke(_VirtualStation);

                return _VirtualStation;

            }

            return null;

        }

        #endregion

        #endregion


        #region (Admin-)Status management

        #region OnData/(Admin)StatusChanged

        /// <summary>
        /// An event fired whenever the static data of the charging station changed.
        /// </summary>
        public event OnRemoteChargingPoolDataChangedDelegate         OnDataChanged;

        /// <summary>
        /// An event fired whenever the admin status of the charging station changed.
        /// </summary>
        public event OnRemoteChargingPoolAdminStatusChangedDelegate  OnAdminStatusChanged;

        /// <summary>
        /// An event fired whenever the dynamic status of the charging station changed.
        /// </summary>
        public event OnRemoteChargingPoolStatusChangedDelegate       OnStatusChanged;

        #endregion


        #region SetAdminStatus(NewAdminStatus)

        /// <summary>
        /// Set the admin status.
        /// </summary>
        /// <param name="NewAdminStatus">A new timestamped admin status.</param>
        public void SetAdminStatus(ChargingPoolAdminStatusTypes  NewAdminStatus)
        {
            _AdminStatusSchedule.Insert(NewAdminStatus);
        }

        #endregion

        #region SetAdminStatus(NewTimestampedAdminStatus)

        /// <summary>
        /// Set the admin status.
        /// </summary>
        /// <param name="NewTimestampedAdminStatus">A new timestamped admin status.</param>
        public void SetAdminStatus(Timestamped<ChargingPoolAdminStatusTypes> NewTimestampedAdminStatus)
        {
            _AdminStatusSchedule.Insert(NewTimestampedAdminStatus);
        }

        #endregion

        #region SetAdminStatus(NewAdminStatus, Timestamp)

        /// <summary>
        /// Set the admin status.
        /// </summary>
        /// <param name="NewAdminStatus">A new admin status.</param>
        /// <param name="Timestamp">The timestamp when this change was detected.</param>
        public void SetAdminStatus(ChargingPoolAdminStatusTypes  NewAdminStatus,
                                   DateTime                         Timestamp)
        {
            _AdminStatusSchedule.Insert(NewAdminStatus, Timestamp);
        }

        #endregion

        #region SetAdminStatus(NewAdminStatusList, ChangeMethod = ChangeMethods.Replace)

        /// <summary>
        /// Set the timestamped admin status.
        /// </summary>
        /// <param name="NewAdminStatusList">A list of new timestamped admin status.</param>
        /// <param name="ChangeMethod">The change mode.</param>
        public void SetAdminStatus(IEnumerable<Timestamped<ChargingPoolAdminStatusTypes>>  NewAdminStatusList,
                                   ChangeMethods                                              ChangeMethod = ChangeMethods.Replace)
        {
            _AdminStatusSchedule.Insert(NewAdminStatusList, ChangeMethod);
        }

        #endregion


        #region SetStatus(NewStatus)

        /// <summary>
        /// Set the current status.
        /// </summary>
        /// <param name="NewStatus">A new status.</param>
        public void SetStatus(ChargingPoolStatusTypes  NewStatus)
        {
            _StatusSchedule.Insert(NewStatus);
        }

        #endregion

        #region SetStatus(NewTimestampedStatus)

        /// <summary>
        /// Set the current status.
        /// </summary>
        /// <param name="NewTimestampedStatus">A new timestamped status.</param>
        public void SetStatus(Timestamped<ChargingPoolStatusTypes> NewTimestampedStatus)
        {
            _StatusSchedule.Insert(NewTimestampedStatus);
        }

        #endregion

        #region SetStatus(NewStatus, Timestamp)

        /// <summary>
        /// Set the status.
        /// </summary>
        /// <param name="NewStatus">A new status.</param>
        /// <param name="Timestamp">The timestamp when this change was detected.</param>
        public void SetStatus(ChargingPoolStatusTypes  NewStatus,
                              DateTime                   Timestamp)
        {
            _StatusSchedule.Insert(NewStatus, Timestamp);
        }

        #endregion

        #region SetStatus(NewStatusList, ChangeMethod = ChangeMethods.Replace)

        /// <summary>
        /// Set the timestamped status.
        /// </summary>
        /// <param name="NewStatusList">A list of new timestamped status.</param>
        /// <param name="ChangeMethod">The change mode.</param>
        public void SetStatus(IEnumerable<Timestamped<ChargingPoolStatusTypes>>  NewStatusList,
                              ChangeMethods                                        ChangeMethod = ChangeMethods.Replace)
        {
            _StatusSchedule.Insert(NewStatusList, ChangeMethod);
        }

        #endregion


        #region (internal) UpdateAdminStatus(Timestamp, EventTrackingId, OldStatus, NewStatus)

        /// <summary>
        /// Update the current status.
        /// </summary>
        /// <param name="Timestamp">The timestamp when this change was detected.</param>
        /// <param name="OldStatus">The old EVSE admin status.</param>
        /// <param name="NewStatus">The new EVSE admin status.</param>
        internal async Task UpdateAdminStatus(DateTime                                   Timestamp,
                                              EventTracking_Id                           EventTrackingId,
                                              Timestamped<ChargingPoolAdminStatusTypes>  OldStatus,
                                              Timestamped<ChargingPoolAdminStatusTypes>  NewStatus)
        {

            OnAdminStatusChanged?.Invoke(Timestamp,
                                         EventTrackingId,
                                         this,
                                         OldStatus,
                                         NewStatus);

        }

        #endregion

        #region (internal) UpdateStatus     (Timestamp, EventTrackingId, OldStatus, NewStatus)

        /// <summary>
        /// Update the current status.
        /// </summary>
        /// <param name="Timestamp">The timestamp when this change was detected.</param>
        /// <param name="OldStatus">The old EVSE status.</param>
        /// <param name="NewStatus">The new EVSE status.</param>
        internal async Task UpdateStatus(DateTime                              Timestamp,
                                         EventTracking_Id                      EventTrackingId,
                                         Timestamped<ChargingPoolStatusTypes>  OldStatus,
                                         Timestamped<ChargingPoolStatusTypes>  NewStatus)
        {

            OnStatusChanged?.Invoke(Timestamp,
                                    EventTrackingId,
                                    this,
                                    OldStatus,
                                    NewStatus);

        }

        #endregion

        #endregion


        #region IComparable<VirtualChargingPool> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        public Int32 CompareTo(Object Object)
        {

            if (Object == null)
                throw new ArgumentNullException(nameof(Object), "The given object must not be null!");

            var VirtualChargingPool = Object as VirtualChargingPool;
            if ((Object) VirtualChargingPool == null)
                throw new ArgumentException("The given object is not a virtual charging station!");

            return CompareTo(VirtualChargingPool);

        }

        #endregion

        #region CompareTo(VirtualChargingPool)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="VirtualChargingPool">An virtual charging station to compare with.</param>
        public Int32 CompareTo(VirtualChargingPool VirtualChargingPool)
        {

            if ((Object) VirtualChargingPool == null)
                throw new ArgumentNullException(nameof(VirtualChargingPool),  "The given virtual charging station must not be null!");

            return Id.CompareTo(VirtualChargingPool.Id);

        }

        #endregion

        #endregion

        #region IEquatable<VirtualChargingPool> Members

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

            var VirtualChargingPool = Object as VirtualChargingPool;
            if ((Object) VirtualChargingPool == null)
                return false;

            return Equals(VirtualChargingPool);

        }

        #endregion

        #region Equals(VirtualChargingPool)

        /// <summary>
        /// Compares two virtual charging stations for equality.
        /// </summary>
        /// <param name="VirtualChargingPool">A virtual charging station to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public Boolean Equals(VirtualChargingPool VirtualChargingPool)
        {

            if ((Object) VirtualChargingPool == null)
                return false;

            return Id.Equals(VirtualChargingPool.Id);

        }

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

            => Id.ToString();

        #endregion



        public IEnumerable<ChargingReservation> ChargingReservations
        {
            get
            {
                return new ChargingReservation[0];
            }
        }

        public bool TryGetReservationById(ChargingReservation_Id ReservationId, out ChargingReservation Reservation)
        {
            throw new NotImplementedException();
        }

        #region CancelReservation(...ReservationId, Reason, ...)

        /// <summary>
        /// Try to remove the given charging reservation.
        /// </summary>
        /// <param name="Timestamp">The timestamp of this request.</param>
        /// <param name="CancellationToken">A token to cancel this request.</param>
        /// <param name="EventTrackingId">An unique event tracking identification for correlating this request with other events.</param>
        /// <param name="ReservationId">The unique charging reservation identification.</param>
        /// <param name="Reason">A reason for this cancellation.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public async Task<CancelReservationResult> CancelReservation(DateTime                               Timestamp,
                                                                     CancellationToken                      CancellationToken,
                                                                     EventTracking_Id                       EventTrackingId,
                                                                     ChargingReservation_Id                 ReservationId,
                                                                     ChargingReservationCancellationReason  Reason,
                                                                     TimeSpan?                              RequestTimeout  = null)
        {
            throw new NotImplementedException();
        }

        #endregion

        public Task<ReservationResult> Reserve(DateTime Timestamp, CancellationToken CancellationToken, EventTracking_Id EventTrackingId, EVSE_Id EVSEId, DateTime? StartTime, TimeSpan? Duration, ChargingReservation_Id? ReservationId = default(ChargingReservation_Id?), eMobilityProvider_Id? ProviderId = default(eMobilityProvider_Id?), ChargingProduct_Id? ChargingProductId = default(ChargingProduct_Id?), IEnumerable<Auth_Token> AuthTokens = null, IEnumerable<eMobilityAccount_Id> eMAIds = null, IEnumerable<uint> PINs = null, TimeSpan? RequestTimeout = default(TimeSpan?))
        {
            throw new NotImplementedException();
        }

        public Task<ReservationResult> Reserve(DateTime Timestamp, CancellationToken CancellationToken, EventTracking_Id EventTrackingId, DateTime? StartTime, TimeSpan? Duration, ChargingReservation_Id? ReservationId = default(ChargingReservation_Id?), eMobilityProvider_Id? ProviderId = default(eMobilityProvider_Id?), ChargingProduct_Id? ChargingProductId = default(ChargingProduct_Id?), IEnumerable<Auth_Token> AuthTokens = null, IEnumerable<eMobilityAccount_Id> eMAIds = null, IEnumerable<uint> PINs = null, TimeSpan? RequestTimeout = default(TimeSpan?))
        {
            throw new NotImplementedException();
        }

        public Task<RemoteStartChargingStationResult> RemoteStart(DateTime Timestamp, CancellationToken CancellationToken, EventTracking_Id EventTrackingId, ChargingProduct_Id? ChargingProductId, ChargingReservation_Id? ReservationId, ChargingSession_Id? SessionId, eMobilityProvider_Id? ProviderId, eMobilityAccount_Id? eMAId, TimeSpan? RequestTimeout = default(TimeSpan?))
        {
            throw new NotImplementedException();
        }

        public Task<RemoteStartEVSEResult> RemoteStart(DateTime Timestamp, CancellationToken CancellationToken, EventTracking_Id EventTrackingId, EVSE_Id EVSEId, ChargingProduct_Id? ChargingProductId, ChargingReservation_Id? ReservationId, ChargingSession_Id? SessionId, eMobilityProvider_Id? ProviderId, eMobilityAccount_Id? eMAId, TimeSpan? RequestTimeout = default(TimeSpan?))
        {
            throw new NotImplementedException();
        }

        public Task<RemoteStopResult> RemoteStop(DateTime Timestamp, CancellationToken CancellationToken, EventTracking_Id EventTrackingId, ChargingSession_Id SessionId, ReservationHandling ReservationHandling, eMobilityProvider_Id? ProviderId, TimeSpan? RequestTimeout = default(TimeSpan?))
        {
            throw new NotImplementedException();
        }

        public Task<RemoteStopEVSEResult> RemoteStop(DateTime Timestamp, CancellationToken CancellationToken, EventTracking_Id EventTrackingId, EVSE_Id EVSEId, ChargingSession_Id SessionId, ReservationHandling ReservationHandling, eMobilityProvider_Id? ProviderId, TimeSpan? RequestTimeout = default(TimeSpan?))
        {
            throw new NotImplementedException();
        }

        public Task<RemoteStartChargingStationResult> RemoteStop(DateTime Timestamp, CancellationToken CancellationToken, EventTracking_Id EventTrackingId, ChargingPool_Id ChargingPoolId, ChargingSession_Id SessionId, ReservationHandling ReservationHandling, eMobilityProvider_Id? ProviderId, TimeSpan? RequestTimeout = default(TimeSpan?))
        {
            throw new NotImplementedException();
        }

        public Task<RemoteStopChargingStationResult> RemoteStop(DateTime Timestamp, CancellationToken CancellationToken, EventTracking_Id EventTrackingId, ChargingStation_Id ChargingStationId, ChargingSession_Id SessionId, ReservationHandling ReservationHandling, eMobilityProvider_Id? ProviderId, TimeSpan? RequestTimeout = default(TimeSpan?))
        {
            throw new NotImplementedException();
        }



    }

}
