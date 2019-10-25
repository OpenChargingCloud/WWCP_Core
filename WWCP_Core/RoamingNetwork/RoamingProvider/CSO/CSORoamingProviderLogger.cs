///*
// * Copyright (c) 2014-2019 GraphDefined GmbH
// * This file is part of WWCP OICP <https://github.com/OpenChargingCloud/WWCP_OICP>
// *
// * Licensed under the Apache License, Version 2.0 (the "License");
// * you may not use this file except in compliance with the License.
// * You may obtain a copy of the License at
// *
// *     http://www.apache.org/licenses/LICENSE-2.0
// *
// * Unless required by applicable law or agreed to in writing, software
// * distributed under the License is distributed on an "AS IS" BASIS,
// * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// * See the License for the specific language governing permissions and
// * limitations under the License.
// */

//#region Usings

//using System;
//using System.Linq;
//using System.Threading;
//using System.Threading.Tasks;
//using System.Collections.Generic;
//using System.Text.RegularExpressions;

//using org.GraphDefined.Vanaheimr.Illias;
//using org.GraphDefined.Vanaheimr.Hermod.DNS;

//#endregion

//namespace org.GraphDefined.WWCP
//{

//    /// <summary>
//    /// A WWCP wrapper for the OICP CPO Roaming client which maps
//    /// WWCP data structures onto OICP data structures and vice versa.
//    /// </summary>
//    public class CSORoamingProviderLogger : AWWCPCSOAdapter,
//                                            ICSORoamingProvider,
//                                            IEquatable <CSORoamingProviderLogger>,
//                                            IComparable<CSORoamingProviderLogger>,
//                                            IComparable
//    {

//        #region Data

//        private        readonly  ChargingStationOperatorNameSelectorDelegate   _OperatorNameSelector;

//        private static readonly  Regex                                         pattern                      = new Regex(@"\s=\s");

//        public  static readonly  ChargingStationOperatorNameSelectorDelegate   DefaultOperatorNameSelector  = I18N => I18N.FirstText();

//        private        readonly  HashSet<EVSE>                                 EVSEsToAddQueue;
//        private        readonly  HashSet<EVSE>                                 EVSEsToUpdateQueue;
//        private        readonly  List<EVSEStatusUpdate>                        EVSEStatusChangesFastQueue;
//        private        readonly  List<EVSEStatusUpdate>                        EVSEStatusChangesDelayedQueue;
//        private        readonly  HashSet<EVSE>                                 EVSEsToRemoveQueue;
//        private        readonly  List<ChargeDetailRecord>                      ChargeDetailRecordQueue;

//        public  static readonly  TimeSpan                                      DefaultRequestTimeout        = TimeSpan.FromSeconds(30);

//        #endregion

//        #region Properties

//        IId ISendAuthorizeStartStop.AuthId
//            => Id;

//        IId ISendChargeDetailRecords.Id
//            => Id;

//        IEnumerable<IId> ISendChargeDetailRecords.Ids
//            => Ids.Cast<IId>();

//        #endregion

//        #region Events

//        // Client logging...

//        #region OnPushEVSEDataWWCPRequest/-Response

//        ///// <summary>
//        ///// An event fired whenever new EVSE data will be send upstream.
//        ///// </summary>
//        //public event OnPushEVSEDataWWCPRequestDelegate   OnPushEVSEDataWWCPRequest;

//        ///// <summary>
//        ///// An event fired whenever new EVSE data had been sent upstream.
//        ///// </summary>
//        //public event OnPushEVSEDataWWCPResponseDelegate  OnPushEVSEDataWWCPResponse;

//        #endregion

//        #region OnPushEVSEStatusWWCPRequest/-Response

//        ///// <summary>
//        ///// An event fired whenever new EVSE status will be send upstream.
//        ///// </summary>
//        //public event OnPushEVSEStatusWWCPRequestDelegate   OnPushEVSEStatusWWCPRequest;

//        ///// <summary>
//        ///// An event fired whenever new EVSE status had been sent upstream.
//        ///// </summary>
//        //public event OnPushEVSEStatusWWCPResponseDelegate  OnPushEVSEStatusWWCPResponse;

//        #endregion


//        #region OnAuthorizeStartRequest/-Response

//        /// <summary>
//        /// An event fired whenever an authentication token will be verified for charging.
//        /// </summary>
//        public event OnAuthorizeStartRequestDelegate                  OnAuthorizeStartRequest;

//        /// <summary>
//        /// An event fired whenever an authentication token had been verified for charging.
//        /// </summary>
//        public event OnAuthorizeStartResponseDelegate                 OnAuthorizeStartResponse;


//        /// <summary>
//        /// An event fired whenever an authentication token will be verified for charging at the given EVSE.
//        /// </summary>
//        public event OnAuthorizeEVSEStartRequestDelegate              OnAuthorizeEVSEStartRequest;

//        /// <summary>
//        /// An event fired whenever an authentication token had been verified for charging at the given EVSE.
//        /// </summary>
//        public event OnAuthorizeEVSEStartResponseDelegate             OnAuthorizeEVSEStartResponse;


//        /// <summary>
//        /// An event fired whenever an authentication token will be verified for charging at the given charging station.
//        /// </summary>
//        public event OnAuthorizeChargingStationStartRequestDelegate   OnAuthorizeChargingStationStartRequest;

//        /// <summary>
//        /// An event fired whenever an authentication token had been verified for charging at the given charging station.
//        /// </summary>
//        public event OnAuthorizeChargingStationStartResponseDelegate  OnAuthorizeChargingStationStartResponse;


//        /// <summary>
//        /// An event fired whenever an authentication token will be verified for charging at the given charging pool.
//        /// </summary>
//        public event OnAuthorizeChargingPoolStartRequestDelegate      OnAuthorizeChargingPoolStartRequest;

//        /// <summary>
//        /// An event fired whenever an authentication token had been verified for charging at the given charging pool.
//        /// </summary>
//        public event OnAuthorizeChargingPoolStartResponseDelegate     OnAuthorizeChargingPoolStartResponse;

//        #endregion

//        #region OnAuthorizeStopRequest/-Response

//        /// <summary>
//        /// An event fired whenever an authentication token will be verified to stop a charging process.
//        /// </summary>
//        public event OnAuthorizeStopRequestDelegate                  OnAuthorizeStopRequest;

//        /// <summary>
//        /// An event fired whenever an authentication token had been verified to stop a charging process.
//        /// </summary>
//        public event OnAuthorizeStopResponseDelegate                 OnAuthorizeStopResponse;


//        /// <summary>
//        /// An event fired whenever an authentication token will be verified to stop a charging process at the given EVSE.
//        /// </summary>
//        public event OnAuthorizeEVSEStopRequestDelegate              OnAuthorizeEVSEStopRequest;

//        /// <summary>
//        /// An event fired whenever an authentication token had been verified to stop a charging process at the given EVSE.
//        /// </summary>
//        public event OnAuthorizeEVSEStopResponseDelegate             OnAuthorizeEVSEStopResponse;


//        /// <summary>
//        /// An event fired whenever an authentication token will be verified to stop a charging process at the given charging station.
//        /// </summary>
//        public event OnAuthorizeChargingStationStopRequestDelegate   OnAuthorizeChargingStationStopRequest;

//        /// <summary>
//        /// An event fired whenever an authentication token had been verified to stop a charging process at the given charging station.
//        /// </summary>
//        public event OnAuthorizeChargingStationStopResponseDelegate  OnAuthorizeChargingStationStopResponse;


//        /// <summary>
//        /// An event fired whenever an authentication token will be verified to stop a charging process at the given charging pool.
//        /// </summary>
//        public event OnAuthorizeChargingPoolStopRequestDelegate      OnAuthorizeChargingPoolStopRequest;

//        /// <summary>
//        /// An event fired whenever an authentication token had been verified to stop a charging process at the given charging pool.
//        /// </summary>
//        public event OnAuthorizeChargingPoolStopResponseDelegate     OnAuthorizeChargingPoolStopResponse;

//        #endregion

//        #region OnSendCDRRequest/-Response

//        /// <summary>
//        /// An event fired whenever a charge detail record was enqueued for later sending upstream.
//        /// </summary>
//        public event OnSendCDRRequestDelegate   OnEnqueueSendCDRsRequest;

//        /// <summary>
//        /// An event fired whenever a charge detail record will be send upstream.
//        /// </summary>
//        public event OnSendCDRRequestDelegate   OnSendCDRsRequest;

//        /// <summary>
//        /// An event fired whenever a charge detail record had been sent upstream.
//        /// </summary>
//        public event OnSendCDRResponseDelegate  OnSendCDRsResponse;

//        #endregion

//        #endregion

//        #region Constructor(s)

//        /// <summary>
//        /// Create a new WWCP wrapper for the OICP roaming client for Charging Station Operators/CPOs.
//        /// </summary>
//        /// <param name="Id">The unique identification of the roaming provider.</param>
//        /// <param name="Name">The offical (multi-language) name of the roaming provider.</param>
//        /// <param name="Description">An optional (multi-language) description of the charging station operator roaming provider.</param>
//        /// <param name="RoamingNetwork">A WWCP roaming network.</param>
//        /// 
//        /// <param name="IncludeEVSEIds">Only include the EVSE matching the given delegate.</param>
//        /// <param name="IncludeEVSEs">Only include the EVSEs matching the given delegate.</param>
//        /// <param name="CustomEVSEIdMapper">A delegate to customize the mapping of EVSE identifications.</param>
//        /// 
//        /// <param name="ServiceCheckEvery">The service check intervall.</param>
//        /// <param name="StatusCheckEvery">The status check intervall.</param>
//        /// <param name="CDRCheckEvery">The charge detail record intervall.</param>
//        /// 
//        /// <param name="DisablePushData">This service can be disabled, e.g. for debugging reasons.</param>
//        /// <param name="DisablePushStatus">This service can be disabled, e.g. for debugging reasons.</param>
//        /// <param name="DisableAuthentication">This service can be disabled, e.g. for debugging reasons.</param>
//        /// <param name="DisableSendChargeDetailRecords">This service can be disabled, e.g. for debugging reasons.</param>
//        /// 
//        /// <param name="DNSClient">The attached DNS service.</param>
//        public CSORoamingProviderLogger(CSORoamingProvider_Id       Id,
//                                        I18NString                  Name,
//                                        I18NString                  Description,
//                                        RoamingNetwork              RoamingNetwork,

//                                        IncludeEVSEIdDelegate       IncludeEVSEIds                   = null,
//                                        IncludeEVSEDelegate         IncludeEVSEs                     = null,
//                                        CustomEVSEIdMapperDelegate  CustomEVSEIdMapper               = null,

//                                        TimeSpan?                   ServiceCheckEvery                = null,
//                                        TimeSpan?                   StatusCheckEvery                 = null,
//                                        TimeSpan?                   CDRCheckEvery                    = null,

//                                        Boolean                     DisablePushData                  = false,
//                                        Boolean                     DisablePushStatus                = false,
//                                        Boolean                     DisableAuthentication            = false,
//                                        Boolean                     DisableSendChargeDetailRecords   = false,

//                                        DNSClient                   DNSClient                        = null)

//            : base(Id,
//                   Name,
//                   Description,
//                   RoamingNetwork,

//                   IncludeEVSEIds,
//                   IncludeEVSEs,
//                   //CustomEVSEIdMapper,

//                   ServiceCheckEvery,
//                   StatusCheckEvery,
//                   CDRCheckEvery,

//                   DisablePushData,
//                   DisablePushStatus,
//                   DisableAuthentication,
//                   DisableSendChargeDetailRecords,

//                   DNSClient: DNSClient)

//        {

//            #region Initial checks

//            if (Name.IsNullOrEmpty())
//                throw new ArgumentNullException(nameof(Name),        "The given roaming provider name must not be null or empty!");

//            #endregion

//            this.EVSEsToAddQueue                = new HashSet<EVSE>();
//            this.EVSEsToUpdateQueue             = new HashSet<EVSE>();
//            this.EVSEStatusChangesFastQueue     = new List<EVSEStatusUpdate>();
//            this.EVSEStatusChangesDelayedQueue  = new List<EVSEStatusUpdate>();
//            this.EVSEsToRemoveQueue             = new HashSet<EVSE>();
//            this.ChargeDetailRecordQueue        = new List<ChargeDetailRecord>();

//        }

//        #endregion


//        // RN -> External service requests...

//        #region PushEVSEData/-Status directly...

//        #region (Set/Add/Update/Delete) EVSE(s)...

//        #region SetStaticData   (EVSE, TransmissionType = Enqueue, ...)

//        /// <summary>
//        /// Set the given EVSE as new static EVSE data at the OICP server.
//        /// </summary>
//        /// <param name="EVSE">An EVSE to upload.</param>
//        /// <param name="TransmissionType">Whether to send the EVSE directly or enqueue it for a while.</param>
//        /// 
//        /// <param name="Timestamp">The optional timestamp of the request.</param>
//        /// <param name="CancellationToken">An optional token to cancel this request.</param>
//        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
//        /// <param name="RequestTimeout">An optional timeout for this request.</param>
//        Task<PushEVSEDataResult>

//            ISendData.SetStaticData(EVSE                EVSE,
//                                    TransmissionTypes   TransmissionType,

//                                    DateTime?           Timestamp,
//                                    CancellationToken?  CancellationToken,
//                                    EventTracking_Id    EventTrackingId,
//                                    TimeSpan?           RequestTimeout)

//        {

//            #region Initial checks

//            if (EVSE == null)
//                throw new ArgumentNullException(nameof(EVSE), "The given EVSE must not be null!");

//            #endregion

//            #region Enqueue, if requested...

//            if (TransmissionType == TransmissionTypes.Enqueue)
//            {

//                #region Send OnEnqueueSendCDRRequest event

//                //try
//                //{

//                //    OnEnqueueSendCDRRequest?.Invoke(StartTime,
//                //                                    Timestamp.Value,
//                //                                    this,
//                //                                    EventTrackingId,
//                //                                    RoamingNetwork.Id,
//                //                                    ChargeDetailRecord,
//                //                                    RequestTimeout);

//                //}
//                //catch (Exception e)
//                //{
//                //    e.Log(nameof(CSORoamingProviderLogger) + "." + nameof(OnSendCDRRequest));
//                //}

//                #endregion

//                //var LockTaken = await DataAndStatusLock.WaitAsync(0);
//                lock(DataAndStatusLockOld)
//                {

//                    if (IncludeEVSEs == null ||
//                       (IncludeEVSEs != null && IncludeEVSEs(EVSE)))
//                    {

//                        EVSEsToAddQueue.Add(EVSE);

//                        FlushEVSEDataAndStatusTimer.Change(FlushEVSEDataAndStatusEvery, TimeSpan.FromMilliseconds(-1));

//                    }

//                }

//                return Task.FromResult(PushEVSEDataResult.Enqueued(Id, this));

//            }

//            #endregion

//            return Task.FromResult(PushEVSEDataResult.Success(Id, this));

//        }

//        #endregion

//        #region AddStaticData   (EVSE, TransmissionType = Enqueue, ...)

//        /// <summary>
//        /// Add the given EVSE to the static EVSE data at the OICP server.
//        /// </summary>
//        /// <param name="EVSE">An EVSE to upload.</param>
//        /// <param name="TransmissionType">Whether to send the EVSE directly or enqueue it for a while.</param>
//        /// 
//        /// <param name="Timestamp">The optional timestamp of the request.</param>
//        /// <param name="CancellationToken">An optional token to cancel this request.</param>
//        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
//        /// <param name="RequestTimeout">An optional timeout for this request.</param>
//        Task<PushEVSEDataResult>

//            ISendData.AddStaticData(EVSE                EVSE,
//                                    TransmissionTypes   TransmissionType,

//                                    DateTime?           Timestamp,
//                                    CancellationToken?  CancellationToken,
//                                    EventTracking_Id    EventTrackingId,
//                                    TimeSpan?           RequestTimeout)

//        {

//            #region Initial checks

//            if (EVSE == null)
//                throw new ArgumentNullException(nameof(EVSE), "The given EVSE must not be null!");

//            #endregion

//            #region Enqueue, if requested...

//            if (TransmissionType == TransmissionTypes.Enqueue)
//            {

//                #region Send OnEnqueueSendCDRRequest event

//                //try
//                //{

//                //    OnEnqueueSendCDRRequest?.Invoke(StartTime,
//                //                                    Timestamp.Value,
//                //                                    this,
//                //                                    EventTrackingId,
//                //                                    RoamingNetwork.Id,
//                //                                    ChargeDetailRecord,
//                //                                    RequestTimeout);

//                //}
//                //catch (Exception e)
//                //{
//                //    e.Log(nameof(CSORoamingProviderLogger) + "." + nameof(OnSendCDRRequest));
//                //}

//                #endregion

//                lock(DataAndStatusLockOld)
//                {

//                    if (IncludeEVSEs == null ||
//                       (IncludeEVSEs != null && IncludeEVSEs(EVSE)))
//                    {

//                        EVSEsToAddQueue.Add(EVSE);

//                        FlushEVSEDataAndStatusTimer.Change(FlushEVSEDataAndStatusEvery, TimeSpan.FromMilliseconds(-1));

//                    }

//                }

//                return Task.FromResult(PushEVSEDataResult.Enqueued(Id,
//                                                               this));

//            }

//            #endregion

//            return Task.FromResult(PushEVSEDataResult.Success(Id, this));

//        }

//        #endregion

//        #region UpdateStaticData(EVSE, PropertyName = null, OldValue = null, NewValue = null, TransmissionType = Enqueue, ...)

//        /// <summary>
//        /// Update the static data of the given EVSE.
//        /// The EVSE can be uploaded as a whole, or just a single property of the EVSE.
//        /// </summary>
//        /// <param name="EVSE">An EVSE to update.</param>
//        /// <param name="PropertyName">The name of the EVSE property to update.</param>
//        /// <param name="OldValue">The old value of the EVSE property to update.</param>
//        /// <param name="NewValue">The new value of the EVSE property to update.</param>
//        /// <param name="TransmissionType">Whether to send the EVSE update directly or enqueue it for a while.</param>
//        /// 
//        /// <param name="Timestamp">The optional timestamp of the request.</param>
//        /// <param name="CancellationToken">An optional token to cancel this request.</param>
//        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
//        /// <param name="RequestTimeout">An optional timeout for this request.</param>
//        Task<PushEVSEDataResult>

//            ISendData.UpdateStaticData(EVSE                EVSE,
//                                       String              PropertyName,
//                                       Object              OldValue,
//                                       Object              NewValue,
//                                       TransmissionTypes   TransmissionType,

