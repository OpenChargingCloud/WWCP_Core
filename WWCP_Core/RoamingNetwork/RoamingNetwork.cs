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
using System.IO;
using System.Linq;
using System.Threading;
using System.Collections;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Collections.Concurrent;

using org.GraphDefined.Vanaheimr.Illias;
using org.GraphDefined.Vanaheimr.Hermod;
using org.GraphDefined.Vanaheimr.Illias.Votes;
using org.GraphDefined.Vanaheimr.Styx.Arrows;

#endregion

namespace org.GraphDefined.WWCP
{

    /// <summary>
    /// A Electric Vehicle Roaming Network is a service abstraction to allow multiple
    /// independent roaming services to be delivered over the same infrastructure.
    /// This can e.g. be a differentation of service levels (premiun, basic,
    /// discount) or allow a simplified testing (production, qa, featureX, ...)
    /// </summary>
    public class RoamingNetwork : AEMobilityEntity<RoamingNetwork_Id>, IRoamingNetwork, IRoamingNetwork1
    {

        #region Data

        /// <summary>
        /// The default max size of the admin status list.
        /// </summary>
        public const UInt16 DefaultMaxAdminStatusListSize   = 15;

        /// <summary>
        /// The default max size of the status list.
        /// </summary>
        public const UInt16 DefaultMaxStatusListSize        = 15;

        /// <summary>
        /// The request timeout.
        /// </summary>
        public readonly TimeSpan RequestTimeout = TimeSpan.FromSeconds(60);

        #endregion

        #region Properties

        IId ISendAuthorizeStartStop.AuthId => Id;

        IId ISendChargeDetailRecords.Id
            => Id;

        IEnumerable<IId> ISendChargeDetailRecords.Ids
            => Ids.Cast<IId>();

        public Boolean DisableAuthentication            { get; set; }

        public Boolean DisableSendChargeDetailRecords   { get; set; }


        #region Name

        private I18NString _Name;

