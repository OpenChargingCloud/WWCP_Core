/*
 * Copyright (c) 2014-2016 GraphDefined GmbH <achim.friedland@graphdefined.com>
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

#endregion

namespace org.GraphDefined.WWCP
{

    /// <summary>
    /// An abstract EVSE operator roaming provider.
    /// </summary>
    public abstract class AEVSEOperatorRoamingProvider : ARoamingProvider,
                                                         IEVSEOperatorRoamingProvider
    {

        #region Data

        /// <summary>
        /// The default service check intervall.
        /// </summary>
        public  readonly static TimeSpan                    DefaultServiceCheckEvery = TimeSpan.FromSeconds(31);

        /// <summary>
        /// The default status check intervall.
        /// </summary>
        public  readonly static TimeSpan                    DefaultStatusCheckEvery  = TimeSpan.FromSeconds(3);


        private readonly        Object                      ServiceCheckLock;
        private readonly        Timer                       ServiceCheckTimer;
        private readonly        Object                      StatusCheckLock;
        private readonly        Timer                       StatusCheckTimer;

        private readonly        HashSet<EVSE>               EVSEsToAddQueue;
        private readonly        HashSet<EVSE>               EVSEDataUpdatesQueue;
        private readonly        List<EVSEStatusChange>      EVSEStatusChangesFastQueue;
        private readonly        List<EVSEStatusChange>      EVSEStatusChangesDelayedQueue;
        private readonly        HashSet<EVSE>               EVSEsToRemoveQueue;
        private readonly        List<ChargeDetailRecord>    ChargeDetailRecordQueue;

        private                 UInt64                      _ServiceRunId;
        private                 UInt64                      _StatusRunId;
        private                 Func<EVSE, Boolean>         _IncludeEVSEs;

        public readonly static TimeSpan DefaultRequestTimeout = TimeSpan.FromSeconds(30);

        #endregion

        #region Properties

        #region ServiceCheckEvery

        private UInt32 _ServiceCheckEvery;

        /// <summary>
        /// The service check intervall.
        /// </summary>
        public TimeSpan ServiceCheckEvery {

            get
            {
                return TimeSpan.FromSeconds(_ServiceCheckEvery);
            }

            set
            {
                _ServiceCheckEvery = (UInt32) value.TotalSeconds;
            }

        }

        #endregion

        #region StatusCheckEvery

        private UInt32 _StatusCheckEvery;

        /// <summary>
        /// The status check intervall.
        /// </summary>
        public TimeSpan StatusCheckEvery
        {

            get
            {
                return TimeSpan.FromSeconds(_StatusCheckEvery);
            }

            set
            {
                _StatusCheckEvery = (UInt32) value.TotalSeconds;
            }

        }

        #endregion

        #region DisableAutoUploads

        private volatile Boolean _DisableAutoUploads;

        /// <summary>
        /// This service can be disabled, e.g. for debugging reasons.
        /// </summary>
        public Boolean DisableAutoUploads
        {

            get
            {
                return _DisableAutoUploads;
            }

            set
            {
                _DisableAutoUploads = value;
            }

        }

        #endregion

        #endregion

        #region Events

        // Client methods (logging)

        #region OnEVSEDataPush/-Pushed

        /// <summary>
        /// An event fired whenever new EVSE data will be send upstream.
        /// </summary>
        public abstract event OnEVSEDataPushDelegate    OnEVSEDataPush;

        /// <summary>
        /// An event fired whenever new EVSE data had been sent upstream.
        /// </summary>
        public abstract event OnEVSEDataPushedDelegate  OnEVSEDataPushed;

        #endregion

        #region OnEVSEStatusPush/-Pushed

        /// <summary>
        /// An event fired whenever new EVSE status will be send upstream.
        /// </summary>
        public abstract event OnEVSEStatusPushDelegate    OnEVSEStatusPush;

        /// <summary>
        /// An event fired whenever new EVSE status had been sent upstream.
        /// </summary>
        public abstract event OnEVSEStatusPushedDelegate  OnEVSEStatusPushed;

        #endregion

        #region OnAuthorizeStart/-Started

        /// <summary>
        /// An event fired whenever an authentication token will be verified for charging.
        /// </summary>
        public abstract event OnAuthorizeStartDelegate                   OnAuthorizeStart;

        /// <summary>
        /// An event fired whenever an authentication token had been verified for charging.
        /// </summary>
        public abstract event OnAuthorizeStartedDelegate                 OnAuthorizeStarted;

        /// <summary>
        /// An event fired whenever an authentication token will be verified for charging at the given EVSE.
        /// </summary>
        public abstract event OnAuthorizeEVSEStartDelegate               OnAuthorizeEVSEStart;

        /// <summary>
        /// An event fired whenever an authentication token had been verified for charging at the given EVSE.
        /// </summary>
        public abstract event OnAuthorizeEVSEStartedDelegate             OnAuthorizeEVSEStarted;

        /// <summary>
        /// An event fired whenever an authentication token will be verified for charging at the given charging station.
        /// </summary>
        public abstract event OnAuthorizeChargingStationStartDelegate    OnAuthorizeChargingStationStart;

        /// <summary>
        /// An event fired whenever an authentication token had been verified for charging at the given charging station.
        /// </summary>
        public abstract event OnAuthorizeChargingStationStartedDelegate  OnAuthorizeChargingStationStarted;

        #endregion

        #region OnAuthorizeStop/-Stopped

        /// <summary>
        /// An event fired whenever an authentication token will be verified to stop a charging process.
        /// </summary>
        public abstract event OnAuthorizeStopDelegate                    OnAuthorizeStop;

        /// <summary>
        /// An event fired whenever an authentication token had been verified to stop a charging process.
        /// </summary>
        public abstract event OnAuthorizeStoppedDelegate                 OnAuthorizeStopped;

        /// <summary>
        /// An event fired whenever an authentication token will be verified to stop a charging process at the given EVSE.
        /// </summary>
        public abstract event OnAuthorizeEVSEStopDelegate                OnAuthorizeEVSEStop;

        /// <summary>
        /// An event fired whenever an authentication token had been verified to stop a charging process at the given EVSE.
        /// </summary>
        public abstract event OnAuthorizeEVSEStoppedDelegate             OnAuthorizeEVSEStopped;

        /// <summary>
        /// An event fired whenever an authentication token will be verified to stop a charging process at the given charging station.
        /// </summary>
        public abstract event OnAuthorizeChargingStationStopDelegate     OnAuthorizeChargingStationStop;

        /// <summary>
        /// An event fired whenever an authentication token had been verified to stop a charging process at the given charging station.
        /// </summary>
        public abstract event OnAuthorizeChargingStationStoppedDelegate  OnAuthorizeChargingStationStopped;

        #endregion


        #region OnEVSEOperatorRoamingProviderException

        public delegate Task OnEVSEOperatorRoamingProviderExceptionDelegate(DateTime                      Timestamp,
                                                                            AEVSEOperatorRoamingProvider  Sender,
                                                                            Exception                     Exception);

        public event OnEVSEOperatorRoamingProviderExceptionDelegate OnEVSEOperatorRoamingProviderException;

        #endregion

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create an EVSE operator roaming provider.
        /// </summary>
        /// <param name="Id">The unique identification of the roaming provider.</param>
        /// <param name="Name">The offical (multi-language) name of the roaming provider.</param>
        /// <param name="RoamingNetwork">The associated roaming network.</param>
        /// <param name="IncludeEVSEs">Only include the EVSEs matching the given delegate.</param>
        /// <param name="ServiceCheckEvery">The service check intervall.</param>
        /// <param name="StatusCheckEvery">The status check intervall.</param>
        /// <param name="DisableAutoUploads">This service can be disabled, e.g. for debugging reasons.</param>
        public AEVSEOperatorRoamingProvider(RoamingProvider_Id       Id,
                                            I18NString               Name,
                                            RoamingNetwork           RoamingNetwork,

                                            Func<EVSE, Boolean>      IncludeEVSEs        = null,
                                            TimeSpan?                ServiceCheckEvery   = null,
                                            TimeSpan?                StatusCheckEvery    = null,
                                            Boolean                  DisableAutoUploads  = false)

            : base(Id,
                   Name,
                   RoamingNetwork)

        {

            this.EVSEsToAddQueue                 = new HashSet<EVSE>();
            this.EVSEDataUpdatesQueue            = new HashSet<EVSE>();
            this.EVSEStatusChangesFastQueue      = new List<EVSEStatusChange>();
            this.EVSEStatusChangesDelayedQueue   = new List<EVSEStatusChange>();
            this.EVSEsToRemoveQueue              = new HashSet<EVSE>();
            this.ChargeDetailRecordQueue         = new List<ChargeDetailRecord>();

            this._IncludeEVSEs                   = IncludeEVSEs;

            this._ServiceCheckEvery              = (UInt32) (ServiceCheckEvery.HasValue
                                                                ? ServiceCheckEvery.Value. TotalMilliseconds
                                                                : DefaultServiceCheckEvery.TotalMilliseconds);

            this.ServiceCheckLock                = new Object();
            this.ServiceCheckTimer               = new Timer(ServiceCheck, null, 0, _ServiceCheckEvery);

            this._StatusCheckEvery               = (UInt32) (StatusCheckEvery.HasValue
                                                                ? StatusCheckEvery.Value.  TotalMilliseconds
                                                                : DefaultStatusCheckEvery. TotalMilliseconds);

            this.StatusCheckLock                 = new Object();
            this.StatusCheckTimer                = new Timer(StatusCheck, null, 0, _StatusCheckEvery);

            this._DisableAutoUploads             = DisableAutoUploads;

        }

        #endregion


        // Direct upstream methods...

        #region PushEVSEData...

        #region PushEVSEData(GroupedEVSEs,     ActionType = fullLoad, OperatorId = null, OperatorName = null,                      QueryTimeout = null)

        /// <summary>
        /// Upload the EVSE data of the given lookup of EVSEs grouped by their EVSE operator.
        /// </summary>
        /// <param name="GroupedEVSEs">A lookup of EVSEs grouped by their EVSE operator.</param>
        /// <param name="ActionType">The server-side data management operation.</param>
        /// <param name="OperatorId">An optional unique identification of the EVSE operator.</param>
        /// <param name="OperatorName">The optional name of the EVSE operator.</param>
        /// <param name="QueryTimeout">An optional timeout of the HTTP client [default 60 sec.]</param>
        public abstract Task<Acknowledgement>

            PushEVSEData(ILookup<EVSEOperator, EVSE>  GroupedEVSEs,
                         ActionType                   ActionType    = WWCP.ActionType.fullLoad,
                         EVSEOperator_Id              OperatorId    = null,
                         String                       OperatorName  = null,
                         TimeSpan?                    QueryTimeout  = null);

        #endregion


        #region PushEVSEData(EVSE,             ActionType = insert,   OperatorId = null, OperatorName = null,                      QueryTimeout = null)

        /// <summary>
        /// Upload the EVSE data of the given EVSE.
        /// </summary>
        /// <param name="EVSE">An EVSE.</param>
        /// <param name="ActionType">The server-side data management operation.</param>
        /// <param name="OperatorId">An optional unique identification of the EVSE operator.</param>
        /// <param name="OperatorName">The optional name of the EVSE operator.</param>
        /// <param name="QueryTimeout">An optional timeout of the HTTP client [default 60 sec.]</param>
        public abstract Task<Acknowledgement>

            PushEVSEData(EVSE             EVSE,
                         ActionType       ActionType    = WWCP.ActionType.insert,
                         EVSEOperator_Id  OperatorId    = null,
                         String           OperatorName  = null,
                         TimeSpan?        QueryTimeout  = null);

        #endregion

        #region PushEVSEData(EVSEs,            ActionType = fullLoad, OperatorId = null, OperatorName = null, IncludeEVSEs = null, QueryTimeout = null)

        /// <summary>
        /// Upload the EVSE data of the given enumeration of EVSEs.
        /// </summary>
        /// <param name="EVSEs">An enumeration of EVSEs.</param>
        /// <param name="ActionType">The server-side data management operation.</param>
        /// <param name="OperatorId">An optional unique identification of the EVSE operator.</param>
        /// <param name="OperatorName">The optional name of the EVSE operator.</param>
        /// <param name="IncludeEVSEs">Only upload the EVSEs returned by the given filter delegate.</param>
        /// <param name="QueryTimeout">An optional timeout of the HTTP client [default 60 sec.]</param>
        public abstract Task<Acknowledgement>

            PushEVSEData(IEnumerable<EVSE>    EVSEs,
                         ActionType           ActionType    = WWCP.ActionType.fullLoad,
                         EVSEOperator_Id      OperatorId    = null,
                         String               OperatorName  = null,
                         Func<EVSE, Boolean>  IncludeEVSEs  = null,
                         TimeSpan?            QueryTimeout  = null);

        #endregion

        #region PushEVSEData(ChargingStation,  ActionType = fullLoad, OperatorId = null, OperatorName = null, IncludeEVSEs = null, QueryTimeout = null)

        /// <summary>
        /// Upload the EVSE data of the given charging station.
        /// </summary>
        /// <param name="ChargingStation">A charging station.</param>
        /// <param name="ActionType">The server-side data management operation.</param>
        /// <param name="OperatorId">An optional unique identification of the EVSE operator.</param>
        /// <param name="OperatorName">The optional name of the EVSE operator.</param>
        /// <param name="IncludeEVSEs">Only upload the EVSEs returned by the given filter delegate.</param>
        /// <param name="QueryTimeout">An optional timeout of the HTTP client [default 60 sec.]</param>
        public abstract Task<Acknowledgement>

            PushEVSEData(ChargingStation      ChargingStation,
                         ActionType           ActionType    = WWCP.ActionType.fullLoad,
                         EVSEOperator_Id      OperatorId    = null,
                         String               OperatorName  = null,
                         Func<EVSE, Boolean>  IncludeEVSEs  = null,
                         TimeSpan?            QueryTimeout  = null);

        #endregion

        #region PushEVSEData(ChargingStations, ActionType = fullLoad, OperatorId = null, OperatorName = null, IncludeEVSEs = null, QueryTimeout = null)

        /// <summary>
        /// Upload the EVSE data of the given charging stations.
        /// </summary>
        /// <param name="ChargingStations">An enumeration of charging stations.</param>
        /// <param name="ActionType">The server-side data management operation.</param>
        /// <param name="OperatorId">An optional unique identification of the EVSE operator.</param>
        /// <param name="OperatorName">The optional name of the EVSE operator.</param>
        /// <param name="IncludeEVSEs">Only upload the EVSEs returned by the given filter delegate.</param>
        /// <param name="QueryTimeout">An optional timeout of the HTTP client [default 60 sec.]</param>
        public abstract Task<Acknowledgement>

            PushEVSEData(IEnumerable<ChargingStation>  ChargingStations,
                         ActionType                    ActionType    = WWCP.ActionType.fullLoad,
                         EVSEOperator_Id               OperatorId    = null,
                         String                        OperatorName  = null,
                         Func<EVSE, Boolean>           IncludeEVSEs  = null,
                         TimeSpan?                     QueryTimeout  = null);

        #endregion

        #region PushEVSEData(ChargingPool,     ActionType = fullLoad, OperatorId = null, OperatorName = null, IncludeEVSEs = null, QueryTimeout = null)

        /// <summary>
        /// Upload the EVSE data of the given charging pool.
        /// </summary>
        /// <param name="ChargingPool">A charging pool.</param>
        /// <param name="ActionType">The server-side data management operation.</param>
        /// <param name="OperatorId">An optional unique identification of the EVSE operator.</param>
        /// <param name="OperatorName">The optional name of the EVSE operator.</param>
        /// <param name="IncludeEVSEs">Only upload the EVSEs returned by the given filter delegate.</param>
        /// <param name="QueryTimeout">An optional timeout of the HTTP client [default 60 sec.]</param>
        public abstract Task<Acknowledgement>

            PushEVSEData(ChargingPool         ChargingPool,
                         ActionType           ActionType    = WWCP.ActionType.fullLoad,
                         EVSEOperator_Id      OperatorId    = null,
                         String               OperatorName  = null,
                         Func<EVSE, Boolean>  IncludeEVSEs  = null,
                         TimeSpan?            QueryTimeout  = null);

        #endregion

        #region PushEVSEData(ChargingPools,    ActionType = fullLoad, OperatorId = null, OperatorName = null, IncludeEVSEs = null, QueryTimeout = null)

        /// <summary>
        /// Upload the EVSE data of the given charging pools.
        /// </summary>
        /// <param name="ChargingPools">An enumeration of charging pools.</param>
        /// <param name="ActionType">The server-side data management operation.</param>
        /// <param name="OperatorId">An optional unique identification of the EVSE operator.</param>
        /// <param name="OperatorName">The optional name of the EVSE operator.</param>
        /// <param name="IncludeEVSEs">Only upload the EVSEs returned by the given filter delegate.</param>
        /// <param name="QueryTimeout">An optional timeout of the HTTP client [default 60 sec.]</param>
        public abstract Task<Acknowledgement>

            PushEVSEData(IEnumerable<ChargingPool>  ChargingPools,
                         ActionType                 ActionType    = WWCP.ActionType.fullLoad,
                         EVSEOperator_Id            OperatorId    = null,
                         String                     OperatorName  = null,
                         Func<EVSE, Boolean>        IncludeEVSEs  = null,
                         TimeSpan?                  QueryTimeout  = null);

        #endregion

        #region PushEVSEData(EVSEOperator,     ActionType = fullLoad, OperatorId = null, OperatorName = null, IncludeEVSEs = null, QueryTimeout = null)

        /// <summary>
        /// Upload the EVSE data of the given EVSE operator.
        /// </summary>
        /// <param name="EVSEOperator">An EVSE operator.</param>
        /// <param name="ActionType">The server-side data management operation.</param>
        /// <param name="OperatorId">An optional unique identification of the EVSE operator.</param>
        /// <param name="OperatorName">The optional name of the EVSE operator.</param>
        /// <param name="IncludeEVSEs">Only upload the EVSEs returned by the given filter delegate.</param>
        /// <param name="QueryTimeout">An optional timeout of the HTTP client [default 60 sec.]</param>
        public abstract Task<Acknowledgement>

            PushEVSEData(EVSEOperator         EVSEOperator,
                         ActionType           ActionType    = WWCP.ActionType.fullLoad,
                         EVSEOperator_Id      OperatorId    = null,
                         String               OperatorName  = null,
                         Func<EVSE, Boolean>  IncludeEVSEs  = null,
                         TimeSpan?            QueryTimeout  = null);

        #endregion

        #region PushEVSEData(EVSEOperators,    ActionType = fullLoad, OperatorId = null, OperatorName = null, IncludeEVSEs = null, QueryTimeout = null)

        /// <summary>
        /// Upload the EVSE data of the given EVSE operators.
        /// </summary>
        /// <param name="EVSEOperators">An enumeration of EVSE operators.</param>
        /// <param name="ActionType">The server-side data management operation.</param>
        /// <param name="OperatorId"></param>
        /// <param name="OperatorName">An optional alternative EVSE operator name used for uploading all EVSEs.</param>
        /// <param name="IncludeEVSEs">Only upload the EVSEs returned by the given filter delegate.</param>
        /// <param name="QueryTimeout">An optional timeout for this query.</param>
        public abstract Task<Acknowledgement>

            PushEVSEData(IEnumerable<EVSEOperator>  EVSEOperators,
                         ActionType                 ActionType    = WWCP.ActionType.fullLoad,
                         EVSEOperator_Id            OperatorId    = null,
                         String                     OperatorName  = null,
                         Func<EVSE, Boolean>        IncludeEVSEs  = null,
                         TimeSpan?                  QueryTimeout  = null);

        #endregion

        #region PushEVSEData(RoamingNetwork,   ActionType = fullLoad, OperatorId = null, OperatorName = null, IncludeEVSEs = null, QueryTimeout = null)

        /// <summary>
        /// Upload the EVSE data of the given roaming network.
        /// </summary>
        /// <param name="RoamingNetwork">A roaming network.</param>
        /// <param name="ActionType">The server-side data management operation.</param>
        /// <param name="OperatorId">An optional unique identification of the EVSE operator.</param>
        /// <param name="OperatorName">The optional name of the EVSE operator.</param>
        /// <param name="IncludeEVSEs">Only upload the EVSEs returned by the given filter delegate.</param>
        /// <param name="QueryTimeout">An optional timeout of the HTTP client [default 60 sec.]</param>
        public abstract Task<Acknowledgement>

            PushEVSEData(RoamingNetwork       RoamingNetwork,
                         ActionType           ActionType    = WWCP.ActionType.fullLoad,
                         EVSEOperator_Id      OperatorId    = null,
                         String               OperatorName  = null,
                         Func<EVSE, Boolean>  IncludeEVSEs  = null,
                         TimeSpan?            QueryTimeout  = null);

        #endregion

        #endregion

        #region PushEVSEStatus...

        #region PushEVSEStatus(GroupedEVSEStatus, ActionType = update, OperatorId = null, OperatorName = null,                      QueryTimeout = null)

        /// <summary>
        /// Upload the given lookup of EVSE status grouped by their EVSE operator.
        /// </summary>
        /// <param name="GroupedEVSEStatus">A lookup of EVSE status grouped by their EVSE operator.</param>
        /// <param name="ActionType">The server-side data management operation.</param>
        /// <param name="OperatorId">An optional unique identification of the EVSE operator.</param>
        /// <param name="OperatorName">The optional name of the EVSE operator.</param>
        /// <param name="QueryTimeout">An optional timeout of the HTTP client [default 60 sec.]</param>
        public abstract Task<Acknowledgement>

            PushEVSEStatus(ILookup<EVSEOperator_Id, EVSEStatus>  GroupedEVSEStatus,
                           ActionType                            ActionType    = WWCP.ActionType.update,
                           EVSEOperator_Id                       OperatorId    = null,
                           String                                OperatorName  = null,
                           TimeSpan?                             QueryTimeout  = null);

        #endregion


        #region PushEVSEStatus(EVSEStatus,        ActionType = update, OperatorId = null, OperatorName = null,                      QueryTimeout = null)

        /// <summary>
        /// Upload the given EVSE status.
        /// </summary>
        /// <param name="EVSEStatus">An EVSE status.</param>
        /// <param name="ActionType">The server-side data management operation.</param>
        /// <param name="OperatorId">An optional unique identification of the EVSE operator.</param>
        /// <param name="OperatorName">The optional name of the EVSE operator.</param>
        /// <param name="QueryTimeout">An optional timeout of the HTTP client [default 60 sec.]</param>
        public abstract Task<Acknowledgement>

            PushEVSEStatus(EVSEStatus       EVSEStatus,
                           ActionType       ActionType    = WWCP.ActionType.update,
                           EVSEOperator_Id  OperatorId    = null,
                           String           OperatorName  = null,
                           TimeSpan?        QueryTimeout  = null);

        #endregion

        #region PushEVSEStatus(EVSEStatus,        ActionType = update, OperatorId = null, OperatorName = null,                      QueryTimeout = null)

        /// <summary>
        /// Upload the given enumeration of EVSE status.
        /// </summary>
        /// <param name="EVSEStatus">An enumeration of EVSE status.</param>
        /// <param name="ActionType">The server-side data management operation.</param>
        /// <param name="OperatorId">An optional unique identification of the EVSE operator.</param>
        /// <param name="OperatorName">The optional name of the EVSE operator.</param>
        /// <param name="QueryTimeout">An optional timeout of the HTTP client [default 60 sec.]</param>
        public abstract Task<Acknowledgement>

            PushEVSEStatus(IEnumerable<EVSEStatus>  EVSEStatus,
                           ActionType               ActionType    = WWCP.ActionType.update,
                           EVSEOperator_Id          OperatorId    = null,
                           String                   OperatorName  = null,
                           TimeSpan?                QueryTimeout  = null);

        #endregion

        #region PushEVSEStatus(EVSE,              ActionType = update, OperatorId = null, OperatorName = null, IncludeEVSEs = null, QueryTimeout = null)

        /// <summary>
        /// Upload the EVSE status of the given EVSE.
        /// </summary>
        /// <param name="EVSE">An EVSE.</param>
        /// <param name="ActionType">The server-side data management operation.</param>
        /// <param name="OperatorId">An optional unique identification of the EVSE operator.</param>
        /// <param name="OperatorName">The optional name of the EVSE operator.</param>
        /// <param name="IncludeEVSEs">Only upload the EVSEs returned by the given filter delegate.</param>
        /// <param name="QueryTimeout">An optional timeout of the HTTP client [default 60 sec.]</param>
        public abstract Task<Acknowledgement>

            PushEVSEStatus(EVSE      EVSE,
                           WWCP.ActionType      ActionType    = WWCP.ActionType.update,
                           EVSEOperator_Id      OperatorId    = null,
                           String               OperatorName  = null,
                           Func<EVSE, Boolean>  IncludeEVSEs  = null,
                           TimeSpan?            QueryTimeout  = null);

        #endregion

        #region PushEVSEStatus(EVSEs,             ActionType = update, OperatorId = null, OperatorName = null, IncludeEVSEs = null, QueryTimeout = null)

        /// <summary>
        /// Upload all EVSE status of the given enumeration of EVSEs.
        /// </summary>
        /// <param name="EVSEs">An enumeration of EVSEs.</param>
        /// <param name="ActionType">The server-side data management operation.</param>
        /// <param name="OperatorId">An optional unique identification of the EVSE operator.</param>
        /// <param name="OperatorName">The optional name of the EVSE operator.</param>
        /// <param name="IncludeEVSEs">Only upload the EVSEs returned by the given filter delegate.</param>
        /// <param name="QueryTimeout">An optional timeout of the HTTP client [default 60 sec.]</param>
        public abstract Task<Acknowledgement>

            PushEVSEStatus(IEnumerable<EVSE>    EVSEs,
                           ActionType           ActionType    = WWCP.ActionType.update,
                           EVSEOperator_Id      OperatorId    = null,
                           String               OperatorName  = null,
                           Func<EVSE, Boolean>  IncludeEVSEs  = null,
                           TimeSpan?            QueryTimeout  = null);

        #endregion

        #region PushEVSEStatus(ChargingStation,   ActionType = update, OperatorId = null, OperatorName = null, IncludeEVSEs = null, QueryTimeout = null)

        /// <summary>
        /// Upload all EVSE status of the given charging station.
        /// </summary>
        /// <param name="ChargingStation">A charging station.</param>
        /// <param name="ActionType">The server-side data management operation.</param>
        /// <param name="OperatorId">An optional unique identification of the EVSE operator.</param>
        /// <param name="OperatorName">The optional name of the EVSE operator.</param>
        /// <param name="IncludeEVSEs">Only upload the EVSEs returned by the given filter delegate.</param>
        /// <param name="QueryTimeout">An optional timeout of the HTTP client [default 60 sec.]</param>
        public abstract Task<Acknowledgement>

            PushEVSEStatus(ChargingStation      ChargingStation,
                           WWCP.ActionType      ActionType    = WWCP.ActionType.update,
                           EVSEOperator_Id      OperatorId    = null,
                           String               OperatorName  = null,
                           Func<EVSE, Boolean>  IncludeEVSEs  = null,
                           TimeSpan?            QueryTimeout  = null);

        #endregion

        #region PushEVSEStatus(ChargingStations,  ActionType = update, OperatorId = null, OperatorName = null, IncludeEVSEs = null, QueryTimeout = null)

        /// <summary>
        /// Upload all EVSE status of the given enumeration of charging stations.
        /// </summary>
        /// <param name="ChargingStations">An enumeration of charging stations.</param>
        /// <param name="ActionType">The server-side data management operation.</param>
        /// <param name="OperatorId">An optional unique identification of the EVSE operator.</param>
        /// <param name="OperatorName">The optional name of the EVSE operator.</param>
        /// <param name="IncludeEVSEs">Only upload the EVSEs returned by the given filter delegate.</param>
        /// <param name="QueryTimeout">An optional timeout of the HTTP client [default 60 sec.]</param>
        public abstract Task<Acknowledgement>

            PushEVSEStatus(IEnumerable<ChargingStation>  ChargingStations,
                           ActionType                    ActionType    = WWCP.ActionType.update,
                           EVSEOperator_Id               OperatorId    = null,
                           String                        OperatorName  = null,
                           Func<EVSE, Boolean>           IncludeEVSEs  = null,
                           TimeSpan?                     QueryTimeout  = null);

        #endregion

        #region PushEVSEStatus(ChargingPool,      ActionType = update, OperatorId = null, OperatorName = null, IncludeEVSEs = null, QueryTimeout = null)

        /// <summary>
        /// Upload all EVSE status of the given charging pool.
        /// </summary>
        /// <param name="ChargingPool">A charging pool.</param>
        /// <param name="ActionType">The server-side data management operation.</param>
        /// <param name="OperatorId">An optional unique identification of the EVSE operator.</param>
        /// <param name="OperatorName">The optional name of the EVSE operator.</param>
        /// <param name="IncludeEVSEs">Only upload the EVSEs returned by the given filter delegate.</param>
        /// <param name="QueryTimeout">An optional timeout of the HTTP client [default 60 sec.]</param>
        public abstract Task<Acknowledgement>

            PushEVSEStatus(ChargingPool         ChargingPool,
                           ActionType           ActionType    = WWCP.ActionType.update,
                           EVSEOperator_Id      OperatorId    = null,
                           String               OperatorName  = null,
                           Func<EVSE, Boolean>  IncludeEVSEs  = null,
                           TimeSpan?            QueryTimeout  = null);

        #endregion

        #region PushEVSEStatus(ChargingPools,     ActionType = update, OperatorId = null, OperatorName = null, IncludeEVSEs = null, QueryTimeout = null)

        /// <summary>
        /// Upload all EVSE status of the given enumeration of charging pools.
        /// </summary>
        /// <param name="ChargingPools">An enumeration of charging pools.</param>
        /// <param name="ActionType">The server-side data management operation.</param>
        /// <param name="OperatorId">An optional unique identification of the EVSE operator.</param>
        /// <param name="OperatorName">The optional name of the EVSE operator.</param>
        /// <param name="IncludeEVSEs">Only upload the EVSEs returned by the given filter delegate.</param>
        /// <param name="QueryTimeout">An optional timeout of the HTTP client [default 60 sec.]</param>
        public abstract Task<Acknowledgement>

            PushEVSEStatus(IEnumerable<ChargingPool>  ChargingPools,
                           ActionType                 ActionType    = WWCP.ActionType.update,
                           EVSEOperator_Id            OperatorId    = null,
                           String                     OperatorName  = null,
                           Func<EVSE, Boolean>        IncludeEVSEs  = null,
                           TimeSpan?                  QueryTimeout  = null);

        #endregion

        #region PushEVSEStatus(EVSEOperator,      ActionType = update, OperatorId = null, OperatorName = null, IncludeEVSEs = null, QueryTimeout = null)

        /// <summary>
        /// Upload all EVSE status of the given EVSE operator.
        /// </summary>
        /// <param name="EVSEOperator">An EVSE operator.</param>
        /// <param name="ActionType">The server-side data management operation.</param>
        /// <param name="OperatorId">An optional unique identification of the EVSE operator.</param>
        /// <param name="OperatorName">The optional name of the EVSE operator.</param>
        /// <param name="IncludeEVSEs">Only upload the EVSEs returned by the given filter delegate.</param>
        /// <param name="QueryTimeout">An optional timeout of the HTTP client [default 60 sec.]</param>
        public abstract Task<Acknowledgement>

            PushEVSEStatus(EVSEOperator         EVSEOperator,
                           ActionType           ActionType    = WWCP.ActionType.update,
                           EVSEOperator_Id      OperatorId    = null,
                           String               OperatorName  = null,
                           Func<EVSE, Boolean>  IncludeEVSEs  = null,
                           TimeSpan?            QueryTimeout  = null);

        #endregion

        #region PushEVSEStatus(EVSEOperators,     ActionType = update, OperatorId = null, OperatorName = null, IncludeEVSEs = null, QueryTimeout = null)

        /// <summary>
        /// Upload all EVSE status of the given enumeration of EVSE operators.
        /// </summary>
        /// <param name="EVSEOperators">An enumeration of EVSES operators.</param>
        /// <param name="ActionType">The server-side data management operation.</param>
        /// <param name="OperatorId">An optional unique identification of the EVSE operator.</param>
        /// <param name="OperatorName">The optional name of the EVSE operator.</param>
        /// <param name="IncludeEVSEs">Only upload the EVSEs returned by the given filter delegate.</param>
        /// <param name="QueryTimeout">An optional timeout of the HTTP client [default 60 sec.]</param>
        public abstract Task<Acknowledgement>

            PushEVSEStatus(IEnumerable<EVSEOperator>  EVSEOperators,
                           ActionType                 ActionType    = WWCP.ActionType.update,
                           EVSEOperator_Id            OperatorId    = null,
                           String                     OperatorName  = null,
                           Func<EVSE, Boolean>        IncludeEVSEs  = null,
                           TimeSpan?                  QueryTimeout  = null);

        #endregion

        #region PushEVSEStatus(RoamingNetwork,    ActionType = update, OperatorId = null, OperatorName = null, IncludeEVSEs = null, QueryTimeout = null)

        /// <summary>
        /// Upload all EVSE status of the given roaming network.
        /// </summary>
        /// <param name="RoamingNetwork">A roaming network.</param>
        /// <param name="ActionType">The server-side data management operation.</param>
        /// <param name="OperatorId">An optional unique identification of the EVSE operator.</param>
        /// <param name="OperatorName">The optional name of the EVSE operator.</param>
        /// <param name="IncludeEVSEs">Only upload the EVSEs returned by the given filter delegate.</param>
        /// <param name="QueryTimeout">An optional timeout of the HTTP client [default 60 sec.]</param>
        public abstract Task<Acknowledgement>

            PushEVSEStatus(RoamingNetwork       RoamingNetwork,
                           ActionType           ActionType    = WWCP.ActionType.update,
                           EVSEOperator_Id      OperatorId    = null,
                           String               OperatorName  = null,
                           Func<EVSE, Boolean>  IncludeEVSEs  = null,
                           TimeSpan?            QueryTimeout  = null);

        #endregion


        #region PushEVSEStatus(EVSEStatusDiff, QueryTimeout = null)

        /// <summary>
        /// Send EVSE status updates.
        /// </summary>
        /// <param name="EVSEStatusDiff">An EVSE status diff.</param>
        /// <param name="QueryTimeout">An optional timeout for this query.</param>
        public abstract Task

            PushEVSEStatus(EVSEStatusDiff  EVSEStatusDiff,
                           TimeSpan?       QueryTimeout  = null);

        #endregion

        #endregion

        #region AuthorizeStart/-Stop...

        #region AuthorizeStart(...OperatorId, AuthToken, ChargingProductId = null, SessionId = null, ...)

        /// <summary>
        /// Create an authorize start request.
        /// </summary>
        /// <param name="Timestamp">The timestamp of the request.</param>
        /// <param name="CancellationToken">A token to cancel this request.</param>
        /// <param name="EventTrackingId">An unique event tracking identification for correlating this request with other events.</param>
        /// <param name="OperatorId">An EVSE operator identification.</param>
        /// <param name="AuthToken">A (RFID) user identification.</param>
        /// <param name="ChargingProductId">An optional charging product identification.</param>
        /// <param name="SessionId">An optional session identification.</param>
        /// <param name="QueryTimeout">An optional timeout for this request.</param>
        public abstract Task<AuthStartResult>

            AuthorizeStart(EVSEOperator_Id     OperatorId,
                           Auth_Token          AuthToken,
                           ChargingProduct_Id  ChargingProductId  = null,
                           ChargingSession_Id  SessionId          = null,

                           DateTime?           Timestamp          = null,
                           CancellationToken?  CancellationToken  = null,
                           EventTracking_Id    EventTrackingId    = null,
                           TimeSpan?           RequestTimeout     = null);

        #endregion

        #region AuthorizeStart(...OperatorId, AuthToken, EVSEId, ChargingProductId = null, SessionId = null, ...)

        /// <summary>
        /// Create an authorize start request.
        /// </summary>
        /// <param name="Timestamp">The timestamp of the request.</param>
        /// <param name="CancellationToken">A token to cancel this request.</param>
        /// <param name="EventTrackingId">An unique event tracking identification for correlating this request with other events.</param>
        /// <param name="OperatorId">An EVSE operator identification.</param>
        /// <param name="AuthToken">A (RFID) user identification.</param>
        /// <param name="EVSEId">The unique identification of an EVSE.</param>
        /// <param name="ChargingProductId">An optional charging product identification.</param>
        /// <param name="SessionId">An optional session identification.</param>
        /// <param name="QueryTimeout">An optional timeout for this request.</param>
        public abstract Task<AuthStartEVSEResult>

            AuthorizeStart(EVSEOperator_Id     OperatorId,
                           Auth_Token          AuthToken,
                           EVSE_Id             EVSEId,
                           ChargingProduct_Id  ChargingProductId  = null,
                           ChargingSession_Id  SessionId          = null,

                           DateTime?           Timestamp          = null,
                           CancellationToken?  CancellationToken  = null,
                           EventTracking_Id    EventTrackingId    = null,
                           TimeSpan?           RequestTimeout     = null);

        #endregion

        #region AuthorizeStart(...OperatorId, AuthToken, ChargingStationId, ChargingProductId = null, SessionId = null, ...)

        /// <summary>
        /// Create an authorize start request.
        /// </summary>
        /// <param name="Timestamp">The timestamp of the request.</param>
        /// <param name="CancellationToken">A token to cancel this request.</param>
        /// <param name="EventTrackingId">An unique event tracking identification for correlating this request with other events.</param>
        /// <param name="OperatorId">An EVSE operator identification.</param>
        /// <param name="AuthToken">A (RFID) user identification.</param>
        /// <param name="ChargingStationId">The unique identification of a charging station.</param>
        /// <param name="ChargingProductId">An optional charging product identification.</param>
        /// <param name="SessionId">An optional session identification.</param>
        /// <param name="QueryTimeout">An optional timeout for this request.</param>
        public abstract Task<AuthStartChargingStationResult>

            AuthorizeStart(EVSEOperator_Id     OperatorId,
                           Auth_Token          AuthToken,
                           ChargingStation_Id  ChargingStationId,
                           ChargingProduct_Id  ChargingProductId  = null,
                           ChargingSession_Id  SessionId          = null,

                           DateTime?           Timestamp          = null,
                           CancellationToken?  CancellationToken  = null,
                           EventTracking_Id    EventTrackingId    = null,
                           TimeSpan?           RequestTimeout     = null);

        #endregion


        #region AuthorizeStop(...OperatorId, SessionId, AuthToken, ...)

        // UID => Not everybody can stop any session, but maybe another
        //        UID than the UID which started the session!
        //        (e.g. car sharing)

        /// <summary>
        /// Create an authorize stop request.
        /// </summary>
        /// <param name="Timestamp">The timestamp of the request.</param>
        /// <param name="CancellationToken">A token to cancel this request.</param>
        /// <param name="EventTrackingId">An unique event tracking identification for correlating this request with other events.</param>
        /// <param name="OperatorId">An EVSE Operator identification.</param>
        /// <param name="SessionId">The OICP session identification from the AuthorizeStart request.</param>
        /// <param name="AuthToken">A (RFID) user identification.</param>
        /// <param name="QueryTimeout">An optional timeout for this request.</param>
        public abstract Task<AuthStopResult>

            AuthorizeStop(EVSEOperator_Id     OperatorId,
                          ChargingSession_Id  SessionId,
                          Auth_Token          AuthToken,

                          DateTime?           Timestamp          = null,
                          CancellationToken?  CancellationToken  = null,
                          EventTracking_Id    EventTrackingId    = null,
                          TimeSpan?           RequestTimeout     = null);

        #endregion

        #region AuthorizeStop(...OperatorId, EVSEId, SessionId, AuthToken, ...)

        // UID => Not everybody can stop any session, but maybe another
        //        UID than the UID which started the session!
        //        (e.g. car sharing)

        /// <summary>
        /// Create an authorize stop request.
        /// </summary>
        /// <param name="Timestamp">The timestamp of the request.</param>
        /// <param name="CancellationToken">A token to cancel this request.</param>
        /// <param name="EventTrackingId">An unique event tracking identification for correlating this request with other events.</param>
        /// <param name="OperatorId">An EVSE Operator identification.</param>
        /// <param name="EVSEId">The unique identification of an EVSE.</param>
        /// <param name="SessionId">The OICP session identification from the AuthorizeStart request.</param>
        /// <param name="AuthToken">A (RFID) user identification.</param>
        /// <param name="QueryTimeout">An optional timeout for this request.</param>
        public abstract Task<AuthStopEVSEResult>

            AuthorizeStop(EVSEOperator_Id     OperatorId,
                          EVSE_Id             EVSEId,
                          ChargingSession_Id  SessionId,
                          Auth_Token          AuthToken,

                          DateTime?           Timestamp          = null,
                          CancellationToken?  CancellationToken  = null,
                          EventTracking_Id    EventTrackingId    = null,
                          TimeSpan?           RequestTimeout     = null);

        #endregion

        #region AuthorizeStop(...OperatorId, ChargingStationId, SessionId, AuthToken, ...)

        // UID => Not everybody can stop any session, but maybe another
        //        UID than the UID which started the session!
        //        (e.g. car sharing)

        /// <summary>
        /// Create an authorize stop request.
        /// </summary>
        /// <param name="Timestamp">The timestamp of the request.</param>
        /// <param name="CancellationToken">A token to cancel this request.</param>
        /// <param name="EventTrackingId">An unique event tracking identification for correlating this request with other events.</param>
        /// <param name="OperatorId">An EVSE Operator identification.</param>
        /// <param name="ChargingStationId">The unique identification of a charging station.</param>
        /// <param name="SessionId">The OICP session identification from the AuthorizeStart request.</param>
        /// <param name="AuthToken">A (RFID) user identification.</param>
        /// <param name="QueryTimeout">An optional timeout for this request.</param>
        public abstract Task<AuthStopChargingStationResult>

            AuthorizeStop(EVSEOperator_Id     OperatorId,
                          ChargingStation_Id  ChargingStationId,
                          ChargingSession_Id  SessionId,
                          Auth_Token          AuthToken,

                          DateTime?           Timestamp          = null,
                          CancellationToken?  CancellationToken  = null,
                          EventTracking_Id    EventTrackingId    = null,
                          TimeSpan?           RequestTimeout     = null);

        #endregion

        #endregion

        #region SendChargeDetailRecord(ChargeDetailRecord, ...)

        /// <summary>
        /// Send a charge detail record to an OICP server.
        /// </summary>
        /// <param name="ChargeDetailRecord">A charge detail record.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public abstract Task<SendCDRResult>

            SendChargeDetailRecord(ChargeDetailRecord  ChargeDetailRecord,

                                   DateTime?           Timestamp          = null,
                                   CancellationToken?  CancellationToken  = null,
                                   EventTracking_Id    EventTrackingId    = null,
                                   TimeSpan?           RequestTimeout     = null);

        #endregion

        //ToDo: Fix me!
        public abstract void

            RemoveChargingStations(DateTime Timestamp,
                                   IEnumerable<ChargingStation> ChargingStations);


        // Delayed upstream methods...

        #region EnqueueChargingPoolDataUpdate(ChargingPool, PropertyName, OldValue, NewValue)

        /// <summary>
        /// Enqueue the given EVSE data for a delayed upload.
        /// </summary>
        /// <param name="ChargingPool">A charging station.</param>
        public Task<Acknowledgement>

            EnqueueChargingPoolDataUpdate(ChargingPool  ChargingPool,
                                          String        PropertyName,
                                          Object        OldValue,
                                          Object        NewValue)

        {

            #region Initial checks

            if (ChargingPool == null)
                throw new ArgumentNullException(nameof(ChargingPool), "The given charging station must not be null!");

            #endregion

            lock (ServiceCheckLock)
            {

                foreach (var EVSE in ChargingPool.SelectMany(station => station))
                {

                    if (_IncludeEVSEs == null ||
                       (_IncludeEVSEs != null && _IncludeEVSEs(EVSE)))
                    {

                        EVSEDataUpdatesQueue.Add(EVSE);

                        ServiceCheckTimer.Change(_ServiceCheckEvery, Timeout.Infinite);

                    }

                }

            }

            return Task.FromResult(new Acknowledgement(true));

        }

        #endregion

        #region EnqueueChargingStationDataUpdate(ChargingStation, PropertyName, OldValue, NewValue)

        /// <summary>
        /// Enqueue the given EVSE data for a delayed upload.
        /// </summary>
        /// <param name="ChargingStation">A charging station.</param>
        public Task<Acknowledgement>

            EnqueueChargingStationDataUpdate(ChargingStation  ChargingStation,
                                             String           PropertyName,
                                             Object           OldValue,
                                             Object           NewValue)

        {

            #region Initial checks

            if (ChargingStation == null)
                throw new ArgumentNullException(nameof(ChargingStation), "The given charging station must not be null!");

            #endregion

            lock (ServiceCheckLock)
            {

                foreach (var EVSE in ChargingStation)
                {

                    if (_IncludeEVSEs == null ||
                       (_IncludeEVSEs != null && _IncludeEVSEs(EVSE)))
                    {

                        EVSEDataUpdatesQueue.Add(EVSE);

                        ServiceCheckTimer.Change(_ServiceCheckEvery, Timeout.Infinite);

                    }

                }

            }

            return Task.FromResult(new Acknowledgement(true));

        }

        #endregion

        #region EnqueueEVSEAdddition(Timestamp, ChargingStation, EVSE)

        public Task EnqueueEVSEAdddition(DateTime         Timestamp,
                                         ChargingStation  ChargingStation,
                                         EVSE             EVSE)

        {

            #region Initial checks

            if (ChargingStation == null)
                throw new ArgumentNullException(nameof(ChargingStation),  "The given charging station must not be null!");

            if (EVSE            == null)
                throw new ArgumentNullException(nameof(EVSE),             "The given EVSE must not be null!");

            #endregion

            lock (ServiceCheckLock)
            {

                if (_IncludeEVSEs == null ||
                   (_IncludeEVSEs != null && _IncludeEVSEs(EVSE)))
                {

                    EVSEsToAddQueue.Add(EVSE);

                    ServiceCheckTimer.Change(_ServiceCheckEvery, Timeout.Infinite);

                }

            }

            return Task.FromResult(new Acknowledgement(true));

        }

        #endregion

        #region EnqueueEVSERemoval(Timestamp, ChargingStation, EVSE)

        public Task EnqueueEVSERemoval(DateTime         Timestamp,
                                       ChargingStation  ChargingStation,
                                       EVSE             EVSE)

        {

            #region Initial checks

            if (ChargingStation == null)
                throw new ArgumentNullException(nameof(ChargingStation),  "The given charging station must not be null!");

            if (EVSE            == null)
                throw new ArgumentNullException(nameof(EVSE),             "The given EVSE must not be null!");

            #endregion

            lock (ServiceCheckLock)
            {

                if (_IncludeEVSEs == null ||
                   (_IncludeEVSEs != null && _IncludeEVSEs(EVSE)))
                {

                    EVSEsToRemoveQueue.Add(EVSE);

                    ServiceCheckTimer.Change(_ServiceCheckEvery, Timeout.Infinite);

                }

            }

            return Task.FromResult(new Acknowledgement(true));

        }

        #endregion

        #region EnqueueEVSEDataUpdate(EVSE, PropertyName, OldValue, NewValue)

        /// <summary>
        /// Enqueue the given EVSE data for a delayed upload.
        /// </summary>
        /// <param name="EVSE">An EVSE.</param>
        public Task<Acknowledgement>

            EnqueueEVSEDataUpdate(EVSE    EVSE,
                                  String  PropertyName,
                                  Object  OldValue,
                                  Object  NewValue)

        {

            #region Initial checks

            if (EVSE == null)
                throw new ArgumentNullException(nameof(EVSE), "The given EVSE must not be null!");

            #endregion

            lock (ServiceCheckLock)
            {

                if (_IncludeEVSEs == null ||
                   (_IncludeEVSEs != null && _IncludeEVSEs(EVSE)))
                {

                    EVSEDataUpdatesQueue.Add(EVSE);

                    ServiceCheckTimer.Change(_ServiceCheckEvery, Timeout.Infinite);

                }

            }

            return Task.FromResult(new Acknowledgement(true));

        }

        #endregion

        #region EnqueueEVSEStatusUpdate(EVSE, OldStatus, NewStatus)

        /// <summary>
        /// Enqueue the given EVSE status for a delayed upload.
        /// </summary>
        /// <param name="EVSE">An EVSE.</param>
        /// <param name="OldStatus">The old status of the EVSE.</param>
        /// <param name="NewStatus">The new status of the EVSE.</param>
        public Task<Acknowledgement>

            EnqueueEVSEStatusUpdate(EVSE                         EVSE,
                                    Timestamped<EVSEStatusType>  OldStatus,
                                    Timestamped<EVSEStatusType>  NewStatus)

        {

            #region Initial checks

            if (EVSE == null)
                throw new ArgumentNullException(nameof(EVSE), "The given EVSE must not be null!");

            #endregion

            lock (ServiceCheckLock)
            {

                if (_IncludeEVSEs == null ||
                   (_IncludeEVSEs != null && _IncludeEVSEs(EVSE)))
                {

                    EVSEStatusChangesFastQueue.Add(new EVSEStatusChange(EVSE.Id, OldStatus, NewStatus));
                    StatusCheckTimer.Change(_StatusCheckEvery, Timeout.Infinite);

                }

            }

            return Task.FromResult(new Acknowledgement(true));

        }

        #endregion

        #region EnqueueChargeDetailRecord(ChargeDetailRecord)

        /// <summary>
        /// Enqueue the given charge detail record for a delayed upload.
        /// </summary>
        public Task<SendCDRResult>

            EnqueueChargeDetailRecord(ChargeDetailRecord ChargeDetailRecord)

        {

            #region Initial checks

            if (ChargeDetailRecord == null)
                throw new ArgumentNullException(nameof(ChargeDetailRecord), "The given charge detail record must not be null!");

            #endregion

            lock (ServiceCheckLock)
            {

                ChargeDetailRecordQueue.Add(ChargeDetailRecord);

                ServiceCheckTimer.Change(_ServiceCheckEvery, Timeout.Infinite);

            }

            return Task.FromResult(SendCDRResult.NotForwared(Authorizator_Id.Parse("EVSE Operator Roaming Provider")));

        }

        #endregion


        #region (timer) ServiceCheck(State)

        private void ServiceCheck(Object State)
        {

            if (!_DisableAutoUploads)
            {

                try
                {

                    FlushServiceQueues().Wait();

                }
                catch (Exception e)
                {

                    while (e.InnerException != null)
                        e = e.InnerException;

                    OnEVSEOperatorRoamingProviderException?.Invoke(DateTime.Now,
                                                                   this,
                                                                   e);

                }

            }

        }

        public async Task FlushServiceQueues()
        {

            DebugX.Log("ServiceCheck, as every " + _ServiceCheckEvery + "ms!");

            #region Make a thread local copy of all data

            //ToDo: AsyncLocal is currently not implemented in Mono!
            //var EVSEDataQueueCopy   = new AsyncLocal<HashSet<EVSE>>();
            //var EVSEStatusQueueCopy = new AsyncLocal<List<EVSEStatusChange>>();

            var EVSEsToAddQueueCopy          = new ThreadLocal<HashSet<EVSE>>();
            var EVSEDataQueueCopy            = new ThreadLocal<HashSet<EVSE>>();
            var EVSEStatusQueueCopy          = new ThreadLocal<List<EVSEStatusChange>>();
            var EVSEsToRemoveQueueCopy       = new ThreadLocal<HashSet<EVSE>>();
            var ChargeDetailRecordQueueCopy  = new ThreadLocal<List<ChargeDetailRecord>>();

            if (Monitor.TryEnter(ServiceCheckLock))
            {

                try
                {

                    if (EVSEsToAddQueue.               Count == 0 &&
                        EVSEDataUpdatesQueue.          Count == 0 &&
                        EVSEStatusChangesDelayedQueue.Count == 0 &&
                        EVSEsToRemoveQueue.            Count == 0 &&
                        ChargeDetailRecordQueue.       Count == 0)
                        return;

                    _ServiceRunId++;

                    // Copy 'EVSEs to add', remove originals...
                    EVSEsToAddQueueCopy.Value          = new HashSet<EVSE>           (EVSEsToAddQueue);
                    EVSEsToAddQueue.Clear();

                    // Copy 'EVSEs to update', remove originals...
                    EVSEDataQueueCopy.Value            = new HashSet<EVSE>           (EVSEDataUpdatesQueue);
                    EVSEDataUpdatesQueue.Clear();

                    // Copy 'EVSE status changes', remove originals...
                    EVSEStatusQueueCopy.Value          = new List<EVSEStatusChange>  (EVSEStatusChangesDelayedQueue);
                    EVSEStatusChangesDelayedQueue.Clear();

                    // Copy 'EVSEs to remove', remove originals...
                    EVSEsToRemoveQueueCopy.Value       = new HashSet<EVSE>           (EVSEsToRemoveQueue);
                    EVSEsToRemoveQueue.Clear();

                    // Copy 'EVSEs to remove', remove originals...
                    ChargeDetailRecordQueueCopy.Value  = new List<ChargeDetailRecord>(ChargeDetailRecordQueue);
                    ChargeDetailRecordQueue.Clear();

                    // Stop the timer. Will be rescheduled by next EVSE data/status change...
                    ServiceCheckTimer.Change(Timeout.Infinite, Timeout.Infinite);

                }
                catch (Exception e)
                {

                    while (e.InnerException != null)
                        e = e.InnerException;

                    DebugX.LogT(nameof(AEVSEOperatorRoamingProvider) + " '" + Id + "' led to an exception: " + e.Message + Environment.NewLine + e.StackTrace);

                }

                finally
                {
                    Monitor.Exit(ServiceCheckLock);
                }

            }

            else
            {

                Console.WriteLine("ServiceCheckLock missed!");
                ServiceCheckTimer.Change(_ServiceCheckEvery, Timeout.Infinite);

            }

            #endregion

            // Upload status changes...
            if (EVSEsToAddQueueCopy.        Value != null ||
                EVSEDataQueueCopy.          Value != null ||
                EVSEStatusQueueCopy.        Value != null ||
                EVSEsToRemoveQueueCopy.     Value != null ||
                ChargeDetailRecordQueueCopy.Value != null)
            {

                // Use the events to evaluate if something went wrong!

                var EventTrackingId = EventTracking_Id.New;

                #region Send new EVSE data

                if (EVSEsToAddQueueCopy.Value.Count > 0)
                {

                    var EVSEsToAddTask = PushEVSEData(EVSEsToAddQueueCopy.Value,
                                                      _ServiceRunId == 1
                                                          ? ActionType.fullLoad
                                                          : ActionType.insert);

                    EVSEsToAddTask.Wait();

                }

                #endregion

                #region Send changed EVSE data

                if (EVSEDataQueueCopy.Value.Count > 0)
                {

                    // Surpress EVSE data updates for all newly added EVSEs
                    var EVSEsWithoutNewEVSEs = EVSEDataQueueCopy.Value.
                                                   Where(evse => !EVSEsToAddQueueCopy.Value.Contains(evse)).
                                                   ToArray();


                    if (EVSEsWithoutNewEVSEs.Length > 0)
                    {

                        var PushEVSEDataTask = PushEVSEData(EVSEsWithoutNewEVSEs, ActionType.update);

                        PushEVSEDataTask.Wait();

                    }

                }

                #endregion

                #region Send changed EVSE status

                if (EVSEStatusQueueCopy.Value.Count > 0)
                {

                    var PushEVSEStatusTask = PushEVSEStatus(EVSEStatusQueueCopy.Value.Select(statuschange => statuschange.CurrentStatus).ToArray(),
                                                            _ServiceRunId == 1
                                                                ? ActionType.fullLoad
                                                                : ActionType.update);

                    PushEVSEStatusTask.Wait();

                }

                #endregion

                #region Send charge detail records

                if (ChargeDetailRecordQueueCopy.Value.Count > 0)
                {

                    var SendCDRResults   = new Dictionary<ChargeDetailRecord, SendCDRResult>();

                    foreach (var _ChargeDetailRecord in ChargeDetailRecordQueueCopy.Value)
                    {

                        SendCDRResults.Add(_ChargeDetailRecord,
                                           await SendChargeDetailRecord(_ChargeDetailRecord,
                                                                        DateTime.Now,
                                                                        new CancellationTokenSource().Token,
                                                                        EventTrackingId,
                                                                        DefaultRequestTimeout));

                    }

                    //ToDo: Send results events...
                    //ToDo: Readd to queue if it could not be sent...

                }

                #endregion

                //ToDo: Send removed EVSE data!

            }

            return;

        }

        #endregion

        #region (timer) StatusCheck(State)

        private void StatusCheck(Object State)
        {

            if (!_DisableAutoUploads)
            {

                FlushStatusQueues().Wait();

                //ToDo: Handle errors!

            }

        }

        public async Task FlushStatusQueues()
        {

            DebugX.Log("StatusCheck, as every " + _StatusCheckEvery + "ms!");

            #region Make a thread local copy of all data

            //ToDo: AsyncLocal is currently not implemented in Mono!
            //var EVSEStatusQueueCopy = new AsyncLocal<List<EVSEStatusChange>>();

            var EVSEStatusFastQueueCopy = new ThreadLocal<List<EVSEStatusChange>>();

            if (Monitor.TryEnter(ServiceCheckLock,
                                 TimeSpan.FromMinutes(5)))
            {

                try
                {

                    if (EVSEStatusChangesFastQueue.Count == 0)
                        return;

                    _StatusRunId++;

                    // Copy 'EVSE status changes', remove originals...
                    EVSEStatusFastQueueCopy.Value = new List<EVSEStatusChange>(EVSEStatusChangesFastQueue.Where(evsestatuschange => !EVSEsToAddQueue.Any(evse => evse.Id == evsestatuschange.Id)));

                    // Add all evse status changes of EVSE *NOT YET UPLOADED* into the delayed queue...
                    EVSEStatusChangesDelayedQueue.AddRange(EVSEStatusChangesFastQueue.Where(evsestatuschange => EVSEsToAddQueue.Any(evse => evse.Id == evsestatuschange.Id)));

                    EVSEStatusChangesFastQueue.Clear();

                    // Stop the timer. Will be rescheduled by next EVSE status change...
                    StatusCheckTimer.Change(Timeout.Infinite, Timeout.Infinite);

                }
                catch (Exception e)
                {

                    while (e.InnerException != null)
                        e = e.InnerException;

                    DebugX.LogT(nameof(AEVSEOperatorRoamingProvider) + " '" + Id + "' led to an exception: " + e.Message + Environment.NewLine + e.StackTrace);

                }

                finally
                {
                    Monitor.Exit(ServiceCheckLock);
                }

            }

            else
            {

                Console.WriteLine("StatusCheckLock missed!");
                StatusCheckTimer.Change(_StatusCheckEvery, Timeout.Infinite);

            }

            #endregion

            // Upload status changes...
            if (EVSEStatusFastQueueCopy.Value != null)
            {

                // Use the events to evaluate if something went wrong!

                #region Send changed EVSE status

                if (EVSEStatusFastQueueCopy.Value.Count > 0)
                {

                    var PushEVSEStatusTask = PushEVSEStatus(EVSEStatusFastQueueCopy.Value.Select(statuschange => statuschange.CurrentStatus).ToArray(),
                                                            _StatusRunId == 1
                                                                ? ActionType.fullLoad
                                                                : ActionType.update);

                    PushEVSEStatusTask.Wait();

                }

                #endregion

            }

            return;

        }

        #endregion


        #region IComparable<RoamingProvider> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        public new Int32 CompareTo(Object Object)
        {

            if (Object == null)
                throw new ArgumentNullException(nameof(Object), "The given object must not be null!");

            // Check if the given object is a roaming provider.
            var RoamingProvider = Object as AEMPRoamingProvider;
            if ((Object) RoamingProvider == null)
                throw new ArgumentException("The given object is not a roaming provider!");

            return CompareTo(RoamingProvider);

        }

        #endregion

        #region CompareTo(RoamingProvider)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="RoamingProvider">A roaming provider object to compare with.</param>
        public new Int32 CompareTo(AEMPRoamingProvider RoamingProvider)
        {

            if ((Object) RoamingProvider == null)
                throw new ArgumentNullException(nameof(RoamingProvider), "The given roaming provider must not be null!");

            return Id.CompareTo(RoamingProvider.Id);

        }

        #endregion

        #endregion

        #region IEquatable<RoamingProvider> Members

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

            // Check if the given object is a roaming provider.
            var RoamingProvider = Object as AEMPRoamingProvider;
            if ((Object) RoamingProvider == null)
                return false;

            return this.Equals(RoamingProvider);

        }

        #endregion

        #region Equals(RoamingProvider)

        /// <summary>
        /// Compares two roaming provider for equality.
        /// </summary>
        /// <param name="RoamingProvider">A roaming provider to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public new Boolean Equals(AEMPRoamingProvider RoamingProvider)
        {

            if ((Object) RoamingProvider == null)
                return false;

            return Id.Equals(RoamingProvider.Id);

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