//                                       DateTime?           Timestamp,
//                                       CancellationToken?  CancellationToken,
//                                       EventTracking_Id    EventTrackingId,
//                                       TimeSpan?           RequestTimeout)

//        {

//            #region Initial checks

//            if (EVSE         == null)
//                throw new ArgumentNullException(nameof(EVSE),          "The given EVSE must not be null!");

//            if (PropertyName == null)
//                throw new ArgumentNullException(nameof(PropertyName),  "The given EVSE property name must not be null!");

//            #endregion

//            #region Enqueue, if requested...

//            if (TransmissionType == TransmissionTypes.Enqueue)
//            {

//                #region Send OnEnqueueSendCDRRequest event

//                //try
//                //{

//                //    OnEnqueueSendCDRRequest?.Invoke(StartTime,
//                //                                    Timestamp.Value,
//                //                                    this,
//                //                                    EventTrackingId,
//                //                                    RoamingNetwork.Id,
//                //                                    ChargeDetailRecord,
//                //                                    RequestTimeout);

//                //}
//                //catch (Exception e)
//                //{
//                //    e.Log(nameof(CSORoamingProviderLogger) + "." + nameof(OnSendCDRRequest));
//                //}

//                #endregion

//                lock(DataAndStatusLockOld)
//                {

//                    if (IncludeEVSEs == null ||
//                       (IncludeEVSEs != null && IncludeEVSEs(EVSE)))
//                    {

//                        if (EVSEsUpdateLog.TryGetValue(EVSE, out List<PropertyUpdateInfos> PropertyUpdateInfo))
//                            PropertyUpdateInfo.Add(new PropertyUpdateInfos(PropertyName, OldValue, NewValue));

//                        else
//                        {

//                            var List = new List<PropertyUpdateInfos>();
//                            List.Add(new PropertyUpdateInfos(PropertyName, OldValue, NewValue));
//                            EVSEsUpdateLog.Add(EVSE, List);

//                        }

//                        EVSEsToUpdateQueue.Add(EVSE);

//                        FlushEVSEDataAndStatusTimer.Change(FlushEVSEDataAndStatusEvery, TimeSpan.FromMilliseconds(-1));

//                    }

//                }

//                return Task.FromResult(PushEVSEDataResult.Enqueued(Id,
//                                                               this));

//            }

//            #endregion

//            return Task.FromResult(PushEVSEDataResult.Success(Id, this));

//        }

//        #endregion

//        #region DeleteStaticData(EVSE, TransmissionType = Enqueue, ...)

//        /// <summary>
//        /// Delete the static data of the given EVSE.
//        /// </summary>
//        /// <param name="EVSE">An EVSE to delete.</param>
//        /// <param name="TransmissionType">Whether to send the EVSE deletion directly or enqueue it for a while.</param>
//        /// 
//        /// <param name="Timestamp">The optional timestamp of the request.</param>
//        /// <param name="CancellationToken">An optional token to cancel this request.</param>
//        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
//        /// <param name="RequestTimeout">An optional timeout for this request.</param>
//        Task<PushEVSEDataResult>

//            ISendData.DeleteStaticData(EVSE                EVSE,
//                                       TransmissionTypes   TransmissionType,

//                                       DateTime?           Timestamp,
//                                       CancellationToken?  CancellationToken,
//                                       EventTracking_Id    EventTrackingId,
//                                       TimeSpan?           RequestTimeout)

//        {

//            #region Initial checks

//            if (EVSE == null)
//                throw new ArgumentNullException(nameof(EVSE), "The given EVSE must not be null!");

//            #endregion

//            #region Enqueue, if requested...

//            if (TransmissionType == TransmissionTypes.Enqueue)
//            {

//                #region Send OnEnqueueSendCDRRequest event

//                //try
//                //{

//                //    OnEnqueueSendCDRRequest?.Invoke(StartTime,
//                //                                    Timestamp.Value,
//                //                                    this,
//                //                                    EventTrackingId,
//                //                                    RoamingNetwork.Id,
//                //                                    ChargeDetailRecord,
//                //                                    RequestTimeout);

//                //}
//                //catch (Exception e)
//                //{
//                //    e.Log(nameof(CSORoamingProviderLogger) + "." + nameof(OnSendCDRRequest));
//                //}

//                #endregion

//                lock(DataAndStatusLockOld)
//                {

//                    if (IncludeEVSEs == null ||
//                       (IncludeEVSEs != null && IncludeEVSEs(EVSE)))
//                    {

//                        EVSEsToRemoveQueue.Add(EVSE);

//                        FlushEVSEDataAndStatusTimer.Change(FlushEVSEDataAndStatusEvery, TimeSpan.FromMilliseconds(-1));

//                    }

//                }

//                return Task.FromResult(PushEVSEDataResult.Enqueued(Id,
//                                                               this));

//            }

//            #endregion

//            return Task.FromResult(PushEVSEDataResult.Success(Id, this));

//        }

//        #endregion


//        #region SetStaticData   (EVSEs, TransmissionType = Enqueue, ...)

//        /// <summary>
//        /// Set the given enumeration of EVSEs as new static EVSE data at the OICP server.
//        /// </summary>
//        /// <param name="EVSEs">An enumeration of EVSEs.</param>
//        /// <param name="TransmissionType">Whether to send the EVSE directly or enqueue it for a while.</param>
//        /// 
//        /// <param name="Timestamp">The optional timestamp of the request.</param>
//        /// <param name="CancellationToken">An optional token to cancel this request.</param>
//        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
//        /// <param name="RequestTimeout">An optional timeout for this request.</param>
//        Task<PushEVSEDataResult>

//            ISendData.SetStaticData(IEnumerable<EVSE>   EVSEs,
//                                    TransmissionTypes   TransmissionType,

//                                    DateTime?           Timestamp,
//                                    CancellationToken?  CancellationToken,
//                                    EventTracking_Id    EventTrackingId,
//                                    TimeSpan?           RequestTimeout)

//        {

//            #region Initial checks

//            if (EVSEs == null || !EVSEs.Any())
//                return Task.FromResult(PushEVSEDataResult.NoOperation(Id, this));

//            #endregion

//            #region Enqueue, if requested...

//            if (TransmissionType == TransmissionTypes.Enqueue)
//            {

//                #region Send OnEnqueueSendCDRRequest event

//                //try
//                //{

//                //    OnEnqueueSendCDRRequest?.Invoke(StartTime,
//                //                                    Timestamp.Value,
//                //                                    this,
//                //                                    EventTrackingId,
//                //                                    RoamingNetwork.Id,
//                //                                    ChargeDetailRecord,
//                //                                    RequestTimeout);

//                //}
//                //catch (Exception e)
//                //{
//                //    e.Log(nameof(CSORoamingProviderLogger) + "." + nameof(OnSendCDRRequest));
//                //}

//                #endregion

//                lock(DataAndStatusLockOld)
//                {

//                    var FilteredEVSEs = EVSEs.Where(evse => IncludeEVSEs  (evse) &&
//                                                            IncludeEVSEIds(evse.Id)).
//                                              ToArray();

//                    if (FilteredEVSEs.Any())
//                    {

//                        foreach (var EVSE in FilteredEVSEs)
//                            EVSEsToAddQueue.Add(EVSE);

//                        FlushEVSEDataAndStatusTimer.Change(FlushEVSEDataAndStatusEvery, TimeSpan.FromMilliseconds(-1));

//                        return Task.FromResult(PushEVSEDataResult.Enqueued(Id, this));

//                    }

//                    return Task.FromResult(PushEVSEDataResult.NoOperation(Id, this));

//                }

//            }

//            #endregion

//            return Task.FromResult(PushEVSEDataResult.Success(Id, this));

//        }

//        #endregion

//        #region AddStaticData   (EVSEs, TransmissionType = Enqueue, ...)

//        /// <summary>
//        /// Add the given enumeration of EVSEs to the static EVSE data at the OICP server.
//        /// </summary>
//        /// <param name="EVSEs">An enumeration of EVSEs.</param>
//        /// <param name="TransmissionType">Whether to send the EVSE directly or enqueue it for a while.</param>
//        /// 
//        /// <param name="Timestamp">The optional timestamp of the request.</param>
//        /// <param name="CancellationToken">An optional token to cancel this request.</param>
//        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
//        /// <param name="RequestTimeout">An optional timeout for this request.</param>
//        Task<PushEVSEDataResult>

//            ISendData.AddStaticData(IEnumerable<EVSE>   EVSEs,
//                                    TransmissionTypes   TransmissionType,

//                                    DateTime?           Timestamp,
//                                    CancellationToken?  CancellationToken,
//                                    EventTracking_Id    EventTrackingId,
//                                    TimeSpan?           RequestTimeout)

//        {

//            #region Initial checks

//            if (EVSEs == null || !EVSEs.Any())
//                return Task.FromResult(PushEVSEDataResult.NoOperation(Id, this));

//            #endregion

//            #region Enqueue, if requested...

//            if (TransmissionType == TransmissionTypes.Enqueue)
//            {

//                #region Send OnEnqueueSendCDRRequest event

//                //try
//                //{

//                //    OnEnqueueSendCDRRequest?.Invoke(StartTime,
//                //                                    Timestamp.Value,
//                //                                    this,
//                //                                    EventTrackingId,
//                //                                    RoamingNetwork.Id,
//                //                                    ChargeDetailRecord,
//                //                                    RequestTimeout);

//                //}
//                //catch (Exception e)
//                //{
//                //    e.Log(nameof(CSORoamingProviderLogger) + "." + nameof(OnSendCDRRequest));
//                //}

//                #endregion

//                lock(DataAndStatusLockOld)
//                {

//                    var FilteredEVSEs = EVSEs.Where(evse => IncludeEVSEs  (evse) &&
//                                                            IncludeEVSEIds(evse.Id)).
//                                              ToArray();

//                    if (FilteredEVSEs.Any())
//                    {

//                        foreach (var EVSE in FilteredEVSEs)
//                            EVSEsToAddQueue.Add(EVSE);

//                        FlushEVSEDataAndStatusTimer.Change(FlushEVSEDataAndStatusEvery, TimeSpan.FromMilliseconds(-1));

//                        return Task.FromResult(PushEVSEDataResult.Enqueued(Id, this));

//                    }

//                    return Task.FromResult(PushEVSEDataResult.NoOperation(Id, this));

//                }

//            }

//            #endregion

//            return Task.FromResult(PushEVSEDataResult.Success(Id, this));

//        }

//        #endregion

//        #region UpdateStaticData(EVSEs, TransmissionType = Enqueue, ...)

//        /// <summary>
//        /// Update the given enumeration of EVSEs within the static EVSE data at the OICP server.
//        /// </summary>
//        /// <param name="EVSEs">An enumeration of EVSEs.</param>
//        /// <param name="TransmissionType">Whether to send the EVSE directly or enqueue it for a while.</param>
//        /// 
//        /// <param name="Timestamp">The optional timestamp of the request.</param>
//        /// <param name="CancellationToken">An optional token to cancel this request.</param>
//        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
//        /// <param name="RequestTimeout">An optional timeout for this request.</param>
//        Task<PushEVSEDataResult>

//            ISendData.UpdateStaticData(IEnumerable<EVSE>   EVSEs,
//                                       TransmissionTypes   TransmissionType,

//                                       DateTime?           Timestamp,
//                                       CancellationToken?  CancellationToken,
//                                       EventTracking_Id    EventTrackingId,
//                                       TimeSpan?           RequestTimeout)

//        {

//            #region Initial checks

//            if (EVSEs == null || !EVSEs.Any())
//                return Task.FromResult(PushEVSEDataResult.NoOperation(Id, this));

//            #endregion

//            #region Enqueue, if requested...

//            if (TransmissionType == TransmissionTypes.Enqueue)
//            {

//                #region Send OnEnqueueSendCDRRequest event

//                //try
//                //{

//                //    OnEnqueueSendCDRRequest?.Invoke(StartTime,
//                //                                    Timestamp.Value,
//                //                                    this,
//                //                                    EventTrackingId,
//                //                                    RoamingNetwork.Id,
//                //                                    ChargeDetailRecord,
//                //                                    RequestTimeout);

//                //}
//                //catch (Exception e)
//                //{
//                //    e.Log(nameof(CSORoamingProviderLogger) + "." + nameof(OnSendCDRRequest));
//                //}

//                #endregion

//                lock(DataAndStatusLockOld)
//                {

//                    var FilteredEVSEs = EVSEs.Where(evse => IncludeEVSEs  (evse) &&
//                                                            IncludeEVSEIds(evse.Id)).
//                                              ToArray();

//                    if (FilteredEVSEs.Any())
//                    {

//                        foreach (var EVSE in FilteredEVSEs)
//                            EVSEsToUpdateQueue.Add(EVSE);

//                        FlushEVSEDataAndStatusTimer.Change(FlushEVSEDataAndStatusEvery, TimeSpan.FromMilliseconds(-1));

//                        return Task.FromResult(PushEVSEDataResult.Enqueued(Id, this));

//                    }

//                    return Task.FromResult(PushEVSEDataResult.NoOperation(Id, this));

//                }

//            }

//            #endregion

//            return Task.FromResult(PushEVSEDataResult.Success(Id, this));

//        }

//        #endregion

//        #region DeleteStaticData(EVSEs, TransmissionType = Enqueue, ...)

//        /// <summary>
//        /// Delete the given enumeration of EVSEs from the static EVSE data at the OICP server.
//        /// </summary>
//        /// <param name="EVSEs">An enumeration of EVSEs.</param>
//        /// <param name="TransmissionType">Whether to send the EVSE directly or enqueue it for a while.</param>
//        /// 
//        /// <param name="Timestamp">The optional timestamp of the request.</param>
//        /// <param name="CancellationToken">An optional token to cancel this request.</param>
//        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
//        /// <param name="RequestTimeout">An optional timeout for this request.</param>
//        Task<PushEVSEDataResult>

//            ISendData.DeleteStaticData(IEnumerable<EVSE>   EVSEs,
//                                       TransmissionTypes   TransmissionType,

//                                       DateTime?           Timestamp,
//                                       CancellationToken?  CancellationToken,
//                                       EventTracking_Id    EventTrackingId,
//                                       TimeSpan?           RequestTimeout)

//        {

//            #region Initial checks

//            if (EVSEs == null || !EVSEs.Any())
//                return Task.FromResult(PushEVSEDataResult.NoOperation(Id, this));

//            #endregion

//            #region Enqueue, if requested...

//            if (TransmissionType == TransmissionTypes.Enqueue)
//            {

//                #region Send OnEnqueueSendCDRRequest event

//                //try
//                //{

//                //    OnEnqueueSendCDRRequest?.Invoke(StartTime,
//                //                                    Timestamp.Value,
//                //                                    this,
//                //                                    EventTrackingId,
//                //                                    RoamingNetwork.Id,
//                //                                    ChargeDetailRecord,
//                //                                    RequestTimeout);

//                //}
//                //catch (Exception e)
//                //{
//                //    e.Log(nameof(CSORoamingProviderLogger) + "." + nameof(OnSendCDRRequest));
//                //}

//                #endregion

//                lock(DataAndStatusLockOld)
//                {

//                    var FilteredEVSEs = EVSEs.Where(evse => IncludeEVSEs  (evse) &&
//                                                            IncludeEVSEIds(evse.Id)).
//                                              ToArray();

//                    if (FilteredEVSEs.Any())
//                    {

//                        foreach (var EVSE in FilteredEVSEs)
//                            EVSEsToRemoveQueue.Add(EVSE);

//                        FlushEVSEDataAndStatusTimer.Change(FlushEVSEDataAndStatusEvery, TimeSpan.FromMilliseconds(-1));

//                        return Task.FromResult(PushEVSEDataResult.Enqueued(Id, this));

//                    }

//                    return Task.FromResult(PushEVSEDataResult.NoOperation(Id, this));

//                }

//            }

//            #endregion

//            return Task.FromResult(PushEVSEDataResult.Success(Id, this));

//        }

//        #endregion


//        #region UpdateAdminStatus(AdminStatusUpdates, TransmissionType = Enqueue, ...)

//        /// <summary>
//        /// Update the given enumeration of EVSE admin status updates.
//        /// </summary>
//        /// <param name="AdminStatusUpdates">An enumeration of EVSE admin status updates.</param>
//        /// <param name="TransmissionType">Whether to send the EVSE admin status updates directly or enqueue it for a while.</param>
//        /// 
//        /// <param name="Timestamp">The optional timestamp of the request.</param>
//        /// <param name="CancellationToken">An optional token to cancel this request.</param>
//        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
//        /// <param name="RequestTimeout">An optional timeout for this request.</param>
//        Task<PushEVSEAdminStatusResult>

//            ISendAdminStatus.UpdateAdminStatus(IEnumerable<EVSEAdminStatusUpdate>  AdminStatusUpdates,
//                                               TransmissionTypes                   TransmissionType,

//                                               DateTime?                           Timestamp,
//                                               CancellationToken?                  CancellationToken,
//                                               EventTracking_Id                    EventTrackingId,
//                                               TimeSpan?                           RequestTimeout)

//        {

//            #region Initial checks

//            if (AdminStatusUpdates == null || !AdminStatusUpdates.Any())
//                return Task.FromResult(PushEVSEAdminStatusResult.NoOperation(Id, this));

//            #endregion

//            return Task.FromResult(PushEVSEAdminStatusResult.OutOfService(Id,
//                                                                      this,
//                                                                      AdminStatusUpdates));

//        }

//        #endregion

//        #region UpdateStatus     (StatusUpdates,      TransmissionType = Enqueue, ...)

//        /// <summary>
//        /// Update the given enumeration of EVSE status updates.
//        /// </summary>
//        /// <param name="StatusUpdates">An enumeration of EVSE status updates.</param>
//        /// <param name="TransmissionType">Whether to send the EVSE status updates directly or enqueue it for a while.</param>
//        /// 
//        /// <param name="Timestamp">The optional timestamp of the request.</param>
//        /// <param name="CancellationToken">An optional token to cancel this request.</param>
//        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
//        /// <param name="RequestTimeout">An optional timeout for this request.</param>
//        Task<PushEVSEStatusResult>

//            ISendStatus.UpdateStatus(IEnumerable<EVSEStatusUpdate>  StatusUpdates,
//                                     TransmissionTypes              TransmissionType,

//                                     DateTime?                      Timestamp,
//                                     CancellationToken?             CancellationToken,
//                                     EventTracking_Id               EventTrackingId,
//                                     TimeSpan?                      RequestTimeout)

//        {

//            #region Initial checks

//            if (StatusUpdates == null || !StatusUpdates.Any())
//                return Task.FromResult(PushEVSEStatusResult.NoOperation(Id, this));

