/*
 * Copyright (c) 2014-2023 GraphDefined GmbH <achim.friedland@graphdefined.com>
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

#endregion

namespace cloud.charging.open.protocols.WWCP
{

    #region (class) PropertyUpdateInfo

    public class PropertyUpdateInfo
    {

        public String   PropertyName    { get; }
        public Object?  OldValue        { get; }
        public Object?  NewValue        { get; }

        public PropertyUpdateInfo(String   PropertyName,
                                  Object?  OldValue,
                                  Object?  NewValue)
        {

            this.PropertyName  = PropertyName;
            this.OldValue      = OldValue;
            this.NewValue      = NewValue;

        }


        public override String ToString()

            => String.Concat("Update of '", PropertyName, "' '",
                                OldValue != null ? OldValue.ToString() : "",
                                "' -> '",
                                NewValue != null ? NewValue.ToString() : "",
                                "'!");

    }

    #endregion


    public abstract class AWWCPEMPAdapter<TChargeDetailRecords> : ACryptoEMobilityEntity<EMPRoamingProvider_Id,
                                                                                         EMPRoamingProviderAdminStatusTypes,
                                                                                         EMPRoamingProviderStatusTypes>
    {

        #region Data

        /// <summary>
        /// The default service check intervall.
        /// </summary>
        public  readonly static TimeSpan                                                         DefaultFlushEVSEDataAndStatusEvery      = TimeSpan.FromSeconds(31);

        /// <summary>
        /// The default status check intervall.
        /// </summary>
        public  readonly static TimeSpan                                                         DefaultFlushEVSEFastStatusEvery         = TimeSpan.FromSeconds(3);

        /// <summary>
        /// The default CDR check intervall.
        /// </summary>
        public  readonly static TimeSpan                                                         DefaultFlushChargeDetailRecordsEvery    = TimeSpan.FromSeconds(15);


        protected               UInt64                                                           _FlushEVSEDataRunId                     = 1;
        protected               UInt64                                                           _StatusRunId                            = 1;
        protected               UInt64                                                           _CDRRunId                               = 1;

        protected readonly      SemaphoreSlim                                                    DataAndStatusLock                       = new (1, 1);
        protected readonly      Object                                                           DataAndStatusLockOld                    = new ();

        protected readonly      SemaphoreSlim                                                    FlushEVSEDataAndStatusLock              = new (1, 1);
        protected readonly      Timer                                                            FlushEVSEDataAndStatusTimer;

        protected readonly      SemaphoreSlim                                                    FlushEVSEFastStatusLock                 = new (1, 1);
        protected readonly      Timer                                                            FlushEVSEFastStatusTimer;

        protected readonly      SemaphoreSlim                                                    FlushChargeDetailRecordsLock            = new (1, 1);
        protected readonly      Timer                                                            FlushChargeDetailRecordsTimer;

        protected readonly      Dictionary<RoamingNetwork,          List<PropertyUpdateInfo>>    RoamingNetworksUpdateLog;

        protected readonly      Dictionary<ChargingStationOperator, List<PropertyUpdateInfo>>    ChargingStationOperatorsUpdateLog;

        protected readonly      Dictionary<IChargingPool,           List<PropertyUpdateInfo>>    ChargingPoolsUpdateLog;
        protected readonly      HashSet<IChargingPool>                                           ChargingPoolsToAddQueue;
        protected readonly      HashSet<IChargingPool>                                           ChargingPoolsToUpdateQueue;
        protected readonly      HashSet<IChargingPool>                                           ChargingPoolsToRemoveQueue;
        protected readonly      List<ChargingPoolAdminStatusUpdate>                              ChargingPoolAdminStatusChangesFastQueue;
        protected readonly      List<ChargingPoolAdminStatusUpdate>                              ChargingPoolAdminStatusChangesDelayedQueue;
        protected readonly      List<ChargingPoolStatusUpdate>                                   ChargingPoolStatusChangesFastQueue;
        protected readonly      List<ChargingPoolStatusUpdate>                                   ChargingPoolStatusChangesDelayedQueue;

        protected readonly      HashSet<IChargingStation>                                        ChargingStationsToAddQueue;
        protected readonly      HashSet<IChargingStation>                                        ChargingStationsToUpdateQueue;
        protected readonly      HashSet<IChargingStation>                                        ChargingStationsToRemoveQueue;
        protected readonly      List<ChargingStationAdminStatusUpdate>                           ChargingStationAdminStatusChangesFastQueue;
        protected readonly      List<ChargingStationAdminStatusUpdate>                           ChargingStationAdminStatusChangesDelayedQueue;
        protected readonly      List<ChargingStationStatusUpdate>                                ChargingStationStatusChangesFastQueue;
        protected readonly      List<ChargingStationStatusUpdate>                                ChargingStationStatusChangesDelayedQueue;
        protected readonly      Dictionary<IChargingStation,        List<PropertyUpdateInfo>>    ChargingStationsUpdateLog;

        protected readonly      HashSet<IEVSE>                                                   EVSEsToAddQueue;
        protected readonly      HashSet<IEVSE>                                                   EVSEsToUpdateQueue;
        protected readonly      HashSet<IEVSE>                                                   EVSEsToRemoveQueue;
        protected readonly      List<EVSEAdminStatusUpdate>                                      EVSEAdminStatusChangesFastQueue;
        protected readonly      List<EVSEAdminStatusUpdate>                                      EVSEAdminStatusChangesDelayedQueue;
        protected readonly      List<EVSEStatusUpdate>                                           EVSEStatusChangesFastQueue;
        protected readonly      List<EVSEStatusUpdate>                                           EVSEStatusChangesDelayedQueue;
        protected readonly      Dictionary<IEVSE,                   List<PropertyUpdateInfo>>    EVSEsUpdateLog;

        protected readonly      List<TChargeDetailRecords>                                       ChargeDetailRecordsQueue;

        protected readonly      TimeSpan                                                         MaxLockWaitingTime                      = TimeSpan.FromSeconds(120);

        public static readonly  TimeSpan                                                         DefaultRequestTimeout                   = TimeSpan.FromSeconds(30);

        #endregion

        #region Properties

        /// <summary>
        /// Only include EVSE identifications matching the given delegate.
        /// </summary>
        public IncludeEVSEIdDelegate              IncludeEVSEIds                     { get; }

        /// <summary>
        /// Only include EVSEs matching the given delegate.
        /// </summary>
        public IncludeEVSEDelegate                IncludeEVSEs                       { get; }

        /// <summary>
        /// Only include charging station identifications matching the given delegate.
        /// </summary>
        public IncludeChargingStationIdDelegate   IncludeChargingStationIds          { get; }

        /// <summary>
        /// Only include charging stations matching the given delegate.
        /// </summary>
        public IncludeChargingStationDelegate     IncludeChargingStations            { get; }

        /// <summary>
        /// Only include charging pool identifications matching the given delegate.
        /// </summary>
        public IncludeChargingPoolIdDelegate      IncludeChargingPoolIds             { get; }

        /// <summary>
        /// Only include charging pools matching the given delegate.
        /// </summary>
        public IncludeChargingPoolDelegate        IncludeChargingPools               { get; }

        ///// <summary>
        ///// A delegate to customize the mapping of EVSE identifications.
        ///// </summary>
        //public CustomEVSEIdMapperDelegate        CustomEVSEIdMapper                { get; }

        /// <summary>
        /// A delegate for filtering charge detail records.
        /// </summary>
        public ChargeDetailRecordFilterDelegate   ChargeDetailRecordFilter           { get; }



        /// <summary>
        /// This service can be disabled, e.g. for debugging reasons.
        /// </summary>
        public Boolean                            DisablePushData                    { get; set; }

        /// <summary>
        /// This service can be disabled, e.g. for debugging reasons.
        /// </summary>
        public Boolean                            DisablePushAdminStatus             { get; set; }

        /// <summary>
        /// This service can be disabled, e.g. for debugging reasons.
        /// </summary>
        public Boolean                            DisablePushStatus                  { get; set; }

        /// <summary>
        /// This service can be disabled, e.g. for debugging reasons.
        /// </summary>
        public Boolean                            DisableAuthentication              { get; set; }

        /// <summary>
        /// This service can be disabled, e.g. for debugging reasons.
        /// </summary>
        public Boolean                            DisableSendChargeDetailRecords     { get; set; }

        /// <summary>
        /// The EVSE data updates transmission intervall.
        /// </summary>
        public TimeSpan                           FlushEVSEDataAndStatusEvery        { get; set; }

        /// <summary>
        /// The EVSE status updates transmission intervall.
        /// </summary>
        public TimeSpan                           FlushEVSEFastStatusEvery           { get; set; }

        /// <summary>
        /// The charge detail record transmission intervall.
        /// </summary>
        public TimeSpan                           FlushChargeDetailRecordsEvery      { get; set; }

        #endregion

        #region Events

        #region OnWWCPCPOAdapterException

        public delegate Task OnWWCPCPOAdapterExceptionDelegate(DateTime             Timestamp,
                                                               AWWCPEMPAdapter<TChargeDetailRecords>      Sender,
                                                               Exception            Exception);

        public event OnWWCPCPOAdapterExceptionDelegate OnWWCPCPOAdapterException;

        #endregion


        #region FlushEVSEDataAndStatusQueues

        public delegate void FlushEVSEDataAndStatusQueuesStartedDelegate(AWWCPEMPAdapter<TChargeDetailRecords> Sender, DateTime StartTime, TimeSpan Every, UInt64 RunId);

        public event FlushEVSEDataAndStatusQueuesStartedDelegate FlushEVSEDataAndStatusQueuesStartedEvent;

        public delegate void FlushEVSEDataAndStatusQueuesFinishedDelegate(AWWCPEMPAdapter<TChargeDetailRecords> Sender, DateTime StartTime, DateTime EndTime, TimeSpan Runtime, TimeSpan Every, UInt64 RunId);

        public event FlushEVSEDataAndStatusQueuesFinishedDelegate FlushEVSEDataAndStatusQueuesFinishedEvent;

        #endregion

        #region FlushEVSEFastStatusQueues

        public delegate void FlushEVSEFastStatusQueuesStartedDelegate(AWWCPEMPAdapter<TChargeDetailRecords> Sender, DateTime StartTime, TimeSpan Every, UInt64 RunId);

        public event FlushEVSEFastStatusQueuesStartedDelegate FlushEVSEFastStatusQueuesStartedEvent;

        public delegate void FlushEVSEFastStatusQueuesFinishedDelegate(AWWCPEMPAdapter<TChargeDetailRecords> Sender, DateTime StartTime, DateTime EndTime, TimeSpan Runtime, TimeSpan Every, UInt64 RunId);

        public event FlushEVSEFastStatusQueuesFinishedDelegate FlushEVSEFastStatusQueuesFinishedEvent;

        #endregion

        #region FlushChargeDetailRecordsQueues

        public delegate void FlushChargeDetailRecordsQueuesStartedDelegate(AWWCPEMPAdapter<TChargeDetailRecords> Sender, DateTime StartTime, TimeSpan Every, UInt64 RunId);

        public event FlushChargeDetailRecordsQueuesStartedDelegate FlushChargeDetailRecordsQueuesStartedEvent;

        public delegate void FlushChargeDetailRecordsQueuesFinishedDelegate(AWWCPEMPAdapter<TChargeDetailRecords> Sender, DateTime StartTime, DateTime EndTime, TimeSpan Runtime, TimeSpan Every, UInt64 RunId);

        public event FlushChargeDetailRecordsQueuesFinishedDelegate FlushChargeDetailRecordsQueuesFinishedEvent;

        #endregion

        public delegate Task OnWarningsDelegate(DateTime Timestamp, String Class, String Method, IEnumerable<Warning> Warnings);

        public event OnWarningsDelegate OnWarnings;

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new WWCP wrapper for the OICP roaming client for Charging Station Operators/CPOs.
        /// </summary>
        /// <param name="Id">The unique identification of the roaming provider.</param>
        /// <param name="Name">The offical (multi-language) name of the roaming provider.</param>
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
        /// <param name="FlushEVSEDataAndStatusEvery">The service check intervall.</param>
        /// <param name="FlushEVSEFastStatusEvery">The status check intervall.</param>
        /// <param name="FlushChargeDetailRecordsEvery">The charge detail record intervall.</param>
        /// 
        /// <param name="DisablePushData">This service can be disabled, e.g. for debugging reasons.</param>
        /// <param name="DisablePushStatus">This service can be disabled, e.g. for debugging reasons.</param>
        /// <param name="DisableAuthentication">This service can be disabled, e.g. for debugging reasons.</param>
        /// <param name="DisableSendChargeDetailRecords">This service can be disabled, e.g. for debugging reasons.</param>
        protected AWWCPEMPAdapter(EMPRoamingProvider_Id              Id,
                                  RoamingNetwork                     RoamingNetwork,

                                  I18NString?                        Name                             = null,
                                  I18NString?                        Description                      = null,

                                  IncludeEVSEIdDelegate?             IncludeEVSEIds                   = null,
                                  IncludeEVSEDelegate?               IncludeEVSEs                     = null,
                                  IncludeChargingStationIdDelegate?  IncludeChargingStationIds        = null,
                                  IncludeChargingStationDelegate?    IncludeChargingStations          = null,
                                  IncludeChargingPoolIdDelegate?     IncludeChargingPoolIds           = null,
                                  IncludeChargingPoolDelegate?       IncludeChargingPools             = null,
                                  ChargeDetailRecordFilterDelegate?  ChargeDetailRecordFilter         = null,

                                  TimeSpan?                          FlushEVSEDataAndStatusEvery      = null,
                                  TimeSpan?                          FlushEVSEFastStatusEvery         = null,
                                  TimeSpan?                          FlushChargeDetailRecordsEvery    = null,

                                  Boolean                            DisablePushData                  = false,
                                  Boolean                            DisablePushAdminStatus           = false,
                                  Boolean                            DisablePushStatus                = false,
                                  Boolean                            DisableAuthentication            = false,
                                  Boolean                            DisableSendChargeDetailRecords   = false,

                                  String                             EllipticCurve                    = "P-256",
                                  ECPrivateKeyParameters?            PrivateKey                       = null,
                                  PublicKeyCertificates?             PublicKeyCertificates            = null)

            : base(Id,
                   RoamingNetwork,
                   Name,
                   Description,
                   EllipticCurve,
                   PrivateKey,
                   PublicKeyCertificates)

        {

            this.IncludeEVSEIds                                  = IncludeEVSEIds                ?? (evseid             => true);
            this.IncludeEVSEs                                    = IncludeEVSEs                  ?? (evse               => true);
            this.IncludeChargingStationIds                       = IncludeChargingStationIds     ?? (chargingStationId  => true);
            this.IncludeChargingStations                         = IncludeChargingStations       ?? (chargingStation    => true);
            this.IncludeChargingPoolIds                          = IncludeChargingPoolIds        ?? (chargingPoolId     => true);
            this.IncludeChargingPools                            = IncludeChargingPools          ?? (chargingPool       => true);
            this.ChargeDetailRecordFilter                        = ChargeDetailRecordFilter      ?? (chargeDetailRecord => ChargeDetailRecordFilters.forward);

            this.DisablePushData                                 = DisablePushData;
            this.DisablePushAdminStatus                          = DisablePushAdminStatus;
            this.DisablePushStatus                               = DisablePushStatus;
            this.DisableAuthentication                           = DisableAuthentication;
            this.DisableSendChargeDetailRecords                  = DisableSendChargeDetailRecords;

            this.FlushEVSEDataAndStatusEvery                     = FlushEVSEDataAndStatusEvery   ?? DefaultFlushEVSEDataAndStatusEvery;
            this.FlushEVSEFastStatusEvery                        = FlushEVSEFastStatusEvery      ?? DefaultFlushEVSEFastStatusEvery;
            this.FlushChargeDetailRecordsEvery                   = FlushChargeDetailRecordsEvery ?? DefaultFlushChargeDetailRecordsEvery;

            this.FlushEVSEDataAndStatusTimer                     = new Timer(FlushEVSEDataAndStatus);
            this.FlushEVSEFastStatusTimer                        = new Timer(FlushEVSEFastStatus);
            this.FlushChargeDetailRecordsTimer                   = new Timer(FlushChargeDetailRecords);

            this.ChargingPoolsToAddQueue                         = new HashSet<IChargingPool>();
            this.ChargingPoolsToUpdateQueue                      = new HashSet<IChargingPool>();
            this.ChargingPoolsToRemoveQueue                      = new HashSet<IChargingPool>();
            this.ChargingPoolAdminStatusChangesFastQueue         = new List<ChargingPoolAdminStatusUpdate>();
            this.ChargingPoolAdminStatusChangesDelayedQueue      = new List<ChargingPoolAdminStatusUpdate>();
            this.ChargingPoolStatusChangesFastQueue              = new List<ChargingPoolStatusUpdate>();
            this.ChargingPoolStatusChangesDelayedQueue           = new List<ChargingPoolStatusUpdate>();
            this.ChargingPoolsUpdateLog                          = new Dictionary<IChargingPool,    List<PropertyUpdateInfo>>();

            this.ChargingStationsToAddQueue                      = new HashSet<IChargingStation>();
            this.ChargingStationsToUpdateQueue                   = new HashSet<IChargingStation>();
            this.ChargingStationsToRemoveQueue                   = new HashSet<IChargingStation>();
            this.ChargingStationAdminStatusChangesFastQueue      = new List<ChargingStationAdminStatusUpdate>();
            this.ChargingStationAdminStatusChangesDelayedQueue   = new List<ChargingStationAdminStatusUpdate>();
            this.ChargingStationStatusChangesFastQueue           = new List<ChargingStationStatusUpdate>();
            this.ChargingStationStatusChangesDelayedQueue        = new List<ChargingStationStatusUpdate>();
            this.ChargingStationsUpdateLog                       = new Dictionary<IChargingStation, List<PropertyUpdateInfo>>();

            this.EVSEsToAddQueue                                 = new HashSet<IEVSE>();
            this.EVSEsToUpdateQueue                              = new HashSet<IEVSE>();
            this.EVSEsToRemoveQueue                              = new HashSet<IEVSE>();
            this.EVSEAdminStatusChangesFastQueue                 = new List<EVSEAdminStatusUpdate>();
            this.EVSEAdminStatusChangesDelayedQueue              = new List<EVSEAdminStatusUpdate>();
            this.EVSEStatusChangesFastQueue                      = new List<EVSEStatusUpdate>();
            this.EVSEStatusChangesDelayedQueue                   = new List<EVSEStatusUpdate>();
            this.EVSEsUpdateLog                                  = new Dictionary<IEVSE,            List<PropertyUpdateInfo>>();

            this.ChargeDetailRecordsQueue                        = new List<TChargeDetailRecords>();

        }

        #endregion


        #region SetStaticData   (Sender, EVSE, ...)

        /// <summary>
        /// Set the given EVSE as new static EVSE data at the eMIP server.
        /// </summary>
        /// <param name="Sender">The sender of the new EVSE data.</param>
        /// <param name="EVSE">An EVSE to upload.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        protected async Task<PushEVSEDataResult>

            SetStaticData(ISendPOIData        Sender,
                          IEVSE               EVSE,

                          DateTime?           Timestamp,
                          CancellationToken?  CancellationToken,
                          EventTracking_Id?   EventTrackingId,
                          TimeSpan?           RequestTimeout)

        {

            #region Initial checks

            if (DisablePushData)
                return PushEVSEDataResult.AdminDown(Id,
                                                    Sender,
                                                    new IEVSE[] { EVSE });

            #endregion

            #region Send OnEnqueueSendCDRRequest event

            //try
            //{

            //    OnEnqueueSendCDRRequest?.Invoke(StartTime,
            //                                    Timestamp.Value,
            //                                    this,
            //                                    EventTrackingId,
            //                                    RoamingNetwork.Id,
            //                                    ChargeDetailRecord,
            //                                    RequestTimeout);

            //}
            //catch (Exception e)
            //{
            //    DebugX.LogException(e, nameof(WWCPCPOAdapter) + "." + nameof(OnSendCDRRequest));
            //}

            #endregion

            await DataAndStatusLock.WaitAsync();

            try
            {

                if (IncludeEVSEs == null ||
                   (IncludeEVSEs != null && IncludeEVSEs(EVSE)))
                {

                    EVSEsToAddQueue.Add(EVSE);

                    FlushEVSEDataAndStatusTimer.Change(FlushEVSEDataAndStatusEvery, TimeSpan.FromMilliseconds(-1));

                }

            }
            finally
            {
                DataAndStatusLock.Release();
            }

            return PushEVSEDataResult.Enqueued(Id, Sender, new IEVSE[] { EVSE });

        }

        #endregion

        #region AddStaticData   (Sender, EVSE, ...)

        /// <summary>
        /// Add the given EVSE to the static EVSE data at the eMIP server.
        /// </summary>
        /// <param name="Sender">The sender of the new EVSE data.</param>
        /// <param name="EVSE">An EVSE to upload.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        protected async Task<PushEVSEDataResult>

            AddStaticData(ISendPOIData        Sender,
                          IEVSE               EVSE,

                          DateTime?           Timestamp,
                          CancellationToken?  CancellationToken,
                          EventTracking_Id?   EventTrackingId,
                          TimeSpan?           RequestTimeout)

        {

            #region Initial checks

            if (DisablePushData)
                return PushEVSEDataResult.AdminDown(Id,
                                                    Sender,
                                                    new IEVSE[] { EVSE });

            #endregion

            #region Send OnEnqueueSendCDRRequest event

            //try
            //{

            //    OnEnqueueSendCDRRequest?.Invoke(StartTime,
            //                                    Timestamp.Value,
            //                                    this,
            //                                    EventTrackingId,
            //                                    RoamingNetwork.Id,
            //                                    ChargeDetailRecord,
            //                                    RequestTimeout);

            //}
            //catch (Exception e)
            //{
            //    DebugX.LogException(e, nameof(WWCPCPOAdapter) + "." + nameof(OnSendCDRRequest));
            //}

            #endregion

            await DataAndStatusLock.WaitAsync();

            try
            {

                if (IncludeEVSEs == null ||
                   (IncludeEVSEs != null && IncludeEVSEs(EVSE)))
                {

                    EVSEsToAddQueue.Add(EVSE);

                    FlushEVSEDataAndStatusTimer.Change(FlushEVSEDataAndStatusEvery, TimeSpan.FromMilliseconds(-1));

                }

            }
            finally
            {
                DataAndStatusLock.Release();
            }

            return PushEVSEDataResult.Enqueued(Id, Sender, new IEVSE[] { EVSE });

        }

        #endregion


        #region UpdateAdminStatus(Sender, AdminStatusUpdates, ...)

        /// <summary>
        /// Update the given enumeration of EVSE admin status updates.
        /// </summary>
        /// <param name="Sender">The sender of the admin status update.</param>
        /// <param name="AdminStatusUpdates">An enumeration of EVSE admin status updates.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        protected async Task<PushEVSEAdminStatusResult>

            UpdateStatus(ISendAdminStatus                    Sender,
                         IEnumerable<EVSEAdminStatusUpdate>  AdminStatusUpdates,

                         DateTime?                           Timestamp,
                         CancellationToken?                  CancellationToken,
                         EventTracking_Id                    EventTrackingId,
                         TimeSpan?                           RequestTimeout)

        {

            #region Initial checks

            if (AdminStatusUpdates == null || !AdminStatusUpdates.Any())
                return PushEVSEAdminStatusResult.NoOperation(Id,
                                                             Sender);

            if (DisablePushStatus)
                return PushEVSEAdminStatusResult.AdminDown(Id,
                                                           Sender,
                                                           AdminStatusUpdates);

            #endregion


            #region Send OnEnqueueSendCDRRequest event

            //try
            //{

            //    OnEnqueueSendCDRRequest?.Invoke(StartTime,
            //                                    Timestamp.Value,
            //                                    this,
            //                                    EventTrackingId,
            //                                    RoamingNetwork.Id,
            //                                    ChargeDetailRecord,
            //                                    RequestTimeout);

            //}
            //catch (Exception e)
            //{
            //    DebugX.LogException(e, nameof(WWCPCPOAdapter) + "." + nameof(OnSendCDRRequest));
            //}

            #endregion

            await DataAndStatusLock.WaitAsync().ConfigureAwait(false);

            try
            {

                var FilteredUpdates = AdminStatusUpdates.Where(adminstatusupdate => IncludeEVSEIds(adminstatusupdate.Id)).
                                                         ToArray();

                if (FilteredUpdates.Length > 0)
                {

                    foreach (var Update in FilteredUpdates)
                    {

                        // Delay the status update until the EVSE data had been uploaded!
                        if (!DisablePushData && EVSEsToAddQueue.Any(evse => evse.Id == Update.Id))
                            EVSEAdminStatusChangesDelayedQueue.Add(Update);

                        else
                            EVSEAdminStatusChangesFastQueue.Add(Update);

                    }

                    FlushEVSEFastStatusTimer.Change(FlushEVSEFastStatusEvery, TimeSpan.FromMilliseconds(-1));

                    return PushEVSEAdminStatusResult.Enqueued(Id, Sender);

                }

                return PushEVSEAdminStatusResult.NoOperation(Id, Sender);

            }
            finally
            {
                DataAndStatusLock.Release();
            }

        }

        #endregion

        #region UpdateStatus     (Sender, StatusUpdates,      ...)

        /// <summary>
        /// Update the given enumeration of EVSE status updates.
        /// </summary>
        /// <param name="Sender">The sender of the status update.</param>
        /// <param name="StatusUpdates">An enumeration of EVSE status updates.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        protected async Task<PushEVSEStatusResult>

            UpdateStatus(ISendStatus                    Sender,
                         IEnumerable<EVSEStatusUpdate>  StatusUpdates,

                         DateTime?                      Timestamp,
                         CancellationToken?             CancellationToken,
                         EventTracking_Id               EventTrackingId,
                         TimeSpan?                      RequestTimeout)

        {

            #region Initial checks

            if (StatusUpdates == null || !StatusUpdates.Any())
                return PushEVSEStatusResult.NoOperation(Id,
                                                    Sender);

            if (DisablePushStatus)
                return PushEVSEStatusResult.AdminDown(Id,
                                                  Sender,
                                                  StatusUpdates);

            #endregion


            #region Send OnEnqueueSendCDRRequest event

            //try
            //{

            //    OnEnqueueSendCDRRequest?.Invoke(StartTime,
            //                                    Timestamp.Value,
            //                                    this,
            //                                    EventTrackingId,
            //                                    RoamingNetwork.Id,
            //                                    ChargeDetailRecord,
            //                                    RequestTimeout);

            //}
            //catch (Exception e)
            //{
            //    DebugX.LogException(e, nameof(WWCPCPOAdapter) + "." + nameof(OnSendCDRRequest));
            //}

            #endregion

            await DataAndStatusLock.WaitAsync().ConfigureAwait(false);

            try
            {

                var FilteredUpdates = StatusUpdates.Where(statusupdate => IncludeEVSEIds(statusupdate.Id)).
                                                    ToArray();

                if (FilteredUpdates.Length > 0)
                {

                    foreach (var Update in FilteredUpdates)
                    {

                        // Delay the status update until the EVSE data had been uploaded!
                        if (!DisablePushData && EVSEsToAddQueue.Any(evse => evse.Id == Update.Id))
                            EVSEStatusChangesDelayedQueue.Add(Update);

                        else
                            EVSEStatusChangesFastQueue.Add(Update);

                    }

                    FlushEVSEFastStatusTimer.Change(FlushEVSEFastStatusEvery, TimeSpan.FromMilliseconds(-1));

                    return PushEVSEStatusResult.Enqueued(Id, Sender);

                }

                return PushEVSEStatusResult.NoOperation(Id, Sender);

            }
            finally
            {
                DataAndStatusLock.Release();
            }

        }

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

                while (e.InnerException != null)
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
            if (!DisablePushStatus)
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

                while (e.InnerException != null)
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

                    var ChargeDetailRecordsQueueCopy = ChargeDetailRecordsQueue.ToArray();
                    ChargeDetailRecordsQueue.Clear();

                    await FlushChargeDetailRecordsQueues(ChargeDetailRecordsQueueCopy);

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

                while (e.InnerException != null)
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



        protected void SendOnWarnings(DateTime              Timestamp,
                                      String                Class,
                                      String                Method,
                                      IEnumerable<Warning>  Warnings)
        {

            if (Warnings is not null && Warnings.Any())
                OnWarnings?.Invoke(Timestamp,
                                   Class,
                                   Method,
                                   Warnings);

        }

    }

}
