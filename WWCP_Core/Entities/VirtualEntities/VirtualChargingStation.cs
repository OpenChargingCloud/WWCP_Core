/*
 * Copyright (c) 2014-2019 GraphDefined GmbH <achim.friedland@graphdefined.com>
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
using System.Threading.Tasks;
using System.Collections.Generic;

using org.GraphDefined.Vanaheimr.Illias;
using org.GraphDefined.Vanaheimr.Styx.Arrows;
using org.GraphDefined.WWCP.ChargingPools;
using org.GraphDefined.Vanaheimr.Hermod;
using Org.BouncyCastle.Bcpg.OpenPgp;
using System.Diagnostics;

#endregion

namespace org.GraphDefined.WWCP.ChargingStations
{

    /// <summary>
    /// A demo implementation of a virtual WWCP charging station.
    /// </summary>
    public class VirtualChargingStation : AEMobilityEntity<ChargingStation_Id>,
                                          IEquatable<VirtualChargingStation>, IComparable<VirtualChargingStation>, IComparable,
                                          IStatus<ChargingStationStatusTypes>,
                                          IRemoteChargingStation
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

        /// <summary>
        /// The default time span between self checks.
        /// </summary>
        public static readonly TimeSpan DefaultSelfCheckTimeSpan = TimeSpan.FromSeconds(3);

        private static readonly Object SelfCheckLock = new Object();
        private Timer _SelfCheckTimer;


        public const String DefaultWhiteListName = "default";

        #endregion

        #region Properties

        public PgpSecretKeyRing SecretKeyRing   { get; }

        public PgpPublicKeyRing PublicKeyRing   { get; }


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
        public Timestamped<ChargingStationAdminStatusTypes> AdminStatus
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

        private StatusSchedule<ChargingStationAdminStatusTypes> _AdminStatusSchedule;

        /// <summary>
        /// The charging station admin status schedule.
        /// </summary>
        public IEnumerable<Timestamped<ChargingStationAdminStatusTypes>> AdminStatusSchedule
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
        public Timestamped<ChargingStationStatusTypes> Status
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

        private StatusSchedule<ChargingStationStatusTypes> _StatusSchedule;

        /// <summary>
        /// The charging station status schedule.
        /// </summary>
        public IEnumerable<Timestamped<ChargingStationStatusTypes>> StatusSchedule
        {
            get
            {
                return _StatusSchedule;
            }
        }

        #endregion

        /// <summary>
        /// The authentication white lists.
        /// </summary>
        public Boolean UseWhiteLists { get; set; }

        /// <summary>
        /// The authentication white lists.
        /// </summary>
        [InternalUseOnly]
        public Dictionary<String, HashSet<LocalAuthentication>> WhiteLists { get; }

        #region DefaultWhiteList

        /// <summary>
        /// The authentication white lists.
        /// </summary>
        [InternalUseOnly]
        public HashSet<LocalAuthentication> DefaultWhiteList
        {
            get
            {
                return WhiteLists[DefaultWhiteListName];
            }
        }

        #endregion

        /// <summary>
        /// The time span between self checks.
        /// </summary>
        public TimeSpan SelfCheckTimeSpan { get; }

        #endregion

        #region Links

        /// <summary>
        /// The optional linked charging pool.
        /// </summary>
        public VirtualChargingPool  ChargingPool      { get; }

        #region OperatorId

        /// <summary>
        /// The identification of the operator of this virtual EVSE.
        /// </summary>
        [InternalUseOnly]
        public ChargingStationOperator_Id OperatorId
            => Id.OperatorId;

        #endregion

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a virtual charging station.
        /// </summary>
        /// <param name="Id">The unique identification of this EVSE.</param>
        /// <param name="ChargingPool">The parent virtual charging pool.</param>
        /// <param name="SelfCheckTimeSpan">The time span between self checks.</param>
        /// <param name="MaxStatusListSize">The maximum size of the charging station status list.</param>
        /// <param name="MaxAdminStatusListSize">The maximum size of the charging station admin status list.</param>
        public VirtualChargingStation(ChargingStation_Id               Id,
                                      VirtualChargingPool              ChargingPool             = null,
                                      PgpSecretKeyRing                 SecretKeyRing            = null,
                                      PgpPublicKeyRing                 PublicKeyRing            = null,
                                      ChargingStationAdminStatusTypes  InitialAdminStatus       = ChargingStationAdminStatusTypes.Operational,
                                      ChargingStationStatusTypes       InitialStatus            = ChargingStationStatusTypes.Available,
                                      UInt16                           MaxAdminStatusListSize   = DefaultMaxAdminStatusListSize,
                                      UInt16                           MaxStatusListSize        = DefaultMaxStatusListSize,
                                      TimeSpan?                        SelfCheckTimeSpan        = null)

            : base(Id)

        {

            #region Init data and properties

            this.ChargingPool          = ChargingPool;
            this.SecretKeyRing         = SecretKeyRing;
            this.PublicKeyRing         = PublicKeyRing;

            this._EVSEs                = new HashSet<IRemoteEVSE>();

            this._AdminStatusSchedule  = new StatusSchedule<ChargingStationAdminStatusTypes>(MaxAdminStatusListSize);
            this._AdminStatusSchedule.Insert(InitialAdminStatus);

            this._StatusSchedule       = new StatusSchedule<ChargingStationStatusTypes>(MaxStatusListSize);
            this._StatusSchedule.Insert(InitialStatus);

            this.WhiteLists           = new Dictionary<String, HashSet<LocalAuthentication>>();
            WhiteLists.Add("default", new HashSet<LocalAuthentication>());

            this.SelfCheckTimeSpan    = SelfCheckTimeSpan != null && SelfCheckTimeSpan.HasValue ? SelfCheckTimeSpan.Value : DefaultSelfCheckTimeSpan;
            this._SelfCheckTimer       = new Timer(SelfCheck, null, this.SelfCheckTimeSpan, this.SelfCheckTimeSpan);

            this._ChargingSessions     = new Dictionary<ChargingSession_Id, ChargingSession>();

            #endregion

            #region Link events

            this._AdminStatusSchedule.OnStatusChanged += (Timestamp, EventTrackingId, StatusSchedule, OldStatus, NewStatus)
                                                          => UpdateAdminStatus(Timestamp, EventTrackingId, OldStatus, NewStatus);

            this._StatusSchedule.     OnStatusChanged += (Timestamp, EventTrackingId, StatusSchedule, OldStatus, NewStatus)
                                                          => UpdateStatus(Timestamp, EventTrackingId, OldStatus, NewStatus);

            #endregion

        }

        #endregion


        public ChargingStation_Id     RemoteChargingStationId    { get; set; }

        public String                 RemoteEVSEIdPrefix         { get; set; }

        public void AddMapping(EVSE_Id LocalEVSEId,
                               EVSE_Id RemoteEVSEId)
        {
        }


        #region (Admin-)Status management

        #region OnData/(Admin)StatusChanged

        /// <summary>
        /// An event fired whenever the static data of the charging station changed.
        /// </summary>
        public event OnRemoteChargingStationDataChangedDelegate         OnDataChanged;

        /// <summary>
        /// An event fired whenever the admin status of the charging station changed.
        /// </summary>
        public event OnRemoteChargingStationAdminStatusChangedDelegate  OnAdminStatusChanged;

        /// <summary>
        /// An event fired whenever the dynamic status of the charging station changed.
        /// </summary>
        public event OnRemoteChargingStationStatusChangedDelegate       OnStatusChanged;

        #endregion


        #region SetAdminStatus(NewAdminStatus)

        /// <summary>
        /// Set the admin status.
        /// </summary>
        /// <param name="NewAdminStatus">A new timestamped admin status.</param>
        public void SetAdminStatus(ChargingStationAdminStatusTypes  NewAdminStatus)
        {
            _AdminStatusSchedule.Insert(NewAdminStatus);
        }

        #endregion

        #region SetAdminStatus(NewTimestampedAdminStatus)

        /// <summary>
        /// Set the admin status.
        /// </summary>
        /// <param name="NewTimestampedAdminStatus">A new timestamped admin status.</param>
        public void SetAdminStatus(Timestamped<ChargingStationAdminStatusTypes> NewTimestampedAdminStatus)
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
        public void SetAdminStatus(ChargingStationAdminStatusTypes  NewAdminStatus,
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
        public void SetAdminStatus(IEnumerable<Timestamped<ChargingStationAdminStatusTypes>>  NewAdminStatusList,
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
        public void SetStatus(ChargingStationStatusTypes  NewStatus)
        {
            _StatusSchedule.Insert(NewStatus);
        }

        #endregion

        #region SetStatus(NewTimestampedStatus)

        /// <summary>
        /// Set the current status.
        /// </summary>
        /// <param name="NewTimestampedStatus">A new timestamped status.</param>
        public void SetStatus(Timestamped<ChargingStationStatusTypes> NewTimestampedStatus)
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
        public void SetStatus(ChargingStationStatusTypes  NewStatus,
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
        public void SetStatus(IEnumerable<Timestamped<ChargingStationStatusTypes>>  NewStatusList,
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
        internal async Task UpdateAdminStatus(DateTime                                      Timestamp,
                                              EventTracking_Id                              EventTrackingId,
                                              Timestamped<ChargingStationAdminStatusTypes>  OldStatus,
                                              Timestamped<ChargingStationAdminStatusTypes>  NewStatus)
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
        internal async Task UpdateStatus(DateTime                                 Timestamp,
                                         EventTracking_Id                         EventTrackingId,
                                         Timestamped<ChargingStationStatusTypes>  OldStatus,
                                         Timestamped<ChargingStationStatusTypes>  NewStatus)
        {

            OnStatusChanged?.Invoke(Timestamp,
                                    EventTrackingId,
                                    this,
                                    OldStatus,
                                    NewStatus);

        }

        #endregion

        #endregion

        #region (private) SelfCheck(Context)

        private void SelfCheck(Object Context)
        {

            if (Monitor.TryEnter(SelfCheckLock))
            {

                try
                {

                    foreach (var _EVSE in _EVSEs)
                        _EVSE.CheckIfReservationIsExpired().Wait();

                }
                catch (Exception e)
                {

                    while (e.InnerException != null)
                        e = e.InnerException;

                    DebugX.LogT("VirtualChargingStation SelfCheck() '" + Id + "' led to an exception: " + e.Message + Environment.NewLine + e.StackTrace);

                }

                finally
                {
                    Monitor.Exit(SelfCheckLock);
                }

            }

        }

        #endregion


        #region EVSEs...

        #region EVSEs

        private readonly HashSet<IRemoteEVSE> _EVSEs;

        /// <summary>
        /// All registered EVSEs.
        /// </summary>
        public IEnumerable<IRemoteEVSE> EVSEs
            => _EVSEs;

        #endregion

        #region CreateVirtualEVSE(EVSEId, Configurator = null, OnSuccess = null, OnError = null)

        /// <summary>
        /// Create and register a new EVSE having the given
        /// unique EVSE identification.
        /// </summary>
        /// <param name="EVSEId">The unique identification of the new EVSE.</param>
        /// <param name="Configurator">An optional delegate to configure the new EVSE after its creation.</param>
        /// <param name="OnSuccess">An optional delegate called after successful creation of the EVSE.</param>
        /// <param name="OnError">An optional delegate for signaling errors.</param>
        public VirtualEVSE CreateVirtualEVSE(EVSE_Id                       EVSEId,
                                             EnergyMeter_Id                EnergyMeterId,
                                             PgpSecretKeyRing              SecretKeyRing            = null,
                                             PgpPublicKeyRing              PublicKeyRing            = null,
                                             EVSEAdminStatusTypes          InitialAdminStatus       = EVSEAdminStatusTypes.Operational,
                                             EVSEStatusTypes               InitialStatus            = EVSEStatusTypes.Available,
                                             UInt16                        MaxAdminStatusListSize   = DefaultMaxAdminStatusListSize,
                                             UInt16                        MaxStatusListSize        = DefaultMaxStatusListSize,
                                             Action<VirtualEVSE>           Configurator             = null,
                                             Action<VirtualEVSE>           OnSuccess                = null,
                                             Action<VirtualEVSE, EVSE_Id>  OnError                  = null)
        {

            #region Initial checks

            if (_EVSEs.Any(evse => evse.Id == EVSEId))
            {
                throw new Exception("EVSEAlreadyExistsInStation");
               // if (OnError == null)
               //     throw new EVSEAlreadyExistsInStation(this.ChargingStation, EVSEId);
               // else
               //     OnError?.Invoke(this, EVSEId);
            }

            #endregion

            var Now           = DateTime.UtcNow;
            var _VirtualEVSE  = new VirtualEVSE(EVSEId,
                                                this,
                                                EnergyMeterId,
                                                SecretKeyRing,
                                                PublicKeyRing,
                                                InitialAdminStatus,
                                                InitialStatus,
                                                MaxAdminStatusListSize,
                                                MaxStatusListSize);

            Configurator?.Invoke(_VirtualEVSE);

            if (_EVSEs.Add(_VirtualEVSE))
            {

                //_VirtualChargingStation.OnPropertyChanged        += (Timestamp, Sender, PropertyName, OldValue, NewValue)
                //                                           => UpdateEVSEData(Timestamp, Sender as VirtualChargingStation, PropertyName, OldValue, NewValue);
                //
                //_VirtualChargingStation.OnStatusChanged          += UpdateEVSEStatus;
                //_VirtualChargingStation.OnAdminStatusChanged     += UpdateEVSEAdminStatus;
                //_VirtualChargingStation.OnNewReservation         += SendNewReservation;
                //_VirtualChargingStation.OnNewChargingSession     += SendNewChargingSession;
                //_VirtualChargingStation.OnNewChargeDetailRecord  += SendNewChargeDetailRecord;
                //_VirtualChargingStation.OnCancelReservationResponse   += SendOnCancelReservationResponse;

                OnSuccess?.Invoke(_VirtualEVSE);

                return _VirtualEVSE;

            }

            return null;

        }

        #endregion

        public IRemoteEVSE AddEVSE(IRemoteEVSE                       EVSE,
                                   Action<EVSE>                      Configurator  = null,
                                   Action<EVSE>                      OnSuccess     = null,
                                   Action<ChargingStation, EVSE_Id>  OnError       = null)

        {

            _EVSEs.Add(EVSE);

            return EVSE;

        }


        #region ContainsEVSE(EVSE)

        /// <summary>
        /// Check if the given EVSE is already present within the charging station.
        /// </summary>
        /// <param name="EVSE">An EVSE.</param>
        public Boolean ContainsEVSE(EVSE EVSE)
            => _EVSEs.Any(evse => evse.Id == EVSE.Id);

        #endregion

        #region ContainsEVSE(EVSEId)

        /// <summary>
        /// Check if the given EVSE identification is already present within the charging station.
        /// </summary>
        /// <param name="EVSEId">The unique identification of an EVSE.</param>
        public Boolean ContainsEVSE(EVSE_Id EVSEId)
            => _EVSEs.Any(evse => evse.Id == EVSEId);

        #endregion

        #region GetEVSEById(EVSEId)

        public IRemoteEVSE GetEVSEById(EVSE_Id EVSEId)
            => _EVSEs.FirstOrDefault(evse => evse.Id == EVSEId);

        #endregion

        #region TryGetEVSEbyId(EVSEId, out EVSE)

        public Boolean TryGetEVSEbyId(EVSE_Id EVSEId, out IRemoteEVSE EVSE)
        {

            EVSE = GetEVSEById(EVSEId);

            return EVSE != null;

        }

        #endregion


        #region OnRemoteEVSEData/(Admin)StatusChanged

        /// <summary>
        /// An event fired whenever the static data of any subordinated EVSE changed.
        /// </summary>
        public event OnRemoteEVSEDataChangedDelegate         OnEVSEDataChanged;

        /// <summary>
        /// An event fired whenever the dynamic status of any subordinated EVSE changed.
        /// </summary>
        public event OnRemoteEVSEStatusChangedDelegate       OnEVSEStatusChanged;

        /// <summary>
        /// An event fired whenever the admin status of any subordinated EVSE changed.
        /// </summary>
        public event OnRemoteEVSEAdminStatusChangedDelegate  OnEVSEAdminStatusChanged;

        #endregion

        #region (internal) UpdateEVSEData(Timestamp, RemoteEVSE, OldStatus, NewStatus)

        /// <summary>
        /// Update the data of a remote EVSE.
        /// </summary>
        /// <param name="Timestamp">The timestamp when this change was detected.</param>
        /// <param name="RemoteEVSE">The remote EVSE.</param>
        /// <param name="PropertyName">The name of the changed property.</param>
        /// <param name="OldValue">The old value of the changed property.</param>
        /// <param name="NewValue">The new value of the changed property.</param>
        internal void UpdateEVSEData(DateTime     Timestamp,
                                     IRemoteEVSE  RemoteEVSE,
                                     String       PropertyName,
                                     Object       OldValue,
                                     Object       NewValue)
        {

            var OnEVSEDataChangedLocal = OnEVSEDataChanged;
            if (OnEVSEDataChangedLocal != null)
                OnEVSEDataChangedLocal(Timestamp, RemoteEVSE, PropertyName, OldValue, NewValue);

        }

        #endregion

        #region (internal) UpdateEVSEAdminStatus(Timestamp, EventTrackingId, RemoteEVSE, OldStatus, NewStatus)

        /// <summary>
        /// Update the current charging station status.
        /// </summary>
        /// <param name="Timestamp">The timestamp when this change was detected.</param>
        /// <param name="EventTrackingId">An event tracking identification for correlating this request with other events.</param>
        /// <param name="RemoteEVSE">The updated remote EVSE.</param>
        /// <param name="OldStatus">The old EVSE status.</param>
        /// <param name="NewStatus">The new EVSE status.</param>
        internal async Task UpdateEVSEAdminStatus(DateTime                          Timestamp,
                                                  EventTracking_Id                  EventTrackingId,
                                                  IRemoteEVSE                       RemoteEVSE,
                                                  Timestamped<EVSEAdminStatusTypes>  OldStatus,
                                                  Timestamped<EVSEAdminStatusTypes>  NewStatus)
        {

            var OnEVSEAdminStatusChangedLocal = OnEVSEAdminStatusChanged;
            if (OnEVSEAdminStatusChangedLocal != null)
                await OnEVSEAdminStatusChangedLocal(Timestamp,
                                                    EventTrackingId,
                                                    RemoteEVSE,
                                                    OldStatus,
                                                    NewStatus);

        }

        #endregion

        #region (internal) UpdateEVSEStatus     (Timestamp, EventTrackingId, RemoteEVSE, OldStatus, NewStatus)

        /// <summary>
        /// Update the remote EVSE station status.
        /// </summary>
        /// <param name="Timestamp">The timestamp when this change was detected.</param>
        /// <param name="EventTrackingId">An event tracking identification for correlating this request with other events.</param>
        /// <param name="RemoteEVSE">The updated EVSE.</param>
        /// <param name="OldStatus">The old EVSE status.</param>
        /// <param name="NewStatus">The new EVSE status.</param>
        internal async Task UpdateEVSEStatus(DateTime                     Timestamp,
                                             EventTracking_Id             EventTrackingId,
                                             IRemoteEVSE                  RemoteEVSE,
                                             Timestamped<EVSEStatusTypes>  OldStatus,
                                             Timestamped<EVSEStatusTypes>  NewStatus)
        {

            var OnEVSEStatusChangedLocal = OnEVSEStatusChanged;
            if (OnEVSEStatusChangedLocal != null)
                await OnEVSEStatusChangedLocal(Timestamp,
                                               EventTrackingId,
                                               RemoteEVSE,
                                               OldStatus,
                                               NewStatus);

        }

        #endregion


        IRemoteEVSE IRemoteChargingStation.CreateNewEVSE(EVSE_Id EVSEId, Action<EVSE> Configurator = null, Action<EVSE> OnSuccess = null, Action<ChargingStation, EVSE_Id> OnError = null)
        {
            throw new NotImplementedException();
        }


        // Socket events

        #region SocketOutletAddition

        internal readonly IVotingNotificator<DateTime, VirtualChargingStation, SocketOutlet, Boolean> SocketOutletAddition;

        /// <summary>
        /// Called whenever a socket outlet will be or was added.
        /// </summary>
        public IVotingSender<DateTime, VirtualChargingStation, SocketOutlet, Boolean> OnSocketOutletAddition
        {
            get
            {
                return SocketOutletAddition;
            }
        }

        #endregion

        #region SocketOutletRemoval

        internal readonly IVotingNotificator<DateTime, VirtualChargingStation, SocketOutlet, Boolean> SocketOutletRemoval;

        /// <summary>
        /// Called whenever a socket outlet will be or was removed.
        /// </summary>
        public IVotingSender<DateTime, VirtualChargingStation, SocketOutlet, Boolean> OnSocketOutletRemoval
        {
            get
            {
                return SocketOutletRemoval;
            }
        }

        #endregion


        #region GetEVSEStatus(...)

        public async Task<IEnumerable<EVSEStatus>> GetEVSEStatus(DateTime           Timestamp,
                                                                 CancellationToken  CancellationToken,
                                                                 EventTracking_Id   EventTrackingId,
                                                                 TimeSpan?          RequestTimeout  = null)

            => _EVSEs.Select(evse => new EVSEStatus(evse.Id,
                                                    new Timestamped<EVSEStatusTypes>(
                                                        evse.Status.Timestamp,
                                                        evse.Status.Value
                                                    )));

        #endregion

        #endregion

        #region Reservations...

        #region Data

        private readonly Dictionary<ChargingReservation_Id, ChargingReservation> _Reservations;

        /// <summary>
        /// All current charging reservations.
        /// </summary>
        public IEnumerable<ChargingReservation> Reservations
            => _Reservations.Select(_ => _.Value);

        #region TryGetReservationById(ReservationId, out Reservation)

        /// <summary>
        /// Return the charging reservation specified by the given identification.
        /// </summary>
        /// <param name="ReservationId">The charging reservation identification.</param>
        /// <param name="Reservation">The charging reservation.</param>
        public Boolean TryGetChargingReservationById(ChargingReservation_Id ReservationId, out ChargingReservation Reservation)
            => _Reservations.TryGetValue(ReservationId, out Reservation);

        #endregion

        #endregion

        #region Events

        /// <summary>
        /// An event fired whenever a charging location is being reserved.
        /// </summary>
        public event OnReserveRequestDelegate             OnReserveRequest;

        /// <summary>
        /// An event fired whenever a charging location was reserved.
        /// </summary>
        public event OnReserveResponseDelegate            OnReserveResponse;

        /// <summary>
        /// An event fired whenever a new charging reservation was created.
        /// </summary>
        public event OnNewReservationDelegate             OnNewReservation;


        /// <summary>
        /// An event fired whenever a charging reservation is being canceled.
        /// </summary>
        public event OnCancelReservationRequestDelegate   OnCancelReservationRequest;

        /// <summary>
        /// An event fired whenever a charging reservation was canceled.
        /// </summary>
        public event OnCancelReservationResponseDelegate  OnCancelReservationResponse;

        /// <summary>
        /// An event fired whenever a charging reservation was canceled.
        /// </summary>
        public event OnReservationCanceledDelegate        OnReservationCanceled;

        #endregion

        #region Reserve(                                           StartTime = null, Duration = null, ReservationId = null, ProviderId = null, ...)

        /// <summary>
        /// Reserve the possibility to charge at this station.
        /// </summary>
        /// <param name="StartTime">The starting time of the reservation.</param>
        /// <param name="Duration">The duration of the reservation.</param>
        /// <param name="ReservationId">An optional unique identification of the reservation. Mandatory for updates.</param>
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
        public Task<ReservationResult>

            Reserve(DateTime?                         StartTime              = null,
                    TimeSpan?                         Duration               = null,
                    ChargingReservation_Id?           ReservationId          = null,
                    eMobilityProvider_Id?             ProviderId             = null,
                    RemoteAuthentication              RemoteAuthentication   = null,
                    ChargingProduct                   ChargingProduct        = null,
                    IEnumerable<Auth_Token>           AuthTokens             = null,
                    IEnumerable<eMobilityAccount_Id>  eMAIds                 = null,
                    IEnumerable<UInt32>               PINs                   = null,

                    DateTime?                         Timestamp              = null,
                    CancellationToken?                CancellationToken      = null,
                    EventTracking_Id                  EventTrackingId        = null,
                    TimeSpan?                         RequestTimeout         = null)


                => Reserve(ChargingLocation.FromChargingStationId(Id),
                           ChargingReservationLevel.ChargingStation,
                           StartTime,
                           Duration,
                           ReservationId,
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

        #endregion

        #region Reserve(ChargingLocation, ReservationLevel = EVSE, StartTime = null, Duration = null, ReservationId = null, ProviderId = null, ...)

        /// <summary>
        /// Reserve the possibility to charge at the given charging location.
        /// </summary>
        /// <param name="ChargingLocation">A charging location.</param>
        /// <param name="ReservationLevel">The level of the reservation to create (EVSE, charging station, ...).</param>
        /// <param name="StartTime">The starting time of the reservation.</param>
        /// <param name="Duration">The duration of the reservation.</param>
        /// <param name="ReservationId">An optional unique identification of the reservation. Mandatory for updates.</param>
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

            Reserve(ChargingLocation                  ChargingLocation,
                    ChargingReservationLevel          ReservationLevel       = ChargingReservationLevel.EVSE,
                    DateTime?                         StartTime              = null,
                    TimeSpan?                         Duration               = null,
                    ChargingReservation_Id?           ReservationId          = null,
                    eMobilityProvider_Id?             ProviderId             = null,
                    RemoteAuthentication              RemoteAuthentication   = null,
                    ChargingProduct                   ChargingProduct        = null,
                    IEnumerable<Auth_Token>           AuthTokens             = null,
                    IEnumerable<eMobilityAccount_Id>  eMAIds                 = null,
                    IEnumerable<UInt32>               PINs                   = null,

                    DateTime?                         Timestamp              = null,
                    CancellationToken?                CancellationToken      = null,
                    EventTracking_Id                  EventTrackingId        = null,
                    TimeSpan?                         RequestTimeout         = null)

        {

            #region Initial checks

            if (!Timestamp.HasValue)
                Timestamp = DateTime.UtcNow;

            if (!CancellationToken.HasValue)
                CancellationToken = new CancellationTokenSource().Token;

            if (EventTrackingId == null)
                EventTrackingId = EventTracking_Id.New;


            ChargingReservation newReservation  = null;
            ReservationResult   result          = null;

            #endregion

            #region Send OnReserveRequest event

            var Runtime = Stopwatch.StartNew();

            try
            {

                OnReserveRequest?.Invoke(DateTime.UtcNow,
                                         Timestamp.Value,
                                         this,
                                         EventTrackingId,
                                         ReservationId,
                                         ChargingLocation,
                                         StartTime,
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
                e.Log(nameof(EVSE) + "." + nameof(OnReserveRequest));
            }

            #endregion


            if (ChargingLocation.ChargingStationId.HasValue && ChargingLocation.ChargingStationId.Value != Id)
                result = ReservationResult.UnknownLocation;

            else if (AdminStatus.Value == ChargingStationAdminStatusTypes.Operational ||
                     AdminStatus.Value == ChargingStationAdminStatusTypes.InternalUse)
            {

                #region Check if the eMAId is on the white list

                if (UseWhiteLists &&
                   !WhiteLists["default"].Contains(RemoteAuthentication.ToLocal))
                {
                    result = ReservationResult.InvalidCredentials;
                }

                #endregion

                else
                {

                    if (ReservationLevel == ChargingReservationLevel.EVSE &&
                        ChargingLocation.EVSEId.HasValue &&
                        TryGetEVSEbyId(ChargingLocation.EVSEId.Value, out IRemoteEVSE remoteEVSE))
                    {

                        result = await remoteEVSE.
                                           Reserve(ChargingLocation,
                                                   ReservationLevel,
                                                   StartTime,
                                                   Duration,
                                                   ReservationId,
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

                        newReservation = result.Reservation;

                    }

                    else if (ReservationLevel == ChargingReservationLevel.ChargingStation &&
                             ChargingLocation.ChargingStationId.HasValue)
                    {

                        var results = new List<ReservationResult>();

                        foreach (var remoteEVSE2 in _EVSEs)
                        {

                            results.Add(await remoteEVSE2.
                                                  Reserve(ChargingLocation,
                                                          ReservationLevel,
                                                          StartTime,
                                                          Duration,
                                                          ChargingReservation_Id.Random(OperatorId),
                                                          ProviderId,
                                                          RemoteAuthentication,
                                                          ChargingProduct,
                                                          AuthTokens,
                                                          eMAIds,
                                                          PINs,

                                                          Timestamp,
                                                          CancellationToken,
                                                          EventTrackingId,
                                                          RequestTimeout));

                        }

                        var newReservations = results.Where (_result => _result.Result == ReservationResultType.Success).
                                                      Select(_result => _result.Reservation).
                                                      ToArray();

                        if (newReservations.Length > 0)
                        {

                            newReservation = new ChargingReservation(ReservationId:           ReservationId ?? ChargingReservation_Id.Random(OperatorId),
                                                                     Timestamp:               Timestamp.Value,
                                                                     StartTime:               StartTime ?? DateTime.UtcNow,
                                                                     Duration:                Duration  ?? MaxReservationDuration,
                                                                     EndTime:                 (StartTime ?? DateTime.UtcNow) + (Duration ?? MaxReservationDuration),
                                                                     ConsumedReservationTime: TimeSpan.FromSeconds(0),
                                                                     ReservationLevel:        ReservationLevel,
                                                                     ProviderId:              ProviderId,
                                                                     Identification:          RemoteAuthentication,
                                                                     RoamingNetwork:          null,
                                                                     ChargingPoolId:          null,
                                                                     ChargingStationId:       Id,
                                                                     EVSEId:                  null,
                                                                     ChargingProduct:         ChargingProduct,
                                                                     SubReservations:         newReservations);

                            foreach (var subReservation in newReservation.SubReservations)
                            {
                                subReservation.ParentReservation = newReservation;
                                subReservation.ChargingStationId = Id;
                            }

                            result = ReservationResult.Success(newReservation);

                        }

                        else
                            result = ReservationResult.AlreadyReserved;

                    }

                    else
                        result = ReservationResult.UnknownLocation;

                }

            }
            else
            {

                switch (AdminStatus.Value)
                {

                    default:
                        return ReservationResult.OutOfService;

                }

            }


            if (result.Result == ReservationResultType.Success &&
                newReservation != null)
            {

                _Reservations.Add(newReservation.Id, newReservation);

                foreach (var subReservation in newReservation.SubReservations)
                    _Reservations.Add(subReservation.Id, subReservation);

                OnNewReservation?.Invoke(DateTime.UtcNow,
                                         this,
                                         newReservation);

            }


            #region Send OnReserveResponse event

            Runtime.Stop();

            try
            {

                OnReserveResponse?.Invoke(DateTime.UtcNow,
                                          Timestamp.Value,
                                          this,
                                          EventTrackingId,
                                          ReservationId,
                                          ChargingLocation,
                                          StartTime,
                                          Duration,
                                          ProviderId,
                                          RemoteAuthentication,
                                          ChargingProduct,
                                          AuthTokens,
                                          eMAIds,
                                          PINs,
                                          result,
                                          Runtime.Elapsed,
                                          RequestTimeout);

            }
            catch (Exception e)
            {
                e.Log(nameof(EVSE) + "." + nameof(OnReserveResponse));
            }

            #endregion

            return result;

        }

        #endregion

        #region CancelReservation(ReservationId, Reason, ProviderId = null, ...)

        /// <summary>
        /// Try to remove the given charging reservation.
        /// </summary>
        /// <param name="ReservationId">The unique charging reservation identification.</param>
        /// <param name="Reason">A reason for this cancellation.</param>
        /// <param name="ProviderId">An optional unique identification of e-Mobility service provider.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public async Task<CancelReservationResult>

            CancelReservation(ChargingReservation_Id                 ReservationId,
                              ChargingReservationCancellationReason  Reason,
                              eMobilityProvider_Id?                  ProviderId         = null,

                              DateTime?                              Timestamp          = null,
                              CancellationToken?                     CancellationToken  = null,
                              EventTracking_Id                       EventTrackingId    = null,
                              TimeSpan?                              RequestTimeout     = null)

        {

            #region Initial checks

            if (!Timestamp.HasValue)
                Timestamp = DateTime.UtcNow;

            if (!CancellationToken.HasValue)
                CancellationToken = new CancellationTokenSource().Token;

            if (EventTrackingId == null)
                EventTrackingId = EventTracking_Id.New;


            CancelReservationResult result = null;

            #endregion

            #region Check admin status

            if (AdminStatus.Value != ChargingStationAdminStatusTypes.Operational &&
                AdminStatus.Value != ChargingStationAdminStatusTypes.InternalUse)
                return CancelReservationResult.OutOfService(ReservationId,
                                                            Reason);

            #endregion


            var _Reservation = Reservations.FirstOrDefault(reservation => reservation.Id == ReservationId);

            if (_Reservation        != null &&
                _Reservation.EVSEId.HasValue)
            {

                result = await GetEVSEById(_Reservation.EVSEId.Value).
                                   CancelReservation(ReservationId,
                                                     Reason,
                                                     ProviderId,

                                                     Timestamp,
                                                     CancellationToken,
                                                     EventTrackingId,
                                                     RequestTimeout);

                if (result.Result != CancelReservationResultTypes.UnknownReservationId)
                    return result;

            }

            foreach (var evse in _EVSEs)
            {

                result = await evse.CancelReservation(ReservationId,
                                                      Reason,
                                                      ProviderId,

                                                      Timestamp,
                                                      CancellationToken,
                                                      EventTrackingId,
                                                      RequestTimeout);

                if (result.Result != CancelReservationResultTypes.UnknownReservationId)
                    return result;

            }

            return CancelReservationResult.UnknownReservationId(ReservationId,
                                                                Reason);

        }

        #endregion


        #region SendNewReservation

        internal void SendNewReservation(DateTime             Timestamp,
                                         Object               Sender,
                                         ChargingReservation  Reservation)
        {

            OnNewReservation?.Invoke(Timestamp,
                                     Sender,
                                     Reservation);

        }

        #endregion

        #region SendReservationCanceled

        internal void SendReservationCanceled(DateTime                               Timestamp,
                                              Object                                 Sender,
                                              ChargingReservation                    Reservation,
                                              ChargingReservationCancellationReason  Reason)
        {

            OnReservationCanceled?.Invoke(Timestamp,
                                          Sender,
                                          Reservation,
                                          Reason);

        }

        #endregion

        #endregion

        #region RemoteStart/-Stop and Sessions

        #region Data

        private readonly Dictionary<ChargingSession_Id, ChargingSession> _ChargingSessions;

        public IEnumerable<ChargingSession> ChargingSessions
            => _ChargingSessions.Select(_ => _.Value);

        #region TryGetChargingSessionById(SessionId, out ChargingSession)

        /// <summary>
        /// Return the charging session specified by the given identification.
        /// </summary>
        /// <param name="SessionId">The charging session identification.</param>
        /// <param name="ChargingSession">The charging session.</param>
        public Boolean TryGetChargingSessionById(ChargingSession_Id SessionId, out ChargingSession ChargingSession)
            => _ChargingSessions.TryGetValue(SessionId, out ChargingSession);

        #endregion

        #endregion

        #region Events

        /// <summary>
        /// An event fired whenever a remote start command was received.
        /// </summary>
        public event OnRemoteStartRequestDelegate     OnRemoteStartRequest;

        /// <summary>
        /// An event fired whenever a remote start command completed.
        /// </summary>
        public event OnRemoteStartResponseDelegate    OnRemoteStartResponse;


        /// <summary>
        /// An event fired whenever a remote stop command was received.
        /// </summary>
        public event OnRemoteStopRequestDelegate      OnRemoteStopRequest;

        /// <summary>
        /// An event fired whenever a remote stop command completed.
        /// </summary>
        public event OnRemoteStopResponseDelegate     OnRemoteStopResponse;


        /// <summary>
        /// An event fired whenever a new charging session was created.
        /// </summary>
        public event OnNewChargingSessionDelegate     OnNewChargingSession;

        /// <summary>
        /// An event fired whenever a new charge detail record was created.
        /// </summary>
        public event OnNewChargeDetailRecordDelegate  OnNewChargeDetailRecord;

        #endregion

        #region RemoteStart(                  ChargingProduct = null, ReservationId = null, SessionId = null, ProviderId = null, RemoteAuthentication = null, ...)

        /// <summary>
        /// Start a charging session.
        /// </summary>
        /// <param name="ChargingProduct">The choosen charging product.</param>
        /// <param name="ReservationId">The unique identification for a charging reservation.</param>
        /// <param name="SessionId">The unique identification for this charging session.</param>
        /// <param name="ProviderId">The unique identification of the e-mobility service provider for the case it is different from the current message sender.</param>
        /// <param name="RemoteAuthentication">The unique identification of the e-mobility account.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public Task<RemoteStartResult>

            RemoteStart(ChargingProduct          ChargingProduct        = null,
                        ChargingReservation_Id?  ReservationId          = null,
                        ChargingSession_Id?      SessionId              = null,
                        eMobilityProvider_Id?    ProviderId             = null,
                        RemoteAuthentication     RemoteAuthentication   = null,

                        DateTime?                Timestamp              = null,
                        CancellationToken?       CancellationToken      = null,
                        EventTracking_Id         EventTrackingId        = null,
                        TimeSpan?                RequestTimeout         = null)


                => RemoteStart(ChargingLocation.FromChargingStationId(Id),
                               ChargingProduct,
                               ReservationId,
                               SessionId,
                               ProviderId,
                               RemoteAuthentication,

                               Timestamp,
                               CancellationToken,
                               EventTrackingId,
                               RequestTimeout);

        #endregion

        #region RemoteStart(ChargingLocation, ChargingProduct = null, ReservationId = null, SessionId = null, ProviderId = null, RemoteAuthentication = null, ...)

        /// <summary>
        /// Start a charging session at the given EVSE.
        /// </summary>
        /// <param name="ChargingLocation">The charging location.</param>
        /// <param name="ChargingProduct">The choosen charging product.</param>
        /// <param name="ReservationId">The unique identification for a charging reservation.</param>
        /// <param name="SessionId">The unique identification for this charging session.</param>
        /// <param name="ProviderId">The unique identification of the e-mobility service provider for the case it is different from the current message sender.</param>
        /// <param name="RemoteAuthentication">The unique identification of the e-mobility account.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public async Task<RemoteStartResult>

            RemoteStart(ChargingLocation         ChargingLocation,
                        ChargingProduct          ChargingProduct        = null,
                        ChargingReservation_Id?  ReservationId          = null,
                        ChargingSession_Id?      SessionId              = null,
                        eMobilityProvider_Id?    ProviderId             = null,
                        RemoteAuthentication     RemoteAuthentication   = null,

                        DateTime?                Timestamp              = null,
                        CancellationToken?       CancellationToken      = null,
                        EventTracking_Id         EventTrackingId        = null,
                        TimeSpan?                RequestTimeout         = null)
        {

            #region Initial checks

            if (!Timestamp.HasValue)
                Timestamp = DateTime.UtcNow;

            if (!CancellationToken.HasValue)
                CancellationToken = new CancellationTokenSource().Token;

            if (EventTrackingId == null)
                EventTrackingId = EventTracking_Id.New;


            RemoteStartResult result = null;

            #endregion

            if (AdminStatus.Value == ChargingStationAdminStatusTypes.Operational ||
                AdminStatus.Value == ChargingStationAdminStatusTypes.InternalUse)
            {

                #region Check if the eMAId is on the white list

                if (UseWhiteLists &&
                   !WhiteLists["default"].Contains(RemoteAuthentication.ToLocal))
                    return RemoteStartResult.InvalidCredentials;

                #endregion

                if (!ChargingLocation.EVSEId.HasValue)
                    return RemoteStartResult.UnknownLocation;

                var remoteEVSE = GetEVSEById(ChargingLocation.EVSEId.Value);
                if (remoteEVSE == null)
                    return RemoteStartResult.UnknownLocation;


                return await remoteEVSE.
                                 RemoteStart(ChargingProduct,
                                             ReservationId,
                                             SessionId,
                                             ProviderId,
                                             RemoteAuthentication,

                                             Timestamp,
                                             CancellationToken,
                                             EventTrackingId,
                                             RequestTimeout);

            }
            else
            {

                switch (AdminStatus.Value)
                {

                    default:
                        return RemoteStartResult.OutOfService;

                }

            }

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
        public async Task<RemoteStopResult>

            RemoteStop(ChargingSession_Id     SessionId,
                       ReservationHandling?   ReservationHandling    = null,
                       eMobilityProvider_Id?  ProviderId             = null,
                       RemoteAuthentication   RemoteAuthentication   = null,

                       DateTime?              Timestamp              = null,
                       CancellationToken?     CancellationToken      = null,
                       EventTracking_Id       EventTrackingId        = null,
                       TimeSpan?              RequestTimeout         = null)

        {

            #region Initial checks

            if (!Timestamp.HasValue)
                Timestamp = DateTime.UtcNow;

            if (!CancellationToken.HasValue)
                CancellationToken = new CancellationTokenSource().Token;

            if (EventTrackingId == null)
                EventTrackingId = EventTracking_Id.New;

            #endregion


            if (AdminStatus.Value == ChargingStationAdminStatusTypes.Operational ||
                AdminStatus.Value == ChargingStationAdminStatusTypes.InternalUse)
            {

                #region Check if the eMAId is on the white list

                if (UseWhiteLists &&
                   !WhiteLists["default"].Contains(RemoteAuthentication.ToLocal))
                    return RemoteStopResult.InvalidCredentials(SessionId);

                #endregion

                var remoteEVSE = _EVSEs.FirstOrDefault(evse => evse.ChargingSessions.Any(session => session.Id == SessionId));

                if (remoteEVSE == null)
                    return RemoteStopResult.InvalidSessionId(SessionId);


                var result = await remoteEVSE.
                                       RemoteStop(SessionId,
                                                  ReservationHandling,
                                                  ProviderId,
                                                  RemoteAuthentication,

                                                  Timestamp,
                                                  CancellationToken,
                                                  EventTrackingId,
                                                  RequestTimeout);

                switch (result.Result)
                {

                    case RemoteStopResultType.Error:
                        return RemoteStopResult.Error(SessionId, result.Message);

                    case RemoteStopResultType.InternalUse:
                        return RemoteStopResult.InternalUse(SessionId);

                    case RemoteStopResultType.InvalidSessionId:
                        return RemoteStopResult.InvalidSessionId(SessionId);

                    case RemoteStopResultType.Offline:
                        return RemoteStopResult.Offline(SessionId);

                    case RemoteStopResultType.OutOfService:
                        return RemoteStopResult.OutOfService(SessionId);

                    case RemoteStopResultType.Success:
                        if (result.ChargeDetailRecord != null)
                            return RemoteStopResult.Success(result.ChargeDetailRecord, result.ReservationId, result.ReservationHandling);
                        else
                            return RemoteStopResult.Success(result.SessionId, result.ReservationId, result.ReservationHandling);

                    case RemoteStopResultType.Timeout:
                        return RemoteStopResult.Timeout(SessionId);

                    case RemoteStopResultType.UnknownOperator:
                        return RemoteStopResult.UnknownOperator(SessionId);

                    case RemoteStopResultType.Unspecified:
                        return RemoteStopResult.Unspecified(SessionId);

                }

                return RemoteStopResult.Error(SessionId);

            }
            else
            {

                switch (AdminStatus.Value)
                {

                    default:
                        return RemoteStopResult.OutOfService(SessionId);

                }

            }

        }

        #endregion


        #region SendNewChargingSession

        internal void SendNewChargingSession(DateTime         Timestamp,
                                             Object           Sender,
                                             ChargingSession  ChargingSession)
        {

            OnNewChargingSession?.Invoke(Timestamp, Sender, ChargingSession);

        }

        #endregion

        #region SendNewChargeDetailRecord

        internal void SendNewChargeDetailRecord(DateTime            Timestamp,
                                                Object              Sender,
                                                ChargeDetailRecord  ChargeDetailRecord)
        {

            OnNewChargeDetailRecord?.Invoke(Timestamp, Sender, ChargeDetailRecord);

        }

        #endregion

        #endregion


        #region WhiteLists

        #region GetWhiteList(Name)

        public HashSet<LocalAuthentication> GetWhiteList(String Name)

            => WhiteLists[Name];

        #endregion

        #endregion


        //-- Client-side methods -----------------------------------------

        #region Authenticate(LocalAuthentication)

        public Boolean Authenticate(LocalAuthentication LocalAuthentication)
        {
            return false;
        }

        #endregion



        #region IComparable<VirtualChargingStation> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        public Int32 CompareTo(Object Object)
        {

            if (Object == null)
                throw new ArgumentNullException(nameof(Object), "The given object must not be null!");

            if (!(Object is VirtualChargingStation VirtualChargingStation))
                throw new ArgumentException("The given object is not a virtual charging station!");

            return CompareTo(VirtualChargingStation);

        }

        #endregion

        #region CompareTo(VirtualChargingStation)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="VirtualChargingStation">An virtual charging station to compare with.</param>
        public Int32 CompareTo(VirtualChargingStation VirtualChargingStation)
        {

            if ((Object) VirtualChargingStation == null)
                throw new ArgumentNullException(nameof(VirtualChargingStation),  "The given virtual charging station must not be null!");

            return Id.CompareTo(VirtualChargingStation.Id);

        }

        #endregion

        #endregion

        #region IEquatable<VirtualChargingStation> Members

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

            var VirtualChargingStation = Object as VirtualChargingStation;
            if ((Object) VirtualChargingStation == null)
                return false;

            return Equals(VirtualChargingStation);

        }

        #endregion

        #region Equals(VirtualChargingStation)

        /// <summary>
        /// Compares two virtual charging stations for equality.
        /// </summary>
        /// <param name="VirtualChargingStation">A virtual charging station to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public Boolean Equals(VirtualChargingStation VirtualChargingStation)
        {

            if ((Object) VirtualChargingStation == null)
                return false;

            return Id.Equals(VirtualChargingStation.Id);

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


    }

}