//            #endregion

//            #region Enqueue, if requested...

//            if (TransmissionType == TransmissionTypes.Enqueue)
//            {

//                #region Send OnEnqueueSendCDRRequest event

//                //try
//                //{

//                //    OnEnqueueSendCDRRequest?.Invoke(StartTime,
//                //                                    Timestamp.Value,
//                //                                    this,
//                //                                    EventTrackingId,
//                //                                    RoamingNetwork.Id,
//                //                                    ChargeDetailRecord,
//                //                                    RequestTimeout);

//                //}
//                //catch (Exception e)
//                //{
//                //    e.Log(nameof(CSORoamingProviderLogger) + "." + nameof(OnSendCDRRequest));
//                //}

//                #endregion

//                lock(DataAndStatusLockOld)
//                {

//                    var FilteredUpdates = StatusUpdates.Where(statusupdate => IncludeEVSEs  (statusupdate.EVSE) &&
//                                                                              IncludeEVSEIds(statusupdate.EVSE.Id)).
//                                                        ToArray();

//                    if (FilteredUpdates.Length > 0)
//                    {

//                        foreach (var Update in FilteredUpdates)
//                        {

//                            // Delay the status update until the EVSE data had been uploaded!
//                            if (EVSEsToAddQueue.Any(evse => evse == Update.EVSE))
//                                EVSEStatusChangesDelayedQueue.Add(Update);

//                            else
//                                EVSEStatusChangesFastQueue.Add(Update);

//                        }

//                        FlushEVSEFastStatusTimer.Change(FlushEVSEFastStatusEvery, TimeSpan.FromMilliseconds(-1));

//                        return Task.FromResult(PushEVSEStatusResult.Enqueued(Id, this));

//                    }

//                    return Task.FromResult(PushEVSEStatusResult.NoOperation(Id, this));

//                }

//            }

//            #endregion

//            return Task.FromResult(PushEVSEStatusResult.Success(Id, this));

//        }

//        #endregion

//        #endregion

//        #region (Set/Add/Update/Delete) Charging station(s)...

//        #region SetStaticData   (ChargingStation, TransmissionType = Enqueue, ...)

//        /// <summary>
//        /// Set the EVSE data of the given charging station as new static EVSE data at the OICP server.
//        /// </summary>
//        /// <param name="ChargingStation">A charging station.</param>
//        /// <param name="TransmissionType">Whether to send the charging pool update directly or enqueue it for a while.</param>
//        /// 
//        /// <param name="Timestamp">The optional timestamp of the request.</param>
//        /// <param name="CancellationToken">An optional token to cancel this request.</param>
//        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
//        /// <param name="RequestTimeout">An optional timeout for this request.</param>
//        Task<PushEVSEDataResult>

//            ISendData.SetStaticData(ChargingStation     ChargingStation,
//                                    TransmissionTypes   TransmissionType,

//                                    DateTime?           Timestamp,
//                                    CancellationToken?  CancellationToken,
//                                    EventTracking_Id    EventTrackingId,
//                                    TimeSpan?           RequestTimeout)

//        {

//            #region Initial checks

//            if (ChargingStation == null)
//                throw new ArgumentNullException(nameof(ChargingStation), "The given charging station must not be null!");

//            #endregion

//            #region Enqueue, if requested...

//            if (TransmissionType == TransmissionTypes.Enqueue)
//            {

//                #region Send OnEnqueueSendCDRRequest event

//                //try
//                //{

//                //    OnEnqueueSendCDRRequest?.Invoke(StartTime,
//                //                                    Timestamp.Value,
//                //                                    this,
//                //                                    EventTrackingId,
//                //                                    RoamingNetwork.Id,
//                //                                    ChargeDetailRecord,
//                //                                    RequestTimeout);

//                //}
//                //catch (Exception e)
//                //{
//                //    e.Log(nameof(CSORoamingProviderLogger) + "." + nameof(OnSendCDRRequest));
//                //}

//                #endregion

//                lock(DataAndStatusLockOld)
//                {

//                    foreach (var evse in ChargingStation)
//                    {

//                        if (IncludeEVSEs == null ||
//                           (IncludeEVSEs != null && IncludeEVSEs(evse)))
//                        {

//                            EVSEsToAddQueue.Add(evse);

//                            FlushEVSEDataAndStatusTimer.Change(FlushEVSEDataAndStatusEvery, TimeSpan.FromMilliseconds(-1));

//                        }

//                    }

//                }

//                return Task.FromResult(PushEVSEDataResult.Enqueued(Id, this));

//            }

//            #endregion

//            return Task.FromResult(PushEVSEDataResult.Success(Id, this));

//        }

//        #endregion

//        #region AddStaticData   (ChargingStation, TransmissionType = Enqueue, ...)

//        /// <summary>
//        /// Add the EVSE data of the given charging station to the static EVSE data at the OICP server.
//        /// </summary>
//        /// <param name="ChargingStation">A charging station.</param>
//        /// <param name="TransmissionType">Whether to send the charging pool update directly or enqueue it for a while.</param>
//        /// 
//        /// <param name="Timestamp">The optional timestamp of the request.</param>
//        /// <param name="CancellationToken">An optional token to cancel this request.</param>
//        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
//        /// <param name="RequestTimeout">An optional timeout for this request.</param>
//        Task<PushEVSEDataResult>

//            ISendData.AddStaticData(ChargingStation     ChargingStation,
//                                    TransmissionTypes   TransmissionType,

//                                    DateTime?           Timestamp,
//                                    CancellationToken?  CancellationToken,
//                                    EventTracking_Id    EventTrackingId,
//                                    TimeSpan?           RequestTimeout)

//        {

//            #region Initial checks

//            if (ChargingStation == null)
//                throw new ArgumentNullException(nameof(ChargingStation), "The given charging station must not be null!");

//            #endregion

//            #region Enqueue, if requested...

//            if (TransmissionType == TransmissionTypes.Enqueue)
//            {

//                #region Send OnEnqueueSendCDRRequest event

//                //try
//                //{

//                //    OnEnqueueSendCDRRequest?.Invoke(StartTime,
//                //                                    Timestamp.Value,
//                //                                    this,
//                //                                    EventTrackingId,
//                //                                    RoamingNetwork.Id,
//                //                                    ChargeDetailRecord,
//                //                                    RequestTimeout);

//                //}
//                //catch (Exception e)
//                //{
//                //    e.Log(nameof(CSORoamingProviderLogger) + "." + nameof(OnSendCDRRequest));
//                //}

//                #endregion

//                lock(DataAndStatusLockOld)
//                {

//                    foreach (var evse in ChargingStation)
//                    {

//                        if (IncludeEVSEs == null ||
//                           (IncludeEVSEs != null && IncludeEVSEs(evse)))
//                        {

//                            EVSEsToAddQueue.Add(evse);

//                            FlushEVSEDataAndStatusTimer.Change(FlushEVSEDataAndStatusEvery, TimeSpan.FromMilliseconds(-1));

//                        }

//                    }

//                }

//                return Task.FromResult(PushEVSEDataResult.Enqueued(Id, this));

//            }

//            #endregion

//            return Task.FromResult(PushEVSEDataResult.Success(Id, this));

//        }

//        #endregion

//        #region UpdateStaticData(ChargingStation, PropertyName = null, OldValue = null, NewValue = null, TransmissionType = Enqueue, ...)

//        /// <summary>
//        /// Update the EVSE data of the given charging station within the static EVSE data at the OICP server.
//        /// </summary>
//        /// <param name="ChargingStation">A charging station.</param>
//        /// <param name="PropertyName">The name of the charging station property to update.</param>
//        /// <param name="OldValue">The old value of the charging station property to update.</param>
//        /// <param name="NewValue">The new value of the charging station property to update.</param>
//        /// <param name="TransmissionType">Whether to send the charging station update directly or enqueue it for a while.</param>
//        /// 
//        /// <param name="Timestamp">The optional timestamp of the request.</param>
//        /// <param name="CancellationToken">An optional token to cancel this request.</param>
//        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
//        /// <param name="RequestTimeout">An optional timeout for this request.</param>
//        Task<PushEVSEDataResult>

//            ISendData.UpdateStaticData(ChargingStation     ChargingStation,
//                                       String              PropertyName,
//                                       Object              OldValue,
//                                       Object              NewValue,
//                                       TransmissionTypes   TransmissionType,

//                                       DateTime?           Timestamp,
//                                       CancellationToken?  CancellationToken,
//                                       EventTracking_Id    EventTrackingId,
//                                       TimeSpan?           RequestTimeout)

//        {

//            #region Initial checks

//            if (ChargingStation == null)
//                throw new ArgumentNullException(nameof(ChargingStation), "The given charging station must not be null!");

//            #endregion

//            #region Enqueue, if requested...

//            if (TransmissionType == TransmissionTypes.Enqueue)
//            {

//                #region Send OnEnqueueSendCDRRequest event

//                //try
//                //{

//                //    OnEnqueueSendCDRRequest?.Invoke(StartTime,
//                //                                    Timestamp.Value,
//                //                                    this,
//                //                                    EventTrackingId,
//                //                                    RoamingNetwork.Id,
//                //                                    ChargeDetailRecord,
//                //                                    RequestTimeout);

//                //}
//                //catch (Exception e)
//                //{
//                //    e.Log(nameof(CSORoamingProviderLogger) + "." + nameof(OnSendCDRRequest));
//                //}

//                #endregion

//                lock(DataAndStatusLockOld)
//                {

//                    var AddData = false;

//                    foreach (var evse in ChargingStation)
//                    {
//                        if (IncludeEVSEs == null ||
//                           (IncludeEVSEs != null && IncludeEVSEs(evse)))
//                        {
//                            EVSEsToUpdateQueue.Add(evse);
//                            AddData = true;
//                        }
//                    }

//                    if (AddData)
//                    {

//                        if (ChargingStationsUpdateLog.TryGetValue(ChargingStation, out List<PropertyUpdateInfos> PropertyUpdateInfo))
//                            PropertyUpdateInfo.Add(new PropertyUpdateInfos(PropertyName, OldValue, NewValue));

//                        else
//                        {
//                            var List = new List<PropertyUpdateInfos>();
//                            List.Add(new PropertyUpdateInfos(PropertyName, OldValue, NewValue));
//                            ChargingStationsUpdateLog.Add(ChargingStation, List);
//                        }

//                        FlushEVSEDataAndStatusTimer.Change(FlushEVSEDataAndStatusEvery, TimeSpan.FromMilliseconds(-1));

//                    }

//                }

//                return Task.FromResult(PushEVSEDataResult.Enqueued(Id, this));

//            }

//            #endregion

//            return Task.FromResult(PushEVSEDataResult.Success(Id, this));

//        }

//        #endregion

//        #region DeleteStaticData(ChargingStation, TransmissionType = Enqueue, ...)

//        /// <summary>
//        /// Delete the EVSE data of the given charging station from the static EVSE data at the OICP server.
//        /// </summary>
//        /// <param name="ChargingStation">A charging station.</param>
//        /// <param name="TransmissionType">Whether to send the charging station update directly or enqueue it for a while.</param>
//        /// 
//        /// <param name="Timestamp">The optional timestamp of the request.</param>
//        /// <param name="CancellationToken">An optional token to cancel this request.</param>
//        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
//        /// <param name="RequestTimeout">An optional timeout for this request.</param>
//        Task<PushEVSEDataResult>

//            ISendData.DeleteStaticData(ChargingStation     ChargingStation,
//                                       TransmissionTypes   TransmissionType,

//                                       DateTime?           Timestamp,
//                                       CancellationToken?  CancellationToken,
//                                       EventTracking_Id    EventTrackingId,
//                                       TimeSpan?           RequestTimeout)

//        {

//            #region Initial checks

//            if (ChargingStation == null)
//                throw new ArgumentNullException(nameof(ChargingStation), "The given charging station must not be null!");

//            #endregion

//            return Task.FromResult(PushEVSEDataResult.Success(Id, this));

//        }

//        #endregion


//        #region SetStaticData   (ChargingStations, TransmissionType = Enqueue, ...)

//        /// <summary>
//        /// Set the EVSE data of the given enumeration of charging stations as new static EVSE data at the OICP server.
//        /// </summary>
//        /// <param name="ChargingStations">An enumeration of charging stations.</param>
//        /// <param name="TransmissionType">Whether to send the charging station update directly or enqueue it for a while.</param>
//        /// 
//        /// <param name="Timestamp">The optional timestamp of the request.</param>
//        /// <param name="CancellationToken">An optional token to cancel this request.</param>
//        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
//        /// <param name="RequestTimeout">An optional timeout for this request.</param>
//        Task<PushEVSEDataResult>

//            ISendData.SetStaticData(IEnumerable<ChargingStation>  ChargingStations,
//                                    TransmissionTypes             TransmissionType,

//                                    DateTime?                     Timestamp,
//                                    CancellationToken?            CancellationToken,
//                                    EventTracking_Id              EventTrackingId,
//                                    TimeSpan?                     RequestTimeout)

//        {

//            #region Initial checks

//            if (ChargingStations == null)
//                throw new ArgumentNullException(nameof(ChargingStations), "The given enumeration of charging stations must not be null!");

//            #endregion

//            return Task.FromResult(PushEVSEDataResult.Success(Id, this));

//        }

//        #endregion

//        #region AddStaticData   (ChargingStations, TransmissionType = Enqueue, ...)

//        /// <summary>
//        /// Add the EVSE data of the given enumeration of charging stations to the static EVSE data at the OICP server.
//        /// </summary>
//        /// <param name="ChargingStations">An enumeration of charging stations.</param>
//        /// <param name="TransmissionType">Whether to send the charging station update directly or enqueue it for a while.</param>
//        /// 
//        /// <param name="Timestamp">The optional timestamp of the request.</param>
//        /// <param name="CancellationToken">An optional token to cancel this request.</param>
//        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
//        /// <param name="RequestTimeout">An optional timeout for this request.</param>
//        Task<PushEVSEDataResult>

//            ISendData.AddStaticData(IEnumerable<ChargingStation>  ChargingStations,
//                                    TransmissionTypes             TransmissionType,


//                                    DateTime?                     Timestamp,
//                                    CancellationToken?            CancellationToken,
//                                    EventTracking_Id              EventTrackingId,
//                                    TimeSpan?                     RequestTimeout)

//        {

//            #region Initial checks

//            if (ChargingStations == null)
//                throw new ArgumentNullException(nameof(ChargingStations), "The given enumeration of charging stations must not be null!");

//            #endregion

//            return Task.FromResult(PushEVSEDataResult.Success(Id, this));

//        }

//        #endregion

//        #region UpdateStaticData(ChargingStations, TransmissionType = Enqueue, ...)

//        /// <summary>
//        /// Update the EVSE data of the given enumeration of charging stations within the static EVSE data at the OICP server.
//        /// </summary>
//        /// <param name="ChargingStations">An enumeration of charging stations.</param>
//        /// <param name="TransmissionType">Whether to send the charging station update directly or enqueue it for a while.</param>
//        /// 
//        /// <param name="Timestamp">The optional timestamp of the request.</param>
//        /// <param name="CancellationToken">An optional token to cancel this request.</param>
//        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
//        /// <param name="RequestTimeout">An optional timeout for this request.</param>
//        Task<PushEVSEDataResult>

//            ISendData.UpdateStaticData(IEnumerable<ChargingStation>  ChargingStations,
//                                       TransmissionTypes             TransmissionType,

//                                       DateTime?                     Timestamp,
//                                       CancellationToken?            CancellationToken,
//                                       EventTracking_Id              EventTrackingId,
//                                       TimeSpan?                     RequestTimeout)

//        {

//            #region Initial checks

//            if (ChargingStations == null)
//                throw new ArgumentNullException(nameof(ChargingStations), "The given enumeration of charging stations must not be null!");

//            #endregion

//            return Task.FromResult(PushEVSEDataResult.Success(Id, this));

//        }

//        #endregion

//        #region DeleteStaticData(ChargingStations, TransmissionType = Enqueue, ...)

//        /// <summary>
//        /// Delete the EVSE data of the given enumeration of charging stations from the static EVSE data at the OICP server.
//        /// </summary>
//        /// <param name="ChargingStations">An enumeration of charging stations.</param>
//        /// <param name="TransmissionType">Whether to send the charging station update directly or enqueue it for a while.</param>
//        /// 
//        /// <param name="Timestamp">The optional timestamp of the request.</param>
//        /// <param name="CancellationToken">An optional token to cancel this request.</param>
//        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
//        /// <param name="RequestTimeout">An optional timeout for this request.</param>
//        Task<PushEVSEDataResult>

//            ISendData.DeleteStaticData(IEnumerable<ChargingStation>  ChargingStations,
//                                       TransmissionTypes             TransmissionType,

//                                       DateTime?                     Timestamp,
//                                       CancellationToken?            CancellationToken,
//                                       EventTracking_Id              EventTrackingId,
//                                       TimeSpan?                     RequestTimeout)

//        {

//            #region Initial checks

//            if (ChargingStations == null)
//                throw new ArgumentNullException(nameof(ChargingStations), "The given enumeration of charging stations must not be null!");

//            #endregion

//            return Task.FromResult(PushEVSEDataResult.Success(Id, this));

//        }

//        #endregion


//        #region UpdateAdminStatus(AdminStatusUpdates, TransmissionType = Enqueue, ...)

//        /// <summary>
//        /// Update the given enumeration of charging station admin status updates.
//        /// </summary>
//        /// <param name="AdminStatusUpdates">An enumeration of charging station admin status updates.</param>
//        /// <param name="TransmissionType">Whether to send the charging station admin status updates directly or enqueue it for a while.</param>
//        /// 
//        /// <param name="Timestamp">The optional timestamp of the request.</param>
//        /// <param name="CancellationToken">An optional token to cancel this request.</param>
//        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
//        /// <param name="RequestTimeout">An optional timeout for this request.</param>
//        Task<PushChargingStationAdminStatusResult>

//            ISendAdminStatus.UpdateAdminStatus(IEnumerable<ChargingStationAdminStatusUpdate>  AdminStatusUpdates,
//                                               TransmissionTypes                              TransmissionType,

//                                               DateTime?                                      Timestamp,
//                                               CancellationToken?                             CancellationToken,
//                                               EventTracking_Id                               EventTrackingId,
//                                               TimeSpan?                                      RequestTimeout)

//        {

//            #region Initial checks

//            if (AdminStatusUpdates == null || !AdminStatusUpdates.Any())
//                return Task.FromResult(PushChargingStationAdminStatusResult.NoOperation(Id, this));

//            #endregion

//            return Task.FromResult(PushChargingStationAdminStatusResult.OutOfService(Id,
//                                                                                     this,
//                                                                                     AdminStatusUpdates));

