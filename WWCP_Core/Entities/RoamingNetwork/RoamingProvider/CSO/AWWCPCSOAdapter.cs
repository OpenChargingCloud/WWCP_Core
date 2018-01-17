/*
 * Copyright (c) 2014-2018 GraphDefined GmbH <achim.friedland@graphdefined.com>
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
using org.GraphDefined.Vanaheimr.Hermod.DNS;
using Org.BouncyCastle.Bcpg.OpenPgp;

#endregion

namespace org.GraphDefined.WWCP
{


    public abstract class AWWCPCSOAdapter : ABaseEMobilityEntity<CSORoamingProvider_Id>
                                         //   ICSORoamingProvider,
                                         //   IComparable
    {

        #region (class) PropertyUpdateInfos

        public class PropertyUpdateInfos
        {

            public String PropertyName  { get; }
            public Object OldValue      { get; }
            public Object NewValue      { get; }

            public PropertyUpdateInfos(String PropertyName,
                                       Object OldValue,
                                       Object NewValue)
            {

                this.PropertyName  = PropertyName;
                this.OldValue      = OldValue;
                this.NewValue      = NewValue;

            }


            public override string ToString()

                => String.Concat("Update of '", PropertyName, "' '",
                                 OldValue != null ? OldValue.ToString() : "",
                                 "' -> '",
                                 NewValue != null ? NewValue.ToString() : "",
                                 "'!");

        }

        #endregion

        #region Data

        /// <summary>
        /// The default service check intervall.
        /// </summary>
        public readonly static TimeSpan                                                         DefaultServiceCheckEvery          = TimeSpan.FromSeconds(31);

        /// <summary>
        /// The default status check intervall.
        /// </summary>
        public readonly static TimeSpan                                                         DefaultStatusCheckEvery           = TimeSpan.FromSeconds(3);

        /// <summary>
        /// The default CDR check intervall.
        /// </summary>
        public readonly static TimeSpan                                                         DefaultCDRCheckEvery              = TimeSpan.FromSeconds(15);


        protected              UInt64                                                           _FlushEVSEDataRunId               = 1;
        protected              UInt64                                                           _StatusRunId                      = 1;
        protected              UInt64                                                           _CDRRunId                         = 1;

        protected readonly     SemaphoreSlim                                                    DataAndStatusLock                 = new SemaphoreSlim(1, 1);

        protected readonly     SemaphoreSlim                                                    FlushEVSEDataAndStatusLock        = new SemaphoreSlim(1, 1);
        protected readonly     Timer                                                            FlushEVSEDataAndStatusTimer;

        protected readonly     SemaphoreSlim                                                    FlushEVSEFastStatusLock           = new SemaphoreSlim(1, 1);
        protected readonly     Timer                                                            FlushEVSEFastStatusTimer;

        protected readonly     SemaphoreSlim                                                    FlushChargeDetailRecordsLock      = new SemaphoreSlim(1, 1);
        protected readonly     Timer                                                            FlushChargeDetailRecordsTimer;

        protected readonly     Dictionary<EVSE,                    List<PropertyUpdateInfos>>   EVSEsUpdateLog;
        protected readonly     Dictionary<ChargingStation,         List<PropertyUpdateInfos>>   ChargingStationsUpdateLog;
        protected readonly     Dictionary<ChargingPool,            List<PropertyUpdateInfos>>   ChargingPoolsUpdateLog;
        protected readonly     Dictionary<ChargingStationOperator, List<PropertyUpdateInfos>>   ChargingStationOperatorsUpdateLog;
        protected readonly     Dictionary<RoamingNetwork,          List<PropertyUpdateInfos>>   RoamingNetworksUpdateLog;


        protected readonly     IncludeEVSEIdDelegate                                            _IncludeEVSEIds;
        protected readonly     IncludeEVSEDelegate                                              _IncludeEVSEs;
        protected readonly     CustomEVSEIdMapperDelegate                                       CustomEVSEIdMapper;

        protected readonly     HashSet<EVSE>                                                    EVSEsToAddQueue;
        protected readonly     HashSet<EVSE>                                                    EVSEsToUpdateQueue;
        protected readonly     HashSet<EVSE>                                                    EVSEsToRemoveQueue;
        protected readonly     List<EVSEAdminStatusUpdate>                                      EVSEAdminStatusChangesFastQueue;
        protected readonly     List<EVSEAdminStatusUpdate>                                      EVSEAdminStatusChangesDelayedQueue;
        protected readonly     List<EVSEStatusUpdate>                                           EVSEStatusChangesFastQueue;
        protected readonly     List<EVSEStatusUpdate>                                           EVSEStatusChangesDelayedQueue;

        #endregion

        #region Properties

        /// <summary>
        /// The offical (multi-language) name of the charging station operator roaming provider.
        /// </summary>
        [Mandatory]
        public I18NString  Name                              { get; }

        /// <summary>
        /// An optional (multi-language) description of the charging station operator roaming provider.
        /// </summary>
        [Optional]
        public I18NString  Description                       { get; }


        /// <summary>
        /// This service can be disabled, e.g. for debugging reasons.
        /// </summary>
        public Boolean     DisablePushData                   { get; set; }

        /// <summary>
        /// This service can be disabled, e.g. for debugging reasons.
        /// </summary>
        public Boolean     DisablePushAdminStatus            { get; set; }

        /// <summary>
        /// This service can be disabled, e.g. for debugging reasons.
        /// </summary>
        public Boolean     DisablePushStatus                 { get; set; }

        /// <summary>
        /// This service can be disabled, e.g. for debugging reasons.
        /// </summary>
        public Boolean     DisableAuthentication             { get; set; }

        /// <summary>
        /// This service can be disabled, e.g. for debugging reasons.
        /// </summary>
        public Boolean     DisableSendChargeDetailRecords    { get; set; }


        /// <summary>
        /// The attached DNS service.
        /// </summary>
        public DNSClient   DNSClient                         { get; }


        #region FlushEVSEDataEvery

        protected UInt32 _FlushEVSEDataAndStatusEvery;

        /// <summary>
        /// The EVSE data updates transmission intervall.
        /// </summary>
        public TimeSpan FlushEVSEDataEvery
        {

            get
            {
                return TimeSpan.FromSeconds(_FlushEVSEDataAndStatusEvery);
            }

            set
            {
                _FlushEVSEDataAndStatusEvery = (UInt32) value.TotalSeconds;
            }

        }

        #endregion

        #region FlushEVSEFastStatusEvery

        protected UInt32 _FlushEVSEFastStatusEvery;

        /// <summary>
        /// The EVSE status updates transmission intervall.
        /// </summary>
        public TimeSpan FlushEVSEFastStatusEvery
        {

            get
            {
                return TimeSpan.FromSeconds(_FlushEVSEFastStatusEvery);
            }

            set
            {
                _FlushEVSEFastStatusEvery = (UInt32) value.TotalSeconds;
            }

        }

        #endregion

        #region FlushChargeDetailRecordsEvery

        protected UInt32 _FlushChargeDetailRecordsEvery;

        /// <summary>
        /// The charge detail record transmission intervall.
        /// </summary>
        public TimeSpan FlushChargeDetailRecordsEvery
        {

            get
            {
                return TimeSpan.FromMilliseconds(_FlushChargeDetailRecordsEvery);
            }

            set
            {
                _FlushChargeDetailRecordsEvery = (UInt32) value.TotalMilliseconds;
            }

        }

        #endregion

        #endregion

        #region Events

        #region OnWWCPCPOAdapterException

        public delegate Task OnWWCPCPOAdapterExceptionDelegate(DateTime         Timestamp,
                                                               AWWCPCSOAdapter  Sender,
                                                               Exception        Exception);

        public event OnWWCPCPOAdapterExceptionDelegate OnWWCPCPOAdapterException;

        #endregion


        #region FlushEVSEDataAndStatusQueues

        public delegate void FlushEVSEDataAndStatusQueuesStartedDelegate(AWWCPCSOAdapter Sender, DateTime StartTime, TimeSpan Every, UInt64 RunId);

        public event FlushEVSEDataAndStatusQueuesStartedDelegate FlushEVSEDataAndStatusQueuesStartedEvent;

        public delegate void FlushEVSEDataAndStatusQueuesFinishedDelegate(AWWCPCSOAdapter Sender, DateTime StartTime, DateTime EndTime, TimeSpan Runtime, TimeSpan Every, UInt64 RunId);

        public event FlushEVSEDataAndStatusQueuesFinishedDelegate FlushEVSEDataAndStatusQueuesFinishedEvent;

        #endregion

        #region FlushEVSEFastStatusQueues

        public delegate void FlushEVSEFastStatusQueuesStartedDelegate(AWWCPCSOAdapter Sender, DateTime StartTime, TimeSpan Every, UInt64 RunId);

        public event FlushEVSEFastStatusQueuesStartedDelegate FlushEVSEFastStatusQueuesStartedEvent;

        public delegate void FlushEVSEFastStatusQueuesFinishedDelegate(AWWCPCSOAdapter Sender, DateTime StartTime, DateTime EndTime, TimeSpan Runtime, TimeSpan Every, UInt64 RunId);

        public event FlushEVSEFastStatusQueuesFinishedDelegate FlushEVSEFastStatusQueuesFinishedEvent;

        #endregion

        #region FlushChargeDetailRecordsQueues

        public delegate void FlushChargeDetailRecordsQueuesStartedDelegate(AWWCPCSOAdapter Sender, DateTime StartTime, TimeSpan Every, UInt64 RunId);

        public event FlushChargeDetailRecordsQueuesStartedDelegate FlushChargeDetailRecordsQueuesStartedEvent;

        public delegate void FlushChargeDetailRecordsQueuesFinishedDelegate(AWWCPCSOAdapter Sender, DateTime StartTime, DateTime EndTime, TimeSpan Runtime, TimeSpan Every, UInt64 RunId);

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
        /// <param name="IncludeEVSEIds">Only include the EVSE matching the given delegate.</param>
        /// <param name="IncludeEVSEs">Only include the EVSEs matching the given delegate.</param>
        /// <param name="CustomEVSEIdMapper">A delegate to customize the mapping of EVSE identifications.</param>
        /// 
        /// <param name="ServiceCheckEvery">The service check intervall.</param>
        /// <param name="StatusCheckEvery">The status check intervall.</param>
        /// <param name="CDRCheckEvery">The charge detail record intervall.</param>
        /// 
        /// <param name="DisablePushData">This service can be disabled, e.g. for debugging reasons.</param>
        /// <param name="DisablePushStatus">This service can be disabled, e.g. for debugging reasons.</param>
        /// <param name="DisableAuthentication">This service can be disabled, e.g. for debugging reasons.</param>
        /// <param name="DisableSendChargeDetailRecords">This service can be disabled, e.g. for debugging reasons.</param>
        /// 
        /// <param name="PublicKeyRing">The public key ring of the entity.</param>
        /// <param name="SecretKeyRing">The secrect key ring of the entity.</param>
        /// <param name="DNSClient">The attached DNS service.</param>
        public AWWCPCSOAdapter(CSORoamingProvider_Id       Id,
                               I18NString                  Name,
                               I18NString                  Description,
                               RoamingNetwork              RoamingNetwork,

                               IncludeEVSEIdDelegate       IncludeEVSEIds                   = null,
                               IncludeEVSEDelegate         IncludeEVSEs                     = null,
                               CustomEVSEIdMapperDelegate  CustomEVSEIdMapper               = null,

                               TimeSpan?                   ServiceCheckEvery                = null,
                               TimeSpan?                   StatusCheckEvery                 = null,
                               TimeSpan?                   CDRCheckEvery                    = null,

                               Boolean                     DisablePushData                  = false,
                               Boolean                     DisablePushStatus                = false,
                               Boolean                     DisableAuthentication            = false,
                               Boolean                     DisableSendChargeDetailRecords   = false,

                               PgpPublicKeyRing            PublicKeyRing                    = null,
                               PgpSecretKeyRing            SecretKeyRing                    = null,
                               DNSClient                   DNSClient                        = null)

            : base(Id,
                   RoamingNetwork,
                   PublicKeyRing,
                   SecretKeyRing)

        {

            this.Name                                            = Name;
            this.Description                                     = Description;

            this._IncludeEVSEIds                                 = IncludeEVSEIds ?? (evseid => true);
            this._IncludeEVSEs                                   = IncludeEVSEs   ?? (evse   => true);
            this.CustomEVSEIdMapper                              = CustomEVSEIdMapper;

            this.DisablePushData                                 = DisablePushData;
            this.DisablePushStatus                               = DisablePushStatus;
            this.DisableAuthentication                           = DisableAuthentication;
            this.DisableSendChargeDetailRecords                  = DisableSendChargeDetailRecords;

            this._FlushEVSEDataAndStatusEvery                    = (UInt32) (ServiceCheckEvery.HasValue
                                                                      ? ServiceCheckEvery.Value. TotalMilliseconds
                                                                      : DefaultServiceCheckEvery.TotalMilliseconds);

            this.FlushEVSEDataAndStatusTimer                     = new Timer(FlushEVSEDataAndStatus);

            this._FlushEVSEFastStatusEvery                       = (UInt32) (StatusCheckEvery.HasValue
                                                                        ? StatusCheckEvery.Value.  TotalMilliseconds
                                                                        : DefaultStatusCheckEvery. TotalMilliseconds);

            this._FlushChargeDetailRecordsEvery                  = (UInt32) (CDRCheckEvery.HasValue
                                                                        ? CDRCheckEvery.Value.  TotalMilliseconds
                                                                        : DefaultCDRCheckEvery. TotalMilliseconds);

            this.FlushEVSEFastStatusTimer                        = new Timer(FlushEVSEFastStatus);

            this.FlushChargeDetailRecordsTimer                   = new Timer(FlushChargeDetailRecords);

            this.EVSEsUpdateLog                                  = new Dictionary<EVSE,            List<PropertyUpdateInfos>>();
            this.ChargingStationsUpdateLog                       = new Dictionary<ChargingStation, List<PropertyUpdateInfos>>();
            this.ChargingPoolsUpdateLog                          = new Dictionary<ChargingPool,    List<PropertyUpdateInfos>>();

            this.DNSClient                                       = DNSClient;

            this.EVSEsToAddQueue                                  = new HashSet<EVSE>();
            this.EVSEsToUpdateQueue                               = new HashSet<EVSE>();
            this.EVSEsToRemoveQueue                               = new HashSet<EVSE>();
            this.EVSEAdminStatusChangesFastQueue                  = new List<EVSEAdminStatusUpdate>();
            this.EVSEAdminStatusChangesDelayedQueue               = new List<EVSEAdminStatusUpdate>();
            this.EVSEStatusChangesFastQueue                       = new List<EVSEStatusUpdate>();
            this.EVSEStatusChangesDelayedQueue                    = new List<EVSEStatusUpdate>();

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

            SetStaticData(ISendData           Sender,
                          EVSE                EVSE,

                          DateTime?           Timestamp,
                          CancellationToken?  CancellationToken,
                          EventTracking_Id    EventTrackingId,
                          TimeSpan?           RequestTimeout)

        {

            #region Initial checks

            if (EVSE == null)
                return PushEVSEDataResult.NoOperation(Id,
                                                      Sender);

            if (DisablePushData)
                return PushEVSEDataResult.AdminDown(Id,
                                                    Sender,
                                                    new EVSE[] { EVSE });

            #endregion

            #region Send OnEnqueueSendCDRRequest event

            //try
            //{

            //    OnEnqueueSendCDRRequest?.Invoke(DateTime.UtcNow,
            //                                    Timestamp.Value,
            //                                    this,
            //                                    EventTrackingId,
            //                                    RoamingNetwork.Id,
            //                                    ChargeDetailRecord,
            //                                    RequestTimeout);

            //}
            //catch (Exception e)
            //{
            //    e.Log(nameof(WWCPCPOAdapter) + "." + nameof(OnSendCDRRequest));
            //}

            #endregion

            await DataAndStatusLock.WaitAsync();

            try
            {

                if (_IncludeEVSEs == null ||
                   (_IncludeEVSEs != null && _IncludeEVSEs(EVSE)))
                {

                    EVSEsToAddQueue.Add(EVSE);

                    FlushEVSEDataAndStatusTimer.Change(_FlushEVSEDataAndStatusEvery, Timeout.Infinite);

                }

            }
            finally
            {
                DataAndStatusLock.Release();
            }

            return PushEVSEDataResult.Enqueued(Id, Sender);

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

            AddStaticData(ISendData           Sender,
                          EVSE                EVSE,

                          DateTime?           Timestamp,
                          CancellationToken?  CancellationToken,
                          EventTracking_Id    EventTrackingId,
                          TimeSpan?           RequestTimeout)

        {

            #region Initial checks

            if (EVSE == null)
                return PushEVSEDataResult.NoOperation(Id,
                                                      Sender);

            if (DisablePushData)
                return PushEVSEDataResult.AdminDown(Id,
                                                    Sender,
                                                    new EVSE[] { EVSE });

            #endregion

            #region Send OnEnqueueSendCDRRequest event

            //try
            //{

            //    OnEnqueueSendCDRRequest?.Invoke(DateTime.UtcNow,
            //                                    Timestamp.Value,
            //                                    this,
            //                                    EventTrackingId,
            //                                    RoamingNetwork.Id,
            //                                    ChargeDetailRecord,
            //                                    RequestTimeout);

            //}
            //catch (Exception e)
            //{
            //    e.Log(nameof(WWCPCPOAdapter) + "." + nameof(OnSendCDRRequest));
            //}

            #endregion

            await DataAndStatusLock.WaitAsync();

            try
            {

                if (_IncludeEVSEs == null ||
                   (_IncludeEVSEs != null && _IncludeEVSEs(EVSE)))
                {

                    EVSEsToAddQueue.Add(EVSE);

                    FlushEVSEDataAndStatusTimer.Change(_FlushEVSEDataAndStatusEvery, Timeout.Infinite);

                }

            }
            finally
            {
                DataAndStatusLock.Release();
            }

            return PushEVSEDataResult.Enqueued(Id, Sender);

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

            //    OnEnqueueSendCDRRequest?.Invoke(DateTime.UtcNow,
            //                                    Timestamp.Value,
            //                                    this,
            //                                    EventTrackingId,
            //                                    RoamingNetwork.Id,
            //                                    ChargeDetailRecord,
            //                                    RequestTimeout);

            //}
            //catch (Exception e)
            //{
            //    e.Log(nameof(WWCPCPOAdapter) + "." + nameof(OnSendCDRRequest));
            //}

            #endregion

            await DataAndStatusLock.WaitAsync().ConfigureAwait(false);

            try
            {

                var FilteredUpdates = AdminStatusUpdates.Where(adminstatusupdate => _IncludeEVSEs  (adminstatusupdate.EVSE) &&
                                                                                    _IncludeEVSEIds(adminstatusupdate.EVSE.Id)).
                                                         ToArray();

                if (FilteredUpdates.Length > 0)
                {

                    foreach (var Update in FilteredUpdates)
                    {

                        // Delay the status update until the EVSE data had been uploaded!
                        if (!DisablePushData && EVSEsToAddQueue.Any(evse => evse == Update.EVSE))
                            EVSEAdminStatusChangesDelayedQueue.Add(Update);

                        else
                            EVSEAdminStatusChangesFastQueue.Add(Update);

                    }

                    FlushEVSEFastStatusTimer.Change(_FlushEVSEFastStatusEvery, Timeout.Infinite);

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

            //    OnEnqueueSendCDRRequest?.Invoke(DateTime.UtcNow,
            //                                    Timestamp.Value,
            //                                    this,
            //                                    EventTrackingId,
            //                                    RoamingNetwork.Id,
            //                                    ChargeDetailRecord,
            //                                    RequestTimeout);

            //}
            //catch (Exception e)
            //{
            //    e.Log(nameof(WWCPCPOAdapter) + "." + nameof(OnSendCDRRequest));
            //}

            #endregion

            await DataAndStatusLock.WaitAsync().ConfigureAwait(false);

            try
            {

                var FilteredUpdates = StatusUpdates.Where(statusupdate => _IncludeEVSEs  (statusupdate.EVSE) &&
                                                                          _IncludeEVSEIds(statusupdate.EVSE.Id)).
                                                    ToArray();

                if (FilteredUpdates.Length > 0)
                {

                    foreach (var Update in FilteredUpdates)
                    {

                        // Delay the status update until the EVSE data had been uploaded!
                        if (!DisablePushData && EVSEsToAddQueue.Any(evse => evse == Update.EVSE))
                            EVSEStatusChangesDelayedQueue.Add(Update);

                        else
                            EVSEStatusChangesFastQueue.Add(Update);

                    }

                    FlushEVSEFastStatusTimer.Change(_FlushEVSEFastStatusEvery, Timeout.Infinite);

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
                FlushEVSEDataAndStatus2().Wait();
        }

        private async Task FlushEVSEDataAndStatus2()
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

                    var StartTime = DateTime.UtcNow;

                    FlushEVSEDataAndStatusQueuesStartedEvent?.Invoke(this,
                                                                     StartTime,
                                                                     TimeSpan.FromMilliseconds(_FlushEVSEDataAndStatusEvery),
                                                                     _FlushEVSEDataRunId);

                    #endregion

                    await FlushEVSEDataAndStatusQueues();

                    #region Send Finished Event...

                    var EndTime = DateTime.UtcNow;

                    FlushEVSEDataAndStatusQueuesFinishedEvent?.Invoke(this,
                                                                      StartTime,
                                                                      EndTime,
                                                                      EndTime - StartTime,
                                                                      TimeSpan.FromMilliseconds(_FlushEVSEDataAndStatusEvery),
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

                OnWWCPCPOAdapterException?.Invoke(DateTime.UtcNow,
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
                FlushEVSEFastStatus2().Wait();
        }

        private async Task FlushEVSEFastStatus2()
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

                    var StartTime = DateTime.UtcNow;

                    FlushEVSEFastStatusQueuesStartedEvent?.Invoke(this,
                                                                  StartTime,
                                                                  TimeSpan.FromMilliseconds(_FlushEVSEFastStatusEvery),
                                                                  _StatusRunId);

                    #endregion

                    await FlushEVSEFastStatusQueues();

                    #region Send Finished Event...

                    var EndTime = DateTime.UtcNow;

                    FlushEVSEFastStatusQueuesFinishedEvent?.Invoke(this,
                                                                   StartTime,
                                                                   EndTime,
                                                                   EndTime - StartTime,
                                                                   TimeSpan.FromMilliseconds(_FlushEVSEFastStatusEvery),
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

                OnWWCPCPOAdapterException?.Invoke(DateTime.UtcNow,
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

        protected abstract Task     FlushChargeDetailRecordsQueues();

        private void FlushChargeDetailRecords(Object State)
        {
            if (!DisableSendChargeDetailRecords)
                FlushChargeDetailRecords2().Wait();
        }

        private async Task FlushChargeDetailRecords2()
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

                    var StartTime = DateTime.UtcNow;

                    FlushChargeDetailRecordsQueuesStartedEvent?.Invoke(this,
                                                                       StartTime,
                                                                       TimeSpan.FromMilliseconds(_FlushEVSEFastStatusEvery),
                                                                       _CDRRunId);

                    #endregion

                    await FlushChargeDetailRecordsQueues();

                    #region Send Finished Event...

                    var EndTime = DateTime.UtcNow;

                    FlushChargeDetailRecordsQueuesFinishedEvent?.Invoke(this,
                                                                        StartTime,
                                                                        EndTime,
                                                                        EndTime - StartTime,
                                                                        TimeSpan.FromMilliseconds(_FlushEVSEFastStatusEvery),
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

                OnWWCPCPOAdapterException?.Invoke(DateTime.UtcNow,
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

            if (Warnings != null && Warnings.Any())
                OnWarnings?.Invoke(Timestamp,
                                   Class,
                                   Method,
                                   Warnings);

        }

    }

}
