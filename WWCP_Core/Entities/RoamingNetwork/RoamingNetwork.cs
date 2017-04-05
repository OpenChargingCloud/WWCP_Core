/*
 * Copyright (c) 2014-2017 GraphDefined GmbH <achim.friedland@graphdefined.com>
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
    /// This can e.g. be a differentation of service levels (premiun, basic,
    /// discount) or allow a simplified testing (production, qa, featureX, ...)
    /// </summary>
    public class RoamingNetwork : ACryptoEMobilityEntity<RoamingNetwork_Id>,
                                  IEquatable<RoamingNetwork>, IComparable<RoamingNetwork>, IComparable,
                                  IEnumerable<IEntity>,
                                  IStatus<RoamingNetworkStatusType>
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

        #endregion

        #region Properties

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


        #region AdminStatus

        /// <summary>
        /// The current admin status.
        /// </summary>
        [Optional]
        public Timestamped<RoamingNetworkAdminStatusType> AdminStatus

            => _AdminStatusSchedule.CurrentStatus;

        #endregion

        #region AdminStatusSchedule

        private StatusSchedule<RoamingNetworkAdminStatusType> _AdminStatusSchedule;

        /// <summary>
        /// The admin status schedule.
        /// </summary>
        public IEnumerable<Timestamped<RoamingNetworkAdminStatusType>> AdminStatusSchedule(UInt64? HistorySize = null)
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
        public Timestamped<RoamingNetworkStatusType> Status

            => _StatusSchedule.CurrentStatus;

        #endregion

        #region StatusSchedule

        private StatusSchedule<RoamingNetworkStatusType> _StatusSchedule;

        /// <summary>
        /// The status schedule.
        /// </summary>
        public IEnumerable<Timestamped<RoamingNetworkStatusType>> StatusSchedule(UInt64? HistorySize = null)
        {

            if (HistorySize.HasValue)
                return _StatusSchedule.Take(HistorySize);

            return _StatusSchedule;

        }

        #endregion


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
                              RoamingNetworkAdminStatusType             AdminStatus                                = RoamingNetworkAdminStatusType.Operational,
                              RoamingNetworkStatusType                  Status                                     = RoamingNetworkStatusType.Available,
                              UInt16                                    MaxAdminStatusListSize                     = DefaultMaxAdminStatusListSize,
                              UInt16                                    MaxStatusListSize                          = DefaultMaxStatusListSize,
                              ChargingStationSignatureDelegate          ChargingStationSignatureGenerator          = null,
                              ChargingPoolSignatureDelegate             ChargingPoolSignatureGenerator             = null,
                              ChargingStationOperatorSignatureDelegate  ChargingStationOperatorSignatureGenerator  = null)

            : base(Id)

        {

            #region Init data and properties

            this._Name                                              = Description ?? new I18NString();
            this._Description                                       = Description ?? new I18NString();

            this._ChargingStationOperators                          = new EntityHashSet<RoamingNetwork, ChargingStationOperator_Id, ChargingStationOperator>(this);
            this._ParkingOperators                                  = new EntityHashSet<RoamingNetwork, ParkingOperator_Id,         ParkingOperator>        (this);
            this._eMobilityProviders                                = new EntityHashSet<RoamingNetwork, eMobilityProvider_Id,       eMobilityProvider>      (this);
            this._SmartCities                                       = new EntityHashSet<RoamingNetwork, SmartCity_Id,               SmartCity>              (this);
            this._NavigationProviders                               = new EntityHashSet<RoamingNetwork, NavigationProvider_Id,      NavigationProvider>     (this);
            this._GridOperators                                     = new EntityHashSet<RoamingNetwork, GridOperator_Id,            GridOperator>           (this);

            this._ChargingStationOperatorRoamingProviders           = new ConcurrentDictionary<CSORoamingProvider_Id, ICSORoamingProvider>();
            this._EMPRoamingProviders                               = new ConcurrentDictionary<EMPRoamingProvider_Id, IEMPRoamingProvider>();
            this._ChargingReservations_AtChargingStationOperators   = new ConcurrentDictionary<ChargingReservation_Id, ChargingStationOperator>();
            this._ChargingReservations_AtEMPRoamingProviders        = new ConcurrentDictionary<ChargingReservation_Id, IEMPRoamingProvider>();

            this._eMobilityRoamingServices                          = new ConcurrentDictionary<UInt32, IEMPRoamingProvider>();
            this._PushEVSEDataToOperatorRoamingServices             = new ConcurrentDictionary<UInt32, IPushData>();
            this._PushEVSEStatusToOperatorRoamingServices           = new ConcurrentDictionary<UInt32, IPushStatus>();

            this._ChargingSessions                                  = new ConcurrentDictionary<ChargingSession_Id, ChargingSession>();
            this._ChargeDetailRecords                               = new ConcurrentDictionary<ChargingSession_Id, ChargeDetailRecord>();

            this._AdminStatusSchedule                               = new StatusSchedule<RoamingNetworkAdminStatusType>(MaxAdminStatusListSize);
            this._AdminStatusSchedule.Insert(AdminStatus);

            this._StatusSchedule                                    = new StatusSchedule<RoamingNetworkStatusType>(MaxStatusListSize);
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
            this.ChargingStationRemoval      = new AggregatedNotificator<ChargingStation>();

            // ChargingStation events
            this.EVSEAddition                = new VotingNotificator<DateTime, ChargingStation, EVSE, Boolean>(() => new VetoVote(), true);
            this.EVSERemoval                 = new VotingNotificator<DateTime, ChargingStation, EVSE, Boolean>(() => new VetoVote(), true);

            // EVSE events
            this.SocketOutletAddition        = new VotingNotificator<DateTime, EVSE, SocketOutlet, Boolean>(() => new VetoVote(), true);
            this.SocketOutletRemoval         = new VotingNotificator<DateTime, EVSE, SocketOutlet, Boolean>(() => new VetoVote(), true);

            #endregion

            this.OnPropertyChanged += UpdateData;

        }

        #endregion


        private readonly ConcurrentDictionary<UInt32, IRemotePushData>   _IRemotePushData    = new ConcurrentDictionary<UInt32, IRemotePushData>();

        public void AddIRemotePushData(IRemotePushData iRemotePushData)
        {
            lock(_IRemotePushData)
            {

                _IRemotePushData.TryAdd(_IRemotePushData.Count > 0
                                            ? _IRemotePushData.Keys.Max() + 1
                                            : 1,
                                        iRemotePushData);

            }
        }

        private readonly ConcurrentDictionary<UInt32, IRemotePushStatus> _IRemotePushStatus  = new ConcurrentDictionary<UInt32, IRemotePushStatus>();

        public void AddIRemotePushStatus(IRemotePushStatus iRemotePushStatus)
        {
            lock (_IRemotePushStatus)
            {

                _IRemotePushStatus.TryAdd(_IRemotePushStatus.Count > 0
                                              ? _IRemotePushStatus.Keys.Max() + 1
                                              : 1,
                                          iRemotePushStatus);

            }
        }


        private readonly ConcurrentDictionary<UInt32, IRemoteAuthorizeStartStop> _IRemoteAuthorizeStartStop = new ConcurrentDictionary<UInt32, IRemoteAuthorizeStartStop>();

        public void AddIRemoteAuthorizeStartStop(IRemoteAuthorizeStartStop iRemoteAuthorizeStartStop)
        {
            lock (_IRemoteAuthorizeStartStop)
            {

                _IRemoteAuthorizeStartStop.TryAdd(_IRemoteAuthorizeStartStop.Count > 0
                                                      ? _IRemoteAuthorizeStartStop.Keys.Max() + 1
                                                      : 1,
                                                  iRemoteAuthorizeStartStop);

            }
        }


        private readonly ConcurrentDictionary<UInt32, IRemoteSendChargeDetailRecords> _IRemoteSendChargeDetailRecord = new ConcurrentDictionary<UInt32, IRemoteSendChargeDetailRecords>();

        public void AddIRemoteSendChargeDetailRecord(IRemoteSendChargeDetailRecords iRemoteSendChargeDetailRecord)
        {
            lock (_IRemoteSendChargeDetailRecord)
            {

                _IRemoteSendChargeDetailRecord.TryAdd(_IRemoteSendChargeDetailRecord.Count > 0
                                                         ? _IRemoteSendChargeDetailRecord.Keys.Max() + 1
                                                         : 1,
                                                     iRemoteSendChargeDetailRecord);

            }
        }


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


        #region (internal) UpdateData(Timestamp, Sender, PropertyName, OldValue, NewValue)

        /// <summary>
        /// Update the static data of the roaming network.
        /// </summary>
        /// <param name="Timestamp">The timestamp when this change was detected.</param>
        /// <param name="Sender">The changed roaming network.</param>
        /// <param name="PropertyName">The name of the changed property.</param>
        /// <param name="OldValue">The old value of the changed property.</param>
        /// <param name="NewValue">The new value of the changed property.</param>
        internal async Task UpdateData(DateTime  Timestamp,
                                       Object    Sender,
                                       String    PropertyName,
                                       Object    OldValue,
                                       Object    NewValue)
        {

            var OnDataChangedLocal = OnDataChanged;
            if (OnDataChangedLocal != null)
                await OnDataChangedLocal(Timestamp, Sender as RoamingNetwork, PropertyName, OldValue, NewValue);

        }

        #endregion

        #endregion

        #region Charging Station Operators...

        #region ChargingStationOperators

        private readonly EntityHashSet<RoamingNetwork, ChargingStationOperator_Id, ChargingStationOperator> _ChargingStationOperators;

        /// <summary>
        /// Return all Charging Station Operators registered within this roaming network.
        /// </summary>
        public IEnumerable<ChargingStationOperator> ChargingStationOperators

            => _ChargingStationOperators;

        #endregion

        #region ChargingStationOperatorAdminStatus

        /// <summary>
        /// Return the admin status of all charging station operators registered within this roaming network.
        /// </summary>
        public IEnumerable<KeyValuePair<ChargingStationOperator_Id, IEnumerable<Timestamped<ChargingStationOperatorAdminStatusType>>>> ChargingStationOperatorAdminStatus

            => _ChargingStationOperators.
                   Select(cso => new KeyValuePair<ChargingStationOperator_Id, IEnumerable<Timestamped<ChargingStationOperatorAdminStatusType>>>(cso.Id,
                                                                                                                                                cso.AdminStatusSchedule()));

        #endregion

        #region ChargingStationOperatorStatus

        /// <summary>
        /// Return the status of all charging station operators registered within this roaming network.
        /// </summary>
        public IEnumerable<KeyValuePair<ChargingStationOperator_Id, IEnumerable<Timestamped<ChargingStationOperatorStatusType>>>> ChargingStationOperatorStatus

            => _ChargingStationOperators.
                   Select(cso => new KeyValuePair<ChargingStationOperator_Id, IEnumerable<Timestamped<ChargingStationOperatorStatusType>>>(cso.Id,
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
                                          ChargingStationOperatorAdminStatusType              AdminStatus                           = ChargingStationOperatorAdminStatusType.Operational,
                                          ChargingStationOperatorStatusType                   Status                                = ChargingStationOperatorStatusType.Available,
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
                                          I18NString                                          Name                                  = null,
                                          I18NString                                          Description                           = null,
                                          Action<ChargingStationOperator>                     Configurator                          = null,
                                          RemoteChargingStationOperatorCreatorDelegate        RemoteChargingStationOperatorCreator  = null,
                                          ChargingStationOperatorAdminStatusType              AdminStatus                           = ChargingStationOperatorAdminStatusType.Operational,
                                          ChargingStationOperatorStatusType                   Status                                = ChargingStationOperatorStatusType.Available,
                                          Action<ChargingStationOperator>                     OnSuccess                             = null,
                                          Action<RoamingNetwork, ChargingStationOperator_Id>  OnError                               = null)

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

                    _ChargingStationOperator.OnDataChanged                                 += UpdatecsoData;
                    _ChargingStationOperator.OnStatusChanged                               += UpdateStatus;
                    _ChargingStationOperator.OnAdminStatusChanged                          += UpdateAdminStatus;

                    _ChargingStationOperator.OnChargingPoolDataChanged                     += UpdateChargingPoolData;
                    _ChargingStationOperator.OnChargingPoolStatusChanged                   += UpdateChargingPoolStatus;
                    _ChargingStationOperator.OnChargingPoolAdminStatusChanged              += UpdateChargingPoolAdminStatus;

                    _ChargingStationOperator.OnChargingStationDataChanged                  += UpdateChargingStationData;
                    _ChargingStationOperator.OnChargingStationStatusChanged                += UpdateChargingStationStatus;
                    _ChargingStationOperator.OnChargingStationAdminStatusChanged           += UpdateChargingStationAdminStatus;

                    //_cso.EVSEAddition.OnVoting                         += SendEVSEAdding;
                    _ChargingStationOperator.EVSEAddition.OnNotification                   += SendEVSEAdded;
                    //_cso.EVSERemoval.OnVoting                          += SendEVSERemoving;
                    _ChargingStationOperator.EVSERemoval.OnNotification                    += SendEVSERemoved;
                    _ChargingStationOperator.OnEVSEDataChanged                             += UpdateEVSEData;
                    _ChargingStationOperator.OnEVSEStatusChanged                           += UpdateEVSEStatus;
                    _ChargingStationOperator.OnEVSEAdminStatusChanged                      += UpdateEVSEAdminStatus;


                    _ChargingStationOperator.OnNewReservation                              += SendNewReservation;
                    _ChargingStationOperator.OnReservationCancelled                        += SendOnReservationCancelled;
                    _ChargingStationOperator.OnNewChargingSession                          += SendNewChargingSession;
                    _ChargingStationOperator.OnNewChargeDetailRecord                       += SendNewChargeDetailRecord;

                    return _ChargingStationOperator;

                }

                //ToDo: Throw a more usefull exception!
                throw new ChargingStationOperatorAlreadyExists(this, ChargingStationOperatorIds.FirstOrDefault(), Name);

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
                                                   ChargingStationOperator  cso,
                                                   String        PropertyName,
                                                   Object        OldValue,
                                                   Object        NewValue)
        {

            var OnChargingStationOperatorDataChangedLocal = OnChargingStationOperatorDataChanged;
            if (OnChargingStationOperatorDataChangedLocal != null)
                await OnChargingStationOperatorDataChangedLocal(Timestamp, cso, PropertyName, OldValue, NewValue);

        }

        #endregion

        #region (internal) UpdateStatus(Timestamp, cso, OldStatus, NewStatus)

        /// <summary>
        /// Update the current Charging Station Operator status.
        /// </summary>
        /// <param name="Timestamp">The timestamp when this change was detected.</param>
        /// <param name="cso">The updated Charging Station Operator.</param>
        /// <param name="OldStatus">The old aggreagted Charging Station Operator status.</param>
        /// <param name="NewStatus">The new aggreagted Charging Station Operator status.</param>
        internal async Task UpdateStatus(DateTime                             Timestamp,
                                         ChargingStationOperator                         cso,
                                         Timestamped<ChargingStationOperatorStatusType>  OldStatus,
                                         Timestamped<ChargingStationOperatorStatusType>  NewStatus)
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

        #region (internal) UpdateAdminStatus(Timestamp, cso, OldStatus, NewStatus)

        /// <summary>
        /// Update the current Charging Station Operator admin status.
        /// </summary>
        /// <param name="Timestamp">The timestamp when this change was detected.</param>
        /// <param name="cso">The updated Charging Station Operator.</param>
        /// <param name="OldStatus">The old aggreagted Charging Station Operator status.</param>
        /// <param name="NewStatus">The new aggreagted Charging Station Operator status.</param>
        internal async Task UpdateAdminStatus(DateTime                                  Timestamp,
                                              ChargingStationOperator                              cso,
                                              Timestamped<ChargingStationOperatorAdminStatusType>  OldStatus,
                                              Timestamped<ChargingStationOperatorAdminStatusType>  NewStatus)
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


                _ParkingOperator.OnNewReservation                              += SendNewReservation;
                _ParkingOperator.OnReservationCancelled                        += SendOnReservationCancelled;
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

        #region e-Mobility Providers...

        #region eMobilityProviders

        private readonly EntityHashSet<RoamingNetwork, eMobilityProvider_Id, eMobilityProvider> _eMobilityProviders;

        /// <summary>
        /// Return all e-mobility providers registered within this roaming network.
        /// </summary>
        public IEnumerable<eMobilityProvider> eMobilityProviders

            => _eMobilityProviders;

        #endregion

        #region EMobilityProviderAdminStatus

        /// <summary>
        /// Return the admin status of all e-mobility providers registered within this roaming network.
        /// </summary>
        public IEnumerable<KeyValuePair<eMobilityProvider_Id, IEnumerable<Timestamped<eMobilityProviderAdminStatusType>>>> EMobilityProviderAdminStatus

            => _eMobilityProviders.
                   Select(emp => new KeyValuePair<eMobilityProvider_Id, IEnumerable<Timestamped<eMobilityProviderAdminStatusType>>>(emp.Id, emp.AdminStatusSchedule));

        #endregion

        #region EMobilityProviderStatus

        /// <summary>
        /// Return the status of all e-mobility providers registered within this roaming network.
        /// </summary>
        public IEnumerable<KeyValuePair<eMobilityProvider_Id, IEnumerable<Timestamped<eMobilityProviderStatusType>>>> EMobilityProviderStatus

            => _eMobilityProviders.
                   Select(emp => new KeyValuePair<eMobilityProvider_Id, IEnumerable<Timestamped<eMobilityProviderStatusType>>>(emp.Id, emp.StatusSchedule));

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
        public eMobilityProvider CreateNewEMobilityProvider(eMobilityProvider_Id                          ProviderId,
                                                            I18NString                                    Name                            = null,
                                                            I18NString                                    Description                     = null,
                                                            eMobilityProviderPriority                     Priority                        = null,
                                                            Action<eMobilityProvider>                     Configurator                    = null,
                                                            RemoteEMobilityProviderCreatorDelegate        RemoteEMobilityProviderCreator  = null,
                                                            eMobilityProviderAdminStatusType              AdminStatus                     = eMobilityProviderAdminStatusType.Available,
                                                            eMobilityProviderStatusType                   Status                          = eMobilityProviderStatusType.Available,
                                                            Action<eMobilityProvider>                     OnSuccess                       = null,
                                                            Action<RoamingNetwork, eMobilityProvider_Id>  OnError                         = null)
        {

            var _EMobilityProvider = new eMobilityProvider(ProviderId,
                                                           this,
                                                           Configurator,
                                                           RemoteEMobilityProviderCreator,
                                                           Name,
                                                           Description,
                                                           Priority,
                                                           AdminStatus,
                                                           Status);


            if (_eMobilityProviders.TryAdd(_EMobilityProvider, OnSuccess))
            {

                //AddIRemotePushData              (_EMobilityProvider);
                //AddIRemotePushStatus            (_EMobilityProvider);
                //AddIRemoteAuthorizeStartStop    (_EMobilityProvider);
                //AddIRemoteSendChargeDetailRecord(_EMobilityProvider);


                // Link events!

                return _EMobilityProvider;

            }

            throw new eMobilityProviderAlreadyExists(this, ProviderId);

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


        #region AllTokens

        public IEnumerable<KeyValuePair<Auth_Token, TokenAuthorizationResultType>> AllTokens

            => _eMobilityProviders.SelectMany(provider => provider.AllTokens);

        #endregion

        #region AuthorizedTokens

        public IEnumerable<KeyValuePair<Auth_Token, TokenAuthorizationResultType>> AuthorizedTokens

            => _eMobilityProviders.SelectMany(provider => provider.AuthorizedTokens);

        #endregion

        #region NotAuthorizedTokens

        public IEnumerable<KeyValuePair<Auth_Token, TokenAuthorizationResultType>> NotAuthorizedTokens

            => _eMobilityProviders.SelectMany(provider => provider.NotAuthorizedTokens);

        #endregion

        #region BlockedTokens

        public IEnumerable<KeyValuePair<Auth_Token, TokenAuthorizationResultType>> BlockedTokens

            => _eMobilityProviders.SelectMany(provider => provider.BlockedTokens);

        #endregion

        #endregion

        #region Smart Cities...

        #region SmartCities

        private readonly EntityHashSet<RoamingNetwork, SmartCity_Id, SmartCity> _SmartCities;

        /// <summary>
        /// Return all smart cities registered within this roaming network.
        /// </summary>
        public IEnumerable<SmartCity> SmartCities

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
        public IVotingSender<DateTime, RoamingNetwork, SmartCity, Boolean> OnSmartCityAddition
            => _SmartCities.OnAddition;

        #endregion

        #region OnSmartCityRemoval

        /// <summary>
        /// Called whenever an EVServiceProvider will be or was removed.
        /// </summary>
        public IVotingSender<DateTime, RoamingNetwork, SmartCity, Boolean> OnSmartCityRemoval
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
        public SmartCity CreateNewSmartCity(SmartCity_Id                      SmartCityId,
                                            I18NString                            Name                     = null,
                                            I18NString                            Description              = null,
                                            SmartCityPriority                     Priority                 = null,
                                            SmartCityAdminStatusType              AdminStatus              = SmartCityAdminStatusType.Available,
                                            SmartCityStatusType                   Status                   = SmartCityStatusType.Available,
                                            Action<SmartCity>                 Configurator             = null,
                                            Action<SmartCity>                 OnSuccess                = null,
                                            Action<RoamingNetwork, SmartCity_Id>  OnError                  = null,
                                            RemoteSmartCityCreatorDelegate        RemoteSmartCityCreator   = null)
        {

            #region Initial checks

            if (SmartCityId == null)
                throw new ArgumentNullException(nameof(SmartCityId),  "The given smart city identification must not be null!");

            #endregion

            var _SmartCity = new SmartCity(SmartCityId,
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
        public Boolean ContainsSmartCity(SmartCity SmartCity)

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

        public SmartCity GetSmartCityById(SmartCity_Id SmartCityId)

            => _SmartCities.GetById(SmartCityId);

        #endregion

        #region TryGetSmartCityById(SmartCityId, out SmartCity)

        public Boolean TryGetSmartCityById(SmartCity_Id SmartCityId, out SmartCity SmartCity)

            => _SmartCities.TryGet(SmartCityId, out SmartCity);

        #endregion

        #region RemoveSmartCity(SmartCityId)

        public SmartCity RemoveSmartCity(SmartCity_Id SmartCityId)
        {

            SmartCity _SmartCity = null;

            if (_SmartCities.TryRemove(SmartCityId, out _SmartCity))
                return _SmartCity;

            return null;

        }

        #endregion

        #region TryRemoveSmartCity(RemoveSmartCityId, out RemoveSmartCity)

        public Boolean TryRemoveSmartCity(SmartCity_Id SmartCityId, out SmartCity SmartCity)

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
        private readonly ConcurrentDictionary<UInt32, IPushData>            _PushEVSEDataToOperatorRoamingServices;
        private readonly ConcurrentDictionary<UInt32, IPushStatus>          _PushEVSEStatusToOperatorRoamingServices;

        #region ChargingStationOperatorRoamingProviders

        //private readonly ConcurrentDictionary<UInt32,             IChargingStationOperatorRoamingProvider>  _ChargingStationOperatorRoamingProviderPriorities;
        private readonly ConcurrentDictionary<CSORoamingProvider_Id, ICSORoamingProvider>  _ChargingStationOperatorRoamingProviders;

        /// <summary>
        /// Return all Charging Station Operator roaming providers registered within this roaming network.
        /// </summary>
        public IEnumerable<ICSORoamingProvider> ChargingStationOperatorRoamingProviders => _ChargingStationOperatorRoamingProviders.Values;

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

            if (_CPORoamingProvider.Name.IsNullOrEmpty())
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

                    AddIRemotePushData              (_CPORoamingProvider);
                    AddIRemotePushStatus            (_CPORoamingProvider);
                    AddIRemoteAuthorizeStartStop    (_CPORoamingProvider);
                    AddIRemoteSendChargeDetailRecord(_CPORoamingProvider);

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

            if (eMobilityRoamingService.Name.IsNullOrEmpty())
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

        /// <summary>
        /// Register the given push-data service.
        /// </summary>
        /// <param name="Priority">The priority of the service.</param>
        /// <param name="PushEVSEDataServices">The push-data service.</param>
        public Boolean RegisterPushEVSEStatusService(UInt32              Priority,
                                                     IPushData           PushEVSEDataServices)

            => _PushEVSEDataToOperatorRoamingServices.TryAdd(Priority, PushEVSEDataServices);

        #endregion

        #region RegisterPushEVSEStatusService(Priority, PushEVSEStatusServices)

        /// <summary>
        /// Register the given push-status service.
        /// </summary>
        /// <param name="Priority">The priority of the service.</param>
        /// <param name="PushEVSEStatusServices">The push-status service.</param>
        public Boolean RegisterPushEVSEStatusService(UInt32       Priority,
                                                     IPushStatus  PushEVSEStatusServices)

            => _PushEVSEStatusToOperatorRoamingServices.TryAdd(Priority, PushEVSEStatusServices);

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

        #region ChargingPoolAdminStatus

        /// <summary>
        /// Return the admin status of all charging pools registered within this roaming network.
        /// </summary>
        public IEnumerable<KeyValuePair<ChargingPool_Id, IEnumerable<Timestamped<ChargingPoolAdminStatusType>>>> ChargingPoolAdminStatus

            => _ChargingStationOperators.
                   SelectMany(cso => cso.Select(pool => new KeyValuePair<ChargingPool_Id, IEnumerable<Timestamped<ChargingPoolAdminStatusType>>>(pool.Id,
                                                                                                                                                 pool.AdminStatusSchedule())));

        #endregion

        #region ChargingPoolStatus

        /// <summary>
        /// Return the status of all charging pools registered within this roaming network.
        /// </summary>
        public IEnumerable<KeyValuePair<ChargingPool_Id, IEnumerable<Timestamped<ChargingPoolStatusType>>>> ChargingPoolStatus

            => _ChargingStationOperators.
                   SelectMany(cso => cso.Select(pool => new KeyValuePair<ChargingPool_Id, IEnumerable<Timestamped<ChargingPoolStatusType>>>(pool.Id,
                                                                                                                                            pool.StatusSchedule())));

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

        public ChargingPool GetChargingPoolbyId(ChargingPool_Id ChargingPoolId)
        {

            ChargingPool            _ChargingPool             = null;
            ChargingStationOperator _ChargingStationOperator  = null;

            if (TryGetChargingStationOperatorById(ChargingPoolId.OperatorId, out _ChargingStationOperator) &&
                _ChargingStationOperator.TryGetChargingPoolbyId(ChargingPoolId, out _ChargingPool))
                    return _ChargingPool;

            return null;

        }

        #endregion

        #region TryGetChargingPoolbyId(ChargingPoolId, out ChargingPool)

        public Boolean TryGetChargingPoolbyId(ChargingPool_Id ChargingPoolId, out ChargingPool ChargingPool)
        {

            ChargingStationOperator _ChargingStationOperator  = null;

            if (TryGetChargingStationOperatorById(ChargingPoolId.OperatorId, out _ChargingStationOperator))
                return _ChargingStationOperator.TryGetChargingPoolbyId(ChargingPoolId, out ChargingPool);

            ChargingPool = null;
            return false;

        }

        #endregion

        #region SetChargingPoolAdminStatus(ChargingPoolId, StatusList)

        public void SetChargingPoolAdminStatus(ChargingPool_Id                                        ChargingPoolId,
                                               IEnumerable<Timestamped<ChargingPoolAdminStatusType>>  StatusList)
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
        /// An event fired whenever the aggregated dynamic status of any subordinated charging pool changed.
        /// </summary>
        public event OnChargingPoolStatusChangedDelegate       OnChargingPoolStatusChanged;

        /// <summary>
        /// An event fired whenever the aggregated admin status of any subordinated charging pool changed.
        /// </summary>
        public event OnChargingPoolAdminStatusChangedDelegate  OnChargingPoolAdminStatusChanged;

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


        #region (internal) UpdateChargingPoolData(Timestamp, ChargingPool, OldStatus, NewStatus)

        /// <summary>
        /// Update the data of an charging pool.
        /// </summary>
        /// <param name="Timestamp">The timestamp when this change was detected.</param>
        /// <param name="ChargingPool">The changed charging pool.</param>
        /// <param name="PropertyName">The name of the changed property.</param>
        /// <param name="OldValue">The old value of the changed property.</param>
        /// <param name="NewValue">The new value of the changed property.</param>
        internal async Task UpdateChargingPoolData(DateTime      Timestamp,
                                                   ChargingPool  ChargingPool,
                                                   String        PropertyName,
                                                   Object        OldValue,
                                                   Object        NewValue)
        {

            Acknowledgement result = null;


            //foreach (var AuthenticationService in _IeMobilityServiceProviders.
            //                                          OrderBy(AuthServiceWithPriority => AuthServiceWithPriority.Key).
            //                                          Select (AuthServiceWithPriority => AuthServiceWithPriority.Value))
            //{

            //    result = await AuthenticationService.PushEVSEStatus(new EVSEStatus(EVSE.Id, NewStatus.Value, NewStatus.Timestamp),
            //                                                        ActionType.update,
            //                                                        EVSE.Operator.Id);

            //}

            foreach (var iRemotePushData in _IRemotePushData.
                                                OrderBy(kvp => kvp.Key).
                                                Select (kvp => kvp.Value))
            {

                result = await iRemotePushData.
                                   UpdateStaticData(ChargingPool,
                                                    PropertyName,
                                                    OldValue,
                                                    NewValue).
                                   ConfigureAwait(false);

            }

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
                await OnChargingPoolDataChangedLocal(Timestamp, ChargingPool, PropertyName, OldValue, NewValue);

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
        internal async Task UpdateChargingPoolStatus(DateTime                             Timestamp,
                                                     ChargingPool                         ChargingPool,
                                                     Timestamped<ChargingPoolStatusType>  OldStatus,
                                                     Timestamped<ChargingPoolStatusType>  NewStatus)
        {

            var OnChargingPoolStatusChangedLocal = OnChargingPoolStatusChanged;
            if (OnChargingPoolStatusChangedLocal != null)
                await OnChargingPoolStatusChangedLocal(Timestamp, ChargingPool, OldStatus, NewStatus);

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
        internal async Task UpdateChargingPoolAdminStatus(DateTime                                  Timestamp,
                                                          ChargingPool                              ChargingPool,
                                                          Timestamped<ChargingPoolAdminStatusType>  OldStatus,
                                                          Timestamped<ChargingPoolAdminStatusType>  NewStatus)
        {

            var OnChargingPoolAdminStatusChangedLocal = OnChargingPoolAdminStatusChanged;
            if (OnChargingPoolAdminStatusChangedLocal != null)
                await OnChargingPoolAdminStatusChangedLocal(Timestamp, ChargingPool, OldStatus, NewStatus);

        }

        #endregion

        #endregion

        #region ChargingStations...

        #region ChargingStations

        /// <summary>
        /// Return all charging stations registered within this roaming network.
        /// </summary>
        public IEnumerable<ChargingStation> ChargingStations

            => _ChargingStationOperators.SelectMany(cso => cso.SelectMany(pool => pool));

        #endregion

        #region ChargingStationAdminStatus

        /// <summary>
        /// Return the admin status of all charging stations registered within this roaming network.
        /// </summary>
        public IEnumerable<KeyValuePair<ChargingStation_Id, IEnumerable<Timestamped<ChargingStationAdminStatusTypes>>>> ChargingStationAdminStatus

            => _ChargingStationOperators.
                   SelectMany(cso =>
                       cso.SelectMany(pool =>
                           pool.Select(station =>

                                   new KeyValuePair<ChargingStation_Id, IEnumerable<Timestamped<ChargingStationAdminStatusTypes>>>(
                                       station.Id,
                                       station.AdminStatusSchedule())

                               )));

        #endregion

        #region ChargingStationStatus

        /// <summary>
        /// Return the status of all charging stations registered within this roaming network.
        /// </summary>
        public IEnumerable<KeyValuePair<ChargingStation_Id, IEnumerable<Timestamped<ChargingStationStatusTypes>>>> ChargingStationStatus

            => _ChargingStationOperators.
                   SelectMany(cso =>
                       cso.SelectMany(pool =>
                           pool.Select(station =>

                                   new KeyValuePair<ChargingStation_Id, IEnumerable<Timestamped<ChargingStationStatusTypes>>>(
                                       station.Id,
                                       station.StatusSchedule())

                               )));

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

        public ChargingStation GetChargingStationbyId(ChargingStation_Id ChargingStationId)
        {

            ChargingStation         _ChargingStation          = null;
            ChargingStationOperator _ChargingStationOperator  = null;

            if (TryGetChargingStationOperatorById(ChargingStationId.OperatorId, out _ChargingStationOperator) &&
                _ChargingStationOperator.TryGetChargingStationbyId(ChargingStationId, out _ChargingStation))
                    return _ChargingStation;

            return null;

        }

        #endregion

        #region TryGetChargingStationbyId(ChargingStationId, out ChargingStation)

        public Boolean TryGetChargingStationbyId(ChargingStation_Id ChargingStationId, out ChargingStation ChargingStation)
        {

            ChargingStationOperator _ChargingStationOperator  = null;

            if (TryGetChargingStationOperatorById(ChargingStationId.OperatorId, out _ChargingStationOperator))
                return _ChargingStationOperator.TryGetChargingStationbyId(ChargingStationId, out ChargingStation);

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
        /// An event fired whenever the aggregated dynamic status of any subordinated charging station changed.
        /// </summary>
        public event OnChargingStationStatusChangedDelegate       OnChargingStationStatusChanged;

        /// <summary>
        /// An event fired whenever the admin status of any subordinated ChargingStation changed.
        /// </summary>
        public event OnChargingStationAdminStatusChangedDelegate  OnChargingStationAdminStatusChanged;

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

        internal readonly AggregatedNotificator<ChargingStation> ChargingStationRemoval;

        /// <summary>
        /// Called whenever a charging station will be or was removed.
        /// </summary>
        public AggregatedNotificator<ChargingStation> OnChargingStationRemoval

            => ChargingStationRemoval;

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
        internal async Task UpdateChargingStationData(DateTime         Timestamp,
                                                      ChargingStation  ChargingStation,
                                                      String           PropertyName,
                                                      Object           OldValue,
                                                      Object           NewValue)
        {

            Acknowledgement result = null;


            //foreach (var AuthenticationService in _IeMobilityServiceProviders.
            //                                          OrderBy(AuthServiceWithPriority => AuthServiceWithPriority.Key).
            //                                          Select (AuthServiceWithPriority => AuthServiceWithPriority.Value))
            //{

            //    result = await AuthenticationService.PushEVSEStatus(new EVSEStatus(EVSE.Id, NewStatus.Value, NewStatus.Timestamp),
            //                                                        ActionType.update,
            //                                                        EVSE.Operator.Id);

            //}

            foreach (var iRemotePushData in _IRemotePushData.
                                                OrderBy(kvp => kvp.Key).
                                                Select (kvp => kvp.Value))
            {

                result = await iRemotePushData.
                                   UpdateStaticData(ChargingStation,
                                                    PropertyName,
                                                    OldValue,
                                                    NewValue).
                                   ConfigureAwait(false);

            }

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
                await OnChargingStationDataChangedLocal(Timestamp, ChargingStation, PropertyName, OldValue, NewValue);

        }

        #endregion

        #region (internal) UpdateChargingStationStatus(Timestamp, ChargingStation, OldStatus, NewStatus)

        /// <summary>
        /// Update the current charging station admin status.
        /// </summary>
        /// <param name="Timestamp">The timestamp when this change was detected.</param>
        /// <param name="ChargingStation">The updated charging station.</param>
        /// <param name="OldStatus">The old charging station admin status.</param>
        /// <param name="NewStatus">The new charging station admin status.</param>
        internal async Task UpdateChargingStationStatus(DateTime                                Timestamp,
                                                        ChargingStation                         ChargingStation,
                                                        Timestamped<ChargingStationStatusTypes>  OldStatus,
                                                        Timestamped<ChargingStationStatusTypes>  NewStatus)
        {

            var OnChargingStationStatusChangedLocal = OnChargingStationStatusChanged;
            if (OnChargingStationStatusChangedLocal != null)
                await OnChargingStationStatusChangedLocal(Timestamp, ChargingStation, OldStatus, NewStatus);

        }

        #endregion

        #region (internal) UpdateChargingStationAdminStatus(Timestamp, ChargingStation, OldStatus, NewStatus)

        /// <summary>
        /// Update the current charging station admin status.
        /// </summary>
        /// <param name="Timestamp">The timestamp when this change was detected.</param>
        /// <param name="ChargingStation">The updated charging station.</param>
        /// <param name="OldStatus">The old aggreagted charging station status.</param>
        /// <param name="NewStatus">The new aggreagted charging station status.</param>
        internal async Task UpdateChargingStationAdminStatus(DateTime                                     Timestamp,
                                                             ChargingStation                              ChargingStation,
                                                             Timestamped<ChargingStationAdminStatusTypes>  OldStatus,
                                                             Timestamped<ChargingStationAdminStatusTypes>  NewStatus)
        {

            var OnChargingStationAdminStatusChangedLocal = OnChargingStationAdminStatusChanged;
            if (OnChargingStationAdminStatusChangedLocal != null)
                await OnChargingStationAdminStatusChangedLocal(Timestamp, ChargingStation, OldStatus, NewStatus);

        }

        #endregion

        #endregion

        #region EVSEs...

        #region EVSEs

        /// <summary>
        /// Return all EVSEs registered within this roaming network.
        /// </summary>
        public IEnumerable<EVSE> EVSEs

            => _ChargingStationOperators.SelectMany(cso => cso.SelectMany(pool => pool.SelectMany(station => station)));

        #endregion

        #region EVSEAdminStatus

        /// <summary>
        /// Return the admin status of all EVSEs registered within this roaming network.
        /// </summary>

        public IEnumerable<KeyValuePair<EVSE_Id, IEnumerable<Timestamped<EVSEAdminStatusType>>>> EVSEAdminStatus(UInt64 HistorySize)

            => _ChargingStationOperators.
                   SelectMany(cso =>
                       cso.SelectMany(pool =>
                           pool.SelectMany(station =>
                               station.Select(evse =>

                                   new KeyValuePair<EVSE_Id, IEnumerable<Timestamped<EVSEAdminStatusType>>>(
                                       evse.Id,
                                       evse.AdminStatusSchedule(HistorySize))

                               ))));

        #endregion

        #region EVSEStatus

        /// <summary>
        /// Return the status of all EVSEs registered within this roaming network.
        /// </summary>
        public IEnumerable<KeyValuePair<EVSE_Id, IEnumerable<Timestamped<EVSEStatusTypes>>>> EVSEStatus(UInt64 HistorySize)

            => _ChargingStationOperators.
                   SelectMany(cso =>
                       cso.SelectMany(pool =>
                           pool.SelectMany(station =>
                               station.Select(evse =>

                                   new KeyValuePair<EVSE_Id, IEnumerable<Timestamped<EVSEStatusTypes>>>(
                                       evse.Id,
                                       evse.StatusSchedule(HistorySize))

                               ))));

        #endregion


        #region ContainsEVSE(EVSE)

        /// <summary>
        /// Check if the given EVSE is already present within the roaming network.
        /// </summary>
        /// <param name="EVSE">An EVSE.</param>
        public Boolean ContainsEVSE(EVSE EVSE)
        {

            ChargingStationOperator _ChargingStationOperator  = null;

            if (TryGetChargingStationOperatorById(EVSE.Operator.Id, out _ChargingStationOperator))
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

            ChargingStationOperator _ChargingStationOperator  = null;

            if (TryGetChargingStationOperatorById(EVSEId.OperatorId, out _ChargingStationOperator))
                return _ChargingStationOperator.ContainsEVSE(EVSEId);

            return false;

        }

        #endregion

        #region GetEVSEbyId(EVSEId)

        public EVSE GetEVSEbyId(EVSE_Id EVSEId)
        {

            EVSE                    _EVSE                     = null;
            ChargingStationOperator _ChargingStationOperator  = null;

            if (TryGetChargingStationOperatorById(EVSEId.OperatorId, out _ChargingStationOperator) &&
                _ChargingStationOperator.TryGetEVSEbyId(EVSEId, out _EVSE))
                    return _EVSE;

            return null;

        }

        #endregion

        #region TryGetEVSEbyId(EVSEId, out EVSE)

        public Boolean TryGetEVSEbyId(EVSE_Id EVSEId, out EVSE EVSE)
        {

            ChargingStationOperator _ChargingStationOperator  = null;

            if (TryGetChargingStationOperatorById(EVSEId.OperatorId, out _ChargingStationOperator))
                return _ChargingStationOperator.TryGetEVSEbyId(EVSEId, out EVSE);

            EVSE = null;
            return false;

        }

        #endregion


        #region SetEVSEStatus(EVSEId, NewStatus)

        public void SetEVSEStatus(EVSE_Id                      EVSEId,
                                  Timestamped<EVSEStatusTypes>  NewStatus)
        {

            ChargingStationOperator _cso = null;

            if (TryGetChargingStationOperatorById(EVSEId.OperatorId, out _cso))
                _cso.SetEVSEStatus(EVSEId, NewStatus);

        }

        #endregion

        #region SetEVSEStatus(EVSEId, Timestamp, NewStatus)

        public void SetEVSEStatus(EVSE_Id         EVSEId,
                                  DateTime        Timestamp,
                                  EVSEStatusTypes  NewStatus)
        {

            ChargingStationOperator _cso = null;

            if (TryGetChargingStationOperatorById(EVSEId.OperatorId, out _cso))
                _cso.SetEVSEStatus(EVSEId, NewStatus);

        }

        #endregion

        #region SetEVSEStatus(EVSEId, StatusList)

        public void SetEVSEStatus(EVSE_Id                                   EVSEId,
                                  IEnumerable<Timestamped<EVSEStatusTypes>>  StatusList,
                                  ChangeMethods                             ChangeMethod  = ChangeMethods.Replace)
        {

            ChargingStationOperator _cso = null;

            if (TryGetChargingStationOperatorById(EVSEId.OperatorId, out _cso))
                _cso.SetEVSEStatus(EVSEId, StatusList, ChangeMethod);

        }

        #endregion


        #region SetEVSEAdminStatus(EVSEId, NewStatus)

        public void SetEVSEAdminStatus(EVSE_Id                           EVSEId,
                                       Timestamped<EVSEAdminStatusType>  NewAdminStatus)
        {

            ChargingStationOperator _cso = null;

            if (TryGetChargingStationOperatorById(EVSEId.OperatorId, out _cso))
                _cso.SetEVSEAdminStatus(EVSEId, NewAdminStatus);

        }

        #endregion

        #region SetEVSEAdminStatus(EVSEId, Timestamp, NewAdminStatus)

        public void SetEVSEAdminStatus(EVSE_Id              EVSEId,
                                       DateTime             Timestamp,
                                       EVSEAdminStatusType  NewAdminStatus)
        {

            ChargingStationOperator _cso = null;

            if (TryGetChargingStationOperatorById(EVSEId.OperatorId, out _cso))
                _cso.SetEVSEAdminStatus(EVSEId, NewAdminStatus);

        }

        #endregion

        #region SetEVSEAdminStatus(EVSEId, AdminStatusList)

        public void SetEVSEAdminStatus(EVSE_Id                                        EVSEId,
                                       IEnumerable<Timestamped<EVSEAdminStatusType>>  AdminStatusList,
                                       ChangeMethods                                  ChangeMethod  = ChangeMethods.Replace)
        {

            ChargingStationOperator _cso = null;

            if (TryGetChargingStationOperatorById(EVSEId.OperatorId, out _cso))
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

            //foreach (var AuthenticationService in _IeMobilityServiceProviders.
            //                                          OrderBy(AuthServiceWithPriority => AuthServiceWithPriority.Key).
            //                                          Select (AuthServiceWithPriority => AuthServiceWithPriority.Value))
            //{

            //    result = await AuthenticationService.PushEVSEStatus(new EVSEStatus(EVSE.Id, NewStatus.Value, NewStatus.Timestamp),
            //                                                        ActionType.update,
            //                                                        EVSE.Operator.Id);

            //}

            foreach (var iRemotePushData in _IRemotePushData.
                                                OrderBy(kvp => kvp.Key).
                                                Select (kvp => kvp.Value))
            {

                //result = await iRemotePushData.
                iRemotePushData.
                    SetStaticData(EVSE).
                    ConfigureAwait(false);

            }

            //foreach (var PushEVSEStatusService in _PushEVSEStatusToOperatorRoamingServices.
            //                                          OrderBy(AuthServiceWithPriority => AuthServiceWithPriority.Key).
            //                                          Select (AuthServiceWithPriority => AuthServiceWithPriority.Value))
            //{

            //    result = await PushEVSEStatusService.PushEVSEStatus(new EVSEStatus(EVSE.Id, NewStatus.Value, NewStatus.Timestamp),
            //                                                        ActionType.update,
            //                                                        EVSE.Operator.Id);

            //}


            EVSEAddition.SendNotification(Timestamp, ChargingStation, EVSE);

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

            //foreach (var AuthenticationService in _IeMobilityServiceProviders.
            //                                          OrderBy(AuthServiceWithPriority => AuthServiceWithPriority.Key).
            //                                          Select (AuthServiceWithPriority => AuthServiceWithPriority.Value))
            //{

            //    result = await AuthenticationService.PushEVSEStatus(new EVSEStatus(EVSE.Id, NewStatus.Value, NewStatus.Timestamp),
            //                                                        ActionType.update,
            //                                                        EVSE.Operator.Id);

            //}

            foreach (var iRemotePushData in _IRemotePushData.
                                                OrderBy(kvp => kvp.Key).
                                                Select (kvp => kvp.Value))
            {

                //result = await iRemotePushData.
                iRemotePushData.DeleteStaticData(EVSE).
                                   ConfigureAwait(false);

            }

            //foreach (var PushEVSEStatusService in _PushEVSEStatusToOperatorRoamingServices.
            //                                          OrderBy(AuthServiceWithPriority => AuthServiceWithPriority.Key).
            //                                          Select (AuthServiceWithPriority => AuthServiceWithPriority.Value))
            //{

            //    result = await PushEVSEStatusService.PushEVSEStatus(new EVSEStatus(EVSE.Id, NewStatus.Value, NewStatus.Timestamp),
            //                                                        ActionType.update,
            //                                                        EVSE.Operator.Id);

            //}


            EVSERemoval.SendNotification(Timestamp, ChargingStation, EVSE);

        }

        #endregion

        #region OnEVSEData/(Admin)StatusChanged

        /// <summary>
        /// An event fired whenever the static data of any subordinated EVSE changed.
        /// </summary>
        public event OnEVSEDataChangedDelegate         OnEVSEDataChanged;

        /// <summary>
        /// An event fired whenever the dynamic status of any subordinated EVSE changed.
        /// </summary>
        public event OnEVSEStatusChangedDelegate       OnEVSEStatusChanged;

        /// <summary>
        /// An event fired whenever the admin status of any subordinated EVSE changed.
        /// </summary>
        public event OnEVSEAdminStatusChangedDelegate  OnEVSEAdminStatusChanged;

        #endregion

        #region OnEVSEStatusDiff

        public delegate void OnEVSEStatusDiffDelegate(EVSEStatusDiff StatusDiff);

        /// <summary>
        /// An event fired whenever a EVSE status diff was received.
        /// </summary>
        public event OnEVSEStatusDiffDelegate OnEVSEStatusDiff;

        #endregion

        #region SocketOutletAddition

        internal readonly IVotingNotificator<DateTime, EVSE, SocketOutlet, Boolean> SocketOutletAddition;

        /// <summary>
        /// Called whenever a socket outlet will be or was added.
        /// </summary>
        public IVotingSender<DateTime, EVSE, SocketOutlet, Boolean> OnSocketOutletAddition

            => SocketOutletAddition;

        #endregion

        #region SocketOutletRemoval

        internal readonly IVotingNotificator<DateTime, EVSE, SocketOutlet, Boolean> SocketOutletRemoval;

        /// <summary>
        /// Called whenever a socket outlet will be or was removed.
        /// </summary>
        public IVotingSender<DateTime, EVSE, SocketOutlet, Boolean> OnSocketOutletRemoval

            => SocketOutletRemoval;

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
        internal async Task UpdateEVSEData(DateTime  Timestamp,
                                           EVSE      EVSE,
                                           String    PropertyName,
                                           Object    OldValue,
                                           Object    NewValue)
        {

            Acknowledgement result = null;


            //foreach (var AuthenticationService in _IeMobilityServiceProviders.
            //                                          OrderBy(AuthServiceWithPriority => AuthServiceWithPriority.Key).
            //                                          Select (AuthServiceWithPriority => AuthServiceWithPriority.Value))
            //{

            //    result = await AuthenticationService.PushEVSEStatus(new EVSEStatus(EVSE.Id, NewStatus.Value, NewStatus.Timestamp),
            //                                                        ActionType.update,
            //                                                        EVSE.Operator.Id);

            //}

            foreach (var iRemotePushData in _IRemotePushData.
                                                OrderBy(kvp => kvp.Key).
                                                Select (kvp => kvp.Value))
            {

                result = await iRemotePushData.
                                   UpdateStaticData(EVSE,
                                                    PropertyName,
                                                    OldValue,
                                                    NewValue).
                                   ConfigureAwait(false);

            }

            //foreach (var PushEVSEStatusService in _PushEVSEStatusToOperatorRoamingServices.
            //                                          OrderBy(AuthServiceWithPriority => AuthServiceWithPriority.Key).
            //                                          Select (AuthServiceWithPriority => AuthServiceWithPriority.Value))
            //{

            //    result = await PushEVSEStatusService.PushEVSEStatus(new EVSEStatus(EVSE.Id, NewStatus.Value, NewStatus.Timestamp),
            //                                                        ActionType.update,
            //                                                        EVSE.Operator.Id);

            //}


            var OnEVSEDataChangedLocal = OnEVSEDataChanged;
            if (OnEVSEDataChangedLocal != null)
                await OnEVSEDataChangedLocal(Timestamp, EVSE, PropertyName, OldValue, NewValue);

        }

        #endregion

        #region (internal) UpdateEVSEStatus(Timestamp, EventTrackingId, EVSE, OldStatus, NewStatus)

        /// <summary>
        /// Update an EVSE status.
        /// </summary>
        /// <param name="Timestamp">The timestamp when this change was detected.</param>
        /// <param name="EventTrackingId">An event tracking identification for correlating this request with other events.</param>
        /// <param name="EVSE">The updated EVSE.</param>
        /// <param name="OldStatus">The old EVSE status.</param>
        /// <param name="NewStatus">The new EVSE status.</param>
        internal async Task UpdateEVSEStatus(DateTime                     Timestamp,
                                             EventTracking_Id             EventTrackingId,
                                             EVSE                         EVSE,
                                             Timestamped<EVSEStatusTypes>  OldStatus,
                                             Timestamped<EVSEStatusTypes>  NewStatus)
        {

            Acknowledgement result = null;


            //foreach (IRemotePushStatus iRemotePushStatus in _EMobilityProviders.
            //                                                    OrderByDescending(provider => provider.Priority.Value))
            //{

            //    result = await iRemotePushStatus.
            //                       UpdateEVSEStatus(new EVSEStatusUpdate(EVSE.Id,
            //                                                             OldStatus,
            //                                                             NewStatus)).
            //                       ConfigureAwait(false);

            //}

            foreach (var iRemotePushStatus in _IRemotePushStatus.
                                                  OrderBy(kvp => kvp.Key).
                                                  Select (kvp => kvp.Value))
            {

                result = await iRemotePushStatus.
                                   UpdateEVSEStatus(new EVSEStatusUpdate(EVSE.Id,
                                                                         OldStatus,
                                                                         NewStatus)).
                                   ConfigureAwait(false);

            }

            //foreach (var iPushStatus in _PushEVSEStatusToOperatorRoamingServices.
            //                                          OrderBy(AuthServiceWithPriority => AuthServiceWithPriority.Key).
            //                                          Select (AuthServiceWithPriority => AuthServiceWithPriority.Value))
            //{

            //    result = await iPushStatus.
            //                       Update
            //                       PushEVSEStatus(new EVSEStatus(EVSE.Id,
            //                                                     NewStatus.Value,
            //                                                     NewStatus.Timestamp)).
            //                       ConfigureAwait(false);

            //}


            var OnEVSEStatusChangedLocal = OnEVSEStatusChanged;
            if (OnEVSEStatusChangedLocal != null)
                await OnEVSEStatusChangedLocal(Timestamp,
                                               EventTrackingId,
                                               EVSE,
                                               OldStatus,
                                               NewStatus);

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
        internal async Task UpdateEVSEAdminStatus(DateTime                          Timestamp,
                                                  EventTracking_Id                  EventTrackingId,
                                                  EVSE                              EVSE,
                                                  Timestamped<EVSEAdminStatusType>  OldStatus,
                                                  Timestamped<EVSEAdminStatusType>  NewStatus)
        {

            //Acknowledgement result = null;

            //if (!DisableStatusUpdates)
            //{

            //    foreach (var AuthenticationService in _IeMobilityServiceProviders.
            //                                              OrderBy(AuthServiceWithPriority => AuthServiceWithPriority.Key).
            //                                              Select (AuthServiceWithPriority => AuthServiceWithPriority.Value))
            //    {

            //        result = await AuthenticationService.PushEVSEAdminStatus(new EVSEAdminStatus(EVSE.Id, NewStatus.Value, NewStatus.Timestamp),
            //                                                                 ActionType.update,
            //                                                                 EVSE.Operator.Id);

            //    }

            //    foreach (var OperatorRoamingService in _OperatorRoamingServices.
            //                                              OrderBy(AuthServiceWithPriority => AuthServiceWithPriority.Key).
            //                                              Select (AuthServiceWithPriority => AuthServiceWithPriority.Value))
            //    {

            //        result = await OperatorRoamingService.PushEVSEAdminStatus(new EVSEAdminStatus(EVSE.Id, NewStatus.Value, NewStatus.Timestamp),
            //                                                                  ActionType.update,
            //                                                                  EVSE.Operator.Id);

            //    }

            //    foreach (var PushEVSEStatusService in _PushEVSEStatusToOperatorRoamingServices.
            //                                              OrderBy(AuthServiceWithPriority => AuthServiceWithPriority.Key).
            //                                              Select (AuthServiceWithPriority => AuthServiceWithPriority.Value))
            //    {

            //        result = await PushEVSEStatusService.PushEVSEAdminStatus(new EVSEAdminStatus(EVSE.Id, NewStatus.Value, NewStatus.Timestamp),
            //                                                                 ActionType.update,
            //                                                                 EVSE.Operator.Id);

            //    }

            //}

            var OnEVSEAdminStatusChangedLocal = OnEVSEAdminStatusChanged;
            if (OnEVSEAdminStatusChangedLocal != null)
                await OnEVSEAdminStatusChangedLocal(Timestamp,
                                                    EventTrackingId,
                                                    EVSE,
                                                    OldStatus,
                                                    NewStatus);

        }

        #endregion

        #endregion


        #region Reservations...

        public static readonly TimeSpan MaxReservationDuration = TimeSpan.FromMinutes(15);

        #region ChargingReservations

        private readonly ConcurrentDictionary<ChargingReservation_Id, ChargingStationOperator>  _ChargingReservations_AtChargingStationOperators;
        private readonly ConcurrentDictionary<ChargingReservation_Id, IEMPRoamingProvider>          _ChargingReservations_AtEMPRoamingProviders;

        /// <summary>
        /// Return all current charging reservations.
        /// </summary>
        public IEnumerable<ChargingReservation> ChargingReservations

            => _ChargingStationOperators.
                    SelectMany(cso => cso.ChargingReservations);

        #endregion

        #region OnReserve... / OnReserved... / OnNewReservation

        /// <summary>
        /// An event fired whenever an EVSE is being reserved.
        /// </summary>
        public event OnReserveEVSERequestDelegate              OnReserveEVSERequest;

        /// <summary>
        /// An event fired whenever an EVSE was reserved.
        /// </summary>
        public event OnReserveEVSEResponseDelegate             OnReserveEVSEResponse;

        /// <summary>
        /// An event fired whenever a charging station is being reserved.
        /// </summary>
        public event OnReserveChargingStationRequestDelegate   OnReserveChargingStationRequest;

        /// <summary>
        /// An event fired whenever a charging station was reserved.
        /// </summary>
        public event OnReserveChargingStationResponseDelegate  OnReserveChargingStationResponse;

        /// <summary>
        /// An event fired whenever a charging pool is being reserved.
        /// </summary>
        public event OnReserveChargingPoolRequestDelegate      OnReserveChargingPoolRequest;

        /// <summary>
        /// An event fired whenever a charging pool was reserved.
        /// </summary>
        public event OnReserveChargingPoolResponseDelegate     OnReserveChargingPoolResponse;

        /// <summary>
        /// An event fired whenever a new charging reservation was created.
        /// </summary>
        public event OnNewReservationDelegate                  OnNewReservation;

        #endregion

        #region Reserve(...EVSEId, StartTime, Duration, ReservationId = null, ProviderId = null, ...)

        /// <summary>
        /// Reserve the possibility to charge at the given EVSE.
        /// </summary>
        /// <param name="EVSEId">The unique identification of the EVSE to be reserved.</param>
        /// <param name="StartTime">The starting time of the reservation.</param>
        /// <param name="Duration">The duration of the reservation.</param>
        /// <param name="ReservationId">An optional unique identification of the reservation. Mandatory for updates.</param>
        /// <param name="ProviderId">An optional unique identification of e-Mobility service provider.</param>
        /// <param name="eMAId">An optional unique identification of e-Mobility account/customer requesting this reservation.</param>
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

            Reserve(EVSE_Id                           EVSEId,
                    DateTime?                         StartTime           = null,
                    TimeSpan?                         Duration            = null,
                    ChargingReservation_Id?           ReservationId       = null,
                    eMobilityProvider_Id?             ProviderId          = null,
                    eMobilityAccount_Id?              eMAId               = null,
                    ChargingProduct                   ChargingProduct     = null,
                    IEnumerable<Auth_Token>           AuthTokens          = null,
                    IEnumerable<eMobilityAccount_Id>  eMAIds              = null,
                    IEnumerable<UInt32>               PINs                = null,

                    DateTime?                         Timestamp           = null,
                    CancellationToken?                CancellationToken   = null,
                    EventTracking_Id                  EventTrackingId     = null,
                    TimeSpan?                         RequestTimeout      = null)

        {

            #region Initial checks

            if (EVSEId == null)
                throw new ArgumentNullException(nameof(EVSEId),  "The given EVSE identification must not be null!");

            ChargingStationOperator _ChargingStationOperator  = null;
            ReservationResult       result                    = null;

            if (!Timestamp.HasValue)
                Timestamp = DateTime.Now;

            if (!CancellationToken.HasValue)
                CancellationToken = new CancellationTokenSource().Token;

            if (EventTrackingId == null)
                EventTrackingId = EventTracking_Id.New;

            #endregion

            #region Send OnReserveEVSERequest event

            var Runtime = Stopwatch.StartNew();

            try
            {

                OnReserveEVSERequest?.Invoke(DateTime.Now,
                                             Timestamp.Value,
                                             this,
                                             EventTrackingId,
                                             Id,
                                             ReservationId,
                                             EVSEId,
                                             StartTime,
                                             Duration,
                                             ProviderId,
                                             eMAId,
                                             ChargingProduct,
                                             AuthTokens,
                                             eMAIds,
                                             PINs,
                                             RequestTimeout);

            }
            catch (Exception e)
            {
                e.Log(nameof(RoamingNetwork) + "." + nameof(OnReserveEVSERequest));
            }

            #endregion


            if (TryGetChargingStationOperatorById(EVSEId.OperatorId, out _ChargingStationOperator))
            {

                result = await _ChargingStationOperator.
                                   Reserve(EVSEId,
                                           StartTime,
                                           Duration,
                                           ReservationId,
                                           ProviderId,
                                           eMAId,
                                           ChargingProduct,
                                           AuthTokens,
                                           eMAIds,
                                           PINs,

                                           Timestamp,
                                           CancellationToken,
                                           EventTrackingId,
                                           RequestTimeout);

                if (result.Result == ReservationResultType.Success)
                    _ChargingReservations_AtChargingStationOperators.TryAdd(result.Reservation.Id, _ChargingStationOperator);

            }

            if (result == null || result.Result == ReservationResultType.UnknownEVSE)
            {

                foreach (var EMPRoamingService in _EMPRoamingProviders.
                                                      OrderBy(EMPRoamingServiceWithPriority => EMPRoamingServiceWithPriority.Key).
                                                      Select (EMPRoamingServiceWithPriority => EMPRoamingServiceWithPriority.Value))
                {

                    result = await EMPRoamingService.
                                       Reserve(EVSEId,
                                               StartTime,
                                               Duration,
                                               ReservationId,
                                               ProviderId,
                                               eMAId,
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
                            _ChargingReservations_AtEMPRoamingProviders.TryAdd(result.Reservation.Id, EMPRoamingService);

                    }

                }

            }

            if (result == null)
                result = ReservationResult.UnknownChargingStationOperator;


            #region Send OnReserveEVSEResponse event

            Runtime.Stop();

            try
            {

                OnReserveEVSEResponse?.Invoke(DateTime.Now,
                                              Timestamp.Value,
                                              this,
                                              EventTrackingId,
                                              Id,
                                              ReservationId,
                                              EVSEId,
                                              StartTime,
                                              Duration,
                                              ProviderId,
                                              eMAId,
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
                e.Log(nameof(RoamingNetwork) + "." + nameof(OnReserveEVSEResponse));
            }

            #endregion

            return result;

        }

        #endregion

        #region Reserve(...ChargingStationId, StartTime, Duration, ReservationId = null, ProviderId = null, ...)

        /// <summary>
        /// Reserve the possibility to charge at the given charging station.
        /// </summary>
        /// <param name="ChargingStationId">The unique identification of the charging station to be reserved.</param>
        /// <param name="StartTime">The starting time of the reservation.</param>
        /// <param name="Duration">The duration of the reservation.</param>
        /// <param name="ReservationId">An optional unique identification of the reservation. Mandatory for updates.</param>
        /// <param name="ProviderId">An optional unique identification of e-Mobility service provider.</param>
        /// <param name="eMAId">An optional unique identification of e-Mobility account/customer requesting this reservation.</param>
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

            Reserve(ChargingStation_Id                ChargingStationId,
                    DateTime?                         StartTime           = null,
                    TimeSpan?                         Duration            = null,
                    ChargingReservation_Id?           ReservationId       = null,
                    eMobilityProvider_Id?             ProviderId          = null,
                    eMobilityAccount_Id?              eMAId               = null,
                    ChargingProduct                   ChargingProduct     = null,
                    IEnumerable<Auth_Token>           AuthTokens          = null,
                    IEnumerable<eMobilityAccount_Id>  eMAIds              = null,
                    IEnumerable<UInt32>               PINs                = null,

                    DateTime?                         Timestamp           = null,
                    CancellationToken?                CancellationToken   = null,
                    EventTracking_Id                  EventTrackingId     = null,
                    TimeSpan?                         RequestTimeout      = null)

        {

            #region Initial checks

            if (ChargingStationId == null)
                throw new ArgumentNullException(nameof(ChargingStationId),  "The given charging station identification must not be null!");

            ChargingStationOperator _ChargingStationOperator  = null;
            ReservationResult       result                    = null;

            if (!Timestamp.HasValue)
                Timestamp = DateTime.Now;

            if (!CancellationToken.HasValue)
                CancellationToken = new CancellationTokenSource().Token;

            if (EventTrackingId == null)
                EventTrackingId = EventTracking_Id.New;

            #endregion

            #region Send OnReserveChargingStationRequest event

            var Runtime = Stopwatch.StartNew();

            try
            {

                OnReserveChargingStationRequest?.Invoke(DateTime.Now,
                                                        Timestamp.Value,
                                                        this,
                                                        EventTrackingId,
                                                        Id,
                                                        ChargingStationId,
                                                        StartTime,
                                                        Duration,
                                                        ReservationId,
                                                        ProviderId,
                                                        eMAId,
                                                        ChargingProduct,
                                                        AuthTokens,
                                                        eMAIds,
                                                        PINs,
                                                        RequestTimeout);

            }
            catch (Exception e)
            {
                e.Log(nameof(RoamingNetwork) + "." + nameof(OnReserveChargingStationRequest));
            }

            #endregion


            if (TryGetChargingStationOperatorById(ChargingStationId.OperatorId, out _ChargingStationOperator))
            {

                result = await _ChargingStationOperator.
                                   Reserve(ChargingStationId,
                                           StartTime,
                                           Duration,
                                           ReservationId,
                                           ProviderId,
                                           eMAId,
                                           ChargingProduct,
                                           AuthTokens,
                                           eMAIds,
                                           PINs,

                                           Timestamp,
                                           CancellationToken,
                                           EventTrackingId,
                                           RequestTimeout);

                if (result.Result == ReservationResultType.Success)
                    _ChargingReservations_AtChargingStationOperators.TryAdd(result.Reservation.Id, _ChargingStationOperator);

            }

            else
                result = ReservationResult.UnknownChargingStationOperator;


            #region Send OnReserveChargingStationResponse event

            Runtime.Stop();

            try
            {

                OnReserveChargingStationResponse?.Invoke(DateTime.Now,
                                                         Timestamp.Value,
                                                         this,
                                                         EventTrackingId,
                                                         Id,
                                                         ChargingStationId,
                                                         StartTime,
                                                         Duration,
                                                         ReservationId,
                                                         ProviderId,
                                                         eMAId,
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
                e.Log(nameof(RoamingNetwork) + "." + nameof(OnReserveChargingStationResponse));
            }

            #endregion

            return result;

        }

        #endregion

        #region Reserve(...ChargingPoolId, StartTime, Duration, ReservationId = null, ProviderId = null, ...)

        /// <summary>
        /// Reserve the possibility to charge within the given charging pool.
        /// </summary>
        /// <param name="ChargingPoolId">The unique identification of the charging pool to be reserved.</param>
        /// <param name="StartTime">The starting time of the reservation.</param>
        /// <param name="Duration">The duration of the reservation.</param>
        /// <param name="ReservationId">An optional unique identification of the reservation. Mandatory for updates.</param>
        /// <param name="ProviderId">An optional unique identification of e-Mobility service provider.</param>
        /// <param name="eMAId">An optional unique identification of e-Mobility account/customer requesting this reservation.</param>
        /// <param name="ChargingProductId">An optional unique identification of the charging product to be reserved.</param>
        /// <param name="AuthTokens">A list of authentication tokens, who can use this reservation.</param>
        /// <param name="eMAIds">A list of eMobility account identifications, who can use this reservation.</param>
        /// <param name="PINs">A list of PINs, who can be entered into a pinpad to use this reservation.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public async Task<ReservationResult>

            Reserve(ChargingPool_Id                   ChargingPoolId,
                    DateTime?                         StartTime           = null,
                    TimeSpan?                         Duration            = null,
                    ChargingReservation_Id?           ReservationId       = null,
                    eMobilityProvider_Id?             ProviderId          = null,
                    eMobilityAccount_Id?              eMAId               = null,
                    ChargingProduct                   ChargingProduct     = null,
                    IEnumerable<Auth_Token>           AuthTokens          = null,
                    IEnumerable<eMobilityAccount_Id>  eMAIds              = null,
                    IEnumerable<UInt32>               PINs                = null,

                    DateTime?                         Timestamp           = null,
                    CancellationToken?                CancellationToken   = null,
                    EventTracking_Id                  EventTrackingId     = null,
                    TimeSpan?                         RequestTimeout      = null)

        {

            #region Initial checks

            if (ChargingPoolId == null)
                throw new ArgumentNullException(nameof(ChargingPoolId),  "The given charging pool identification must not be null!");

            ChargingStationOperator _ChargingStationOperator  = null;
            ReservationResult       result                    = null;

            if (!Timestamp.HasValue)
                Timestamp = DateTime.Now;

            if (!CancellationToken.HasValue)
                CancellationToken = new CancellationTokenSource().Token;

            if (EventTrackingId == null)
                EventTrackingId = EventTracking_Id.New;

            #endregion

            #region Send OnReserveChargingPoolRequest event

            var Runtime = Stopwatch.StartNew();

            try
            {

                OnReserveChargingPoolRequest?.Invoke(DateTime.Now,
                                                     Timestamp.Value,
                                                     this,
                                                     EventTrackingId,
                                                     Id,
                                                     ChargingPoolId,
                                                     StartTime,
                                                     Duration,
                                                     ReservationId,
                                                     ProviderId,
                                                     eMAId,
                                                     ChargingProduct,
                                                     AuthTokens,
                                                     eMAIds,
                                                     PINs,
                                                     RequestTimeout);

            }
            catch (Exception e)
            {
                e.Log(nameof(RoamingNetwork) + "." + nameof(OnReserveChargingPoolRequest));
            }

            #endregion


            if (TryGetChargingStationOperatorById(ChargingPoolId.OperatorId, out _ChargingStationOperator))
            {

                result = await _ChargingStationOperator.
                                   Reserve(ChargingPoolId,
                                           StartTime,
                                           Duration,
                                           ReservationId,
                                           ProviderId,
                                           eMAId,
                                           ChargingProduct,
                                           AuthTokens,
                                           eMAIds,
                                           PINs,

                                           Timestamp,
                                           CancellationToken,
                                           EventTrackingId,
                                           RequestTimeout);

                if (result.Result == ReservationResultType.Success)
                    _ChargingReservations_AtChargingStationOperators.TryAdd(result.Reservation.Id, _ChargingStationOperator);

            }

            else
                result = ReservationResult.UnknownChargingStationOperator;


            #region Send OnReserveChargingPoolResponse event

            Runtime.Stop();

            try
            {

                OnReserveChargingPoolResponse?.Invoke(DateTime.Now,
                                                      Timestamp.Value,
                                                      this,
                                                      EventTrackingId,
                                                      Id,
                                                      ChargingPoolId,
                                                      StartTime,
                                                      Duration,
                                                      ReservationId,
                                                      ProviderId,
                                                      eMAId,
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
                e.Log(nameof(RoamingNetwork) + "." + nameof(OnReserveChargingPoolResponse));
            }

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

            ChargingStationOperator _cso = null;

            if (_ChargingReservations_AtChargingStationOperators.TryGetValue(ReservationId, out _cso))
                return _cso.TryGetReservationById(ReservationId, out Reservation);

            Reservation = null;
            return false;

        }

        #endregion


        #region CancelReservation(...ReservationId, Reason, ProviderId = null, EVSEId = null, ...)

        /// <summary>
        /// Cancel the given charging reservation.
        /// </summary>
        /// <param name="ReservationId">The unique charging reservation identification.</param>
        /// <param name="Reason">A reason for this cancellation.</param>
        /// <param name="ProviderId">An optional unique identification of e-Mobility service provider.</param>
        /// <param name="EVSEId">An optional identification of the EVSE.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public async Task<CancelReservationResult>

            CancelReservation(ChargingReservation_Id                 ReservationId,
                              ChargingReservationCancellationReason  Reason,
                              eMobilityProvider_Id?                  ProviderId          = null,
                              EVSE_Id?                               EVSEId              = null,

                              DateTime?                              Timestamp           = null,
                              CancellationToken?                     CancellationToken   = null,
                              EventTracking_Id                       EventTrackingId     = null,
                              TimeSpan?                              RequestTimeout      = null)

        {

            #region Initial checks

            ChargingStationOperator _ChargingStationOperator  = null;
            CancelReservationResult result                    = null;

            if (!Timestamp.HasValue)
                Timestamp = DateTime.Now;

            if (!CancellationToken.HasValue)
                CancellationToken = new CancellationTokenSource().Token;

            if (EventTrackingId == null)
                EventTrackingId = EventTracking_Id.New;

            #endregion


            #region Check Charging Station Operator charging reservation lookup...

            if (_ChargingReservations_AtChargingStationOperators.TryRemove(ReservationId, out _ChargingStationOperator))
            {

                result = await _ChargingStationOperator.CancelReservation(ReservationId,
                                                                          Reason,
                                                                          ProviderId,
                                                                          EVSEId,

                                                                          Timestamp,
                                                                          CancellationToken,
                                                                          EventTrackingId,
                                                                          RequestTimeout);

            }

            #endregion

            #region ...or check EMP roaming provider charging reservation lookup...

            if (result        == null                                    ||
                result.Result == CancelReservationResults.UnknownEVSE ||
                result.Result == CancelReservationResults.UnknownReservationId)
            {

                IEMPRoamingProvider _IEMPRoamingProvider = null;

                if (_ChargingReservations_AtEMPRoamingProviders.TryRemove(ReservationId, out _IEMPRoamingProvider))
                {

                    result = await _IEMPRoamingProvider.
                                       CancelReservation(ReservationId,
                                                         Reason,
                                                         ProviderId,
                                                         EVSEId,

                                                         Timestamp,
                                                         CancellationToken,
                                                         EventTrackingId,
                                                         RequestTimeout);

                }

            }

            #endregion

            #region ...or try to check every EMP roaming provider...

            if (result == null ||
                result.Result == CancelReservationResults.UnknownEVSE ||
                result.Result == CancelReservationResults.UnknownReservationId)
            {

                foreach (var EMPRoamingService in _EMPRoamingProviders.
                                                      OrderBy(EMPRoamingServiceWithPriority => EMPRoamingServiceWithPriority.Key).
                                                      Select(EMPRoamingServiceWithPriority => EMPRoamingServiceWithPriority.Value))
                {

                    result = await EMPRoamingService.
                                       CancelReservation(ReservationId,
                                                         Reason,
                                                         ProviderId,
                                                         EVSEId,

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

                SendOnReservationCancelled(DateTime.Now,
                                           Timestamp.HasValue
                                               ? Timestamp.Value
                                               : DateTime.Now,
                                           this,
                                           EventTrackingId,
                                           Id,
                                           ProviderId,
                                           ReservationId,
                                           null,
                                           Reason,
                                           result,
                                           result.Runtime.HasValue
                                               ? result.Runtime.Value
                                               : TimeSpan.FromMilliseconds(5),
                                           RequestTimeout);

            }

            #endregion


            return result;

        }

        #endregion

        #region OnReservationCancelled

        /// <summary>
        /// An event fired whenever a charging reservation was deleted.
        /// </summary>
        public event OnCancelReservationResponseDelegate OnReservationCancelled;

        #endregion

        #region SendOnReservationCancelled(...)

        internal Task SendOnReservationCancelled(DateTime                               LogTimestamp,
                                                 DateTime                               RequestTimestamp,
                                                 Object                                 Sender,
                                                 EventTracking_Id                       EventTrackingId,

                                                 RoamingNetwork_Id?                     RoamingNetworkId,
                                                 eMobilityProvider_Id?                  ProviderId,
                                                 ChargingReservation_Id                 ReservationId,
                                                 ChargingReservation                    Reservation,
                                                 ChargingReservationCancellationReason  Reason,
                                                 CancelReservationResult                Result,
                                                 TimeSpan                               Runtime,
                                                 TimeSpan?                              RequestTimeout)
        {

            ChargingStationOperator _Operator = null;

            _ChargingReservations_AtChargingStationOperators.TryRemove(ReservationId, out _Operator);

            return OnReservationCancelled?.Invoke(LogTimestamp,
                                                  RequestTimestamp,
                                                  Sender,
                                                  EventTrackingId,
                                                  RoamingNetworkId.HasValue ? RoamingNetworkId : Id,
                                                  ProviderId,
                                                  ReservationId,
                                                  Reservation,
                                                  Reason,
                                                  Result,
                                                  Runtime,
                                                  RequestTimeout);

        }

        #endregion

        #endregion

        #region RemoteStart/-Stop

        #region OnRemote...Start / OnRemote...Started

        /// <summary>
        /// An event fired whenever a remote start EVSE command was received.
        /// </summary>
        public event OnRemoteStartEVSERequestDelegate              OnRemoteEVSEStartRequest;

        /// <summary>
        /// An event fired whenever a remote start EVSE command completed.
        /// </summary>
        public event OnRemoteStartEVSEResponseDelegate             OnRemoteEVSEStartResponse;

        /// <summary>
        /// An event fired whenever a remote start charging station command was received.
        /// </summary>
        public event OnRemoteChargingStationStartRequestDelegate   OnRemoteChargingStationStartRequest;

        /// <summary>
        /// An event fired whenever a remote start charging station command completed.
        /// </summary>
        public event OnRemoteChargingStationStartResponseDelegate  OnRemoteChargingStationStartResponse;

        #endregion

        #region RemoteStart(EVSEId,            ChargingProduct = null, ReservationId = null, SessionId = null, ProviderId = null, eMAId = null, ...)

        /// <summary>
        /// Start a charging session at the given EVSE.
        /// </summary>
        /// <param name="EVSEId">The unique identification of the EVSE to be started.</param>
        /// <param name="ChargingProduct">The choosen charging product.</param>
        /// <param name="ReservationId">Use the given optinal unique charging reservation identification for charging.</param>
        /// <param name="SessionId">The unique identification for this charging session.</param>
        /// <param name="ProviderId">The unique identification of the e-mobility service provider for the case it is different from the current message sender.</param>
        /// <param name="eMAId">The unique identification of the e-mobility account.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public async Task<RemoteStartEVSEResult>

            RemoteStart(EVSE_Id                  EVSEId,
                        ChargingProduct          ChargingProduct     = null,
                        ChargingReservation_Id?  ReservationId       = null,
                        ChargingSession_Id?      SessionId           = null,
                        eMobilityProvider_Id?    ProviderId          = null,
                        eMobilityAccount_Id?     eMAId               = null,

                        DateTime?                Timestamp           = null,
                        CancellationToken?       CancellationToken   = null,
                        EventTracking_Id         EventTrackingId     = null,
                        TimeSpan?                RequestTimeout      = null)

        {

            #region Initial checks

            if (!Timestamp.HasValue)
                Timestamp = DateTime.Now;

            if (!CancellationToken.HasValue)
                CancellationToken = new CancellationTokenSource().Token;

            if (EventTrackingId == null)
                EventTrackingId = EventTracking_Id.New;


            RemoteStartEVSEResult result = null;

            #endregion

            #region Send OnRemoteEVSEStartRequest event

            var Runtime = Stopwatch.StartNew();

            try
            {

                OnRemoteEVSEStartRequest?.Invoke(DateTime.Now,
                                                 Timestamp.Value,
                                                 this,
                                                 EventTrackingId,
                                                 Id,
                                                 EVSEId,
                                                 ChargingProduct,
                                                 ReservationId,
                                                 SessionId,
                                                 ProviderId,
                                                 eMAId,
                                                 RequestTimeout);

            }
            catch (Exception e)
            {
                e.Log(nameof(RoamingNetwork) + "." + nameof(OnRemoteEVSEStartRequest));
            }

            #endregion


            #region Try to lookup the charging station operator given in the EVSE identification...

            ChargingStationOperator _ChargingStationOperator;

            if (TryGetChargingStationOperatorById(EVSEId.OperatorId, out _ChargingStationOperator))
            {

                result = await _ChargingStationOperator.
                                   RemoteStart(EVSEId,
                                               ChargingProduct,
                                               ReservationId,
                                               SessionId,
                                               ProviderId,
                                               eMAId,

                                               Timestamp,
                                               CancellationToken,
                                               EventTrackingId,
                                               RequestTimeout);


                if (result.Result == RemoteStartEVSEResultType.Success)
                    _ChargingSessions.TryAdd(result.Session.Id,
                                             result.Session.SetChargingStationOperator(_ChargingStationOperator));

            }

            #endregion

            #region ...or try every EMP roaming provider...

            if (result        == null ||
                result.Result == RemoteStartEVSEResultType.UnknownEVSE)
            {

                foreach (var _EMPRoamingProvier in _EMPRoamingProviders.
                                                       OrderBy(EMPRoamingServiceWithPriority => EMPRoamingServiceWithPriority.Key).
                                                       Select (EMPRoamingServiceWithPriority => EMPRoamingServiceWithPriority.Value))
                {

                    result = await _EMPRoamingProvier.
                                       RemoteStart(EVSEId,
                                                   ChargingProduct,
                                                   ReservationId,
                                                   SessionId,
                                                   ProviderId,
                                                   eMAId,

                                                   Timestamp,
                                                   CancellationToken,
                                                   EventTrackingId,
                                                   RequestTimeout);


                    if (result.Result == RemoteStartEVSEResultType.Success)
                        _ChargingSessions.TryAdd(result.Session.Id,
                                                 result.Session.SetEMPRoamingProvider(_EMPRoamingProvier));

                }

            }

            #endregion

            #region ...or fail!

            if (result == null)
                result = RemoteStartEVSEResult.UnknownOperator;

            #endregion


            #region Send OnRemoteEVSEStartResponse event

            Runtime.Stop();

            try
            {

                OnRemoteEVSEStartResponse?.Invoke(DateTime.Now,
                                                  Timestamp.Value,
                                                  this,
                                                  EventTrackingId,
                                                  Id,
                                                  EVSEId,
                                                  ChargingProduct,
                                                  ReservationId,
                                                  SessionId,
                                                  ProviderId,
                                                  eMAId,
                                                  RequestTimeout,
                                                  result,
                                                  Runtime.Elapsed);

            }
            catch (Exception e)
            {
                e.Log(nameof(RoamingNetwork) + "." + nameof(OnRemoteEVSEStartResponse));
            }

            #endregion

            return result;

        }

        #endregion

        #region RemoteStart(ChargingStationId, ChargingProduct = null, ReservationId = null, SessionId = null, ProviderId = null, eMAId = null, ...)

        /// <summary>
        /// Start a charging session at the given charging station.
        /// </summary>
        /// <param name="ChargingStationId">The unique identification of the charging station to be started.</param>
        /// <param name="ChargingProduct">The choosen charging product.</param>
        /// <param name="ReservationId">The unique identification for a charging reservation.</param>
        /// <param name="SessionId">The unique identification for this charging session.</param>
        /// <param name="ProviderId">The unique identification of the e-mobility service provider for the case it is different from the current message sender.</param>
        /// <param name="eMAId">The unique identification of the e-mobility account.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public async Task<RemoteStartChargingStationResult>

            RemoteStart(ChargingStation_Id       ChargingStationId,
                        ChargingProduct          ChargingProduct     = null,
                        ChargingReservation_Id?  ReservationId       = null,
                        ChargingSession_Id?      SessionId           = null,
                        eMobilityProvider_Id?    ProviderId          = null,
                        eMobilityAccount_Id?     eMAId               = null,

                        DateTime?                Timestamp           = null,
                        CancellationToken?       CancellationToken   = null,
                        EventTracking_Id         EventTrackingId     = null,
                        TimeSpan?                RequestTimeout      = null)

        {

            #region Initial checks

            if (!Timestamp.HasValue)
                Timestamp = DateTime.Now;

            if (!CancellationToken.HasValue)
                CancellationToken = new CancellationTokenSource().Token;

            if (EventTrackingId == null)
                EventTrackingId = EventTracking_Id.New;


            RemoteStartChargingStationResult result = null;

            #endregion

            #region Send OnRemoteChargingStationStartRequest event

            var Runtime = Stopwatch.StartNew();

            try
            {

                OnRemoteChargingStationStartRequest?.Invoke(DateTime.Now,
                                                            Timestamp.Value,
                                                            this,
                                                            EventTrackingId,
                                                            Id,
                                                            ChargingStationId,
                                                            ChargingProduct,
                                                            ReservationId,
                                                            SessionId,
                                                            ProviderId,
                                                            eMAId,
                                                            RequestTimeout);

            }
            catch (Exception e)
            {
                e.Log(nameof(RoamingNetwork) + "." + nameof(OnRemoteChargingStationStartRequest));
            }

            #endregion


            #region Try to lookup the charging station operator given in the charging station identification...

            ChargingStationOperator _ChargingStationOperator;

            if (TryGetChargingStationOperatorById(ChargingStationId.OperatorId, out _ChargingStationOperator))
            {

                result = await _ChargingStationOperator.
                                   RemoteStart(ChargingStationId,
                                               ChargingProduct,
                                               ReservationId,
                                               SessionId,
                                               ProviderId,
                                               eMAId,

                                               Timestamp,
                                               CancellationToken,
                                               EventTrackingId,
                                               RequestTimeout);


                if (result.Result == RemoteStartChargingStationResultType.Success)
                    _ChargingSessions.TryAdd(result.Session.Id,
                                             result.Session.SetChargingStationOperator(_ChargingStationOperator));

            }

            #endregion

            #region ...or try every EMP roaming provider...

            if (result        == null ||
                result.Result == RemoteStartChargingStationResultType.UnknownChargingStation)
            {

                foreach (var _EMPRoamingProvider in _EMPRoamingProviders.
                                                      OrderBy(EMPRoamingServiceWithPriority => EMPRoamingServiceWithPriority.Key).
                                                      Select (EMPRoamingServiceWithPriority => EMPRoamingServiceWithPriority.Value))
                {

                    result = await _EMPRoamingProvider.
                                       RemoteStart(ChargingStationId,
                                                   ChargingProduct,
                                                   ReservationId,
                                                   SessionId,
                                                   ProviderId,
                                                   eMAId,

                                                   Timestamp,
                                                   CancellationToken,
                                                   EventTrackingId,
                                                   RequestTimeout);


                    if (result.Result == RemoteStartChargingStationResultType.Success)
                        _ChargingSessions.TryAdd(result.Session.Id,
                                                 result.Session.SetEMPRoamingProvider(_EMPRoamingProvider));

                }

            }

            #endregion

            #region ...or fail!

            if (result == null)
                result = RemoteStartChargingStationResult.UnknownOperator;

            #endregion


            #region Send OnRemoteChargingStationStartResponse event

            try
            {

                OnRemoteChargingStationStartResponse?.Invoke(DateTime.Now,
                                                             Timestamp.Value,
                                                             this,
                                                             EventTrackingId,
                                                             Id,
                                                             ChargingStationId,
                                                             ChargingProduct,
                                                             ReservationId,
                                                             SessionId,
                                                             ProviderId,
                                                             eMAId,
                                                             RequestTimeout,
                                                             result,
                                                             Runtime.Elapsed);

            }
            catch (Exception e)
            {
                e.Log(nameof(RoamingNetwork) + "." + nameof(OnRemoteChargingStationStartResponse));
            }

            #endregion

            return result;

        }

        #endregion


        #region OnRemote...Stop / OnRemote...Stopped

        /// <summary>
        /// An event fired whenever a remote stop command was received.
        /// </summary>
        public event OnRemoteStopRequestDelegate                  OnRemoteStopRequest;

        /// <summary>
        /// An event fired whenever a remote stop command completed.
        /// </summary>
        public event OnRemoteStopResponseDelegate                 OnRemoteStopResponse;

        /// <summary>
        /// An event fired whenever a remote stop EVSE command was received.
        /// </summary>
        public event OnRemoteStopEVSERequestDelegate              OnRemoteEVSEStopRequest;

        /// <summary>
        /// An event fired whenever a remote stop EVSE command completed.
        /// </summary>
        public event OnRemoteStopEVSEResponseDelegate             OnRemoteEVSEStopResponse;

        /// <summary>
        /// An event fired whenever a remote stop charging station command was received.
        /// </summary>
        public event OnRemoteChargingStationStopRequestDelegate   OnRemoteChargingStationStopRequest;

        /// <summary>
        /// An event fired whenever a remote stop charging station command completed.
        /// </summary>
        public event OnRemoteChargingStationStopResponseDelegate  OnRemoteChargingStationStopResponse;

        #endregion

        #region RemoteStop(                   SessionId, ReservationHandling, ProviderId = null, eMAId = null, ...)

        /// <summary>
        /// Stop the given charging session.
        /// </summary>
        /// <param name="SessionId">The unique identification for this charging session.</param>
        /// <param name="ReservationHandling">Whether to remove the reservation after session end, or to keep it open for some more time.</param>
        /// <param name="ProviderId">The unique identification of the e-mobility service provider.</param>
        /// <param name="eMAId">The unique identification of the e-mobility account.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public async Task<RemoteStopResult>

            RemoteStop(ChargingSession_Id     SessionId,
                       ReservationHandling    ReservationHandling,
                       eMobilityProvider_Id?  ProviderId          = null,
                       eMobilityAccount_Id?   eMAId               = null,

                       DateTime?              Timestamp           = null,
                       CancellationToken?     CancellationToken   = null,
                       EventTracking_Id       EventTrackingId     = null,
                       TimeSpan?              RequestTimeout      = null)

        {

            #region Initial checks

            if (!Timestamp.HasValue)
                Timestamp = DateTime.Now;

            if (!CancellationToken.HasValue)
                CancellationToken = new CancellationTokenSource().Token;

            if (EventTrackingId == null)
                EventTrackingId = EventTracking_Id.New;


            RemoteStopResult result = null;

            #endregion

            #region Send OnRemoteStopRequest event

            var Runtime = Stopwatch.StartNew();

            try
            {

                OnRemoteStopRequest?.Invoke(DateTime.Now,
                                            Timestamp.Value,
                                            this,
                                            EventTrackingId,
                                            Id,
                                            SessionId,
                                            ReservationHandling,
                                            ProviderId,
                                            eMAId,
                                            RequestTimeout);

            }
            catch (Exception e)
            {
                e.Log(nameof(RoamingNetwork) + "." + nameof(OnRemoteStopRequest));
            }

            #endregion


            #region Check charging station operator charging session lookup...

            ChargingSession _ChargingSession;

            if (_ChargingSessions.TryRemove(SessionId, out _ChargingSession))
            {

                result = await _ChargingSession?.ChargingStationOperator?.
                                   RemoteStop(SessionId,
                                              ReservationHandling,
                                              ProviderId,
                                              eMAId,

                                              Timestamp,
                                              CancellationToken,
                                              EventTrackingId,
                                              RequestTimeout);

            }

            #endregion

            #region ...or check EMP roaming provider charging session lookup...

            if (result        == null ||
                result.Result == RemoteStopResultType.InvalidSessionId)
            {

                result = await _ChargingSession?.EMPRoamingProvider?.
                                   RemoteStop(SessionId,
                                              ReservationHandling,
                                              ProviderId,
                                              eMAId,

                                              Timestamp,
                                              CancellationToken,
                                              EventTrackingId,
                                              RequestTimeout);

            }

            #endregion

            #region ...or try to check every charging station operator...

            if (result        == null ||
                result.Result == RemoteStopResultType.InvalidSessionId)
            {

                foreach (var Operator in _ChargingStationOperators)
                {

                    result = await Operator.
                                       RemoteStop(SessionId,
                                                  ReservationHandling,
                                                  ProviderId,
                                                  eMAId,

                                                  Timestamp,
                                                  CancellationToken,
                                                  EventTrackingId,
                                                  RequestTimeout);

                }

            }

            #endregion

            #region ...or try to check every EMP roaming provider...

            if (result        == null ||
                result.Result == RemoteStopResultType.InvalidSessionId)
            {

                foreach (var EMPRoamingService in _EMPRoamingProviders.
                                                      OrderBy(EMPRoamingServiceWithPriority => EMPRoamingServiceWithPriority.Key).
                                                      Select(EMPRoamingServiceWithPriority => EMPRoamingServiceWithPriority.Value))
                {

                    result = await EMPRoamingService.
                                       RemoteStop(SessionId,
                                                  ReservationHandling,
                                                  ProviderId,
                                                  eMAId,

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


            #region Send OnRemoteStopResponse event

            Runtime.Stop();

            try
            {

                OnRemoteStopResponse?.Invoke(DateTime.Now,
                                             Timestamp.Value,
                                             this,
                                             EventTrackingId,
                                             Id,
                                             SessionId,
                                             ReservationHandling,
                                             ProviderId,
                                             eMAId,
                                             RequestTimeout,
                                             result,
                                             Runtime.Elapsed);

            }
            catch (Exception e)
            {
                e.Log(nameof(RoamingNetwork) + "." + nameof(OnRemoteStopResponse));
            }

            #endregion

            return result;

        }

        #endregion

        #region RemoteStop(EVSEId,            SessionId, ReservationHandling, ProviderId = null, eMAId = null, ...)

        /// <summary>
        /// Stop the given charging session at the given EVSE.
        /// </summary>
        /// <param name="EVSEId">The unique identification of the EVSE to be stopped.</param>
        /// <param name="SessionId">The unique identification for this charging session.</param>
        /// <param name="ReservationHandling">Whether to remove the reservation after session end, or to keep it open for some more time.</param>
        /// <param name="ProviderId">The unique identification of the e-mobility service provider.</param>
        /// <param name="eMAId">The unique identification of the e-mobility account.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public async Task<RemoteStopEVSEResult>

            RemoteStop(EVSE_Id                EVSEId,
                       ChargingSession_Id     SessionId,
                       ReservationHandling    ReservationHandling,
                       eMobilityProvider_Id?  ProviderId          = null,
                       eMobilityAccount_Id?   eMAId               = null,

                       DateTime?              Timestamp           = null,
                       CancellationToken?     CancellationToken   = null,
                       EventTracking_Id       EventTrackingId     = null,
                       TimeSpan?              RequestTimeout      = null)

        {

            #region Initial checks

            if (!Timestamp.HasValue)
                Timestamp = DateTime.Now;

            if (!CancellationToken.HasValue)
                CancellationToken = new CancellationTokenSource().Token;

            if (EventTrackingId == null)
                EventTrackingId = EventTracking_Id.New;


            RemoteStopEVSEResult result = null;

            #endregion

            #region Send OnRemoteEVSEStopRequest event

            var Runtime = Stopwatch.StartNew();

            try
            {

                OnRemoteEVSEStopRequest?.Invoke(DateTime.Now,
                                                Timestamp.Value,
                                                this,
                                                EventTrackingId,
                                                Id,
                                                EVSEId,
                                                SessionId,
                                                ReservationHandling,
                                                ProviderId,
                                                eMAId,
                                                RequestTimeout);

            }
            catch (Exception e)
            {
                e.Log(nameof(RoamingNetwork) + "." + nameof(OnRemoteEVSEStopRequest));
            }

            #endregion


            #region Check charging station operator charging session lookup...

            ChargingSession _ChargingSession;

            if (_ChargingSessions.TryRemove(SessionId, out _ChargingSession))
            {

                result = await _ChargingSession?.ChargingStationOperator?.
                                   RemoteStop(EVSEId,
                                              SessionId,
                                              ReservationHandling,
                                              ProviderId,
                                              eMAId,

                                              Timestamp,
                                              CancellationToken,
                                              EventTrackingId,
                                              RequestTimeout);

            }

            #endregion

            #region ...or check EMP roaming provider charging session lookup...

            if (result        == null                                 ||
                result.Result == RemoteStopEVSEResultType.UnknownEVSE ||
                result.Result == RemoteStopEVSEResultType.InvalidSessionId)
            {

                result = await _ChargingSession?.EMPRoamingProvider?.
                                   RemoteStop(EVSEId,
                                              SessionId,
                                              ReservationHandling,
                                              ProviderId,
                                              eMAId,

                                              Timestamp,
                                              CancellationToken,
                                              EventTrackingId,
                                              RequestTimeout);

            }

            #endregion

            #region ...or try to get the charging station operator from the EVSE identification...

            if (result        == null                                 ||
                result.Result == RemoteStopEVSEResultType.UnknownEVSE ||
                result.Result == RemoteStopEVSEResultType.InvalidSessionId)
            {

                result = await GetChargingStationOperatorById(EVSEId.OperatorId).
                                   RemoteStop(EVSEId,
                                              SessionId,
                                              ReservationHandling,
                                              ProviderId,
                                              eMAId,

                                              Timestamp,
                                              CancellationToken,
                                              EventTrackingId,
                                              RequestTimeout);

            }

            #endregion

            #region ...or try to check every EMP roaming provider...

            if (result        == null                                 ||
                result.Result == RemoteStopEVSEResultType.UnknownEVSE ||
                result.Result == RemoteStopEVSEResultType.InvalidSessionId)
            {

                foreach (var EMPRoamingService in _EMPRoamingProviders.
                                                      OrderBy(EMPRoamingServiceWithPriority => EMPRoamingServiceWithPriority.Key).
                                                      Select(EMPRoamingServiceWithPriority => EMPRoamingServiceWithPriority.Value))
                {

                    result = await EMPRoamingService.
                                       RemoteStop(EVSEId,
                                                  SessionId,
                                                  ReservationHandling,
                                                  ProviderId,
                                                  eMAId,

                                                  Timestamp,
                                                  CancellationToken,
                                                  EventTrackingId,
                                                  RequestTimeout);

                }

            }

            #endregion

            #region ...or fail!

            if (result == null)
                result = RemoteStopEVSEResult.InvalidSessionId(SessionId);

            #endregion


            #region Send OnRemoteEVSEStopResponse event

            Runtime.Stop();

            try
            {

                OnRemoteEVSEStopResponse?.Invoke(DateTime.Now,
                                                 Timestamp.Value,
                                                 this,
                                                 EventTrackingId,
                                                 Id,
                                                 EVSEId,
                                                 SessionId,
                                                 ReservationHandling,
                                                 ProviderId,
                                                 eMAId,
                                                 RequestTimeout,
                                                 result,
                                                 Runtime.Elapsed);

            }
            catch (Exception e)
            {
                e.Log(nameof(RoamingNetwork) + "." + nameof(OnRemoteEVSEStopResponse));
            }

            #endregion

            return result;

        }

        #endregion

        #region RemoteStop(ChargingStationId, SessionId, ReservationHandling, ProviderId = null, eMAId = null, ...)

        /// <summary>
        /// Stop the given charging session at the given charging station.
        /// </summary>
        /// <param name="ChargingStationId">The unique identification of the charging station to be stopped.</param>
        /// <param name="SessionId">The unique identification for this charging session.</param>
        /// <param name="ReservationHandling">Whether to remove the reservation after session end, or to keep it open for some more time.</param>
        /// <param name="ProviderId">The unique identification of the e-mobility service provider.</param>
        /// <param name="eMAId">The unique identification of the e-mobility account.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public async Task<RemoteStopChargingStationResult>

            RemoteStop(ChargingStation_Id     ChargingStationId,
                       ChargingSession_Id     SessionId,
                       ReservationHandling    ReservationHandling,
                       eMobilityProvider_Id?  ProviderId          = null,
                       eMobilityAccount_Id?   eMAId               = null,

                       DateTime?              Timestamp           = null,
                       CancellationToken?     CancellationToken   = null,
                       EventTracking_Id       EventTrackingId     = null,
                       TimeSpan?              RequestTimeout      = null)

        {

            #region Initial checks

            if (!Timestamp.HasValue)
                Timestamp = DateTime.Now;

            if (!CancellationToken.HasValue)
                CancellationToken = new CancellationTokenSource().Token;

            if (EventTrackingId == null)
                EventTrackingId = EventTracking_Id.New;


            RemoteStopChargingStationResult result = null;

            #endregion

            #region Send OnRemoteChargingStationStopRequest event

            var Runtime = Stopwatch.StartNew();

            try
            {

                OnRemoteChargingStationStopRequest?.Invoke(DateTime.Now,
                                                           Timestamp.Value,
                                                           this,
                                                           EventTrackingId,
                                                           Id,
                                                           ChargingStationId,
                                                           SessionId,
                                                           ReservationHandling,
                                                           ProviderId,
                                                           eMAId,
                                                           RequestTimeout);

            }
            catch (Exception e)
            {
                e.Log(nameof(RoamingNetwork) + "." + nameof(OnRemoteChargingStationStopRequest));
            }

            #endregion


            #region Check charging station operator charging session lookup...

            ChargingSession _ChargingSession;

            if (_ChargingSessions.TryGetValue(SessionId, out _ChargingSession))
            {

                result = await _ChargingSession?.ChargingStationOperator?.
                                   RemoteStop(ChargingStationId,
                                              SessionId,
                                              ReservationHandling,
                                              ProviderId,
                                              eMAId,

                                              Timestamp,
                                              CancellationToken,
                                              EventTrackingId,
                                              RequestTimeout);

            }

            #endregion

            #region ...or check EMP roaming provider charging session lookup...

            if (result        == null                                                       ||
                result.Result == RemoteStopChargingStationResultType.UnknownChargingStation ||
                result.Result == RemoteStopChargingStationResultType.InvalidSessionId)
            {

                result = await _ChargingSession?.EMPRoamingProvider?.
                                   RemoteStop(ChargingStationId,
                                              SessionId,
                                              ReservationHandling,
                                              ProviderId,
                                              eMAId,

                                              Timestamp,
                                              CancellationToken,
                                              EventTrackingId,
                                              RequestTimeout);

            }

            #endregion

            #region ...or try to get the charging station operator from the charging station identification...

            if (result        == null                                                       ||
                result.Result == RemoteStopChargingStationResultType.UnknownChargingStation ||
                result.Result == RemoteStopChargingStationResultType.InvalidSessionId)
            {

                result = await GetChargingStationOperatorById(ChargingStationId.OperatorId).
                                   RemoteStop(ChargingStationId,
                                              SessionId,
                                              ReservationHandling,
                                              ProviderId,
                                              eMAId,

                                              Timestamp,
                                              CancellationToken,
                                              EventTrackingId,
                                              RequestTimeout);

            }

            #endregion

            #region ...or try to check every EMP roaming provider...

            if (result        == null                                                       ||
                result.Result == RemoteStopChargingStationResultType.UnknownChargingStation ||
                result.Result == RemoteStopChargingStationResultType.InvalidSessionId)
            {

                foreach (var EMPRoamingService in _EMPRoamingProviders.
                                                      OrderBy(EMPRoamingServiceWithPriority => EMPRoamingServiceWithPriority.Key).
                                                      Select (EMPRoamingServiceWithPriority => EMPRoamingServiceWithPriority.Value))
                {

                    result = await EMPRoamingService.
                                       RemoteStop(ChargingStationId,
                                                  SessionId,
                                                  ReservationHandling,
                                                  ProviderId,
                                                  eMAId,

                                                  Timestamp,
                                                  CancellationToken,
                                                  EventTrackingId,
                                                  RequestTimeout);

                }

            }

            #endregion

            #region ...or fail!

            if (result == null)
                result = RemoteStopChargingStationResult.InvalidSessionId(SessionId);

            #endregion


            #region Send OnRemoteChargingStationStopResponse event

            Runtime.Stop();

            try
            {

                OnRemoteChargingStationStopResponse?.Invoke(DateTime.Now,
                                                            Timestamp.Value,
                                                            this,
                                                            EventTrackingId,
                                                            Id,
                                                            ChargingStationId,
                                                            SessionId,
                                                            ReservationHandling,
                                                            ProviderId,
                                                            eMAId,
                                                            RequestTimeout,
                                                            result,
                                                            Runtime.Elapsed);

            }
            catch (Exception e)
            {
                e.Log(nameof(RoamingNetwork) + "." + nameof(OnRemoteChargingStationStopResponse));
            }

            #endregion

            return result;

        }

        #endregion

        #endregion

        #region AuthorizeStart/-Stop

        #region AuthorizeStart(AuthToken,                    ChargingProduct = null, SessionId = null, OperatorId = null, ...)

        /// <summary>
        /// Create an authorize start request.
        /// </summary>
        /// <param name="AuthToken">A (RFID) user identification.</param>
        /// <param name="ChargingProduct">An optional charging product.</param>
        /// <param name="SessionId">An optional session identification.</param>
        /// <param name="OperatorId">An optional charging station operator identification.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public async Task<AuthStartResult>

            AuthorizeStart(Auth_Token                   AuthToken,
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

            if (AuthToken  == null)
                throw new ArgumentNullException(nameof(AuthToken),   "The given authentication token must not be null!");

            if (!Timestamp.HasValue)
                Timestamp = DateTime.Now;

            if (!CancellationToken.HasValue)
                CancellationToken = new CancellationTokenSource().Token;

            if (EventTrackingId == null)
                EventTrackingId = EventTracking_Id.New;

            #endregion

            #region Send OnAuthorizeStartRequest event

            var StartTime = DateTime.Now;

            try
            {

                OnAuthorizeStartRequest?.Invoke(StartTime,
                                                Timestamp.Value,
                                                this,
                                                EventTrackingId,
                                                Id,
                                                OperatorId,
                                                AuthToken,
                                                ChargingProduct,
                                                SessionId,
                                                RequestTimeout);

            }
            catch (Exception e)
            {
                e.Log(nameof(RoamingNetwork) + "." + nameof(OnAuthorizeStartRequest));
            }

            #endregion


            AuthStartResult result = null;

            foreach (var iRemoteAuthorizeStartStop in _IRemoteAuthorizeStartStop.
                                                          OrderBy(kvp => kvp.Key).
                                                          Select (kvp => kvp.Value))
            {

                result = await iRemoteAuthorizeStartStop.
                                   AuthorizeStart(AuthToken,
                                                  ChargingProduct,
                                                  SessionId,
                                                  OperatorId,

                                                  Timestamp,
                                                  CancellationToken,
                                                  EventTrackingId,
                                                  RequestTimeout);


                #region Authorized

                if (result.Result == AuthStartResultType.Authorized)
                {

                    // Store the upstream session id in order to contact the right authenticator at later requests!
                    // Will be deleted when the CDRecord was sent!
                    //_ChargingSessions.TryAdd(result.SessionId,
                    //                         new ChargingSession(result.SessionId) {
                    //                             OperatorRoamingService  = OperatorRoamingService,
                    //                             csoId          = OperatorId,
                    //                             AuthToken               = AuthToken,
                    //                             ChargingProductId       = ChargingProductId
                    //                         });

                    break;

                }

                #endregion

                #region Blocked

                else if (result.Result == AuthStartResultType.Blocked)
                    break;

                #endregion

            }

            #region ...or fail!

            if (result == null)
                result =  AuthStartResult.Error(Id,
                                                SessionId,
                                                "No authorization service returned a positiv result!");

            #endregion


            var Endtime = DateTime.Now;
            var Runtime = Endtime - StartTime;

            #region Send OnAuthorizeStartResponse event

            try
            {

                OnAuthorizeStartResponse?.Invoke(Endtime,
                                                 Timestamp.Value,
                                                 this,
                                                 EventTrackingId,
                                                 Id,
                                                 OperatorId,
                                                 AuthToken,
                                                 ChargingProduct,
                                                 SessionId,
                                                 RequestTimeout,
                                                 result,
                                                 Runtime);

            }
            catch (Exception e)
            {
                e.Log(nameof(RoamingNetwork) + "." + nameof(OnAuthorizeStartResponse));
            }

            #endregion

            return result;

        }

        #endregion

        #region AuthorizeStart(AuthToken, EVSEId,            ChargingProduct = null, SessionId = null, OperatorId = null, ...)

        /// <summary>
        /// Create an authorize start request at the given EVSE.
        /// </summary>
        /// <param name="AuthToken">A (RFID) user identification.</param>
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

            AuthorizeStart(Auth_Token                   AuthToken,
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

            if (AuthToken  == null)
                throw new ArgumentNullException(nameof(AuthToken),   "The given authentication token must not be null!");


            if (!Timestamp.HasValue)
                Timestamp = DateTime.Now;

            if (!CancellationToken.HasValue)
                CancellationToken = new CancellationTokenSource().Token;

            if (EventTrackingId == null)
                EventTrackingId = EventTracking_Id.New;


            AuthStartEVSEResult result = null;

            #endregion

            #region Send OnAuthorizeEVSEStartRequest event

            var StartTime = DateTime.Now;

            try
            {

                OnAuthorizeEVSEStartRequest?.Invoke(StartTime,
                                                    Timestamp.Value,
                                                    this,
                                                    EventTrackingId,
                                                    Id,
                                                    OperatorId,
                                                    AuthToken,
                                                    EVSEId,
                                                    ChargingProduct,
                                                    SessionId,
                                                    RequestTimeout);

            }
            catch (Exception e)
            {
                e.Log(nameof(RoamingNetwork) + "." + nameof(OnAuthorizeEVSEStartRequest));
            }

            #endregion


            var results = await Task.WhenAll(_IRemoteAuthorizeStartStop.
                                                 OrderBy(kvp => kvp.Key).
                                                 Select (kvp => kvp.Value.AuthorizeStart(AuthToken,
                                                                                         EVSEId,
                                                                                         ChargingProduct,
                                                                                         SessionId,
                                                                                         OperatorId,

                                                                                         Timestamp,
                                                                                         CancellationToken,
                                                                                         EventTrackingId,
                                                                                         RequestTimeout)));


            #region The fastest Authorized|Blocked will win!

            result = results.
                         Where  (res => res        != null &&
                                        res.Result == AuthStartEVSEResultType.Authorized).
                         OrderBy(res => res.Runtime).
                         FirstOrDefault();

            if (result == null)
                result = results.
                             Where  (res => res        != null &&
                                            res.Result == AuthStartEVSEResultType.Blocked).
                             OrderBy(res => res.Runtime).
                             FirstOrDefault();

            #endregion

            #region If authorized...

            if (result?.Result == AuthStartEVSEResultType.Authorized)
            {

                // Store the upstream session id in order to contact the right authenticator at later requests!
                // Will be deleted when the charge detail record was sent!

                var NewChargingSession = new ChargingSession(result.SessionId.Value) {
                                             AuthService      = _IRemoteAuthorizeStartStop.Values.FirstOrDefault(_ => _.AuthId == result.AuthorizatorId),
                                             AuthorizatorId   = result.AuthorizatorId,
                                             OperatorId       = OperatorId,
                                             EVSEId           = EVSEId,
                                             AuthTokenStart   = AuthToken,
                                             ChargingProduct  = ChargingProduct
                                         };

                if (_ChargingSessions.TryAdd(NewChargingSession.Id, NewChargingSession))

                if (result.SessionId.HasValue)
                    RegisterExternalChargingSession(DateTime.Now,
                                                    this,
                                                    NewChargingSession);

            }

            #endregion


            var Endtime = DateTime.Now;
            var Runtime = Endtime - StartTime;

            #region ...or fail!

            if (result == null)
                result =  AuthStartEVSEResult.Error(
                              Id,
                              SessionId,
                              "No attached authorization service returned a positiv result!",
                              Runtime
                          );

            #endregion


            #region Send OnAuthorizeEVSEStartResponse event

            try
            {

                OnAuthorizeEVSEStartResponse?.Invoke(Endtime,
                                                     Timestamp.Value,
                                                     this,
                                                     EventTrackingId,
                                                     Id,
                                                     OperatorId,
                                                     AuthToken,
                                                     EVSEId,
                                                     ChargingProduct,
                                                     SessionId,
                                                     RequestTimeout,
                                                     result,
                                                     Runtime);

            }
            catch (Exception e)
            {
                e.Log(nameof(RoamingNetwork) + "." + nameof(OnAuthorizeEVSEStartResponse));
            }

            #endregion

            return result;

        }

        #endregion

        #region AuthorizeStart(AuthToken, ChargingStationId, ChargingProduct = null, SessionId = null, OperatorId = null, ...)

        /// <summary>
        /// Create an authorize start request at the given charging station.
        /// </summary>
        /// <param name="AuthToken">A (RFID) user identification.</param>
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

            AuthorizeStart(Auth_Token                   AuthToken,
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

            if (AuthToken == null)
                throw new ArgumentNullException(nameof(AuthToken), "The given authentication token must not be null!");


            if (!Timestamp.HasValue)
                Timestamp = DateTime.Now;

            if (!CancellationToken.HasValue)
                CancellationToken = new CancellationTokenSource().Token;

            if (EventTrackingId == null)
                EventTrackingId = EventTracking_Id.New;


            AuthStartChargingStationResult result = null;

            #endregion

            #region Send OnAuthorizeChargingStationStartRequest event

            var StartTime = DateTime.Now;

            try
            {

                OnAuthorizeChargingStationStartRequest?.Invoke(DateTime.Now,
                                                               Timestamp.Value,
                                                               this,
                                                               EventTrackingId,
                                                               Id,
                                                               OperatorId,
                                                               AuthToken,
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


            foreach (var iRemoteAuthorizeStartStop in _IRemoteAuthorizeStartStop.
                                                          OrderBy(kvp => kvp.Key).
                                                          Select(kvp => kvp.Value))
            {

                result = await iRemoteAuthorizeStartStop.AuthorizeStart(AuthToken,
                                                                        ChargingStationId,
                                                                        ChargingProduct,
                                                                        SessionId,
                                                                        OperatorId,

                                                                        Timestamp,
                                                                        CancellationToken,
                                                                        EventTrackingId,
                                                                        RequestTimeout);


                #region Authorized

                if (result.Result == AuthStartChargingStationResultType.Authorized)
                {

                    // Store the upstream session id in order to contact the right authenticator at later requests!
                    // Will be deleted when the CDRecord was sent!
                    //_ChargingSessions.TryAdd(result.SessionId,
                    //                         new ChargingSession(result.SessionId) {
                    //                             OperatorRoamingService  = OperatorRoamingService,
                    //                             csoId          = OperatorId,
                    //                             ChargingStationId       = ChargingStationId,
                    //                             AuthToken               = AuthToken,
                    //                             ChargingProductId       = ChargingProductId
                    //                         });

                    break;

                }

                #endregion

                #region Blocked

                else if (result.Result == AuthStartChargingStationResultType.Blocked)
                    break;

                #endregion

            }

            #region ...or fail!

            if (result == null)
                result = AuthStartChargingStationResult.Error(
                             Id,
                             SessionId,
                             "No authorization service returned a positiv result!"
                         );

            #endregion


            var Endtime = DateTime.Now;
            var Runtime = Endtime - StartTime;

            #region Send OnAuthorizeChargingStationStarted event

            try
            {

                OnAuthorizeChargingStationStartResponse?.Invoke(DateTime.Now,
                                                                Timestamp.Value,
                                                                this,
                                                                EventTrackingId,
                                                                Id,
                                                                OperatorId,
                                                                AuthToken,
                                                                ChargingStationId,
                                                                ChargingProduct,
                                                                SessionId,
                                                                RequestTimeout,
                                                                result,
                                                                Runtime);

            }
            catch (Exception e)
            {
                e.Log(nameof(RoamingNetwork) + "." + nameof(OnAuthorizeChargingStationStartResponse));
            }

            #endregion

            return result;

        }

        #endregion

        #region AuthorizeStart(AuthToken, ChargingPoolId,    ChargingProduct = null, SessionId = null, OperatorId = null, ...)

        /// <summary>
        /// Create an authorize start request at the given charging station.
        /// </summary>
        /// <param name="AuthToken">A (RFID) user identification.</param>
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

            AuthorizeStart(Auth_Token                   AuthToken,
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

            if (AuthToken == null)
                throw new ArgumentNullException(nameof(AuthToken), "The given authentication token must not be null!");


            if (!Timestamp.HasValue)
                Timestamp = DateTime.Now;

            if (!CancellationToken.HasValue)
                CancellationToken = new CancellationTokenSource().Token;

            if (EventTrackingId == null)
                EventTrackingId = EventTracking_Id.New;


            AuthStartChargingPoolResult result = null;

            #endregion

            #region Send OnAuthorizeChargingPoolStartRequest event

            var StartTime = DateTime.Now;

            try
            {

                OnAuthorizeChargingPoolStartRequest?.Invoke(DateTime.Now,
                                                            Timestamp.Value,
                                                            this,
                                                            EventTrackingId,
                                                            Id,
                                                            OperatorId,
                                                            AuthToken,
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


            foreach (var iRemoteAuthorizeStartStop in _IRemoteAuthorizeStartStop.
                                                          OrderBy(kvp => kvp.Key).
                                                          Select(kvp => kvp.Value))
            {

                result = await iRemoteAuthorizeStartStop.AuthorizeStart(AuthToken,
                                                                        ChargingPoolId,
                                                                        ChargingProduct,
                                                                        SessionId,
                                                                        OperatorId,

                                                                        Timestamp,
                                                                        CancellationToken,
                                                                        EventTrackingId,
                                                                        RequestTimeout);


                #region Authorized

                if (result.Result == AuthStartChargingPoolResultType.Authorized)
                {

                    // Store the upstream session id in order to contact the right authenticator at later requests!
                    // Will be deleted when the CDRecord was sent!
                    //_ChargingSessions.TryAdd(result.SessionId,
                    //                         new ChargingSession(result.SessionId) {
                    //                             OperatorRoamingService  = OperatorRoamingService,
                    //                             csoId          = OperatorId,
                    //                             ChargingStationId       = ChargingStationId,
                    //                             AuthToken               = AuthToken,
                    //                             ChargingProductId       = ChargingProductId
                    //                         });

                    break;

                }

                #endregion

                #region Blocked

                else if (result.Result == AuthStartChargingPoolResultType.Blocked)
                    break;

                #endregion

            }

            #region ...or fail!

            if (result == null)
                result = AuthStartChargingPoolResult.Error(
                             Id,
                             SessionId,
                             "No authorization service returned a positiv result!"
                         );

            #endregion


            var Endtime = DateTime.Now;
            var Runtime = Endtime - StartTime;

            #region Send OnAuthorizeChargingPoolStartResponse event

            try
            {

                OnAuthorizeChargingPoolStartResponse?.Invoke(DateTime.Now,
                                                             Timestamp.Value,
                                                             this,
                                                             EventTrackingId,
                                                             Id,
                                                             OperatorId,
                                                             AuthToken,
                                                             ChargingPoolId,
                                                             ChargingProduct,
                                                             SessionId,
                                                             RequestTimeout,
                                                             result,
                                                             Runtime);

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



        #region AuthorizeStop(SessionId, AuthToken,                    OperatorId = null, ...)

        /// <summary>
        /// Create an authorize stop request.
        /// </summary>
        /// <param name="SessionId">The session identification from the AuthorizeStart request.</param>
        /// <param name="AuthToken">A (RFID) user identification.</param>
        /// <param name="OperatorId">An optional charging station operator identification.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public async Task<AuthStopResult>

            AuthorizeStop(ChargingSession_Id           SessionId,
                          Auth_Token                   AuthToken,
                          ChargingStationOperator_Id?  OperatorId          = null,

                          DateTime?                    Timestamp           = null,
                          CancellationToken?           CancellationToken   = null,
                          EventTracking_Id             EventTrackingId     = null,
                          TimeSpan?                    RequestTimeout      = null)

        {

            #region Initial checks

            if (AuthToken == null)
                throw new ArgumentNullException(nameof(AuthToken), "The given parameter must not be null!");


            if (!Timestamp.HasValue)
                Timestamp = DateTime.Now;

            if (!CancellationToken.HasValue)
                CancellationToken = new CancellationTokenSource().Token;

            if (EventTrackingId == null)
                EventTrackingId = EventTracking_Id.New;


            AuthStopResult result = null;

            #endregion

            #region Send OnAuthorizeStopRequest event

            var StartTime = DateTime.Now;

            try
            {

                OnAuthorizeStopRequest?.Invoke(StartTime,
                                               Timestamp.Value,
                                               this,
                                               EventTrackingId,
                                               Id,
                                               OperatorId,
                                               SessionId,
                                               AuthToken,
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
                foreach (var iRemoteAuthorizeStartStop in _IRemoteAuthorizeStartStop.
                                                          OrderBy(kvp => kvp.Key).
                                                          Select (kvp => kvp.Value))
                {

                    result = await iRemoteAuthorizeStartStop.AuthorizeStop(SessionId,
                                                                           AuthToken,
                                                                           OperatorId,

                                                                           Timestamp,
                                                                           CancellationToken,
                                                                           EventTrackingId,
                                                                           RequestTimeout);

                    if (result.Result == AuthStopResultType.Authorized)
                        break;

                }

            #endregion

            #region ...or fail!

            if (result == null)
                result = AuthStopResult.Error(
                             Id,
                             SessionId,
                             "No authorization service returned a positiv result!"
                         );

            #endregion


            var Endtime = DateTime.Now;
            var Runtime = Endtime - StartTime;

            #region Send OnAuthorizeStopResponse event

            try
            {

                OnAuthorizeStopResponse?.Invoke(Endtime,
                                                Timestamp.Value,
                                                this,
                                                EventTrackingId,
                                                Id,
                                                OperatorId,
                                                SessionId,
                                                AuthToken,
                                                RequestTimeout,
                                                result,
                                                Runtime);

            }
            catch (Exception e)
            {
                e.Log(nameof(RoamingNetwork) + "." + nameof(OnAuthorizeStopResponse));
            }

            #endregion

            return result;

        }

        #endregion

        #region AuthorizeStop(SessionId, AuthToken, EVSEId,            OperatorId = null, ...)

        /// <summary>
        /// Create an authorize stop request at the given EVSE.
        /// </summary>
        /// <param name="SessionId">The session identification from the AuthorizeStart request.</param>
        /// <param name="AuthToken">A (RFID) user identification.</param>
        /// <param name="EVSEId">The unique identification of an EVSE.</param>
        /// <param name="OperatorId">An optional charging station operator identification.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public async Task<AuthStopEVSEResult>

            AuthorizeStop(ChargingSession_Id           SessionId,
                          Auth_Token                   AuthToken,
                          EVSE_Id                      EVSEId,
                          ChargingStationOperator_Id?  OperatorId          = null,

                          DateTime?                    Timestamp           = null,
                          CancellationToken?           CancellationToken   = null,
                          EventTracking_Id             EventTrackingId     = null,
                          TimeSpan?                    RequestTimeout      = null)

        {

            #region Initial checks

            if (AuthToken == null)
                throw new ArgumentNullException(nameof(AuthToken), "The given parameter must not be null!");


            if (!Timestamp.HasValue)
                Timestamp = DateTime.Now;

            if (!CancellationToken.HasValue)
                CancellationToken = new CancellationTokenSource().Token;

            if (EventTrackingId == null)
                EventTrackingId = EventTracking_Id.New;


            AuthStopEVSEResult result = null;

            #endregion

            #region Send OnAuthorizeEVSEStopRequest event

            var StartTime = DateTime.Now;

            try
            {

                OnAuthorizeEVSEStopRequest?.Invoke(DateTime.Now,
                                                   Timestamp.Value,
                                                   this,
                                                   EventTrackingId,
                                                   Id,
                                                   OperatorId,
                                                   EVSEId,
                                                   SessionId,
                                                   AuthToken,
                                                   RequestTimeout);

            }
            catch (Exception e)
            {
                e.Log(nameof(RoamingNetwork) + "." + nameof(OnAuthorizeEVSEStopRequest));
            }

            #endregion


            #region An authenticator was found for the upstream SessionId!

            ChargingSession _ChargingSession = null;

            if (_ChargingSessions.TryGetValue(SessionId, out _ChargingSession) &&
                _ChargingSession. AuthService != null)
            {

                result = await _ChargingSession.AuthService.AuthorizeStop(SessionId,
                                                                          AuthToken,
                                                                          EVSEId,
                                                                          OperatorId,

                                                                          Timestamp,
                                                                          CancellationToken,
                                                                          EventTrackingId,
                                                                          RequestTimeout);

            }

            #endregion

            else
            {

                var results = await Task.WhenAll(_IRemoteAuthorizeStartStop.
                                                     OrderBy(kvp => kvp.Key).
                                                     Select (kvp => kvp.Value.AuthorizeStop(SessionId,
                                                                                            AuthToken,
                                                                                            EVSEId,
                                                                                            OperatorId,

                                                                                            Timestamp,
                                                                                            CancellationToken,
                                                                                            EventTrackingId,
                                                                                            RequestTimeout)));


                #region The fastest Authorized|Blocked will win!

                result = results.
                             Where  (res => res        != null &&
                                            res.Result == AuthStopEVSEResultType.Authorized).
                             OrderBy(res => res.Runtime).
                             FirstOrDefault();

                if (result == null)
                    result = results.
                                 Where  (res => res        != null &&
                                                res.Result == AuthStopEVSEResultType.Blocked).
                                 OrderBy(res => res.Runtime).
                                 FirstOrDefault();

                #endregion

            }


            var Endtime = DateTime.Now;
            var Runtime = Endtime - StartTime;

            #region ...or fail!

            if (result == null)
                result = AuthStopEVSEResult.Error(
                              Id,
                              SessionId,
                              "No attached authorization service returned a positiv result!",
                              Runtime
                          );

            #endregion


            #region Send OnAuthorizeEVSEStopResponse event

            try
            {

                OnAuthorizeEVSEStopResponse?.Invoke(Endtime,
                                                    Timestamp.Value,
                                                    this,
                                                    EventTrackingId,
                                                    Id,
                                                    OperatorId,
                                                    EVSEId,
                                                    SessionId,
                                                    AuthToken,
                                                    RequestTimeout,
                                                    result,
                                                    Runtime);

            }
            catch (Exception e)
            {
                e.Log(nameof(RoamingNetwork) + "." + nameof(OnAuthorizeEVSEStopResponse));
            }

            #endregion

            return result;

        }

        #endregion

        #region AuthorizeStop(SessionId, AuthToken, ChargingStationId, OperatorId = null, ...)

        /// <summary>
        /// Create an authorize stop request at the given charging station.
        /// </summary>
        /// <param name="SessionId">The session identification from the AuthorizeStart request.</param>
        /// <param name="AuthToken">A (RFID) user identification.</param>
        /// <param name="ChargingStationId">The unique identification of a charging station.</param>
        /// <param name="OperatorId">An optional charging station operator identification.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public async Task<AuthStopChargingStationResult>

            AuthorizeStop(ChargingSession_Id           SessionId,
                          Auth_Token                   AuthToken,
                          ChargingStation_Id           ChargingStationId,
                          ChargingStationOperator_Id?  OperatorId          = null,

                          DateTime?                    Timestamp           = null,
                          CancellationToken?           CancellationToken   = null,
                          EventTracking_Id             EventTrackingId     = null,
                          TimeSpan?                    RequestTimeout      = null)

        {

            #region Initial checks

            if (AuthToken         == null)
                throw new ArgumentNullException(nameof(AuthToken),          "The given parameter must not be null!");

            if (ChargingStationId == null)
                throw new ArgumentNullException(nameof(ChargingStationId),  "The given parameter must not be null!");


            if (!Timestamp.HasValue)
                Timestamp = DateTime.Now;

            if (!CancellationToken.HasValue)
                CancellationToken = new CancellationTokenSource().Token;

            if (EventTrackingId == null)
                EventTrackingId = EventTracking_Id.New;


            AuthStopChargingStationResult result = null;

            #endregion

            #region Send OnAuthorizeChargingStationStopRequest event

            var StartTime = DateTime.Now;

            try
            {

                OnAuthorizeChargingStationStopRequest?.Invoke(StartTime,
                                                              Timestamp.Value,
                                                              this,
                                                              EventTrackingId,
                                                              Id,
                                                              OperatorId,
                                                              ChargingStationId,
                                                              SessionId,
                                                              AuthToken,
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
                foreach (var iRemoteAuthorizeStartStop in _IRemoteAuthorizeStartStop.
                                                          OrderBy(kvp => kvp.Key).
                                                          Select (kvp => kvp.Value))
                {

                    result = await iRemoteAuthorizeStartStop.AuthorizeStop(SessionId,
                                                                           AuthToken,
                                                                           ChargingStationId,
                                                                           OperatorId,

                                                                           Timestamp,
                                                                           CancellationToken,
                                                                           EventTrackingId,
                                                                           RequestTimeout);

                    if (result.Result == AuthStopChargingStationResultType.Authorized)
                        break;

                }

            #endregion

            #region ...or fail!

            if (result == null)
                result = AuthStopChargingStationResult.Error(
                             Id,
                             SessionId,
                             "No authorization service returned a positiv result!"
                         );

            #endregion


            var Endtime = DateTime.Now;
            var Runtime = Endtime - StartTime;

            #region Send OnAuthorizeChargingStationStopResponse event

            try
            {

                OnAuthorizeChargingStationStopResponse?.Invoke(Endtime,
                                                               Timestamp.Value,
                                                               this,
                                                               EventTrackingId,
                                                               Id,
                                                               OperatorId,
                                                               ChargingStationId,
                                                               SessionId,
                                                               AuthToken,
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

        #endregion

        #region Charging Sessions / Charge Detail Records...

        #region ChargingSessions

        private readonly ConcurrentDictionary<ChargingSession_Id, ChargingSession> _ChargingSessions;

        /// <summary>
        /// Return all current CSO charging sessions.
        /// </summary>
        public IEnumerable<ChargingSession> ChargingSessions

            => _ChargingSessions.Values;
         //   => _ChargingStationOperators.
         //           SelectMany(cso => cso.ChargingSessions);

        #endregion

        /// <summary>
        /// An event fired whenever a new charging session was created.
        /// </summary>
        public event OnNewChargingSessionDelegate OnNewChargingSession;

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

            #region Initial checks

            if (ChargingSession == null)
                throw new ArgumentNullException(nameof(ChargingSession), "The given charging session must not be null!");

            #endregion


            //if (!_ChargingSessions_RFID.ContainsKey(ChargingSession.Id))
            //{

                DebugX.LogT("Registered external charging session '" + ChargingSession.Id + "'!");

                //_ChargingSessions.TryAdd(ChargingSession.Id, ChargingSession);

                if (ChargingSession.EVSEId.HasValue)
                {

                    var _EVSE = GetEVSEbyId(ChargingSession.EVSEId.Value);

                    if (_EVSE != null)
                    {

                        ChargingSession.EVSE = _EVSE;

                        // Will NOT set the EVSE status!
                        _EVSE.ChargingSession = ChargingSession;

                    }

                }

                // else charging station

                // else charging pool

                //SendNewChargingSession(Timestamp, Sender, ChargingSession);

            //}

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

            #region Initial checks

            if (ChargingSession == null)
                throw new ArgumentNullException(nameof(ChargingSession), "The given charging session must not be null!");

            #endregion

            ChargingStationOperator _cso = null;

            //if (_ChargingSessions_RFID.TryRemove(ChargingSession.Id, out _cso))
            //{

                DebugX.LogT("Removing external charging session '" + ChargingSession.Id + "'!");

            //}

            if (ChargingSession.EVSE != null)
            {

                if (ChargingSession.EVSE.ChargingSession != null &&
                    ChargingSession.EVSE.ChargingSession == ChargingSession)
                {

                    ChargingSession.EVSE.ChargingSession = null;

                }

            }

            else if (ChargingSession.EVSEId.HasValue)
            {

                var _EVSE = GetEVSEbyId(ChargingSession.EVSEId.Value);

                if (_EVSE                 != null &&
                    _EVSE.ChargingSession != null &&
                    _EVSE.ChargingSession == ChargingSession)
                {

                    _EVSE.ChargingSession = null;

                }

            }

        }

        #endregion

        #region (internal) SendNewChargingSession(Timestamp, Sender, Session)

        internal void SendNewChargingSession(DateTime         Timestamp,
                                             Object           Sender,
                                             ChargingSession  Session)
        {

            if (Session != null)
            {

                if (Session.RoamingNetwork == null)
                    Session.RoamingNetwork = this;

            }

            OnNewChargingSession?.Invoke(Timestamp, Sender, Session);

        }

        #endregion


        #region ChargeDetailRecords

        private readonly ConcurrentDictionary<ChargingSession_Id, ChargeDetailRecord> _ChargeDetailRecords;

        /// <summary>
        /// Return all current charge detail records.
        /// </summary>
        public IEnumerable<ChargeDetailRecord> ChargeDetailRecords

            => _ChargeDetailRecords.Select(kvp => kvp.Value);

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

        ///// <summary>
        ///// An event fired whenever a new charge detail record was created.
        ///// </summary>
        //public event OnNewChargeDetailRecordDelegate OnNewChargeDetailRecord;


        #region OnFilterCDRRecords

        public delegate String OnFilterCDRRecordsDelegate(IId AuthorizatorId, ChargeDetailRecord ChargeDetailRecord);

        /// <summary>
        /// An event fired whenever a CDR needs to be filtered.
        /// </summary>
        public event OnFilterCDRRecordsDelegate OnFilterCDRRecords;

        #endregion

        #region SendChargeDetailRecords(...ChargeDetailRecords, ...)

        /// <summary>
        /// Send a charge detail record.
        /// </summary>
        /// <param name="ChargeDetailRecords">An enumeration of charge detail records.</param>
        /// 
        /// <param name="Timestamp">The timestamp of the request.</param>
        /// <param name="CancellationToken">A token to cancel this request.</param>
        /// <param name="EventTrackingId">An unique event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public async Task<SendCDRsResult>

            SendChargeDetailRecords(IEnumerable<ChargeDetailRecord>  ChargeDetailRecords,

                                    DateTime?                        Timestamp           = null,
                                    CancellationToken?               CancellationToken   = null,
                                    EventTracking_Id                 EventTrackingId     = null,
                                    TimeSpan?                        RequestTimeout      = null)

        {

            #region Initial checks

            if (ChargeDetailRecords == null)
                throw new ArgumentNullException(nameof(ChargeDetailRecords), "The given enumeration of charge detail records must not be null!");


            if (!Timestamp.HasValue)
                Timestamp = DateTime.Now;

            if (!CancellationToken.HasValue)
                CancellationToken = new CancellationTokenSource().Token;

            if (EventTrackingId == null)
                EventTrackingId = EventTracking_Id.New;

            #endregion

            //ToDo: Merge given cdr information with local information!

            foreach (var ChargeDetailRecord in ChargeDetailRecords)
                _ChargeDetailRecords.TryAdd(ChargeDetailRecord.SessionId, ChargeDetailRecord);

            #region Send OnSendCDRsRequest event

            var Runtime = Stopwatch.StartNew();

            try
            {

                OnSendCDRsRequest?.Invoke(DateTime.Now,
                                          Timestamp.Value,
                                          this,
                                          EventTrackingId,
                                          Id,
                                          ChargeDetailRecords,
                                          RequestTimeout);

            }
            catch (Exception e)
            {
                e.Log(nameof(RoamingNetwork) + "." + nameof(OnSendCDRsRequest));
            }

            #endregion


            #region Delete cached session information

            foreach (var ChargeDetailRecord in ChargeDetailRecords)
            {
                if (ChargeDetailRecord.EVSEId.HasValue)
                {

                    EVSE _EVSE = null;

                    if (TryGetEVSEbyId(ChargeDetailRecord.EVSEId.Value, out _EVSE))
                    {

                        if (_EVSE.ChargingSession != null &&
                            _EVSE.ChargingSession.Id == ChargeDetailRecord.SessionId)
                        {

                            //_EVSE.Status = EVSEStatusType.Available;
                            _EVSE.ChargingSession = null;
                            _EVSE.Reservation = null;

                        }

                    }

                }
            }

            #endregion

            #region Some charge detail records should perhaps be filtered...

            var FilteredCDRs  = new Dictionary<ChargingSession_Id, String>();
            var CDRsToSend    = new HashSet<ChargeDetailRecord>(ChargeDetailRecords);

            var OnFilterCDRRecordsLocal = OnFilterCDRRecords;
            if (OnFilterCDRRecordsLocal != null)
            {

                foreach (var ChargeDetailRecord in ChargeDetailRecords)
                {

                    var FilterResult = OnFilterCDRRecordsLocal(Id, ChargeDetailRecord);

                    if (FilterResult.IsNotNullOrEmpty())
                    {
                        FilteredCDRs.Add(ChargeDetailRecord.SessionId, FilterResult);
                        CDRsToSend.Remove(ChargeDetailRecord);
                    }

                }

            }

            #endregion



            SendCDRsResult result = null;

            if (CDRsToSend.Count ==  0)
                result = SendCDRsResult.NotForwared(Id, Description: "All " + FilteredCDRs.Count + " charge detail record(s) had been filtered!");

            else
            {

                #region Group charge detail records by their targets...

                var UpstreamProviders         = new Dictionary<eMobilityProvider, List<ChargeDetailRecord>>();
                var UpstreamRoamingProviders  = new Dictionary<IEMPRoamingProvider,   List<ChargeDetailRecord>>();
                var UnknownCDRTargets         = new List<ChargeDetailRecord>();

                ChargingStationOperator _Operator;
                ChargingSession         _ChargingSession;
                IEMPRoamingProvider     _EMPRoamingProvider;

                foreach (var ChargeDetailRecord in ChargeDetailRecords)
                {

                    //if (_ChargingSessions.TryGetValue(ChargeDetailRecord.SessionId, out _ChargingSession))

                    #region Group by e-mobility providers...

                    //if (_ChargingSessions_at_Operators.TryGetValue(ChargeDetailRecord.SessionId, out _Operator))
                    //{

                    //    if (!UpstreamOperators.ContainsKey(_Operator))
                    //        UpstreamOperators.Add(_Operator, new List<ChargeDetailRecord>());

                    //    UpstreamOperators[_Operator].Add(ChargeDetailRecord);

                    //}

                    #endregion

                    #region ...or group by EMP roaming operators...

                    //if (_ChargingSessions_at_EMPRoamingProviders.TryGetValue(ChargeDetailRecord.SessionId, out _EMPRoamingProvider))
                    //{

                    //    if (!UpstreamRoamingProviders.ContainsKey(_EMPRoamingProvider))
                    //        UpstreamRoamingProviders.Add(_EMPRoamingProvider, new List<ChargeDetailRecord>());

                    //    UpstreamRoamingProviders[_EMPRoamingProvider].Add(ChargeDetailRecord);

                    //}

                    #endregion

                    #region ...or save as unknown!

                    //else
                    //    UnknownCDRTargets.Add(ChargeDetailRecord);

                    #endregion

                }

                #endregion


                //foreach (var Operator in UpstreamProviderss)
                //{

                //    Operator.Key.send

                //}




                #region An authenticator was found for the upstream SessionId!

                //if (_ChargingSessions.TryGetValue(ChargeDetailRecord.SessionId, out _ChargingSession))
                //{

                //    if (_ChargingSession.AuthService != null)
                //        result = await _ChargingSession.AuthService.SendChargeDetailRecord(Timestamp,
                //                                                                           CancellationToken,
                //                                                                           EventTrackingId,
                //                                                                           ChargeDetailRecord,
                //                                                                           RequestTimeout);

                //    else if (_ChargingSession.OperatorRoamingService != null)
                //        result = await _ChargingSession.OperatorRoamingService.SendChargeDetailRecord(Timestamp,
                //                                                                                      CancellationToken,
                //                                                                                      EventTrackingId,
                //                                                                                      ChargeDetailRecord,
                //                                                                                      RequestTimeout);

                //    _ChargingSession.RemoveMe = true;

                //}

                #endregion

                #region Try to find *Roaming Providers* who might kown anything about the given SessionId!

                if (result == null ||
                    result.Status == SendCDRsResultType.InvalidSessionId)
                {

                    foreach (var iRemoteSendChargeDetailRecord in _IRemoteSendChargeDetailRecord.
                                                                      OrderBy(v => v.Key).
                                                                      Select(v => v.Value).
                                                                      ToArray())
                    {

                        //result = await OtherOperatorRoamingService.SendChargeDetailRecord(Timestamp,
                        //                                                                  CancellationToken,
                        //                                                                  EventTrackingId,
                        //                                                                  ChargeDetailRecord,
                        //                                                                  RequestTimeout);

                        result = await iRemoteSendChargeDetailRecord.
                                           SendChargeDetailRecords(ChargeDetailRecords,
                                                                   CancellationToken: CancellationToken,
                                                                   EventTrackingId: EventTrackingId);

                    }

                }

                #endregion

                #region ...else fail!

                if (result == null ||
                    result.Status == SendCDRsResultType.InvalidSessionId)
                {

                    return SendCDRsResult.NotForwared(Id,
                                                      ChargeDetailRecords,
                                                      "No authorization service returned a positiv result!");

                }

                #endregion

            }


            #region Send OnSendCDRsResponse event

            try
            {

                OnSendCDRsResponse?.Invoke(DateTime.Now,
                                           Timestamp.Value,
                                           this,
                                           EventTrackingId,
                                           Id,
                                           ChargeDetailRecords,
                                           RequestTimeout,
                                           result,
                                           Runtime.Elapsed);

            }
            catch (Exception e)
            {
                e.Log(nameof(RoamingNetwork) + "." + nameof(OnSendCDRsResponse));
            }

            #endregion

            return result;

        }

        #endregion

        #region (internal) SendNewChargeDetailRecord(Timestamp, Sender, ChargeDetailRecord)

        internal void SendNewChargeDetailRecord(DateTime            Timestamp,
                                                Object              Sender,
                                                ChargeDetailRecord  ChargeDetailRecord)
        {

            SendChargeDetailRecords(new ChargeDetailRecord[] { ChargeDetailRecord },

                                    Timestamp,
                                    new CancellationTokenSource().Token,
                                    EventTracking_Id.New,
                                    TimeSpan.FromMinutes(1)).

                                    Wait(TimeSpan.FromMinutes(1));

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
        /// Return a string representation of this object.
        /// </summary>
        public override String ToString()
            => Id.ToString();

        #endregion

    }

}