//        }

//        #endregion

//        #region UpdateStatus     (StatusUpdates,      TransmissionType = Enqueue, ...)

//        /// <summary>
//        /// Update the given enumeration of charging station status updates.
//        /// </summary>
//        /// <param name="StatusUpdates">An enumeration of charging station status updates.</param>
//        /// <param name="TransmissionType">Whether to send the charging station status updates directly or enqueue it for a while.</param>
//        /// 
//        /// <param name="Timestamp">The optional timestamp of the request.</param>
//        /// <param name="CancellationToken">An optional token to cancel this request.</param>
//        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
//        /// <param name="RequestTimeout">An optional timeout for this request.</param>
//        Task<PushChargingStationStatusResult>

//            ISendStatus.UpdateStatus(IEnumerable<ChargingStationStatusUpdate>  StatusUpdates,
//                                     TransmissionTypes                         TransmissionType,

//                                     DateTime?                                 Timestamp,
//                                     CancellationToken?                        CancellationToken,
//                                     EventTracking_Id                          EventTrackingId,
//                                     TimeSpan?                                 RequestTimeout)

//        {

//            #region Initial checks

//            if (StatusUpdates == null || !StatusUpdates.Any())
//                return Task.FromResult(PushChargingStationStatusResult.NoOperation(Id, this));

//            #endregion

//            return Task.FromResult(PushChargingStationStatusResult.Success(Id, this));

//        }

//        #endregion

//        #endregion

//        #region (Set/Add/Update/Delete) Charging pool(s)...

//        #region SetStaticData   (ChargingPool, TransmissionType = Enqueue, ...)

//        /// <summary>
//        /// Set the EVSE data of the given charging pool as new static EVSE data at the OICP server.
//        /// </summary>
//        /// <param name="ChargingPool">A charging pool.</param>
//        /// <param name="TransmissionType">Whether to send the charging pool update directly or enqueue it for a while.</param>
//        /// 
//        /// <param name="Timestamp">The optional timestamp of the request.</param>
//        /// <param name="CancellationToken">An optional token to cancel this request.</param>
//        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
//        /// <param name="RequestTimeout">An optional timeout for this request.</param>
//        Task<PushEVSEDataResult>

//            ISendData.SetStaticData(ChargingPool        ChargingPool,
//                                    TransmissionTypes   TransmissionType,

//                                    DateTime?           Timestamp,
//                                    CancellationToken?  CancellationToken,
//                                    EventTracking_Id    EventTrackingId,
//                                    TimeSpan?           RequestTimeout)

//        {

//            #region Initial checks

//            if (ChargingPool == null)
//                throw new ArgumentNullException(nameof(ChargingPool), "The given charging pool must not be null!");

//            #endregion

//            #region Enqueue, if requested...

//            if (TransmissionType == TransmissionTypes.Enqueue)
//            {

//                #region Send OnEnqueueSendCDRRequest event

//                //try
//                //{

//                //    OnEnqueueSendCDRRequest?.Invoke(StartTime,
//                //                                    Timestamp.Value,
//                //                                    this,
//                //                                    EventTrackingId,
//                //                                    RoamingNetwork.Id,
//                //                                    ChargeDetailRecord,
//                //                                    RequestTimeout);

//                //}
//                //catch (Exception e)
//                //{
//                //    e.Log(nameof(CSORoamingProviderLogger) + "." + nameof(OnSendCDRRequest));
//                //}

//                #endregion

//                lock(DataAndStatusLockOld)
//                {

//                    foreach (var evse in ChargingPool.EVSEs)
//                    {

//                        if (IncludeEVSEs == null ||
//                           (IncludeEVSEs != null && IncludeEVSEs(evse)))
//                        {

//                            EVSEsToAddQueue.Add(evse);

//                            FlushEVSEDataAndStatusTimer.Change(FlushEVSEDataAndStatusEvery, TimeSpan.FromMilliseconds(-1));

//                        }

//                    }

//                }

//                return Task.FromResult(PushEVSEDataResult.Enqueued(Id, this));

//            }

//            #endregion

//            return Task.FromResult(PushEVSEDataResult.Success(Id, this));

//        }

//        #endregion

//        #region AddStaticData   (ChargingPool, TransmissionType = Enqueue, ...)

//        /// <summary>
//        /// Add the EVSE data of the given charging pool to the static EVSE data at the OICP server.
//        /// </summary>
//        /// <param name="ChargingPool">A charging pool.</param>
//        /// <param name="TransmissionType">Whether to send the charging pool update directly or enqueue it for a while.</param>
//        /// 
//        /// <param name="Timestamp">The optional timestamp of the request.</param>
//        /// <param name="CancellationToken">An optional token to cancel this request.</param>
//        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
//        /// <param name="RequestTimeout">An optional timeout for this request.</param>
//        Task<PushEVSEDataResult>

//            ISendData.AddStaticData(ChargingPool        ChargingPool,
//                                    TransmissionTypes   TransmissionType,

//                                    DateTime?           Timestamp,
//                                    CancellationToken?  CancellationToken,
//                                    EventTracking_Id    EventTrackingId,
//                                    TimeSpan?           RequestTimeout)

//        {

//            #region Initial checks

//            if (ChargingPool == null)
//                throw new ArgumentNullException(nameof(ChargingPool), "The given charging pool must not be null!");

//            #endregion

//            #region Enqueue, if requested...

//            if (TransmissionType == TransmissionTypes.Enqueue)
//            {

//                #region Send OnEnqueueSendCDRRequest event

//                //try
//                //{

//                //    OnEnqueueSendCDRRequest?.Invoke(StartTime,
//                //                                    Timestamp.Value,
//                //                                    this,
//                //                                    EventTrackingId,
//                //                                    RoamingNetwork.Id,
//                //                                    ChargeDetailRecord,
//                //                                    RequestTimeout);

//                //}
//                //catch (Exception e)
//                //{
//                //    e.Log(nameof(CSORoamingProviderLogger) + "." + nameof(OnSendCDRRequest));
//                //}

//                #endregion

//                lock(DataAndStatusLockOld)
//                {

//                    foreach (var evse in ChargingPool.EVSEs)
//                    {

//                        if (IncludeEVSEs == null ||
//                           (IncludeEVSEs != null && IncludeEVSEs(evse)))
//                        {

//                            EVSEsToAddQueue.Add(evse);

//                            FlushEVSEDataAndStatusTimer.Change(FlushEVSEDataAndStatusEvery, TimeSpan.FromMilliseconds(-1));

//                        }

//                    }

//                }

//                return Task.FromResult(PushEVSEDataResult.Enqueued(Id, this));

//            }

//            #endregion

//            return Task.FromResult(PushEVSEDataResult.Success(Id, this));

//        }

//        #endregion

//        #region UpdateStaticData(ChargingPool, PropertyName = null, OldValue = null, NewValue = null, TransmissionType = Enqueue, ...)

//        /// <summary>
//        /// Update the EVSE data of the given charging pool within the static EVSE data at the OICP server.
//        /// </summary>
//        /// <param name="ChargingPool">A charging pool.</param>
//        /// <param name="PropertyName">The name of the charging pool property to update.</param>
//        /// <param name="OldValue">The old value of the charging pool property to update.</param>
//        /// <param name="NewValue">The new value of the charging pool property to update.</param>
//        /// <param name="TransmissionType">Whether to send the charging pool update directly or enqueue it for a while.</param>
//        /// 
//        /// <param name="Timestamp">The optional timestamp of the request.</param>
//        /// <param name="CancellationToken">An optional token to cancel this request.</param>
//        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
//        /// <param name="RequestTimeout">An optional timeout for this request.</param>
//        Task<PushEVSEDataResult>

//            ISendData.UpdateStaticData(ChargingPool        ChargingPool,
//                                       String              PropertyName,
//                                       Object              OldValue,
//                                       Object              NewValue,
//                                       TransmissionTypes   TransmissionType,

//                                       DateTime?           Timestamp,
//                                       CancellationToken?  CancellationToken,
//                                       EventTracking_Id    EventTrackingId,
//                                       TimeSpan?           RequestTimeout)

//        {

//            #region Initial checks

//            if (ChargingPool == null)
//                throw new ArgumentNullException(nameof(ChargingPool), "The given charging pool must not be null!");

//            #endregion

//            #region Enqueue, if requested...

//            if (TransmissionType == TransmissionTypes.Enqueue)
//            {

//                #region Send OnEnqueueSendCDRRequest event

//                //try
//                //{

//                //    OnEnqueueSendCDRRequest?.Invoke(StartTime,
//                //                                    Timestamp.Value,
//                //                                    this,
//                //                                    EventTrackingId,
//                //                                    RoamingNetwork.Id,
//                //                                    ChargeDetailRecord,
//                //                                    RequestTimeout);

//                //}
//                //catch (Exception e)
//                //{
//                //    e.Log(nameof(CSORoamingProviderLogger) + "." + nameof(OnSendCDRRequest));
//                //}

//                #endregion

//                lock(DataAndStatusLockOld)
//                {

//                    var AddData = false;

//                    foreach (var evse in ChargingPool.EVSEs)
//                    {
//                        if (IncludeEVSEs == null ||
//                           (IncludeEVSEs != null && IncludeEVSEs(evse)))
//                        {
//                            EVSEsToUpdateQueue.Add(evse);
//                            AddData = true;
//                        }
//                    }

//                    if (AddData)
//                    {

//                        if (ChargingPoolsUpdateLog.TryGetValue(ChargingPool, out List<PropertyUpdateInfos> PropertyUpdateInfo))
//                            PropertyUpdateInfo.Add(new PropertyUpdateInfos(PropertyName, OldValue, NewValue));

//                        else
//                        {
//                            var List = new List<PropertyUpdateInfos>();
//                            List.Add(new PropertyUpdateInfos(PropertyName, OldValue, NewValue));
//                            ChargingPoolsUpdateLog.Add(ChargingPool, List);
//                        }

//                        FlushEVSEDataAndStatusTimer.Change(FlushEVSEDataAndStatusEvery, TimeSpan.FromMilliseconds(-1));

//                    }

//                }

//                return Task.FromResult(PushEVSEDataResult.Enqueued(Id, this));

//            }

//            #endregion

//            return Task.FromResult(PushEVSEDataResult.Success(Id, this));

//        }

//        #endregion

//        #region DeleteStaticData(ChargingPool, TransmissionType = Enqueue, ...)

//        /// <summary>
//        /// Delete the EVSE data of the given charging pool from the static EVSE data at the OICP server.
//        /// </summary>
//        /// <param name="ChargingPool">A charging pool.</param>
//        /// <param name="TransmissionType">Whether to send the charging pool update directly or enqueue it for a while.</param>
//        /// 
//        /// <param name="Timestamp">The optional timestamp of the request.</param>
//        /// <param name="CancellationToken">An optional token to cancel this request.</param>
//        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
//        /// <param name="RequestTimeout">An optional timeout for this request.</param>
//        Task<PushEVSEDataResult>

//            ISendData.DeleteStaticData(ChargingPool        ChargingPool,
//                                       TransmissionTypes   TransmissionType,

//                                       DateTime?           Timestamp,
//                                       CancellationToken?  CancellationToken,
//                                       EventTracking_Id    EventTrackingId,
//                                       TimeSpan?           RequestTimeout)

//        {

//            #region Initial checks

//            if (ChargingPool == null)
//                throw new ArgumentNullException(nameof(ChargingPool), "The given charging pool must not be null!");

//            #endregion

//            return Task.FromResult(PushEVSEDataResult.Success(Id, this));

//        }

//        #endregion


//        #region SetStaticData   (ChargingPools, TransmissionType = Enqueue, ...)

//        /// <summary>
//        /// Set the EVSE data of the given enumeration of charging pools as new static EVSE data at the OICP server.
//        /// </summary>
//        /// <param name="ChargingPools">An enumeration of charging pools.</param>
//        /// <param name="TransmissionType">Whether to send the charging pool update directly or enqueue it for a while.</param>
//        /// 
//        /// <param name="Timestamp">The optional timestamp of the request.</param>
//        /// <param name="CancellationToken">An optional token to cancel this request.</param>
//        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
//        /// <param name="RequestTimeout">An optional timeout for this request.</param>
//        Task<PushEVSEDataResult>

//            ISendData.SetStaticData(IEnumerable<ChargingPool>  ChargingPools,
//                                    TransmissionTypes          TransmissionType,

//                                    DateTime?                  Timestamp,
//                                    CancellationToken?         CancellationToken,
//                                    EventTracking_Id           EventTrackingId,
//                                    TimeSpan?                  RequestTimeout)

//        {

//            #region Initial checks

//            if (ChargingPools == null)
//                throw new ArgumentNullException(nameof(ChargingPools), "The given enumeration of charging pools must not be null!");

//            #endregion

//            return Task.FromResult(PushEVSEDataResult.Success(Id, this));

//        }

//        #endregion

//        #region AddStaticData   (ChargingPools, TransmissionType = Enqueue, ...)

//        /// <summary>
//        /// Add the EVSE data of the given enumeration of charging pools to the static EVSE data at the OICP server.
//        /// </summary>
//        /// <param name="ChargingPools">An enumeration of charging pools.</param>
//        /// <param name="TransmissionType">Whether to send the charging pool update directly or enqueue it for a while.</param>
//        /// 
//        /// <param name="Timestamp">The optional timestamp of the request.</param>
//        /// <param name="CancellationToken">An optional token to cancel this request.</param>
//        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
//        /// <param name="RequestTimeout">An optional timeout for this request.</param>
//        Task<PushEVSEDataResult>

//            ISendData.AddStaticData(IEnumerable<ChargingPool>  ChargingPools,
//                                    TransmissionTypes          TransmissionType,

//                                    DateTime?                  Timestamp,
//                                    CancellationToken?         CancellationToken,
//                                    EventTracking_Id           EventTrackingId,
//                                    TimeSpan?                  RequestTimeout)

//        {

//            #region Initial checks

//            if (ChargingPools == null)
//                throw new ArgumentNullException(nameof(ChargingPools), "The given enumeration of charging pools must not be null!");

//            #endregion

//            return Task.FromResult(PushEVSEDataResult.Success(Id, this));

//        }

//        #endregion

//        #region UpdateStaticData(ChargingPools, TransmissionType = Enqueue, ...)

//        /// <summary>
//        /// Update the EVSE data of the given enumeration of charging pools within the static EVSE data at the OICP server.
//        /// </summary>
//        /// <param name="ChargingPools">An enumeration of charging pools.</param>
//        /// <param name="TransmissionType">Whether to send the charging pool update directly or enqueue it for a while.</param>
//        /// 
//        /// <param name="Timestamp">The optional timestamp of the request.</param>
//        /// <param name="CancellationToken">An optional token to cancel this request.</param>
//        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
//        /// <param name="RequestTimeout">An optional timeout for this request.</param>
//        Task<PushEVSEDataResult>

//            ISendData.UpdateStaticData(IEnumerable<ChargingPool>  ChargingPools,
//                                       TransmissionTypes          TransmissionType,

//                                       DateTime?                  Timestamp,
//                                       CancellationToken?         CancellationToken,
//                                       EventTracking_Id           EventTrackingId,
//                                       TimeSpan?                  RequestTimeout)

//        {

//            #region Initial checks

//            if (ChargingPools == null)
//                throw new ArgumentNullException(nameof(ChargingPools), "The given enumeration of charging pools must not be null!");

//            #endregion

//            return Task.FromResult(PushEVSEDataResult.Success(Id, this));

//        }

//        #endregion

//        #region DeleteStaticData(ChargingPools, TransmissionType = Enqueue, ...)

//        /// <summary>
//        /// Delete the EVSE data of the given enumeration of charging pools from the static EVSE data at the OICP server.
//        /// </summary>
//        /// <param name="ChargingPools">An enumeration of charging pools.</param>
//        /// <param name="TransmissionType">Whether to send the charging pool update directly or enqueue it for a while.</param>
//        /// 
//        /// <param name="Timestamp">The optional timestamp of the request.</param>
//        /// <param name="CancellationToken">An optional token to cancel this request.</param>
//        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
//        /// <param name="RequestTimeout">An optional timeout for this request.</param>
//        Task<PushEVSEDataResult>

//            ISendData.DeleteStaticData(IEnumerable<ChargingPool>  ChargingPools,
//                                       TransmissionTypes          TransmissionType,

//                                       DateTime?                  Timestamp,
//                                       CancellationToken?         CancellationToken,
//                                       EventTracking_Id           EventTrackingId,
//                                       TimeSpan?                  RequestTimeout)

//        {

//            #region Initial checks

//            if (ChargingPools == null)
//                throw new ArgumentNullException(nameof(ChargingPools), "The given enumeration of charging pools must not be null!");

//            #endregion

//            return Task.FromResult(PushEVSEDataResult.Success(Id, this));

//        }

//        #endregion


//        #region UpdateAdminStatus(AdminStatusUpdates, TransmissionType = Enqueue, ...)

//        /// <summary>
//        /// Update the given enumeration of charging pool admin status updates.
//        /// </summary>
//        /// <param name="AdminStatusUpdates">An enumeration of charging pool admin status updates.</param>
//        /// <param name="TransmissionType">Whether to send the charging pool admin status updates directly or enqueue it for a while.</param>
//        /// 
//        /// <param name="Timestamp">The optional timestamp of the request.</param>
//        /// <param name="CancellationToken">An optional token to cancel this request.</param>
//        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
//        /// <param name="RequestTimeout">An optional timeout for this request.</param>
//        Task<PushChargingPoolAdminStatusResult>

//            ISendAdminStatus.UpdateAdminStatus(IEnumerable<ChargingPoolAdminStatusUpdate>  AdminStatusUpdates,
//                                               TransmissionTypes                           TransmissionType,

//                                               DateTime?                                   Timestamp,
//                                               CancellationToken?                          CancellationToken,
//                                               EventTracking_Id                            EventTrackingId,
//                                               TimeSpan?                                   RequestTimeout)

//        {

//            #region Initial checks

//            if (AdminStatusUpdates == null || !AdminStatusUpdates.Any())
//                return Task.FromResult(PushChargingPoolAdminStatusResult.NoOperation(Id, this));

//            #endregion

//            return Task.FromResult(PushChargingPoolAdminStatusResult.OutOfService(Id,
//                                                                                  this,
//                                                                                  AdminStatusUpdates));

//        }

//        #endregion

//        #region UpdateStatus     (StatusUpdates,      TransmissionType = Enqueue, ...)

