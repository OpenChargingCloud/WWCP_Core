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
    /// The EV Roaming Provider provided EVSE Operator services interface.
    /// </summary>
    public interface IPushDataAndStatus
    {

        #region Events

        #region OnEVSEDataPush/-Pushed

        /// <summary>
        /// An event fired whenever new EVSE data will be send upstream.
        /// </summary>
        event OnEVSEDataPushDelegate    OnEVSEDataPush;

        /// <summary>
        /// An event fired whenever new EVSE data had been sent upstream.
        /// </summary>
        event OnEVSEDataPushedDelegate  OnEVSEDataPushed;

        #endregion

        #region OnEVSEStatusPush/-Pushed

        /// <summary>
        /// An event fired whenever new EVSE status will be send upstream.
        /// </summary>
        event OnEVSEStatusPushDelegate    OnEVSEStatusPush;

        /// <summary>
        /// An event fired whenever new EVSE status had been sent upstream.
        /// </summary>
        event OnEVSEStatusPushedDelegate  OnEVSEStatusPushed;

        #endregion

        #endregion


        #region PushEVSEData

        /// <summary>
        /// Upload the EVSE data of the given lookup of EVSEs grouped by their EVSE operator.
        /// </summary>
        /// <param name="GroupedEVSEs">A lookup of EVSEs grouped by their EVSE operator.</param>
        /// <param name="ActionType">The server-side data management operation.</param>
        /// <param name="OperatorId">An optional unique identification of the EVSE operator.</param>
        /// <param name="OperatorName">The optional name of the EVSE operator.</param>
        /// <param name="QueryTimeout">An optional timeout of the HTTP client [default 60 sec.]</param>
        Task<Acknowledgement>

            PushEVSEData(ILookup<EVSEOperator, EVSE>  GroupedEVSEs,
                         ActionType                   ActionType    = WWCP.ActionType.fullLoad,
                         EVSEOperator_Id              OperatorId    = null,
                         String                       OperatorName  = null,
                         TimeSpan?                    QueryTimeout  = null);

        /// <summary>
        /// Upload the EVSE data of the given EVSE.
        /// </summary>
        /// <param name="EVSE">An EVSE.</param>
        /// <param name="ActionType">The server-side data management operation.</param>
        /// <param name="OperatorId">An optional unique identification of the EVSE operator.</param>
        /// <param name="OperatorName">The optional name of the EVSE operator.</param>
        /// <param name="QueryTimeout">An optional timeout of the HTTP client [default 60 sec.]</param>
        Task<Acknowledgement>

            PushEVSEData(EVSE                 EVSE,
                         ActionType           ActionType    = WWCP.ActionType.insert,
                         EVSEOperator_Id      OperatorId    = null,
                         String               OperatorName  = null,
                         TimeSpan?            QueryTimeout  = null);

        /// <summary>
        /// Upload the EVSE data of the given enumeration of EVSEs.
        /// </summary>
        /// <param name="EVSEs">An enumeration of EVSEs.</param>
        /// <param name="ActionType">The server-side data management operation.</param>
        /// <param name="OperatorId">An optional unique identification of the EVSE operator.</param>
        /// <param name="OperatorName">The optional name of the EVSE operator.</param>
        /// <param name="IncludeEVSEs">Only upload the EVSEs returned by the given filter delegate.</param>
        /// <param name="QueryTimeout">An optional timeout of the HTTP client [default 60 sec.]</param>
        Task<Acknowledgement>

            PushEVSEData(IEnumerable<EVSE>    EVSEs,
                         ActionType           ActionType    = WWCP.ActionType.fullLoad,
                         EVSEOperator_Id      OperatorId    = null,
                         String               OperatorName  = null,
                         Func<EVSE, Boolean>  IncludeEVSEs  = null,
                         TimeSpan?            QueryTimeout  = null);

        /// <summary>
        /// Upload the EVSE data of the given charging station.
        /// </summary>
        /// <param name="ChargingStation">A charging station.</param>
        /// <param name="ActionType">The server-side data management operation.</param>
        /// <param name="OperatorId">An optional unique identification of the EVSE operator.</param>
        /// <param name="OperatorName">The optional name of the EVSE operator.</param>
        /// <param name="IncludeEVSEs">Only upload the EVSEs returned by the given filter delegate.</param>
        /// <param name="QueryTimeout">An optional timeout of the HTTP client [default 60 sec.]</param>
        Task<Acknowledgement>

            PushEVSEData(ChargingStation      ChargingStation,
                         ActionType           ActionType    = WWCP.ActionType.fullLoad,
                         EVSEOperator_Id      OperatorId    = null,
                         String               OperatorName  = null,
                         Func<EVSE, Boolean>  IncludeEVSEs  = null,
                         TimeSpan?            QueryTimeout  = null);

        /// <summary>
        /// Upload the EVSE data of the given charging stations.
        /// </summary>
        /// <param name="ChargingStations">An enumeration of charging stations.</param>
        /// <param name="ActionType">The server-side data management operation.</param>
        /// <param name="OperatorId">An optional unique identification of the EVSE operator.</param>
        /// <param name="OperatorName">The optional name of the EVSE operator.</param>
        /// <param name="IncludeEVSEs">Only upload the EVSEs returned by the given filter delegate.</param>
        /// <param name="QueryTimeout">An optional timeout of the HTTP client [default 60 sec.]</param>
        Task<Acknowledgement>

            PushEVSEData(IEnumerable<ChargingStation>  ChargingStations,
                         ActionType                    ActionType    = WWCP.ActionType.fullLoad,
                         EVSEOperator_Id               OperatorId    = null,
                         String                        OperatorName  = null,
                         Func<EVSE, Boolean>           IncludeEVSEs  = null,
                         TimeSpan?                     QueryTimeout  = null);

        /// <summary>
        /// Upload the EVSE data of the given charging pool.
        /// </summary>
        /// <param name="ChargingPool">A charging pool.</param>
        /// <param name="ActionType">The server-side data management operation.</param>
        /// <param name="OperatorId">An optional unique identification of the EVSE operator.</param>
        /// <param name="OperatorName">The optional name of the EVSE operator.</param>
        /// <param name="IncludeEVSEs">Only upload the EVSEs returned by the given filter delegate.</param>
        /// <param name="QueryTimeout">An optional timeout of the HTTP client [default 60 sec.]</param>
        Task<Acknowledgement>

            PushEVSEData(ChargingPool         ChargingPool,
                         ActionType           ActionType    = WWCP.ActionType.fullLoad,
                         EVSEOperator_Id      OperatorId    = null,
                         String               OperatorName  = null,
                         Func<EVSE, Boolean>  IncludeEVSEs  = null,
                         TimeSpan?            QueryTimeout  = null);

        /// <summary>
        /// Upload the EVSE data of the given charging pools.
        /// </summary>
        /// <param name="ChargingPools">An enumeration of charging pools.</param>
        /// <param name="ActionType">The server-side data management operation.</param>
        /// <param name="OperatorId">An optional unique identification of the EVSE operator.</param>
        /// <param name="OperatorName">The optional name of the EVSE operator.</param>
        /// <param name="IncludeEVSEs">Only upload the EVSEs returned by the given filter delegate.</param>
        /// <param name="QueryTimeout">An optional timeout of the HTTP client [default 60 sec.]</param>
        Task<Acknowledgement>

            PushEVSEData(IEnumerable<ChargingPool>  ChargingPools,
                         ActionType                 ActionType    = WWCP.ActionType.fullLoad,
                         EVSEOperator_Id            OperatorId    = null,
                         String                     OperatorName  = null,
                         Func<EVSE, Boolean>        IncludeEVSEs  = null,
                         TimeSpan?                  QueryTimeout  = null);

        /// <summary>
        /// Upload the EVSE data of the given EVSE operator.
        /// </summary>
        /// <param name="EVSEOperator">An EVSE operator.</param>
        /// <param name="ActionType">The server-side data management operation.</param>
        /// <param name="OperatorId">An optional unique identification of the EVSE operator.</param>
        /// <param name="OperatorName">The optional name of the EVSE operator.</param>
        /// <param name="IncludeEVSEs">Only upload the EVSEs returned by the given filter delegate.</param>
        /// <param name="QueryTimeout">An optional timeout of the HTTP client [default 60 sec.]</param>
        Task<Acknowledgement>

            PushEVSEData(EVSEOperator         EVSEOperator,
                         ActionType           ActionType    = WWCP.ActionType.fullLoad,
                         EVSEOperator_Id      OperatorId    = null,
                         String               OperatorName  = null,
                         Func<EVSE, Boolean>  IncludeEVSEs  = null,
                         TimeSpan?            QueryTimeout  = null);


        /// <summary>
        /// Upload the EVSE data of the given EVSE operators.
        /// </summary>
        /// <param name="EVSEOperators">An enumeration of EVSE operators.</param>
        /// <param name="ActionType">The server-side data management operation.</param>
        /// <param name="OperatorId"></param>
        /// <param name="OperatorName">An optional alternative EVSE operator name used for uploading all EVSEs.</param>
        /// <param name="IncludeEVSEs">Only upload the EVSEs returned by the given filter delegate.</param>
        /// <param name="QueryTimeout">An optional timeout for this query.</param>
        Task<Acknowledgement>

            PushEVSEData(IEnumerable<EVSEOperator>  EVSEOperators,
                         ActionType                 ActionType    = WWCP.ActionType.fullLoad,
                         EVSEOperator_Id            OperatorId    = null,
                         String                     OperatorName  = null,
                         Func<EVSE, Boolean>        IncludeEVSEs  = null,
                         TimeSpan?                  QueryTimeout  = null);

        /// <summary>
        /// Upload the EVSE data of the given roaming network.
        /// </summary>
        /// <param name="RoamingNetwork">A roaming network.</param>
        /// <param name="ActionType">The server-side data management operation.</param>
        /// <param name="OperatorId">An optional unique identification of the EVSE operator.</param>
        /// <param name="OperatorName">The optional name of the EVSE operator.</param>
        /// <param name="IncludeEVSEs">Only upload the EVSEs returned by the given filter delegate.</param>
        /// <param name="QueryTimeout">An optional timeout of the HTTP client [default 60 sec.]</param>
        Task<Acknowledgement>

            PushEVSEData(RoamingNetwork       RoamingNetwork,
                         ActionType           ActionType    = WWCP.ActionType.fullLoad,
                         EVSEOperator_Id      OperatorId    = null,
                         String               OperatorName  = null,
                         Func<EVSE, Boolean>  IncludeEVSEs  = null,
                         TimeSpan?            QueryTimeout  = null);

        #endregion

        #region PushEVSEStatus

        /// <summary>
        /// Upload the EVSE status of the given lookup of EVSE status types grouped by their EVSE operator.
        /// </summary>
        /// <param name="GroupedEVSEStatus">A lookup of EVSE status grouped by their EVSE operator.</param>
        /// <param name="ActionType">The server-side data management operation.</param>
        /// <param name="OperatorId">An optional unique identification of the EVSE operator.</param>
        /// <param name="OperatorName">The optional name of the EVSE operator.</param>
        /// <param name="QueryTimeout">An optional timeout of the HTTP client [default 60 sec.]</param>
        Task<Acknowledgement> PushEVSEStatus(ILookup<EVSEOperator_Id, EVSEStatus>  GroupedEVSEStatus,
                                             ActionType                            ActionType    = ActionType.update,
                                             EVSEOperator_Id                       OperatorId    = null,
                                             String                                OperatorName  = null,
                                             TimeSpan?                             QueryTimeout  = null);

        /// <summary>
        /// Upload the given EVSE status.
        /// </summary>
        /// <param name="EVSEStatus">An EVSE status.</param>
        /// <param name="ActionType">The server-side data management operation.</param>
        /// <param name="OperatorId">An optional unique identification of the EVSE operator.</param>
        /// <param name="OperatorName">The optional name of the EVSE operator.</param>
        /// <param name="QueryTimeout">An optional timeout of the HTTP client [default 60 sec.]</param>
        Task<Acknowledgement> PushEVSEStatus(EVSEStatus       EVSEStatus,
                                             ActionType       ActionType    = ActionType.update,
                                             EVSEOperator_Id  OperatorId    = null,
                                             String           OperatorName  = null,
                                             TimeSpan?        QueryTimeout  = null);

        /// <summary>
        /// Upload the given enumeration of EVSE status.
        /// </summary>
        /// <param name="EVSEStatus">An enumeration of EVSE status.</param>
        /// <param name="ActionType">The server-side data management operation.</param>
        /// <param name="OperatorId">An optional unique identification of the EVSE operator.</param>
        /// <param name="OperatorName">The optional name of the EVSE operator.</param>
        /// <param name="QueryTimeout">An optional timeout of the HTTP client [default 60 sec.]</param>
        Task<Acknowledgement> PushEVSEStatus(IEnumerable<EVSEStatus>  EVSEStatus,
                                             ActionType               ActionType    = ActionType.update,
                                             EVSEOperator_Id          OperatorId    = null,
                                             String                   OperatorName  = null,
                                             TimeSpan?                QueryTimeout  = null);

        /// <summary>
        /// Upload the EVSE status of the given EVSE.
        /// </summary>
        /// <param name="EVSE">An EVSE.</param>
        /// <param name="ActionType">The server-side data management operation.</param>
        /// <param name="OperatorId">An optional unique identification of the EVSE operator.</param>
        /// <param name="OperatorName">The optional name of the EVSE operator.</param>
        /// <param name="IncludeEVSEs">Only upload the EVSEs returned by the given filter delegate.</param>
        /// <param name="QueryTimeout">An optional timeout of the HTTP client [default 60 sec.]</param>
        Task<Acknowledgement> PushEVSEStatus(EVSE                 EVSE,
                                             ActionType           ActionType    = ActionType.update,
                                             EVSEOperator_Id      OperatorId    = null,
                                             String               OperatorName  = null,
                                             Func<EVSE, Boolean>  IncludeEVSEs  = null,
                                             TimeSpan?            QueryTimeout  = null);

        /// <summary>
        /// Upload all EVSE status of the given enumeration of EVSEs.
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
        /// Upload all EVSE status of the given charging station.
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
        /// Upload all EVSE status of the given enumeration of charging stations.
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
        /// Upload all EVSE status of the given charging pool.
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
        /// Upload all EVSE status of the given enumeration of charging pools.
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
        /// Upload all EVSE status of the given EVSE operator.
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
        /// Upload all EVSE status of the given enumeration of EVSE operators.
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
        /// Upload all EVSE status of the given roaming network.
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


        #endregion


        void RemoveChargingStations(DateTime                      Timestamp,
                                   IEnumerable<ChargingStation>  ChargingStations);

    }

}
