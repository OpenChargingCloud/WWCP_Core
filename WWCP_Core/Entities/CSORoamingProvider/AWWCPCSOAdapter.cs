/*
 * Copyright (c) 2014-2025 GraphDefined GmbH <achim.friedland@graphdefined.com>
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

using Org.BouncyCastle.Crypto.Parameters;

using org.GraphDefined.Vanaheimr.Illias;
using org.GraphDefined.Vanaheimr.Hermod.DNS;
using org.GraphDefined.Vanaheimr.Hermod.Logging;

#endregion

namespace cloud.charging.open.protocols.WWCP
{

    /// <summary>
    /// The common abstract WWCP CSO adapter.
    /// Allows you to override only the methods you need.
    /// </summary>
    public abstract class AWWCPCSOAdapter<TChargeDetailRecords> : ACryptoEMobilityEntity<CSORoamingProvider_Id,
                                                                                         CSORoamingProviderAdminStatusTypes,
                                                                                         CSORoamingProviderStatusTypes>,

                                                                  ISendRoamingNetworkData,
                                                                  ISendChargingStationOperatorData,
                                                                  ISendChargingPoolData,
                                                                  ISendChargingStationData,
                                                                  ISendEVSEData,

                                                                  ISendAdminStatus,
                                                                  ISendStatus,
                                                                  ISendEnergyStatus
    {

        #region Data

        public  const           String                                                           Default_LoggingPath                     = "default";

        /// <summary>
        /// The default EVSE data & status interval.
        /// </summary>
        public  readonly static TimeSpan                                                         DefaultFlushEVSEDataAndStatusEvery      = TimeSpan.FromSeconds(31);

        /// <summary>
        /// The default EVSE fast status interval.
        /// </summary>
        public  readonly static TimeSpan                                                         DefaultFlushEVSEFastStatusEvery         = TimeSpan.FromSeconds(3);

        /// <summary>
        /// The default EVSe status refresh interval.
        /// </summary>
        public  readonly static TimeSpan                                                         DefaultEVSEStatusRefreshEvery           = TimeSpan.FromHours(6);

        /// <summary>
        /// The default CDR check interval.
        /// </summary>
        public  readonly static TimeSpan                                                         DefaultFlushChargeDetailRecordsEvery    = TimeSpan.FromSeconds(15);


        protected               UInt64                                                           _FlushEVSEDataRunId                     = 1;
        protected               UInt64                                                           _StatusRunId                            = 1;
        protected               UInt64                                                           _CDRRunId                               = 1;

        protected readonly      SemaphoreSlim                                                    DataAndStatusLock                       = new (1, 1);
        protected readonly      Object                                                           DataAndStatusLockOld                    = new();

        protected readonly      SemaphoreSlim                                                    FlushEVSEDataAndStatusLock              = new (1, 1);
        protected readonly      Timer                                                            FlushEVSEDataAndStatusTimer;

        protected readonly      SemaphoreSlim                                                    FlushEVSEFastStatusLock                 = new (1, 1);
        protected readonly      Timer                                                            FlushEVSEFastStatusTimer;

        protected readonly      SemaphoreSlim                                                    FlushChargeDetailRecordsLock            = new (1, 1);
        protected readonly      Timer                                                            FlushChargeDetailRecordsTimer;

        protected readonly      Dictionary<IRoamingNetwork,         List<PropertyUpdateInfo>>    roamingNetworksUpdateLog;
        protected readonly      Dictionary<ChargingStationOperator, List<PropertyUpdateInfo>>    chargingStationOperatorsUpdateLog;

        protected readonly      Dictionary<IChargingPool,           List<PropertyUpdateInfo>>    chargingPoolsUpdateLog;
        protected readonly      HashSet<IChargingPool>                                           chargingPoolsToAddQueue;
        protected readonly      HashSet<IChargingPool>                                           chargingPoolsToUpdateQueue;
        protected readonly      HashSet<IChargingPool>                                           chargingPoolsToRemoveQueue;
        protected readonly      List<ChargingPoolAdminStatusUpdate>                              chargingPoolAdminStatusChangesFastQueue;
        protected readonly      List<ChargingPoolAdminStatusUpdate>                              chargingPoolAdminStatusChangesDelayedQueue;
        protected readonly      List<ChargingPoolStatusUpdate>                                   chargingPoolStatusChangesFastQueue;
        protected readonly      List<ChargingPoolStatusUpdate>                                   chargingPoolStatusChangesDelayedQueue;

        protected readonly      HashSet<IChargingStation>                                        chargingStationsToAddQueue;
        protected readonly      HashSet<IChargingStation>                                        chargingStationsToUpdateQueue;
        protected readonly      HashSet<IChargingStation>                                        chargingStationsToRemoveQueue;
        protected readonly      List<ChargingStationAdminStatusUpdate>                           chargingStationAdminStatusChangesFastQueue;
        protected readonly      List<ChargingStationAdminStatusUpdate>                           chargingStationAdminStatusChangesDelayedQueue;
        protected readonly      List<ChargingStationStatusUpdate>                                chargingStationStatusChangesFastQueue;
        protected readonly      List<ChargingStationStatusUpdate>                                chargingStationStatusChangesDelayedQueue;
        protected readonly      Dictionary<IChargingStation,        List<PropertyUpdateInfo>>    chargingStationsUpdateLog;

        protected readonly      HashSet<IEVSE>                                                   evsesToAddQueue;
        protected readonly      HashSet<IEVSE>                                                   evsesToUpdateQueue;
        protected readonly      HashSet<IEVSE>                                                   evsesToRemoveQueue;
        protected readonly      List<EVSEAdminStatusUpdate>                                      evseAdminStatusChangesFastQueue;
        protected readonly      List<EVSEAdminStatusUpdate>                                      evseAdminStatusChangesDelayedQueue;
        protected readonly      List<EVSEStatusUpdate>                                           evseStatusChangesFastQueue;
        protected readonly      List<EVSEStatusUpdate>                                           evseStatusChangesDelayedQueue;
        protected readonly      Dictionary<IEVSE,                   List<PropertyUpdateInfo>>    evsesUpdateLog;

        protected readonly      List<TChargeDetailRecords>                                       chargeDetailRecordsQueue;

        protected readonly      TimeSpan                                                         MaxLockWaitingTime                      = TimeSpan.FromSeconds(120);

        public static readonly  TimeSpan                                                         DefaultRequestTimeout                   = TimeSpan.FromSeconds(30);

        #endregion

        #region Properties

        #region Disable... PushXXX/Authentication/SendChargeDetailRecords

        /// <summary>
        /// This service can be disabled, e.g. for debugging reasons.
        /// </summary>
        public Boolean                                   DisablePushData                      { get; set; }

        /// <summary>
        /// This service can be disabled, e.g. for debugging reasons.
        /// </summary>
        public Boolean                                   DisableSendAdminStatus               { get; set; }

        /// <summary>
        /// This service can be disabled, e.g. for debugging reasons.
        /// </summary>
        public Boolean                                   DisableSendStatus                    { get; set; }

        /// <summary>
        /// This service can be disabled, e.g. for debugging reasons.
        /// </summary>
        public Boolean                                   DisableEVSEStatusRefresh             { get; set; }

        /// <summary>
        /// This service can be disabled, e.g. for debugging reasons.
        /// </summary>
        public Boolean                                   DisableSendEnergyStatus              { get; set; }

        /// <summary>
        /// This service can be disabled, e.g. for debugging reasons.
        /// </summary>
        public Boolean                                   DisableAuthorization                { get; set; }

        /// <summary>
        /// This service can be disabled, e.g. for debugging reasons.
        /// </summary>
        public Boolean                                   DisableSendChargeDetailRecords       { get; set; }

        #endregion

        #region Filters

        /// <summary>
        /// A delegate for filtering charge detail records.
        /// </summary>
        public ChargeDetailRecordFilterDelegate          ChargeDetailRecordFilter             { get; }

        #endregion

        #region Flush...Every

        /// <summary>
        /// The EVSE data updates transmission interval.
        /// </summary>
        public TimeSpan                                  FlushEVSEDataAndStatusEvery          { get; set; }

        /// <summary>
        /// The EVSE status updates transmission interval.
        /// </summary>
        public TimeSpan                                  FlushEVSEFastStatusEvery             { get; set; }

        /// <summary>
        /// The EVSE status refresh interval.
        /// </summary>
        public TimeSpan                                  EVSEStatusRefreshEvery               { get; set; }

        /// <summary>
        /// The charge detail record transmission interval.
        /// </summary>
        public TimeSpan                                  FlushChargeDetailRecordsEvery        { get; set; }

        #endregion

        #region Logging

        public Boolean?                                   IsDevelopment            { get; }
        public IEnumerable<String>?                       DevelopmentServers       { get; }
        public Boolean?                                   DisableLogging           { get; set; }
        public String                                     LoggingPath              { get; }
        public String?                                    LoggingContext           { get; }
        public String?                                    LogfileName              { get; }
        public LogfileCreatorDelegate?                    LogfileCreator           { get; }

        public String                                     ClientsLoggingPath       { get; }
        public String?                                    ClientsLoggingContext    { get; }
        public LogfileCreatorDelegate?                    ClientsLogfileCreator    { get; }

        #endregion

        public IDNSClient?  DNSClient    { get; }

        #endregion

        #region Events

        #region OnWWCPCPOAdapterException

        public delegate Task OnWWCPCPOAdapterExceptionDelegate(DateTimeOffset                         Timestamp,
                                                               AWWCPCSOAdapter<TChargeDetailRecords>  Sender,
                                                               Exception                              Exception);

        public event OnWWCPCPOAdapterExceptionDelegate OnWWCPCPOAdapterException;

        #endregion


        #region FlushEVSEDataAndStatusQueues

        public delegate void FlushEVSEDataAndStatusQueuesStartedDelegate(AWWCPCSOAdapter<TChargeDetailRecords> Sender, DateTimeOffset StartTime, TimeSpan Every, UInt64 RunId);

        public event FlushEVSEDataAndStatusQueuesStartedDelegate? FlushEVSEDataAndStatusQueuesStartedEvent;

        public delegate void FlushEVSEDataAndStatusQueuesFinishedDelegate(AWWCPCSOAdapter<TChargeDetailRecords> Sender, DateTimeOffset StartTime, DateTimeOffset EndTime, TimeSpan Runtime, TimeSpan Every, UInt64 RunId);

        public event FlushEVSEDataAndStatusQueuesFinishedDelegate? FlushEVSEDataAndStatusQueuesFinishedEvent;

        #endregion

        #region FlushEVSEFastStatusQueues

        public delegate void FlushEVSEFastStatusQueuesStartedDelegate(AWWCPCSOAdapter<TChargeDetailRecords> Sender, DateTimeOffset StartTime, TimeSpan Every, UInt64 RunId);

        public event FlushEVSEFastStatusQueuesStartedDelegate? FlushEVSEFastStatusQueuesStartedEvent;

        public delegate void FlushEVSEFastStatusQueuesFinishedDelegate(AWWCPCSOAdapter<TChargeDetailRecords> Sender, DateTimeOffset StartTime, DateTimeOffset EndTime, TimeSpan Runtime, TimeSpan Every, UInt64 RunId);

        public event FlushEVSEFastStatusQueuesFinishedDelegate? FlushEVSEFastStatusQueuesFinishedEvent;

        #endregion

        #region FlushChargeDetailRecordsQueues

        public delegate void FlushChargeDetailRecordsQueuesStartedDelegate(AWWCPCSOAdapter<TChargeDetailRecords> Sender, DateTimeOffset StartTime, TimeSpan Every, UInt64 RunId);

        public event FlushChargeDetailRecordsQueuesStartedDelegate? FlushChargeDetailRecordsQueuesStartedEvent;

        public delegate void FlushChargeDetailRecordsQueuesFinishedDelegate(AWWCPCSOAdapter<TChargeDetailRecords> Sender, DateTimeOffset StartTime, DateTimeOffset EndTime, TimeSpan Runtime, TimeSpan Every, UInt64 RunId);

        public event FlushChargeDetailRecordsQueuesFinishedDelegate? FlushChargeDetailRecordsQueuesFinishedEvent;

        #endregion

        public delegate Task OnWarningsDelegate(DateTimeOffset Timestamp, String Class, String Method, IEnumerable<Warning> Warnings);

        public event OnWarningsDelegate? OnWarnings;

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new WWCP wrapper for the OICP roaming client for Charging Station Operators/CPOs.
        /// </summary>
        /// <param name="Id">The unique identification of the roaming provider.</param>
        /// <param name="Name">The official (multi-language) name of the roaming provider.</param>
        /// <param name="Description">An optional (multi-language) description of the charging station operator roaming provider.</param>
        /// <param name="RoamingNetwork">A WWCP roaming network.</param>
        /// 
        /// <param name="IncludeEVSEIds">Only include EVSE identifications matching the given delegate.</param>
        /// <param name="IncludeEVSEs">Only include EVSEs matching the given delegate.</param>
        /// <param name="IncludeChargingStationIds">Only include charging station identifications matching the given delegate.</param>
        /// <param name="IncludeChargingStations">Only include charging stations matching the given delegate.</param>
        /// <param name="IncludeChargingPoolIds">Only include charging pool identifications matching the given delegate.</param>
        /// <param name="IncludeChargingPools">Only include charging pools matching the given delegate.</param>
        /// <param name="ChargeDetailRecordFilter">A delegate for filtering charge detail records.</param>
        /// 
        /// <param name="FlushEVSEDataAndStatusEvery">The service check interval.</param>
        /// <param name="FlushEVSEFastStatusEvery">The status check interval.</param>
        /// <param name="FlushChargeDetailRecordsEvery">The charge detail record interval.</param>
        /// 
        /// <param name="DisablePushData">This service can be disabled, e.g. for debugging reasons.</param>
        /// <param name="DisablePushStatus">This service can be disabled, e.g. for debugging reasons.</param>
        /// <param name="DisableAuthorization">This service can be disabled, e.g. for debugging reasons.</param>
        /// <param name="DisableSendChargeDetailRecords">This service can be disabled, e.g. for debugging reasons.</param>
        protected AWWCPCSOAdapter(CSORoamingProvider_Id                      Id,
                                  IRoamingNetwork                            RoamingNetwork,

                                  I18NString?                                Name                                = null,
                                  I18NString?                                Description                         = null,

                                  IncludeEVSEIdDelegate?                     IncludeEVSEIds                      = null,
                                  IncludeEVSEDelegate?                       IncludeEVSEs                        = null,
                                  IncludeChargingStationIdDelegate?          IncludeChargingStationIds           = null,
                                  IncludeChargingStationDelegate?            IncludeChargingStations             = null,
                                  IncludeChargingPoolIdDelegate?             IncludeChargingPoolIds              = null,
                                  IncludeChargingPoolDelegate?               IncludeChargingPools                = null,
                                  IncludeChargingStationOperatorIdDelegate?  IncludeChargingStationOperatorIds   = null,
                                  IncludeChargingStationOperatorDelegate?    IncludeChargingStationOperators     = null,
                                  ChargeDetailRecordFilterDelegate?          ChargeDetailRecordFilter            = null,

                                  TimeSpan?                                  FlushEVSEDataAndStatusEvery         = null,
                                  TimeSpan?                                  FlushEVSEFastStatusEvery            = null,
                                  TimeSpan?                                  EVSEStatusRefreshEvery              = null,
                                  TimeSpan?                                  FlushChargeDetailRecordsEvery       = null,

                                  Boolean                                    DisablePushData                     = false,
                                  Boolean                                    DisablePushAdminStatus              = false,
                                  Boolean                                    DisablePushStatus                   = false,
                                  Boolean                                    DisableEVSEStatusRefresh            = false,
                                  Boolean                                    DisablePushEnergyStatus             = false,
                                  Boolean                                    DisableAuthorization                = false,
                                  Boolean                                    DisableSendChargeDetailRecords      = false,

                                  String                                     EllipticCurve                       = "P-256",
                                  ECPrivateKeyParameters?                    PrivateKey                          = null,
                                  PublicKeyCertificates?                     PublicKeyCertificates               = null,

                                  Boolean?                                   IsDevelopment                       = null,
                                  IEnumerable<String>?                       DevelopmentServers                  = null,
                                  Boolean?                                   DisableLogging                      = false,
                                  String?                                    LoggingPath                         = null,
                                  String?                                    LoggingContext                      = null,
                                  String?                                    LogfileName                         = null,
                                  LogfileCreatorDelegate?                    LogfileCreator                      = null,

                                  String?                                    ClientsLoggingPath                  = null,
                                  String?                                    ClientsLoggingContext               = null,
                                  LogfileCreatorDelegate?                    ClientsLogfileCreator               = null,
                                  IDNSClient?                                DNSClient                           = null)

            : base(Id,
                   RoamingNetwork,
                   Name,
                   Description,
                   EllipticCurve,
                   PrivateKey,
                   PublicKeyCertificates)

        {

            this.IncludeEVSEIds                                  = IncludeEVSEIds                    ?? (evseid             => true);
            this.IncludeEVSEs                                    = IncludeEVSEs                      ?? (evse               => true);
            this.IncludeChargingStationIds                       = IncludeChargingStationIds         ?? (chargingStationId  => true);
            this.IncludeChargingStations                         = IncludeChargingStations           ?? (chargingStation    => true);
            this.IncludeChargingPoolIds                          = IncludeChargingPoolIds            ?? (chargingPoolId     => true);
            this.IncludeChargingPools                            = IncludeChargingPools              ?? (chargingPool       => true);
            this.IncludeChargingStationOperatorIds               = IncludeChargingStationOperatorIds ?? (chargingStationId  => true);
            this.IncludeChargingStationOperators                 = IncludeChargingStationOperators   ?? (chargingStation    => true);
            this.ChargeDetailRecordFilter                        = ChargeDetailRecordFilter          ?? (chargeDetailRecord => ChargeDetailRecordFilters.forward);

            this.DisablePushData                                 = DisablePushData;
            this.DisableSendAdminStatus                          = DisablePushAdminStatus;
            this.DisableSendStatus                               = DisablePushStatus;
            this.DisableEVSEStatusRefresh                        = DisableEVSEStatusRefresh;
            this.DisableSendEnergyStatus                         = DisablePushEnergyStatus;
            this.DisableAuthorization                            = DisableAuthorization;
            this.DisableSendChargeDetailRecords                  = DisableSendChargeDetailRecords;

            this.FlushEVSEDataAndStatusEvery                     = FlushEVSEDataAndStatusEvery       ?? DefaultFlushEVSEDataAndStatusEvery;
            this.FlushEVSEFastStatusEvery                        = FlushEVSEFastStatusEvery          ?? DefaultFlushEVSEFastStatusEvery;
            this.EVSEStatusRefreshEvery                          = EVSEStatusRefreshEvery            ?? DefaultEVSEStatusRefreshEvery;
            this.FlushChargeDetailRecordsEvery                   = FlushChargeDetailRecordsEvery     ?? DefaultFlushChargeDetailRecordsEvery;

            this.FlushEVSEDataAndStatusTimer                     = new Timer(FlushEVSEDataAndStatus);
            this.FlushEVSEFastStatusTimer                        = new Timer(FlushEVSEFastStatus);
            this.FlushChargeDetailRecordsTimer                   = new Timer(FlushChargeDetailRecords);

            this.chargingPoolsToAddQueue                         = new HashSet<IChargingPool>();
            this.chargingPoolsToUpdateQueue                      = new HashSet<IChargingPool>();
            this.chargingPoolsToRemoveQueue                      = new HashSet<IChargingPool>();
            this.chargingPoolAdminStatusChangesFastQueue         = new List<ChargingPoolAdminStatusUpdate>();
            this.chargingPoolAdminStatusChangesDelayedQueue      = new List<ChargingPoolAdminStatusUpdate>();
            this.chargingPoolStatusChangesFastQueue              = new List<ChargingPoolStatusUpdate>();
            this.chargingPoolStatusChangesDelayedQueue           = new List<ChargingPoolStatusUpdate>();
            this.chargingPoolsUpdateLog                          = new Dictionary<IChargingPool,    List<PropertyUpdateInfo>>();

            this.chargingStationsToAddQueue                      = new HashSet<IChargingStation>();
            this.chargingStationsToUpdateQueue                   = new HashSet<IChargingStation>();
            this.chargingStationsToRemoveQueue                   = new HashSet<IChargingStation>();
            this.chargingStationAdminStatusChangesFastQueue      = new List<ChargingStationAdminStatusUpdate>();
            this.chargingStationAdminStatusChangesDelayedQueue   = new List<ChargingStationAdminStatusUpdate>();
            this.chargingStationStatusChangesFastQueue           = new List<ChargingStationStatusUpdate>();
            this.chargingStationStatusChangesDelayedQueue        = new List<ChargingStationStatusUpdate>();
            this.chargingStationsUpdateLog                       = new Dictionary<IChargingStation, List<PropertyUpdateInfo>>();

            this.evsesToAddQueue                                 = new HashSet<IEVSE>();
            this.evsesToUpdateQueue                              = new HashSet<IEVSE>();
            this.evsesToRemoveQueue                              = new HashSet<IEVSE>();
            this.evseAdminStatusChangesFastQueue                 = new List<EVSEAdminStatusUpdate>();
            this.evseAdminStatusChangesDelayedQueue              = new List<EVSEAdminStatusUpdate>();
            this.evseStatusChangesFastQueue                      = new List<EVSEStatusUpdate>();
            this.evseStatusChangesDelayedQueue                   = new List<EVSEStatusUpdate>();
            this.evsesUpdateLog                                  = new Dictionary<IEVSE,            List<PropertyUpdateInfo>>();

            this.chargeDetailRecordsQueue                        = new List<TChargeDetailRecords>();

            this.IsDevelopment                                   = IsDevelopment;
            this.DevelopmentServers                              = DevelopmentServers;
            this.DisableLogging                                  = DisableLogging;
            this.LoggingPath                                     = LoggingPath              ?? Path.Combine(AppContext.BaseDirectory, Default_LoggingPath);
            this.LoggingContext                                  = LoggingContext;
            this.LogfileName                                     = LogfileName;
            this.LogfileCreator                                  = LogfileCreator;

            if (this.LoggingPath[^1]        != Path.DirectorySeparatorChar)
                this.LoggingPath            += Path.DirectorySeparatorChar;

            this.ClientsLoggingPath                              = ClientsLoggingPath       ?? this.LoggingPath;
            this.ClientsLoggingContext                           = ClientsLoggingContext;
            this.ClientsLogfileCreator                           = ClientsLogfileCreator;

            if (this.ClientsLoggingPath[^1] != Path.DirectorySeparatorChar)
                this.ClientsLoggingPath     += Path.DirectorySeparatorChar;

            this.DNSClient                                       = DNSClient;

        }

        #endregion


        #region (Set/Add/Update/Delete) Roaming network...

        /// <summary>
        /// This service can be disabled, e.g. for debugging reasons.
        /// </summary>
        public Boolean                          DisableSendRoamingNetworkData    { get; set; }

        /// <summary>
        /// Only include roaming network identifications matching the given delegate.
        /// </summary>
        public IncludeRoamingNetworkIdDelegate  IncludeRoamingNetworkIds         { get; }

        /// <summary>
        /// Only include roaming network matching the given delegate.
        /// </summary>
        public IncludeRoamingNetworkDelegate    IncludeRoamingNetworks           { get; }


        #region AddRoamingNetwork            (RoamingNetwork, ...)

        /// <summary>
        /// Add the EVSE data of the given roaming network.
        /// </summary>
        /// <param name="RoamingNetwork">A roaming network.</param>
        /// <param name="TransmissionType">Whether to send the roaming network update directly or enqueue it for a while.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        public virtual Task<AddRoamingNetworkResult>

            AddRoamingNetwork(IRoamingNetwork    RoamingNetwork,
                              TransmissionTypes  TransmissionType    = TransmissionTypes.Enqueue,

                              DateTimeOffset?    Timestamp           = null,
                              EventTracking_Id?  EventTrackingId     = null,
                              TimeSpan?          RequestTimeout      = null,
                              User_Id?           CurrentUserId       = null,
                              CancellationToken  CancellationToken   = default)


                => Task.FromResult(
                       AddRoamingNetworkResult.NoOperation(
                           RoamingNetwork:   RoamingNetwork,
                           EventTrackingId:  EventTrackingId,
                           SenderId:         Id,
                           Sender:           this
                       )
                   );

        #endregion

        #region AddRoamingNetworkIfNotExists (RoamingNetwork, ...)

        /// <summary>
        /// Add the EVSE data of the given roaming network.
        /// </summary>
        /// <param name="RoamingNetwork">A roaming network.</param>
        /// <param name="TransmissionType">Whether to send the roaming network update directly or enqueue it for a while.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        public virtual Task<AddRoamingNetworkResult>

            AddRoamingNetworkIfNotExists(IRoamingNetwork    RoamingNetwork,
                                         TransmissionTypes  TransmissionType    = TransmissionTypes.Enqueue,

                                         DateTimeOffset?    Timestamp           = null,
                                         EventTracking_Id?  EventTrackingId     = null,
                                         TimeSpan?          RequestTimeout      = null,
                                         User_Id?           CurrentUserId       = null,
                                         CancellationToken  CancellationToken   = default)


                => Task.FromResult(
                       AddRoamingNetworkResult.NoOperation(
                           RoamingNetwork:   RoamingNetwork,
                           EventTrackingId:  EventTrackingId,
                           SenderId:         Id,
                           Sender:           this
                       )
                   );

        #endregion

        #region AddOrUpdateRoamingNetwork    (RoamingNetwork, ...)

        /// <summary>
        /// Set the EVSE data of the given roaming network as new static EVSE data.
        /// </summary>
        /// <param name="RoamingNetwork">A roaming network.</param>
        /// <param name="TransmissionType">Whether to send the roaming network update directly or enqueue it for a while.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        public virtual Task<AddOrUpdateRoamingNetworkResult>

            AddOrUpdateRoamingNetwork(IRoamingNetwork    RoamingNetwork,
                                      TransmissionTypes  TransmissionType    = TransmissionTypes.Enqueue,

                                      DateTimeOffset?    Timestamp           = null,
                                      EventTracking_Id?  EventTrackingId     = null,
                                      TimeSpan?          RequestTimeout      = null,
                                      User_Id?           CurrentUserId       = null,
                                      CancellationToken  CancellationToken   = default)


                => Task.FromResult(
                       AddOrUpdateRoamingNetworkResult.NoOperation(
                           RoamingNetwork:   RoamingNetwork,
                           EventTrackingId:  EventTrackingId,
                           SenderId:         Id,
                           Sender:           this
                       )
                   );

        #endregion

        #region UpdateRoamingNetwork         (RoamingNetwork, PropertyName = null, NewValue = null, OldValue = null, DataSource = null, TransmissionType = Enqueue, ...)

        /// <summary>
        /// Update the EVSE data of the given roaming network within the static EVSE data.
        /// </summary>
        /// <param name="RoamingNetwork">A roaming network.</param>
        /// <param name="PropertyName">The name of the roaming network property to update, if any specific.</param>
        /// <param name="NewValue">The new value of the roaming network property to update.</param>
        /// <param name="OldValue">The optional old value of the roaming network property to update.</param>
        /// <param name="DataSource">An optional data source or context for the roaming network property update.</param>
        /// <param name="TransmissionType">Whether to send the roaming network update directly or enqueue it for a while.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        public virtual Task<UpdateRoamingNetworkResult>

            UpdateRoamingNetwork(IRoamingNetwork    RoamingNetwork,
                                 String?            PropertyName        = null,
                                 Object?            NewValue            = null,
                                 Object?            OldValue            = null,
                                 Context?           DataSource          = null,
                                 TransmissionTypes  TransmissionType    = TransmissionTypes.Enqueue,

                                 DateTimeOffset?    Timestamp           = null,
                                 EventTracking_Id?  EventTrackingId     = null,
                                 TimeSpan?          RequestTimeout      = null,
                                 User_Id?           CurrentUserId       = null,
                                 CancellationToken  CancellationToken   = default)


                => Task.FromResult(
                       UpdateRoamingNetworkResult.NoOperation(
                           RoamingNetwork:   RoamingNetwork,
                           EventTrackingId:  EventTrackingId,
                           SenderId:         Id,
                           Sender:           this
                       )
                   );

        #endregion

        #region DeleteRoamingNetwork         (RoamingNetwork, ...)

        /// <summary>
        /// Delete the EVSE data of the given roaming network from the static EVSE data.
        /// </summary>
        /// <param name="RoamingNetwork">A roaming network to upload.</param>
        /// <param name="TransmissionType">Whether to send the roaming network update directly or enqueue it for a while.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        public virtual Task<DeleteRoamingNetworkResult>

            DeleteRoamingNetwork(IRoamingNetwork    RoamingNetwork,
                                 TransmissionTypes  TransmissionType    = TransmissionTypes.Enqueue,

                                 DateTimeOffset?    Timestamp           = null,
                                 EventTracking_Id?  EventTrackingId     = null,
                                 TimeSpan?          RequestTimeout      = null,
                                 User_Id?           CurrentUserId       = null,
                                 CancellationToken  CancellationToken   = default)


                => Task.FromResult(
                       DeleteRoamingNetworkResult.NoOperation(
                           RoamingNetwork:   RoamingNetwork,
                           EventTrackingId:  EventTrackingId,
                           SenderId:         Id,
                           Sender:           this
                       )
                   );

        #endregion


        #region AddRoamingNetworks           (RoamingNetworks, TransmissionType = Enqueue, ...)

        /// <summary>
        /// Add the given enumeration of roaming networks.
        /// </summary>
        /// <param name="RoamingNetworks">An enumeration of roaming networks.</param>
        /// <param name="TransmissionType">Whether to send the roaming network directly or enqueue it for a while.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        public virtual Task<AddRoamingNetworksResult>

            AddRoamingNetworks(IEnumerable<IRoamingNetwork>  RoamingNetworks,
                               TransmissionTypes             TransmissionType    = TransmissionTypes.Enqueue,

                               DateTimeOffset?               Timestamp           = null,
                               EventTracking_Id?             EventTrackingId     = null,
                               TimeSpan?                     RequestTimeout      = null,
                               User_Id?                      CurrentUserId       = null,
                               CancellationToken             CancellationToken   = default)


                => Task.FromResult(
                       AddRoamingNetworksResult.NoOperation(
                           RejectedRoamingNetworks:  RoamingNetworks,
                           SenderId:                 Id,
                           Sender:                   this,
                           EventTrackingId:          EventTrackingId
                       )
                   );

        #endregion

        #region AddRoamingNetworksIfNotExist (RoamingNetworks, TransmissionType = Enqueue, ...)

        /// <summary>
        /// Add the given enumeration of roaming networks, if they do not already exist.
        /// </summary>
        /// <param name="RoamingNetworks">An enumeration of roaming networks to add, if they do not already exist.</param>
        /// <param name="TransmissionType">Whether to send the roaming network directly or enqueue it for a while.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        public virtual Task<AddRoamingNetworksResult>

            AddRoamingNetworksIfNotExist(IEnumerable<IRoamingNetwork>  RoamingNetworks,
                                         TransmissionTypes             TransmissionType    = TransmissionTypes.Enqueue,

                                         DateTimeOffset?               Timestamp           = null,
                                         EventTracking_Id?             EventTrackingId     = null,
                                         TimeSpan?                     RequestTimeout      = null,
                                         User_Id?                      CurrentUserId       = null,
                                         CancellationToken             CancellationToken   = default)


                => Task.FromResult(
                       AddRoamingNetworksResult.NoOperation(
                           RejectedRoamingNetworks:  RoamingNetworks,
                           SenderId:                 Id,
                           Sender:                   this,
                           EventTrackingId:          EventTrackingId
                       )
                   );

        #endregion

        #region AddOrUpdateRoamingNetworks   (RoamingNetworks, TransmissionType = Enqueue, ...)

        /// <summary>
        /// Add or update the given enumeration of roaming networks.
        /// </summary>
        /// <param name="RoamingNetworks">An enumeration of roaming networks to add or update.</param>
        /// <param name="TransmissionType">Whether to send the roaming network directly or enqueue it for a while.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        public virtual Task<AddOrUpdateRoamingNetworksResult>

            AddOrUpdateRoamingNetworks(IEnumerable<IRoamingNetwork>  RoamingNetworks,
                                       TransmissionTypes             TransmissionType    = TransmissionTypes.Enqueue,

                                       DateTimeOffset?               Timestamp           = null,
                                       EventTracking_Id?             EventTrackingId     = null,
                                       TimeSpan?                     RequestTimeout      = null,
                                       User_Id?                      CurrentUserId       = null,
                                       CancellationToken             CancellationToken   = default)


                => Task.FromResult(
                       AddOrUpdateRoamingNetworksResult.NoOperation(
                           RejectedRoamingNetworks:  RoamingNetworks,
                           SenderId:                 Id,
                           Sender:                   this,
                           EventTrackingId:          EventTrackingId
                       )
                   );

        #endregion

        #region UpdateRoamingNetworks        (RoamingNetworks, TransmissionType = Enqueue, ...)

        /// <summary>
        /// Update the given enumeration of roaming networks.
        /// </summary>
        /// <param name="RoamingNetworks">An enumeration of roaming networks to update.</param>
        /// <param name="TransmissionType">Whether to send the roaming network directly or enqueue it for a while.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        public virtual Task<UpdateRoamingNetworksResult>

            UpdateRoamingNetworks(IEnumerable<IRoamingNetwork>  RoamingNetworks,
                                  TransmissionTypes             TransmissionType    = TransmissionTypes.Enqueue,

                                  DateTimeOffset?               Timestamp           = null,
                                  EventTracking_Id?             EventTrackingId     = null,
                                  TimeSpan?                     RequestTimeout      = null,
                                  User_Id?                      CurrentUserId       = null,
                                  CancellationToken             CancellationToken   = default)


                => Task.FromResult(
                       UpdateRoamingNetworksResult.NoOperation(
                           RejectedRoamingNetworks:  RoamingNetworks,
                           SenderId:                 Id,
                           Sender:                   this,
                           EventTrackingId:          EventTrackingId
                       )
                   );

        #endregion

        #region DeleteRoamingNetworks        (RoamingNetworks, TransmissionType = Enqueue, ...)

        /// <summary>
        /// Delete the given enumeration of roaming networks.
        /// </summary>
        /// <param name="RoamingNetworks">An enumeration of roaming networks to delete.</param>
        /// <param name="TransmissionType">Whether to send the roaming network directly or enqueue it for a while.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        public virtual Task<DeleteRoamingNetworksResult>

            DeleteRoamingNetworks(IEnumerable<IRoamingNetwork>  RoamingNetworks,
                                  TransmissionTypes             TransmissionType    = TransmissionTypes.Enqueue,

                                  DateTimeOffset?               Timestamp           = null,
                                  EventTracking_Id?             EventTrackingId     = null,
                                  TimeSpan?                     RequestTimeout      = null,
                                  User_Id?                      CurrentUserId       = null,
                                  CancellationToken             CancellationToken   = default)


                => Task.FromResult(
                       DeleteRoamingNetworksResult.NoOperation(
                           RejectedRoamingNetworks:  RoamingNetworks,
                           SenderId:                 Id,
                           Sender:                   this,
                           EventTrackingId:          EventTrackingId
                       )
                   );

        #endregion


        #region UpdateRoamingNetworkAdminStatus (RoamingNetworkAdminStatusUpdates, TransmissionType = Enqueue, ...)

        /// <summary>
        /// Update the given enumeration of roaming network admin status updates.
        /// </summary>
        /// <param name="RoamingNetworkAdminStatusUpdates">An enumeration of roaming network admin status updates.</param>
        /// <param name="TransmissionType">Whether to send the roaming network admin status updates directly or enqueue it for a while.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        public virtual Task<PushRoamingNetworkAdminStatusResult>

            UpdateRoamingNetworkAdminStatus(IEnumerable<RoamingNetworkAdminStatusUpdate>  RoamingNetworkAdminStatusUpdates,
                                            TransmissionTypes                             TransmissionType    = TransmissionTypes.Enqueue,

                                            DateTimeOffset?                               Timestamp           = null,
                                            EventTracking_Id?                             EventTrackingId     = null,
                                            TimeSpan?                                     RequestTimeout      = null,
                                            User_Id?                                      CurrentUserId       = null,
                                            CancellationToken                             CancellationToken   = default)


                => Task.FromResult(
                       PushRoamingNetworkAdminStatusResult.NoOperation(
                           Id,
                           this
                       )
                   );

        #endregion

        #region UpdateRoamingNetworkStatus      (RoamingNetworkStatusUpdates,      TransmissionType = Enqueue, ...)

        /// <summary>
        /// Update the given enumeration of roaming network status updates.
        /// </summary>
        /// <param name="RoamingNetworkStatusUpdates">An enumeration of roaming network status updates.</param>
        /// <param name="TransmissionType">Whether to send the roaming network status updates directly or enqueue it for a while.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        public virtual Task<PushRoamingNetworkStatusResult>

            UpdateRoamingNetworkStatus(IEnumerable<RoamingNetworkStatusUpdate>  RoamingNetworkStatusUpdates,
                                       TransmissionTypes                        TransmissionType    = TransmissionTypes.Enqueue,

                                       DateTimeOffset?                          Timestamp           = null,
                                       EventTracking_Id?                        EventTrackingId     = null,
                                       TimeSpan?                                RequestTimeout      = null,
                                       User_Id?                                 CurrentUserId       = null,
                                       CancellationToken                        CancellationToken   = default)


                => Task.FromResult(
                       PushRoamingNetworkStatusResult.NoOperation(
                           Id,
                           this
                       )
                   );

        #endregion

        #endregion

        #region (Set/Add/Update/Delete) Charging station operator(s)...

        /// <summary>
        /// This service can be disabled, e.g. for debugging reasons.
        /// </summary>
        public Boolean                                   DisableSendChargingStationOperatorData    { get; set; }

        /// <summary>
        /// Only include charging station identifications matching the given delegate.
        /// </summary>
        public IncludeChargingStationOperatorIdDelegate  IncludeChargingStationOperatorIds         { get; }

        /// <summary>
        /// Only include charging stations matching the given delegate.
        /// </summary>
        public IncludeChargingStationOperatorDelegate    IncludeChargingStationOperators           { get; }


        #region AddChargingStationOperator            (ChargingStationOperator, ...)

        /// <summary>
        /// Add the given charging station.
        /// </summary>
        /// <param name="ChargingStationOperator">A charging station operator.</param>
        /// <param name="TransmissionType">Whether to send the charging pool update directly or enqueue it for a while.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        public virtual Task<AddChargingStationOperatorResult>

            AddChargingStationOperator(IChargingStationOperator  ChargingStationOperator,
                                       TransmissionTypes         TransmissionType    = TransmissionTypes.Enqueue,

                                       DateTimeOffset?           Timestamp           = null,
                                       EventTracking_Id?         EventTrackingId     = null,
                                       TimeSpan?                 RequestTimeout      = null,
                                       User_Id?                  CurrentUserId       = null,
                                       CancellationToken         CancellationToken   = default)


                => Task.FromResult(
                       AddChargingStationOperatorResult.NoOperation(
                           ChargingStationOperator:  ChargingStationOperator,
                           EventTrackingId:          EventTrackingId,
                           SenderId:                 Id,
                           Sender:                   this
                       )
                   );

        #endregion

        #region AddChargingStationOperatorIfNotExists (ChargingStationOperator, ...)

        /// <summary>
        /// Add the given charging station.
        /// </summary>
        /// <param name="ChargingStationOperator">A charging station operator.</param>
        /// <param name="TransmissionType">Whether to send the charging pool update directly or enqueue it for a while.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        public virtual Task<AddChargingStationOperatorResult>

            AddChargingStationOperatorIfNotExists(IChargingStationOperator  ChargingStationOperator,
                                                  TransmissionTypes         TransmissionType    = TransmissionTypes.Enqueue,

                                                  DateTimeOffset?           Timestamp           = null,
                                                  EventTracking_Id?         EventTrackingId     = null,
                                                  TimeSpan?                 RequestTimeout      = null,
                                                  User_Id?                  CurrentUserId       = null,
                                                  CancellationToken         CancellationToken   = default)


                => Task.FromResult(
                       AddChargingStationOperatorResult.NoOperation(
                           ChargingStationOperator:  ChargingStationOperator,
                           EventTrackingId:          EventTrackingId,
                           SenderId:                 Id,
                           Sender:                   this
                       )
                   );

        #endregion

        #region AddOrUpdateChargingStationOperator    (ChargingStationOperator, ...)

        /// <summary>
        /// Set the EVSE data of the given charging station operator as new static EVSE data.
        /// </summary>
        /// <param name="ChargingStationOperator">A charging station operator.</param>
        /// <param name="TransmissionType">Whether to send the charging pool update directly or enqueue it for a while.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        public virtual Task<AddOrUpdateChargingStationOperatorResult>

            AddOrUpdateChargingStationOperator(IChargingStationOperator  ChargingStationOperator,
                                               TransmissionTypes         TransmissionType    = TransmissionTypes.Enqueue,

                                               DateTimeOffset?           Timestamp           = null,
                                               EventTracking_Id?         EventTrackingId     = null,
                                               TimeSpan?                 RequestTimeout      = null,
                                               User_Id?                  CurrentUserId       = null,
                                               CancellationToken         CancellationToken   = default)


                => Task.FromResult(
                       AddOrUpdateChargingStationOperatorResult.NoOperation(
                           ChargingStationOperator:  ChargingStationOperator,
                           EventTrackingId:          EventTrackingId,
                           SenderId:                 Id,
                           Sender:                   this
                       )
                   );

        #endregion

        #region UpdateChargingStationOperator         (ChargingStationOperator, PropertyName = null, NewValue = null, OldValue = null, DataSource = null, TransmissionType = Enqueue, ...)

        /// <summary>
        /// Update the EVSE data of the given charging station operator within the static EVSE data.
        /// </summary>
        /// <param name="ChargingStationOperator">A charging station operator.</param>
        /// <param name="PropertyName">The name of the charging station operator property to update, if any specific.</param>
        /// <param name="NewValue">The new value of the charging station operator property to update.</param>
        /// <param name="OldValue">The optional old value of the charging station operator property to update.</param>
        /// <param name="DataSource">An optional data source or context for the charging station operator property update.</param>
        /// <param name="TransmissionType">Whether to send the charging pool update directly or enqueue it for a while.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        public virtual Task<UpdateChargingStationOperatorResult>

            UpdateChargingStationOperator(IChargingStationOperator  ChargingStationOperator,
                                          String?                   PropertyName        = null,
                                          Object?                   NewValue            = null,
                                          Object?                   OldValue            = null,
                                          Context?                  DataSource          = null,
                                          TransmissionTypes         TransmissionType    = TransmissionTypes.Enqueue,

                                          DateTimeOffset?           Timestamp           = null,
                                          EventTracking_Id?         EventTrackingId     = null,
                                          TimeSpan?                 RequestTimeout      = null,
                                          User_Id?                  CurrentUserId       = null,
                                          CancellationToken         CancellationToken   = default)


                => Task.FromResult(
                       UpdateChargingStationOperatorResult.NoOperation(
                           ChargingStationOperator:  ChargingStationOperator,
                           EventTrackingId:          EventTrackingId,
                           SenderId:                 Id,
                           Sender:                   this
                       )
                   );

        #endregion

        #region DeleteChargingStationOperator         (ChargingStationOperator, ...)

        /// <summary>
        /// Delete the EVSE data of the given charging station operator from the static EVSE data.
        /// </summary>
        /// <param name="ChargingStationOperator">A charging station operator.</param>
        /// <param name="TransmissionType">Whether to send the charging pool update directly or enqueue it for a while.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        public virtual Task<DeleteChargingStationOperatorResult>

            DeleteChargingStationOperator(IChargingStationOperator  ChargingStationOperator,
                                          TransmissionTypes         TransmissionType    = TransmissionTypes.Enqueue,

                                          DateTimeOffset?           Timestamp           = null,
                                          EventTracking_Id?         EventTrackingId     = null,
                                          TimeSpan?                 RequestTimeout      = null,
                                          User_Id?                  CurrentUserId       = null,
                                          CancellationToken         CancellationToken   = default)


                => Task.FromResult(
                       DeleteChargingStationOperatorResult.NoOperation(
                           ChargingStationOperator:  ChargingStationOperator,
                           EventTrackingId:          EventTrackingId,
                           SenderId:                 Id,
                           Sender:                   this
                       )
                   );

        #endregion


        #region AddChargingStationOperators           (ChargingStationOperators, ...)

        /// <summary>
        /// Add the EVSE data of the given enumeration of charging station operators.
        /// </summary>
        /// <param name="ChargingStationOperators">An enumeration of charging station operators.</param>
        /// <param name="TransmissionType">Whether to send the charging pool update directly or enqueue it for a while.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        public virtual Task<AddChargingStationOperatorsResult>

            AddChargingStationOperators(IEnumerable<IChargingStationOperator>  ChargingStationOperators,
                                        TransmissionTypes                      TransmissionType    = TransmissionTypes.Enqueue,

                                        DateTimeOffset?                        Timestamp           = null,
                                        EventTracking_Id?                      EventTrackingId     = null,
                                        TimeSpan?                              RequestTimeout      = null,
                                        User_Id?                               CurrentUserId       = null,
                                        CancellationToken                      CancellationToken   = default)


                => Task.FromResult(
                       AddChargingStationOperatorsResult.NoOperation(
                           RejectedChargingStationOperators:  ChargingStationOperators,
                           SenderId:                          Id,
                           Sender:                            this,
                           EventTrackingId:                   EventTrackingId
                       )
                   );

        #endregion

        #region AddChargingStationOperatorsIfNotExist (ChargingStationOperators, ...)

        /// <summary>
        /// Add the EVSE data of the given enumeration of charging station operators.
        /// </summary>
        /// <param name="ChargingStationOperators">An enumeration of charging station operators.</param>
        /// <param name="TransmissionType">Whether to send the charging pool update directly or enqueue it for a while.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        public virtual Task<AddChargingStationOperatorsResult>

            AddChargingStationOperatorsIfNotExist(IEnumerable<IChargingStationOperator>  ChargingStationOperators,
                                                  TransmissionTypes                      TransmissionType    = TransmissionTypes.Enqueue,

                                                  DateTimeOffset?                        Timestamp           = null,
                                                  EventTracking_Id?                      EventTrackingId     = null,
                                                  TimeSpan?                              RequestTimeout      = null,
                                                  User_Id?                               CurrentUserId       = null,
                                                  CancellationToken                      CancellationToken   = default)


                => Task.FromResult(
                       AddChargingStationOperatorsResult.NoOperation(
                           RejectedChargingStationOperators:  ChargingStationOperators,
                           SenderId:                          Id,
                           Sender:                            this,
                           EventTrackingId:                   EventTrackingId
                       )
                   );

        #endregion

        #region AddOrUpdateChargingStationOperators   (ChargingStationOperators, ...)

        /// <summary>
        /// Set the EVSE data of the given enumeration of charging station operators as new static EVSE data.
        /// </summary>
        /// <param name="ChargingStationOperators">An enumeration of charging station operators.</param>
        /// <param name="TransmissionType">Whether to send the charging pool update directly or enqueue it for a while.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        public virtual Task<AddOrUpdateChargingStationOperatorsResult>

            AddOrUpdateChargingStationOperators(IEnumerable<IChargingStationOperator>  ChargingStationOperators,
                                                TransmissionTypes                      TransmissionType    = TransmissionTypes.Enqueue,

                                                DateTimeOffset?                        Timestamp           = null,
                                                EventTracking_Id?                      EventTrackingId     = null,
                                                TimeSpan?                              RequestTimeout      = null,
                                                User_Id?                               CurrentUserId       = null,
                                                CancellationToken                      CancellationToken   = default)


                => Task.FromResult(
                       AddOrUpdateChargingStationOperatorsResult.NoOperation(
                           RejectedChargingStationOperators:  ChargingStationOperators,
                           SenderId:                          Id,
                           Sender:                            this,
                           EventTrackingId:                   EventTrackingId
                       )
                   );

        #endregion

        #region UpdateChargingStationOperators        (ChargingStationOperators, ...)

        /// <summary>
        /// Update the EVSE data of the given enumeration of charging station operators within the static EVSE data.
        /// </summary>
        /// <param name="ChargingStationOperators">An enumeration of charging station operators.</param>
        /// <param name="TransmissionType">Whether to send the charging pool update directly or enqueue it for a while.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        public virtual Task<UpdateChargingStationOperatorsResult>

            UpdateChargingStationOperators(IEnumerable<IChargingStationOperator>  ChargingStationOperators,
                                           TransmissionTypes                      TransmissionType    = TransmissionTypes.Enqueue,

                                           DateTimeOffset?                        Timestamp           = null,
                                           EventTracking_Id?                      EventTrackingId     = null,
                                           TimeSpan?                              RequestTimeout      = null,
                                           User_Id?                               CurrentUserId       = null,
                                           CancellationToken                      CancellationToken   = default)


                => Task.FromResult(
                       UpdateChargingStationOperatorsResult.NoOperation(
                           RejectedChargingStationOperators:  ChargingStationOperators,
                           SenderId:                          Id,
                           Sender:                            this,
                           EventTrackingId:                   EventTrackingId
                       )
                   );

        #endregion

        #region DeleteChargingStationOperators        (ChargingStationOperators, ...)

        /// <summary>
        /// Delete the EVSE data of the given enumeration of charging station operators from the static EVSE data.
        /// </summary>
        /// <param name="ChargingStationOperators">An enumeration of charging station operators.</param>
        /// <param name="TransmissionType">Whether to send the charging pool update directly or enqueue it for a while.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        public virtual Task<DeleteChargingStationOperatorsResult>

            DeleteChargingStationOperators(IEnumerable<IChargingStationOperator>  ChargingStationOperators,
                                           TransmissionTypes                      TransmissionType    = TransmissionTypes.Enqueue,

                                           DateTimeOffset?                        Timestamp           = null,
                                           EventTracking_Id?                      EventTrackingId     = null,
                                           TimeSpan?                              RequestTimeout      = null,
                                           User_Id?                               CurrentUserId       = null,
                                           CancellationToken                      CancellationToken   = default)


                => Task.FromResult(
                       DeleteChargingStationOperatorsResult.NoOperation(
                           RejectedChargingStationOperators:  ChargingStationOperators,
                           SenderId:                          Id,
                           Sender:                            this,
                           EventTrackingId:                   EventTrackingId
                       )
                   );

        #endregion


        #region UpdateChargingStationOperatorAdminStatus (ChargingStationOperatorAdminStatusUpdates, TransmissionType = Enqueue, ...)

        /// <summary>
        /// Update the given enumeration of charging station operator admin status updates.
        /// </summary>
        /// <param name="ChargingStationOperatorAdminStatusUpdates">An enumeration of charging station operator admin status updates.</param>
        /// <param name="TransmissionType">Whether to send the charging station operator admin status updates directly or enqueue it for a while.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        public virtual Task<PushChargingStationOperatorAdminStatusResult>

            UpdateChargingStationOperatorAdminStatus(IEnumerable<ChargingStationOperatorAdminStatusUpdate>  ChargingStationOperatorAdminStatusUpdates,
                                                     TransmissionTypes                                      TransmissionType    = TransmissionTypes.Enqueue,

                                                     DateTimeOffset?                                        Timestamp           = null,
                                                     EventTracking_Id?                                      EventTrackingId     = null,
                                                     TimeSpan?                                              RequestTimeout      = null,
                                                     User_Id?                                               CurrentUserId       = null,
                                                     CancellationToken                                      CancellationToken   = default)


                => Task.FromResult(
                       PushChargingStationOperatorAdminStatusResult.NoOperation(
                           Id,
                           this
                       )
                   );

        #endregion

        #region UpdateChargingStationOperatorStatus      (ChargingStationOperatorStatusUpdates,      TransmissionType = Enqueue, ...)

        /// <summary>
        /// Update the given enumeration of charging station operator status updates.
        /// </summary>
        /// <param name="ChargingStationOperatorStatusUpdates">An enumeration of charging station operator status updates.</param>
        /// <param name="TransmissionType">Whether to send the charging station operator status updates directly or enqueue it for a while.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        public virtual Task<PushChargingStationOperatorStatusResult>

            UpdateChargingStationOperatorStatus(IEnumerable<ChargingStationOperatorStatusUpdate>  ChargingStationOperatorStatusUpdates,
                                                TransmissionTypes                                 TransmissionType    = TransmissionTypes.Enqueue,

                                                DateTimeOffset?                                   Timestamp           = null,
                                                EventTracking_Id?                                 EventTrackingId     = null,
                                                TimeSpan?                                         RequestTimeout      = null,
                                                User_Id?                                          CurrentUserId       = null,
                                                CancellationToken                                 CancellationToken   = default)


                => Task.FromResult(
                       PushChargingStationOperatorStatusResult.NoOperation(
                           Id,
                           this
                       )
                   );

        #endregion

        #endregion

        #region (Set/Add/Update/Delete) Charging pool(s)...

        /// <summary>
        /// This service can be disabled, e.g. for debugging reasons.
        /// </summary>
        public Boolean                        DisableSendChargingPoolData    { get; set; }

        /// <summary>
        /// Only include charging pool identifications matching the given delegate.
        /// </summary>
        public IncludeChargingPoolIdDelegate  IncludeChargingPoolIds         { get; }

        /// <summary>
        /// Only include charging pools matching the given delegate.
        /// </summary>
        public IncludeChargingPoolDelegate    IncludeChargingPools           { get; }


        #region AddChargingPool            (ChargingPool,  TransmissionType = Enqueue, ...)

        /// <summary>
        /// Add the given charging pool.
        /// </summary>
        /// <param name="ChargingPool">A charging pool to add.</param>
        /// <param name="TransmissionType">Whether to send the charging pool update directly or enqueue it for a while.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        public virtual Task<AddChargingPoolResult>

            AddChargingPool(IChargingPool      ChargingPool,
                            TransmissionTypes  TransmissionType    = TransmissionTypes.Enqueue,

                            DateTimeOffset?    Timestamp           = null,
                            EventTracking_Id?  EventTrackingId     = null,
                            TimeSpan?          RequestTimeout      = null,
                            User_Id?           CurrentUserId       = null,
                            CancellationToken  CancellationToken   = default)


                => Task.FromResult(
                       AddChargingPoolResult.NoOperation(
                           ChargingPool:     ChargingPool,
                           EventTrackingId:  EventTrackingId,
                           SenderId:         Id,
                           Sender:           this
                       )
                   );

        #endregion

        #region AddChargingPoolIfNotExists (ChargingPool,  TransmissionType = Enqueue, ...)

        /// <summary>
        /// Add the given charging pool, if it does not already exist.
        /// </summary>
        /// <param name="ChargingPool">A charging pool to add, if it does not already exist.</param>
        /// <param name="TransmissionType">Whether to send the charging pool update directly or enqueue it for a while.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        public virtual Task<AddChargingPoolResult>

            AddChargingPoolIfNotExists(IChargingPool      ChargingPool,
                                       TransmissionTypes  TransmissionType    = TransmissionTypes.Enqueue,

                                       DateTimeOffset?    Timestamp           = null,
                                       EventTracking_Id?  EventTrackingId     = null,
                                       TimeSpan?          RequestTimeout      = null,
                                       User_Id?           CurrentUserId       = null,
                                       CancellationToken  CancellationToken   = default)


                => Task.FromResult(
                       AddChargingPoolResult.NoOperation(
                           ChargingPool:     ChargingPool,
                           EventTrackingId:  EventTrackingId,
                           SenderId:         Id,
                           Sender:           this
                       )
                   );

        #endregion

        #region AddOrUpdateChargingPool    (ChargingPool,  TransmissionType = Enqueue, ...)

        /// <summary>
        /// Add or update the given charging pool.
        /// </summary>
        /// <param name="ChargingPool">A charging pool to add or update.</param>
        /// <param name="TransmissionType">Whether to send the charging pool update directly or enqueue it for a while.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        public virtual Task<AddOrUpdateChargingPoolResult>

            AddOrUpdateChargingPool(IChargingPool      ChargingPool,
                                    TransmissionTypes  TransmissionType    = TransmissionTypes.Enqueue,

                                    DateTimeOffset?    Timestamp           = null,
                                    EventTracking_Id?  EventTrackingId     = null,
                                    TimeSpan?          RequestTimeout      = null,
                                    User_Id?           CurrentUserId       = null,
                                    CancellationToken  CancellationToken   = default)


                => Task.FromResult(
                       AddOrUpdateChargingPoolResult.NoOperation(
                           ChargingPool:     ChargingPool,
                           EventTrackingId:  EventTrackingId,
                           SenderId:         Id,
                           Sender:           this
                       )
                   );

        #endregion

        #region UpdateChargingPool         (ChargingPool,  PropertyName = null, NewValue = null, OldValue = null, DataSource = null, TransmissionType = Enqueue, ...)

        /// <summary>
        /// Update the given charging pool.
        /// </summary>
        /// <param name="ChargingPool">A charging pool to update.</param>
        /// <param name="PropertyName">The name of the charging pool property to update, if any specific.</param>
        /// <param name="NewValue">The new value of the charging pool property to update.</param>
        /// <param name="OldValue">The optional old value of the charging pool property to update.</param>
        /// <param name="DataSource">An optional data source or context for the charging pool property update.</param>
        /// <param name="TransmissionType">Whether to send the charging pool update directly or enqueue it for a while.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public virtual Task<UpdateChargingPoolResult>

            UpdateChargingPool(IChargingPool      ChargingPool,
                               String?            PropertyName        = null,
                               Object?            NewValue            = null,
                               Object?            OldValue            = null,
                               Context?           DataSource          = null,
                               TransmissionTypes  TransmissionType    = TransmissionTypes.Enqueue,

                               DateTimeOffset?    Timestamp           = null,
                               EventTracking_Id?  EventTrackingId     = null,
                               TimeSpan?          RequestTimeout      = null,
                               User_Id?           CurrentUserId       = null,
                               CancellationToken  CancellationToken   = default)


                => Task.FromResult(
                       UpdateChargingPoolResult.NoOperation(
                           ChargingPool:     ChargingPool,
                           EventTrackingId:  EventTrackingId,
                           SenderId:         Id,
                           Sender:           this
                       )
                   );

        #endregion

        #region DeleteChargingPool         (ChargingPool,  TransmissionType = Enqueue, ...)

        /// <summary>
        /// Delete the given charging pool.
        /// </summary>
        /// <param name="ChargingPool">A charging pool to delete.</param>
        /// <param name="TransmissionType">Whether to send the charging pool update directly or enqueue it for a while.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        public virtual Task<DeleteChargingPoolResult>

            DeleteChargingPool(IChargingPool      ChargingPool,
                               TransmissionTypes  TransmissionType    = TransmissionTypes.Enqueue,

                               DateTimeOffset?    Timestamp           = null,
                               EventTracking_Id?  EventTrackingId     = null,
                               TimeSpan?          RequestTimeout      = null,
                               User_Id?           CurrentUserId       = null,
                               CancellationToken  CancellationToken   = default)


                => Task.FromResult(
                       DeleteChargingPoolResult.NoOperation(
                           ChargingPool:     ChargingPool,
                           EventTrackingId:  EventTrackingId,
                           SenderId:         Id,
                           Sender:           this
                       )
                   );

        #endregion


        #region AddChargingPools           (ChargingPools, TransmissionType = Enqueue, ...)

        /// <summary>
        /// Add the given enumeration of charging pools.
        /// </summary>
        /// <param name="ChargingPools">An enumeration of charging pools to add.</param>
        /// <param name="TransmissionType">Whether to send the charging pool update directly or enqueue it for a while.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        public virtual Task<AddChargingPoolsResult>

            AddChargingPools(IEnumerable<IChargingPool>  ChargingPools,
                             TransmissionTypes           TransmissionType    = TransmissionTypes.Enqueue,

                             DateTimeOffset?             Timestamp           = null,
                             EventTracking_Id?           EventTrackingId     = null,
                             TimeSpan?                   RequestTimeout      = null,
                             User_Id?                    CurrentUserId       = null,
                             CancellationToken           CancellationToken   = default)


                => Task.FromResult(
                       AddChargingPoolsResult.NoOperation(
                           RejectedChargingPools:  ChargingPools,
                           SenderId:               Id,
                           Sender:            this,
                           EventTrackingId:        EventTrackingId
                       )
                   );

        #endregion

        #region AddChargingPoolsIfNotExist (ChargingPools, TransmissionType = Enqueue, ...)

        /// <summary>
        /// Add the given enumeration of charging pools, if they do not already exist.
        /// </summary>
        /// <param name="ChargingPools">An enumeration of charging pools to add, if they do not already exist.</param>
        /// <param name="TransmissionType">Whether to send the charging pool update directly or enqueue it for a while.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        public virtual Task<AddChargingPoolsResult>

            AddChargingPoolsIfNotExist(IEnumerable<IChargingPool>  ChargingPools,
                                       TransmissionTypes           TransmissionType    = TransmissionTypes.Enqueue,

                                       DateTimeOffset?             Timestamp           = null,
                                       EventTracking_Id?           EventTrackingId     = null,
                                       TimeSpan?                   RequestTimeout      = null,
                                       User_Id?                    CurrentUserId       = null,
                                       CancellationToken           CancellationToken   = default)


                => Task.FromResult(
                       AddChargingPoolsResult.NoOperation(
                           RejectedChargingPools:  ChargingPools,
                           SenderId:               Id,
                           Sender:            this,
                           EventTrackingId:        EventTrackingId
                       )
                   );

        #endregion

        #region AddOrUpdateChargingPools   (ChargingPools, TransmissionType = Enqueue, ...)

        /// <summary>
        /// Add or update the given enumeration of charging pools.
        /// </summary>
        /// <param name="ChargingPools">An enumeration of charging pools to add or update.</param>
        /// <param name="TransmissionType">Whether to send the charging pool update directly or enqueue it for a while.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        public virtual Task<AddOrUpdateChargingPoolsResult>

            AddOrUpdateChargingPools(IEnumerable<IChargingPool>  ChargingPools,
                                     TransmissionTypes           TransmissionType    = TransmissionTypes.Enqueue,

                                     DateTimeOffset?             Timestamp           = null,
                                     EventTracking_Id?           EventTrackingId     = null,
                                     TimeSpan?                   RequestTimeout      = null,
                                     User_Id?                    CurrentUserId       = null,
                                     CancellationToken           CancellationToken   = default)


                => Task.FromResult(
                       AddOrUpdateChargingPoolsResult.NoOperation(
                           RejectedChargingPools:  ChargingPools,
                           SenderId:               Id,
                           Sender:            this,
                           EventTrackingId:        EventTrackingId
                       )
                   );

        #endregion

        #region UpdateChargingPools        (ChargingPools, TransmissionType = Enqueue, ...)

        /// <summary>
        /// Update the given enumeration of charging pools.
        /// </summary>
        /// <param name="ChargingPools">An enumeration of charging pools to update.</param>
        /// <param name="TransmissionType">Whether to send the charging pool update directly or enqueue it for a while.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        public virtual Task<UpdateChargingPoolsResult>

            UpdateChargingPools(IEnumerable<IChargingPool>  ChargingPools,
                                TransmissionTypes           TransmissionType    = TransmissionTypes.Enqueue,

                                DateTimeOffset?             Timestamp           = null,
                                EventTracking_Id?           EventTrackingId     = null,
                                TimeSpan?                   RequestTimeout      = null,
                                User_Id?                    CurrentUserId       = null,
                                CancellationToken           CancellationToken   = default)


                => Task.FromResult(
                       UpdateChargingPoolsResult.NoOperation(
                           RejectedChargingPools:  ChargingPools,
                           SenderId:               Id,
                           Sender:            this,
                           EventTrackingId:        EventTrackingId
                       )
                   );

        #endregion

        #region ReplaceChargingPools       (ChargingPools, TransmissionType = Enqueue, ...)

        /// <summary>
        /// Replace the given enumeration of charging pools.
        /// Charging pools not included will be deleted.
        /// </summary>
        /// <param name="ChargingPools">An enumeration of charging stations to replace.</param>
        /// <param name="TransmissionType">Whether to send the charging pool update directly or enqueue it for a while.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        public virtual Task<ReplaceChargingPoolsResult>

            ReplaceChargingPools(IEnumerable<IChargingPool>  ChargingPools,
                                 TransmissionTypes           TransmissionType    = TransmissionTypes.Enqueue,

                                 DateTimeOffset?             Timestamp           = null,
                                 EventTracking_Id?           EventTrackingId     = null,
                                 TimeSpan?                   RequestTimeout      = null,
                                 User_Id?                    CurrentUserId       = null,
                                 CancellationToken           CancellationToken   = default)


                => Task.FromResult(
                       ReplaceChargingPoolsResult.NoOperation(
                           RejectedChargingPools:  ChargingPools,
                           SenderId:               Id,
                           Sender:            this,
                           EventTrackingId:        EventTrackingId
                       )
                   );

        #endregion

        #region DeleteChargingPools        (ChargingPools, TransmissionType = Enqueue, ...)

        /// <summary>
        /// DDelete the given enumeration of charging pools.
        /// </summary>
        /// <param name="ChargingPools">An enumeration of charging pools to delete.</param>
        /// <param name="TransmissionType">Whether to send the charging pool update directly or enqueue it for a while.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        public virtual Task<DeleteChargingPoolsResult>

            DeleteChargingPools(IEnumerable<IChargingPool>  ChargingPools,
                                TransmissionTypes           TransmissionType    = TransmissionTypes.Enqueue,

                                DateTimeOffset?             Timestamp           = null,
                                EventTracking_Id?           EventTrackingId     = null,
                                TimeSpan?                   RequestTimeout      = null,
                                User_Id?                    CurrentUserId       = null,
                                CancellationToken           CancellationToken   = default)


                => Task.FromResult(
                       DeleteChargingPoolsResult.NoOperation(
                           RejectedChargingPools:  ChargingPools,
                           SenderId:               Id,
                           Sender:            this,
                           EventTrackingId:        EventTrackingId
                       )
                   );

        #endregion


        #region UpdateChargingPoolAdminStatus  (ChargingPoolAdminStatusUpdates,  TransmissionType = Enqueue, ...)

        /// <summary>
        /// Update the given enumeration of charging pool admin status updates.
        /// </summary>
        /// <param name="ChargingPoolAdminStatusUpdates">An enumeration of charging pool admin status updates.</param>
        /// <param name="TransmissionType">Whether to send the charging pool admin status updates directly or enqueue it for a while.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        public virtual Task<PushChargingPoolAdminStatusResult>

            UpdateChargingPoolAdminStatus(IEnumerable<ChargingPoolAdminStatusUpdate>  ChargingPoolAdminStatusUpdates,
                                          TransmissionTypes                           TransmissionType    = TransmissionTypes.Enqueue,

                                          DateTimeOffset?                             Timestamp           = null,
                                          EventTracking_Id?                           EventTrackingId     = null,
                                          TimeSpan?                                   RequestTimeout      = null,
                                          User_Id?                                    CurrentUserId       = null,
                                          CancellationToken                           CancellationToken   = default)


                => Task.FromResult(
                       PushChargingPoolAdminStatusResult.NoOperation(
                           Id,
                           this
                       )
                   );

        #endregion

        #region UpdateChargingPoolStatus       (ChargingPoolStatusUpdates,       TransmissionType = Enqueue, ...)

        /// <summary>
        /// Update the given enumeration of charging pool status updates.
        /// </summary>
        /// <param name="ChargingPoolStatusUpdates">An enumeration of charging pool status updates.</param>
        /// <param name="TransmissionType">Whether to send the charging pool status updates directly or enqueue it for a while.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        public virtual Task<PushChargingPoolStatusResult>

            UpdateChargingPoolStatus(IEnumerable<ChargingPoolStatusUpdate>  ChargingPoolStatusUpdates,
                                     TransmissionTypes                      TransmissionType    = TransmissionTypes.Enqueue,

                                     DateTimeOffset?                        Timestamp           = null,
                                     EventTracking_Id?                      EventTrackingId     = null,
                                     TimeSpan?                              RequestTimeout      = null,
                                     User_Id?                               CurrentUserId       = null,
                                     CancellationToken                      CancellationToken   = default)


                => Task.FromResult(
                       PushChargingPoolStatusResult.NoOperation(
                           Id,
                           this
                       )
                   );

        #endregion

        #region UpdateChargingPoolEnergyStatus (ChargingPoolEnergyStatusUpdates, TransmissionType = Enqueue, ...)

        /// <summary>
        /// Update the given enumeration of charging pool energy status updates.
        /// </summary>
        /// <param name="ChargingPoolEnergyStatusUpdates">An enumeration of charging pool energy status updates.</param>
        /// <param name="TransmissionType">Whether to send the charging pool energy status updates directly or enqueue it for a while.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        public virtual Task<PushChargingPoolEnergyStatusResult>

            UpdateChargingPoolEnergyStatus(IEnumerable<ChargingPoolEnergyStatusUpdate>  ChargingPoolEnergyStatusUpdates,
                                           TransmissionTypes                            TransmissionType    = TransmissionTypes.Enqueue,

                                           DateTimeOffset?                              Timestamp           = null,
                                           EventTracking_Id?                            EventTrackingId     = null,
                                           TimeSpan?                                    RequestTimeout      = null,
                                           User_Id?                                     CurrentUserId       = null,
                                           CancellationToken                            CancellationToken   = default)


                => Task.FromResult(
                       PushChargingPoolEnergyStatusResult.NoOperation(
                           Id,
                           this
                       )
                   );

        #endregion

        #endregion

        #region (Set/Add/Update/Delete) Charging station(s)...

        /// <summary>
        /// This service can be disabled, e.g. for debugging reasons.
        /// </summary>
        public Boolean                           DisableSendChargingStationData    { get; set; }

        /// <summary>
        /// Only include charging station identifications matching the given delegate.
        /// </summary>
        public IncludeChargingStationIdDelegate  IncludeChargingStationIds         { get; }

        /// <summary>
        /// Only include charging stations matching the given delegate.
        /// </summary>
        public IncludeChargingStationDelegate    IncludeChargingStations           { get; }


        #region AddChargingStation            (ChargingStation,  TransmissionType = Enqueue, ...)

        /// <summary>
        /// Add the given charging station.
        /// </summary>
        /// <param name="ChargingStation">A charging station to add.</param>
        /// <param name="TransmissionType">Whether to send the charging pool update directly or enqueue it for a while.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        public virtual Task<AddChargingStationResult>

            AddChargingStation(IChargingStation   ChargingStation,
                               TransmissionTypes  TransmissionType    = TransmissionTypes.Enqueue,

                               DateTimeOffset?    Timestamp           = null,
                               EventTracking_Id?  EventTrackingId     = null,
                               TimeSpan?          RequestTimeout      = null,
                               User_Id?           CurrentUserId       = null,
                               CancellationToken  CancellationToken   = default)


                => Task.FromResult(
                       AddChargingStationResult.NoOperation(
                           ChargingStation:  ChargingStation,
                           EventTrackingId:  EventTrackingId,
                           SenderId:         Id,
                           Sender:           this
                       )
                   );

        #endregion

        #region AddChargingStationIfNotExists (ChargingStation,  TransmissionType = Enqueue, ...)

        /// <summary>
        /// Add the given charging station, if it does not already exist.
        /// </summary>
        /// <param name="ChargingStation">A charging station to add, if it does not already exist.</param>
        /// <param name="TransmissionType">Whether to send the charging pool update directly or enqueue it for a while.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        public virtual Task<AddChargingStationResult>

            AddChargingStationIfNotExists(IChargingStation   ChargingStation,
                                          TransmissionTypes  TransmissionType    = TransmissionTypes.Enqueue,

                                          DateTimeOffset?    Timestamp           = null,
                                          EventTracking_Id?  EventTrackingId     = null,
                                          TimeSpan?          RequestTimeout      = null,
                                          User_Id?           CurrentUserId       = null,
                                          CancellationToken  CancellationToken   = default)


                => Task.FromResult(
                       AddChargingStationResult.NoOperation(
                           ChargingStation:  ChargingStation,
                           EventTrackingId:  EventTrackingId,
                           SenderId:         Id,
                           Sender:           this
                       )
                   );

        #endregion

        #region AddOrUpdateChargingStation    (ChargingStation,  TransmissionType = Enqueue, ...)

        /// <summary>
        /// Add or update the given charging station.
        /// </summary>
        /// <param name="ChargingStation">A charging station to add or update.</param>
        /// <param name="TransmissionType">Whether to send the charging pool update directly or enqueue it for a while.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        public virtual Task<AddOrUpdateChargingStationResult>

            AddOrUpdateChargingStation(IChargingStation   ChargingStation,
                                       TransmissionTypes  TransmissionType    = TransmissionTypes.Enqueue,

                                       DateTimeOffset?    Timestamp           = null,
                                       EventTracking_Id?  EventTrackingId     = null,
                                       TimeSpan?          RequestTimeout      = null,
                                       User_Id?           CurrentUserId       = null,
                                       CancellationToken  CancellationToken   = default)


                => Task.FromResult(
                       AddOrUpdateChargingStationResult.NoOperation(
                           ChargingStation:  ChargingStation,
                           EventTrackingId:  EventTrackingId,
                           SenderId:         Id,
                           Sender:           this
                       )
                   );

        #endregion

        #region UpdateChargingStation         (ChargingStation,  PropertyName = null, NewValue = null, OldValue = null, DataSource = null, TransmissionType = Enqueue, ...)

        /// <summary>
        /// Update the given charging station.
        /// </summary>
        /// <param name="ChargingStation">A charging station to update.</param>
        /// <param name="PropertyName">The name of the charging station property to update, if any specific.</param>
        /// <param name="NewValue">The new value of the charging station property to update.</param>
        /// <param name="OldValue">The optional old value of the charging station property to update.</param>
        /// <param name="DataSource">An optional data source or context for the charging station property update.</param>
        /// <param name="TransmissionType">Whether to send the charging station update directly or enqueue it for a while.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        public virtual Task<UpdateChargingStationResult>

            UpdateChargingStation(IChargingStation   ChargingStation,
                                  String?            PropertyName        = null,
                                  Object?            NewValue            = null,
                                  Object?            OldValue            = null,
                                  Context?           DataSource          = null,
                                  TransmissionTypes  TransmissionType    = TransmissionTypes.Enqueue,

                                  DateTimeOffset?    Timestamp           = null,
                                  EventTracking_Id?  EventTrackingId     = null,
                                  TimeSpan?          RequestTimeout      = null,
                                  User_Id?           CurrentUserId       = null,
                                  CancellationToken  CancellationToken   = default)


                => Task.FromResult(
                       UpdateChargingStationResult.NoOperation(
                           ChargingStation:  ChargingStation,
                           EventTrackingId:  EventTrackingId,
                           SenderId:         Id,
                           Sender:           this
                       )
                   );

        #endregion

        #region DeleteChargingStation         (ChargingStation,  TransmissionType = Enqueue, ...)

        /// <summary>
        /// Delete the given charging station.
        /// </summary>
        /// <param name="ChargingStation">A charging station to delete.</param>
        /// <param name="TransmissionType">Whether to send the charging station update directly or enqueue it for a while.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public virtual Task<DeleteChargingStationResult>

            DeleteChargingStation(IChargingStation   ChargingStation,
                                  TransmissionTypes  TransmissionType    = TransmissionTypes.Enqueue,

                                  DateTimeOffset?    Timestamp           = null,
                                  EventTracking_Id?  EventTrackingId     = null,
                                  TimeSpan?          RequestTimeout      = null,
                                  User_Id?           CurrentUserId       = null,
                                  CancellationToken  CancellationToken   = default)


                => Task.FromResult(
                       DeleteChargingStationResult.NoOperation(
                           ChargingStation:  ChargingStation,
                           EventTrackingId:  EventTrackingId,
                           SenderId:         Id,
                           Sender:           this
                       )
                   );

        #endregion


        #region AddChargingStations           (ChargingStations, TransmissionType = Enqueue, ...)

        /// <summary>
        /// Add the given enumeration of charging stations.
        /// </summary>
        /// <param name="ChargingStations">An enumeration of charging stations to add.</param>
        /// <param name="TransmissionType">Whether to send the charging station update directly or enqueue it for a while.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public virtual Task<AddChargingStationsResult>

            AddChargingStations(IEnumerable<IChargingStation>  ChargingStations,
                                TransmissionTypes              TransmissionType    = TransmissionTypes.Enqueue,

                                DateTimeOffset?                Timestamp           = null,
                                EventTracking_Id?              EventTrackingId     = null,
                                TimeSpan?                      RequestTimeout      = null,
                                User_Id?                       CurrentUserId       = null,
                                CancellationToken              CancellationToken   = default)


                => Task.FromResult(
                       AddChargingStationsResult.NoOperation(
                           RejectedChargingStations:  ChargingStations,
                           SenderId:                  Id,
                           Sender:                    this,
                           EventTrackingId:           EventTrackingId
                       )
                   );

        #endregion

        #region AddChargingStationsIfNotExist (ChargingStations, TransmissionType = Enqueue, ...)

        /// <summary>
        /// Add the given enumeration of charging stations, if they do not already exist.
        /// </summary>
        /// <param name="ChargingStations">An enumeration of charging stations to add, if they do not already exist.</param>
        /// <param name="TransmissionType">Whether to send the charging station update directly or enqueue it for a while.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public virtual Task<AddChargingStationsResult>

            AddChargingStationsIfNotExist(IEnumerable<IChargingStation>  ChargingStations,
                                          TransmissionTypes              TransmissionType    = TransmissionTypes.Enqueue,

                                          DateTimeOffset?                Timestamp           = null,
                                          EventTracking_Id?              EventTrackingId     = null,
                                          TimeSpan?                      RequestTimeout      = null,
                                          User_Id?                       CurrentUserId       = null,
                                          CancellationToken              CancellationToken   = default)


                => Task.FromResult(
                       AddChargingStationsResult.NoOperation(
                           RejectedChargingStations:  ChargingStations,
                           SenderId:                  Id,
                           Sender:                    this,
                           EventTrackingId:           EventTrackingId
                       )
                   );

        #endregion

        #region AddOrUpdateChargingStations   (ChargingStations, TransmissionType = Enqueue, ...)

        /// <summary>
        /// Add or update the given enumeration of charging stations.
        /// </summary>
        /// <param name="ChargingStations">An enumeration of charging stations to add or update.</param>
        /// <param name="TransmissionType">Whether to send the charging station update directly or enqueue it for a while.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public virtual Task<AddOrUpdateChargingStationsResult>

            AddOrUpdateChargingStations(IEnumerable<IChargingStation>  ChargingStations,
                                        TransmissionTypes              TransmissionType    = TransmissionTypes.Enqueue,

                                        DateTimeOffset?                Timestamp           = null,
                                        EventTracking_Id?              EventTrackingId     = null,
                                        TimeSpan?                      RequestTimeout      = null,
                                        User_Id?                       CurrentUserId       = null,
                                        CancellationToken              CancellationToken   = default)


                => Task.FromResult(
                       AddOrUpdateChargingStationsResult.NoOperation(
                           RejectedChargingStations:  ChargingStations,
                           SenderId:                  Id,
                           Sender:                    this,
                           EventTrackingId:           EventTrackingId
                       )
                   );

        #endregion

        #region UpdateChargingStations        (ChargingStations, TransmissionType = Enqueue, ...)

        /// <summary>
        /// Update the given enumeration of charging stations.
        /// </summary>
        /// <param name="ChargingStations">An enumeration of charging stations to update.</param>
        /// <param name="TransmissionType">Whether to send the charging station update directly or enqueue it for a while.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        public virtual Task<UpdateChargingStationsResult>

            UpdateChargingStations(IEnumerable<IChargingStation>  ChargingStations,
                                   TransmissionTypes              TransmissionType    = TransmissionTypes.Enqueue,

                                   DateTimeOffset?                Timestamp           = null,
                                   EventTracking_Id?              EventTrackingId     = null,
                                   TimeSpan?                      RequestTimeout      = null,
                                   User_Id?                       CurrentUserId       = null,
                                   CancellationToken              CancellationToken   = default)


                => Task.FromResult(
                       UpdateChargingStationsResult.NoOperation(
                           RejectedChargingStations:  ChargingStations,
                           SenderId:                  Id,
                           Sender:                    this,
                           EventTrackingId:           EventTrackingId
                       )
                   );

        #endregion

        #region ReplaceChargingStations       (ChargingStations, TransmissionType = Enqueue, ...)

        /// <summary>
        /// Replace the given enumeration of charging stations.
        /// Charging stations not included will be deleted.
        /// </summary>
        /// <param name="ChargingStations">An enumeration of charging stations to replace.</param>
        /// <param name="TransmissionType">Whether to send the charging station update directly or enqueue it for a while.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public virtual Task<ReplaceChargingStationsResult>

            ReplaceChargingStations(IEnumerable<IChargingStation>  ChargingStations,
                                    TransmissionTypes              TransmissionType    = TransmissionTypes.Enqueue,

                                    DateTimeOffset?                Timestamp           = null,
                                    EventTracking_Id?              EventTrackingId     = null,
                                    TimeSpan?                      RequestTimeout      = null,
                                    User_Id?                       CurrentUserId       = null,
                                    CancellationToken              CancellationToken   = default)


                => Task.FromResult(
                       ReplaceChargingStationsResult.NoOperation(
                           RejectedChargingStations:  ChargingStations,
                           SenderId:                  Id,
                           Sender:                    this,
                           EventTrackingId:           EventTrackingId
                       )
                   );

        #endregion

        #region DeleteChargingStations        (ChargingStations, TransmissionType = Enqueue, ...)

        /// <summary>
        /// Delete the given enumeration of charging stations.
        /// </summary>
        /// <param name="ChargingStations">An enumeration of charging stations to delete.</param>
        /// <param name="TransmissionType">Whether to send the charging station update directly or enqueue it for a while.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        public virtual Task<DeleteChargingStationsResult>

            DeleteChargingStations(IEnumerable<IChargingStation>  ChargingStations,
                                   TransmissionTypes              TransmissionType    = TransmissionTypes.Enqueue,

                                   DateTimeOffset?                Timestamp           = null,
                                   EventTracking_Id?              EventTrackingId     = null,
                                   TimeSpan?                      RequestTimeout      = null,
                                   User_Id?                       CurrentUserId       = null,
                                   CancellationToken              CancellationToken   = default)


                => Task.FromResult(
                       DeleteChargingStationsResult.NoOperation(
                           RejectedChargingStations:  ChargingStations,
                           SenderId:                  Id,
                           Sender:                    this,
                           EventTrackingId:           EventTrackingId
                       )
                   );

        #endregion


        #region UpdateChargingStationAdminStatus  (ChargingStationAdminStatusUpdates,  TransmissionType = Enqueue, ...)

        /// <summary>
        /// Update the given enumeration of charging station admin status updates.
        /// </summary>
        /// <param name="ChargingStationAdminStatusUpdates">An enumeration of charging station admin status updates.</param>
        /// <param name="TransmissionType">Whether to send the charging station admin status updates directly or enqueue it for a while.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        public virtual Task<PushChargingStationAdminStatusResult>

            UpdateChargingStationAdminStatus(IEnumerable<ChargingStationAdminStatusUpdate>  ChargingStationAdminStatusUpdates,
                                             TransmissionTypes                              TransmissionType    = TransmissionTypes.Enqueue,

                                             DateTimeOffset?                                Timestamp           = null,
                                             EventTracking_Id?                              EventTrackingId     = null,
                                             TimeSpan?                                      RequestTimeout      = null,
                                             User_Id?                                       CurrentUserId       = null,
                                             CancellationToken                              CancellationToken   = default)


                => Task.FromResult(
                       PushChargingStationAdminStatusResult.NoOperation(
                           Id,
                           this
                       )
                   );

        #endregion

        #region UpdateChargingStationStatus       (ChargingStationStatusUpdates,       TransmissionType = Enqueue, ...)

        /// <summary>
        /// Update the given enumeration of charging station status updates.
        /// </summary>
        /// <param name="ChargingStationStatusUpdates">An enumeration of charging station status updates.</param>
        /// <param name="TransmissionType">Whether to send the charging station status updates directly or enqueue it for a while.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        public virtual Task<PushChargingStationStatusResult>

            UpdateChargingStationStatus(IEnumerable<ChargingStationStatusUpdate>  ChargingStationStatusUpdates,
                                        TransmissionTypes                         TransmissionType    = TransmissionTypes.Enqueue,

                                        DateTimeOffset?                           Timestamp           = null,
                                        EventTracking_Id?                         EventTrackingId     = null,
                                        TimeSpan?                                 RequestTimeout      = null,
                                        User_Id?                                  CurrentUserId       = null,
                                        CancellationToken                         CancellationToken   = default)


                => Task.FromResult(
                       PushChargingStationStatusResult.NoOperation(
                           Id,
                           this
                       )
                   );

        #endregion

        #region UpdateChargingStationEnergyStatus (ChargingStationEnergyStatusUpdates, TransmissionType = Enqueue, ...)

        /// <summary>
        /// Update the given enumeration of charging station energy status updates.
        /// </summary>
        /// <param name="ChargingStationEnergyStatusUpdates">An enumeration of charging station energy status updates.</param>
        /// <param name="TransmissionType">Whether to send the charging station energy status updates directly or enqueue it for a while.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        public virtual Task<PushChargingStationEnergyStatusResult>

            UpdateChargingStationEnergyStatus(IEnumerable<ChargingStationEnergyStatusUpdate>  ChargingStationEnergyStatusUpdates,
                                              TransmissionTypes                               TransmissionType    = TransmissionTypes.Enqueue,

                                              DateTimeOffset?                                 Timestamp           = null,
                                              EventTracking_Id?                               EventTrackingId     = null,
                                              TimeSpan?                                       RequestTimeout      = null,
                                              User_Id?                                        CurrentUserId       = null,
                                              CancellationToken                               CancellationToken   = default)


                => Task.FromResult(
                       PushChargingStationEnergyStatusResult.NoOperation(
                           Id,
                           this
                       )
                   );

        #endregion

        #endregion

        #region (Set/Add/Update/Delete) EVSE(s)...

        /// <summary>
        /// This service can be disabled, e.g. for debugging reasons.
        /// </summary>
        public Boolean                DisableSendEVSEData    { get; set; }

        /// <summary>
        /// Only include EVSE identifications matching the given delegate.
        /// </summary>
        public IncludeEVSEIdDelegate  IncludeEVSEIds         { get; }

        /// <summary>
        /// Only include EVSEs matching the given delegate.
        /// </summary>
        public IncludeEVSEDelegate    IncludeEVSEs           { get; }


        #region AddEVSE            (EVSE,  TransmissionType = Enqueue, ...)

        /// <summary>
        /// Add the given EVSE.
        /// </summary>
        /// <param name="EVSE">An EVSE to add.</param>
        /// <param name="TransmissionType">Whether to send the EVSE directly or enqueue it for a while.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        public virtual Task<AddEVSEResult>

            AddEVSE(IEVSE              EVSE,
                    TransmissionTypes  TransmissionType    = TransmissionTypes.Enqueue,

                    DateTimeOffset?    Timestamp           = null,
                    EventTracking_Id?  EventTrackingId     = null,
                    TimeSpan?          RequestTimeout      = null,
                    User_Id?           CurrentUserId       = null,
                    CancellationToken  CancellationToken   = default)


                => Task.FromResult(
                       AddEVSEResult.NoOperation(
                           EVSE:             EVSE,
                           EventTrackingId:  EventTrackingId,
                           SenderId:         Id,
                           Sender:           this
                       )
                   );

        #endregion

        #region AddEVSEIfNotExists (EVSE,  TransmissionType = Enqueue, ...)

        /// <summary>
        /// Add the given EVSE, if it does not already exist.
        /// </summary>
        /// <param name="EVSE">An EVSE to add.</param>
        /// <param name="TransmissionType">Whether to send the EVSE directly or enqueue it for a while.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        public virtual Task<AddEVSEResult>

            AddEVSEIfNotExists(IEVSE              EVSE,
                               TransmissionTypes  TransmissionType    = TransmissionTypes.Enqueue,

                               DateTimeOffset?    Timestamp           = null,
                               EventTracking_Id?  EventTrackingId     = null,
                               TimeSpan?          RequestTimeout      = null,
                               User_Id?           CurrentUserId       = null,
                               CancellationToken  CancellationToken   = default)


                => Task.FromResult(
                       AddEVSEResult.NoOperation(
                           EVSE:             EVSE,
                           EventTrackingId:  EventTrackingId,
                           SenderId:         Id,
                           Sender:           this
                       )
                   );

        #endregion

        #region AddOrUpdateEVSE    (EVSE,  TransmissionType = Enqueue, ...)

        /// <summary>
        /// Add or update the given EVSE.
        /// </summary>
        /// <param name="EVSE">An EVSE to add or update.</param>
        /// <param name="TransmissionType">Whether to send the EVSE directly or enqueue it for a while.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        public virtual Task<AddOrUpdateEVSEResult>

            AddOrUpdateEVSE(IEVSE              EVSE,
                            TransmissionTypes  TransmissionType    = TransmissionTypes.Enqueue,

                            DateTimeOffset?    Timestamp           = null,
                            EventTracking_Id?  EventTrackingId     = null,
                            TimeSpan?          RequestTimeout      = null,
                            User_Id?           CurrentUserId       = null,
                            CancellationToken  CancellationToken   = default)


                => Task.FromResult(
                       AddOrUpdateEVSEResult.NoOperation(
                           EVSE:             EVSE,
                           EventTrackingId:  EventTrackingId,
                           SenderId:         Id,
                           Sender:           this
                       )
                   );

        #endregion

        #region UpdateEVSE         (EVSE,  PropertyName = null, NewValue = null, OldValue = null, DataSource = null, TransmissionType = Enqueue, ...)

        /// <summary>
        /// Update the given EVSE.
        /// </summary>
        /// <param name="EVSE">An EVSE to update.</param>
        /// <param name="PropertyName">The name of the EVSE property to update, if any specific.</param>
        /// <param name="NewValue">The new value of the EVSE property to update.</param>
        /// <param name="OldValue">The optional old value of the EVSE property to update.</param>
        /// <param name="DataSource">An optional data source or context for the EVSE property update.</param>
        /// <param name="TransmissionType">Whether to send the EVSE update directly or enqueue it for a while.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        public virtual Task<UpdateEVSEResult>

            UpdateEVSE(IEVSE              EVSE,
                       String?            PropertyName        = null,
                       Object?            NewValue            = null,
                       Object?            OldValue            = null,
                       Context?           DataSource          = null,
                       TransmissionTypes  TransmissionType    = TransmissionTypes.Enqueue,

                       DateTimeOffset?    Timestamp           = null,
                       EventTracking_Id?  EventTrackingId     = null,
                       TimeSpan?          RequestTimeout      = null,
                       User_Id?           CurrentUserId       = null,
                       CancellationToken  CancellationToken   = default)


                => Task.FromResult(
                       UpdateEVSEResult.NoOperation(
                           EVSE:             EVSE,
                           EventTrackingId:  EventTrackingId,
                           SenderId:         Id,
                           Sender:           this
                       )
                   );

        #endregion

        #region DeleteEVSE         (EVSE,  TransmissionType = Enqueue, ...)

        /// <summary>
        /// Delete the given EVSE.
        /// </summary>
        /// <param name="EVSE">An EVSE to delete.</param>
        /// <param name="TransmissionType">Whether to send the EVSE deletion directly or enqueue it for a while.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        public virtual Task<DeleteEVSEResult>

            DeleteEVSE(IEVSE              EVSE,
                       TransmissionTypes  TransmissionType    = TransmissionTypes.Enqueue,

                       DateTimeOffset?    Timestamp           = null,
                       EventTracking_Id?  EventTrackingId     = null,
                       TimeSpan?          RequestTimeout      = null,
                       User_Id?           CurrentUserId       = null,
                       CancellationToken  CancellationToken   = default)


                => Task.FromResult(
                       DeleteEVSEResult.NoOperation(
                           EVSE:             EVSE,
                           EventTrackingId:  EventTrackingId,
                           SenderId:         Id,
                           Sender:           this
                       )
                   );

        #endregion


        #region AddEVSEs           (EVSEs, TransmissionType = Enqueue, ...)

        /// <summary>
        /// Add the given enumeration of EVSEs.
        /// </summary>
        /// <param name="EVSEs">An enumeration of EVSEs to add.</param>
        /// <param name="TransmissionType">Whether to send the EVSE directly or enqueue it for a while.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        public virtual Task<AddEVSEsResult>

            AddEVSEs(IEnumerable<IEVSE>  EVSEs,
                     TransmissionTypes   TransmissionType    = TransmissionTypes.Enqueue,

                     DateTimeOffset?     Timestamp           = null,
                     EventTracking_Id?   EventTrackingId     = null,
                     TimeSpan?           RequestTimeout      = null,
                     User_Id?            CurrentUserId       = null,
                     CancellationToken   CancellationToken   = default)


                => Task.FromResult(
                       AddEVSEsResult.NoOperation(
                           RejectedEVSEs:    EVSEs,
                           SenderId:         Id,
                           Sender:           this,
                           EventTrackingId:  EventTrackingId
                       )
                   );

        #endregion

        #region AddEVSEsIfNotExist (EVSEs, TransmissionType = Enqueue, ...)

        /// <summary>
        /// Add the given enumeration of EVSEs, if they do not already exist.
        /// </summary>
        /// <param name="EVSEs">An enumeration of EVSEs to add, if they do not already exist.</param>
        /// <param name="TransmissionType">Whether to send the EVSE directly or enqueue it for a while.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        public virtual Task<AddEVSEsResult>

            AddEVSEsIfNotExist(IEnumerable<IEVSE>  EVSEs,
                               TransmissionTypes   TransmissionType    = TransmissionTypes.Enqueue,

                               DateTimeOffset?     Timestamp           = null,
                               EventTracking_Id?   EventTrackingId     = null,
                               TimeSpan?           RequestTimeout      = null,
                               User_Id?            CurrentUserId       = null,
                               CancellationToken   CancellationToken   = default)


                => Task.FromResult(
                       AddEVSEsResult.NoOperation(
                           RejectedEVSEs:    EVSEs,
                           SenderId:         Id,
                           Sender:           this,
                           EventTrackingId:  EventTrackingId
                       )
                   );

        #endregion

        #region AddOrUpdateEVSEs   (EVSEs, TransmissionType = Enqueue, ...)

        /// <summary>
        /// Add or update the given enumeration of EVSEs.
        /// </summary>
        /// <param name="EVSEs">An enumeration of EVSEs to add or update.</param>
        /// <param name="TransmissionType">Whether to send the EVSE directly or enqueue it for a while.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        public virtual Task<AddOrUpdateEVSEsResult>

            AddOrUpdateEVSEs(IEnumerable<IEVSE>  EVSEs,
                             TransmissionTypes   TransmissionType    = TransmissionTypes.Enqueue,

                             DateTimeOffset?     Timestamp           = null,
                             EventTracking_Id?   EventTrackingId     = null,
                             TimeSpan?           RequestTimeout      = null,
                             User_Id?            CurrentUserId       = null,
                             CancellationToken   CancellationToken   = default)


                => Task.FromResult(
                       AddOrUpdateEVSEsResult.NoOperation(
                           RejectedEVSEs:    EVSEs,
                           SenderId:         Id,
                           Sender:           this,
                           EventTrackingId:  EventTrackingId
                       )
                   );

        #endregion

        #region UpdateEVSEs        (EVSEs, TransmissionType = Enqueue, ...)

        /// <summary>
        /// Update the given enumeration of EVSEs.
        /// </summary>
        /// <param name="EVSEs">An enumeration of EVSEs to update.</param>
        /// <param name="TransmissionType">Whether to send the EVSE directly or enqueue it for a while.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        public virtual Task<UpdateEVSEsResult>

            UpdateEVSEs(IEnumerable<IEVSE>  EVSEs,
                        TransmissionTypes   TransmissionType    = TransmissionTypes.Enqueue,

                        DateTimeOffset?     Timestamp           = null,
                        EventTracking_Id?   EventTrackingId     = null,
                        TimeSpan?           RequestTimeout      = null,
                        User_Id?            CurrentUserId       = null,
                        CancellationToken   CancellationToken   = default)


                => Task.FromResult(
                       UpdateEVSEsResult.NoOperation(
                           RejectedEVSEs:    EVSEs,
                           SenderId:         Id,
                           Sender:           this,
                           EventTrackingId:  EventTrackingId
                       )
                   );

        #endregion

        #region ReplaceEVSEs       (EVSEs, TransmissionType = Enqueue, ...)

        /// <summary>
        /// Replace the given enumeration of EVSEs.
        /// EVSEs not included will be deleted.
        /// </summary>
        /// <param name="EVSEs">An enumeration of EVSEs to replace.</param>
        /// <param name="TransmissionType">Whether to send the EVSE directly or enqueue it for a while.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        public virtual Task<ReplaceEVSEsResult>

            ReplaceEVSEs(IEnumerable<IEVSE>  EVSEs,
                         TransmissionTypes   TransmissionType    = TransmissionTypes.Enqueue,

                         DateTimeOffset?     Timestamp           = null,
                         EventTracking_Id?   EventTrackingId     = null,
                         TimeSpan?           RequestTimeout      = null,
                         User_Id?            CurrentUserId       = null,
                         CancellationToken   CancellationToken   = default)


                => Task.FromResult(
                       ReplaceEVSEsResult.NoOperation(
                           RejectedEVSEs:    EVSEs,
                           SenderId:         Id,
                           Sender:           this,
                           EventTrackingId:  EventTrackingId
                       )
                   );

        #endregion

        #region DeleteEVSEs        (EVSEs, TransmissionType = Enqueue, ...)

        /// <summary>
        /// Delete the given enumeration of EVSEs.
        /// </summary>
        /// <param name="EVSEs">An enumeration of EVSEs to delete.</param>
        /// <param name="TransmissionType">Whether to send the EVSE directly or enqueue it for a while.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        public virtual Task<DeleteEVSEsResult>

            DeleteEVSEs(IEnumerable<IEVSE>  EVSEs,
                        TransmissionTypes   TransmissionType    = TransmissionTypes.Enqueue,

                        DateTimeOffset?     Timestamp           = null,
                        EventTracking_Id?   EventTrackingId     = null,
                        TimeSpan?           RequestTimeout      = null,
                        User_Id?            CurrentUserId       = null,
                        CancellationToken   CancellationToken   = default)


                => Task.FromResult(
                       DeleteEVSEsResult.NoOperation(
                           RejectedEVSEs:    EVSEs,
                           SenderId:         Id,
                           Sender:           this,
                           EventTrackingId:  EventTrackingId
                       )
                   );

        #endregion


        #region UpdateEVSEAdminStatus  (EVSEAdminStatusUpdates,  TransmissionType = Enqueue, ...)

        /// <summary>
        /// Update the given enumeration of EVSE admin status updates.
        /// </summary>
        /// <param name="EVSEAdminStatusUpdates">An enumeration of EVSE admin status updates.</param>
        /// <param name="TransmissionType">Whether to send the EVSE admin status updates directly or enqueue it for a while.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        public virtual Task<PushEVSEAdminStatusResult>

            UpdateEVSEAdminStatus(IEnumerable<EVSEAdminStatusUpdate>  EVSEAdminStatusUpdates,
                                  TransmissionTypes                   TransmissionType    = TransmissionTypes.Enqueue,

                                  DateTimeOffset?                     Timestamp           = null,
                                  EventTracking_Id?                   EventTrackingId     = null,
                                  TimeSpan?                           RequestTimeout      = null,
                                  User_Id?                            CurrentUserId       = null,
                                  CancellationToken                   CancellationToken   = default)


                => Task.FromResult(
                       PushEVSEAdminStatusResult.NoOperation(
                           Id,
                           this
                       )
                   );

        #endregion

        #region UpdateEVSEStatus       (EVSEStatusUpdates,       TransmissionType = Enqueue, ...)

        /// <summary>
        /// Update the given enumeration of EVSE status updates.
        /// </summary>
        /// <param name="EVSEStatusUpdates">An enumeration of EVSE status updates.</param>
        /// <param name="TransmissionType">Whether to send the EVSE status updates directly or enqueue it for a while.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        public virtual Task<PushEVSEStatusResult>

            UpdateEVSEStatus(IEnumerable<EVSEStatusUpdate>  EVSEStatusUpdates,
                             TransmissionTypes              TransmissionType    = TransmissionTypes.Enqueue,

                             DateTimeOffset?                Timestamp           = null,
                             EventTracking_Id?              EventTrackingId     = null,
                             TimeSpan?                      RequestTimeout      = null,
                             User_Id?                       CurrentUserId       = null,
                             CancellationToken              CancellationToken   = default)


                => Task.FromResult(
                       PushEVSEStatusResult.NoOperation(
                           Id,
                           this
                       )
                   );

        #endregion

        #region UpdateEVSEEnergyStatus (EVSEEnergyStatusUpdates, TransmissionType = Enqueue, ...)

        /// <summary>
        /// Update the given enumeration of EVSE energy status updates.
        /// </summary>
        /// <param name="EnergyStatusUpdates">An enumeration of EVSE energy status updates.</param>
        /// <param name="TransmissionType">Whether to send the EVSE energy status updates directly or enqueue it for a while.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        public virtual Task<PushEVSEEnergyStatusResult>

            UpdateEVSEEnergyStatus(IEnumerable<EVSEEnergyStatusUpdate>  EVSEEnergyStatusUpdates,
                                   TransmissionTypes                    TransmissionType    = TransmissionTypes.Enqueue,

                                   DateTimeOffset?                      Timestamp           = null,
                                   EventTracking_Id?                    EventTrackingId     = null,
                                   TimeSpan?                            RequestTimeout      = null,
                                   User_Id?                             CurrentUserId       = null,
                                   CancellationToken                    CancellationToken   = default)


                => Task.FromResult(
                       PushEVSEEnergyStatusResult.NoOperation(
                           Id,
                           this
                       )
                   );

        #endregion

        #endregion


        #region (timer) FlushEVSEDataAndStatus(State)

        protected abstract Boolean  SkipFlushEVSEDataAndStatusQueues();

        protected abstract Task     FlushEVSEDataAndStatusQueues();

        private void FlushEVSEDataAndStatus(Object State)
        {
            if (!DisablePushData)
                FlushEVSEDataAndStatus2(State).Wait();
        }

        private async Task FlushEVSEDataAndStatus2(Object State)
        {

            var LockTaken = await FlushEVSEDataAndStatusLock.WaitAsync(0);

            try
            {

                if (LockTaken)
                {

                    //DebugX.LogT("FlushEVSEDataAndStatusLock entered!");

                    if (SkipFlushEVSEDataAndStatusQueues())
                        return;

                    #region Send StartEvent...

                    var StartTime = Timestamp.Now;

                    FlushEVSEDataAndStatusQueuesStartedEvent?.Invoke(this,
                                                                     StartTime,
                                                                     FlushEVSEDataAndStatusEvery,
                                                                     _FlushEVSEDataRunId);

                    #endregion

                    await FlushEVSEDataAndStatusQueues();

                    #region Send Finished Event...

                    var EndTime = Timestamp.Now;

                    FlushEVSEDataAndStatusQueuesFinishedEvent?.Invoke(this,
                                                                      StartTime,
                                                                      EndTime,
                                                                      EndTime - StartTime,
                                                                      FlushEVSEDataAndStatusEvery,
                                                                      _FlushEVSEDataRunId);

                    #endregion

                    _FlushEVSEDataRunId++;

                }

            }
            catch (Exception e)
            {

                while (e.InnerException is not null)
                    e = e.InnerException;

                DebugX.LogT(GetType().Name + ".FlushEVSEDataAndStatus '" + Id + "' led to an exception: " + e.Message + Environment.NewLine + e.StackTrace);

                OnWWCPCPOAdapterException?.Invoke(Timestamp.Now,
                                                  this,
                                                  e);

            }

            finally
            {

                if (LockTaken)
                {
                    FlushEVSEDataAndStatusLock.Release();
               //     DebugX.LogT("FlushEVSEDataAndStatusLock released!");
                }

                else
                    DebugX.LogT("FlushEVSEDataAndStatusLock exited!");

            }

        }

        #endregion

        #region (timer) FlushEVSEFastStatus(State)

        protected abstract Boolean  SkipFlushEVSEFastStatusQueues();

        protected abstract Task     FlushEVSEFastStatusQueues();

        private void FlushEVSEFastStatus(Object State)
        {
            if (!DisableSendStatus)
                FlushEVSEFastStatus2(State).Wait();
        }

        private async Task FlushEVSEFastStatus2(Object State)
        {

            var LockTaken = await FlushEVSEFastStatusLock.WaitAsync(0);

            try
            {

                if (LockTaken)
                {

                    //DebugX.LogT("FlushEVSEFastStatusLock entered!");

                    if (SkipFlushEVSEFastStatusQueues())
                        return;

                    #region Send StartEvent...

                    var StartTime = Timestamp.Now;

                    FlushEVSEFastStatusQueuesStartedEvent?.Invoke(this,
                                                                  StartTime,
                                                                  FlushEVSEFastStatusEvery,
                                                                  _StatusRunId);

                    #endregion

                    await FlushEVSEFastStatusQueues();

                    #region Send Finished Event...

                    var EndTime = Timestamp.Now;

                    FlushEVSEFastStatusQueuesFinishedEvent?.Invoke(this,
                                                                   StartTime,
                                                                   EndTime,
                                                                   EndTime - StartTime,
                                                                   FlushEVSEFastStatusEvery,
                                                                   _StatusRunId);

                    #endregion

                    _StatusRunId++;

                }

            }
            catch (Exception e)
            {

                while (e.InnerException is not null)
                    e = e.InnerException;

                DebugX.LogT(GetType().Name + ".FlushEVSEFastStatus '" + Id + "' led to an exception: " + e.Message + Environment.NewLine + e.StackTrace);

                OnWWCPCPOAdapterException?.Invoke(Timestamp.Now,
                                                  this,
                                                  e);

            }

            finally
            {

                if (LockTaken)
                {
                    FlushEVSEFastStatusLock.Release();
                 //   DebugX.LogT("FlushEVSEFastStatusLock released!");
                }

                else
                    DebugX.LogT("FlushEVSEFastStatusLock exited!");

            }

        }

        #endregion

        #region (timer) FlushChargeDetailRecords(State)

        protected abstract Boolean  SkipFlushChargeDetailRecordsQueues();

        protected abstract Task     FlushChargeDetailRecordsQueues(IEnumerable<TChargeDetailRecords> ChargeDetailsRecords);

        private void FlushChargeDetailRecords(Object State)
        {
            if (!DisableSendChargeDetailRecords)
                FlushChargeDetailRecords2(State).Wait();
        }

        private async Task FlushChargeDetailRecords2(Object State)
        {

            var LockTaken = await FlushChargeDetailRecordsLock.WaitAsync(0);

            try
            {

                if (LockTaken)
                {

                    //DebugX.LogT("FlushChargeDetailRecordsLock entered!");

                    if (SkipFlushChargeDetailRecordsQueues())
                        return;

                    #region Send StartEvent...

                    var StartTime = Timestamp.Now;

                    FlushChargeDetailRecordsQueuesStartedEvent?.Invoke(this,
                                                                       StartTime,
                                                                       FlushEVSEFastStatusEvery,
                                                                       _CDRRunId);

                    #endregion

                    var chargeDetailRecordsQueueCopy = chargeDetailRecordsQueue.ToArray();
                    chargeDetailRecordsQueue.Clear();

                    await FlushChargeDetailRecordsQueues(chargeDetailRecordsQueueCopy);

                    #region Send Finished Event...

                    var EndTime = Timestamp.Now;

                    FlushChargeDetailRecordsQueuesFinishedEvent?.Invoke(this,
                                                                        StartTime,
                                                                        EndTime,
                                                                        EndTime - StartTime,
                                                                        FlushEVSEFastStatusEvery,
                                                                        _CDRRunId);

                    #endregion

                    _CDRRunId++;

                }

            }
            catch (Exception e)
            {

                while (e.InnerException is not null)
                    e = e.InnerException;

                DebugX.LogT(GetType().Name + ".FlushChargeDetailRecords '" + Id + "' led to an exception: " + e.Message + Environment.NewLine + e.StackTrace);

                OnWWCPCPOAdapterException?.Invoke(Timestamp.Now,
                                                  this,
                                                  e);

            }

            finally
            {

                if (LockTaken)
                {
                    FlushChargeDetailRecordsLock.Release();
                    DebugX.LogT("FlushChargeDetailRecordsLock released!");
                }

                else
                    DebugX.LogT("FlushChargeDetailRecordsLock exited!");

            }

        }

        #endregion



        protected void SendOnWarnings(DateTimeOffset        Timestamp,
                                      String                Class,
                                      String                Method,
                                      IEnumerable<Warning>  Warnings)
        {

            if (Warnings.Any())
                OnWarnings?.Invoke(Timestamp,
                                   Class,
                                   Method,
                                   Warnings);

        }


    }

}