//        /// <summary>
//        /// Update the given enumeration of charging pool status updates.
//        /// </summary>
//        /// <param name="StatusUpdates">An enumeration of charging pool status updates.</param>
//        /// <param name="TransmissionType">Whether to send the charging pool status updates directly or enqueue it for a while.</param>
//        /// 
//        /// <param name="Timestamp">The optional timestamp of the request.</param>
//        /// <param name="CancellationToken">An optional token to cancel this request.</param>
//        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
//        /// <param name="RequestTimeout">An optional timeout for this request.</param>
//        Task<PushChargingPoolStatusResult>

//            ISendStatus.UpdateStatus(IEnumerable<ChargingPoolStatusUpdate>  StatusUpdates,
//                                     TransmissionTypes                      TransmissionType,

//                                     DateTime?                              Timestamp,
//                                     CancellationToken?                     CancellationToken,
//                                     EventTracking_Id                       EventTrackingId,
//                                     TimeSpan?                              RequestTimeout)

//        {

//            #region Initial checks

//            if (StatusUpdates == null || !StatusUpdates.Any())
//                return Task.FromResult(PushChargingPoolStatusResult.NoOperation(Id, this));

//            #endregion

//            return Task.FromResult(PushChargingPoolStatusResult.Success(Id, this));

//        }

//        #endregion

//        #endregion

//        #region (Set/Add/Update/Delete) Charging station operator(s)...

//        #region SetStaticData   (ChargingStationOperator, ...)

//        /// <summary>
//        /// Set the EVSE data of the given charging station operator as new static EVSE data at the OICP server.
//        /// </summary>
//        /// <param name="ChargingStationOperator">A charging station operator.</param>
//        /// 
//        /// <param name="Timestamp">The optional timestamp of the request.</param>
//        /// <param name="CancellationToken">An optional token to cancel this request.</param>
//        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
//        /// <param name="RequestTimeout">An optional timeout for this request.</param>
//        Task<PushEVSEDataResult>

//            ISendData.SetStaticData(ChargingStationOperator  ChargingStationOperator,

//                                    DateTime?                     Timestamp,
//                                    CancellationToken?            CancellationToken,
//                                    EventTracking_Id              EventTrackingId,
//                                    TimeSpan?                     RequestTimeout)

//        {

//            #region Initial checks

//            if (ChargingStationOperator == null)
//                throw new ArgumentNullException(nameof(ChargingStationOperator), "The given charging station operator must not be null!");

//            #endregion

//            return Task.FromResult(PushEVSEDataResult.Success(Id, this));

//        }

//        #endregion

//        #region AddStaticData   (ChargingStationOperator, ...)

//        /// <summary>
//        /// Add the EVSE data of the given charging station operator to the static EVSE data at the OICP server.
//        /// </summary>
//        /// <param name="ChargingStationOperator">A charging station operator.</param>
//        /// 
//        /// <param name="Timestamp">The optional timestamp of the request.</param>
//        /// <param name="CancellationToken">An optional token to cancel this request.</param>
//        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
//        /// <param name="RequestTimeout">An optional timeout for this request.</param>
//        Task<PushEVSEDataResult>

//            ISendData.AddStaticData(ChargingStationOperator  ChargingStationOperator,

//                                    DateTime?                     Timestamp,
//                                    CancellationToken?            CancellationToken,
//                                    EventTracking_Id              EventTrackingId,
//                                    TimeSpan?                     RequestTimeout)

//        {

//            #region Initial checks

//            if (ChargingStationOperator == null)
//                throw new ArgumentNullException(nameof(ChargingStationOperator), "The given charging station operator must not be null!");

//            #endregion

//            return Task.FromResult(PushEVSEDataResult.Success(Id, this));

//        }

//        #endregion

//        #region UpdateStaticData(ChargingStationOperator, ...)

//        /// <summary>
//        /// Update the EVSE data of the given charging station operator within the static EVSE data at the OICP server.
//        /// </summary>
//        /// <param name="ChargingStationOperator">A charging station operator.</param>
//        /// 
//        /// <param name="Timestamp">The optional timestamp of the request.</param>
//        /// <param name="CancellationToken">An optional token to cancel this request.</param>
//        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
//        /// <param name="RequestTimeout">An optional timeout for this request.</param>
//        Task<PushEVSEDataResult>

//            ISendData.UpdateStaticData(ChargingStationOperator  ChargingStationOperator,

//                                       DateTime?                     Timestamp,
//                                       CancellationToken?            CancellationToken,
//                                       EventTracking_Id              EventTrackingId,
//                                       TimeSpan?                     RequestTimeout)

//        {

//            #region Initial checks

//            if (ChargingStationOperator == null)
//                throw new ArgumentNullException(nameof(ChargingStationOperator), "The given charging station operator must not be null!");

//            #endregion

//            return Task.FromResult(PushEVSEDataResult.Success(Id, this));

//        }

//        #endregion

//        #region DeleteStaticData(ChargingStationOperator, ...)

//        /// <summary>
//        /// Delete the EVSE data of the given charging station operator from the static EVSE data at the OICP server.
//        /// </summary>
//        /// <param name="ChargingStationOperator">A charging station operator.</param>
//        /// 
//        /// <param name="Timestamp">The optional timestamp of the request.</param>
//        /// <param name="CancellationToken">An optional token to cancel this request.</param>
//        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
//        /// <param name="RequestTimeout">An optional timeout for this request.</param>
//        Task<PushEVSEDataResult>

//            ISendData.DeleteStaticData(ChargingStationOperator  ChargingStationOperator,

//                                       DateTime?                     Timestamp,
//                                       CancellationToken?            CancellationToken,
//                                       EventTracking_Id              EventTrackingId,
//                                       TimeSpan?                     RequestTimeout)

//        {

//            #region Initial checks

//            if (ChargingStationOperator == null)
//                throw new ArgumentNullException(nameof(ChargingStationOperator), "The given charging station operator must not be null!");

//            #endregion

//            return Task.FromResult(PushEVSEDataResult.Success(Id, this));

//        }

//        #endregion


//        #region SetStaticData   (ChargingStationOperators, ...)

//        /// <summary>
//        /// Set the EVSE data of the given enumeration of charging station operators as new static EVSE data at the OICP server.
//        /// </summary>
//        /// <param name="ChargingStationOperators">An enumeration of charging station operators.</param>
//        /// 
//        /// <param name="Timestamp">The optional timestamp of the request.</param>
//        /// <param name="CancellationToken">An optional token to cancel this request.</param>
//        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
//        /// <param name="RequestTimeout">An optional timeout for this request.</param>
//        Task<PushEVSEDataResult>

//            ISendData.SetStaticData(IEnumerable<ChargingStationOperator>  ChargingStationOperators,

//                                    DateTime?                                  Timestamp,
//                                    CancellationToken?                         CancellationToken,
//                                    EventTracking_Id                           EventTrackingId,
//                                    TimeSpan?                                  RequestTimeout)

//        {

//            #region Initial checks

//            if (ChargingStationOperators == null)
//                throw new ArgumentNullException(nameof(ChargingStationOperators), "The given enumeration of charging station operators must not be null!");

//            #endregion

//            return Task.FromResult(PushEVSEDataResult.Success(Id, this));

//        }

//        #endregion

//        #region AddStaticData   (ChargingStationOperators, ...)

//        /// <summary>
//        /// Add the EVSE data of the given enumeration of charging station operators to the static EVSE data at the OICP server.
//        /// </summary>
//        /// <param name="ChargingStationOperators">An enumeration of charging station operators.</param>
//        /// 
//        /// <param name="Timestamp">The optional timestamp of the request.</param>
//        /// <param name="CancellationToken">An optional token to cancel this request.</param>
//        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
//        /// <param name="RequestTimeout">An optional timeout for this request.</param>
//        Task<PushEVSEDataResult>

//            ISendData.AddStaticData(IEnumerable<ChargingStationOperator>  ChargingStationOperators,

//                                    DateTime?                                  Timestamp,
//                                    CancellationToken?                         CancellationToken,
//                                    EventTracking_Id                           EventTrackingId,
//                                    TimeSpan?                                  RequestTimeout)

//        {

//            #region Initial checks

//            if (ChargingStationOperators == null)
//                throw new ArgumentNullException(nameof(ChargingStationOperators), "The given enumeration of charging station operators must not be null!");

//            #endregion

//            return Task.FromResult(PushEVSEDataResult.Success(Id, this));

//        }

//        #endregion

//        #region UpdateStaticData(ChargingStationOperators, ...)

//        /// <summary>
//        /// Update the EVSE data of the given enumeration of charging station operators within the static EVSE data at the OICP server.
//        /// </summary>
//        /// <param name="ChargingStationOperators">An enumeration of charging station operators.</param>
//        /// 
//        /// <param name="Timestamp">The optional timestamp of the request.</param>
//        /// <param name="CancellationToken">An optional token to cancel this request.</param>
//        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
//        /// <param name="RequestTimeout">An optional timeout for this request.</param>
//        Task<PushEVSEDataResult>

//            ISendData.UpdateStaticData(IEnumerable<ChargingStationOperator>  ChargingStationOperators,

//                                       DateTime?                                  Timestamp,
//                                       CancellationToken?                         CancellationToken,
//                                       EventTracking_Id                           EventTrackingId,
//                                       TimeSpan?                                  RequestTimeout)

//        {

//            #region Initial checks

//            if (ChargingStationOperators == null)
//                throw new ArgumentNullException(nameof(ChargingStationOperators), "The given enumeration of charging station operators must not be null!");

//            #endregion

//            return Task.FromResult(PushEVSEDataResult.Success(Id, this));

//        }

//        #endregion

//        #region DeleteStaticData(ChargingStationOperators, ...)

//        /// <summary>
//        /// Delete the EVSE data of the given enumeration of charging station operators from the static EVSE data at the OICP server.
//        /// </summary>
//        /// <param name="ChargingStationOperators">An enumeration of charging station operators.</param>
//        /// 
//        /// <param name="Timestamp">The optional timestamp of the request.</param>
//        /// <param name="CancellationToken">An optional token to cancel this request.</param>
//        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
//        /// <param name="RequestTimeout">An optional timeout for this request.</param>
//        Task<PushEVSEDataResult>

//            ISendData.DeleteStaticData(IEnumerable<ChargingStationOperator>  ChargingStationOperators,

//                                       DateTime?                                  Timestamp,
//                                       CancellationToken?                         CancellationToken,
//                                       EventTracking_Id                           EventTrackingId,
//                                       TimeSpan?                                  RequestTimeout)

//        {

//            #region Initial checks

//            if (ChargingStationOperators == null)
//                throw new ArgumentNullException(nameof(ChargingStationOperators), "The given enumeration of charging station operators must not be null!");

//            #endregion

//            return Task.FromResult(PushEVSEDataResult.Success(Id, this));

//        }

//        #endregion


//        #region UpdateChargingStationOperatorAdminStatus(AdminStatusUpdates, TransmissionType = Enqueue, ...)

//        /// <summary>
//        /// Update the given enumeration of charging station operator admin status updates.
//        /// </summary>
//        /// <param name="AdminStatusUpdates">An enumeration of charging station operator admin status updates.</param>
//        /// <param name="TransmissionType">Whether to send the charging station operator admin status updates directly or enqueue it for a while.</param>
//        /// 
//        /// <param name="Timestamp">The optional timestamp of the request.</param>
//        /// <param name="CancellationToken">An optional token to cancel this request.</param>
//        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
//        /// <param name="RequestTimeout">An optional timeout for this request.</param>
//        Task<PushChargingStationOperatorAdminStatusResult>

//            ISendAdminStatus.UpdateAdminStatus(IEnumerable<ChargingStationOperatorAdminStatusUpdate>  AdminStatusUpdates,
//                                               TransmissionTypes                                      TransmissionType,

//                                               DateTime?                                              Timestamp,
//                                               CancellationToken?                                     CancellationToken,
//                                               EventTracking_Id                                       EventTrackingId,
//                                               TimeSpan?                                              RequestTimeout)

//        {

//            #region Initial checks

//            if (AdminStatusUpdates == null || !AdminStatusUpdates.Any())
//                return Task.FromResult(PushChargingStationOperatorAdminStatusResult.NoOperation(Id, this));

//            #endregion

//            return Task.FromResult(PushChargingStationOperatorAdminStatusResult.OutOfService(Id,
//                                                                                             this,
//                                                                                             AdminStatusUpdates));

//        }

//        #endregion

//        #region UpdateChargingStationOperatorStatus     (StatusUpdates,      TransmissionType = Enqueue, ...)

//        /// <summary>
//        /// Update the given enumeration of charging station operator status updates.
//        /// </summary>
//        /// <param name="StatusUpdates">An enumeration of charging station operator status updates.</param>
//        /// <param name="TransmissionType">Whether to send the charging station operator status updates directly or enqueue it for a while.</param>
//        /// 
//        /// <param name="Timestamp">The optional timestamp of the request.</param>
//        /// <param name="CancellationToken">An optional token to cancel this request.</param>
//        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
//        /// <param name="RequestTimeout">An optional timeout for this request.</param>
//        Task<PushChargingStationOperatorStatusResult>

//            ISendStatus.UpdateStatus(IEnumerable<ChargingStationOperatorStatusUpdate>  StatusUpdates,
//                                     TransmissionTypes                                 TransmissionType,

//                                     DateTime?                                         Timestamp,
//                                     CancellationToken?                                CancellationToken,
//                                     EventTracking_Id                                  EventTrackingId,
//                                     TimeSpan?                                         RequestTimeout)

//        {

//            #region Initial checks

//            if (StatusUpdates == null || !StatusUpdates.Any())
//                return Task.FromResult(PushChargingStationOperatorStatusResult.NoOperation(Id, this));

//            #endregion

//            return Task.FromResult(PushChargingStationOperatorStatusResult.Success(Id, this));

//        }

//        #endregion

//        #endregion

//        #region (Set/Add/Update/Delete) Roaming network...

//        #region SetStaticData   (RoamingNetwork, ...)

//        /// <summary>
//        /// Set the EVSE data of the given roaming network as new static EVSE data at the OICP server.
//        /// </summary>
//        /// <param name="RoamingNetwork">A roaming network.</param>
//        /// 
//        /// <param name="Timestamp">The optional timestamp of the request.</param>
//        /// <param name="CancellationToken">An optional token to cancel this request.</param>
//        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
//        /// <param name="RequestTimeout">An optional timeout for this request.</param>
//        Task<PushEVSEDataResult>

//            ISendData.SetStaticData(RoamingNetwork      RoamingNetwork,

//                                    DateTime?           Timestamp,
//                                    CancellationToken?  CancellationToken,
//                                    EventTracking_Id    EventTrackingId,
//                                    TimeSpan?           RequestTimeout)

//        {

//            #region Initial checks

//            if (RoamingNetwork == null)
//                throw new ArgumentNullException(nameof(RoamingNetwork), "The given roaming network must not be null!");

//            #endregion

//            return Task.FromResult(PushEVSEDataResult.Success(Id, this));

//        }

//        #endregion

//        #region AddStaticData   (RoamingNetwork, ...)

//        /// <summary>
//        /// Add the EVSE data of the given roaming network to the static EVSE data at the OICP server.
//        /// </summary>
//        /// <param name="RoamingNetwork">A roaming network.</param>
//        /// 
//        /// <param name="Timestamp">The optional timestamp of the request.</param>
//        /// <param name="CancellationToken">An optional token to cancel this request.</param>
//        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
//        /// <param name="RequestTimeout">An optional timeout for this request.</param>
//        Task<PushEVSEDataResult>

//            ISendData.AddStaticData(RoamingNetwork      RoamingNetwork,

//                                    DateTime?           Timestamp,
//                                    CancellationToken?  CancellationToken,
//                                    EventTracking_Id    EventTrackingId,
//                                    TimeSpan?           RequestTimeout)

//        {

//            #region Initial checks

//            if (RoamingNetwork == null)
//                throw new ArgumentNullException(nameof(RoamingNetwork), "The given roaming network must not be null!");

//            #endregion

//            return Task.FromResult(PushEVSEDataResult.Success(Id, this));

//        }

//        #endregion

//        #region UpdateStaticData(RoamingNetwork, ...)

//        /// <summary>
//        /// Update the EVSE data of the given roaming network within the static EVSE data at the OICP server.
//        /// </summary>
//        /// <param name="RoamingNetwork">A roaming network.</param>
//        /// 
//        /// <param name="Timestamp">The optional timestamp of the request.</param>
//        /// <param name="CancellationToken">An optional token to cancel this request.</param>
//        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
//        /// <param name="RequestTimeout">An optional timeout for this request.</param>
//        Task<PushEVSEDataResult>

//            ISendData.UpdateStaticData(RoamingNetwork      RoamingNetwork,

//                                       DateTime?           Timestamp,
//                                       CancellationToken?  CancellationToken,
//                                       EventTracking_Id    EventTrackingId,
//                                       TimeSpan?           RequestTimeout)

//        {

//            #region Initial checks

//            if (RoamingNetwork == null)
//                throw new ArgumentNullException(nameof(RoamingNetwork), "The given roaming network must not be null!");

//            #endregion

//            return Task.FromResult(PushEVSEDataResult.Success(Id, this));

//        }

//        #endregion

//        #region DeleteStaticData(RoamingNetwork, ...)

//        /// <summary>
//        /// Delete the EVSE data of the given roaming network from the static EVSE data at the OICP server.
//        /// </summary>
//        /// <param name="RoamingNetwork">A roaming network to upload.</param>
//        /// 
//        /// <param name="Timestamp">The optional timestamp of the request.</param>
//        /// <param name="CancellationToken">An optional token to cancel this request.</param>
//        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
//        /// <param name="RequestTimeout">An optional timeout for this request.</param>
//        Task<PushEVSEDataResult>

//            ISendData.DeleteStaticData(RoamingNetwork      RoamingNetwork,

//                                       DateTime?           Timestamp,
//                                       CancellationToken?  CancellationToken,
//                                       EventTracking_Id    EventTrackingId,
//                                       TimeSpan?           RequestTimeout)

//        {

//            #region Initial checks

//            if (RoamingNetwork == null)
//                throw new ArgumentNullException(nameof(RoamingNetwork), "The given roaming network must not be null!");

//            #endregion

//            return Task.FromResult(PushEVSEDataResult.Success(Id, this));

//        }

//        #endregion


//        #region UpdateRoamingNetworkAdminStatus(AdminStatusUpdates, TransmissionType = Enqueue, ...)

//        /// <summary>
//        /// Update the given enumeration of roaming network admin status updates.
//        /// </summary>
//        /// <param name="AdminStatusUpdates">An enumeration of roaming network admin status updates.</param>
//        /// <param name="TransmissionType">Whether to send the roaming network admin status updates directly or enqueue it for a while.</param>
//        /// 
//        /// <param name="Timestamp">The optional timestamp of the request.</param>
//        /// <param name="CancellationToken">An optional token to cancel this request.</param>
//        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
//        /// <param name="RequestTimeout">An optional timeout for this request.</param>
//        Task<PushRoamingNetworkAdminStatusResult>

