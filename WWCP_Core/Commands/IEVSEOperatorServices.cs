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
    public delegate void OnEVSEDataPushDelegate(DateTime                                  Timestamp,
                                                Object                                    Sender,
                                                String                                    SenderId,
                                                RoamingNetwork_Id                         RoamingNetworkId,
                                                ActionType                                ActionType,
                                                ILookup<EVSEOperator, IEnumerable<EVSE>>  EVSEData,
                                                UInt32                                    NumberOfEVSEs);


    /// <summary>
    /// A delegate called whenever new EVSE data had been send upstream.
    /// </summary>
    public delegate void OnEVSEDataPushedDelegate(DateTime                                  Timestamp,
                                                  Object                                    Sender,
                                                  String                                    SenderId,
                                                  RoamingNetwork_Id                         RoamingNetworkId,
                                                  ActionType                                ActionType,
                                                  ILookup<EVSEOperator, IEnumerable<EVSE>>  EVSEData,
                                                  UInt32                                    NumberOfEVSEs,
                                                  Acknowledgement                           Result);



    /// <summary>
    /// The EV Roaming Provider provided EVSE Operator services interface.
    /// </summary>
    public interface IEVSEOperatorServices
    {

        event OnEVSEDataPushDelegate    OnEVSEDataPush;
        event OnEVSEDataPushedDelegate  OnEVSEDataPushed;


        #region PushEVSEStatus

        /// <summary>
        /// Create a new task pushing EVSE status key-value-pairs.
        /// </summary>
        /// <param name="EVSEStatus">An enumeration of EVSE identification and status key-value-pairs.</param>
        /// <param name="OICPAction">An optional action.</param>
        /// <param name="OperatorId">An optional EVSE operator identification to use. Otherwise it will be taken from the EVSE data records.</param>
        /// <param name="QueryTimeout">An optional timeout for this query.</param>
        Task<Acknowledgement> PushEVSEStatus(IEnumerable<KeyValuePair<EVSE_Id, EVSEStatusType>>  EVSEStatus,
                                             ActionType                                          OICPAction    = ActionType.update,
                                             EVSEOperator_Id                                     OperatorId    = null,
                                             TimeSpan?                                           QueryTimeout  = null);


        ///// <summary>
        ///// Create a new task pushing EVSE status records.
        ///// </summary>
        ///// <param name="EVSEStatus">An enumeration of EVSE Id and status records.</param>
        ///// <param name="OICPAction">An optional OICP action.</param>
        ///// <param name="OperatorId">An optional EVSE operator identification to use. Otherwise it will be taken from the EVSE data records.</param>
        ///// <param name="OperatorName">An optional EVSE operator name.</param>
        ///// <param name="QueryTimeout">An optional timeout for this query.</param>
        //Task<eRoamingAcknowledgement> PushEVSEStatus(IEnumerable<EVSEStatusRecord>  EVSEStatus,
        //                                             ActionType                     OICPAction    = ActionType.update,
        //                                             EVSEOperator_Id                OperatorId    = null,
        //                                             String                         OperatorName  = null,
        //                                             TimeSpan?                      QueryTimeout  = null);

        /// <summary>
        /// Send EVSE status updates.
        /// </summary>
        /// <param name="EVSEStatusDiff">An EVSE status diff.</param>
        /// <param name="QueryTimeout">An optional timeout for this query.</param>
        Task PushEVSEStatus(EVSEStatusDiff  EVSEStatusDiff,
                            TimeSpan?       QueryTimeout = null);

        #endregion

        //#region PullAuthenticationData
        //
        ///// <summary>
        ///// Create an OICP v2.0 PullAuthenticationData request.
        ///// </summary>
        ///// <param name="OperatorId">An EVSE operator identification.</param>
        ///// <param name="QueryTimeout">An optional timeout for this query.</param>
        //Task<eRoamingAuthenticationData> PullAuthenticationData(EVSEOperator_Id  OperatorId,
        //                                                        TimeSpan?        QueryTimeout = null);
        //
        //#endregion

        #region AuthorizeStart

        /// <summary>
        /// Create an authorize start request.
        /// </summary>
        /// <param name="OperatorId">An EVSE operator identification.</param>
        /// <param name="AuthToken">A (RFID) user identification.</param>
        /// <param name="ChargingProductId">An optional charging product identification.</param>
        /// <param name="SessionId">An optional session identification.</param>
        /// <param name="QueryTimeout">An optional timeout for this query.</param>
        Task<AuthStartResult> AuthorizeStart(EVSEOperator_Id      OperatorId,
                                             Auth_Token           AuthToken,
                                             ChargingProduct_Id   ChargingProductId  = null,
                                             ChargingSession_Id   SessionId          = null,
                                             TimeSpan?            QueryTimeout       = null);

        /// <summary>
        /// Create an authorize start request at the given EVSE.
        /// </summary>
        /// <param name="OperatorId">An EVSE operator identification.</param>
        /// <param name="AuthToken">A (RFID) user identification.</param>
        /// <param name="EVSEId">The unique identification of an EVSE.</param>
        /// <param name="ChargingProductId">An optional charging product identification.</param>
        /// <param name="SessionId">An optional session identification.</param>
        /// <param name="QueryTimeout">An optional timeout for this query.</param>
        Task<AuthStartEVSEResult> AuthorizeStart(EVSEOperator_Id      OperatorId,
                                                 Auth_Token           AuthToken,
                                                 EVSE_Id              EVSEId,
                                                 ChargingProduct_Id   ChargingProductId  = null,
                                                 ChargingSession_Id   SessionId          = null,
                                                 TimeSpan?            QueryTimeout       = null);

        /// <summary>
        /// Create an authorize start request at the given charging station.
        /// </summary>
        /// <param name="OperatorId">An EVSE operator identification.</param>
        /// <param name="AuthToken">A (RFID) user identification.</param>
        /// <param name="ChargingStationId">The unique identification of a charging station.</param>
        /// <param name="ChargingProductId">An optional charging product identification.</param>
        /// <param name="SessionId">An optional session identification.</param>
        /// <param name="QueryTimeout">An optional timeout for this query.</param>
        Task<AuthStartChargingStationResult> AuthorizeStart(EVSEOperator_Id      OperatorId,
                                                            Auth_Token           AuthToken,
                                                            ChargingStation_Id   ChargingStationId,
                                                            ChargingProduct_Id   ChargingProductId  = null,
                                                            ChargingSession_Id   SessionId          = null,
                                                            TimeSpan?            QueryTimeout       = null);

        #endregion

        #region AuthorizeStop

        /// <summary>
        /// Create an authorize stop request.
        /// </summary>
        /// <param name="OperatorId">An EVSE operator identification.</param>
        /// <param name="SessionId">The session identification from the AuthorizeStart request.</param>
        /// <param name="AuthToken">A (RFID) user identification.</param>
        /// <param name="QueryTimeout">An optional timeout for this query.</param>
        Task<AuthStopResult> AuthorizeStop(EVSEOperator_Id     OperatorId,
                                           ChargingSession_Id  SessionId,
                                           Auth_Token          AuthToken,
                                           TimeSpan?           QueryTimeout  = null);

        /// <summary>
        /// Create an authorize stop request at the given EVSE.
        /// </summary>
        /// <param name="OperatorId">An EVSE operator identification.</param>
        /// <param name="EVSEId">The unique identification of an EVSE.</param>
        /// <param name="SessionId">The session identification from the AuthorizeStart request.</param>
        /// <param name="AuthToken">A (RFID) user identification.</param>
        /// <param name="QueryTimeout">An optional timeout for this query.</param>
        Task<AuthStopEVSEResult> AuthorizeStop(EVSEOperator_Id     OperatorId,
                                               EVSE_Id             EVSEId,
                                               ChargingSession_Id  SessionId,
                                               Auth_Token          AuthToken,
                                               TimeSpan?           QueryTimeout  = null);

        /// <summary>
        /// Create an authorize stop request at the given charging station.
        /// </summary>
        /// <param name="OperatorId">An EVSE operator identification.</param>
        /// <param name="ChargingStationId">A charging station identification.</param>
        /// <param name="SessionId">The session identification from the AuthorizeStart request.</param>
        /// <param name="AuthToken">A (RFID) user identification.</param>
        /// <param name="QueryTimeout">An optional timeout for this query.</param>
        Task<AuthStopChargingStationResult> AuthorizeStop(EVSEOperator_Id     OperatorId,
                                                          ChargingStation_Id  ChargingStationId,
                                                          ChargingSession_Id  SessionId,
                                                          Auth_Token          AuthToken,
                                                          TimeSpan?           QueryTimeout  = null);

        #endregion

        #region SendChargeDetailRecord

        /// <summary>
        /// Create an SendChargeDetailRecord request.
        /// </summary>
        /// <param name="ChargeDetailRecord">A charge detail record.</param>
        /// <param name="QueryTimeout">An optional timeout for this query.</param>
        Task<SendCDRResult> SendChargeDetailRecord(ChargeDetailRecord  ChargeDetailRecord,
                                                   TimeSpan?           QueryTimeout = null);

        /// <summary>
        /// Create a SendChargeDetailRecord request.
        /// </summary>
        /// <param name="EVSEId">An EVSE identification.</param>
        /// <param name="SessionId">The session identification from the Authorize Start request.</param>
        /// <param name="ChargingProductId">An optional charging product identification.</param>
        /// <param name="SessionStart">The timestamp of the session start.</param>
        /// <param name="SessionEnd">The timestamp of the session end.</param>
        /// <param name="AuthInfo">AuthInfo</param>.
        /// <param name="ChargingStart">An optional charging start timestamp.</param>
        /// <param name="ChargingEnd">An optional charging end timestamp.</param>
        /// <param name="MeterValueStart">An optional initial value of the energy meter.</param>
        /// <param name="MeterValueEnd">An optional final value of the energy meter.</param>
        /// <param name="MeterValuesInBetween">An optional enumeration of meter values during the charging session.</param>
        /// <param name="ConsumedEnergy">The optional amount of consumed energy.</param>
        /// <param name="MeteringSignature">An optional signature for the metering values.</param>
        /// <param name="HubOperatorId">An optional identification of the hub operator.</param>
        /// <param name="HubProviderId">An optional identification of the hub provider.</param>
        /// <param name="QueryTimeout">An optional timeout for this query.</param>
        Task<SendCDRResult> SendChargeDetailRecord(EVSE_Id              EVSEId,
                                                   ChargingSession_Id   SessionId,
                                                   ChargingProduct_Id   ChargingProductId,
                                                   DateTime             SessionStart,
                                                   DateTime             SessionEnd,
                                                   AuthInfo             AuthInfo,
                                                   DateTime?            ChargingStart         = null,
                                                   DateTime?            ChargingEnd           = null,
                                                   Double?              MeterValueStart       = null,
                                                   Double?              MeterValueEnd         = null,
                                                   IEnumerable<Double>  MeterValuesInBetween  = null,
                                                   Double?              ConsumedEnergy        = null,
                                                   String               MeteringSignature     = null,
                                                   HubOperator_Id       HubOperatorId         = null,
                                                   EVSP_Id              HubProviderId         = null,
                                                   TimeSpan?            QueryTimeout          = null);

        #endregion

    }

}