        /// <summary>
        /// The multi-language name of the roaming network.
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
                SetProperty(ref _Name, value);
            }

        }

        #endregion

        #region Description

        private I18NString _Description;

        /// <summary>
        /// An optional multi-language description of the roaming network.
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
                SetProperty(ref _Description, value);
            }

        }

        #endregion

        #region DataLicense

        private ReactiveSet<DataLicense> _DataLicenses;

        /// <summary>
        /// The license of the roaming network data.
        /// </summary>
        [Mandatory]
        public ReactiveSet<DataLicense> DataLicenses
            => _DataLicenses;

        #endregion


        #region AdminStatus

        /// <summary>
        /// The current admin status.
        /// </summary>
        [Optional]
        public Timestamped<RoamingNetworkAdminStatusTypes> AdminStatus

            => _AdminStatusSchedule.CurrentStatus;

        #endregion

        #region AdminStatusSchedule

        private StatusSchedule<RoamingNetworkAdminStatusTypes> _AdminStatusSchedule;

        /// <summary>
        /// The admin status schedule.
        /// </summary>
        public IEnumerable<Timestamped<RoamingNetworkAdminStatusTypes>> AdminStatusSchedule(UInt64? HistorySize = null)
        {

            if (HistorySize.HasValue)
                return _AdminStatusSchedule.Take(HistorySize);

            return _AdminStatusSchedule;

        }

        #endregion


        #region Status

        /// <summary>
        /// The current status.
        /// </summary>
        [Optional]
        public Timestamped<RoamingNetworkStatusTypes> Status

            => _StatusSchedule.CurrentStatus;

        #endregion

        #region StatusSchedule

        private StatusSchedule<RoamingNetworkStatusTypes> _StatusSchedule;

        /// <summary>
        /// The status schedule.
        /// </summary>
        public IEnumerable<Timestamped<RoamingNetworkStatusTypes>> StatusSchedule(UInt64? HistorySize = null)
        {

            if (HistorySize.HasValue)
                return _StatusSchedule.Take(HistorySize);

            return _StatusSchedule;

        }

        #endregion


        public ChargingReservationsStore  ReservationsStore           { get; }
        public ChargingSessionsStore      SessionsStore               { get; }
        public ChargeDetailRecordsStore   ChargeDetailRecordsStore    { get; }

        /// <summary>
        /// A delegate to sign a charging station.
        /// </summary>
        public ChargingStationSignatureDelegate          ChargingStationSignatureGenerator            { get; }

        /// <summary>
        /// A delegate to sign a charging pool.
        /// </summary>
        public ChargingPoolSignatureDelegate             ChargingPoolSignatureGenerator               { get; }

        /// <summary>
        /// A delegate to sign a charging station operator.
        /// </summary>
        public ChargingStationOperatorSignatureDelegate  ChargingStationOperatorSignatureGenerator    { get; }

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new roaming network having the given unique roaming network identification.
        /// </summary>
        /// <param name="Id">The unique identification of the roaming network.</param>
        /// <param name="Name">The multi-language name of the roaming network.</param>
        /// <param name="Description">An optional multi-language description of the roaming network.</param>
        /// <param name="AdminStatus">The initial admin status of the roaming network.</param>
        /// <param name="Status">The initial status of the roaming network.</param>
        /// <param name="MaxAdminStatusListSize">The maximum number of entries in the admin status history.</param>
        /// <param name="MaxStatusListSize">The maximum number of entries in the status history.</param>
        /// <param name="ChargingStationSignatureGenerator">A delegate to sign a charging station.</param>
        /// <param name="ChargingPoolSignatureGenerator">A delegate to sign a charging pool.</param>
        /// <param name="ChargingStationOperatorSignatureGenerator">A delegate to sign a charging station operator.</param>
        public RoamingNetwork(RoamingNetwork_Id                         Id,
                              I18NString                                Name                                       = null,
                              I18NString                                Description                                = null,
                              RoamingNetworkAdminStatusTypes            AdminStatus                                = RoamingNetworkAdminStatusTypes.Operational,
                              RoamingNetworkStatusTypes                 Status                                     = RoamingNetworkStatusTypes.Available,
                              UInt16                                    MaxAdminStatusListSize                     = DefaultMaxAdminStatusListSize,
                              UInt16                                    MaxStatusListSize                          = DefaultMaxStatusListSize,
                              ChargingStationSignatureDelegate          ChargingStationSignatureGenerator          = null,
                              ChargingPoolSignatureDelegate             ChargingPoolSignatureGenerator             = null,
                              ChargingStationOperatorSignatureDelegate  ChargingStationOperatorSignatureGenerator  = null,
                              Func<RoamingNetwork_Id, String>           ReservationLogFileNameCreator              = null,
                              Func<RoamingNetwork_Id, String>           SessionLogFileNameCreator                  = null,
                              Func<RoamingNetwork_Id, String>           ChargeDetailRecordLogFileNameCreator       = null)

            : base(Id)

        {

            #region Init data and properties

            this._Name                                              = Name        ?? new I18NString();
            this._Description                                       = Description ?? new I18NString();
            this._DataLicenses                                      = new ReactiveSet<DataLicense>();

            this._ChargingStationOperators                          = new EntityHashSet<RoamingNetwork, ChargingStationOperator_Id, ChargingStationOperator>(this);
            this._ParkingOperators                                  = new EntityHashSet<RoamingNetwork, ParkingOperator_Id,         ParkingOperator>        (this);
            this._eMobilityProviders                                = new EntityHashSet<RoamingNetwork, eMobilityProvider_Id,       eMobilityProvider>      (this);
            this._SmartCities                                       = new EntityHashSet<RoamingNetwork, SmartCity_Id,               SmartCityProxy>              (this);
            this._NavigationProviders                               = new EntityHashSet<RoamingNetwork, NavigationProvider_Id,      NavigationProvider>     (this);
            this._GridOperators                                     = new EntityHashSet<RoamingNetwork, GridOperator_Id,            GridOperator>           (this);

            this._ChargingStationOperatorRoamingProviders           = new ConcurrentDictionary<CSORoamingProvider_Id, ICSORoamingProvider>();
            this._EMPRoamingProviders                               = new ConcurrentDictionary<EMPRoamingProvider_Id, IEMPRoamingProvider>();

            this._eMobilityRoamingServices                          = new ConcurrentDictionary<UInt32, IEMPRoamingProvider>();
            //this._PushEVSEDataToOperatorRoamingServices             = new ConcurrentDictionary<UInt32, IPushData>();
            //this._PushEVSEStatusToOperatorRoamingServices           = new ConcurrentDictionary<UInt32, IPushStatus>();


            this.ReservationsStore                                  = new ChargingReservationsStore(this,
                                                                                                    ReservationLogFileNameCreator);

            this.SessionsStore                                      = new ChargingSessionsStore    (this,
                                                                                                    SessionLogFileNameCreator,
                                                                                                    DisableNetworkSync: true);

            this.ChargeDetailRecordsStore                           = new ChargeDetailRecordsStore (this,
                                                                                                    ChargeDetailRecordLogFileNameCreator);


            this._AdminStatusSchedule                               = new StatusSchedule<RoamingNetworkAdminStatusTypes>(MaxAdminStatusListSize);
            this._AdminStatusSchedule.Insert(AdminStatus);

            this._StatusSchedule                                    = new StatusSchedule<RoamingNetworkStatusTypes>(MaxStatusListSize);
            this._StatusSchedule.Insert(Status);

            this.ChargingStationSignatureGenerator                  = ChargingStationSignatureGenerator;
            this.ChargingPoolSignatureGenerator                     = ChargingPoolSignatureGenerator;
            this.ChargingStationOperatorSignatureGenerator          = ChargingStationOperatorSignatureGenerator;

            #endregion

            #region Init events

            // RoamingNetwork events

            this.CPORoamingProviderAddition  = new VotingNotificator<RoamingNetwork, ICSORoamingProvider, Boolean>(() => new VetoVote(), true);
            this.CPORoamingProviderRemoval   = new VotingNotificator<RoamingNetwork, ICSORoamingProvider, Boolean>(() => new VetoVote(), true);

            this.EMPRoamingProviderAddition  = new VotingNotificator<RoamingNetwork, IEMPRoamingProvider, Boolean>(() => new VetoVote(), true);
            this.EMPRoamingProviderRemoval   = new VotingNotificator<RoamingNetwork, IEMPRoamingProvider, Boolean>(() => new VetoVote(), true);

            // cso events
            this.ChargingPoolAddition        = new VotingNotificator<DateTime, ChargingStationOperator, ChargingPool, Boolean>(() => new VetoVote(), true);
            this.ChargingPoolRemoval         = new VotingNotificator<DateTime, ChargingStationOperator, ChargingPool, Boolean>(() => new VetoVote(), true);

            // ChargingPool events
            this.ChargingStationAddition     = new VotingNotificator<DateTime, ChargingPool, ChargingStation, Boolean>(() => new VetoVote(), true);
            this.ChargingStationRemoval      = new VotingNotificator<DateTime, ChargingPool, ChargingStation, Boolean>(() => new VetoVote(), true);

            // ChargingStation events
            this.EVSEAddition                = new VotingNotificator<DateTime, ChargingStation, EVSE, Boolean>(() => new VetoVote(), true);
            this.EVSERemoval                 = new VotingNotificator<DateTime, ChargingStation, EVSE, Boolean>(() => new VetoVote(), true);

            // EVSE events

            #endregion

            this.OnPropertyChanged += UpdateData;

            SessionsStore.ReloadData();

        }

        #endregion


        private readonly PriorityList<ISendData>                 _ISendData                        = new PriorityList<ISendData>();
        private readonly PriorityList<ISendAdminStatus>          _ISendAdminStatus                 = new PriorityList<ISendAdminStatus>();
        private readonly PriorityList<ISendStatus>               _ISendStatus                      = new PriorityList<ISendStatus>();
        private readonly PriorityList<ISendAuthorizeStartStop>   _ISend2RemoteAuthorizeStartStop   = new PriorityList<ISendAuthorizeStartStop>();
        private readonly PriorityList<ISendChargeDetailRecords>  _IRemoteSendChargeDetailRecord    = new PriorityList<ISendChargeDetailRecords>();


        #region Data/(Admin-)Status management

        #region OnData/(Admin)StatusChanged

        /// <summary>
        /// An event fired whenever the static data changed.
        /// </summary>
        public event OnRoamingNetworkDataChangedDelegate         OnDataChanged;

        /// <summary>
        /// An event fired whenever the dynamic status changed.
        /// </summary>
        public event OnRoamingNetworkStatusChangedDelegate       OnStatusChanged;

        /// <summary>
        /// An event fired whenever the admin status changed.
        /// </summary>
        public event OnRoamingNetworkAdminStatusChangedDelegate  OnAggregatedAdminStatusChanged;

        #endregion


        #region (internal) UpdateData(Timestamp, EventTrackingId, Sender, PropertyName, OldValue, NewValue)

        /// <summary>
        /// Update the static data of the roaming network.
        /// </summary>
        /// <param name="Timestamp">The timestamp when this change was detected.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="Sender">The changed roaming network.</param>
        /// <param name="PropertyName">The name of the changed property.</param>
        /// <param name="OldValue">The old value of the changed property.</param>
        /// <param name="NewValue">The new value of the changed property.</param>
        internal async Task UpdateData(DateTime          Timestamp,
                                       EventTracking_Id  EventTrackingId,
                                       Object            Sender,
                                       String            PropertyName,
                                       Object            OldValue,
                                       Object            NewValue)
        {

            var OnDataChangedLocal = OnDataChanged;
            if (OnDataChangedLocal != null)
                await OnDataChangedLocal(Timestamp,
                                         EventTrackingId,
                                         Sender as RoamingNetwork,
                                         PropertyName,
                                         OldValue,
                                         NewValue);

        }

        #endregion

        #endregion

        #region Charging Station Operators...

        #region ChargingStationOperators

        private readonly EntityHashSet<RoamingNetwork, ChargingStationOperator_Id, ChargingStationOperator> _ChargingStationOperators;

        /// <summary>
        /// Return all charging station operators registered within this roaming network.
        /// </summary>
        public IEnumerable<ChargingStationOperator> ChargingStationOperators

            => _ChargingStationOperators;

        #endregion

        #region ChargingStationOperatorIds

        /// <summary>
        /// Return all charging station operator identifications registered within this roaming network.
        /// </summary>
        public IEnumerable<ChargingStationOperator_Id> ChargingStationOperatorIds

            => _ChargingStationOperators.SafeSelect(cso => cso.Id);

        #endregion

        #region ChargingStationOperatorAdminStatus

        /// <summary>
        /// Return the admin status of all charging station operators registered within this roaming network.
        /// </summary>
        public IEnumerable<KeyValuePair<ChargingStationOperator_Id, IEnumerable<Timestamped<ChargingStationOperatorAdminStatusTypes>>>> ChargingStationOperatorAdminStatus

            => _ChargingStationOperators.
                   SafeSelect(cso => new KeyValuePair<ChargingStationOperator_Id, IEnumerable<Timestamped<ChargingStationOperatorAdminStatusTypes>>>(cso.Id,
                                                                                                                                                     cso.AdminStatusSchedule()));

        #endregion

        #region ChargingStationOperatorStatus

        /// <summary>
        /// Return the status of all charging station operators registered within this roaming network.
        /// </summary>
        public IEnumerable<KeyValuePair<ChargingStationOperator_Id, IEnumerable<Timestamped<ChargingStationOperatorStatusTypes>>>> ChargingStationOperatorStatus

            => _ChargingStationOperators.
                   SafeSelect(cso => new KeyValuePair<ChargingStationOperator_Id, IEnumerable<Timestamped<ChargingStationOperatorStatusTypes>>>(cso.Id,
                                                                                                                                                cso.StatusSchedule()));

        #endregion


        #region ChargingStationOperatorAddition

        /// <summary>
        /// Called whenever a charging station operator will be or was added.
        /// </summary>
        public IVotingSender<DateTime, RoamingNetwork, ChargingStationOperator, Boolean> OnChargingStationOperatorAddition
            => _ChargingStationOperators.OnAddition;

        #endregion

        #region ChargingStationOperatorRemoval

        /// <summary>
        /// Called whenever charging station operator will be or was removed.
        /// </summary>
        public IVotingSender<DateTime, RoamingNetwork, ChargingStationOperator, Boolean> OnChargingStationOperatorRemoval
            => _ChargingStationOperators.OnRemoval;

        #endregion


        #region CreateChargingStationOperator(ChargingStationOperatorId, Name = null, Description = null, Configurator = null, OnSuccess = null, OnError = null)

        public ChargingStationOperator

            CreateChargingStationOperator(ChargingStationOperator_Id                          ChargingStationOperatorId,
                                          I18NString                                          Name                                  = null,
                                          I18NString                                          Description                           = null,
                                          Action<ChargingStationOperator>                     Configurator                          = null,
                                          RemoteChargingStationOperatorCreatorDelegate        RemoteChargingStationOperatorCreator  = null,
                                          ChargingStationOperatorAdminStatusTypes             AdminStatus                           = ChargingStationOperatorAdminStatusTypes.Operational,
                                          ChargingStationOperatorStatusTypes                  Status                                = ChargingStationOperatorStatusTypes.Available,
                                          Action<ChargingStationOperator>                     OnSuccess                             = null,
                                          Action<RoamingNetwork, ChargingStationOperator_Id>  OnError                               = null)

                => CreateChargingStationOperator(new ChargingStationOperator_Id[] { ChargingStationOperatorId },
                                                 Name,
                                                 Description,
                                                 Configurator,
                                                 RemoteChargingStationOperatorCreator,
                                                 AdminStatus,
                                                 Status,
                                                 OnSuccess,
                                                 OnError);


        /// <summary>
        /// Create and register a new charging station operator having the given
        /// unique charging station operator identification.
        /// </summary>
        /// <param name="ChargingStationOperatorIds">The unique identification of the new charging station operator.</param>
        /// <param name="Name">The offical (multi-language) name of the charging station operator.</param>
        /// <param name="Description">An optional (multi-language) description of the charging station operator.</param>
        /// <param name="Configurator">An optional delegate to configure the new charging station operator before its successful creation.</param>
        /// <param name="OnSuccess">An optional delegate to configure the new charging station operator after its successful creation.</param>
        /// <param name="OnError">An optional delegate to be called whenever the creation of the charging station operator failed.</param>
        public ChargingStationOperator

            CreateChargingStationOperator(IEnumerable<ChargingStationOperator_Id>             ChargingStationOperatorIds,
                                          I18NString                                          Name                                   = null,
                                          I18NString                                          Description                            = null,
                                          Action<ChargingStationOperator>                     Configurator                           = null,
                                          RemoteChargingStationOperatorCreatorDelegate        RemoteChargingStationOperatorCreator   = null,
                                          ChargingStationOperatorAdminStatusTypes             AdminStatus                            = ChargingStationOperatorAdminStatusTypes.Operational,
                                          ChargingStationOperatorStatusTypes                  Status                                 = ChargingStationOperatorStatusTypes.Available,
                                          Action<ChargingStationOperator>                     OnSuccess                              = null,
                                          Action<RoamingNetwork, ChargingStationOperator_Id>  OnError                                = null)

        {

            #region Initial checks

            if (ChargingStationOperatorIds == null)
                throw new ArgumentNullException(nameof(ChargingStationOperatorIds),  "The given charging station operator identification must not be null!");

            #endregion

            lock (_ChargingStationOperators)
            {

                foreach (var ChargingStationOperatorId in ChargingStationOperatorIds)
                {

                    if (_ChargingStationOperators.ContainsId(ChargingStationOperatorId))
                        throw new ChargingStationOperatorAlreadyExists(this, ChargingStationOperatorId, Name);

                }


                var _ChargingStationOperator = new ChargingStationOperator(ChargingStationOperatorIds,
                                                                           this,
                                                                           Configurator,
                                                                           RemoteChargingStationOperatorCreator,
                                                                           Name,
                                                                           Description,
                                                                           AdminStatus,
                                                                           Status);


                if (_ChargingStationOperators.TryAdd(_ChargingStationOperator, OnSuccess))
                {

                    _ChargingStationOperator.OnDataChanged                                 += UpdateCSOData;
                    _ChargingStationOperator.OnStatusChanged                               += UpdateCSOStatus;
                    _ChargingStationOperator.OnAdminStatusChanged                          += UpdateCSOAdminStatus;

                    _ChargingStationOperator.OnChargingPoolAddition.   OnVoting            += (timestamp, cso, pool, vote)      => ChargingPoolAddition.   SendVoting      (timestamp, cso, pool, vote);
                    _ChargingStationOperator.OnChargingPoolAddition.   OnNotification      += (timestamp, cso, pool)            => ChargingPoolAddition.   SendNotification(timestamp, cso, pool);
                    _ChargingStationOperator.OnChargingPoolDataChanged                     += UpdateChargingPoolData;
                    _ChargingStationOperator.OnChargingPoolStatusChanged                   += UpdateChargingPoolStatus;
                    _ChargingStationOperator.OnChargingPoolAdminStatusChanged              += UpdateChargingPoolAdminStatus;
                    _ChargingStationOperator.OnChargingPoolRemoval.    OnVoting            += (timestamp, cso, pool, vote)      => ChargingPoolRemoval.    SendVoting      (timestamp, cso, pool, vote);
                    _ChargingStationOperator.OnChargingPoolRemoval.    OnNotification      += (timestamp, cso, pool)            => ChargingPoolRemoval.    SendNotification(timestamp, cso, pool);

                    _ChargingStationOperator.OnChargingStationAddition.OnVoting            += (timestamp, pool, station, vote)  => ChargingStationAddition.SendVoting      (timestamp, pool, station, vote);
                    _ChargingStationOperator.OnChargingStationAddition.OnNotification      += (timestamp, pool, station)        => ChargingStationAddition.SendNotification(timestamp, pool, station);
                    _ChargingStationOperator.OnChargingStationDataChanged                  += UpdateChargingStationData;
                    _ChargingStationOperator.OnChargingStationStatusChanged                += UpdateChargingStationStatus;
                    _ChargingStationOperator.OnChargingStationAdminStatusChanged           += UpdateChargingStationAdminStatus;
                    _ChargingStationOperator.OnChargingStationRemoval. OnVoting            += (timestamp, pool, station, vote)  => ChargingStationRemoval. SendVoting      (timestamp, pool, station, vote);
                    _ChargingStationOperator.OnChargingStationRemoval. OnNotification      += (timestamp, pool, station)        => ChargingStationRemoval. SendNotification(timestamp, pool, station);

                    _ChargingStationOperator.OnEVSEAddition.           OnVoting            += (timestamp, station, evse, vote)  => EVSEAddition.           SendVoting      (timestamp, station, evse, vote);
                    _ChargingStationOperator.OnEVSEAddition.           OnNotification      += (timestamp, station, evse)        => EVSEAddition.           SendNotification(timestamp, station, evse);
                    _ChargingStationOperator.EVSEAddition.OnNotification                   += SendEVSEAdded;
                    _ChargingStationOperator.OnEVSEDataChanged                             += UpdateEVSEData;
                    _ChargingStationOperator.OnEVSEStatusChanged                           += UpdateEVSEStatus;
                    _ChargingStationOperator.OnEVSEAdminStatusChanged                      += UpdateEVSEAdminStatus;
                    _ChargingStationOperator.OnEVSERemoval.            OnVoting            += (timestamp, station, evse, vote)  => EVSERemoval.            SendVoting      (timestamp, station, evse, vote);
                    _ChargingStationOperator.OnEVSERemoval.            OnNotification      += (timestamp, station, evse)        => EVSERemoval.            SendNotification(timestamp, station, evse);


                    _ChargingStationOperator.OnNewReservation                              += SendNewReservation;
                    _ChargingStationOperator.OnReservationCanceled                         += SendReservationCanceled;
                    _ChargingStationOperator.OnNewChargingSession                          += SendNewChargingSession;
                    _ChargingStationOperator.OnNewChargeDetailRecord                       += SendNewChargeDetailRecord;

                    return _ChargingStationOperator;

                }

                //ToDo: Throw a more usefull exception!
                throw new ChargingStationOperatorAlreadyExists(this,
                                                               ChargingStationOperatorIds.FirstOrDefault(),
                                                               Name);

            }

        }

        #endregion

        #region ContainsChargingStationOperator(ChargingStationOperator)

        /// <summary>
        /// Check if the given ChargingStationOperator is already present within the roaming network.
        /// </summary>
        /// <param name="ChargingStationOperator">An Charging Station Operator.</param>
        public Boolean ContainsChargingStationOperator(ChargingStationOperator ChargingStationOperator)

            => _ChargingStationOperators.ContainsId(ChargingStationOperator.Id);

        #endregion

        #region ContainsChargingStationOperator(ChargingStationOperatorId)

        /// <summary>
        /// Check if the given ChargingStationOperator identification is already present within the roaming network.
        /// </summary>
        /// <param name="ChargingStationOperatorId">The unique identification of the Charging Station Operator.</param>
        public Boolean ContainsChargingStationOperator(ChargingStationOperator_Id ChargingStationOperatorId)

            => _ChargingStationOperators.ContainsId(ChargingStationOperatorId);

        #endregion

        #region GetChargingStationOperatorById(ChargingStationOperatorId)

        public ChargingStationOperator GetChargingStationOperatorById(ChargingStationOperator_Id ChargingStationOperatorId)

             => _ChargingStationOperators.GetById(ChargingStationOperatorId);

        #endregion

        #region TryGetChargingStationOperatorById(ChargingStationOperatorId, out ChargingStationOperator)

        public Boolean TryGetChargingStationOperatorById(ChargingStationOperator_Id ChargingStationOperatorId, out ChargingStationOperator ChargingStationOperator)

            => _ChargingStationOperators.TryGet(ChargingStationOperatorId, out ChargingStationOperator);

        public Boolean TryGetChargingStationOperatorById(ChargingStationOperator_Id? ChargingStationOperatorId, out ChargingStationOperator ChargingStationOperator)
        {

            if (!ChargingStationOperatorId.HasValue)
            {
                ChargingStationOperator = null;
                return false;
            }

            return _ChargingStationOperators.TryGet(ChargingStationOperatorId.Value, out ChargingStationOperator);

        }

        #endregion

        #region RemoveChargingStationOperator(ChargingStationOperatorId)

        public ChargingStationOperator RemoveChargingStationOperator(ChargingStationOperator_Id ChargingStationOperatorId)
        {

            ChargingStationOperator _ChargingStationOperator = null;

            if (_ChargingStationOperators.TryRemove(ChargingStationOperatorId, out _ChargingStationOperator))
                return _ChargingStationOperator;

            return null;

        }

        #endregion

        #region TryRemoveChargingStationOperator(RemoveChargingStationOperatorId, out RemoveChargingStationOperator)

        public Boolean TryRemoveChargingStationOperator(ChargingStationOperator_Id ChargingStationOperatorId, out ChargingStationOperator ChargingStationOperator)

            => _ChargingStationOperators.TryRemove(ChargingStationOperatorId, out ChargingStationOperator);

        #endregion


        #region OnChargingStationOperatorData/(Admin)StatusChanged

        /// <summary>
        /// An event fired whenever the static data of any subordinated Charging Station Operator changed.
        /// </summary>
        public event OnChargingStationOperatorDataChangedDelegate         OnChargingStationOperatorDataChanged;

        /// <summary>
        /// An event fired whenever the aggregated dynamic status of any subordinated Charging Station Operator changed.
        /// </summary>
        public event OnChargingStationOperatorStatusChangedDelegate       OnChargingStationOperatorStatusChanged;

        /// <summary>
        /// An event fired whenever the aggregated admin status of any subordinated Charging Station Operator changed.
        /// </summary>
        public event OnChargingStationOperatorAdminStatusChangedDelegate  OnChargingStationOperatorAdminStatusChanged;

        #endregion


        #region (internal) UpdateCSOData(Timestamp, cso, OldStatus, NewStatus)

        /// <summary>
        /// Update the data of an evse operator.
        /// </summary>
        /// <param name="Timestamp">The timestamp when this change was detected.</param>
        /// <param name="cso">The changed evse operator.</param>
        /// <param name="PropertyName">The name of the changed property.</param>
        /// <param name="OldValue">The old value of the changed property.</param>
        /// <param name="NewValue">The new value of the changed property.</param>
        internal async Task UpdateCSOData(DateTime                 Timestamp,
                                          ChargingStationOperator  cso,
                                          String                   PropertyName,
                                          Object                   OldValue,
                                          Object                   NewValue)
        {

            var OnChargingStationOperatorDataChangedLocal = OnChargingStationOperatorDataChanged;
            if (OnChargingStationOperatorDataChangedLocal != null)
                await OnChargingStationOperatorDataChangedLocal(Timestamp, cso, PropertyName, OldValue, NewValue);

        }

        #endregion

        #region (internal) UpdateCSOStatus(Timestamp, cso, OldStatus, NewStatus)

        /// <summary>
        /// Update the current Charging Station Operator status.
        /// </summary>
        /// <param name="Timestamp">The timestamp when this change was detected.</param>
        /// <param name="cso">The updated Charging Station Operator.</param>
        /// <param name="OldStatus">The old aggreagted Charging Station Operator status.</param>
        /// <param name="NewStatus">The new aggreagted Charging Station Operator status.</param>
        internal async Task UpdateCSOStatus(DateTime                             Timestamp,
                                            ChargingStationOperator                         cso,
                                            Timestamped<ChargingStationOperatorStatusTypes>  OldStatus,
                                            Timestamped<ChargingStationOperatorStatusTypes>  NewStatus)
        {

            // Send Charging Station Operator status change upstream
            var OnChargingStationOperatorStatusChangedLocal = OnChargingStationOperatorStatusChanged;
            if (OnChargingStationOperatorStatusChangedLocal != null)
                await OnChargingStationOperatorStatusChangedLocal(Timestamp, cso, OldStatus, NewStatus);


            // Calculate new aggregated roaming network status and send upstream
            //if (StatusAggregationDelegate != null)
            //{

            //    var NewAggregatedStatus = new Timestamped<RoamingNetworkStatusType>(StatusAggregationDelegate(new csoStatusReport(_ChargingStationOperators.Values)));

            //    if (NewAggregatedStatus.Value != _StatusHistory.Peek().Value)
            //    {

            //        var OldAggregatedStatus = _StatusHistory.Peek();

            //        _StatusHistory.Push(NewAggregatedStatus);

            //        OnStatusChanged?.Invoke(Timestamp,
            //                                this,
            //                                OldAggregatedStatus,
            //                                NewAggregatedStatus);

            //    }

            //}

        }

        #endregion

        #region (internal) UpdateCSOAdminStatus(Timestamp, cso, OldStatus, NewStatus)

        /// <summary>
        /// Update the current Charging Station Operator admin status.
        /// </summary>
        /// <param name="Timestamp">The timestamp when this change was detected.</param>
        /// <param name="cso">The updated Charging Station Operator.</param>
        /// <param name="OldStatus">The old aggreagted Charging Station Operator status.</param>
        /// <param name="NewStatus">The new aggreagted Charging Station Operator status.</param>
        internal async Task UpdateCSOAdminStatus(DateTime                                             Timestamp,
                                                 ChargingStationOperator                              cso,
                                                 Timestamped<ChargingStationOperatorAdminStatusTypes>  OldStatus,
                                                 Timestamped<ChargingStationOperatorAdminStatusTypes>  NewStatus)
        {

            // Send Charging Station Operator admin status change upstream
            var OnChargingStationOperatorAdminStatusChangedLocal = OnChargingStationOperatorAdminStatusChanged;
            if (OnChargingStationOperatorAdminStatusChangedLocal != null)
                await OnChargingStationOperatorAdminStatusChangedLocal(Timestamp, cso, OldStatus, NewStatus);


            // Calculate new aggregated roaming network status and send upstream
            //if (AdminStatusAggregationDelegate != null)
            //{

            //    var NewAggregatedStatus = new Timestamped<RoamingNetworkAdminStatusType>(AdminStatusAggregationDelegate(new csoAdminStatusReport(_ChargingStationOperators.Values)));

            //    if (NewAggregatedStatus.Value != _AdminStatusHistory.Peek().Value)
            //    {

            //        var OldAggregatedStatus = _AdminStatusHistory.Peek();

            //        _AdminStatusHistory.Push(NewAggregatedStatus);

            //        var OnAggregatedAdminStatusChangedLocal = OnAggregatedAdminStatusChanged;
            //        if (OnAggregatedAdminStatusChangedLocal != null)
            //            OnAggregatedAdminStatusChangedLocal(Timestamp, this, OldAggregatedStatus, NewAggregatedStatus);

            //    }

            //}

        }

        #endregion

        #endregion

        #region Parking Operators...

        #region ParkingOperators

        private readonly EntityHashSet<RoamingNetwork, ParkingOperator_Id, ParkingOperator> _ParkingOperators;

        /// <summary>
        /// Return all parking operators registered within this roaming network.
        /// </summary>
        public IEnumerable<ParkingOperator> ParkingOperators

            => _ParkingOperators;

        #endregion

        #region ParkingOperatorAdminStatus

        /// <summary>
        /// Return the admin status of all parking operators registered within this roaming network.
        /// </summary>
        public IEnumerable<KeyValuePair<ParkingOperator_Id, IEnumerable<Timestamped<ParkingOperatorAdminStatusType>>>> ParkingOperatorAdminStatus

            => _ParkingOperators.
                   Select(pop => new KeyValuePair<ParkingOperator_Id, IEnumerable<Timestamped<ParkingOperatorAdminStatusType>>>(pop.Id,
                                                                                                                                pop.AdminStatusSchedule));

        #endregion

        #region ParkingOperatorStatus

        /// <summary>
        /// Return the status of all parking operators registered within this roaming network.
        /// </summary>
        public IEnumerable<KeyValuePair<ParkingOperator_Id, IEnumerable<Timestamped<ParkingOperatorStatusType>>>> ParkingOperatorStatus

            => _ParkingOperators.
                   Select(pop => new KeyValuePair<ParkingOperator_Id, IEnumerable<Timestamped<ParkingOperatorStatusType>>>(pop.Id,
                                                                                                                           pop.StatusSchedule));

        #endregion


        #region ParkingOperatorAddition

        /// <summary>
        /// Called whenever a parking operator will be or was added.
        /// </summary>
        public IVotingSender<DateTime, RoamingNetwork, ParkingOperator, Boolean> OnParkingOperatorAddition
            => _ParkingOperators.OnAddition;

        #endregion

        #region ParkingOperatorRemoval

        /// <summary>
        /// Called whenever a parking operator will be or was removed.
        /// </summary>
        public IVotingSender<DateTime, RoamingNetwork, ParkingOperator, Boolean> OnParkingOperatorRemoval
            => _ParkingOperators.OnAddition;

        #endregion


        #region CreateNewParkingOperator(ParkingOperatorId, Name = null, Description = null, Configurator = null, OnSuccess = null, OnError = null)

        /// <summary>
        /// Create and register a new parking operator having the given
        /// unique parking operator identification.
        /// </summary>
        /// <param name="ParkingOperatorId">The unique identification of the new parking operator.</param>
        /// <param name="Name">The offical (multi-language) name of the parking operator.</param>
        /// <param name="Description">An optional (multi-language) description of the parking operator.</param>
        /// <param name="Configurator">An optional delegate to configure the new parking operator before its successful creation.</param>
        /// <param name="OnSuccess">An optional delegate to configure the new parking operator after its successful creation.</param>
        /// <param name="OnError">An optional delegate to be called whenever the creation of the parking operator failed.</param>
        public ParkingOperator

            CreateNewParkingOperator(ParkingOperator_Id                          ParkingOperatorId,
                                     I18NString                                  Name                          = null,
                                     I18NString                                  Description                   = null,
                                     Action<ParkingOperator>                     Configurator                  = null,
                                     RemoteParkingOperatorCreatorDelegate        RemoteParkingOperatorCreator  = null,
                                     ParkingOperatorAdminStatusType              AdminStatus                   = ParkingOperatorAdminStatusType.Operational,
                                     ParkingOperatorStatusType                   Status                        = ParkingOperatorStatusType.Available,
                                     Action<ParkingOperator>                     OnSuccess                     = null,
                                     Action<RoamingNetwork, ParkingOperator_Id>  OnError                       = null)

        {

            #region Initial checks

            if (ParkingOperatorId == null)
                throw new ArgumentNullException(nameof(ParkingOperatorId),  "The given parking operator identification must not be null!");

            #endregion

            var _ParkingOperator = new ParkingOperator(ParkingOperatorId,
                                                       this,
                                                       Configurator,
                                                       RemoteParkingOperatorCreator,
                                                       Name,
                                                       Description,
                                                       AdminStatus,
                                                       Status);


            if (_ParkingOperators.TryAdd(_ParkingOperator, OnSuccess))
            {

                _ParkingOperator.OnDataChanged                                 += UpdatecsoData;
                _ParkingOperator.OnStatusChanged                               += UpdateStatus;
                _ParkingOperator.OnAdminStatusChanged                          += UpdateAdminStatus;

                //_ParkingOperator.OnChargingPoolDataChanged                     += UpdateChargingPoolData;
                //_ParkingOperator.OnChargingPoolStatusChanged                   += UpdateChargingPoolStatus;
                //_ParkingOperator.OnChargingPoolAdminStatusChanged              += UpdateChargingPoolAdminStatus;

                //_ParkingOperator.OnDataChanged                  += UpdateParkingData;
                //_ParkingOperator.OnStatusChanged                += UpdateParkingStatus;
                //_ParkingOperator.OnAdminStatusChanged           += UpdateParkingAdminStatus;

                ////_cso.EVSEAddition.OnVoting                         += SendEVSEAdding;
                //_ParkingOperator.EVSEAddition.OnNotification                   += SendEVSEAdded;
                ////_cso.EVSERemoval.OnVoting                          += SendEVSERemoving;
                //_ParkingOperator.EVSERemoval.OnNotification                    += SendEVSERemoved;
                //_ParkingOperator.OnEVSEDataChanged                             += UpdateEVSEData;
                //_ParkingOperator.OnEVSEStatusChanged                           += UpdateEVSEStatus;
                //_ParkingOperator.OnEVSEAdminStatusChanged                      += UpdateEVSEAdminStatus;


                //_ParkingOperator.OnNewReservation                              += SendNewReservation;
                //_ParkingOperator.OnCancelReservationResponse                        += SendOnCancelReservationResponse;
                //_ParkingOperator.OnNewChargingSession                          += SendNewChargingSession;
                //_ParkingOperator.OnNewChargeDetailRecord                       += SendNewChargeDetailRecord;

                return _ParkingOperator;

            }

            throw new ParkingOperatorAlreadyExists(this, ParkingOperatorId);

        }

        #endregion

        #region ContainsParkingOperator(ParkingOperator)

        /// <summary>
        /// Check if the given ParkingOperator is already present within the roaming network.
        /// </summary>
        /// <param name="ParkingOperator">An parking Operator.</param>
        public Boolean ContainsParkingOperator(ParkingOperator ParkingOperator)

            => _ParkingOperators.ContainsId(ParkingOperator.Id);

        #endregion

        #region ContainsParkingOperator(ParkingOperatorId)

        /// <summary>
        /// Check if the given ParkingOperator identification is already present within the roaming network.
        /// </summary>
        /// <param name="ParkingOperatorId">The unique identification of the parking Operator.</param>
        public Boolean ContainsParkingOperator(ParkingOperator_Id ParkingOperatorId)

            => _ParkingOperators.ContainsId(ParkingOperatorId);

        #endregion

        #region GetParkingOperatorById(ParkingOperatorId)

        public ParkingOperator GetParkingOperatorById(ParkingOperator_Id ParkingOperatorId)

            => _ParkingOperators.GetById(ParkingOperatorId);

        #endregion

        #region TryGetParkingOperatorById(ParkingOperatorId, out ParkingOperator)

        public Boolean TryGetParkingOperatorById(ParkingOperator_Id ParkingOperatorId, out ParkingOperator ParkingOperator)

            => _ParkingOperators.TryGet(ParkingOperatorId, out ParkingOperator);

        #endregion

        #region RemoveParkingOperator(ParkingOperatorId)

        public ParkingOperator RemoveParkingOperator(ParkingOperator_Id ParkingOperatorId)
        {

            ParkingOperator _ParkingOperator = null;

            if (_ParkingOperators.TryRemove(ParkingOperatorId, out _ParkingOperator))
                return _ParkingOperator;

            return null;

        }

        #endregion

        #region TryRemoveParkingOperator(RemoveParkingOperatorId, out RemoveParkingOperator)

        public Boolean TryRemoveParkingOperator(ParkingOperator_Id ParkingOperatorId, out ParkingOperator ParkingOperator)

            => _ParkingOperators.TryRemove(ParkingOperatorId, out ParkingOperator);

        #endregion


        #region OnParkingOperatorData/(Admin)StatusChanged

        /// <summary>
        /// An event fired whenever the static data of any subordinated parking Operator changed.
        /// </summary>
        public event OnParkingOperatorDataChangedDelegate         OnParkingOperatorDataChanged;

        /// <summary>
        /// An event fired whenever the aggregated dynamic status of any subordinated parking Operator changed.
        /// </summary>
        public event OnParkingOperatorStatusChangedDelegate       OnParkingOperatorStatusChanged;

        /// <summary>
        /// An event fired whenever the aggregated admin status of any subordinated parking Operator changed.
        /// </summary>
        public event OnParkingOperatorAdminStatusChangedDelegate  OnParkingOperatorAdminStatusChanged;

        #endregion


        #region (internal) UpdatecsoData(Timestamp, cso, OldStatus, NewStatus)

        /// <summary>
        /// Update the data of an evse operator.
        /// </summary>
        /// <param name="Timestamp">The timestamp when this change was detected.</param>
        /// <param name="cso">The changed evse operator.</param>
        /// <param name="PropertyName">The name of the changed property.</param>
        /// <param name="OldValue">The old value of the changed property.</param>
        /// <param name="NewValue">The new value of the changed property.</param>
        internal async Task UpdatecsoData(DateTime      Timestamp,
                                                   ParkingOperator  cso,
                                                   String        PropertyName,
                                                   Object        OldValue,
                                                   Object        NewValue)
        {

            var OnParkingOperatorDataChangedLocal = OnParkingOperatorDataChanged;
            if (OnParkingOperatorDataChangedLocal != null)
                await OnParkingOperatorDataChangedLocal(Timestamp, cso, PropertyName, OldValue, NewValue);

        }

        #endregion

        #region (internal) UpdateStatus(Timestamp, cso, OldStatus, NewStatus)

        /// <summary>
        /// Update the current parking Operator status.
        /// </summary>
        /// <param name="Timestamp">The timestamp when this change was detected.</param>
        /// <param name="cso">The updated parking Operator.</param>
        /// <param name="OldStatus">The old aggreagted parking Operator status.</param>
        /// <param name="NewStatus">The new aggreagted parking Operator status.</param>
        internal async Task UpdateStatus(DateTime                             Timestamp,
                                         ParkingOperator                         cso,
                                         Timestamped<ParkingOperatorStatusType>  OldStatus,
                                         Timestamped<ParkingOperatorStatusType>  NewStatus)
        {

            // Send parking Operator status change upstream
            var OnParkingOperatorStatusChangedLocal = OnParkingOperatorStatusChanged;
            if (OnParkingOperatorStatusChangedLocal != null)
                await OnParkingOperatorStatusChangedLocal(Timestamp, cso, OldStatus, NewStatus);


            // Calculate new aggregated roaming network status and send upstream
            //if (StatusAggregationDelegate != null)
            //{

            //    var NewAggregatedStatus = new Timestamped<RoamingNetworkStatusType>(StatusAggregationDelegate(new csoStatusReport(_ParkingOperators.Values)));

            //    if (NewAggregatedStatus.Value != _StatusHistory.Peek().Value)
            //    {

            //        var OldAggregatedStatus = _StatusHistory.Peek();

            //        _StatusHistory.Push(NewAggregatedStatus);

            //        OnStatusChanged?.Invoke(Timestamp,
            //                                this,
            //                                OldAggregatedStatus,
            //                                NewAggregatedStatus);

            //    }

            //}

        }

        #endregion

        #region (internal) UpdateAdminStatus(Timestamp, cso, OldStatus, NewStatus)

        /// <summary>
        /// Update the current parking Operator admin status.
        /// </summary>
        /// <param name="Timestamp">The timestamp when this change was detected.</param>
        /// <param name="cso">The updated parking Operator.</param>
        /// <param name="OldStatus">The old aggreagted parking Operator status.</param>
        /// <param name="NewStatus">The new aggreagted parking Operator status.</param>
        internal async Task UpdateAdminStatus(DateTime                                  Timestamp,
                                              ParkingOperator                              cso,
                                              Timestamped<ParkingOperatorAdminStatusType>  OldStatus,
                                              Timestamped<ParkingOperatorAdminStatusType>  NewStatus)
        {

            // Send parking Operator admin status change upstream
            var OnParkingOperatorAdminStatusChangedLocal = OnParkingOperatorAdminStatusChanged;
            if (OnParkingOperatorAdminStatusChangedLocal != null)
                await OnParkingOperatorAdminStatusChangedLocal(Timestamp, cso, OldStatus, NewStatus);


            // Calculate new aggregated roaming network status and send upstream
            //if (AdminStatusAggregationDelegate != null)
            //{

            //    var NewAggregatedStatus = new Timestamped<RoamingNetworkAdminStatusType>(AdminStatusAggregationDelegate(new csoAdminStatusReport(_ParkingOperators.Values)));

            //    if (NewAggregatedStatus.Value != _AdminStatusHistory.Peek().Value)
            //    {

            //        var OldAggregatedStatus = _AdminStatusHistory.Peek();

            //        _AdminStatusHistory.Push(NewAggregatedStatus);

            //        var OnAggregatedAdminStatusChangedLocal = OnAggregatedAdminStatusChanged;
            //        if (OnAggregatedAdminStatusChangedLocal != null)
            //            OnAggregatedAdminStatusChangedLocal(Timestamp, this, OldAggregatedStatus, NewAggregatedStatus);

            //    }

            //}

        }

        #endregion

        #endregion

        #region eMobility Providers...

        #region eMobilityProviders

        private readonly EntityHashSet<RoamingNetwork, eMobilityProvider_Id, eMobilityProvider> _eMobilityProviders;

        /// <summary>
        /// Return all e-mobility providers registered within this roaming network.
        /// </summary>
        public IEnumerable<eMobilityProvider> eMobilityProviders

            => _eMobilityProviders;

        #endregion

        #region eMobilityProviderIds

        /// <summary>
        /// Return all e-mobility providers identifications registered within this roaming network.
        /// </summary>
        public IEnumerable<eMobilityProvider_Id> eMobilityProviderIds

            => _eMobilityProviders.SafeSelect(emp => emp.Id);

        #endregion

        #region eMobilityProviderAdminStatus

        /// <summary>
        /// Return the admin status of all e-mobility providerss registered within this roaming network.
        /// </summary>
        public IEnumerable<KeyValuePair<eMobilityProvider_Id, IEnumerable<Timestamped<eMobilityProviderAdminStatusTypes>>>> eMobilityProviderAdminStatus

            => _eMobilityProviders.
                   SafeSelect(emp => new KeyValuePair<eMobilityProvider_Id, IEnumerable<Timestamped<eMobilityProviderAdminStatusTypes>>>(emp.Id,
                                                                                                                                         emp.AdminStatusSchedule));

        #endregion

        #region eMobilityProviderStatus

        /// <summary>
        /// Return the status of all e-mobility providerss registered within this roaming network.
        /// </summary>
        public IEnumerable<KeyValuePair<eMobilityProvider_Id, IEnumerable<Timestamped<eMobilityProviderStatusTypes>>>> eMobilityProviderStatus

            => _eMobilityProviders.
                   SafeSelect(emp => new KeyValuePair<eMobilityProvider_Id, IEnumerable<Timestamped<eMobilityProviderStatusTypes>>>(emp.Id,
                                                                                                                                     emp.StatusSchedule));

        #endregion


        #region OnEMobilityProviderAddition

        /// <summary>
        /// Called whenever an e-mobility provider will be or was added.
        /// </summary>
        public IVotingSender<DateTime, RoamingNetwork, eMobilityProvider, Boolean> OnEMobilityProviderAddition
            => _eMobilityProviders.OnAddition;

        #endregion

        #region OnEMobilityProviderRemoval

        /// <summary>
        /// Called whenever an e-mobility provider will be or was removed.
        /// </summary>
        public IVotingSender<DateTime, RoamingNetwork, eMobilityProvider, Boolean> OnEMobilityProviderRemoval
            => _eMobilityProviders.OnRemoval;

        #endregion


        #region CreateNewEMobilityProvider(EMobilityProviderId, Configurator = null)

        /// <summary>
        /// Create and register a new e-mobility (service) provider having the given
        /// unique e-mobility provider identification.
        /// </summary>
        /// <param name="ProviderId">The unique identification of the new e-mobility provider.</param>
        /// <param name="Name">The offical (multi-language) name of the e-mobility provider.</param>
        /// <param name="Description">An optional (multi-language) description of the e-mobility provider.</param>
        /// <param name="Configurator">An optional delegate to configure the new e-mobility provider before its successful creation.</param>
        /// <param name="OnSuccess">An optional delegate to configure the new e-mobility provider after its successful creation.</param>
        /// <param name="OnError">An optional delegate to be called whenever the creation of the e-mobility provider failed.</param>
        public eMobilityProvider CreateEMobilityProvider(eMobilityProvider_Id                          ProviderId,
                                                         I18NString                                    Name                            = null,
                                                         I18NString                                    Description                     = null,
                                                         eMobilityProviderPriority                     Priority                        = null,
                                                         Action<eMobilityProvider>                     Configurator                    = null,
                                                         RemoteEMobilityProviderCreatorDelegate        RemoteEMobilityProviderCreator  = null,
                                                         eMobilityProviderAdminStatusTypes             AdminStatus                     = eMobilityProviderAdminStatusTypes.Operational,
                                                         eMobilityProviderStatusTypes                  Status                          = eMobilityProviderStatusTypes.Available,
                                                         Action<eMobilityProvider>                     OnSuccess                       = null,
                                                         Action<RoamingNetwork, eMobilityProvider_Id>  OnError                         = null)
        {

            lock (_ChargingStationOperators)
            {

                var _eMobilityProviderProxy = new eMobilityProvider(ProviderId,
                                                                    this,
                                                                    Configurator,
                                                                    RemoteEMobilityProviderCreator,
                                                                    Name,
                                                                    Description,
                                                                    Priority,
                                                                    AdminStatus,
                                                                    Status);


                if (_eMobilityProviders.TryAdd(_eMobilityProviderProxy, OnSuccess))
                {

                    // _eMobilityProviders.OnDataChanged         += UpdateData;
                    // _eMobilityProviders.OnStatusChanged       += UpdateStatus;
                    // _eMobilityProviders.OnAdminStatusChanged  += UpdateAdminStatus;

                    //_EMobilityProvider.OnEMobilityStationAddition

                    //AddIRemotePushData               (_EMobilityProvider);
                    _ISendAdminStatus.Add              (_eMobilityProviderProxy);
                    _ISendStatus.Add                   (_eMobilityProviderProxy);
                    _ISend2RemoteAuthorizeStartStop.Add(_eMobilityProviderProxy);
                    _IRemoteSendChargeDetailRecord.Add (_eMobilityProviderProxy);


                    // Link events!

                    return _eMobilityProviderProxy;

                }

                throw new eMobilityProviderAlreadyExists(this,
                                                         ProviderId);

            }

        }

        #endregion

        #region RegistereMobilityProvider(Priority, eMobilityServiceProvider)

        ///// <summary>
        ///// Register the given e-Mobility (service) provider.
        ///// </summary>
        ///// <param name="Priority">The priority of the service provider.</param>
        ///// <param name="eMobilityServiceProvider">An e-Mobility service provider.</param>
        //public Boolean RegistereMobilityProvider(UInt32                     Priority,
        //                                         IeMobilityServiceProvider  eMobilityServiceProvider)
        //{

        //    var result = _IeMobilityServiceProviders.TryAdd(Priority, eMobilityServiceProvider);

        //    if (result)
        //    {

        //        this.OnChargingStationRemoval.OnNotification += eMobilityServiceProvider.RemoveChargingStations;

        //    }

        //    return result;

        //}

        #endregion

        #region ContainsEMobilityProvider(EMobilityProvider)

        /// <summary>
        /// Check if the given EMobilityProvider is already present within the roaming network.
        /// </summary>
        /// <param name="EMobilityProvider">An Charging Station Operator.</param>
        public Boolean ContainsEMobilityProvider(eMobilityProvider EMobilityProvider)

            => _eMobilityProviders.ContainsId(EMobilityProvider.Id);

        #endregion

        #region ContainsEMobilityProvider(EMobilityProviderId)

        /// <summary>
        /// Check if the given EMobilityProvider identification is already present within the roaming network.
        /// </summary>
        /// <param name="EMobilityProviderId">The unique identification of the Charging Station Operator.</param>
        public Boolean ContainsEMobilityProvider(eMobilityProvider_Id EMobilityProviderId)

            => _eMobilityProviders.ContainsId(EMobilityProviderId);

        #endregion

        #region GetEMobilityProviderById(EMobilityProviderId)

        public eMobilityProvider GetEMobilityProviderById(eMobilityProvider_Id EMobilityProviderId)

            => _eMobilityProviders.GetById(EMobilityProviderId);

        #endregion

        #region TryGetEMobilityProviderById(EMobilityProviderId, out EMobilityProvider)

        public Boolean TryGetEMobilityProviderById(eMobilityProvider_Id EMobilityProviderId, out eMobilityProvider EMobilityProvider)

            => _eMobilityProviders.TryGet(EMobilityProviderId, out EMobilityProvider);

        #endregion

        #region RemoveEMobilityProvider(EMobilityProviderId)

        public eMobilityProvider RemoveEMobilityProvider(eMobilityProvider_Id EMobilityProviderId)
        {

            eMobilityProvider _EMobilityProvider = null;

            if (_eMobilityProviders.TryRemove(EMobilityProviderId, out _EMobilityProvider))
                return _EMobilityProvider;

            return null;

        }

        #endregion

        #region TryRemoveEMobilityProvider(RemoveEMobilityProviderId, out RemoveEMobilityProvider)

        public Boolean TryRemoveEMobilityProvider(eMobilityProvider_Id EMobilityProviderId, out eMobilityProvider EMobilityProvider)

            => _eMobilityProviders.TryRemove(EMobilityProviderId, out EMobilityProvider);

        #endregion

        #endregion

        #region Smart Cities...

        #region SmartCities

        private readonly EntityHashSet<RoamingNetwork, SmartCity_Id, SmartCityProxy> _SmartCities;

        /// <summary>
        /// Return all smart cities registered within this roaming network.
        /// </summary>
        public IEnumerable<SmartCityProxy> SmartCities

            => _SmartCities;

        #endregion

        #region SmartCitiesAdminStatus

        /// <summary>
        /// Return the admin status of all smart cities registered within this roaming network.
        /// </summary>
        public IEnumerable<KeyValuePair<SmartCity_Id, IEnumerable<Timestamped<SmartCityAdminStatusType>>>> SmartCitiesAdminStatus

            => _SmartCities.
                   Select(emp => new KeyValuePair<SmartCity_Id, IEnumerable<Timestamped<SmartCityAdminStatusType>>>(emp.Id, emp.AdminStatusSchedule));

        #endregion

        #region SmartCitiesStatus

        /// <summary>
        /// Return the status of all smart cities registered within this roaming network.
        /// </summary>
        public IEnumerable<KeyValuePair<SmartCity_Id, IEnumerable<Timestamped<SmartCityStatusType>>>> SmartCitiesStatus

            => _SmartCities.
                   Select(emp => new KeyValuePair<SmartCity_Id, IEnumerable<Timestamped<SmartCityStatusType>>>(emp.Id, emp.StatusSchedule));

        #endregion


        #region OnSmartCityAddition

        /// <summary>
        /// Called whenever an EVServiceProvider will be or was added.
        /// </summary>
        public IVotingSender<DateTime, RoamingNetwork, SmartCityProxy, Boolean> OnSmartCityAddition
            => _SmartCities.OnAddition;

        #endregion

        #region OnSmartCityRemoval

        /// <summary>
        /// Called whenever an EVServiceProvider will be or was removed.
        /// </summary>
        public IVotingSender<DateTime, RoamingNetwork, SmartCityProxy, Boolean> OnSmartCityRemoval
            => _SmartCities.OnRemoval;

        #endregion


        #region CreateNewSmartCity(SmartCityId, Configurator = null)

        /// <summary>
        /// Create and register a new e-mobility (service) provider having the given
        /// unique smart city identification.
        /// </summary>
        /// <param name="SmartCityId">The unique identification of the new smart city.</param>
        /// <param name="Name">The offical (multi-language) name of the smart city.</param>
        /// <param name="Description">An optional (multi-language) description of the smart city.</param>
        /// <param name="Configurator">An optional delegate to configure the new smart city before its successful creation.</param>
        /// <param name="OnSuccess">An optional delegate to configure the new smart city after its successful creation.</param>
        /// <param name="OnError">An optional delegate to be called whenever the creation of the smart city failed.</param>
        public SmartCityProxy CreateNewSmartCity(SmartCity_Id                      SmartCityId,
                                            I18NString                            Name                     = null,
                                            I18NString                            Description              = null,
                                            SmartCityPriority                     Priority                 = null,
                                            SmartCityAdminStatusType              AdminStatus              = SmartCityAdminStatusType.Available,
                                            SmartCityStatusType                   Status                   = SmartCityStatusType.Available,
                                            Action<SmartCityProxy>                 Configurator             = null,
                                            Action<SmartCityProxy>                 OnSuccess                = null,
                                            Action<RoamingNetwork, SmartCity_Id>  OnError                  = null,
                                            RemoteSmartCityCreatorDelegate        RemoteSmartCityCreator   = null)
        {

            #region Initial checks

            if (SmartCityId == null)
                throw new ArgumentNullException(nameof(SmartCityId),  "The given smart city identification must not be null!");

            #endregion

            var _SmartCity = new SmartCityProxy(SmartCityId,
                                               Name,
                                               this,
                                               Description,
                                               Configurator,
                                               RemoteSmartCityCreator,
                                               Priority,
                                               AdminStatus,
                                               Status);


            if (_SmartCities.TryAdd(_SmartCity, OnSuccess))
            {

                // Link events!

                return _SmartCity;

            }

            throw new SmartCityAlreadyExists(this, SmartCityId);

        }

        #endregion

        #region ContainsSmartCity(SmartCity)

        /// <summary>
        /// Check if the given SmartCity is already present within the roaming network.
        /// </summary>
        /// <param name="SmartCity">An Charging Station Operator.</param>
        public Boolean ContainsSmartCity(SmartCityProxy SmartCity)

            => _SmartCities.ContainsId(SmartCity.Id);

        #endregion

        #region ContainsSmartCity(SmartCityId)

        /// <summary>
        /// Check if the given SmartCity identification is already present within the roaming network.
        /// </summary>
        /// <param name="SmartCityId">The unique identification of the Charging Station Operator.</param>
        public Boolean ContainsSmartCity(SmartCity_Id SmartCityId)

            => _SmartCities.ContainsId(SmartCityId);

        #endregion

        #region GetSmartCityById(SmartCityId)

        public SmartCityProxy GetSmartCityById(SmartCity_Id SmartCityId)

            => _SmartCities.GetById(SmartCityId);

        #endregion

        #region TryGetSmartCityById(SmartCityId, out SmartCity)

        public Boolean TryGetSmartCityById(SmartCity_Id SmartCityId, out SmartCityProxy SmartCity)

            => _SmartCities.TryGet(SmartCityId, out SmartCity);

        #endregion

        #region RemoveSmartCity(SmartCityId)

        public SmartCityProxy RemoveSmartCity(SmartCity_Id SmartCityId)
        {

            SmartCityProxy _SmartCity = null;

            if (_SmartCities.TryRemove(SmartCityId, out _SmartCity))
                return _SmartCity;

            return null;

        }

        #endregion

        #region TryRemoveSmartCity(RemoveSmartCityId, out RemoveSmartCity)

        public Boolean TryRemoveSmartCity(SmartCity_Id SmartCityId, out SmartCityProxy SmartCity)

            => _SmartCities.TryRemove(SmartCityId, out SmartCity);

        #endregion


        //#region AllTokens

        //public IEnumerable<KeyValuePair<Auth_Token, TokenAuthorizationResultType>> AllTokens

        //    => _SmartCities.SelectMany(provider => provider.AllTokens);

        //#endregion

        //#region AuthorizedTokens

        //public IEnumerable<KeyValuePair<Auth_Token, TokenAuthorizationResultType>> AuthorizedTokens

        //    => _SmartCities.SelectMany(provider => provider.AuthorizedTokens);

        //#endregion

        //#region NotAuthorizedTokens

        //public IEnumerable<KeyValuePair<Auth_Token, TokenAuthorizationResultType>> NotAuthorizedTokens

        //    => _SmartCities.SelectMany(provider => provider.NotAuthorizedTokens);

        //#endregion

        //#region BlockedTokens

        //public IEnumerable<KeyValuePair<Auth_Token, TokenAuthorizationResultType>> BlockedTokens

        //    => _SmartCities.SelectMany(provider => provider.BlockedTokens);

        //#endregion

        #endregion

        #region Navigation Providers...

        #region NavigationProviders

        private readonly EntityHashSet<RoamingNetwork, NavigationProvider_Id, NavigationProvider> _NavigationProviders;

        /// <summary>
        /// Return all navigation providers registered within this roaming network.
        /// </summary>
        public IEnumerable<NavigationProvider> NavigationProviders

            => _NavigationProviders;

        #endregion

        #region NavigationProviderAdminStatus

        /// <summary>
        /// Return the admin status of all navigation providers registered within this roaming network.
        /// </summary>
        public IEnumerable<KeyValuePair<NavigationProvider_Id, IEnumerable<Timestamped<NavigationProviderAdminStatusType>>>> NavigationProviderAdminStatus

            => _NavigationProviders.
                   Select(emp => new KeyValuePair<NavigationProvider_Id, IEnumerable<Timestamped<NavigationProviderAdminStatusType>>>(emp.Id, emp.AdminStatusSchedule));

        #endregion

        #region NavigationProviderStatus

        /// <summary>
        /// Return the status of all navigation providers registered within this roaming network.
        /// </summary>
        public IEnumerable<KeyValuePair<NavigationProvider_Id, IEnumerable<Timestamped<NavigationProviderStatusType>>>> NavigationProviderStatus

            => _NavigationProviders.
                   Select(emp => new KeyValuePair<NavigationProvider_Id, IEnumerable<Timestamped<NavigationProviderStatusType>>>(emp.Id, emp.StatusSchedule));

        #endregion


        #region OnNavigationProviderAddition

        /// <summary>
        /// Called whenever an navigation provider will be or was added.
        /// </summary>
        public IVotingSender<DateTime, RoamingNetwork, NavigationProvider, Boolean> OnNavigationProviderAddition
            => _NavigationProviders.OnAddition;

        #endregion

        #region OnNavigationProviderRemoval

        /// <summary>
        /// Called whenever an navigation provider will be or was removed.
        /// </summary>
        public IVotingSender<DateTime, RoamingNetwork, NavigationProvider, Boolean> OnNavigationProviderRemoval
            => _NavigationProviders.OnRemoval;

        #endregion


        #region CreateNewNavigationProvider(NavigationProviderId, Configurator = null)

        /// <summary>
        /// Create and register a new navigation provider having the given
        /// unique navigation provider identification.
        /// </summary>
        /// <param name="NavigationProviderId">The unique identification of the new navigation provider.</param>
        /// <param name="Name">The offical (multi-language) name of the navigation provider.</param>
        /// <param name="Description">An optional (multi-language) description of the navigation provider.</param>
        /// <param name="Configurator">An optional delegate to configure the new navigation provider before its successful creation.</param>
        /// <param name="OnSuccess">An optional delegate to configure the new navigation provider after its successful creation.</param>
        /// <param name="OnError">An optional delegate to be called whenever the creation of the navigation provider failed.</param>
        public NavigationProvider CreateNewNavigationProvider(NavigationProvider_Id                          NavigationProviderId,
                                                            I18NString                                    Name                            = null,
                                                            I18NString                                    Description                     = null,
                                                            NavigationProviderPriority                     Priority                        = null,
                                                            NavigationProviderAdminStatusType              AdminStatus                     = NavigationProviderAdminStatusType.Available,
                                                            NavigationProviderStatusType                   Status                          = NavigationProviderStatusType.Available,
                                                            Action<NavigationProvider>                     Configurator                    = null,
                                                            Action<NavigationProvider>                     OnSuccess                       = null,
                                                            Action<RoamingNetwork, NavigationProvider_Id>  OnError                         = null,
                                                            RemoteNavigationProviderCreatorDelegate        RemoteNavigationProviderCreator  = null)
        {

            #region Initial checks

            if (NavigationProviderId == null)
                throw new ArgumentNullException(nameof(NavigationProviderId),  "The given navigation provider identification must not be null!");

            #endregion

            var _NavigationProvider = new NavigationProvider(NavigationProviderId,
                                                           this,
                                                           Configurator,
                                                           RemoteNavigationProviderCreator,
                                                           Name,
                                                           Description,
                                                           Priority,
                                                           AdminStatus,
                                                           Status);


            if (_NavigationProviders.TryAdd(_NavigationProvider, OnSuccess))
            {

                // Link events!

                return _NavigationProvider;

            }

            throw new NavigationProviderAlreadyExists(this, NavigationProviderId);

        }

        #endregion

        #region ContainsNavigationProvider(NavigationProvider)

        /// <summary>
        /// Check if the given NavigationProvider is already present within the roaming network.
        /// </summary>
        /// <param name="NavigationProvider">An Charging Station Operator.</param>
        public Boolean ContainsNavigationProvider(NavigationProvider NavigationProvider)

            => _NavigationProviders.ContainsId(NavigationProvider.Id);

        #endregion

        #region ContainsNavigationProvider(NavigationProviderId)

        /// <summary>
        /// Check if the given NavigationProvider identification is already present within the roaming network.
        /// </summary>
        /// <param name="NavigationProviderId">The unique identification of the Charging Station Operator.</param>
        public Boolean ContainsNavigationProvider(NavigationProvider_Id NavigationProviderId)

            => _NavigationProviders.ContainsId(NavigationProviderId);

        #endregion

        #region GetNavigationProviderById(NavigationProviderId)

        public NavigationProvider GetNavigationProviderById(NavigationProvider_Id NavigationProviderId)

            => _NavigationProviders.GetById(NavigationProviderId);

        #endregion

        #region TryGetNavigationProviderById(NavigationProviderId, out NavigationProvider)

        public Boolean TryGetNavigationProviderById(NavigationProvider_Id NavigationProviderId, out NavigationProvider NavigationProvider)

            => _NavigationProviders.TryGet(NavigationProviderId, out NavigationProvider);

        #endregion

        #region RemoveNavigationProvider(NavigationProviderId)

        public NavigationProvider RemoveNavigationProvider(NavigationProvider_Id NavigationProviderId)
        {

            NavigationProvider _NavigationProvider = null;

            if (_NavigationProviders.TryRemove(NavigationProviderId, out _NavigationProvider))
                return _NavigationProvider;

            return null;

        }

        #endregion

        #region TryRemoveNavigationProvider(RemoveNavigationProviderId, out RemoveNavigationProvider)

        public Boolean TryRemoveNavigationProvider(NavigationProvider_Id NavigationProviderId, out NavigationProvider NavigationProvider)

            => _NavigationProviders.TryRemove(NavigationProviderId, out NavigationProvider);

        #endregion

        #endregion

        #region Grid Operators...

        #region GridOperators

        private readonly EntityHashSet<RoamingNetwork, GridOperator_Id, GridOperator> _GridOperators;

        /// <summary>
        /// Return all smart cities registered within this roaming network.
        /// </summary>
        public IEnumerable<GridOperator> GridOperators

            => _GridOperators;

        #endregion

        #region GridOperatorsAdminStatus

        /// <summary>
        /// Return the admin status of all smart cities registered within this roaming network.
        /// </summary>
        public IEnumerable<KeyValuePair<GridOperator_Id, IEnumerable<Timestamped<GridOperatorAdminStatusType>>>> GridOperatorsAdminStatus

            => _GridOperators.
                   Select(emp => new KeyValuePair<GridOperator_Id, IEnumerable<Timestamped<GridOperatorAdminStatusType>>>(emp.Id, emp.AdminStatusSchedule));

        #endregion

        #region GridOperatorsStatus

        /// <summary>
        /// Return the status of all smart cities registered within this roaming network.
        /// </summary>
        public IEnumerable<KeyValuePair<GridOperator_Id, IEnumerable<Timestamped<GridOperatorStatusType>>>> GridOperatorsStatus

            => _GridOperators.
                   Select(emp => new KeyValuePair<GridOperator_Id, IEnumerable<Timestamped<GridOperatorStatusType>>>(emp.Id, emp.StatusSchedule));

        #endregion


        #region OnGridOperatorAddition

        /// <summary>
        /// Called whenever an EVServiceProvider will be or was added.
        /// </summary>
        public IVotingSender<DateTime, RoamingNetwork, GridOperator, Boolean> OnGridOperatorAddition
            => _GridOperators.OnAddition;

        #endregion

        #region OnGridOperatorRemoval

        /// <summary>
        /// Called whenever an EVServiceProvider will be or was removed.
        /// </summary>
        public IVotingSender<DateTime, RoamingNetwork, GridOperator, Boolean> OnGridOperatorRemoval
            => _GridOperators.OnRemoval;

        #endregion


        #region CreateNewGridOperator(GridOperatorId, Configurator = null)

        /// <summary>
        /// Create and register a new e-mobility (service) provider having the given
        /// unique smart city identification.
        /// </summary>
        /// <param name="GridOperatorId">The unique identification of the new smart city.</param>
        /// <param name="Name">The offical (multi-language) name of the smart city.</param>
        /// <param name="Description">An optional (multi-language) description of the smart city.</param>
        /// <param name="Configurator">An optional delegate to configure the new smart city before its successful creation.</param>
        /// <param name="OnSuccess">An optional delegate to configure the new smart city after its successful creation.</param>
        /// <param name="OnError">An optional delegate to be called whenever the creation of the smart city failed.</param>
        public GridOperator CreateNewGridOperator(GridOperator_Id                          GridOperatorId,
                                            I18NString                            Name                    = null,
                                            I18NString                            Description             = null,
                                            GridOperatorPriority                     Priority                = null,
                                            GridOperatorAdminStatusType              AdminStatus             = GridOperatorAdminStatusType.Available,
                                            GridOperatorStatusType                   Status                  = GridOperatorStatusType.Available,
                                            Action<GridOperator>                     Configurator            = null,
                                            Action<GridOperator>                     OnSuccess               = null,
                                            Action<RoamingNetwork, GridOperator_Id>  OnError                 = null,
                                            RemoteGridOperatorCreatorDelegate        RemoteGridOperatorCreator  = null)
        {

            #region Initial checks

            if (GridOperatorId == null)
                throw new ArgumentNullException(nameof(GridOperatorId),  "The given smart city identification must not be null!");

            #endregion

            var _GridOperator = new GridOperator(GridOperatorId,
                                           this,
                                           Configurator,
                                           RemoteGridOperatorCreator,
                                           Name,
                                           Description,
                                           Priority,
                                           AdminStatus,
                                           Status);


            if (_GridOperators.TryAdd(_GridOperator, OnSuccess))
            {

                // Link events!

                return _GridOperator;

            }

            throw new GridOperatorAlreadyExists(this, GridOperatorId);

        }

        #endregion

        #region ContainsGridOperator(GridOperator)

        /// <summary>
        /// Check if the given GridOperator is already present within the roaming network.
        /// </summary>
        /// <param name="GridOperator">An Charging Station Operator.</param>
        public Boolean ContainsGridOperator(GridOperator GridOperator)

            => _GridOperators.ContainsId(GridOperator.Id);

        #endregion

        #region ContainsGridOperator(GridOperatorId)

        /// <summary>
        /// Check if the given GridOperator identification is already present within the roaming network.
        /// </summary>
        /// <param name="GridOperatorId">The unique identification of the Charging Station Operator.</param>
        public Boolean ContainsGridOperator(GridOperator_Id GridOperatorId)

            => _GridOperators.ContainsId(GridOperatorId);

        #endregion

        #region GetGridOperatorById(GridOperatorId)

        public GridOperator GetGridOperatorById(GridOperator_Id GridOperatorId)

            => _GridOperators.GetById(GridOperatorId);

        #endregion

        #region TryGetGridOperatorById(GridOperatorId, out GridOperator)

        public Boolean TryGetGridOperatorById(GridOperator_Id GridOperatorId, out GridOperator GridOperator)

            => _GridOperators.TryGet(GridOperatorId, out GridOperator);

        #endregion

        #region RemoveGridOperator(GridOperatorId)

        public GridOperator RemoveGridOperator(GridOperator_Id GridOperatorId)
        {

            GridOperator _GridOperator = null;

            if (_GridOperators.TryRemove(GridOperatorId, out _GridOperator))
                return _GridOperator;

            return null;

        }

        #endregion

        #region TryRemoveGridOperator(RemoveGridOperatorId, out RemoveGridOperator)

        public Boolean TryRemoveGridOperator(GridOperator_Id GridOperatorId, out GridOperator GridOperator)

            => _GridOperators.TryRemove(GridOperatorId, out GridOperator);

        #endregion

        #endregion


        #region Charging Station Operator Roaming Providers...

        private readonly ConcurrentDictionary<UInt32, IEMPRoamingProvider>  _eMobilityRoamingServices;
        //private readonly ConcurrentDictionary<UInt32, ISendData>            _PushEVSEDataToOperatorRoamingServices;
        //private readonly ConcurrentDictionary<UInt32, ISendAdminStatus>     _PushEVSEAdminStatusToOperatorRoamingServices;
        //private readonly ConcurrentDictionary<UInt32, ISendStatus>          _PushEVSEStatusToOperatorRoamingServices;

        #region ChargingStationOperatorRoamingProviders

        //private readonly ConcurrentDictionary<UInt32,             IChargingStationOperatorRoamingProvider>  _ChargingStationOperatorRoamingProviderPriorities;
        private readonly ConcurrentDictionary<CSORoamingProvider_Id, ICSORoamingProvider>  _ChargingStationOperatorRoamingProviders;

        /// <summary>
        /// Return all Charging Station Operator roaming providers registered within this roaming network.
        /// </summary>
        public IEnumerable<ICSORoamingProvider> ChargingStationOperatorRoamingProviders => _ChargingStationOperatorRoamingProviders.Values;

        public Boolean TryGet(CSORoamingProvider_Id Id, out ICSORoamingProvider CSORoamingProvider)
            => _ChargingStationOperatorRoamingProviders.TryGetValue(Id, out CSORoamingProvider);

        #endregion

        #region CreateNewRoamingProvider(OperatorRoamingService, Configurator = null)

        /// <summary>
        /// Create and register a new electric vehicle roaming provider having the given
        /// unique electric vehicle roaming provider identification.
        /// </summary>
        /// <param name="Configurator">An optional delegate to configure the new roaming provider after its creation.</param>
        public ICSORoamingProvider CreateNewRoamingProvider(ICSORoamingProvider          _CPORoamingProvider,
                                                            Action<ICSORoamingProvider>  Configurator  = null)
        {

            #region Initial checks

            if (_CPORoamingProvider.Id == null)
                throw new ArgumentNullException(nameof(_CPORoamingProvider) + ".Id",    "The given roaming provider identification must not be null!");

            if (IEnumerableExtensions.IsNullOrEmpty(_CPORoamingProvider.Name))
                throw new ArgumentNullException(nameof(_CPORoamingProvider) + ".Name",  "The given roaming provider name must not be null or empty!");

            if (_ChargingStationOperatorRoamingProviders.ContainsKey(_CPORoamingProvider.Id))
                throw new CSORoamingProviderAlreadyExists(this, _CPORoamingProvider.Id);

            if (_CPORoamingProvider.RoamingNetwork.Id != this.Id)
                throw new ArgumentException("The given operator roaming service is not part of this roaming network!", nameof(_CPORoamingProvider));

            #endregion

            Configurator?.Invoke(_CPORoamingProvider);

            if (CPORoamingProviderAddition.SendVoting(this, _CPORoamingProvider))
            {
                if (_ChargingStationOperatorRoamingProviders.TryAdd(_CPORoamingProvider.Id, _CPORoamingProvider))
                {

                    // this.OnChargingStationRemoval.OnNotification += _CPORoamingProvider.RemoveChargingStations;

                    //SetRoamingProviderPriority(_CPORoamingProvider,
                    //                           _ChargingStationOperatorRoamingProviderPriorities.Count > 0
                    //                               ? _ChargingStationOperatorRoamingProviderPriorities.Keys.Max() + 1
                    //                               : 10);

                    _ISendData.Add                     (_CPORoamingProvider);
                    _ISendAdminStatus.Add              (_CPORoamingProvider);
                    _ISendStatus.Add                   (_CPORoamingProvider);
                    _ISend2RemoteAuthorizeStartStop.Add(_CPORoamingProvider);
                    _IRemoteSendChargeDetailRecord.Add (_CPORoamingProvider);

                    CPORoamingProviderAddition.SendNotification(this, _CPORoamingProvider);

                    return _CPORoamingProvider;

                }
            }

            throw new Exception("Could not create new roaming provider '" + _CPORoamingProvider.Id + "'!");

        }

        #endregion

        #region SetRoamingProviderPriority(csoRoamingProvider, Priority)

        ///// <summary>
        ///// Change the given Charging Station Operator roaming service priority.
        ///// </summary>
        ///// <param name="csoRoamingProvider">The Charging Station Operator roaming provider.</param>
        ///// <param name="Priority">The priority of the service.</param>
        //public Boolean SetRoamingProviderPriority(IChargingStationOperatorRoamingProvider csoRoamingProvider,
        //                                          UInt32                                  Priority)
        //{

        //    var a = _ChargingStationOperatorRoamingProviderPriorities.FirstOrDefault(_ => _.Value == csoRoamingProvider);

        //    if (a.Key > 0)
        //    {
        //        IChargingStationOperatorRoamingProvider b = null;
        //        _ChargingStationOperatorRoamingProviderPriorities.TryRemove(a.Key, out b);
        //    }

        //    return _ChargingStationOperatorRoamingProviderPriorities.TryAdd(Priority, csoRoamingProvider);

        //}

        #endregion

        #region CPORoamingProviderAddition

        private readonly IVotingNotificator<RoamingNetwork, ICSORoamingProvider, Boolean> CPORoamingProviderAddition;

        /// <summary>
        /// Called whenever a RoamingProvider will be or was added.
        /// </summary>
        public IVotingSender<RoamingNetwork, ICSORoamingProvider, Boolean> OnCPORoamingProviderAddition => CPORoamingProviderAddition;

        #endregion

        #region CPORoamingProviderRemoval

        private readonly IVotingNotificator<RoamingNetwork, ICSORoamingProvider, Boolean> CPORoamingProviderRemoval;

        /// <summary>
        /// Called whenever a RoamingProvider will be or was removed.
        /// </summary>
        public IVotingSender<RoamingNetwork, ICSORoamingProvider, Boolean> OnCPORoamingProviderRemoval => CPORoamingProviderRemoval;

        #endregion

        #endregion

        #region EMP Roaming Providers...

        #region EMPRoamingProviders

        private readonly ConcurrentDictionary<EMPRoamingProvider_Id, IEMPRoamingProvider> _EMPRoamingProviders;

        /// <summary>
        /// Return all roaming providers registered within this roaming network.
        /// </summary>
        public IEnumerable<IEMPRoamingProvider> EMPRoamingProviders => _EMPRoamingProviders.Values;

        public Boolean TryGet(EMPRoamingProvider_Id Id, out IEMPRoamingProvider EMPRoamingProvider)
            => _EMPRoamingProviders.TryGetValue(Id, out EMPRoamingProvider);

        #endregion

        #region CreateNewRoamingProvider(eMobilityRoamingService, Configurator = null)

        /// <summary>
        /// Create and register a new electric vehicle roaming provider having the given
        /// unique electric vehicle roaming provider identification.
        /// </summary>
        /// <param name="eMobilityRoamingService">A e-mobility roaming service.</param>
        /// <param name="Configurator">An optional delegate to configure the new roaming provider after its creation.</param>
        public IEMPRoamingProvider CreateNewRoamingProvider(IEMPRoamingProvider          eMobilityRoamingService,
                                                            Action<IEMPRoamingProvider>  Configurator = null)
        {

            #region Initial checks

            if (eMobilityRoamingService.Id == null)
                throw new ArgumentNullException(nameof(eMobilityRoamingService) + ".Id",    "The given roaming provider identification must not be null!");

            if (IEnumerableExtensions.IsNullOrEmpty(eMobilityRoamingService.Name))
                throw new ArgumentNullException(nameof(eMobilityRoamingService) + ".Name",  "The given roaming provider name must not be null or empty!");

            if (_EMPRoamingProviders.ContainsKey(eMobilityRoamingService.Id))
                throw new EMPRoamingProviderAlreadyExists(this, eMobilityRoamingService.Id);

            if (eMobilityRoamingService.RoamingNetwork.Id != this.Id)
                throw new ArgumentException("The given operator roaming service is not part of this roaming network!", nameof(eMobilityRoamingService));

            #endregion

            Configurator?.Invoke(eMobilityRoamingService);

            if (EMPRoamingProviderAddition.SendVoting(this, eMobilityRoamingService))
            {
                if (_EMPRoamingProviders.TryAdd(eMobilityRoamingService.Id, eMobilityRoamingService))
                {

                    EMPRoamingProviderAddition.SendNotification(this, eMobilityRoamingService);

                    SetRoamingProviderPriority(eMobilityRoamingService,
                                               _eMobilityRoamingServices.Count > 0
                                                   ? _eMobilityRoamingServices.Keys.Max() + 1
                                                   : 10);

                    return eMobilityRoamingService;

                }
            }

            throw new Exception("Could not create new roaming provider '" + eMobilityRoamingService.Id + "'!");

        }

        #endregion

        #region SetRoamingProviderPriority(eMobilityRoamingService, Priority)

        /// <summary>
        /// Change the prioity of the given e-mobility roaming service.
        /// </summary>
        /// <param name="eMobilityRoamingService">The e-mobility roaming service.</param>
        /// <param name="Priority">The priority of the service.</param>
        public Boolean SetRoamingProviderPriority(IEMPRoamingProvider  eMobilityRoamingService,
                                                  UInt32              Priority)

            => _eMobilityRoamingServices.TryAdd(Priority, eMobilityRoamingService);

        #endregion

        #region EMPRoamingProviderAddition

        private readonly IVotingNotificator<RoamingNetwork, IEMPRoamingProvider, Boolean> EMPRoamingProviderAddition;

        /// <summary>
        /// Called whenever a RoamingProvider will be or was added.
        /// </summary>
        public IVotingSender<RoamingNetwork, IEMPRoamingProvider, Boolean> OnEMPRoamingProviderAddition => EMPRoamingProviderAddition;

        #endregion

        #region EMPRoamingProviderRemoval

        private readonly IVotingNotificator<RoamingNetwork, IEMPRoamingProvider, Boolean> EMPRoamingProviderRemoval;

        /// <summary>
        /// Called whenever a RoamingProvider will be or was removed.
        /// </summary>
        public IVotingSender<RoamingNetwork, IEMPRoamingProvider, Boolean> OnEMPRoamingProviderRemoval => EMPRoamingProviderRemoval;

        #endregion


        #region RegisterPushEVSEDataService(Priority, PushEVSEDataServices)

        ///// <summary>
        ///// Register the given push-data service.
        ///// </summary>
        ///// <param name="Priority">The priority of the service.</param>
        ///// <param name="PushEVSEDataServices">The push-data service.</param>
        //public Boolean RegisterPushEVSEStatusService(UInt32              Priority,
        //                                             IPushData           PushEVSEDataServices)

        //    => _PushEVSEDataToOperatorRoamingServices.TryAdd(Priority, PushEVSEDataServices);

        #endregion

        #region RegisterPushEVSEStatusService(Priority, PushEVSEStatusServices)

        ///// <summary>
        ///// Register the given push-status service.
        ///// </summary>
        ///// <param name="Priority">The priority of the service.</param>
        ///// <param name="PushEVSEStatusServices">The push-status service.</param>
        //public Boolean RegisterPushEVSEStatusService(UInt32       Priority,
        //                                             IPushStatus  PushEVSEStatusServices)

        //    => _PushEVSEStatusToOperatorRoamingServices.TryAdd(Priority, PushEVSEStatusServices);

        #endregion

        #endregion



        #region ChargingPools...

        #region ChargingPools

        /// <summary>
        /// Return all charging pools registered within this roaming network.
        /// </summary>
        public IEnumerable<ChargingPool> ChargingPools

            => _ChargingStationOperators.SelectMany(cso => cso.ChargingPools);

        #endregion

        #region ChargingPoolIds        (IncludePools = null)

        /// <summary>
        /// Return all charging pool identifications registered within this roaming network.
        /// </summary>
        /// <param name="IncludePools">An optional delegate for filtering charging pools.</param>
        public IEnumerable<ChargingPool_Id> ChargingPoolIds(IncludeChargingPoolDelegate IncludePools = null)

            => _ChargingStationOperators.
                   SelectMany(cso => cso.ChargingPoolIds(IncludePools));

        #endregion

        #region ChargingPoolAdminStatus(IncludePools = null)

        /// <summary>
        /// Return the admin status of all charging pools registered within this roaming network.
        /// </summary>
        /// <param name="IncludePools">An optional delegate for filtering charging pools.</param>
        public IEnumerable<ChargingPoolAdminStatus> ChargingPoolAdminStatus(IncludeChargingPoolDelegate IncludePools = null)

            => _ChargingStationOperators.
                   SelectMany(cso => cso.ChargingPoolAdminStatus(IncludePools));

        #endregion

        #region ChargingPoolStatus     (IncludePools = null)

        /// <summary>
        /// Return the status of all charging pools registered within this roaming network.
        /// </summary>
        /// <param name="IncludePools">An optional delegate for filtering charging pools.</param>
        public IEnumerable<ChargingPoolStatus> ChargingPoolStatus(IncludeChargingPoolDelegate IncludePools = null)

            => _ChargingStationOperators.
                   SelectMany(cso => cso.ChargingPoolStatus(IncludePools));

        #endregion


        #region ContainsChargingPool(ChargingPool)

        /// <summary>
        /// Check if the given charging pool is already present within the roaming network.
        /// </summary>
        /// <param name="ChargingPool">A charging pool.</param>
        public Boolean ContainsChargingPool(ChargingPool ChargingPool)
        {

            ChargingStationOperator _ChargingStationOperator  = null;

            if (TryGetChargingStationOperatorById(ChargingPool.Operator.Id, out _ChargingStationOperator))
                return _ChargingStationOperator.ContainsChargingPool(ChargingPool.Id);

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

            ChargingStationOperator _ChargingStationOperator  = null;

            if (TryGetChargingStationOperatorById(ChargingPoolId.OperatorId, out _ChargingStationOperator))
                return _ChargingStationOperator.ContainsChargingPool(ChargingPoolId);

            return false;

        }

        #endregion

        #region GetChargingPoolbyId(ChargingPoolId)

        public ChargingPool GetChargingPoolById(ChargingPool_Id ChargingPoolId)
        {

            ChargingPool            _ChargingPool             = null;
            ChargingStationOperator _ChargingStationOperator  = null;

            if (TryGetChargingStationOperatorById(ChargingPoolId.OperatorId, out _ChargingStationOperator) &&
                _ChargingStationOperator.TryGetChargingPoolById(ChargingPoolId, out _ChargingPool))
                    return _ChargingPool;

            return null;

        }

        #endregion

        #region TryGetChargingPoolbyId(ChargingPoolId, out ChargingPool)

        public Boolean TryGetChargingPoolById(ChargingPool_Id ChargingPoolId, out ChargingPool ChargingPool)
        {

            ChargingStationOperator _ChargingStationOperator  = null;

            if (TryGetChargingStationOperatorById(ChargingPoolId.OperatorId, out _ChargingStationOperator))
                return _ChargingStationOperator.TryGetChargingPoolById(ChargingPoolId, out ChargingPool);

            ChargingPool = null;
            return false;

        }

        #endregion

        #region SetChargingPoolAdminStatus(ChargingPoolId, StatusList)

        public void SetChargingPoolAdminStatus(ChargingPool_Id                                        ChargingPoolId,
                                               IEnumerable<Timestamped<ChargingPoolAdminStatusTypes>>  StatusList)
        {

            ChargingStationOperator _cso  = null;

            if (TryGetChargingStationOperatorById(ChargingPoolId.OperatorId, out _cso))
                _cso.SetChargingPoolAdminStatus(ChargingPoolId, StatusList);

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


        #region OnChargingPoolData/(Admin)StatusChanged

        /// <summary>
        /// An event fired whenever the static data of any subordinated charging pool changed.
        /// </summary>
        public event OnChargingPoolDataChangedDelegate         OnChargingPoolDataChanged;

        /// <summary>
        /// An event fired whenever the admin status of any subordinated charging pool changed.
        /// </summary>
        public event OnChargingPoolAdminStatusChangedDelegate  OnChargingPoolAdminStatusChanged;

        /// <summary>
        /// An event fired whenever the aggregated dynamic status of any subordinated charging pool changed.
        /// </summary>
        public event OnChargingPoolStatusChangedDelegate       OnChargingPoolStatusChanged;

        #endregion

        #region OnChargingPoolAdminDiff

        public delegate void OnChargingPoolAdminDiffDelegate(ChargingPoolAdminStatusDiff StatusDiff);

        /// <summary>
        /// An event fired whenever a charging station admin status diff was received.
        /// </summary>
        public event OnChargingPoolAdminDiffDelegate OnChargingPoolAdminDiff;

        #endregion

        #region ChargingPoolAddition

        internal readonly IVotingNotificator<DateTime, ChargingStationOperator, ChargingPool, Boolean> ChargingPoolAddition;

        /// <summary>
        /// Called whenever an EVS pool will be or was added.
        /// </summary>
        public IVotingSender<DateTime, ChargingStationOperator, ChargingPool, Boolean> OnChargingPoolAddition

            => ChargingPoolAddition;

        #endregion

        #region ChargingPoolRemoval

        internal readonly IVotingNotificator<DateTime, ChargingStationOperator, ChargingPool, Boolean> ChargingPoolRemoval;

        /// <summary>
        /// Called whenever an EVS pool will be or was removed.
        /// </summary>
        public IVotingSender<DateTime, ChargingStationOperator, ChargingPool, Boolean> OnChargingPoolRemoval

            => ChargingPoolRemoval;

        #endregion


        #region (internal) UpdateChargingPoolData       (Timestamp, EventTrackingId, ChargingPool, PropertyName, OldValue, NewValue)

        /// <summary>
        /// Update the data of an charging pool.
        /// </summary>
        /// <param name="Timestamp">The timestamp when this change was detected.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="ChargingPool">The changed charging pool.</param>
        /// <param name="PropertyName">The name of the changed property.</param>
        /// <param name="OldValue">The old value of the changed property.</param>
        /// <param name="NewValue">The new value of the changed property.</param>
        internal async Task UpdateChargingPoolData(DateTime          Timestamp,
                                                   EventTracking_Id  EventTrackingId,
                                                   ChargingPool      ChargingPool,
                                                   String            PropertyName,
                                                   Object            OldValue,
                                                   Object            NewValue)
        {

            //foreach (var AuthenticationService in _IeMobilityServiceProviders.
            //                                          OrderBy(AuthServiceWithPriority => AuthServiceWithPriority.Key).
            //                                          Select (AuthServiceWithPriority => AuthServiceWithPriority.Value))
            //{

            //    result = await AuthenticationService.PushEVSEStatus(new EVSEStatus(EVSE.Id, NewStatus.Value, NewStatus.Timestamp),
            //                                                        ActionType.update,
            //                                                        EVSE.Operator.Id);

            //}

            var results = _ISendData.WhenAll(iSendData => iSendData.
                                                              UpdateStaticData(ChargingPool,
                                                                               PropertyName,
                                                                               OldValue,
                                                                               NewValue));

            //foreach (var PushEVSEStatusService in _PushEVSEStatusToOperatorRoamingServices.
            //                                          OrderBy(AuthServiceWithPriority => AuthServiceWithPriority.Key).
            //                                          Select (AuthServiceWithPriority => AuthServiceWithPriority.Value))
            //{

            //    result = await PushEVSEStatusService.PushEVSEStatus(new EVSEStatus(EVSE.Id, NewStatus.Value, NewStatus.Timestamp),
            //                                                        ActionType.update,
            //                                                        EVSE.Operator.Id);

            //}

            var OnChargingPoolDataChangedLocal = OnChargingPoolDataChanged;
            if (OnChargingPoolDataChangedLocal != null)
                await OnChargingPoolDataChangedLocal(Timestamp,
                                                     EventTrackingId,
                                                     ChargingPool,
                                                     PropertyName,
                                                     OldValue,
                                                     NewValue);

        }

        #endregion

        #region (internal) UpdateChargingPoolAdminStatus(Timestamp, EventTrackingId, ChargingPool, OldStatus, NewStatus)

        /// <summary>
        /// Update a charging pool admin status.
        /// </summary>
        /// <param name="Timestamp">The timestamp when this change was detected.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="ChargingPool">The updated charging pool.</param>
        /// <param name="OldStatus">The old aggregated charging pool status.</param>
        /// <param name="NewStatus">The new aggregated charging pool status.</param>
        internal async Task UpdateChargingPoolAdminStatus(DateTime                                  Timestamp,
                                                          EventTracking_Id                          EventTrackingId,
                                                          ChargingPool                              ChargingPool,
                                                          Timestamped<ChargingPoolAdminStatusTypes>  OldStatus,
                                                          Timestamped<ChargingPoolAdminStatusTypes>  NewStatus)
        {

            var OnChargingPoolAdminStatusChangedLocal = OnChargingPoolAdminStatusChanged;
            if (OnChargingPoolAdminStatusChangedLocal != null)
                await OnChargingPoolAdminStatusChangedLocal(Timestamp,
                                                            EventTrackingId,
                                                            ChargingPool,
                                                            OldStatus,
                                                            NewStatus);

        }

        #endregion

        #region (internal) UpdateChargingPoolStatus     (Timestamp, EventTrackingId, ChargingPool, OldStatus, NewStatus)

        /// <summary>
        /// Update a charging pool status.
        /// </summary>
        /// <param name="Timestamp">The timestamp when this change was detected.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="ChargingPool">The updated charging pool.</param>
        /// <param name="OldStatus">The old aggregated charging pool status.</param>
        /// <param name="NewStatus">The new aggregated charging pool status.</param>
        internal async Task UpdateChargingPoolStatus(DateTime                             Timestamp,
                                                     EventTracking_Id                     EventTrackingId,
                                                     ChargingPool                         ChargingPool,
                                                     Timestamped<ChargingPoolStatusTypes>  OldStatus,
                                                     Timestamped<ChargingPoolStatusTypes>  NewStatus)
        {

            var OnChargingPoolStatusChangedLocal = OnChargingPoolStatusChanged;
            if (OnChargingPoolStatusChangedLocal != null)
                await OnChargingPoolStatusChangedLocal(Timestamp,
                                                       EventTrackingId,
                                                       ChargingPool,
                                                       OldStatus,
                                                       NewStatus);

        }

        #endregion

        #endregion

        #region ChargingStations...

        #region ChargingStations

        /// <summary>
        /// Return all charging stations registered within this roaming network.
        /// </summary>
        public IEnumerable<ChargingStation> ChargingStations

            => _ChargingStationOperators.SelectMany(cso => cso.ChargingStations);

        #endregion

        #region ChargingStationIds        (IncludeStations = null)

        /// <summary>
        /// Return all charging station identifications registered within this roaming network.
        /// </summary>
        /// <param name="IncludeStations">An optional delegate for filtering charging stations.</param>
        public IEnumerable<ChargingStation_Id> ChargingStationIds(IncludeChargingStationDelegate IncludeStations = null)

            => _ChargingStationOperators.
                   SelectMany(cso => cso.ChargingStationIds(IncludeStations));

        #endregion

        #region ChargingStationAdminStatus(IncludeStations = null)

        /// <summary>
        /// Return the admin status of all charging stations registered within this roaming network.
        /// </summary>
        /// <param name="IncludeStations">An optional delegate for filtering charging stations.</param>
        public IEnumerable<ChargingStationAdminStatus> ChargingStationAdminStatus(IncludeChargingStationDelegate IncludeStations = null)

            => _ChargingStationOperators.
                   SelectMany(cso => cso.ChargingStationAdminStatus(IncludeStations));

        #endregion

        #region ChargingStationStatus     (IncludeStations = null)

        /// <summary>
        /// Return the status of all charging stations registered within this roaming network.
        /// </summary>
        /// <param name="IncludeStations">An optional delegate for filtering charging stations.</param>
        public IEnumerable<ChargingStationStatus> ChargingStationStatus(IncludeChargingStationDelegate IncludeStations = null)

            => _ChargingStationOperators.
                   SelectMany(cso => cso.ChargingStationStatus(IncludeStations));

        #endregion


        #region ContainsChargingStation(ChargingStation)

        /// <summary>
        /// Check if the given charging station is already present within the roaming network.
        /// </summary>
        /// <param name="ChargingStation">A charging station.</param>
        public Boolean ContainsChargingStation(ChargingStation ChargingStation)
        {

            ChargingStationOperator _ChargingStationOperator  = null;

            if (TryGetChargingStationOperatorById(ChargingStation.Operator.Id, out _ChargingStationOperator))
                return _ChargingStationOperator.ContainsChargingStation(ChargingStation.Id);

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

            ChargingStationOperator _ChargingStationOperator  = null;

            if (TryGetChargingStationOperatorById(ChargingStationId.OperatorId, out _ChargingStationOperator))
                return _ChargingStationOperator.ContainsChargingStation(ChargingStationId);

            return false;

        }

        #endregion

        #region GetChargingStationbyId(ChargingStationId)

        public ChargingStation GetChargingStationById(ChargingStation_Id ChargingStationId)
        {

            ChargingStation         _ChargingStation          = null;
            ChargingStationOperator _ChargingStationOperator  = null;

            if (TryGetChargingStationOperatorById(ChargingStationId.OperatorId, out _ChargingStationOperator) &&
                _ChargingStationOperator.TryGetChargingStationById(ChargingStationId, out _ChargingStation))
                    return _ChargingStation;

            return null;

        }

        #endregion

        #region TryGetChargingStationbyId(ChargingStationId, out ChargingStation)

        public Boolean TryGetChargingStationById(ChargingStation_Id ChargingStationId, out ChargingStation ChargingStation)
        {

            ChargingStationOperator _ChargingStationOperator  = null;

            if (TryGetChargingStationOperatorById(ChargingStationId.OperatorId, out _ChargingStationOperator))
                return _ChargingStationOperator.TryGetChargingStationById(ChargingStationId, out ChargingStation);

            ChargingStation = null;
            return false;

        }

        #endregion

        #region SetChargingStationStatus(ChargingStationId, CurrentStatus)

        public void SetChargingStationStatus(ChargingStation_Id                      ChargingStationId,
                                             Timestamped<ChargingStationStatusTypes>  CurrentStatus)
        {

            ChargingStationOperator _cso  = null;

            if (TryGetChargingStationOperatorById(ChargingStationId.OperatorId, out _cso))
                _cso.SetChargingStationStatus(ChargingStationId, CurrentStatus);

        }

        #endregion

        #region SetChargingStationAdminStatus(ChargingStationId, CurrentStatus)

        public void SetChargingStationAdminStatus(ChargingStation_Id                           ChargingStationId,
                                                  Timestamped<ChargingStationAdminStatusTypes>  CurrentStatus)
        {

            ChargingStationOperator _cso  = null;

            if (TryGetChargingStationOperatorById(ChargingStationId.OperatorId, out _cso))
                _cso.SetChargingStationAdminStatus(ChargingStationId, CurrentStatus);

        }

        #endregion

        #region SetChargingStationAdminStatus(ChargingStationId, StatusList)

        public void SetChargingStationAdminStatus(ChargingStation_Id                                        ChargingStationId,
                                                  IEnumerable<Timestamped<ChargingStationAdminStatusTypes>>  StatusList)
        {

            ChargingStationOperator _cso  = null;

            if (TryGetChargingStationOperatorById(ChargingStationId.OperatorId, out _cso))
                _cso.SetChargingStationAdminStatus(ChargingStationId, StatusList);

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


        #region OnChargingStationData/(Admin)StatusChanged

        /// <summary>
        /// An event fired whenever the static data of any subordinated charging station changed.
        /// </summary>
        public event OnChargingStationDataChangedDelegate         OnChargingStationDataChanged;

        /// <summary>
        /// An event fired whenever the admin status of any subordinated ChargingStation changed.
        /// </summary>
        public event OnChargingStationAdminStatusChangedDelegate  OnChargingStationAdminStatusChanged;

        /// <summary>
        /// An event fired whenever the aggregated dynamic status of any subordinated charging station changed.
        /// </summary>
        public event OnChargingStationStatusChangedDelegate       OnChargingStationStatusChanged;

        #endregion

        #region OnChargingStationAdminDiff

        public delegate void OnChargingStationAdminDiffDelegate(ChargingStationAdminStatusDiff StatusDiff);

        /// <summary>
        /// An event fired whenever a charging station admin status diff was received.
        /// </summary>
        public event OnChargingStationAdminDiffDelegate OnChargingStationAdminDiff;

        #endregion

        #region ChargingStationAddition

        internal readonly IVotingNotificator<DateTime, ChargingPool, ChargingStation, Boolean> ChargingStationAddition;

        /// <summary>
        /// Called whenever a charging station will be or was added.
        /// </summary>
        public IVotingSender<DateTime, ChargingPool, ChargingStation, Boolean> OnChargingStationAddition

            => ChargingStationAddition;

        #endregion

        #region ChargingStationRemoval

        internal readonly IVotingNotificator<DateTime, ChargingPool, ChargingStation, Boolean> ChargingStationRemoval;

        /// <summary>
        /// Called whenever a charging station will be or was removed.
        /// </summary>
        public IVotingSender<DateTime, ChargingPool, ChargingStation, Boolean> OnChargingStationRemoval

            => ChargingStationRemoval;

        #endregion


        #region (internal) UpdateChargingStationData       (Timestamp, EventTrackingId, ChargingStation, PropertyName, OldValue, NewValue)

        /// <summary>
        /// Update the data of a charging station.
        /// </summary>
        /// <param name="Timestamp">The timestamp when this change was detected.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="ChargingStation">The changed charging station.</param>
        /// <param name="PropertyName">The name of the changed property.</param>
        /// <param name="OldValue">The old value of the changed property.</param>
        /// <param name="NewValue">The new value of the changed property.</param>
        internal async Task UpdateChargingStationData(DateTime          Timestamp,
                                                      EventTracking_Id  EventTrackingId,
                                                      ChargingStation   ChargingStation,
                                                      String            PropertyName,
                                                      Object            OldValue,
                                                      Object            NewValue)
        {

            //foreach (var AuthenticationService in _IeMobilityServiceProviders.
            //                                          OrderBy(AuthServiceWithPriority => AuthServiceWithPriority.Key).
            //                                          Select (AuthServiceWithPriority => AuthServiceWithPriority.Value))
            //{

            //    result = await AuthenticationService.PushEVSEStatus(new EVSEStatus(EVSE.Id, NewStatus.Value, NewStatus.Timestamp),
            //                                                        ActionType.update,
            //                                                        EVSE.Operator.Id);

            //}

            var results = _ISendData.WhenAll(iSendData => iSendData.
                                                              UpdateStaticData(ChargingStation,
                                                                               PropertyName,
                                                                               OldValue,
                                                                               NewValue));

            //foreach (var PushEVSEStatusService in _PushEVSEStatusToOperatorRoamingServices.
            //                                          OrderBy(AuthServiceWithPriority => AuthServiceWithPriority.Key).
            //                                          Select (AuthServiceWithPriority => AuthServiceWithPriority.Value))
            //{

            //    result = await PushEVSEStatusService.PushEVSEStatus(new EVSEStatus(EVSE.Id, NewStatus.Value, NewStatus.Timestamp),
            //                                                        ActionType.update,
            //                                                        EVSE.Operator.Id);

            //}

            var OnChargingStationDataChangedLocal = OnChargingStationDataChanged;
            if (OnChargingStationDataChangedLocal != null)
                await OnChargingStationDataChangedLocal(Timestamp,
                                                        EventTrackingId,
                                                        ChargingStation,
                                                        PropertyName,
                                                        OldValue,
                                                        NewValue);

        }

        #endregion

        #region (internal) UpdateChargingStationAdminStatus(Timestamp, EventTrackingId, ChargingStation, OldStatus, NewStatus)

        /// <summary>
        /// Update the current charging station admin status.
        /// </summary>
        /// <param name="Timestamp">The timestamp when this change was detected.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="ChargingStation">The updated charging station.</param>
        /// <param name="OldStatus">The old aggreagted charging station status.</param>
        /// <param name="NewStatus">The new aggreagted charging station status.</param>
        internal async Task UpdateChargingStationAdminStatus(DateTime                                      Timestamp,
                                                             EventTracking_Id                              EventTrackingId,
                                                             ChargingStation                               ChargingStation,
                                                             Timestamped<ChargingStationAdminStatusTypes>  OldStatus,
                                                             Timestamped<ChargingStationAdminStatusTypes>  NewStatus)
        {

            var OnChargingStationAdminStatusChangedLocal = OnChargingStationAdminStatusChanged;
            if (OnChargingStationAdminStatusChangedLocal != null)
                await OnChargingStationAdminStatusChangedLocal(Timestamp,
                                                               EventTrackingId,
                                                               ChargingStation,
                                                               OldStatus,
                                                               NewStatus);

        }

        #endregion

        #region (internal) UpdateChargingStationStatus     (Timestamp, EventTrackingId, ChargingStation, OldStatus, NewStatus)

        /// <summary>
        /// Update a charging pool admin status.
        /// </summary>
        /// <param name="Timestamp">The timestamp when this change was detected.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="ChargingPool">The updated charging pool.</param>
        /// <param name="OldStatus">The old aggregated charging pool status.</param>
        /// <param name="NewStatus">The new aggregated charging pool status.</param>
        internal async Task UpdateChargingStationStatus(DateTime                                 Timestamp,
                                                        EventTracking_Id                         EventTrackingId,
                                                        ChargingStation                          ChargingStation,
                                                        Timestamped<ChargingStationStatusTypes>  OldStatus,
                                                        Timestamped<ChargingStationStatusTypes>  NewStatus)
        {

            var OnChargingStationStatusChangedLocal = OnChargingStationStatusChanged;
            if (OnChargingStationStatusChangedLocal != null)
                await OnChargingStationStatusChangedLocal(Timestamp,
                                                          EventTrackingId,
                                                          ChargingStation,
                                                          OldStatus,
                                                          NewStatus);

        }

        #endregion

        #endregion

        #region EVSEs...

        #region EVSEs

        /// <summary>
        /// Return all EVSEs registered within this roaming network.
        /// </summary>
        public IEnumerable<EVSE> EVSEs

            => _ChargingStationOperators.SelectMany(cso => cso.EVSEs);

        #endregion

        #region EVSEIds                (IncludeEVSEs = null)

        /// <summary>
        /// Return all EVSE identifications registered within this roaming network.
        /// </summary>
        /// <param name="IncludeEVSEs">An optional delegate for filtering EVSEs.</param>
        public IEnumerable<EVSE_Id> EVSEIds(IncludeEVSEDelegate IncludeEVSEs = null)

            => _ChargingStationOperators.
                   SelectMany(cso => cso.EVSEIds(IncludeEVSEs));

        #endregion

        #region EVSEAdminStatus        (IncludeEVSEs = null)

        /// <summary>
        /// Return the admin status of all EVSEs registered within this roaming network.
        /// </summary>
        /// <param name="IncludeEVSEs">An optional delegate for filtering EVSEs.</param>
        public IEnumerable<EVSEAdminStatus> EVSEAdminStatus(IncludeEVSEDelegate IncludeEVSEs = null)

            => _ChargingStationOperators.
                   SelectMany(cso => cso.EVSEAdminStatus(IncludeEVSEs));

        #endregion

        #region EVSEAdminStatusSchedule(IncludeEVSEs = null)

        /// <summary>
        /// Return the admin status of all EVSEs registered within this roaming network.
        /// </summary>
        /// <param name="IncludeEVSEs">An optional delegate for filtering EVSEs.</param>
        public IEnumerable<EVSEAdminStatus> EVSEAdminStatusSchedule(IncludeEVSEDelegate IncludeEVSEs = null)

            => _ChargingStationOperators.
                   SelectMany(cso => cso.EVSEAdminStatus(IncludeEVSEs));

        #endregion

        #region EVSEStatus             (IncludeEVSEs = null)

        /// <summary>
        /// Return the status of all EVSEs registered within this roaming network.
        /// </summary>
        /// <param name="IncludeEVSEs">An optional delegate for filtering EVSEs.</param>
        public IEnumerable<EVSEStatus> EVSEStatus(IncludeEVSEDelegate IncludeEVSEs = null)

            => _ChargingStationOperators.
                   SelectMany(cso => cso.EVSEStatus(IncludeEVSEs));

        #endregion


        #region ContainsEVSE(EVSE)

        /// <summary>
        /// Check if the given EVSE is already present within the roaming network.
        /// </summary>
        /// <param name="EVSE">An EVSE.</param>
        public Boolean ContainsEVSE(EVSE EVSE)
        {

            if (TryGetChargingStationOperatorById(EVSE.Operator.Id, out ChargingStationOperator _ChargingStationOperator))
                return _ChargingStationOperator.ContainsEVSE(EVSE.Id);

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

            if (TryGetChargingStationOperatorById(EVSEId.OperatorId, out ChargingStationOperator _ChargingStationOperator))
                return _ChargingStationOperator.ContainsEVSE(EVSEId);

            return false;

        }

        #endregion

        #region GetEVSEbyId(EVSEId)

        public EVSE GetEVSEbyId(EVSE_Id EVSEId)
        {

            if (TryGetChargingStationOperatorById(EVSEId.OperatorId, out ChargingStationOperator _ChargingStationOperator) &&
                _ChargingStationOperator.TryGetEVSEById(EVSEId, out EVSE _EVSE))
                    return _EVSE;

            return null;

        }

        #endregion

        #region TryGetEVSEbyId(EVSEId, out EVSE)

        public Boolean TryGetEVSEById(EVSE_Id EVSEId, out EVSE EVSE)
        {

            if (TryGetChargingStationOperatorById(EVSEId.OperatorId, out ChargingStationOperator _ChargingStationOperator))
                return _ChargingStationOperator.TryGetEVSEById(EVSEId, out EVSE);

            EVSE = null;
            return false;

        }

        #endregion


        #region SetEVSEStatus(EVSEStatusList)

        public void SetEVSEStatus(IEnumerable<EVSEStatus> EVSEStatusList)
        {

            foreach (var evseStatus in EVSEStatusList)
            {
                if (TryGetChargingStationOperatorById(evseStatus.Id.OperatorId, out ChargingStationOperator _cso))
                    _cso.SetEVSEStatus(evseStatus);
            }

        }

        #endregion

        #region SetEVSEStatus(EVSEId, NewStatus)

        public void SetEVSEStatus(EVSE_Id                       EVSEId,
                                  Timestamped<EVSEStatusTypes>  NewStatus)
        {

            if (TryGetChargingStationOperatorById(EVSEId.OperatorId, out ChargingStationOperator _cso))
                _cso.SetEVSEStatus(EVSEId, NewStatus);

        }

        #endregion

        #region SetEVSEStatus(EVSEId, Timestamp, NewStatus)

        public void SetEVSEStatus(EVSE_Id          EVSEId,
                                  DateTime         Timestamp,
                                  EVSEStatusTypes  NewStatus)
        {

            if (TryGetChargingStationOperatorById(EVSEId.OperatorId, out ChargingStationOperator _cso))
                _cso.SetEVSEStatus(EVSEId, new Timestamped<EVSEStatusTypes>(Timestamp, NewStatus));

        }

        #endregion

        #region SetEVSEStatus(EVSEId, StatusList)

        public void SetEVSEStatus(EVSE_Id                                    EVSEId,
                                  IEnumerable<Timestamped<EVSEStatusTypes>>  StatusList,
                                  ChangeMethods                              ChangeMethod  = ChangeMethods.Replace)
        {

            if (TryGetChargingStationOperatorById(EVSEId.OperatorId, out ChargingStationOperator _cso))
                _cso.SetEVSEStatus(EVSEId, StatusList, ChangeMethod);

        }

        #endregion


        #region SetEVSEAdminStatus(EVSEId, NewStatus)

        public void SetEVSEAdminStatus(EVSE_Id                            EVSEId,
                                       Timestamped<EVSEAdminStatusTypes>  NewAdminStatus)
        {

            if (TryGetChargingStationOperatorById(EVSEId.OperatorId, out ChargingStationOperator _cso))
                _cso.SetEVSEAdminStatus(EVSEId, NewAdminStatus);

        }

        #endregion

        #region SetEVSEAdminStatus(EVSEId, Timestamp, NewAdminStatus)

        public void SetEVSEAdminStatus(EVSE_Id               EVSEId,
                                       DateTime              Timestamp,
                                       EVSEAdminStatusTypes  NewAdminStatus)
        {

            if (TryGetChargingStationOperatorById(EVSEId.OperatorId, out ChargingStationOperator _cso))
                _cso.SetEVSEAdminStatus(EVSEId, NewAdminStatus);

        }

        #endregion

        #region SetEVSEAdminStatus(EVSEId, AdminStatusList)

        public void SetEVSEAdminStatus(EVSE_Id                                         EVSEId,
                                       IEnumerable<Timestamped<EVSEAdminStatusTypes>>  AdminStatusList,
                                       ChangeMethods                                   ChangeMethod  = ChangeMethods.Replace)
        {

            if (TryGetChargingStationOperatorById(EVSEId.OperatorId, out ChargingStationOperator _cso))
                _cso.SetEVSEAdminStatus(EVSEId, AdminStatusList, ChangeMethod);

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


        #region EVSEAddition

        internal readonly IVotingNotificator<DateTime, ChargingStation, EVSE, Boolean> EVSEAddition;

        /// <summary>
        /// Called whenever an EVSE will be or was added.
        /// </summary>
        public IVotingSender<DateTime, ChargingStation, EVSE, Boolean> OnEVSEAddition

            => EVSEAddition;

        private void SendEVSEAdded(DateTime         Timestamp,
                                   ChargingStation  ChargingStation,
                                   EVSE             EVSE)
        {

            var results = _ISendData.WhenAll(iSendData => iSendData.
                                                              SetStaticData(EVSE));

        }

        #endregion

        #region EVSERemoval

        internal readonly IVotingNotificator<DateTime, ChargingStation, EVSE, Boolean> EVSERemoval;

        /// <summary>
        /// Called whenever an EVSE will be or was removed.
        /// </summary>
        public IVotingSender<DateTime, ChargingStation, EVSE, Boolean> OnEVSERemoval

            => EVSERemoval;

        private void SendEVSERemoved(DateTime         Timestamp,
                                     ChargingStation  ChargingStation,
                                     EVSE             EVSE)
        {

            var results = _ISendData.WhenAll(iSendData => iSendData.
                                                              DeleteStaticData(EVSE));

            EVSERemoval.SendNotification(Timestamp, ChargingStation, EVSE);

        }

        #endregion

        #region OnEVSEData/(Admin)StatusChanged

        /// <summary>
        /// An event fired whenever the static data of any subordinated EVSE changed.
        /// </summary>
        public event OnEVSEDataChangedDelegate         OnEVSEDataChanged;

        /// <summary>
        /// An event fired whenever the admin status of any subordinated EVSE changed.
        /// </summary>
        public event OnEVSEAdminStatusChangedDelegate  OnEVSEAdminStatusChanged;

        /// <summary>
        /// An event fired whenever the dynamic status of any subordinated EVSE changed.
        /// </summary>
        public event OnEVSEStatusChangedDelegate       OnEVSEStatusChanged;

        #endregion

        #region OnEVSEStatusDiff

        public delegate void OnEVSEStatusDiffDelegate(EVSEStatusDiff StatusDiff);

        /// <summary>
        /// An event fired whenever a EVSE status diff was received.
        /// </summary>
        public event OnEVSEStatusDiffDelegate OnEVSEStatusDiff;

        #endregion


        #region (internal) UpdateEVSEData       (Timestamp, EventTrackingId, EVSE, OldStatus, NewStatus)

        /// <summary>
        /// Update the data of an EVSE.
        /// </summary>
        /// <param name="Timestamp">The timestamp when this change was detected.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="EVSE">The changed EVSE.</param>
        /// <param name="PropertyName">The name of the changed property.</param>
        /// <param name="OldValue">The old value of the changed property.</param>
        /// <param name="NewValue">The new value of the changed property.</param>
        internal async Task UpdateEVSEData(DateTime          Timestamp,
                                           EventTracking_Id  EventTrackingId,
                                           EVSE              EVSE,
                                           String            PropertyName,
                                           Object            OldValue,
                                           Object            NewValue)
        {

            var results = _ISendData.WhenAll(iSendData => iSendData.
                                                              UpdateStaticData(EVSE,
                                                                               PropertyName,
                                                                               OldValue,
                                                                               NewValue,
                                                                               EventTrackingId: EventTrackingId));

            var OnEVSEDataChangedLocal = OnEVSEDataChanged;
            if (OnEVSEDataChangedLocal != null)
                await OnEVSEDataChangedLocal(Timestamp,
                                             EventTrackingId,
                                             EVSE,
                                             PropertyName,
                                             OldValue,
                                             NewValue);

        }

        #endregion

        #region (internal) UpdateEVSEAdminStatus(Timestamp, EventTrackingId, EVSE, OldStatus, NewStatus)

        /// <summary>
        /// Update an EVSE admin status.
        /// </summary>
        /// <param name="Timestamp">The timestamp when this change was detected.</param>
        /// <param name="EventTrackingId">An event tracking identification for correlating this request with other events.</param>
        /// <param name="EVSE">The updated EVSE.</param>
        /// <param name="OldStatus">The old EVSE status.</param>
        /// <param name="NewStatus">The new EVSE status.</param>
        internal async Task UpdateEVSEAdminStatus(DateTime                           Timestamp,
                                                  EventTracking_Id                   EventTrackingId,
                                                  EVSE                               EVSE,
                                                  Timestamped<EVSEAdminStatusTypes>  OldStatus,
                                                  Timestamped<EVSEAdminStatusTypes>  NewStatus)
        {

            var results = _ISendAdminStatus.WhenAll(iSendAdminStatus => iSendAdminStatus.
                                                                            UpdateAdminStatus(new EVSEAdminStatusUpdate[] {
                                                                                                  new EVSEAdminStatusUpdate(EVSE,
                                                                                                                            OldStatus,
                                                                                                                            NewStatus)
                                                                                              },
                                                                                              EventTrackingId: EventTrackingId));

            var OnEVSEAdminStatusChangedLocal = OnEVSEAdminStatusChanged;
            if (OnEVSEAdminStatusChangedLocal != null)
                await OnEVSEAdminStatusChangedLocal(Timestamp,
                                                    EventTrackingId,
                                                    EVSE,
                                                    OldStatus,
                                                    NewStatus);

        }

        #endregion

        #region (internal) UpdateEVSEStatus     (Timestamp, EventTrackingId, EVSE, OldStatus, NewStatus)

        /// <summary>
        /// Update an EVSE status.
        /// </summary>
        /// <param name="Timestamp">The timestamp when this change was detected.</param>
        /// <param name="EventTrackingId">An event tracking identification for correlating this request with other events.</param>
        /// <param name="EVSE">The updated EVSE.</param>
        /// <param name="OldStatus">The old EVSE status.</param>
        /// <param name="NewStatus">The new EVSE status.</param>
        internal async Task UpdateEVSEStatus(DateTime                      Timestamp,
                                             EventTracking_Id              EventTrackingId,
                                             EVSE                          EVSE,
                                             Timestamped<EVSEStatusTypes>  OldStatus,
                                             Timestamped<EVSEStatusTypes>  NewStatus)
        {

            var results = _ISendStatus.WhenAll(iSendStatus => iSendStatus.
                                                                  UpdateStatus(new EVSEStatusUpdate[] {
                                                                                   new EVSEStatusUpdate(EVSE,
                                                                                                        OldStatus,
                                                                                                        NewStatus)
                                                                               },
                                                                               EventTrackingId: EventTrackingId));

            var OnEVSEStatusChangedLocal = OnEVSEStatusChanged;
            if (OnEVSEStatusChangedLocal != null)
                await OnEVSEStatusChangedLocal(Timestamp,
                                               EventTrackingId,
                                               EVSE,
                                               OldStatus,
                                               NewStatus);

        }

        #endregion

        #endregion


        public static readonly TimeSpan MaxReservationDuration = TimeSpan.FromMinutes(15);

        #region Reservations...

        #region Data

        public IEnumerable<ChargingReservation> ChargingReservations
            => ReservationsStore.
                   Select(reservation => reservation.Last());

        public Boolean TryGetChargingReservationById(ChargingReservation_Id Id, out ChargingReservation ChargingReservation)
        {

            if (ReservationsStore.TryGet(Id, out ChargingReservationCollection ReservationCollection))
            {
                ChargingReservation = ReservationCollection.Last();
                return true;
            }

            ChargingReservation = null;
            return false;

        }

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

        #region Reserve(ChargingLocation, ReservationLevel = EVSE, StartTime = null, Duration = null, ReservationId = null, ProviderId = null, ...)

        /// <summary>
        /// Reserve the possibility to charge at the given charging location.
        /// </summary>
        /// <param name="ChargingLocation">A charging location.</param>
        /// <param name="ReservationLevel">The level of the reservation to create (EVSE, charging station, ...).</param>
        /// <param name="ReservationStartTime">The starting time of the reservation.</param>
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
                    DateTime?                         ReservationStartTime   = null,
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


            ReservationResult result = null;

            #endregion

            #region Send OnReserveRequest event

            var StartTime = DateTime.UtcNow;

            try
            {

                OnReserveRequest?.Invoke(StartTime,
                                         Timestamp.Value,
                                         this,
                                         EventTrackingId,
                                         Id,
                                         ReservationId,
                                         ChargingLocation,
                                         ReservationStartTime,
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
                e.Log(nameof(RoamingNetwork) + "." + nameof(OnReserveRequest));
            }

            #endregion


            try
            {

                var EVSEId = ChargingLocation.EVSEId.Value;

                if (TryGetChargingStationOperatorById(EVSEId.OperatorId, out ChargingStationOperator _ChargingStationOperator))
                {

                    result = await _ChargingStationOperator.
                                       Reserve(ChargingLocation,
                                               ReservationLevel,
                                               ReservationStartTime,
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

                    if (result.Result == ReservationResultType.Success)
                    {
                        if (result.Reservation != null)
                        {
                            result.Reservation.ChargingStationOperatorId = _ChargingStationOperator.Id;
                            ReservationsStore.NewOrUpdate(result.Reservation);
                        }
                    }

                }

                if (result        == null ||
                   (result        != null &&
                   (result.Result == ReservationResultType.UnknownLocation)))
                {

                    foreach (var EMPRoamingService in _EMPRoamingProviders.
                                                          OrderBy(EMPRoamingServiceWithPriority => EMPRoamingServiceWithPriority.Key).
                                                          Select (EMPRoamingServiceWithPriority => EMPRoamingServiceWithPriority.Value))
                    {

                        result = await EMPRoamingService.
                                           Reserve(ChargingLocation,
                                                   ReservationLevel,
                                                   ReservationStartTime,
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


                        if (result.Result == ReservationResultType.Success)
                        {
                            if (result.Reservation != null)
                            {
                                result.Reservation.EMPRoamingProviderId = EMPRoamingService.Id;
                                ReservationsStore.NewOrUpdate(result.Reservation);
                            }
                        }

                    }

                }

                if (result == null)
                    result = ReservationResult.UnknownChargingStationOperator;


            }
            catch (Exception e)
            {
                result = ReservationResult.Error(e.Message);
            }


            #region Send OnReserveResponse event

            var EndTime = DateTime.UtcNow;

            try
            {

                OnReserveResponse?.Invoke(EndTime,
                                          Timestamp.Value,
                                          this,
                                          EventTrackingId,
                                          Id,
                                          ReservationId,
                                          ChargingLocation,
                                          ReservationStartTime,
                                          Duration,
                                          ProviderId,
                                          RemoteAuthentication,
                                          ChargingProduct,
                                          AuthTokens,
                                          eMAIds,
                                          PINs,
                                          result,
                                          EndTime - StartTime,
                                          RequestTimeout);

            }
            catch (Exception e)
            {
                e.Log(nameof(RoamingNetwork) + "." + nameof(OnReserveResponse));
            }

            #endregion

            return result;

        }

        #endregion

        #region CancelReservation(ReservationId, Reason, ...)

        /// <summary>
        /// Try to remove the given charging reservation.
        /// </summary>
        /// <param name="ReservationId">The unique charging reservation identification.</param>
        /// <param name="Reason">A reason for this cancellation.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public async Task<CancelReservationResult>

            CancelReservation(ChargingReservation_Id                 ReservationId,
                              ChargingReservationCancellationReason  Reason,

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


            ChargingReservation     canceledReservation  = null;
            CancelReservationResult result               = null;

            #endregion

            #region Send OnCancelReservationRequest event

            var StartTime = DateTime.UtcNow;

            try
            {

                OnCancelReservationRequest?.Invoke(StartTime,
                                                   Timestamp.Value,
                                                   this,
                                                   EventTrackingId,
                                                   Id,
                                                   ReservationId,
                                                   Reason,
                                                   RequestTimeout);


            }
            catch (Exception e)
            {
                e.Log(nameof(RoamingNetwork) + "." + nameof(OnCancelReservationRequest));
            }

            #endregion


            try
            {

                if (ReservationsStore.TryGetLatest(ReservationId, out ChargingReservation Reservation))
                {

                    #region Check Charging Station Operator charging reservation lookup...

                    if (Reservation.ChargingStationOperatorId.HasValue &&
                        TryGetChargingStationOperatorById(Reservation.ChargingStationOperatorId.Value, out ChargingStationOperator _ChargingStationOperator))
                    {

                        result = await _ChargingStationOperator.
                                           CancelReservation(ReservationId,
                                                             Reason,

                                                             Timestamp,
                                                             CancellationToken,
                                                             EventTrackingId,
                                                             RequestTimeout);

                    }

                    #endregion

                    #region ...or check EMP roaming provider charging reservation lookup...

                    if (result == null ||
                       (result != null &&
                       (result.Result == CancelReservationResultTypes.UnknownEVSE ||
                        result.Result == CancelReservationResultTypes.UnknownReservationId)))
                    {


                        if (Reservation.EMPRoamingProviderId.HasValue &&
                            TryGet(Reservation.EMPRoamingProviderId.Value, out IEMPRoamingProvider _IEMPRoamingProvider))
                        {

                            result = await _IEMPRoamingProvider.
                                               CancelReservation(ReservationId,
                                                                 Reason,

                                                                 Timestamp,
                                                                 CancellationToken,
                                                                 EventTrackingId,
                                                                 RequestTimeout);

                        }

                    }

                    #endregion

                    #region ...or try to check every EMP roaming provider...

                    if (result == null ||
                       (result != null &&
                       (result.Result == CancelReservationResultTypes.UnknownEVSE ||
                        result.Result == CancelReservationResultTypes.UnknownReservationId)))
                    {

                        foreach (var EMPRoamingService in _EMPRoamingProviders.
                                                              OrderBy(EMPRoamingServiceWithPriority => EMPRoamingServiceWithPriority.Key).
                                                              Select(EMPRoamingServiceWithPriority => EMPRoamingServiceWithPriority.Value))
                        {

                            result = await EMPRoamingService.
                                               CancelReservation(ReservationId,
                                                                 Reason,

                                                                 Timestamp,
                                                                 CancellationToken,
                                                                 EventTrackingId,
                                                                 RequestTimeout);

                        }

                    }

                    #endregion

                    #region ...or fail!

                    if (result == null)
                    {

                        result = CancelReservationResult.UnknownReservationId(ReservationId,
                                                                              Reason);

                        //SendReservationCanceled(DateTime.UtcNow,
                        //                        this,
                        //                           EventTrackingId,
                        //                           Id,
                        //                           ProviderId,
                        //                           ReservationId,
                        //                           null,
                        //                           Reason,
                        //                           result,
                        //                           result.Runtime.HasValue
                        //                               ? result.Runtime.Value
                        //                               : TimeSpan.FromMilliseconds(5),
                        //                           RequestTimeout);

                    }

                    #endregion

                }

                else
                    result = CancelReservationResult.UnknownReservationId(ReservationId,
                                                                          Reason);

            }
            catch (Exception e)
            {
                result = CancelReservationResult.Error(ReservationId,
                                                       Reason,
                                                       e.Message);
            }


            #region Send OnCancelReservationResponse event

            var EndTime = DateTime.UtcNow;

            try
            {

                OnCancelReservationResponse?.Invoke(EndTime,
                                                    Timestamp.Value,
                                                    this,
                                                    EventTrackingId,
                                                    Id,
                                                    ReservationId,
                                                    canceledReservation,
                                                    Reason,
                                                    result,
                                                    EndTime - StartTime,
                                                    RequestTimeout);

            }
            catch (Exception e)
            {
                e.Log(nameof(RoamingNetwork) + "." + nameof(OnCancelReservationResponse));
            }

            #endregion

            return result;

        }

        #endregion


        #region (internal) SendNewReservation     (Timestamp, Sender, Reservation)

        internal void SendNewReservation(DateTime             Timestamp,
                                         Object               Sender,
                                         ChargingReservation  Reservation)
        {

            OnNewReservation?.Invoke(Timestamp, Sender, Reservation);

        }

        #endregion

        #region (internal) SendReservationCanceled(Timestamp, Sender, Reservation, Reason)

        internal void SendReservationCanceled(DateTime                               Timestamp,
                                              Object                                 Sender,
                                              ChargingReservation                    Reservation,
                                              ChargingReservationCancellationReason  Reason)
        {

            OnReservationCanceled?.Invoke(Timestamp, Sender, Reservation, Reason);

        }

        #endregion

        #endregion

        #region RemoteStart/-Stop

        #region Data

        public IEnumerable<ChargingSession> ChargingSessions
            => SessionsStore;

        /// <summary>
        /// Return the charging session specified by the given identification.
        /// </summary>
        /// <param name="SessionId">The charging session identification.</param>
        /// <param name="ChargingSession">The charging session.</param>
        public Boolean TryGetChargingSessionById(ChargingSession_Id SessionId, out ChargingSession ChargingSession)
            => SessionsStore.TryGet(SessionId, out ChargingSession);

        #endregion

        #region Events

        /// <summary>
        /// An event fired whenever a remote start EVSE command was received.
        /// </summary>
        public event OnRemoteStartRequestDelegate              OnRemoteStartRequest;

        /// <summary>
        /// An event fired whenever a remote start EVSE command completed.
        /// </summary>
        public event OnRemoteStartResponseDelegate             OnRemoteStartResponse;

        /// <summary>
        /// An event fired whenever a new charging session was created.
        /// </summary>
        /// <summary>
        /// An event fired whenever a new charging session was created.
        /// </summary>
        public event OnNewChargingSessionDelegate OnNewChargingSession
        {
            add
            {
                SessionsStore.OnNewChargingSession += value;
            }
            remove
            {
                SessionsStore.OnNewChargingSession -= value;
            }
        }


        /// <summary>
        /// An event fired whenever a remote stop command was received.
        /// </summary>
        public event OnRemoteStopRequestDelegate               OnRemoteStopRequest;

        /// <summary>
        /// An event fired whenever a remote stop command completed.
        /// </summary>
        public event OnRemoteStopResponseDelegate              OnRemoteStopResponse;

        /// <summary>
        /// An event fired whenever a new charge detail record was created.
        /// </summary>
        public event OnNewChargeDetailRecordDelegate OnNewChargeDetailRecord
        {
            add
            {
                ChargeDetailRecordsStore.OnNewChargeDetailRecord += value;
            }
            remove
            {
                ChargeDetailRecordsStore.OnNewChargeDetailRecord -= value;
            }
        }

        #endregion

        #region RemoteStart(ChargingLocation, ChargingProduct = null, ReservationId = null, SessionId = null, ProviderId = null, RemoteAuthentication = null, ...)

        /// <summary>
        /// Start a charging session at the given charging location.
        /// </summary>
        /// <param name="ChargingLocation">The charging location.</param>
        /// <param name="ChargingProduct">The choosen charging product.</param>
        /// <param name="ReservationId">Use the given optinal unique charging reservation identification for charging.</param>
        /// <param name="SessionId">The unique identification for this charging session.</param>
        /// <param name="ProviderId">The unique identification of the e-mobility service provider for the case it is different from the current message sender.</param>
        /// <param name="RemoteAuthentication">The unique identification of the e-mobility account.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public Task<RemoteStartResult>

            RemoteStart(ChargingLocation          ChargingLocation,
                        ChargingProduct           ChargingProduct            = null,
                        ChargingReservation_Id?   ReservationId              = null,
                        ChargingSession_Id?       SessionId                  = null,
                        eMobilityProvider_Id?     ProviderId                 = null,
                        RemoteAuthentication      RemoteAuthentication       = null,

                        DateTime?                 Timestamp                  = null,
                        CancellationToken?        CancellationToken          = null,
                        EventTracking_Id          EventTrackingId            = null,
                        TimeSpan?                 RequestTimeout             = null)

            => RemoteStart(null,
                           ChargingLocation,
                           ChargingProduct,
                           ReservationId,
                           SessionId,
                           ProviderId,
                           RemoteAuthentication,

                           Timestamp,
                           CancellationToken,
                           EventTrackingId,
                           RequestTimeout);


        /// <summary>
        /// Start a charging session at the given charging location.
        /// </summary>
        /// <param name="ChargingLocation">The charging location.</param>
        /// <param name="ChargingProduct">The choosen charging product.</param>
        /// <param name="ReservationId">Use the given optinal unique charging reservation identification for charging.</param>
        /// <param name="SessionId">The unique identification for this charging session.</param>
        /// <param name="ProviderId">The unique identification of the e-mobility service provider for the case it is different from the current message sender.</param>
        /// <param name="RemoteAuthentication">The unique identification of the e-mobility account.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public async Task<RemoteStartResult>

            RemoteStart(ICSORoamingProvider       CSORoamingProvider,
                        ChargingLocation          ChargingLocation,
                        ChargingProduct           ChargingProduct            = null,
                        ChargingReservation_Id?   ReservationId              = null,
                        ChargingSession_Id?       SessionId                  = null,
                        eMobilityProvider_Id?     ProviderId                 = null,
                        RemoteAuthentication      RemoteAuthentication       = null,

                        DateTime?                 Timestamp                  = null,
                        CancellationToken?        CancellationToken          = null,
                        EventTracking_Id          EventTrackingId            = null,
                        TimeSpan?                 RequestTimeout             = null)

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

            #region Send OnRemoteStartRequest event

            var StartTime = DateTime.UtcNow;

            try
            {

                OnRemoteStartRequest?.Invoke(StartTime,
                                             Timestamp.Value,
                                             this,
                                             EventTrackingId,
                                             Id,
                                             ChargingLocation,
                                             ChargingProduct,
                                             ReservationId,
                                             SessionId,
                                             ProviderId,
                                             RemoteAuthentication,
                                             RequestTimeout);

            }
            catch (Exception e)
            {
                e.Log(nameof(RoamingNetwork) + "." + nameof(OnRemoteStartRequest));
            }

            #endregion


            try
            {

                if (ChargingLocation.IsNull())
                {
                    result = RemoteStartResult.UnknownLocation;
                }

                else if (AdminStatus.Value == RoamingNetworkAdminStatusTypes.Operational ||
                         AdminStatus.Value == RoamingNetworkAdminStatusTypes.InternalUse)
                {

                    #region Try to lookup the charging station operator given in the EVSE identification...

                    if (TryGetChargingStationOperatorById(ChargingLocation.ChargingStationOperatorId,     out ChargingStationOperator chargingStationOperator) ||
                        TryGetChargingStationOperatorById(ChargingLocation.EVSEId?.           OperatorId, out                         chargingStationOperator) ||
                        TryGetChargingStationOperatorById(ChargingLocation.ChargingStationId?.OperatorId, out                         chargingStationOperator) ||
                        TryGetChargingStationOperatorById(ChargingLocation.ChargingPoolId?.   OperatorId, out                         chargingStationOperator))
                    {

                        result = await chargingStationOperator.
                                           RemoteStart(ChargingLocation,
                                                       ChargingProduct,
                                                       ReservationId,
                                                       SessionId,
                                                       ProviderId,
                                                       RemoteAuthentication,

                                                       Timestamp,
                                                       CancellationToken,
                                                       EventTrackingId,
                                                       RequestTimeout);


                        if (result.Result == RemoteStartResultType.Success)
                        {

                            if (CSORoamingProvider != null)
                                SessionsStore.NewOrUpdate(result.Session,
                                                          session => {
                                                              session.CSORoamingProvider = CSORoamingProvider;
                                                          });

                        }

                    }

                    #endregion

                    //ToDo: Add routing!
                    #region ...or try every EMP roaming provider...

                    if (result        == null ||
                       (result        != null &&
                       (result.Result == RemoteStartResultType.UnknownLocation)))
                    {

                        foreach (var empRoamingProvider in _EMPRoamingProviders.
                                                                OrderBy(EMPRoamingServiceWithPriority => EMPRoamingServiceWithPriority.Key).
                                                                Select (EMPRoamingServiceWithPriority => EMPRoamingServiceWithPriority.Value))
                        {

                            result = await empRoamingProvider.
                                               RemoteStart(ChargingLocation,
                                                           ChargingProduct,
                                                           ReservationId,
                                                           SessionId,
                                                           ProviderId,
                                                           RemoteAuthentication,

                                                           Timestamp,
                                                           CancellationToken,
                                                           EventTrackingId,
                                                           RequestTimeout);


                            if (result.Result == RemoteStartResultType.Success)
                                SessionsStore.NewOrUpdate(result.Session, session => { session.EMPRoamingProvider = empRoamingProvider; });
                                                                        //   SetISendChargeDetailRecords(ISendChargeDetailRecords));

                        }

                    }

                    #endregion

                    #region ...or fail!

                    if (result == null)
                        result = RemoteStartResult.UnknownOperator;

                    #endregion

                }
                else
                {

                    switch (AdminStatus.Value)
                    {

                        default:
                            result = RemoteStartResult.OutOfService;
                            break;

                    }

                }

            }
            catch (Exception e)
            {
                result = RemoteStartResult.Error(e.Message);
            }


            #region Send OnRemoteStartResponse event

            var EndTime = DateTime.UtcNow;

            try
            {

                OnRemoteStartResponse?.Invoke(EndTime,
                                              Timestamp.Value,
                                              this,
                                              EventTrackingId,
                                              Id,
                                              ChargingLocation,
                                              ChargingProduct,
                                              ReservationId,
                                              SessionId,
                                              ProviderId,
                                              RemoteAuthentication,
                                              RequestTimeout,
                                              result,
                                              EndTime - StartTime);

            }
            catch (Exception e)
            {
                e.Log(nameof(RoamingNetwork) + "." + nameof(OnRemoteStartResponse));
            }

            #endregion

            return result;

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


            RemoteStopResult result = null;

            #endregion

            #region Send OnRemoteStopRequest event

            var StartTime = DateTime.UtcNow;

            try
            {

                OnRemoteStopRequest?.Invoke(StartTime,
                                            Timestamp.Value,
                                            this,
                                            EventTrackingId,
                                            Id,
                                            SessionId,
                                            ReservationHandling,
                                            ProviderId,
                                            RemoteAuthentication,
                                            RequestTimeout);

            }
            catch (Exception e)
            {
                e.Log(nameof(RoamingNetwork) + "." + nameof(OnRemoteStopRequest));
            }

            #endregion

            try
            {

                #region Check charging station operator charging session lookup...

                if (SessionsStore.TryGet(SessionId, out ChargingSession chargingSession))
                {

                    if (chargingSession.SessionTime.EndTime.HasValue)
                        result = RemoteStopResult.AlreadyStopped(SessionId,
                                                                 chargingSession.ReservationId);

                    else
                    {

                        if (chargingSession.ChargingStationOperator != null)
                            result = await chargingSession.ChargingStationOperator.
                                               RemoteStop(SessionId,
                                                          ReservationHandling,
                                                          ProviderId,
                                                          RemoteAuthentication,

                                                          Timestamp,
                                                          CancellationToken,
                                                          EventTrackingId,
                                                          RequestTimeout);

                        if (result == null && chargingSession.EMPRoamingProvider != null)
                            result = await chargingSession.EMPRoamingProvider.
                                               RemoteStop(SessionId,
                                                          ReservationHandling,
                                                          ProviderId,
                                                          RemoteAuthentication,

                                                          Timestamp,
                                                          CancellationToken,
                                                          EventTrackingId,
                                                          RequestTimeout);

                    }

                }

                #endregion

                #region ...or try to check every charging station operator...

                if (result        == null ||
                   (result        != null &&
                   (result.Result == RemoteStopResultType.InvalidSessionId)))
                {

                    foreach (var chargingStationOperator in _ChargingStationOperators)
                    {

                        result = await chargingStationOperator.
                                           RemoteStop(SessionId,
                                                      ReservationHandling,
                                                      ProviderId,
                                                      RemoteAuthentication,

                                                      Timestamp,
                                                      CancellationToken,
                                                      EventTrackingId,
                                                      RequestTimeout);

                    }

                }

                #endregion

                #region ...or try to check every EMP roaming provider...

                if (result        == null ||
                   (result        != null &&
                   (result.Result == RemoteStopResultType.InvalidSessionId)))
                {

                    foreach (var empRoamingProvider in _EMPRoamingProviders.
                                                           OrderBy(EMPRoamingServiceWithPriority => EMPRoamingServiceWithPriority.Key).
                                                           Select(EMPRoamingServiceWithPriority => EMPRoamingServiceWithPriority.Value))
                    {

                        result = await empRoamingProvider.
                                           RemoteStop(SessionId,
                                                      ReservationHandling,
                                                      ProviderId,
                                                      RemoteAuthentication,

                                                      Timestamp,
                                                      CancellationToken,
                                                      EventTrackingId,
                                                      RequestTimeout);

                    }

                }

                #endregion

                #region ...or fail!

                if (result == null)
                    result = RemoteStopResult.InvalidSessionId(SessionId);

                #endregion

            }
            catch (Exception e)
            {
                result = RemoteStopResult.Error(SessionId,
                                                e.Message);
            }


            #region Send OnRemoteStopResponse event

            var EndTime = DateTime.UtcNow;

            try
            {

                OnRemoteStopResponse?.Invoke(EndTime,
                                             Timestamp.Value,
                                             this,
                                             EventTrackingId,
                                             Id,
                                             SessionId,
                                             ReservationHandling,
                                             ProviderId,
                                             RemoteAuthentication,
                                             RequestTimeout,
                                             result,
                                             EndTime - StartTime);

            }
            catch (Exception e)
            {
                e.Log(nameof(RoamingNetwork) + "." + nameof(OnRemoteStopResponse));
            }

            #endregion


            if (result.ChargeDetailRecord != null)
                await SendChargeDetailRecord(result.ChargeDetailRecord);


            return result;

        }

        #endregion


        #region (internal) SendNewChargingSession   (Timestamp, Sender, Session)

        internal void SendNewChargingSession(DateTime         Timestamp,
                                             Object           Sender,
                                             ChargingSession  Session)
        {

            if (Session != null)
            {

                if (Session.RoamingNetwork == null)
                {
                    Session.RoamingNetwork    = this;
                    Session.RoamingNetworkId  = Id;
                }

                //OnNewChargingSession?.Invoke(Timestamp, Sender, Session);
                //SessionsStore.SendNewChargingSession(Timestamp, Sender, Session);

            }

        }

        #endregion

        #region (internal) SendNewChargeDetailRecord(Timestamp, Sender, ChargeDetailRecord)

        internal void SendNewChargeDetailRecord(DateTime            Timestamp,
                                                Object              Sender,
                                                ChargeDetailRecord  ChargeDetailRecord)
        {

            if (ChargeDetailRecord != null)
                SendChargeDetailRecords(new ChargeDetailRecord[] { ChargeDetailRecord },
                                        TransmissionTypes.Enqueue,

                                        Timestamp,
                                        new CancellationTokenSource().Token,
                                        EventTracking_Id.New,
                                        TimeSpan.FromMinutes(1)).

                                        Wait(TimeSpan.FromMinutes(1));

        }

        #endregion

        #endregion

        #region AuthorizeStart/-Stop

        #region AuthorizeStart(LocalAuthentication,                    ChargingProduct = null, SessionId = null, OperatorId = null, ...)

        /// <summary>
        /// Create an authorize start request.
        /// </summary>
        /// <param name="LocalAuthentication">An user identification.</param>
        /// <param name="ChargingProduct">An optional charging product.</param>
        /// <param name="SessionId">An optional session identification.</param>
        /// <param name="OperatorId">An optional charging station operator identification.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public async Task<AuthStartResult>

            AuthorizeStart(LocalAuthentication          LocalAuthentication,
                           ChargingProduct              ChargingProduct     = null,
                           ChargingSession_Id?          SessionId           = null,
                           ChargingStationOperator_Id?  OperatorId          = null,

                           DateTime?                    Timestamp           = null,
                           CancellationToken?           CancellationToken   = null,
                           EventTracking_Id             EventTrackingId     = null,
                           TimeSpan?                    RequestTimeout      = null)

        {

            #region Initial checks

            if (OperatorId == null)
                throw new ArgumentNullException(nameof(OperatorId),  "The given charging station operator must not be null!");

            if (LocalAuthentication  == null)
                throw new ArgumentNullException(nameof(LocalAuthentication),   "The given authentication token must not be null!");

            if (!Timestamp.HasValue)
                Timestamp = DateTime.UtcNow;

            if (!CancellationToken.HasValue)
                CancellationToken = new CancellationTokenSource().Token;

            if (EventTrackingId == null)
                EventTrackingId = EventTracking_Id.New;

            #endregion

            #region Send OnAuthorizeStartRequest event

            var StartTime = DateTime.UtcNow;

            try
            {

                OnAuthorizeStartRequest?.Invoke(StartTime,
                                                Timestamp.Value,
                                                this,
                                                Id.ToString(),
                                                EventTrackingId,
                                                Id,
                                                OperatorId,
                                                LocalAuthentication,
                                                ChargingProduct,
                                                SessionId,
                                                RequestTimeout);

            }
            catch (Exception e)
            {
                e.Log(nameof(RoamingNetwork) + "." + nameof(OnAuthorizeStartRequest));
            }

            #endregion


            var result = await _ISend2RemoteAuthorizeStartStop.
                                   WhenFirst(iRemoteAuthorizeStartStop => iRemoteAuthorizeStartStop.
                                                                              AuthorizeStart(LocalAuthentication,
                                                                                             ChargingProduct,
                                                                                             SessionId,
                                                                                             OperatorId,

                                                                                             Timestamp,
                                                                                             CancellationToken,
                                                                                             EventTrackingId,
                                                                                             RequestTimeout),

                                             result2 => result2.Result == AuthStartResultType.Authorized ||
                                                        result2.Result == AuthStartResultType.Blocked,

                                             RequestTimeout ?? this.RequestTimeout,

                                             runtime => AuthStartResult.NotAuthorized(Id,
                                                                                      this,
                                                                                      SessionId,
                                                                                      Description:  "No authorization service returned a positiv result!",
                                                                                      Runtime:      runtime)).ConfigureAwait(false);


            #region If Authorized...

            if (result.Result == AuthStartResultType.Authorized)
            {

                if (result.SessionId.HasValue)
                {

                    // Store the upstream session id in order to contact the right authenticator at later requests!
                    // Will be deleted when the charge detail record was sent!

                    var NewChargingSession = new ChargingSession(result.SessionId.Value) {
                                                 AuthorizatorId             = result.AuthorizatorId,
                                                 ProviderIdStart            = result.ProviderId,
                                                 AuthService                = result.ISendAuthorizeStartStop,
                                                 ChargingStationOperatorId  = OperatorId,
                                                 AuthenticationStart        = LocalAuthentication,
                                                 ChargingProduct            = ChargingProduct
                                             };

                    SessionsStore.NewOrUpdate(NewChargingSession);

                }

                else
                    DebugX.Log("AuthStart response without charging session identification!");

            }

            #endregion


            #region Send OnAuthorizeStartResponse event

            var Endtime = DateTime.UtcNow;

            try
            {

                OnAuthorizeStartResponse?.Invoke(Endtime,
                                                 Timestamp.Value,
                                                 this,
                                                 Id.ToString(),
                                                 EventTrackingId,
                                                 Id,
                                                 OperatorId,
                                                 LocalAuthentication,
                                                 ChargingProduct,
                                                 SessionId,
                                                 RequestTimeout,
                                                 result,
                                                 Endtime - StartTime);

            }
            catch (Exception e)
            {
                e.Log(nameof(RoamingNetwork) + "." + nameof(OnAuthorizeStartResponse));
            }

            #endregion

            return result;

        }

        #endregion

        #region AuthorizeStart(LocalAuthentication, EVSEId,            ChargingProduct = null, SessionId = null, OperatorId = null, ...)

        /// <summary>
        /// Create an authorize start request at the given EVSE.
        /// </summary>
        /// <param name="LocalAuthentication">An user identification.</param>
        /// <param name="EVSEId">The unique identification of an EVSE.</param>
        /// <param name="ChargingProduct">An optional charging product.</param>
        /// <param name="SessionId">An optional session identification.</param>
        /// <param name="OperatorId">An optional charging station operator identification.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public async Task<AuthStartEVSEResult>

            AuthorizeStart(LocalAuthentication          LocalAuthentication,
                           EVSE_Id                      EVSEId,
                           ChargingProduct              ChargingProduct     = null,
                           ChargingSession_Id?          SessionId           = null,
                           ChargingStationOperator_Id?  OperatorId          = null,

                           DateTime?                    Timestamp           = null,
                           CancellationToken?           CancellationToken   = null,
                           EventTracking_Id             EventTrackingId     = null,
                           TimeSpan?                    RequestTimeout      = null)

        {

            #region Initial checks

            if (LocalAuthentication  == null)
                throw new ArgumentNullException(nameof(LocalAuthentication),   "The given authentication token must not be null!");


            if (!Timestamp.HasValue)
                Timestamp = DateTime.UtcNow;

            if (!CancellationToken.HasValue)
                CancellationToken = new CancellationTokenSource().Token;

            if (EventTrackingId == null)
                EventTrackingId = EventTracking_Id.New;

            #endregion

            #region Send OnAuthorizeEVSEStartRequest event

            var StartTime = DateTime.UtcNow;

            try
            {

                OnAuthorizeEVSEStartRequest?.Invoke(StartTime,
                                                    Timestamp.Value,
                                                    this,
                                                    Id.ToString(),
                                                    EventTrackingId,
                                                    Id,
                                                    OperatorId,
                                                    LocalAuthentication,
                                                    EVSEId,
                                                    ChargingProduct,
                                                    SessionId,
                                                    _ISend2RemoteAuthorizeStartStop,
                                                    RequestTimeout);

            }
            catch (Exception e)
            {
                e.Log(nameof(RoamingNetwork) + "." + nameof(OnAuthorizeEVSEStartRequest));
            }

            #endregion


            DebugX.LogT("RN AuthStart Request: " + LocalAuthentication);
            DebugX.LogT(_ISend2RemoteAuthorizeStartStop.Select(_ => _.AuthId).AggregateWith(", "));

            var result = await _ISend2RemoteAuthorizeStartStop.
                                   WhenFirst(iRemoteAuthorizeStartStop => iRemoteAuthorizeStartStop.
                                                                              AuthorizeStart(LocalAuthentication,
                                                                                             EVSEId,
                                                                                             ChargingProduct,
                                                                                             SessionId,
                                                                                             OperatorId,

                                                                                             Timestamp,
                                                                                             CancellationToken,
                                                                                             EventTrackingId,
                                                                                             RequestTimeout),

                                             result2 => result2.Result == AuthStartEVSEResultType.Authorized ||
                                                        result2.Result == AuthStartEVSEResultType.Blocked,

                                             RequestTimeout ?? this.RequestTimeout,

                                             runtime => AuthStartEVSEResult.NotAuthorized(Id,
                                                                                          this,
                                                                                          SessionId,
                                                                                          Description:  "No authorization service returned a positiv result!",
                                                                                          Runtime:      runtime));


            DebugX.LogT("RN AuthStart Response: " + result?.ISendAuthorizeStartStop?.   AuthId?.ToString() ??
                                                    result?.IReceiveAuthorizeStartStop?.AuthId?.ToString() ??
                                                    ""  +
                                                    ": " + LocalAuthentication + " => " + result);

            #region If Authorized...

            if (result.Result == AuthStartEVSEResultType.Authorized)
            {

                if (result.SessionId.HasValue)
                {

                    // Store the upstream session id in order to contact the right authenticator at later requests!
                    // Will be deleted when the charge detail record was sent!

                    var NewChargingSession = new ChargingSession(result.SessionId.Value) {
                                                 AuthorizatorId             = result.AuthorizatorId,
                                                 ProviderIdStart            = result.ProviderId,
                                                 AuthService                = result.ISendAuthorizeStartStop,
                                                 ChargingStationOperatorId  = OperatorId,
                                                 EVSEId                     = EVSEId,
                                                 AuthenticationStart        = LocalAuthentication,
                                                 ChargingProduct            = ChargingProduct,
                                                 //ISendChargeDetailRecords   = result.ISendChargeDetailRecords
                                             };

                    SessionsStore.NewOrUpdate(NewChargingSession);

                }

                else
                    DebugX.Log("AuthStart response without charging session identification!");

            }

            #endregion


            #region Send OnAuthorizeEVSEStartResponse event

            var Endtime = DateTime.UtcNow;

            try
            {

                OnAuthorizeEVSEStartResponse?.Invoke(Endtime,
                                                     Timestamp.Value,
                                                     this,
                                                     Id.ToString(),
                                                     EventTrackingId,
                                                     Id,
                                                     OperatorId,
                                                     LocalAuthentication,
                                                     EVSEId,
                                                     ChargingProduct,
                                                     SessionId,
                                                     _ISend2RemoteAuthorizeStartStop,
                                                     RequestTimeout,
                                                     result,
                                                     Endtime - StartTime);

            }
            catch (Exception e)
            {
                e.Log(nameof(RoamingNetwork) + "." + nameof(OnAuthorizeEVSEStartResponse));
            }

            #endregion

            return result;

        }

        #endregion

        #region AuthorizeStart(LocalAuthentication, ChargingStationId, ChargingProduct = null, SessionId = null, OperatorId = null, ...)

        /// <summary>
        /// Create an authorize start request at the given charging station.
        /// </summary>
        /// <param name="LocalAuthentication">An user identification.</param>
        /// <param name="ChargingStationId">The unique identification of a charging station.</param>
        /// <param name="ChargingProduct">An optional charging product.</param>
        /// <param name="SessionId">An optional session identification.</param>
        /// <param name="OperatorId">An optional charging station operator identification.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public async Task<AuthStartChargingStationResult>

            AuthorizeStart(LocalAuthentication          LocalAuthentication,
                           ChargingStation_Id           ChargingStationId,
                           ChargingProduct              ChargingProduct     = null,
                           ChargingSession_Id?          SessionId           = null,
                           ChargingStationOperator_Id?  OperatorId          = null,

                           DateTime?                    Timestamp           = null,
                           CancellationToken?           CancellationToken   = null,
                           EventTracking_Id             EventTrackingId     = null,
                           TimeSpan?                    RequestTimeout      = null)

        {

            #region Initial checks

            if (LocalAuthentication == null)
                throw new ArgumentNullException(nameof(LocalAuthentication), "The given authentication token must not be null!");


            if (!Timestamp.HasValue)
                Timestamp = DateTime.UtcNow;

            if (!CancellationToken.HasValue)
                CancellationToken = new CancellationTokenSource().Token;

            if (EventTrackingId == null)
                EventTrackingId = EventTracking_Id.New;

            #endregion

            #region Send OnAuthorizeChargingStationStartRequest event

            var StartTime = DateTime.UtcNow;

            try
            {

                OnAuthorizeChargingStationStartRequest?.Invoke(StartTime,
                                                               Timestamp.Value,
                                                               this,
                                                               Id.ToString(),
                                                               EventTrackingId,
                                                               Id,
                                                               OperatorId,
                                                               LocalAuthentication,
                                                               ChargingStationId,
                                                               ChargingProduct,
                                                               SessionId,
                                                               RequestTimeout);

            }
            catch (Exception e)
            {
                e.Log(nameof(RoamingNetwork) + "." + nameof(OnAuthorizeChargingStationStartRequest));
            }

            #endregion


            var result = await _ISend2RemoteAuthorizeStartStop.
                                   WhenFirst(iRemoteAuthorizeStartStop => iRemoteAuthorizeStartStop.
                                                                              AuthorizeStart(LocalAuthentication,
                                                                                             ChargingStationId,
                                                                                             ChargingProduct,
                                                                                             SessionId,
                                                                                             OperatorId,

                                                                                             Timestamp,
                                                                                             CancellationToken,
                                                                                             EventTrackingId,
                                                                                             RequestTimeout),

                                             result2 => result2.Result == AuthStartChargingStationResultType.Authorized ||
                                                        result2.Result == AuthStartChargingStationResultType.Blocked,

                                             RequestTimeout ?? this.RequestTimeout,

                                             runtime => AuthStartChargingStationResult.NotAuthorized(Id,
                                                                                                     this,
                                                                                                     SessionId,
                                                                                                     Description:  "No authorization service returned a positiv result!",
                                                                                                     Runtime:      runtime)).

                                   ConfigureAwait(false);


            #region If Authorized...

            if (result.Result == AuthStartChargingStationResultType.Authorized)
            {

                if (result.SessionId.HasValue)
                {

                    // Store the upstream session id in order to contact the right authenticator at later requests!
                    // Will be deleted when the charge detail record was sent!

                    var NewChargingSession = new ChargingSession(result.SessionId.Value) {
                                                 AuthorizatorId             = result.AuthorizatorId,
                                                 ProviderIdStart            = result.ProviderId,
                                                 AuthService                = result.ISendAuthorizeStartStop,
                                                 ChargingStationOperatorId  = OperatorId,
                                                 ChargingStationId          = ChargingStationId,
                                                 AuthenticationStart        = LocalAuthentication,
                                                 ChargingProduct            = ChargingProduct
                                             };

                    SessionsStore.NewOrUpdate(NewChargingSession);

                }

            }

            #endregion


            #region Send OnAuthorizeChargingStationStarted event

            var Endtime = DateTime.UtcNow;

            try
            {

                OnAuthorizeChargingStationStartResponse?.Invoke(Endtime,
                                                                Timestamp.Value,
                                                                this,
                                                                Id.ToString(),
                                                                EventTrackingId,
                                                                Id,
                                                                OperatorId,
                                                                LocalAuthentication,
                                                                ChargingStationId,
                                                                ChargingProduct,
                                                                SessionId,
                                                                RequestTimeout,
                                                                result,
                                                                Endtime - StartTime);

            }
            catch (Exception e)
            {
                e.Log(nameof(RoamingNetwork) + "." + nameof(OnAuthorizeChargingStationStartResponse));
            }

            #endregion

            return result;

        }

        #endregion

        #region AuthorizeStart(LocalAuthentication, ChargingPoolId,    ChargingProduct = null, SessionId = null, OperatorId = null, ...)

        /// <summary>
        /// Create an authorize start request at the given charging station.
        /// </summary>
        /// <param name="LocalAuthentication">An user identification.</param>
        /// <param name="ChargingPoolId">The unique identification of a charging pool.</param>
        /// <param name="ChargingProduct">An optional charging product.</param>
        /// <param name="SessionId">An optional session identification.</param>
        /// <param name="OperatorId">An optional charging station operator identification.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public async Task<AuthStartChargingPoolResult>

            AuthorizeStart(LocalAuthentication          LocalAuthentication,
                           ChargingPool_Id              ChargingPoolId,
                           ChargingProduct              ChargingProduct     = null,
                           ChargingSession_Id?          SessionId           = null,
                           ChargingStationOperator_Id?  OperatorId          = null,

                           DateTime?                    Timestamp           = null,
                           CancellationToken?           CancellationToken   = null,
                           EventTracking_Id             EventTrackingId     = null,
                           TimeSpan?                    RequestTimeout      = null)

        {

            #region Initial checks

            if (LocalAuthentication == null)
                throw new ArgumentNullException(nameof(LocalAuthentication), "The given authentication token must not be null!");


            if (!Timestamp.HasValue)
                Timestamp = DateTime.UtcNow;

            if (!CancellationToken.HasValue)
                CancellationToken = new CancellationTokenSource().Token;

            if (EventTrackingId == null)
                EventTrackingId = EventTracking_Id.New;


            //AuthStartChargingPoolResult result = null;

            #endregion

            #region Send OnAuthorizeChargingPoolStartRequest event

            var StartTime = DateTime.UtcNow;

            try
            {

                OnAuthorizeChargingPoolStartRequest?.Invoke(StartTime,
                                                            Timestamp.Value,
                                                            this,
                                                            Id.ToString(),
                                                            EventTrackingId,
                                                            Id,
                                                            OperatorId,
                                                            LocalAuthentication,
                                                            ChargingPoolId,
                                                            ChargingProduct,
                                                            SessionId,
                                                            RequestTimeout);

            }
            catch (Exception e)
            {
                e.Log(nameof(RoamingNetwork) + "." + nameof(OnAuthorizeChargingPoolStartRequest));
            }

            #endregion


            var result = await _ISend2RemoteAuthorizeStartStop.
                                   WhenFirst(iRemoteAuthorizeStartStop => iRemoteAuthorizeStartStop.
                                                                              AuthorizeStart(LocalAuthentication,
                                                                                             ChargingPoolId,
                                                                                             ChargingProduct,
                                                                                             SessionId,
                                                                                             OperatorId,

                                                                                             Timestamp,
                                                                                             CancellationToken,
                                                                                             EventTrackingId,
                                                                                             RequestTimeout),

                                             result2 => result2.Result == AuthStartChargingPoolResultType.Authorized ||
                                                        result2.Result == AuthStartChargingPoolResultType.Blocked,

                                             RequestTimeout ?? this.RequestTimeout,

                                             runtime => AuthStartChargingPoolResult.NotAuthorized(Id,
                                                                                                  this,
                                                                                                  SessionId,
                                                                                                  Description:  "No authorization service returned a positiv result!",
                                                                                                  Runtime:      runtime)).

                                   ConfigureAwait(false);


            #region If Authorized...

            if (result.Result == AuthStartChargingPoolResultType.Authorized)
            {

                if (result.SessionId.HasValue)
                {

                    // Store the upstream session id in order to contact the right authenticator at later requests!
                    // Will be deleted when the charge detail record was sent!

                    var NewChargingSession = new ChargingSession(result.SessionId.Value) {
                                                 AuthorizatorId             = result.AuthorizatorId,
                                                 ProviderIdStart            = result.ProviderId,
                                                 AuthService                = result.ISendAuthorizeStartStop,
                                                 ChargingStationOperatorId  = OperatorId,
                                                 ChargingPoolId             = ChargingPoolId,
                                                 AuthenticationStart        = LocalAuthentication,
                                                 ChargingProduct            = ChargingProduct
                                             };

                    SessionsStore.NewOrUpdate(NewChargingSession);

                }

            }

            #endregion


            #region Send OnAuthorizeChargingPoolStartResponse event

            var Endtime = DateTime.UtcNow;

            try
            {

                OnAuthorizeChargingPoolStartResponse?.Invoke(Endtime,
                                                             Timestamp.Value,
                                                             this,
                                                             Id.ToString(),
                                                             EventTrackingId,
                                                             Id,
                                                             OperatorId,
                                                             LocalAuthentication,
                                                             ChargingPoolId,
                                                             ChargingProduct,
                                                             SessionId,
                                                             RequestTimeout,
                                                             result,
                                                             Endtime - StartTime);

            }
            catch (Exception e)
            {
                e.Log(nameof(RoamingNetwork) + "." + nameof(OnAuthorizeChargingPoolStartResponse));
            }

            #endregion

            return result;

        }

        #endregion

        // AuthorizeStart(AuthToken, EVSEGroupId,            ...
        // AuthorizeStart(AuthToken, ChargingStationGroupId, ...
        // AuthorizeStart(AuthToken, ChargingPoolGroupId,    ...


        #region OnAuthorizeStartRequest/-Response

        /// <summary>
        /// An event fired whenever an authorize start command was received.
        /// </summary>
        public event OnAuthorizeStartRequestDelegate   OnAuthorizeStartRequest;

        /// <summary>
        /// An event fired whenever an authorize start command completed.
        /// </summary>
        public event OnAuthorizeStartResponseDelegate  OnAuthorizeStartResponse;

        #endregion

        #region OnAuthorizeEVSEStartRequest/-Response

        /// <summary>
        /// An event fired whenever an authorize start EVSE command was received.
        /// </summary>
        public event OnAuthorizeEVSEStartRequestDelegate   OnAuthorizeEVSEStartRequest;

        /// <summary>
        /// An event fired whenever an authorize start EVSE command completed.
        /// </summary>
        public event OnAuthorizeEVSEStartResponseDelegate  OnAuthorizeEVSEStartResponse;

        #endregion

        #region OnAuthorizeChargingStationStartRequest/-Response

        /// <summary>
        /// An event fired whenever an authorize start charging station command was received.
        /// </summary>
        public event OnAuthorizeChargingStationStartRequestDelegate   OnAuthorizeChargingStationStartRequest;

        /// <summary>
        /// An event fired whenever an authorize start charging station command completed.
        /// </summary>
        public event OnAuthorizeChargingStationStartResponseDelegate  OnAuthorizeChargingStationStartResponse;

        #endregion

        #region OnAuthorizeChargingPoolStartRequest/-Response

        /// <summary>
        /// An event fired whenever an authorize start charging pool command was received.
        /// </summary>
        public event OnAuthorizeChargingPoolStartRequestDelegate OnAuthorizeChargingPoolStartRequest;

        /// <summary>
        /// An event fired whenever an authorize start charging pool command completed.
        /// </summary>
        public event OnAuthorizeChargingPoolStartResponseDelegate OnAuthorizeChargingPoolStartResponse;

        #endregion



        #region AuthorizeStop(SessionId, LocalAuthentication,                    OperatorId = null, ...)

        /// <summary>
        /// Create an authorize stop request.
        /// </summary>
        /// <param name="SessionId">The session identification from the AuthorizeStart request.</param>
        /// <param name="LocalAuthentication">An user identification.</param>
        /// <param name="OperatorId">An optional charging station operator identification.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public async Task<AuthStopResult>

            AuthorizeStop(ChargingSession_Id           SessionId,
                          LocalAuthentication          LocalAuthentication,
                          ChargingStationOperator_Id?  OperatorId          = null,

                          DateTime?                    Timestamp           = null,
                          CancellationToken?           CancellationToken   = null,
                          EventTracking_Id             EventTrackingId     = null,
                          TimeSpan?                    RequestTimeout      = null)

        {

            #region Initial checks

            if (LocalAuthentication == null)
                throw new ArgumentNullException(nameof(LocalAuthentication), "The given parameter must not be null!");


            if (!Timestamp.HasValue)
                Timestamp = DateTime.UtcNow;

            if (!CancellationToken.HasValue)
                CancellationToken = new CancellationTokenSource().Token;

            if (EventTrackingId == null)
                EventTrackingId = EventTracking_Id.New;


            AuthStopResult result = null;

            #endregion

            #region Send OnAuthorizeStopRequest event

            var StartTime = DateTime.UtcNow;

            try
            {

                OnAuthorizeStopRequest?.Invoke(StartTime,
                                               Timestamp.Value,
                                               this,
                                               Id.ToString(),
                                               EventTrackingId,
                                               Id,
                                               OperatorId,
                                               SessionId,
                                               LocalAuthentication,
                                               RequestTimeout);

            }
            catch (Exception e)
            {
                e.Log(nameof(RoamingNetwork) + "." + nameof(OnAuthorizeStopRequest));
            }

            #endregion


            #region An authenticator was found for the upstream SessionId!

            ChargingSession _ChargingSession = null;

            //if (_ChargingSessions.TryGetValue(SessionId, out _ChargingSession) &&
            //    _ChargingSession.AuthService != null)
            //{
            //    result = await _ChargingSession.AuthService.AuthorizeStop(SessionId,
            //                                                                         AuthToken,
            //                                                                         OperatorId,

            //                                                                         Timestamp,
            //                                                                         CancellationToken,
            //                                                                         EventTrackingId,
            //                                                                         RequestTimeout);

            //}

            #endregion

            #region Try to find anyone who might kown anything about the given SessionId!

            if (result == null || result.Result != AuthStopResultType.Authorized)
                result = await _ISend2RemoteAuthorizeStartStop.
                                   WhenFirst(iRemoteAuthorizeStartStop => iRemoteAuthorizeStartStop.
                                                                              AuthorizeStop(SessionId,
                                                                                            LocalAuthentication,
                                                                                            OperatorId,

                                                                                            Timestamp,
                                                                                            CancellationToken,
                                                                                            EventTrackingId,
                                                                                            RequestTimeout),

                                             result2 => result2.Result == AuthStopResultType.Authorized ||
                                                        result2.Result == AuthStopResultType.Blocked,

                                             RequestTimeout ?? this.RequestTimeout,

                                             runtime => AuthStopResult.NotAuthorized(Id,
                                                                                     this,
                                                                                     SessionId,
                                                                                     Description:  "No authorization service returned a positiv result!",
                                                                                     Runtime:      runtime)).

                                   ConfigureAwait(false);

            #endregion


            #region Send OnAuthorizeStopResponse event

            var Endtime = DateTime.UtcNow;

            try
            {

                OnAuthorizeStopResponse?.Invoke(Endtime,
                                                Timestamp.Value,
                                                this,
                                                Id.ToString(),
                                                EventTrackingId,
                                                Id,
                                                OperatorId,
                                                SessionId,
                                                LocalAuthentication,
                                                RequestTimeout,
                                                result,
                                                Endtime - StartTime);

            }
            catch (Exception e)
            {
                e.Log(nameof(RoamingNetwork) + "." + nameof(OnAuthorizeStopResponse));
            }

            #endregion

            return result;

        }

        #endregion

        #region AuthorizeStop(SessionId, LocalAuthentication, EVSEId,            OperatorId = null, ...)

        /// <summary>
        /// Create an authorize stop request at the given EVSE.
        /// </summary>
        /// <param name="SessionId">The session identification from the AuthorizeStart request.</param>
        /// <param name="LocalAuthentication">An user identification.</param>
        /// <param name="EVSEId">The unique identification of an EVSE.</param>
        /// <param name="OperatorId">An optional charging station operator identification.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public async Task<AuthStopEVSEResult>

            AuthorizeStop(ChargingSession_Id           SessionId,
                          LocalAuthentication          LocalAuthentication,
                          EVSE_Id                      EVSEId,
                          ChargingStationOperator_Id?  OperatorId          = null,

                          DateTime?                    Timestamp           = null,
                          CancellationToken?           CancellationToken   = null,
                          EventTracking_Id             EventTrackingId     = null,
                          TimeSpan?                    RequestTimeout      = null)

        {

            #region Initial checks

            if (LocalAuthentication == null)
                throw new ArgumentNullException(nameof(LocalAuthentication), "The given parameter must not be null!");


            if (!Timestamp.HasValue)
                Timestamp = DateTime.UtcNow;

            if (!CancellationToken.HasValue)
                CancellationToken = new CancellationTokenSource().Token;

            if (EventTrackingId == null)
                EventTrackingId = EventTracking_Id.New;


            AuthStopEVSEResult result = null;

            #endregion

            #region Send OnAuthorizeEVSEStopRequest event

            var StartTime = DateTime.UtcNow;

            try
            {

                OnAuthorizeEVSEStopRequest?.Invoke(StartTime,
                                                   Timestamp.Value,
                                                   this,
                                                   Id.ToString(),
                                                   EventTrackingId,
                                                   Id,
                                                   OperatorId,
                                                   EVSEId,
                                                   SessionId,
                                                   LocalAuthentication,
                                                   RequestTimeout);

            }
            catch (Exception e)
            {
                e.Log(nameof(RoamingNetwork) + "." + nameof(OnAuthorizeEVSEStopRequest));
            }

            #endregion


            #region An authenticator was found for the upstream SessionId!

            if (SessionsStore.TryGet(SessionId, out ChargingSession _ChargingSession) &&
                _ChargingSession. AuthService != null)
            {

                result = await _ChargingSession.AuthService.AuthorizeStop(SessionId,
                                                                          LocalAuthentication,
                                                                          EVSEId,
                                                                          OperatorId,

                                                                          Timestamp,
                                                                          CancellationToken,
                                                                          EventTrackingId,
                                                                          RequestTimeout);

            }

            #endregion

            else
                result = await _ISend2RemoteAuthorizeStartStop.
                                   WhenFirst(iRemoteAuthorizeStartStop => iRemoteAuthorizeStartStop.
                                                                              AuthorizeStop(SessionId,
                                                                                            LocalAuthentication,
                                                                                            EVSEId,
                                                                                            OperatorId,

                                                                                            Timestamp,
                                                                                            CancellationToken,
                                                                                            EventTrackingId,
                                                                                            RequestTimeout),

                                             result2 => result2.Result == AuthStopEVSEResultType.Authorized ||
                                                        result2.Result == AuthStopEVSEResultType.Blocked,

                                             RequestTimeout ?? this.RequestTimeout,

                                             runtime => AuthStopEVSEResult.NotAuthorized(Id,
                                                                                         this,
                                                                                         SessionId,
                                                                                         Description:  "No authorization service returned a positiv result!",
                                                                                         Runtime:      runtime)).

                                   ConfigureAwait(false);


            if (result.Result == AuthStopEVSEResultType.Authorized)
                SessionsStore.Remove(SessionId,
                                     LocalAuthentication);


            #region Send OnAuthorizeEVSEStopResponse event

            var Endtime = DateTime.UtcNow;

            try
            {

                OnAuthorizeEVSEStopResponse?.Invoke(Endtime,
                                                    Timestamp.Value,
                                                    this,
                                                    Id.ToString(),
                                                    EventTrackingId,
                                                    Id,
                                                    OperatorId,
                                                    EVSEId,
                                                    SessionId,
                                                    LocalAuthentication,
                                                    RequestTimeout,
                                                    result,
                                                    Endtime - StartTime);

            }
            catch (Exception e)
            {
                e.Log(nameof(RoamingNetwork) + "." + nameof(OnAuthorizeEVSEStopResponse));
            }

            #endregion

            return result;

        }

        #endregion

        #region AuthorizeStop(SessionId, LocalAuthentication, ChargingStationId, OperatorId = null, ...)

        /// <summary>
        /// Create an authorize stop request at the given charging station.
        /// </summary>
        /// <param name="SessionId">The session identification from the AuthorizeStart request.</param>
        /// <param name="LocalAuthentication">An user identification.</param>
        /// <param name="ChargingStationId">The unique identification of a charging station.</param>
        /// <param name="OperatorId">An optional charging station operator identification.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public async Task<AuthStopChargingStationResult>

            AuthorizeStop(ChargingSession_Id           SessionId,
                          LocalAuthentication          LocalAuthentication,
                          ChargingStation_Id           ChargingStationId,
                          ChargingStationOperator_Id?  OperatorId          = null,

                          DateTime?                    Timestamp           = null,
                          CancellationToken?           CancellationToken   = null,
                          EventTracking_Id             EventTrackingId     = null,
                          TimeSpan?                    RequestTimeout      = null)

        {

            #region Initial checks

            if (LocalAuthentication == null)
                throw new ArgumentNullException(nameof(LocalAuthentication),  "The given parameter must not be null!");

            if (ChargingStationId  == null)
                throw new ArgumentNullException(nameof(ChargingStationId),   "The given parameter must not be null!");


            if (!Timestamp.HasValue)
                Timestamp = DateTime.UtcNow;

            if (!CancellationToken.HasValue)
                CancellationToken = new CancellationTokenSource().Token;

            if (EventTrackingId == null)
                EventTrackingId = EventTracking_Id.New;


            AuthStopChargingStationResult result = null;

            #endregion

            #region Send OnAuthorizeChargingStationStopRequest event

            var StartTime = DateTime.UtcNow;

            try
            {

                OnAuthorizeChargingStationStopRequest?.Invoke(StartTime,
                                                              Timestamp.Value,
                                                              this,
                                                              Id.ToString(),
                                                              EventTrackingId,
                                                              Id,
                                                              OperatorId,
                                                              ChargingStationId,
                                                              SessionId,
                                                              LocalAuthentication,
                                                              RequestTimeout);

            }
            catch (Exception e)
            {
                e.Log(nameof(RoamingNetwork) + "." + nameof(OnAuthorizeChargingStationStopRequest));
            }

            #endregion


            #region An authenticator was found for the upstream SessionId!

            ChargingSession _ChargingSession = null;

            //if (_ChargingSessions.TryGetValue(SessionId, out _ChargingSession))
            //{

            //    if (_ChargingSession.AuthService != null)
            //        result = await _ChargingSession.AuthService.           AuthorizeStop(Timestamp,
            //                                                                             CancellationToken,
            //                                                                             EventTrackingId,
            //                                                                             OperatorId,
            //                                                                             ChargingStationId,
            //                                                                             SessionId,
            //                                                                             AuthToken,
            //                                                                             RequestTimeout);

            //    else if (_ChargingSession.OperatorRoamingService != null)
            //        result = await _ChargingSession.OperatorRoamingService.AuthorizeStop(Timestamp,
            //                                                                             CancellationToken,
            //                                                                             EventTrackingId,
            //                                                                             OperatorId,
            //                                                                             ChargingStationId,
            //                                                                             SessionId,
            //                                                                             AuthToken,
            //                                                                             RequestTimeout);

            //}

            #endregion

            #region Try to find anyone who might kown anything about the given SessionId!

            if (result == null || result.Result != AuthStopChargingStationResultType.Authorized)
                result = await _ISend2RemoteAuthorizeStartStop.
                                   WhenFirst(iRemoteAuthorizeStartStop => iRemoteAuthorizeStartStop.
                                                                              AuthorizeStop(SessionId,
                                                                                            LocalAuthentication,
                                                                                            ChargingStationId,
                                                                                            OperatorId,

                                                                                            Timestamp,
                                                                                            CancellationToken,
                                                                                            EventTrackingId,
                                                                                            RequestTimeout),

                                             result2 => result2.Result == AuthStopChargingStationResultType.Authorized ||
                                                        result2.Result == AuthStopChargingStationResultType.Blocked,

                                             RequestTimeout ?? this.RequestTimeout,

                                             runtime => AuthStopChargingStationResult.NotAuthorized(Id,
                                                                                                    this,
                                                                                                    SessionId,
                                                                                                    Description:  "No authorization service returned a positiv result!",
                                                                                                    Runtime:      runtime)).

                                   ConfigureAwait(false);


            //if (result == null || result.Result != AuthStopChargingStationResultType.Authorized)
            //    foreach (var iRemoteAuthorizeStartStop in _ISend2RemoteAuthorizeStartStop.
            //                                              OrderBy(kvp => kvp.Key).
            //                                              Select (kvp => kvp.Value))
            //    {

            //        result = await iRemoteAuthorizeStartStop.AuthorizeStop(SessionId,
            //                                                               AuthToken,
            //                                                               ChargingStationId,
            //                                                               OperatorId,

            //                                                               Timestamp,
            //                                                               CancellationToken,
            //                                                               EventTrackingId,
            //                                                               RequestTimeout);

            //        if (result.Result == AuthStopChargingStationResultType.Authorized)
            //            break;

            //    }

            #endregion


            var Endtime = DateTime.UtcNow;
            var Runtime = Endtime - StartTime;

            #region Send OnAuthorizeChargingStationStopResponse event

            try
            {

                OnAuthorizeChargingStationStopResponse?.Invoke(Endtime,
                                                               Timestamp.Value,
                                                               this,
                                                               Id.ToString(),
                                                               EventTrackingId,
                                                               Id,
                                                               OperatorId,
                                                               ChargingStationId,
                                                               SessionId,
                                                               LocalAuthentication,
                                                               RequestTimeout,
                                                               result,
                                                               Runtime);

            }
            catch (Exception e)
            {
                e.Log(nameof(RoamingNetwork) + "." + nameof(OnAuthorizeChargingStationStopResponse));
            }

            #endregion

            return result;

        }

        #endregion

        #region AuthorizeStop(SessionId, LocalAuthentication, ChargingPoolId,    OperatorId = null, ...)

        /// <summary>
        /// Create an authorize stop request at the given charging station.
        /// </summary>
        /// <param name="SessionId">The session identification from the AuthorizeStart request.</param>
        /// <param name="LocalAuthentication">An user identification.</param>
        /// <param name="ChargingPoolId">The unique identification of a charging pool.</param>
        /// <param name="OperatorId">An optional charging station operator identification.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public async Task<AuthStopChargingPoolResult>

            AuthorizeStop(ChargingSession_Id           SessionId,
                          LocalAuthentication          LocalAuthentication,
                          ChargingPool_Id              ChargingPoolId,
                          ChargingStationOperator_Id?  OperatorId          = null,

                          DateTime?                    Timestamp           = null,
                          CancellationToken?           CancellationToken   = null,
                          EventTracking_Id             EventTrackingId     = null,
                          TimeSpan?                    RequestTimeout      = null)

        {

            #region Initial checks

            if (LocalAuthentication == null)
                throw new ArgumentNullException(nameof(LocalAuthentication),  "The given parameter must not be null!");

            if (ChargingPoolId     == null)
                throw new ArgumentNullException(nameof(ChargingPoolId),      "The given parameter must not be null!");


            if (!Timestamp.HasValue)
                Timestamp = DateTime.UtcNow;

            if (!CancellationToken.HasValue)
                CancellationToken = new CancellationTokenSource().Token;

            if (EventTrackingId == null)
                EventTrackingId = EventTracking_Id.New;


            AuthStopChargingPoolResult result = null;

            #endregion

            #region Send OnAuthorizeChargingPoolStopRequest event

            var StartTime = DateTime.UtcNow;

            try
            {

                OnAuthorizeChargingPoolStopRequest?.Invoke(StartTime,
                                                           Timestamp.Value,
                                                           this,
                                                           Id.ToString(),
                                                           EventTrackingId,
                                                           Id,
                                                           OperatorId,
                                                           ChargingPoolId,
                                                           SessionId,
                                                           LocalAuthentication,
                                                           RequestTimeout);

            }
            catch (Exception e)
            {
                e.Log(nameof(RoamingNetwork) + "." + nameof(OnAuthorizeChargingPoolStopRequest));
            }

            #endregion


            #region An authenticator was found for the upstream SessionId!

            ChargingSession _ChargingSession = null;

            //if (_ChargingSessions.TryGetValue(SessionId, out _ChargingSession))
            //{

            //    if (_ChargingSession.AuthService != null)
            //        result = await _ChargingSession.AuthService.           AuthorizeStop(Timestamp,
            //                                                                             CancellationToken,
            //                                                                             EventTrackingId,
            //                                                                             OperatorId,
            //                                                                             ChargingPoolId,
            //                                                                             SessionId,
            //                                                                             AuthToken,
            //                                                                             RequestTimeout);

            //    else if (_ChargingSession.OperatorRoamingService != null)
            //        result = await _ChargingSession.OperatorRoamingService.AuthorizeStop(Timestamp,
            //                                                                             CancellationToken,
            //                                                                             EventTrackingId,
            //                                                                             OperatorId,
            //                                                                             ChargingPoolId,
            //                                                                             SessionId,
            //                                                                             AuthToken,
            //                                                                             RequestTimeout);

            //}

            #endregion

            #region Try to find anyone who might kown anything about the given SessionId!

            if (result == null || result.Result != AuthStopChargingPoolResultType.Authorized)
                result = await _ISend2RemoteAuthorizeStartStop.
                                   WhenFirst(iRemoteAuthorizeStartStop => iRemoteAuthorizeStartStop.
                                                                              AuthorizeStop(SessionId,
                                                                                            LocalAuthentication,
                                                                                            ChargingPoolId,
                                                                                            OperatorId,

                                                                                            Timestamp,
                                                                                            CancellationToken,
                                                                                            EventTrackingId,
                                                                                            RequestTimeout),

                                             result2 => result2.Result == AuthStopChargingPoolResultType.Authorized ||
                                                        result2.Result == AuthStopChargingPoolResultType.Blocked,

                                             RequestTimeout ?? this.RequestTimeout,

                                             runtime => AuthStopChargingPoolResult.NotAuthorized(Id,
                                                                                                 this,
                                                                                                 SessionId,
                                                                                                 Description:  "No authorization service returned a positiv result!",
                                                                                                 Runtime:      runtime)).

                                   ConfigureAwait(false);


            //if (result == null || result.Result != AuthStopChargingPoolResultType.Authorized)
            //    foreach (var iRemoteAuthorizeStartStop in _ISend2RemoteAuthorizeStartStop.
            //                                              OrderBy(kvp => kvp.Key).
            //                                              Select (kvp => kvp.Value))
            //    {

            //        result = await iRemoteAuthorizeStartStop.AuthorizeStop(SessionId,
            //                                                               AuthToken,
            //                                                               ChargingPoolId,
            //                                                               OperatorId,

            //                                                               Timestamp,
            //                                                               CancellationToken,
            //                                                               EventTrackingId,
            //                                                               RequestTimeout);

            //        if (result.Result == AuthStopChargingPoolResultType.Authorized)
            //            break;

            //    }

            #endregion


            #region Send OnAuthorizeChargingPoolStopResponse event

            var Endtime = DateTime.UtcNow;

            try
            {

                OnAuthorizeChargingPoolStopResponse?.Invoke(Endtime,
                                                            Timestamp.Value,
                                                            this,
                                                            Id.ToString(),
                                                            EventTrackingId,
                                                            Id,
                                                            OperatorId,
                                                            ChargingPoolId,
                                                            SessionId,
                                                            LocalAuthentication,
                                                            RequestTimeout,
                                                            result,
                                                            Endtime - StartTime);

            }
            catch (Exception e)
            {
                e.Log(nameof(RoamingNetwork) + "." + nameof(OnAuthorizeChargingPoolStopResponse));
            }

            #endregion

            return result;

        }

        #endregion


        #region OnAuthorizeStopRequest/-Response

        /// <summary>
        /// An event fired whenever an authorize stop command was received.
        /// </summary>
        public event OnAuthorizeStopRequestDelegate   OnAuthorizeStopRequest;

        /// <summary>
        /// An event fired whenever an authorize stop command completed.
        /// </summary>
        public event OnAuthorizeStopResponseDelegate  OnAuthorizeStopResponse;

        #endregion

        #region OnAuthorizeEVSEStopRequest/-Response

        /// <summary>
        /// An event fired whenever an authorize stop EVSE command was received.
        /// </summary>
        public event OnAuthorizeEVSEStopRequestDelegate   OnAuthorizeEVSEStopRequest;

        /// <summary>
        /// An event fired whenever an authorize stop EVSE command completed.
        /// </summary>
        public event OnAuthorizeEVSEStopResponseDelegate  OnAuthorizeEVSEStopResponse;

        #endregion

        #region OnAuthorizeChargingStationStopRequest/-Response

        /// <summary>
        /// An event fired whenever an authorize stop charging station command was received.
        /// </summary>
        public event OnAuthorizeChargingStationStopRequestDelegate   OnAuthorizeChargingStationStopRequest;

        /// <summary>
        /// An event fired whenever an authorize stop charging station command completed.
        /// </summary>
        public event OnAuthorizeChargingStationStopResponseDelegate  OnAuthorizeChargingStationStopResponse;

        #endregion

        #region OnAuthorizeChargingPoolStopRequest/-Response

        /// <summary>
        /// An event fired whenever an authorize stop charging pool command was received.
        /// </summary>
        public event OnAuthorizeChargingPoolStopRequestDelegate   OnAuthorizeChargingPoolStopRequest;

        /// <summary>
        /// An event fired whenever an authorize stop charging pool command completed.
        /// </summary>
        public event OnAuthorizeChargingPoolStopResponseDelegate  OnAuthorizeChargingPoolStopResponse;

        #endregion

        #endregion

        #region Charging Sessions

        #region RegisterExternalChargingSession(Timestamp, Sender, ChargingSession)

        /// <summary>
        /// Register an external charging session which was not registered
        /// via the RemoteStart or AuthStart mechanisms.
        /// </summary>
        /// <param name="Timestamp">The timestamp of the request.</param>
        /// <param name="Sender">The sender of the charging session.</param>
        /// <param name="ChargingSession">The charging session.</param>
        public void RegisterExternalChargingSession(DateTime         Timestamp,
                                                    Object           Sender,
                                                    ChargingSession  ChargingSession)
        {
            //SessionsStore.RegisterExternalChargingSession(Timestamp, Sender, ChargingSession);
        }

        #endregion

        #region RemoveExternalChargingSession(Timestamp, Sender, ChargingSession)

        /// <summary>
        /// Register an external charging session which was not registered
        /// via the RemoteStart or AuthStart mechanisms.
        /// </summary>
        /// <param name="Timestamp">The timestamp of the request.</param>
        /// <param name="Sender">The sender of the charging session.</param>
        /// <param name="ChargingSession">The charging session.</param>
        public void RemoveExternalChargingSession(DateTime         Timestamp,
                                                  Object           Sender,
                                                  ChargingSession  ChargingSession)
        {
            //SessionsStore.RemoveExternalChargingSession(Timestamp, Sender, ChargingSession);
        }

        #endregion

        #endregion

        #region Charge Detail Records

        #region ChargeDetailRecords

        /// <summary>
        /// Return all current charge detail records.
        /// </summary>
        public IEnumerable<IEnumerable<ChargeDetailRecord>> ChargeDetailRecords
            => ChargeDetailRecordsStore;

        #endregion

        #region OnSendCDRRequest/-Response

        /// <summary>
        /// An event fired whenever a charge detail record will be send upstream.
        /// </summary>
        public event OnSendCDRRequestDelegate   OnSendCDRsRequest;

        /// <summary>
        /// An event fired whenever a charge detail record had been sent upstream.
        /// </summary>
        public event OnSendCDRResponseDelegate  OnSendCDRsResponse;

        #endregion

        #region OnFilterCDRRecords

        public delegate IEnumerable<String> OnFilterCDRRecordsDelegate(IId AuthorizatorId, ChargeDetailRecord ChargeDetailRecord);

        /// <summary>
        /// An event fired whenever a CDR needs to be filtered.
        /// </summary>
        public event OnFilterCDRRecordsDelegate OnFilterCDRRecords;

        #endregion

        #region OnCDRWasFiltered

        public delegate Task OnCDRWasFilteredDelegate(ChargeDetailRecord ChargeDetailRecord, IEnumerable<String> Reason);

        /// <summary>
        /// An event fired whenever a CDR was filtered.
        /// </summary>
        public event OnCDRWasFilteredDelegate OnCDRWasFiltered;

        #endregion

        #region IReceiveChargeDetailRecords.SendChargeDetailRecords(ChargeDetailRecords, ...)

        /// <summary>
        /// Send a charge detail record.
        /// </summary>
        /// <param name="ChargeDetailRecords">An enumeration of charge detail records.</param>
        /// 
        /// <param name="Timestamp">The timestamp of the request.</param>
        /// <param name="CancellationToken">A token to cancel this request.</param>
        /// <param name="EventTrackingId">An unique event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        Task<SendCDRsResult>

            IReceiveChargeDetailRecords.SendChargeDetailRecords(IEnumerable<ChargeDetailRecord>  ChargeDetailRecords,

                                                                DateTime?                        Timestamp,
                                                                CancellationToken?               CancellationToken,
                                                                EventTracking_Id                 EventTrackingId,
                                                                TimeSpan?                        RequestTimeout)

                => SendChargeDetailRecords(ChargeDetailRecords,
                                           TransmissionTypes.Enqueue,

                                           Timestamp,
                                           CancellationToken,
                                           EventTrackingId,
                                           RequestTimeout);

        #endregion

        #region SendChargeDetailRecord (ChargeDetailRecord,  ...)

        /// <summary>
        /// Send a charge detail record.
        /// </summary>
        /// <param name="ChargeDetailRecord">A charge detail record.</param>
        /// <param name="TransmissionType">Whether to send the charge detail record directly or enqueue it for a while.</param>
        /// 
        /// <param name="Timestamp">The timestamp of the request.</param>
        /// <param name="CancellationToken">A token to cancel this request.</param>
        /// <param name="EventTrackingId">An unique event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public Task<SendCDRsResult>

            SendChargeDetailRecord(ChargeDetailRecord  ChargeDetailRecord,
                                   TransmissionTypes   TransmissionType    = TransmissionTypes.Enqueue,

                                   DateTime?           Timestamp           = null,
                                   CancellationToken?  CancellationToken   = null,
                                   EventTracking_Id    EventTrackingId     = null,
                                   TimeSpan?           RequestTimeout      = null)


                => SendChargeDetailRecords(new ChargeDetailRecord[] { ChargeDetailRecord },
                                           TransmissionType,

                                           Timestamp,
                                           CancellationToken,
                                           EventTrackingId,
                                           RequestTimeout);

        #endregion

        #region SendChargeDetailRecords(ChargeDetailRecords, ...)

        /// <summary>
        /// Send a charge detail record.
        /// </summary>
        /// <param name="ChargeDetailRecords">An enumeration of charge detail records.</param>
        /// <param name="TransmissionType">Whether to send the charge detail record directly or enqueue it for a while.</param>
        /// 
        /// <param name="Timestamp">The timestamp of the request.</param>
        /// <param name="CancellationToken">A token to cancel this request.</param>
        /// <param name="EventTrackingId">An unique event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public async Task<SendCDRsResult>

            SendChargeDetailRecords(IEnumerable<ChargeDetailRecord>  ChargeDetailRecords,
                                    TransmissionTypes                TransmissionType    = TransmissionTypes.Enqueue,

                                    DateTime?                        Timestamp           = null,
                                    CancellationToken?               CancellationToken   = null,
                                    EventTracking_Id                 EventTrackingId     = null,
                                    TimeSpan?                        RequestTimeout      = null)

        {

            #region Initial checks

            if (ChargeDetailRecords == null)
                ChargeDetailRecords = new ChargeDetailRecord[0];


            if (!Timestamp.HasValue)
                Timestamp = DateTime.UtcNow;

            if (!CancellationToken.HasValue)
                CancellationToken = new CancellationTokenSource().Token;

            if (EventTrackingId == null)
                EventTrackingId = EventTracking_Id.New;

            if (!RequestTimeout.HasValue)
                RequestTimeout = this.RequestTimeout;

            #endregion

            #region Send OnSendCDRsRequest event

            var StartTime = DateTime.UtcNow;

            try
            {

                OnSendCDRsRequest?.Invoke(StartTime,
                                          Timestamp.Value,
                                          this,
                                          Id.ToString(),
                                          EventTrackingId,
                                          Id,
                                          new ChargeDetailRecord[0],
                                          ChargeDetailRecords,
                                          RequestTimeout);

            }
            catch (Exception e)
            {
                e.Log(nameof(RoamingNetwork) + "." + nameof(OnSendCDRsRequest));
            }

            #endregion


            #region if SendChargeDetailRecords disabled...

            DateTime        Endtime;
            TimeSpan        Runtime;
            SendCDRsResult  result = null;

            if (DisableSendChargeDetailRecords)
            {

                Endtime  = DateTime.UtcNow;
                Runtime  = Endtime - StartTime;
                result   = SendCDRsResult.AdminDown(Id,
                                                    this as ISendChargeDetailRecords,
                                                    ChargeDetailRecords,
                                                    Runtime: Runtime);

            }

            #endregion

            #region ..., or when there are no charge detail records...

            else if (!ChargeDetailRecords.Any())
            {

                Endtime  = DateTime.UtcNow;
                Runtime  = Endtime - StartTime;
                result   = SendCDRsResult.NoOperation(Id,
                                                      this as ISendChargeDetailRecords,
                                                      ChargeDetailRecords,
                                                      Runtime: Runtime);

            }

            #endregion

            else
            {

                var ChargeDetailRecordsToProcess  = ChargeDetailRecords.ToList();
                var ExpectedChargeDetailRecords   = ChargeDetailRecords.ToList();

                //ToDo: Merge given cdr information with local information!

                #region Store all CDRs...

                ChargeDetailRecordsStore.New(ChargeDetailRecords);

                #endregion

                #region Delete cached session information

                foreach (var ChargeDetailRecord in ChargeDetailRecordsToProcess)
                {
                    if (ChargeDetailRecord.EVSEId.HasValue)
                    {

                        if (TryGetEVSEById(ChargeDetailRecord.EVSEId.Value, out EVSE _EVSE))
                        {

                            if (_EVSE.ChargingSession    != null &&
                                _EVSE.ChargingSession.Id == ChargeDetailRecord.SessionId)
                            {

                                //_EVSE.Status = EVSEStatusType.Available;
                                //_EVSE.ChargingSession  = null;
                                //_EVSE.Reservation      = null;

                            }

                        }

                    }
                }

                #endregion

                var resultMap = new List<SendCDRResult>();

                #region Some charge detail records should perhaps be filtered...

                var OnFilterCDRRecordsLocal = OnFilterCDRRecords;
                if (OnFilterCDRRecordsLocal != null)
                {

                    foreach (var ChargeDetailRecord in ChargeDetailRecords)
                    {

                        var FilterResult = OnFilterCDRRecordsLocal(Id, ChargeDetailRecord);
                        if (FilterResult.IsNeitherNullNorEmpty())
                        {

                            resultMap.Add(new SendCDRResult(ChargeDetailRecord,
                                                            SendCDRResultTypes.Filtered,
                                                            FilterResult));

                            try
                            {
                                await OnCDRWasFiltered.Invoke(ChargeDetailRecord, FilterResult);
                            }
                            catch (Exception e)
                            {
                                DebugX.Log("OnCDRWasFiltered event throw an exception: " +
                                           e.Message + Environment.NewLine +
                                           e.StackTrace);
                            }

                            ChargeDetailRecordsToProcess.Remove(ChargeDetailRecord);

                        }

                    }

                }

                #endregion

                #region Group charge detail records by their targets...

                var _ISendChargeDetailRecords = new Dictionary<ISendChargeDetailRecords, List<ChargeDetailRecord>>();

                foreach (var isendcdr in _IRemoteSendChargeDetailRecord)
                    _ISendChargeDetailRecords.Add(isendcdr, new List<ChargeDetailRecord>());

                #endregion


                #region Try to send the CDRs to their target based on their SessionId...

                foreach (var cdr in ChargeDetailRecordsToProcess.ToArray())
                {

                    if (SessionsStore.TryGet(cdr.SessionId, out ChargingSession chargingSession))
                    {

                        // Might occur when the software had been restarted
                        //   while charging sessions still had been active!
                        if (chargingSession.AuthService == null && chargingSession.AuthorizatorId != null)
                            chargingSession.AuthService = _IRemoteSendChargeDetailRecord.FirstOrDefault(rm => rm.Id.ToString() == chargingSession.AuthorizatorId.ToString()) as ISendAuthorizeStartStop;

                        if (chargingSession.AuthService != null && chargingSession.AuthService is ISendChargeDetailRecords sendCDR)
                        {
                            _ISendChargeDetailRecords[sendCDR].Add(cdr);
                            ChargeDetailRecordsToProcess.Remove(cdr);
                        }

                        if (chargingSession.CSORoamingProvider == null && chargingSession.CSORoamingProviderId != null)
                            chargingSession.CSORoamingProvider = _IRemoteSendChargeDetailRecord.FirstOrDefault(rm => rm.Id.ToString() == chargingSession.CSORoamingProviderId.ToString()) as ICSORoamingProvider;

                        if (chargingSession.CSORoamingProvider != null && chargingSession.CSORoamingProvider is ICSORoamingProvider sendCDR2)
                        {
                            _ISendChargeDetailRecords[sendCDR2].Add(cdr);
                            ChargeDetailRecordsToProcess.Remove(cdr);
                        }

                    }

                }

                #endregion

                #region Any CDRs left? => Ask all e-mobility providers!

                foreach (var _cdr in ChargeDetailRecordsToProcess.ToList())
                {

                    #region We have a valid (start) provider identification

                    if (_cdr.ProviderIdStart.HasValue && _eMobilityProviders.TryGet(_cdr.ProviderIdStart.Value, out eMobilityProvider eMobPro))
                    {
                        _ISendChargeDetailRecords[eMobPro].Add(_cdr);
                        ChargeDetailRecordsToProcess.Remove(_cdr);
                    }

                    #endregion

                    #region We have a valid (stop)  provider identification

                    else if (_cdr.ProviderIdStop.HasValue && _eMobilityProviders.TryGet(_cdr.ProviderIdStop.Value, out eMobPro))
                    {
                        _ISendChargeDetailRecords[eMobPro].Add(_cdr);
                        ChargeDetailRecordsToProcess.Remove(_cdr);
                    }

                    #endregion


                    #region We have a valid (start) AuthToken/RFID identification

                    else if (_cdr.IdentificationStart?.AuthToken != null)
                    {

                        // Use a lookup...

                        // Ask all e-mobility providers...
                        foreach (var eMob in _eMobilityProviders)
                        {

                            var authStartResult = await eMob.AuthorizeStart(_cdr.IdentificationStart as LocalAuthentication);

                            if (authStartResult.Result == AuthStartResultType.Authorized ||
                                authStartResult.Result == AuthStartResultType.Blocked)
                            {
                                _ISendChargeDetailRecords[eMob].Add(_cdr);
                                ChargeDetailRecordsToProcess.Remove(_cdr);
                                break;
                            }

                        }

                    }

                    #endregion

                    #region We have a valid (stop)  AuthToken/RFID identification

                    else if (_cdr.IdentificationStop?.AuthToken != null)
                    {

                        // Use a lookup...

                        // Ask all e-mobility providers...
                        foreach (var eMob in _eMobilityProviders)
                        {

                            var authStartResult = await eMob.AuthorizeStart(_cdr.IdentificationStop as LocalAuthentication);

                            if (authStartResult.Result == AuthStartResultType.Authorized ||
                                authStartResult.Result == AuthStartResultType.Blocked)
                            {
                                _ISendChargeDetailRecords[eMob].Add(_cdr);
                                ChargeDetailRecordsToProcess.Remove(_cdr);
                                break;
                            }

                        }

                    }

                    #endregion

                    #region We have a valid (start) QR-Code identification

                    else if (_cdr.IdentificationStart?.QRCodeIdentification != null)
                    {

                        // Use a lookup...

                        // Ask all e-mobility providers...
                        foreach (var eMob in _eMobilityProviders)
                        {

                            var authStartResult = await eMob.AuthorizeStart(_cdr.IdentificationStart as LocalAuthentication);

                            if (authStartResult.Result == AuthStartResultType.Authorized ||
                                authStartResult.Result == AuthStartResultType.Blocked)
                            {
                                _ISendChargeDetailRecords[eMob].Add(_cdr);
                                ChargeDetailRecordsToProcess.Remove(_cdr);
                                break;
                            }

                        }

                    }

                    #endregion

                    #region We have a valid (stop)  QR-Code identification

                    else if (_cdr.IdentificationStop?.QRCodeIdentification != null)
                    {

                        // Use a lookup...

                        // Ask all e-mobility providers...
                        foreach (var eMob in _eMobilityProviders)
                        {

                            var authStartResult = await eMob.AuthorizeStart(_cdr.IdentificationStop as LocalAuthentication);

                            if (authStartResult.Result == AuthStartResultType.Authorized ||
                                authStartResult.Result == AuthStartResultType.Blocked)
                            {
                                _ISendChargeDetailRecords[eMob].Add(_cdr);
                                ChargeDetailRecordsToProcess.Remove(_cdr);
                                break;
                            }

                        }

                    }

                    #endregion

                    #region We have a valid (start) Plug'n'Charge identification

                    else if (_cdr.IdentificationStart?.PlugAndChargeIdentification != null)
                    {

                        // Use a lookup...

                        // Ask all e-mobility providers...
                        foreach (var eMob in _eMobilityProviders)
                        {

                            var authStartResult = await eMob.AuthorizeStart(_cdr.IdentificationStart as LocalAuthentication);

                            if (authStartResult.Result == AuthStartResultType.Authorized ||
                                authStartResult.Result == AuthStartResultType.Blocked)
                            {
                                _ISendChargeDetailRecords[eMob].Add(_cdr);
                                ChargeDetailRecordsToProcess.Remove(_cdr);
                                break;
                            }

                        }

                    }

                    #endregion

                    #region We have a valid (stop)  Plug'n'Charge identification

                    else if (_cdr.IdentificationStop?.PlugAndChargeIdentification != null)
                    {

                        // Use a lookup...

                        // Ask all e-mobility providers...
                        foreach (var eMob in _eMobilityProviders)
                        {

                            var authStartResult = await eMob.AuthorizeStart(_cdr.IdentificationStop as LocalAuthentication);

                            if (authStartResult.Result == AuthStartResultType.Authorized ||
                                authStartResult.Result == AuthStartResultType.Blocked)
                            {
                                _ISendChargeDetailRecords[eMob].Add(_cdr);
                                ChargeDetailRecordsToProcess.Remove(_cdr);
                                break;
                            }

                        }

                    }

                    #endregion

                    #region We have a valid (start) remote identification

                    else if (_cdr.IdentificationStart?.RemoteIdentification != null)
                    {

                        // Use a lookup...

                        // Ask all e-mobility providers...
                        foreach (var eMob in _eMobilityProviders)
                        {

                            var authStartResult = await eMob.AuthorizeStart(_cdr.IdentificationStart as LocalAuthentication);

                            if (authStartResult.Result == AuthStartResultType.Authorized ||
                                authStartResult.Result == AuthStartResultType.Blocked)
                            {
                                _ISendChargeDetailRecords[eMob].Add(_cdr);
                                ChargeDetailRecordsToProcess.Remove(_cdr);
                                break;
                            }

                        }

                    }

                    #endregion

                    #region We have a valid (stop)  remote identification

                    else if (_cdr.IdentificationStop?.RemoteIdentification != null)
                    {

                        // Use a lookup...

                        // Ask all e-mobility providers...
                        foreach (var eMob in _eMobilityProviders)
                        {

                            var authStartResult = await eMob.AuthorizeStart(_cdr.IdentificationStop as LocalAuthentication);

                            if (authStartResult.Result == AuthStartResultType.Authorized ||
                                authStartResult.Result == AuthStartResultType.Blocked)
                            {
                                _ISendChargeDetailRecords[eMob].Add(_cdr);
                                ChargeDetailRecordsToProcess.Remove(_cdr);
                                break;
                            }

                        }

                    }

                    #endregion

                    #region We have a valid (start) public key

                    else if (_cdr.IdentificationStart?.PublicKey != null)
                    {

                        // Use a lookup...

                        // Ask all e-mobility providers...
                        foreach (var eMob in _eMobilityProviders)
                        {

                            var authStartResult = await eMob.AuthorizeStart(_cdr.IdentificationStart as LocalAuthentication);

                            if (authStartResult.Result == AuthStartResultType.Authorized ||
                                authStartResult.Result == AuthStartResultType.Blocked)
                            {
                                _ISendChargeDetailRecords[eMob].Add(_cdr);
                                ChargeDetailRecordsToProcess.Remove(_cdr);
                                break;
                            }

                        }

                    }

                    #endregion

                    #region We have a valid (stop)  public key

                    else if (_cdr.IdentificationStop?.PublicKey != null)
                    {

                        // Use a lookup...

                        // Ask all e-mobility providers...
                        foreach (var eMob in _eMobilityProviders)
                        {

                            var authStartResult = await eMob.AuthorizeStart(_cdr.IdentificationStop as LocalAuthentication);

                            if (authStartResult.Result == AuthStartResultType.Authorized ||
                                authStartResult.Result == AuthStartResultType.Blocked)
                            {
                                _ISendChargeDetailRecords[eMob].Add(_cdr);
                                ChargeDetailRecordsToProcess.Remove(_cdr);
                                break;
                            }

                        }

                    }

                    #endregion

                }

                #endregion

                #region Any CDRs left? => bad!

                foreach (var unknownCDR in ChargeDetailRecordsToProcess)
                {

                    resultMap.Add(new SendCDRResult(unknownCDR,
                                                    SendCDRResultTypes.UnknownSessionId));

                    ChargeDetailRecordsToProcess.Clear();

                }

                #endregion


                #region Send CDRs to IRemoteSendChargeDetailRecord targets...

                SendCDRsResult partResults = null;

                foreach (var sendCDR in _ISendChargeDetailRecords.Where(kvp => kvp.Value.Count > 0))
                {

                    partResults = await sendCDR.Key.SendChargeDetailRecords(sendCDR.Value,
                                                                            TransmissionTypes.Enqueue,

                                                                            Timestamp,
                                                                            CancellationToken,
                                                                            EventTrackingId,
                                                                            RequestTimeout);

                    if (partResults == null)
                    {

                        foreach (var _cdr in sendCDR.Value)
                        {
                            resultMap.Add(new SendCDRResult(_cdr,
                                                            SendCDRResultTypes.Error,
                                                            sendCDR.Key + " returned null!"));
                        }

                    }

                    else
                    {
                        foreach (var singleSendCDRResult in partResults.ResultMap)
                            resultMap.Add(singleSendCDRResult);
                    }

                }

                #endregion


                #region Check if we really received a response for every charge detail record!

                foreach (var cdrresult in resultMap)
                    ExpectedChargeDetailRecords.Remove(cdrresult.ChargeDetailRecord);

                if (ExpectedChargeDetailRecords.Count > 0)
                {
                    foreach (var _cdr in ExpectedChargeDetailRecords)
                    {
                        resultMap.Add(new SendCDRResult(_cdr,
                                                        SendCDRResultTypes.Error,
                                                        "Did not receive an result for this charge detail record!"));
                    }
                }

                #endregion

                var Overview = new Dictionary<SendCDRResultTypes, UInt32>();
                foreach (var res in resultMap)
                {

                    if (!Overview.ContainsKey(res.Result))
                        Overview.Add(res.Result, 1);

                    else
                        Overview[res.Result]++;

                }

                var GlobalResultNumber = Overview.Values.Max();
                var GlobalResults      = Overview.Where(kvp => kvp.Value == GlobalResultNumber).Select(kvp => kvp.Key).ToList();
                if (GlobalResults.Count > 1)
                {

                    if (GlobalResults.Contains(SendCDRResultTypes.Success))
                        GlobalResults.Remove(SendCDRResultTypes.Success);

                    if (GlobalResults.Contains(SendCDRResultTypes.Enqueued))
                        GlobalResults.Remove(SendCDRResultTypes.Enqueued);

                }

                Endtime  = DateTime.UtcNow;
                Runtime  = Endtime - StartTime;
                result   = new SendCDRsResult(Id,
                                              this as IReceiveChargeDetailRecords,
                                              GlobalResults[0].Covert(),
                                              resultMap,
                                              "",
                                              null,
                                              Runtime);


                #region Store "SendCDR"-information within

                //var success = await SessionLogSemaphore.WaitAsync(TimeSpan.FromSeconds(5));

                //if (success)
                //{
                    try
                    {

                        foreach (var sendCDRResult in resultMap)
                        {

                            //SessionsStore.SendCDR(sendCDRResult);

                            //_ChargingSessions.TryRemove(sendCDRResult.ChargeDetailRecord.SessionId, out ChargingSession CS);

                            //var LogLine = String.Concat(DateTime.UtcNow.ToIso8601(), ",",
                            //                            "SendCDR,",
                            //                            sendCDRResult.ChargeDetailRecord.EVSE?.Id ?? sendCDRResult.ChargeDetailRecord.EVSEId, ",",
                            //                            sendCDRResult.ChargeDetailRecord.SessionId, ",",
                            //                            sendCDRResult.ChargeDetailRecord.IdentificationStart, ",",
                            //                            sendCDRResult.ChargeDetailRecord.IdentificationStop, ",",
                            //                            sendCDRResult.Result, ",",
                            //                            sendCDRResult.Warnings.AggregateWith("/"));

                            //File.AppendAllText(SessionLogFileName,
                            //                   LogLine + Environment.NewLine);

                        }

                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                //    finally
                //    {
                //        SessionLogSemaphore.Release();
                //    }

                //}

                #endregion

            }


            #region Send OnSendCDRsResponse event

            try
            {

                OnSendCDRsResponse?.Invoke(Endtime,
                                           Timestamp.Value,
                                           this,
                                           Id.ToString(),
                                           EventTrackingId,
                                           Id,
                                           new ChargeDetailRecord[0],
                                           ChargeDetailRecords,
                                           RequestTimeout,
                                           result,
                                           Runtime);

            }
            catch (Exception e)
            {
                e.Log(nameof(RoamingNetwork) + "." + nameof(OnSendCDRsResponse));
            }

            #endregion

            return result;

        }

        #endregion

        #endregion


        #region CreateParkingSpace(ParkingSpaceId, Configurator = null, OnSuccess = null, OnError = null)

        /// <summary>
        /// Create and register a new parking space having the given
        /// unique parking space identification.
        /// </summary>
        /// <param name="ParkingSpaceId">The unique identification of the new charging pool.</param>
        /// <param name="Configurator">An optional delegate to configure the new charging pool before its successful creation.</param>
        /// <param name="OnSuccess">An optional delegate to configure the new charging pool after its successful creation.</param>
        /// <param name="OnError">An optional delegate to be called whenever the creation of the charging pool failed.</param>
        public ParkingSpace CreateParkingSpace(ParkingSpace_Id                                   ParkingSpaceId,
                                               Action<ParkingSpace>                              Configurator   = null,
                                               Action<ParkingSpace>                              OnSuccess      = null,
                                               Action<ChargingStationOperator, ParkingSpace_Id>  OnError        = null)
        {

            var _ParkingSpace = new ParkingSpace(ParkingSpaceId);

            Configurator?.Invoke(_ParkingSpace);

            return _ParkingSpace;

        }

        #endregion


        #region IEnumerable<IEntity> Members

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
            => _ChargingStationOperators.GetEnumerator();

        public IEnumerator<IEntity> GetEnumerator()
            => _ChargingStationOperators.GetEnumerator();

        #endregion

        #region Operator overloading

        #region Operator == (RoamingNetwork1, RoamingNetwork2)

        /// <summary>
        /// Compares two roaming networks for equality.
        /// </summary>
        /// <param name="RoamingNetwork1">A roaming network.</param>
        /// <param name="RoamingNetwork2">Another roaming network.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator == (RoamingNetwork RoamingNetwork1, RoamingNetwork RoamingNetwork2)
        {

            // If both are null, or both are same instance, return true.
            if (Object.ReferenceEquals(RoamingNetwork1, RoamingNetwork2))
                return true;

            // If one is null, but not both, return false.
            if (((Object) RoamingNetwork1 == null) || ((Object) RoamingNetwork2 == null))
                return false;

            return RoamingNetwork1.Equals(RoamingNetwork2);

        }

        #endregion

        #region Operator != (RoamingNetwork1, RoamingNetwork2)

        /// <summary>
        /// Compares two roaming networks for inequality.
        /// </summary>
        /// <param name="RoamingNetwork1">A roaming network.</param>
        /// <param name="RoamingNetwork2">Another roaming network.</param>
        /// <returns>False if both match; True otherwise.</returns>
        public static Boolean operator != (RoamingNetwork RoamingNetwork1, RoamingNetwork RoamingNetwork2)

            => !(RoamingNetwork1 == RoamingNetwork2);

        #endregion

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
                throw new ArgumentNullException(nameof(Object),  "The given object must not be null!");

            // Check if the given object is a roaming network.
            var RoamingNetwork = Object as RoamingNetwork;
            if ((Object) RoamingNetwork == null)
                throw new ArgumentException("The given object is not a roaming network!");

            return CompareTo(RoamingNetwork);

        }

        #endregion

        #region CompareTo(RoamingNetwork)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="RoamingNetwork">A roaming network object to compare with.</param>
        public Int32 CompareTo(RoamingNetwork RoamingNetwork)
        {

            if ((Object) RoamingNetwork == null)
                throw new ArgumentNullException(nameof(RoamingNetwork),  "The given roaming network must not be null!");

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

            // Check if the given object is a roaming network.
            var RoamingNetwork = Object as RoamingNetwork;
            if ((Object) RoamingNetwork == null)
                return false;

            return this.Equals(RoamingNetwork);

        }

        #endregion

        #region Equals(RoamingNetwork)

        /// <summary>
        /// Compares two roaming networks for equality.
        /// </summary>
        /// <param name="RoamingNetwork">A roaming network to compare with.</param>
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