//            ISendAdminStatus.UpdateAdminStatus(IEnumerable<RoamingNetworkAdminStatusUpdate>  AdminStatusUpdates,
//                                               TransmissionTypes                             TransmissionType,

//                                               DateTime?                                     Timestamp,
//                                               CancellationToken?                            CancellationToken,
//                                               EventTracking_Id                              EventTrackingId,
//                                               TimeSpan?                                     RequestTimeout)

//        {

//            #region Initial checks

//            if (AdminStatusUpdates == null || !AdminStatusUpdates.Any())
//                return Task.FromResult(PushRoamingNetworkAdminStatusResult.NoOperation(Id, this));

//            #endregion

//            return Task.FromResult(PushRoamingNetworkAdminStatusResult.OutOfService(Id,
//                                                                                    this,
//                                                                                    AdminStatusUpdates));

//        }

//        #endregion

//        #region UpdateRoamingNetworkStatus     (StatusUpdates,      TransmissionType = Enqueue, ...)

//        /// <summary>
//        /// Update the given enumeration of roaming network status updates.
//        /// </summary>
//        /// <param name="StatusUpdates">An enumeration of roaming network status updates.</param>
//        /// <param name="TransmissionType">Whether to send the roaming network status updates directly or enqueue it for a while.</param>
//        /// 
//        /// <param name="Timestamp">The optional timestamp of the request.</param>
//        /// <param name="CancellationToken">An optional token to cancel this request.</param>
//        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
//        /// <param name="RequestTimeout">An optional timeout for this request.</param>
//        Task<PushRoamingNetworkStatusResult>

//            ISendStatus.UpdateStatus(IEnumerable<RoamingNetworkStatusUpdate>  StatusUpdates,
//                                     TransmissionTypes                        TransmissionType,

//                                     DateTime?                                Timestamp,
//                                     CancellationToken?                       CancellationToken,
//                                     EventTracking_Id                         EventTrackingId,
//                                     TimeSpan?                                RequestTimeout)

//        {

//            #region Initial checks

//            if (StatusUpdates == null || !StatusUpdates.Any())
//                return Task.FromResult(PushRoamingNetworkStatusResult.NoOperation(Id, this));

//            #endregion

//            return Task.FromResult(PushRoamingNetworkStatusResult.Success(Id, this));

//        }

//        #endregion

//        #endregion

//        #endregion

//        #region AuthorizeStart/-Stop  directly...

//        #region AuthorizeStart(LocalAuthentication,                    ChargingProduct = null, SessionId = null, OperatorId = null, ...)

//        /// <summary>
//        /// Create an authorize start request.
//        /// </summary>
//        /// <param name="LocalAuthentication">An user identification.</param>
//        /// <param name="ChargingProduct">An optional charging product.</param>
//        /// <param name="SessionId">An optional session identification.</param>
//        /// <param name="OperatorId">An optional charging station operator identification.</param>
//        /// 
//        /// <param name="Timestamp">The optional timestamp of the request.</param>
//        /// <param name="CancellationToken">An optional token to cancel this request.</param>
//        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
//        /// <param name="RequestTimeout">An optional timeout for this request.</param>
//        public Task<AuthStartResult>

//            AuthorizeStart(LocalAuthentication          LocalAuthentication,
//                           ChargingProduct              ChargingProduct     = null,
//                           ChargingSession_Id?          SessionId           = null,
//                           ChargingStationOperator_Id?  OperatorId          = null,

//                           DateTime?                    Timestamp           = null,
//                           CancellationToken?           CancellationToken   = null,
//                           EventTracking_Id             EventTrackingId     = null,
//                           TimeSpan?                    RequestTimeout      = null)
//        {

//            #region Initial checks

//            if (LocalAuthentication == null)
//                throw new ArgumentNullException(nameof(LocalAuthentication),   "The given authentication token must not be null!");


//            if (!Timestamp.HasValue)
//                Timestamp = DateTime.UtcNow;

//            if (!CancellationToken.HasValue)
//                CancellationToken = new CancellationTokenSource().Token;

//            if (EventTrackingId == null)
//                EventTrackingId = EventTracking_Id.New;

//            #endregion

//            #region Send OnAuthorizeStartRequest event

//            var StartTime = DateTime.UtcNow;

//            try
//            {

//                OnAuthorizeStartRequest?.Invoke(StartTime,
//                                                Timestamp.Value,
//                                                this,
//                                                Id.ToString(),
//                                                EventTrackingId,
//                                                RoamingNetwork.Id,
//                                                OperatorId,
//                                                LocalAuthentication,
//                                                ChargingProduct,
//                                                SessionId,
//                                                RequestTimeout);

//            }
//            catch (Exception e)
//            {
//                e.Log(nameof(CSORoamingProviderLogger) + "." + nameof(OnAuthorizeStartRequest));
//            }

//            #endregion


//            var Endtime  = DateTime.UtcNow;
//            var Runtime  = Endtime - StartTime;

//            var result   = DisableAuthentication

//                               ? AuthStartResult.AdminDown(Id,
//                                                           this,
//                                                           SessionId,
//                                                           Runtime)

//                               : AuthStartResult.OutOfService(Id,
//                                                              this,
//                                                              SessionId,
//                                                              Runtime);


//            #region Send OnAuthorizeStartResponse event

//            try
//            {

//                OnAuthorizeStartResponse?.Invoke(Endtime,
//                                                 Timestamp.Value,
//                                                 this,
//                                                 Id.ToString(),
//                                                 EventTrackingId,
//                                                 RoamingNetwork.Id,
//                                                 OperatorId,
//                                                 LocalAuthentication,
//                                                 ChargingProduct,
//                                                 SessionId,
//                                                 RequestTimeout,
//                                                 result,
//                                                 Runtime);

//            }
//            catch (Exception e)
//            {
//                e.Log(nameof(CSORoamingProviderLogger) + "." + nameof(OnAuthorizeStartResponse));
//            }

//            #endregion

//            return Task.FromResult(result);

//        }

//        #endregion

//        #region AuthorizeStart(LocalAuthentication, EVSEId,            ChargingProduct = null, SessionId = null, OperatorId = null, ...)

//        /// <summary>
//        /// Create an authorize start request at the given EVSE.
//        /// </summary>
//        /// <param name="LocalAuthentication">An user identification.</param>
//        /// <param name="EVSEId">The unique identification of an EVSE.</param>
//        /// <param name="ChargingProduct">An optional charging product.</param>
//        /// <param name="SessionId">An optional session identification.</param>
//        /// <param name="OperatorId">An optional charging station operator identification.</param>
//        /// 
//        /// <param name="Timestamp">The optional timestamp of the request.</param>
//        /// <param name="CancellationToken">An optional token to cancel this request.</param>
//        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
//        /// <param name="RequestTimeout">An optional timeout for this request.</param>
//        public Task<AuthStartEVSEResult>

//            AuthorizeStart(LocalAuthentication          LocalAuthentication,
//                           WWCP.EVSE_Id                 EVSEId,
//                           ChargingProduct              ChargingProduct     = null,   // [maxlength: 100]
//                           ChargingSession_Id?          SessionId           = null,
//                           ChargingStationOperator_Id?  OperatorId          = null,

//                           DateTime?                    Timestamp           = null,
//                           CancellationToken?           CancellationToken   = null,
//                           EventTracking_Id             EventTrackingId     = null,
//                           TimeSpan?                    RequestTimeout      = null)

//        {

//            #region Initial checks

//            if (LocalAuthentication == null)
//                throw new ArgumentNullException(nameof(LocalAuthentication),  "The given authentication token must not be null!");


//            if (!Timestamp.HasValue)
//                Timestamp = DateTime.UtcNow;

//            if (!CancellationToken.HasValue)
//                CancellationToken = new CancellationTokenSource().Token;

//            if (EventTrackingId == null)
//                EventTrackingId = EventTracking_Id.New;

//            #endregion

//            #region Send OnAuthorizeEVSEStartRequest event

//            var StartTime = DateTime.UtcNow;

//            try
//            {

//                OnAuthorizeEVSEStartRequest?.Invoke(StartTime,
//                                                    Timestamp.Value,
//                                                    this,
//                                                    Id.ToString(),
//                                                    EventTrackingId,
//                                                    RoamingNetwork.Id,
//                                                    OperatorId,
//                                                    LocalAuthentication,
//                                                    EVSEId,
//                                                    ChargingProduct,
//                                                    SessionId,
//                                                    new ISendAuthorizeStartStop[0],
//                                                    RequestTimeout);

//            }
//            catch (Exception e)
//            {
//                e.Log(nameof(CSORoamingProviderLogger) + "." + nameof(OnAuthorizeEVSEStartRequest));
//            }

//            #endregion


//            var Endtime = DateTime.UtcNow;
//            var Runtime = Endtime - StartTime;

//            var result = DisableAuthentication

//                               ? AuthStartEVSEResult.AdminDown(Id,
//                                                               this,
//                                                               SessionId,
//                                                               Runtime)

//                               : AuthStartEVSEResult.OutOfService(Id,
//                                                                  this,
//                                                                  SessionId,
//                                                                  Runtime);


//            #region Send OnAuthorizeEVSEStartResponse event

//            try
//            {

//                OnAuthorizeEVSEStartResponse?.Invoke(Endtime,
//                                                     Timestamp.Value,
//                                                     this,
//                                                     Id.ToString(),
//                                                     EventTrackingId,
//                                                     RoamingNetwork.Id,
//                                                     OperatorId,
//                                                     LocalAuthentication,
//                                                     EVSEId,
//                                                     ChargingProduct,
//                                                     SessionId,
//                                                     new ISendAuthorizeStartStop[0],
//                                                     RequestTimeout,
//                                                     result,
//                                                     Runtime);

//            }
//            catch (Exception e)
//            {
//                e.Log(nameof(CSORoamingProviderLogger) + "." + nameof(OnAuthorizeEVSEStartResponse));
//            }

//            #endregion

//            return Task.FromResult(result);

//        }

//        #endregion

//        #region AuthorizeStart(LocalAuthentication, ChargingStationId, ChargingProduct = null, SessionId = null, OperatorId = null, ...)

//        /// <summary>
//        /// Create an authorize start request at the given charging station.
//        /// </summary>
//        /// <param name="LocalAuthentication">An user identification.</param>
//        /// <param name="ChargingStationId">The unique identification charging station.</param>
//        /// <param name="ChargingProduct">An optional charging product.</param>
//        /// <param name="SessionId">An optional session identification.</param>
//        /// <param name="OperatorId">An optional charging station operator identification.</param>
//        /// 
//        /// <param name="Timestamp">The optional timestamp of the request.</param>
//        /// <param name="CancellationToken">An optional token to cancel this request.</param>
//        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
//        /// <param name="RequestTimeout">An optional timeout for this request.</param>
//        public Task<AuthStartChargingStationResult>

//            AuthorizeStart(LocalAuthentication          LocalAuthentication,
//                           ChargingStation_Id           ChargingStationId,
//                           ChargingProduct              ChargingProduct     = null,   // [maxlength: 100]
//                           ChargingSession_Id?          SessionId           = null,
//                           ChargingStationOperator_Id?  OperatorId          = null,

//                           DateTime?                    Timestamp           = null,
//                           CancellationToken?           CancellationToken   = null,
//                           EventTracking_Id             EventTrackingId     = null,
//                           TimeSpan?                    RequestTimeout      = null)

//        {

//            #region Initial checks

//            if (LocalAuthentication == null)
//                throw new ArgumentNullException(nameof(LocalAuthentication), "The given authentication token must not be null!");


//            if (!Timestamp.HasValue)
//                Timestamp = DateTime.UtcNow;

//            if (!CancellationToken.HasValue)
//                CancellationToken = new CancellationTokenSource().Token;

//            if (EventTrackingId == null)
//                EventTrackingId = EventTracking_Id.New;

//            #endregion

//            #region Send OnAuthorizeChargingStationStartRequest event

//            var StartTime = DateTime.UtcNow;

//            try
//            {

//                OnAuthorizeChargingStationStartRequest?.Invoke(StartTime,
//                                                               Timestamp.Value,
//                                                               this,
//                                                               Id.ToString(),
//                                                               EventTrackingId,
//                                                               RoamingNetwork.Id,
//                                                               OperatorId,
//                                                               LocalAuthentication,
//                                                               ChargingStationId,
//                                                               ChargingProduct,
//                                                               SessionId,
//                                                               RequestTimeout);

//            }
//            catch (Exception e)
//            {
//                e.Log(nameof(CSORoamingProviderLogger) + "." + nameof(OnAuthorizeChargingStationStartRequest));
//            }

//            #endregion


//            var Endtime  = DateTime.UtcNow;
//            var Runtime  = Endtime - StartTime;

//            var result   = DisableAuthentication

//                               ? AuthStartChargingStationResult.AdminDown(Id,
//                                                                          this,
//                                                                          SessionId,
//                                                                          Runtime)

//                               : AuthStartChargingStationResult.OutOfService(Id,
//                                                                             this,
//                                                                             SessionId,
//                                                                             Runtime);


//            #region Send OnAuthorizeChargingStationStartResponse event

//            try
//            {

//                OnAuthorizeChargingStationStartResponse?.Invoke(Endtime,
//                                                                Timestamp.Value,
//                                                                this,
//                                                                Id.ToString(),
//                                                                EventTrackingId,
//                                                                RoamingNetwork.Id,
//                                                                OperatorId,
//                                                                LocalAuthentication,
//                                                                ChargingStationId,
//                                                                ChargingProduct,
//                                                                SessionId,
//                                                                RequestTimeout,
//                                                                result,
//                                                                Runtime);

//            }
//            catch (Exception e)
//            {
//                e.Log(nameof(CSORoamingProviderLogger) + "." + nameof(OnAuthorizeChargingStationStartResponse));
//            }

//            #endregion

//            return Task.FromResult(result);

//        }

//        #endregion

//        #region AuthorizeStart(LocalAuthentication, ChargingPoolId,    ChargingProduct = null, SessionId = null, OperatorId = null, ...)

//        /// <summary>
//        /// Create an authorize start request at the given charging pool.
//        /// </summary>
//        /// <param name="LocalAuthentication">An user identification.</param>
//        /// <param name="ChargingPoolId">The unique identification charging pool.</param>
//        /// <param name="ChargingProduct">An optional charging product.</param>
//        /// <param name="SessionId">An optional session identification.</param>
//        /// <param name="OperatorId">An optional charging station operator identification.</param>
//        /// 
//        /// <param name="Timestamp">The optional timestamp of the request.</param>
//        /// <param name="CancellationToken">An optional token to cancel this request.</param>
//        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
//        /// <param name="RequestTimeout">An optional timeout for this request.</param>
//        public Task<AuthStartChargingPoolResult>

//            AuthorizeStart(LocalAuthentication          LocalAuthentication,
//                           ChargingPool_Id              ChargingPoolId,
//                           ChargingProduct              ChargingProduct     = null,   // [maxlength: 100]
//                           ChargingSession_Id?          SessionId           = null,
//                           ChargingStationOperator_Id?  OperatorId          = null,

//                           DateTime?                    Timestamp           = null,
//                           CancellationToken?           CancellationToken   = null,
//                           EventTracking_Id             EventTrackingId     = null,
//                           TimeSpan?                    RequestTimeout      = null)

//        {

//            #region Initial checks

//            if (LocalAuthentication == null)
//                throw new ArgumentNullException(nameof(LocalAuthentication), "The given authentication token must not be null!");


//            if (!Timestamp.HasValue)
//                Timestamp = DateTime.UtcNow;

//            if (!CancellationToken.HasValue)
//                CancellationToken = new CancellationTokenSource().Token;

//            if (EventTrackingId == null)
//                EventTrackingId = EventTracking_Id.New;

//            #endregion

//            #region Send OnAuthorizeChargingPoolStartRequest event

//            var StartTime = DateTime.UtcNow;

//            try
//            {

//                OnAuthorizeChargingPoolStartRequest?.Invoke(StartTime,
//                                                            Timestamp.Value,
//                                                            this,
//                                                            Id.ToString(),
//                                                            EventTrackingId,
//                                                            RoamingNetwork.Id,
//                                                            OperatorId,
//                                                            LocalAuthentication,
//                                                            ChargingPoolId,
//                                                            ChargingProduct,
//                                                            SessionId,
//                                                            RequestTimeout);

//            }
//            catch (Exception e)
//            {
//                e.Log(nameof(CSORoamingProviderLogger) + "." + nameof(OnAuthorizeChargingPoolStartRequest));
//            }

//            #endregion


//            var Endtime  = DateTime.UtcNow;
//            var Runtime  = Endtime - StartTime;

//            var result   = DisableAuthentication

//                               ? AuthStartChargingPoolResult.AdminDown(Id,
//                                                                       this,
//                                                                       SessionId,
//                                                                       Runtime)

//                               : AuthStartChargingPoolResult.OutOfService(Id,
//                                                                          this,
//                                                                          SessionId,
//                                                                          Runtime);


//            #region Send OnAuthorizeChargingPoolStartResponse event

//            try
//            {

//                OnAuthorizeChargingPoolStartResponse?.Invoke(Endtime,
//                                                             Timestamp.Value,
//                                                             this,
//                                                             Id.ToString(),
//                                                             EventTrackingId,
//                                                             RoamingNetwork.Id,
//                                                             OperatorId,
//                                                             LocalAuthentication,
//                                                             ChargingPoolId,
//                                                             ChargingProduct,
//                                                             SessionId,
//                                                             RequestTimeout,
//                                                             result,
//                                                             Runtime);

//            }
//            catch (Exception e)
//            {
//                e.Log(nameof(CSORoamingProviderLogger) + "." + nameof(OnAuthorizeChargingPoolStartResponse));
//            }

//            #endregion

//            return Task.FromResult(result);

//        }

//        #endregion


//        // UID => Not everybody can stop any session, but maybe another
//        //        UID than the UID which started the session!
//        //        (e.g. car sharing)

//        #region AuthorizeStop(SessionId, LocalAuthentication,                    OperatorId = null, ...)

//        /// <summary>
//        /// Create an authorize stop request.
//        /// </summary>
//        /// <param name="SessionId">The session identification from the AuthorizeStart request.</param>
//        /// <param name="LocalAuthentication">An user identification.</param>
//        /// <param name="OperatorId">An optional charging station operator identification.</param>
//        /// 
//        /// <param name="Timestamp">The optional timestamp of the request.</param>
//        /// <param name="CancellationToken">An optional token to cancel this request.</param>
//        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
//        /// <param name="RequestTimeout">An optional timeout for this request.</param>
//        public Task<AuthStopResult>

