/*
 * Copyright (c) 2014-2016 GraphDefined GmbH <achim.friedland@graphdefined.com>
 * This file is part of WWCP Core <https://github.com/GraphDefined/WWCP_Core>
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
using System.Threading.Tasks;
using System.Collections.Generic;

#endregion

namespace org.GraphDefined.WWCP
{

    /// <summary>
    /// A delegate called whenever new EVSE data will be send upstream.
    /// </summary>
    public delegate void OnEVSEDataPushDelegate(DateTime                     Timestamp,
                                                Object                       Sender,
                                                String                       SenderId,
                                                RoamingNetwork_Id            RoamingNetworkId,
                                                ActionType                   ActionType,
                                                ILookup<EVSEOperator, EVSE>  EVSEData,
                                                UInt32                       NumberOfEVSEs);


    /// <summary>
    /// A delegate called whenever new EVSE data had been send upstream.
    /// </summary>
    public delegate void OnEVSEDataPushedDelegate(DateTime                     Timestamp,
                                                  Object                       Sender,
                                                  String                       SenderId,
                                                  RoamingNetwork_Id            RoamingNetworkId,
                                                  ActionType                   ActionType,
                                                  ILookup<EVSEOperator, EVSE>  EVSEData,
                                                  UInt32                       NumberOfEVSEs,
                                                  Acknowledgement              Result,
                                                  TimeSpan                     Duration);


    /// <summary>
    /// A delegate called whenever new EVSE status will be send upstream.
    /// </summary>
    public delegate void OnEVSEStatusPushDelegate(DateTime                     Timestamp,
                                                  Object                       Sender,
                                                  String                       SenderId,
                                                  RoamingNetwork_Id            RoamingNetworkId,
                                                  ActionType                   ActionType,
                                                  ILookup<EVSEOperator, EVSE>  EVSEStatus,
                                                  UInt32                       NumberOfEVSEs);


    /// <summary>
    /// A delegate called whenever new EVSE status had been send upstream.
    /// </summary>
    public delegate void OnEVSEStatusPushedDelegate(DateTime                     Timestamp,
                                                    Object                       Sender,
                                                    String                       SenderId,
                                                    RoamingNetwork_Id            RoamingNetworkId,
                                                    ActionType                   ActionType,
                                                    ILookup<EVSEOperator, EVSE>  EVSEStatus,
                                                    UInt32                       NumberOfEVSEs,
                                                    Acknowledgement              Result,
                                                    TimeSpan                     Duration);


    /// <summary>
    /// The EV Roaming Provider provided EVSE Operator services interface.
    /// </summary>
    public interface IPushEVSEStatusServices
    {

        #region Events

        event OnEVSEDataPushDelegate    OnEVSEDataPush;

        event OnEVSEDataPushedDelegate  OnEVSEDataPushed;

        #endregion


        /// <summary>
        /// Upload the EVSE status of the given lookup of EVSE status types grouped by their EVSE operator.
        /// </summary>
        /// <param name="GroupedEVSEs">A lookup of EVSEs grouped by their EVSE operator.</param>
        /// <param name="ActionType">The server-side data management operation.</param>
        /// <param name="OperatorId">An optional unique identification of the EVSE operator.</param>
        /// <param name="OperatorName">The optional name of the EVSE operator.</param>
        /// <param name="QueryTimeout">An optional timeout of the HTTP client [default 60 sec.]</param>
        Task<Acknowledgement> PushEVSEStatus(ILookup<EVSEOperator, EVSE>  GroupedEVSEs,
                                             ActionType                   ActionType    = ActionType.update,
                                             EVSEOperator_Id              OperatorId    = null,
                                             String                       OperatorName  = null,
                                             TimeSpan?                    QueryTimeout  = null);

        /// <summary>
        /// Upload the given EVSE.
        /// </summary>
        /// <param name="EVSE">An EVSE.</param>
        /// <param name="ActionType">The server-side data management operation.</param>
        /// <param name="OperatorId">An optional unique identification of the EVSE operator.</param>
        /// <param name="OperatorName">The optional name of the EVSE operator.</param>
        /// <param name="QueryTimeout">An optional timeout of the HTTP client [default 60 sec.]</param>
        Task<Acknowledgement> PushEVSEStatus(EVSE             EVSE,
                                             ActionType       ActionType    = ActionType.update,
                                             EVSEOperator_Id  OperatorId    = null,
                                             String           OperatorName  = null,
                                             TimeSpan?        QueryTimeout  = null);

        /// <summary>
        /// Upload the status of the given enumeration of EVSEs.
        /// </summary>
        /// <param name="EVSEs">An enumeration of EVSEs.</param>
        /// <param name="ActionType">The server-side data management operation.</param>
        /// <param name="OperatorId">An optional unique identification of the EVSE operator.</param>
        /// <param name="OperatorName">The optional name of the EVSE operator.</param>
        /// <param name="IncludeEVSEs">Only upload the EVSEs returned by the given filter delegate.</param>
        /// <param name="QueryTimeout">An optional timeout of the HTTP client [default 60 sec.]</param>
        Task<Acknowledgement> PushEVSEStatus(IEnumerable<EVSE>    EVSEs,
                                             ActionType           ActionType    = ActionType.update,
                                             EVSEOperator_Id      OperatorId    = null,
                                             String               OperatorName  = null,
                                             Func<EVSE, Boolean>  IncludeEVSEs  = null,
                                             TimeSpan?            QueryTimeout  = null);

        /// <summary>
        /// Upload the EVSE status of the given charging station.
        /// </summary>
        /// <param name="ChargingStation">A charging station.</param>
        /// <param name="ActionType">The server-side data management operation.</param>
        /// <param name="OperatorId">An optional unique identification of the EVSE operator.</param>
        /// <param name="OperatorName">The optional name of the EVSE operator.</param>
        /// <param name="IncludeEVSEs">Only upload the EVSEs returned by the given filter delegate.</param>
        /// <param name="QueryTimeout">An optional timeout of the HTTP client [default 60 sec.]</param>
        Task<Acknowledgement> PushEVSEStatus(ChargingStation      ChargingStation,
                                             ActionType           ActionType    = ActionType.update,
                                             EVSEOperator_Id      OperatorId    = null,
                                             String               OperatorName  = null,
                                             Func<EVSE, Boolean>  IncludeEVSEs  = null,
                                             TimeSpan?            QueryTimeout  = null);

        /// <summary>
        /// Upload the EVSE status of the given charging stations.
        /// </summary>
        /// <param name="ChargingStations">An enumeration of charging stations.</param>
        /// <param name="ActionType">The server-side data management operation.</param>
        /// <param name="OperatorId">An optional unique identification of the EVSE operator.</param>
        /// <param name="OperatorName">The optional name of the EVSE operator.</param>
        /// <param name="IncludeEVSEs">Only upload the EVSEs returned by the given filter delegate.</param>
        /// <param name="QueryTimeout">An optional timeout of the HTTP client [default 60 sec.]</param>
        Task<Acknowledgement> PushEVSEStatus(IEnumerable<ChargingStation>  ChargingStations,
                                             ActionType                    ActionType    = ActionType.update,
                                             EVSEOperator_Id               OperatorId    = null,
                                             String                        OperatorName  = null,
                                             Func<EVSE, Boolean>           IncludeEVSEs  = null,
                                             TimeSpan?                     QueryTimeout  = null);

        /// <summary>
        /// Upload the EVSE status of the given charging pool.
        /// </summary>
        /// <param name="ChargingPool">A charging pool.</param>
        /// <param name="ActionType">The server-side data management operation.</param>
        /// <param name="OperatorId">An optional unique identification of the EVSE operator.</param>
        /// <param name="OperatorName">The optional name of the EVSE operator.</param>
        /// <param name="IncludeEVSEs">Only upload the EVSEs returned by the given filter delegate.</param>
        /// <param name="QueryTimeout">An optional timeout of the HTTP client [default 60 sec.]</param>
        Task<Acknowledgement> PushEVSEStatus(ChargingPool         ChargingPool,
                                             ActionType           ActionType    = ActionType.update,
                                             EVSEOperator_Id      OperatorId    = null,
                                             String               OperatorName  = null,
                                             Func<EVSE, Boolean>  IncludeEVSEs  = null,
                                             TimeSpan?            QueryTimeout  = null);

        /// <summary>
        /// Upload the EVSE status of the given charging pools.
        /// </summary>
        /// <param name="ChargingPools">An enumeration of charging pools.</param>
        /// <param name="ActionType">The server-side data management operation.</param>
        /// <param name="OperatorId">An optional unique identification of the EVSE operator.</param>
        /// <param name="OperatorName">The optional name of the EVSE operator.</param>
        /// <param name="IncludeEVSEs">Only upload the EVSEs returned by the given filter delegate.</param>
        /// <param name="QueryTimeout">An optional timeout of the HTTP client [default 60 sec.]</param>
        Task<Acknowledgement> PushEVSEStatus(IEnumerable<ChargingPool>  ChargingPools,
                                             ActionType                 ActionType    = ActionType.update,
                                             EVSEOperator_Id            OperatorId    = null,
                                             String                     OperatorName  = null,
                                             Func<EVSE, Boolean>        IncludeEVSEs  = null,
                                             TimeSpan?                  QueryTimeout  = null);

        /// <summary>
        /// Upload the EVSE status of the given EVSE operator.
        /// </summary>
        /// <param name="EVSEOperator">An EVSE operator.</param>
        /// <param name="ActionType">The server-side data management operation.</param>
        /// <param name="OperatorId">An optional unique identification of the EVSE operator.</param>
        /// <param name="OperatorName">The optional name of the EVSE operator.</param>
        /// <param name="IncludeEVSEs">Only upload the EVSEs returned by the given filter delegate.</param>
        /// <param name="QueryTimeout">An optional timeout of the HTTP client [default 60 sec.]</param>
        Task<Acknowledgement> PushEVSEStatus(EVSEOperator         EVSEOperator,
                                             ActionType           ActionType    = ActionType.update,
                                             EVSEOperator_Id      OperatorId    = null,
                                             String               OperatorName  = null,
                                             Func<EVSE, Boolean>  IncludeEVSEs  = null,
                                             TimeSpan?            QueryTimeout  = null);

        /// <summary>
        /// Upload the EVSE status of the given EVSE operators.
        /// </summary>
        /// <param name="EVSEOperators">An enumeration of EVSES operators.</param>
        /// <param name="ActionType">The server-side data management operation.</param>
        /// <param name="OperatorId">An optional unique identification of the EVSE operator.</param>
        /// <param name="OperatorName">The optional name of the EVSE operator.</param>
        /// <param name="IncludeEVSEs">Only upload the EVSEs returned by the given filter delegate.</param>
        /// <param name="QueryTimeout">An optional timeout of the HTTP client [default 60 sec.]</param>
        Task<Acknowledgement> PushEVSEStatus(IEnumerable<EVSEOperator>  EVSEOperators,
                                             ActionType                 ActionType    = ActionType.update,
                                             EVSEOperator_Id            OperatorId    = null,
                                             String                     OperatorName  = null,
                                             Func<EVSE, Boolean>        IncludeEVSEs  = null,
                                             TimeSpan?                  QueryTimeout  = null);

        /// <summary>
        /// Upload the EVSE status of the given roaming network.
        /// </summary>
        /// <param name="RoamingNetwork">A roaming network.</param>
        /// <param name="ActionType">The server-side data management operation.</param>
        /// <param name="OperatorId">An optional unique identification of the EVSE operator.</param>
        /// <param name="OperatorName">The optional name of the EVSE operator.</param>
        /// <param name="IncludeEVSEs">Only upload the EVSEs returned by the given filter delegate.</param>
        /// <param name="QueryTimeout">An optional timeout of the HTTP client [default 60 sec.]</param>
        Task<Acknowledgement> PushEVSEStatus(RoamingNetwork       RoamingNetwork,
                                             ActionType           ActionType    = ActionType.update,
                                             EVSEOperator_Id      OperatorId    = null,
                                             String               OperatorName  = null,
                                             Func<EVSE, Boolean>  IncludeEVSEs  = null,
                                             TimeSpan?            QueryTimeout  = null);







        /// <summary>
        /// Send EVSE status updates.
        /// </summary>
        /// <param name="EVSEStatusDiff">An EVSE status diff.</param>
        /// <param name="QueryTimeout">An optional timeout for this query.</param>
        Task PushEVSEStatus(EVSEStatusDiff  EVSEStatusDiff,
                            TimeSpan?       QueryTimeout = null);

    }


    public class AnonymousPushEVSEStatusService : IPushEVSEStatusServices
    {

        public event OnEVSEDataPushDelegate    OnEVSEDataPush;
        public event OnEVSEDataPushedDelegate  OnEVSEDataPushed;

        private readonly Action<EVSEStatusDiff> _EVSEStatusDiffDelegate;

        public AnonymousPushEVSEStatusService(Action<EVSEStatusDiff> EVSEStatusDiffDelegate)
        {

            this._EVSEStatusDiffDelegate = EVSEStatusDiffDelegate;

        }


        public Task PushEVSEStatus(EVSEStatusDiff EVSEStatusDiff, TimeSpan? QueryTimeout = default(TimeSpan?))
        {
            return Task.FromResult(new Acknowledgement(true));
        }

        public Task<Acknowledgement> PushEVSEStatus(EVSE EVSE, ActionType ActionType = ActionType.update, EVSEOperator_Id OperatorId = null, string OperatorName = null, TimeSpan? QueryTimeout = default(TimeSpan?))
        {

            this._EVSEStatusDiffDelegate(new EVSEStatusDiff(DateTime.Now, EVSE.Operator.Id, null, new KeyValuePair<EVSE_Id, EVSEStatusType>[] { new KeyValuePair<EVSE_Id, EVSEStatusType>(EVSE.Id, EVSE.Status.Value) }, null));

            return Task.FromResult(new Acknowledgement(true));

        }

        public Task<Acknowledgement> PushEVSEStatus(ILookup<EVSEOperator, EVSE> GroupedEVSEs, ActionType ActionType = ActionType.update, EVSEOperator_Id OperatorId = null, string OperatorName = null, TimeSpan? QueryTimeout = default(TimeSpan?))
        {
            return Task.FromResult(new Acknowledgement(true));
        }

        public Task<Acknowledgement> PushEVSEStatus(IEnumerable<EVSEOperator> EVSEOperators, ActionType ActionType = ActionType.update, EVSEOperator_Id OperatorId = null, string OperatorName = null, Func<EVSE, bool> IncludeEVSEs = null, TimeSpan? QueryTimeout = default(TimeSpan?))
        {
            return Task.FromResult(new Acknowledgement(true));
        }

        public Task<Acknowledgement> PushEVSEStatus(RoamingNetwork RoamingNetwork, ActionType ActionType = ActionType.update, EVSEOperator_Id OperatorId = null, string OperatorName = null, Func<EVSE, bool> IncludeEVSEs = null, TimeSpan? QueryTimeout = default(TimeSpan?))
        {
            return Task.FromResult(new Acknowledgement(true));
        }

        public Task<Acknowledgement> PushEVSEStatus(EVSEOperator EVSEOperator, ActionType ActionType = ActionType.update, EVSEOperator_Id OperatorId = null, string OperatorName = null, Func<EVSE, bool> IncludeEVSEs = null, TimeSpan? QueryTimeout = default(TimeSpan?))
        {
            return Task.FromResult(new Acknowledgement(true));
        }

        public Task<Acknowledgement> PushEVSEStatus(IEnumerable<ChargingStation> ChargingStations, ActionType ActionType = ActionType.update, EVSEOperator_Id OperatorId = null, string OperatorName = null, Func<EVSE, bool> IncludeEVSEs = null, TimeSpan? QueryTimeout = default(TimeSpan?))
        {
            return Task.FromResult(new Acknowledgement(true));
        }

        public Task<Acknowledgement> PushEVSEStatus(IEnumerable<ChargingPool> ChargingPools, ActionType ActionType = ActionType.update, EVSEOperator_Id OperatorId = null, string OperatorName = null, Func<EVSE, bool> IncludeEVSEs = null, TimeSpan? QueryTimeout = default(TimeSpan?))
        {
            return Task.FromResult(new Acknowledgement(true));
        }

        public Task<Acknowledgement> PushEVSEStatus(ChargingPool ChargingPool, ActionType ActionType = ActionType.update, EVSEOperator_Id OperatorId = null, string OperatorName = null, Func<EVSE, bool> IncludeEVSEs = null, TimeSpan? QueryTimeout = default(TimeSpan?))
        {
            return Task.FromResult(new Acknowledgement(true));
        }

        public Task<Acknowledgement> PushEVSEStatus(ChargingStation ChargingStation, ActionType ActionType = ActionType.update, EVSEOperator_Id OperatorId = null, string OperatorName = null, Func<EVSE, bool> IncludeEVSEs = null, TimeSpan? QueryTimeout = default(TimeSpan?))
        {
            return Task.FromResult(new Acknowledgement(true));
        }

        public Task<Acknowledgement> PushEVSEStatus(IEnumerable<EVSE> EVSEs, ActionType ActionType = ActionType.update, EVSEOperator_Id OperatorId = null, string OperatorName = null, Func<EVSE, bool> IncludeEVSEs = null, TimeSpan? QueryTimeout = default(TimeSpan?))
        {
            return Task.FromResult(new Acknowledgement(true));
        }
    }


}