//            AuthorizeStop(ChargingSession_Id           SessionId,
//                          LocalAuthentication          LocalAuthentication,
//                          ChargingStationOperator_Id?  OperatorId          = null,

//                          DateTime?                    Timestamp           = null,
//                          CancellationToken?           CancellationToken   = null,
//                          EventTracking_Id             EventTrackingId     = null,
//                          TimeSpan?                    RequestTimeout      = null)
//        {

//            #region Initial checks

//            if (LocalAuthentication == null)
//                throw new ArgumentNullException(nameof(LocalAuthentication),  "The given authentication token must not be null!");


//            if (!Timestamp.HasValue)
//                Timestamp = DateTime.UtcNow;

//            if (!CancellationToken.HasValue)
//                CancellationToken = new CancellationTokenSource().Token;

//            if (EventTrackingId == null)
//                EventTrackingId = EventTracking_Id.New;

//            #endregion

//            #region Send OnAuthorizeStopRequest event

//            var StartTime = DateTime.UtcNow;

//            try
//            {

//                OnAuthorizeStopRequest?.Invoke(StartTime,
//                                               Timestamp.Value,
//                                               this,
//                                               Id.ToString(),
//                                               EventTrackingId,
//                                               RoamingNetwork.Id,
//                                               OperatorId,
//                                               SessionId,
//                                               LocalAuthentication,
//                                               RequestTimeout);

//            }
//            catch (Exception e)
//            {
//                e.Log(nameof(CSORoamingProviderLogger) + "." + nameof(OnAuthorizeStopRequest));
//            }

//            #endregion


//            var Endtime  = DateTime.UtcNow;
//            var Runtime  = Endtime - StartTime;

//            var result   = DisableAuthentication

//                               ? AuthStopResult.AdminDown(Id,
//                                                          this,
//                                                          SessionId,
//                                                          Runtime)

//                               : AuthStopResult.OutOfService(Id,
//                                                             this,
//                                                             SessionId,
//                                                             Runtime);


//            #region Send OnAuthorizeStopResponse event

//            try
//            {

//                OnAuthorizeStopResponse?.Invoke(Endtime,
//                                                Timestamp.Value,
//                                                this,
//                                                Id.ToString(),
//                                                EventTrackingId,
//                                                RoamingNetwork.Id,
//                                                OperatorId,
//                                                SessionId,
//                                                LocalAuthentication,
//                                                RequestTimeout,
//                                                result,
//                                                Runtime);

//            }
//            catch (Exception e)
//            {
//                e.Log(nameof(CSORoamingProviderLogger) + "." + nameof(OnAuthorizeStopResponse));
//            }

//            #endregion

//            return Task.FromResult(result);

//        }

//        #endregion

//        #region AuthorizeStop(SessionId, LocalAuthentication, EVSEId,            OperatorId = null, ...)

//        /// <summary>
//        /// Create an authorize stop request at the given EVSE.
//        /// </summary>
//        /// <param name="SessionId">The session identification from the AuthorizeStart request.</param>
//        /// <param name="LocalAuthentication">A (RFID) user identification.</param>
//        /// <param name="EVSEId">The unique identification of an EVSE.</param>
//        /// <param name="OperatorId">An optional charging station operator identification.</param>
//        /// 
//        /// <param name="Timestamp">The optional timestamp of the request.</param>
//        /// <param name="CancellationToken">An optional token to cancel this request.</param>
//        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
//        /// <param name="RequestTimeout">An optional timeout for this request.</param>
//        public Task<AuthStopEVSEResult>

//            AuthorizeStop(ChargingSession_Id           SessionId,
//                          LocalAuthentication          LocalAuthentication,
//                          EVSE_Id                      EVSEId,
//                          ChargingStationOperator_Id?  OperatorId          = null,

//                          DateTime?                    Timestamp           = null,
//                          CancellationToken?           CancellationToken   = null,
//                          EventTracking_Id             EventTrackingId     = null,
//                          TimeSpan?                    RequestTimeout      = null)
//        {

//            #region Initial checks

//            if (LocalAuthentication  == null)
//                throw new ArgumentNullException(nameof(LocalAuthentication), "The given authentication token must not be null!");


//            if (!Timestamp.HasValue)
//                Timestamp = DateTime.UtcNow;

//            if (!CancellationToken.HasValue)
//                CancellationToken = new CancellationTokenSource().Token;

//            if (EventTrackingId == null)
//                EventTrackingId = EventTracking_Id.New;

//            #endregion

//            #region Send OnAuthorizeEVSEStopRequest event

//            var StartTime = DateTime.UtcNow;

//            try
//            {

//                OnAuthorizeEVSEStopRequest?.Invoke(StartTime,
//                                                   Timestamp.Value,
//                                                   this,
//                                                   Id.ToString(),
//                                                   EventTrackingId,
//                                                   RoamingNetwork.Id,
//                                                   OperatorId,
//                                                   EVSEId,
//                                                   SessionId,
//                                                   LocalAuthentication,
//                                                   RequestTimeout);

//            }
//            catch (Exception e)
//            {
//                e.Log(nameof(CSORoamingProviderLogger) + "." + nameof(OnAuthorizeEVSEStopRequest));
//            }

//            #endregion


//            var Endtime  = DateTime.UtcNow;
//            var Runtime  = Endtime - StartTime;

//            var result   = DisableAuthentication

//                               ? AuthStopEVSEResult.AdminDown(Id,
//                                                              this,
//                                                              SessionId,
//                                                              Runtime)

//                               : AuthStopEVSEResult.OutOfService(Id,
//                                                                 this,
//                                                                 SessionId,
//                                                                 Runtime);


//            #region Send OnAuthorizeEVSEStopResponse event

//            try
//            {

//                OnAuthorizeEVSEStopResponse?.Invoke(Endtime,
//                                                    Timestamp.Value,
//                                                    this,
//                                                    Id.ToString(),
//                                                    EventTrackingId,
//                                                    RoamingNetwork.Id,
//                                                    OperatorId,
//                                                    EVSEId,
//                                                    SessionId,
//                                                    LocalAuthentication,
//                                                    RequestTimeout,
//                                                    result,
//                                                    Runtime);

//            }
//            catch (Exception e)
//            {
//                e.Log(nameof(CSORoamingProviderLogger) + "." + nameof(OnAuthorizeEVSEStopResponse));
//            }

//            #endregion

//            return Task.FromResult(result);

//        }

//        #endregion

//        #region AuthorizeStop(SessionId, LocalAuthentication, ChargingStationId, OperatorId = null, ...)

//        /// <summary>
//        /// Create an authorize stop request at the given charging station.
//        /// </summary>
//        /// <param name="SessionId">The session identification from the AuthorizeStart request.</param>
//        /// <param name="LocalAuthentication">An user identification.</param>
//        /// <param name="ChargingStationId">The unique identification of a charging station.</param>
//        /// <param name="OperatorId">An optional charging station operator identification.</param>
//        /// 
//        /// <param name="Timestamp">The optional timestamp of the request.</param>
//        /// <param name="CancellationToken">An optional token to cancel this request.</param>
//        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
//        /// <param name="RequestTimeout">An optional timeout for this request.</param>
//        public Task<AuthStopChargingStationResult>

//            AuthorizeStop(ChargingSession_Id           SessionId,
//                          LocalAuthentication          LocalAuthentication,
//                          ChargingStation_Id           ChargingStationId,
//                          ChargingStationOperator_Id?  OperatorId          = null,

//                          DateTime?                    Timestamp           = null,
//                          CancellationToken?           CancellationToken   = null,
//                          EventTracking_Id             EventTrackingId     = null,
//                          TimeSpan?                    RequestTimeout      = null)

//        {

//            #region Initial checks

//            if (LocalAuthentication == null)
//                throw new ArgumentNullException(nameof(LocalAuthentication), "The given authentication token must not be null!");


//            if (!Timestamp.HasValue)
//                Timestamp = DateTime.UtcNow;

//            if (!CancellationToken.HasValue)
//                CancellationToken = new CancellationTokenSource().Token;

//            if (EventTrackingId == null)
//                EventTrackingId = EventTracking_Id.New;

//            #endregion

//            #region Send OnAuthorizeChargingStationStopRequest event

//            var StartTime = DateTime.UtcNow;

//            try
//            {

//                OnAuthorizeChargingStationStopRequest?.Invoke(StartTime,
//                                                              Timestamp.Value,
//                                                              this,
//                                                              Id.ToString(),
//                                                              EventTrackingId,
//                                                              RoamingNetwork.Id,
//                                                              OperatorId,
//                                                              ChargingStationId,
//                                                              SessionId,
//                                                              LocalAuthentication,
//                                                              RequestTimeout);

//            }
//            catch (Exception e)
//            {
//                e.Log(nameof(CSORoamingProviderLogger) + "." + nameof(OnAuthorizeChargingStationStopRequest));
//            }

//            #endregion


//            var Endtime  = DateTime.UtcNow;
//            var Runtime  = Endtime - StartTime;

//            var result   = DisableAuthentication

//                               ? AuthStopChargingStationResult.AdminDown(Id,
//                                                                         this,
//                                                                         SessionId,
//                                                                         Runtime)

//                               : AuthStopChargingStationResult.OutOfService(Id,
//                                                                            this,
//                                                                            SessionId,
//                                                                            Runtime);


//            #region Send OnAuthorizeChargingStationStopResponse event

//            try
//            {

//                OnAuthorizeChargingStationStopResponse?.Invoke(Endtime,
//                                                               Timestamp.Value,
//                                                               this,
//                                                               Id.ToString(),
//                                                               EventTrackingId,
//                                                               RoamingNetwork.Id,
//                                                               OperatorId,
//                                                               ChargingStationId,
//                                                               SessionId,
//                                                               LocalAuthentication,
//                                                               RequestTimeout,
//                                                               result,
//                                                               Runtime);

//            }
//            catch (Exception e)
//            {
//                e.Log(nameof(CSORoamingProviderLogger) + "." + nameof(OnAuthorizeChargingStationStopResponse));
//            }

//            #endregion

//            return Task.FromResult(result);

//        }

//        #endregion

//        #region AuthorizeStop(SessionId, LocalAuthentication, ChargingPoolId,    OperatorId = null, ...)

//        /// <summary>
//        /// Create an authorize stop request at the given charging pool.
//        /// </summary>
//        /// <param name="SessionId">The session identification from the AuthorizeStart request.</param>
//        /// <param name="LocalAuthentication">An user identification.</param>
//        /// <param name="ChargingPoolId">The unique identification of a charging pool.</param>
//        /// <param name="OperatorId">An optional charging station operator identification.</param>
//        /// 
//        /// <param name="Timestamp">The optional timestamp of the request.</param>
//        /// <param name="CancellationToken">An optional token to cancel this request.</param>
//        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
//        /// <param name="RequestTimeout">An optional timeout for this request.</param>
//        public Task<AuthStopChargingPoolResult>

//            AuthorizeStop(ChargingSession_Id           SessionId,
//                          LocalAuthentication          LocalAuthentication,
//                          ChargingPool_Id              ChargingPoolId,
//                          ChargingStationOperator_Id?  OperatorId          = null,

//                          DateTime?                    Timestamp           = null,
//                          CancellationToken?           CancellationToken   = null,
//                          EventTracking_Id             EventTrackingId     = null,
//                          TimeSpan?                    RequestTimeout      = null)

//        {

//            #region Initial checks

//            if (LocalAuthentication == null)
//                throw new ArgumentNullException(nameof(LocalAuthentication), "The given authentication token must not be null!");


//            if (!Timestamp.HasValue)
//                Timestamp = DateTime.UtcNow;

//            if (!CancellationToken.HasValue)
//                CancellationToken = new CancellationTokenSource().Token;

//            if (EventTrackingId == null)
//                EventTrackingId = EventTracking_Id.New;

//            #endregion

//            #region Send OnAuthorizeChargingPoolStopRequest event

//            var StartTime = DateTime.UtcNow;

//            try
//            {

//                OnAuthorizeChargingPoolStopRequest?.Invoke(StartTime,
//                                                           Timestamp.Value,
//                                                           this,
//                                                           Id.ToString(),
//                                                           EventTrackingId,
//                                                           RoamingNetwork.Id,
//                                                           OperatorId,
//                                                           ChargingPoolId,
//                                                           SessionId,
//                                                           LocalAuthentication,
//                                                           RequestTimeout);

//            }
//            catch (Exception e)
//            {
//                e.Log(nameof(CSORoamingProviderLogger) + "." + nameof(OnAuthorizeChargingPoolStopRequest));
//            }

//            #endregion


//            var Endtime  = DateTime.UtcNow;
//            var Runtime  = Endtime - StartTime;

//            var result   = DisableAuthentication

//                               ? AuthStopChargingPoolResult.AdminDown(Id,
//                                                                      this,
//                                                                      SessionId,
//                                                                      Runtime)

//                               : AuthStopChargingPoolResult.OutOfService(Id,
//                                                                         this,
//                                                                         SessionId,
//                                                                         Runtime);


//            #region Send OnAuthorizeChargingPoolStopResponse event

//            try
//            {

//                OnAuthorizeChargingPoolStopResponse?.Invoke(Endtime,
//                                                            Timestamp.Value,
//                                                            this,
//                                                            Id.ToString(),
//                                                            EventTrackingId,
//                                                            RoamingNetwork.Id,
//                                                            OperatorId,
//                                                            ChargingPoolId,
//                                                            SessionId,
//                                                            LocalAuthentication,
//                                                            RequestTimeout,
//                                                            result,
//                                                            Runtime);

//            }
//            catch (Exception e)
//            {
//                e.Log(nameof(CSORoamingProviderLogger) + "." + nameof(OnAuthorizeChargingPoolStopResponse));
//            }

//            #endregion

//            return Task.FromResult(result);

//        }

//        #endregion

//        #endregion

//        #region SendChargeDetailRecords(ChargeDetailRecords, TransmissionType = Enqueue, ...)

//        /// <summary>
//        /// Send charge detail records to an OICP server.
//        /// </summary>
//        /// <param name="ChargeDetailRecords">An enumeration of charge detail records.</param>
//        /// <param name="TransmissionType">Whether to send the CDR directly or enqueue it for a while.</param>
//        /// 
//        /// <param name="Timestamp">The optional timestamp of the request.</param>
//        /// <param name="CancellationToken">An optional token to cancel this request.</param>
//        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
//        /// <param name="RequestTimeout">An optional timeout for this request.</param>
//        public Task<SendCDRsResult>

//            SendChargeDetailRecords(IEnumerable<ChargeDetailRecord>  ChargeDetailRecords,
//                                    TransmissionTypes                TransmissionType    = TransmissionTypes.Enqueue,

//                                    DateTime?                        Timestamp           = null,
//                                    CancellationToken?               CancellationToken   = null,
//                                    EventTracking_Id                 EventTrackingId     = null,
//                                    TimeSpan?                        RequestTimeout      = null)

//        {

//            #region Initial checks

//            if (ChargeDetailRecords == null)
//                throw new ArgumentNullException(nameof(ChargeDetailRecords),  "The given enumeration of charge detail records must not be null!");


//            if (!Timestamp.HasValue)
//                Timestamp = DateTime.UtcNow;

//            if (!CancellationToken.HasValue)
//                CancellationToken = new CancellationTokenSource().Token;

//            if (EventTrackingId == null)
//                EventTrackingId = EventTracking_Id.New;

//            #endregion

//            #region Enqueue, if requested...

//            if (TransmissionType == TransmissionTypes.Enqueue)
//            {

//                #region Send OnEnqueueSendCDRRequest event

//                try
//                {

//                    OnEnqueueSendCDRsRequest?.Invoke(DateTime.UtcNow,
//                                                     Timestamp.Value,
//                                                     this,
//                                                     Id.ToString(),
//                                                     EventTrackingId,
//                                                     RoamingNetwork.Id,
//                                                     new ChargeDetailRecord[0],
//                                                     ChargeDetailRecords,
//                                                     RequestTimeout);

//                }
//                catch (Exception e)
//                {
//                    e.Log(nameof(CSORoamingProviderLogger) + "." + nameof(OnSendCDRsRequest));
//                }

//                #endregion

//                lock(DataAndStatusLockOld)
//                {

//                    ChargeDetailRecordQueue.AddRange(ChargeDetailRecords);

//                    FlushChargeDetailRecordsTimer.Change(FlushChargeDetailRecordsEvery, TimeSpan.FromMilliseconds(-1));

//                }

//                return Task.FromResult(SendCDRsResult.Enqueued(Id, this, ChargeDetailRecords));

//            }

//            #endregion

//            #region Send OnSendCDRsRequest event

//            var StartTime = DateTime.UtcNow;

//            try
//            {

//                OnSendCDRsRequest?.Invoke(StartTime,
//                                          Timestamp.Value,
//                                          this,
//                                          Id.ToString(),
//                                          EventTrackingId,
//                                          RoamingNetwork.Id,
//                                          new ChargeDetailRecord[0],
//                                          ChargeDetailRecords,
//                                          RequestTimeout);

//            }
//            catch (Exception e)
//            {
//                e.Log(nameof(CSORoamingProviderLogger) + "." + nameof(OnSendCDRsRequest));
//            }

//            #endregion


//            var Endtime  = DateTime.UtcNow;
//            var Runtime  = Endtime - StartTime;

//            var result   = DisableAuthentication

//                               ? SendCDRsResult.AdminDown   (Id,
//                                                             this,
//                                                             ChargeDetailRecords,
//                                                             Runtime: Runtime)

//                               : SendCDRsResult.OutOfService(Id,
//                                                             this,
//                                                             ChargeDetailRecords,
//                                                             Runtime: Runtime);


//            #region Send OnSendCDRsResponse event

//            try
//            {

//                OnSendCDRsResponse?.Invoke(Endtime,
//                                           Timestamp.Value,
//                                           this,
//                                           Id.ToString(),
//                                           EventTrackingId,
//                                           RoamingNetwork.Id,
//                                           new ChargeDetailRecord[0],
//                                           ChargeDetailRecords,
//                                           RequestTimeout,
//                                           result,
//                                           Runtime);

//            }
//            catch (Exception e)
//            {
//                e.Log(nameof(CSORoamingProviderLogger) + "." + nameof(OnSendCDRsResponse));
//            }

//            #endregion

//            return Task.FromResult(result);

//        }

//        #endregion


//        // -----------------------------------------------------------------------------------------------------


//        #region (timer) FlushEVSEDataAndStatus()

//        protected override Boolean SkipFlushEVSEDataAndStatusQueues()
//            => EVSEsToAddQueue.              Count == 0 &&
//               EVSEsToUpdateQueue.           Count == 0 &&
//               EVSEStatusChangesDelayedQueue.Count == 0 &&
//               EVSEsToRemoveQueue.           Count == 0;

//        protected override async Task FlushEVSEDataAndStatusQueues()
//        {

//            #region Get a copy of all current EVSE data and delayed status

//            var EVSEsToAddQueueCopy                = new HashSet<EVSE>();
//            var EVSEsToUpdateQueueCopy             = new HashSet<EVSE>();
//            var EVSEStatusChangesDelayedQueueCopy  = new List<EVSEStatusUpdate>();
//            var EVSEsToRemoveQueueCopy             = new HashSet<EVSE>();

//            var EVSEsUpdateLogCopy                 = new Dictionary<EVSE,            PropertyUpdateInfos[]>();
//            var ChargingStationsUpdateLogCopy      = new Dictionary<ChargingStation, PropertyUpdateInfos[]>();
//            var ChargingPoolsUpdateLogCopy         = new Dictionary<ChargingPool,    PropertyUpdateInfos[]>();

//            lock(DataAndStatusLockOld)
//            {

//                // Copy 'EVSEs to add', remove originals...
//                EVSEsToAddQueueCopy                      = new HashSet<EVSE>                (EVSEsToAddQueue);
//                EVSEsToAddQueue.Clear();

//                // Copy 'EVSEs to update', remove originals...
//                EVSEsToUpdateQueueCopy                   = new HashSet<EVSE>                (EVSEsToUpdateQueue);
//                EVSEsToUpdateQueue.Clear();

//                // Copy 'EVSE status changes', remove originals...
//                EVSEStatusChangesDelayedQueueCopy        = new List<EVSEStatusUpdate>       (EVSEStatusChangesDelayedQueue);
//                EVSEStatusChangesDelayedQueueCopy.AddRange(EVSEsToAddQueueCopy.SafeSelect(evse => new EVSEStatusUpdate(evse, evse.Status, evse.Status)));
//                EVSEStatusChangesDelayedQueue.Clear();

//                // Copy 'EVSEs to remove', remove originals...
//                EVSEsToRemoveQueueCopy                   = new HashSet<EVSE>                (EVSEsToRemoveQueue);
//                EVSEsToRemoveQueue.Clear();

//                // Copy EVSE property updates
//                EVSEsUpdateLog.           ForEach(_ => EVSEsUpdateLogCopy.           Add(_.Key, _.Value.ToArray()));
//                EVSEsUpdateLog.Clear();

//                // Copy charging station property updates
//                ChargingStationsUpdateLog.ForEach(_ => ChargingStationsUpdateLogCopy.Add(_.Key, _.Value.ToArray()));
//                ChargingStationsUpdateLog.Clear();

//                // Copy charging pool property updates
//                ChargingPoolsUpdateLog.   ForEach(_ => ChargingPoolsUpdateLogCopy.   Add(_.Key, _.Value.ToArray()));
//                ChargingPoolsUpdateLog.Clear();


//                // Stop the timer. Will be rescheduled by next EVSE data/status change...
//                FlushEVSEDataAndStatusTimer.Change(TimeSpan.FromMilliseconds(-1), TimeSpan.FromMilliseconds(-1));

//            }

//            #endregion


//            // Use events to check if something went wrong!
//            var EventTrackingId = EventTracking_Id.New;

//            Thread.Sleep(30000);

//            #region Send new EVSE data

//            if (EVSEsToAddQueueCopy.Count > 0)
//            {

//                //var EVSEsToAddTask = PushEVSEData(EVSEsToAddQueueCopy,
//                //                                  _FlushEVSEDataRunId == 1
//                //                                      ? ActionTypes.fullLoad
//                //                                      : ActionTypes.update,
//                //                                  EventTrackingId: EventTrackingId);

//                //EVSEsToAddTask.Wait();

//            }

//            #endregion

//            #region Send changed EVSE data

//            if (EVSEsToUpdateQueueCopy.Count > 0)
//            {

//                // Surpress EVSE data updates for all newly added EVSEs
//                var EVSEsWithoutNewEVSEs = EVSEsToUpdateQueueCopy.
//                                               Where(evse => !EVSEsToAddQueueCopy.Contains(evse)).
//                                               ToArray();


//                if (EVSEsWithoutNewEVSEs.Length > 0)
//                {

//                    //var PushEVSEDataTask = PushEVSEData(EVSEsWithoutNewEVSEs,
//                    //                                    ActionTypes.update,
//                    //                                    EventTrackingId: EventTrackingId);

//                    //PushEVSEDataTask.Wait();

//                }

//            }

//            #endregion

//            #region Send changed EVSE status

//            if (!DisablePushStatus &&
//                EVSEStatusChangesDelayedQueueCopy.Count > 0)
//            {

//                //var PushEVSEStatusTask = PushEVSEStatus(EVSEStatusChangesDelayedQueueCopy,
//                //                                        _FlushEVSEDataRunId == 1
//                //                                            ? ActionTypes.fullLoad
//                //                                            : ActionTypes.update,
//                //                                        EventTrackingId: EventTrackingId);

//                //PushEVSEStatusTask.Wait();

//            }

//            #endregion

//            #region Send removed charging stations

//            if (EVSEsToRemoveQueueCopy.Count > 0)
//            {

//                var EVSEsToRemove = EVSEsToRemoveQueueCopy.ToArray();

//                if (EVSEsToRemove.Length > 0)
//                {

//                    //var EVSEsToRemoveTask = PushEVSEData(EVSEsToRemove,
//                    //                                     ActionTypes.delete,
//                    //                                     EventTrackingId: EventTrackingId);

//                    //EVSEsToRemoveTask.Wait();

//                }

//            }

//            #endregion

//        }

//        #endregion

//        #region (timer) FlushEVSEFastStatus()

//        protected override Boolean SkipFlushEVSEFastStatusQueues()
//            => EVSEStatusChangesFastQueue.Count == 0;

//        protected override async Task FlushEVSEFastStatusQueues()
//        {

//            #region Get a copy of all current EVSE data and delayed status

//            var EVSEStatusFastQueueCopy = new List<EVSEStatusUpdate>();

//            lock(DataAndStatusLockOld)
//            {

//                // Copy 'EVSE status changes', remove originals...
//                EVSEStatusFastQueueCopy = new List<EVSEStatusUpdate>(EVSEStatusChangesFastQueue.Where(evsestatuschange => !EVSEsToAddQueue.Any(evse => evse == evsestatuschange.EVSE)));

//                // Add all evse status changes of EVSE *NOT YET UPLOADED* into the delayed queue...
//                var EVSEStatusChangesDelayed = EVSEStatusChangesFastQueue.Where(evsestatuschange => EVSEsToAddQueue.Any(evse => evse == evsestatuschange.EVSE)).ToArray();

//                if (EVSEStatusChangesDelayed.Length > 0)
//                    EVSEStatusChangesDelayedQueue.AddRange(EVSEStatusChangesDelayed);

//                EVSEStatusChangesFastQueue.Clear();

//                // Stop the timer. Will be rescheduled by next EVSE status change...
//                FlushEVSEFastStatusTimer.Change(TimeSpan.FromMilliseconds(-1), TimeSpan.FromMilliseconds(-1));

//            }

//            #endregion


//            // Use events to check if something went wrong!
//            var EventTrackingId = EventTracking_Id.New;

//            #region Send changed EVSE status

//            if (EVSEStatusFastQueueCopy.Count > 0)
//            {

//                //var _PushEVSEStatus = await PushEVSEStatus(EVSEStatusFastQueueCopy,
//                //                                           ActionTypes.update,
//                //                                           EventTrackingId: EventTrackingId).
//                //                            ConfigureAwait(false);

//            }

//            #endregion

//        }

//        #endregion

//        #region (timer) FlushChargeDetailRecords()

//        protected override Boolean SkipFlushChargeDetailRecordsQueues()
//            => ChargeDetailRecordQueue.Count == 0;

//        protected override async Task FlushChargeDetailRecordsQueues()
//        {

//            #region Make a thread local copy of all data

//            var ChargeDetailRecordQueueCopy        = new ThreadLocal<List<WWCP.ChargeDetailRecord>>();

//            if (Monitor.TryEnter(FlushEVSEDataAndStatusLock))
//            {

//                try
//                {

//                    if (ChargeDetailRecordQueue.Count == 0)
//                        return;

//                    _FlushEVSEDataRunId++;

//                    // Copy 'EVSEs to remove', remove originals...
//                    ChargeDetailRecordQueueCopy.Value = new List<ChargeDetailRecord>(ChargeDetailRecordQueue);
//                    ChargeDetailRecordQueue.Clear();

//                    // Stop the timer. Will be rescheduled by next EVSE data/status change...
//                    FlushEVSEDataAndStatusTimer.Change(TimeSpan.FromMilliseconds(-1), TimeSpan.FromMilliseconds(-1));

//                }
//                catch (Exception e)
//                {

//                    while (e.InnerException != null)
//                        e = e.InnerException;

//                    DebugX.LogT(nameof(CSORoamingProviderLogger) + " '" + Id + "' led to an exception: " + e.Message + Environment.NewLine + e.StackTrace);

//                }

//                finally
//                {
//                    Monitor.Exit(FlushEVSEDataAndStatusLock);
//                }

//            }

//            else
//            {

//                Console.WriteLine("ServiceCheckLock missed!");
//                FlushEVSEDataAndStatusTimer.Change(FlushEVSEDataAndStatusEvery, TimeSpan.FromMilliseconds(-1));

//            }

//            #endregion


//            // Upload status changes...
//            if (ChargeDetailRecordQueueCopy.Value != null)
//            {

//                // Use the events to evaluate if something went wrong!

//                var EventTrackingId = EventTracking_Id.New;

//                #region Send charge detail records

//                if (ChargeDetailRecordQueueCopy.Value.Count > 0)
//                {

//                    //var SendCDRResults = await SendChargeDetailRecords(ChargeDetailRecordQueueCopy.Value,
//                    //                                                   TransmissionTypes.Direct,
//                    //                                                   DateTime.UtcNow,
//                    //                                                   new CancellationTokenSource().Token,
//                    //                                                   EventTrackingId,
//                    //                                                   DefaultRequestTimeout).
//                    //                           ConfigureAwait(false);

//                    //ToDo: Send results events...
//                    //ToDo: Read to queue if it could not be sent...

//                }

//                #endregion

//            }

//        }

//        #endregion


//        // -----------------------------------------------------------------------------------------------------


//        #region Operator overloading

//        #region Operator == (CSORoamingProviderLogger1, CSORoamingProviderLogger2)

//        /// <summary>
//        /// Compares two CSORoamingProviderLoggers for equality.
//        /// </summary>
//        /// <param name="CSORoamingProviderLogger1">A CSORoamingProviderLogger.</param>
//        /// <param name="CSORoamingProviderLogger2">Another CSORoamingProviderLogger.</param>
//        /// <returns>True if both match; False otherwise.</returns>
//        public static Boolean operator == (CSORoamingProviderLogger CSORoamingProviderLogger1, CSORoamingProviderLogger CSORoamingProviderLogger2)
//        {

//            // If both are null, or both are same instance, return true.
//            if (Object.ReferenceEquals(CSORoamingProviderLogger1, CSORoamingProviderLogger2))
//                return true;

//            // If one is null, but not both, return false.
//            if (((Object) CSORoamingProviderLogger1 == null) || ((Object) CSORoamingProviderLogger2 == null))
//                return false;

//            return CSORoamingProviderLogger1.Equals(CSORoamingProviderLogger2);

//        }

//        #endregion

//        #region Operator != (CSORoamingProviderLogger1, CSORoamingProviderLogger2)

//        /// <summary>
//        /// Compares two CSORoamingProviderLoggers for inequality.
//        /// </summary>
//        /// <param name="CSORoamingProviderLogger1">A CSORoamingProviderLogger.</param>
//        /// <param name="CSORoamingProviderLogger2">Another CSORoamingProviderLogger.</param>
//        /// <returns>False if both match; True otherwise.</returns>
//        public static Boolean operator != (CSORoamingProviderLogger CSORoamingProviderLogger1, CSORoamingProviderLogger CSORoamingProviderLogger2)

//            => !(CSORoamingProviderLogger1 == CSORoamingProviderLogger2);

//        #endregion

//        #region Operator <  (CSORoamingProviderLogger1, CSORoamingProviderLogger2)

//        /// <summary>
//        /// Compares two instances of this object.
//        /// </summary>
//        /// <param name="CSORoamingProviderLogger1">A CSORoamingProviderLogger.</param>
//        /// <param name="CSORoamingProviderLogger2">Another CSORoamingProviderLogger.</param>
//        /// <returns>true|false</returns>
//        public static Boolean operator < (CSORoamingProviderLogger  CSORoamingProviderLogger1,
//                                          CSORoamingProviderLogger  CSORoamingProviderLogger2)
//        {

//            if ((Object) CSORoamingProviderLogger1 == null)
//                throw new ArgumentNullException(nameof(CSORoamingProviderLogger1),  "The given CSORoamingProviderLogger must not be null!");

//            return CSORoamingProviderLogger1.CompareTo(CSORoamingProviderLogger2) < 0;

//        }

//        #endregion

//        #region Operator <= (CSORoamingProviderLogger1, CSORoamingProviderLogger2)

//        /// <summary>
//        /// Compares two instances of this object.
//        /// </summary>
//        /// <param name="CSORoamingProviderLogger1">A CSORoamingProviderLogger.</param>
//        /// <param name="CSORoamingProviderLogger2">Another CSORoamingProviderLogger.</param>
//        /// <returns>true|false</returns>
//        public static Boolean operator <= (CSORoamingProviderLogger CSORoamingProviderLogger1,
//                                           CSORoamingProviderLogger CSORoamingProviderLogger2)

//            => !(CSORoamingProviderLogger1 > CSORoamingProviderLogger2);

//        #endregion

//        #region Operator >  (CSORoamingProviderLogger1, CSORoamingProviderLogger2)

//        /// <summary>
//        /// Compares two instances of this object.
//        /// </summary>
//        /// <param name="CSORoamingProviderLogger1">A CSORoamingProviderLogger.</param>
//        /// <param name="CSORoamingProviderLogger2">Another CSORoamingProviderLogger.</param>
//        /// <returns>true|false</returns>
//        public static Boolean operator > (CSORoamingProviderLogger CSORoamingProviderLogger1,
//                                          CSORoamingProviderLogger CSORoamingProviderLogger2)
//        {

//            if ((Object) CSORoamingProviderLogger1 == null)
//                throw new ArgumentNullException(nameof(CSORoamingProviderLogger1),  "The given CSORoamingProviderLogger must not be null!");

//            return CSORoamingProviderLogger1.CompareTo(CSORoamingProviderLogger2) > 0;

//        }

//        #endregion

//        #region Operator >= (CSORoamingProviderLogger1, CSORoamingProviderLogger2)

//        /// <summary>
//        /// Compares two instances of this object.
//        /// </summary>
//        /// <param name="CSORoamingProviderLogger1">A CSORoamingProviderLogger.</param>
//        /// <param name="CSORoamingProviderLogger2">Another CSORoamingProviderLogger.</param>
//        /// <returns>true|false</returns>
//        public static Boolean operator >= (CSORoamingProviderLogger CSORoamingProviderLogger1,
//                                           CSORoamingProviderLogger CSORoamingProviderLogger2)

//            => !(CSORoamingProviderLogger1 < CSORoamingProviderLogger2);

//        #endregion

//        #endregion

//        #region IComparable<CSORoamingProviderLogger> Members

//        #region CompareTo(Object)

//        /// <summary>
//        /// Compares two instances of this object.
//        /// </summary>
//        /// <param name="Object">An object to compare with.</param>
//        public Int32 CompareTo(Object Object)
//        {

//            if (Object == null)
//                throw new ArgumentNullException(nameof(Object), "The given object must not be null!");

//            var CSORoamingProviderLogger = Object as CSORoamingProviderLogger;
//            if ((Object) CSORoamingProviderLogger == null)
//                throw new ArgumentException("The given object is not an CSORoamingProviderLogger!", nameof(Object));

//            return CompareTo(CSORoamingProviderLogger);

//        }

//        #endregion

//        #region CompareTo(CSORoamingProviderLogger)

//        /// <summary>
//        /// Compares two instances of this object.
//        /// </summary>
//        /// <param name="CSORoamingProviderLogger">An CSORoamingProviderLogger object to compare with.</param>
//        public Int32 CompareTo(CSORoamingProviderLogger CSORoamingProviderLogger)
//        {

//            if ((Object) CSORoamingProviderLogger == null)
//                throw new ArgumentNullException(nameof(CSORoamingProviderLogger), "The given CSORoamingProviderLogger must not be null!");

//            return Id.CompareTo(CSORoamingProviderLogger.Id);

//        }

//        #endregion

//        #endregion

//        #region IEquatable<CSORoamingProviderLogger> Members

//        #region Equals(Object)

//        /// <summary>
//        /// Compares two instances of this object.
//        /// </summary>
//        /// <param name="Object">An object to compare with.</param>
//        /// <returns>true|false</returns>
//        public override Boolean Equals(Object Object)
//        {

//            if (Object == null)
//                return false;

//            var CSORoamingProviderLogger = Object as CSORoamingProviderLogger;
//            if ((Object) CSORoamingProviderLogger == null)
//                return false;

//            return Equals(CSORoamingProviderLogger);

//        }

//        #endregion

//        #region Equals(CSORoamingProviderLogger)

//        /// <summary>
//        /// Compares two CSORoamingProviderLogger for equality.
//        /// </summary>
//        /// <param name="CSORoamingProviderLogger">An CSORoamingProviderLogger to compare with.</param>
//        /// <returns>True if both match; False otherwise.</returns>
//        public Boolean Equals(CSORoamingProviderLogger CSORoamingProviderLogger)
//        {

//            if ((Object) CSORoamingProviderLogger == null)
//                return false;

//            return Id.Equals(CSORoamingProviderLogger.Id);

//        }

//        #endregion

//        #endregion

//        #region GetHashCode()

//        /// <summary>
//        /// Get the hashcode of this object.
//        /// </summary>
//        public override Int32 GetHashCode()

//            => Id.GetHashCode();

//        #endregion

//        #region (override) ToString()

//        /// <summary>
//        /// Return a text representation of this object.
//        /// </summary>
//        public override String ToString()

//            => "CSO Roaming Provider Logger: " + Id;

//        #endregion


//    }

//}
