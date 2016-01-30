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

using org.GraphDefined.Vanaheimr.Illias;
using System.Threading;
using System.Threading.Tasks;

#endregion

namespace org.GraphDefined.WWCP
{

    /// <summary>
    /// An event fired whenever an authentication token will be verified to stop a charging process.
    /// </summary>
    /// <param name="Timestamp">The timestamp of the request.</param>
    /// <param name="Sender">The sender of the request.</param>
    /// <param name="EventTrackingId">An unique event tracking identification for correlating this request with other events.</param>
    /// <param name="RoamingNetworkId">The unique identification for the roaming network.</param>
    /// <param name="OperatorId">An EVSE operator identification.</param>
    /// <param name="SessionId">The unique identification for this charging session.</param>
    /// <param name="AuthToken">A (RFID) user identification.</param>
    /// <param name="QueryTimeout">An optional timeout for this request.</param>
    public delegate void OnAuthorizeStopDelegate(DateTime            Timestamp,
                                                 Object              Sender,
                                                 EventTracking_Id    EventTrackingId,
                                                 RoamingNetwork_Id   RoamingNetworkId,
                                                 EVSEOperator_Id     OperatorId,
                                                 ChargingSession_Id  SessionId,
                                                 Auth_Token          AuthToken,
                                                 TimeSpan?           QueryTimeout);

    /// <summary>
    /// An event fired whenever an authentication token had been verified to stop a charging process.
    /// </summary>
    /// <param name="Timestamp">The timestamp of the request.</param>
    /// <param name="Sender">The sender of the request.</param>
    /// <param name="EventTrackingId">An unique event tracking identification for correlating this request with other events.</param>
    /// <param name="RoamingNetworkId">The unique identification for the roaming network.</param>
    /// <param name="OperatorId">An EVSE operator identification.</param>
    /// <param name="SessionId">The unique identification for this charging session.</param>
    /// <param name="AuthToken">A (RFID) user identification.</param>
    /// <param name="QueryTimeout">An optional timeout for this request.</param>
    /// <param name="Result">The authorize stop result.</param>
    public delegate void OnAuthorizeStoppedDelegate(DateTime            Timestamp,
                                                    Object              Sender,
                                                    EventTracking_Id    EventTrackingId,
                                                    RoamingNetwork_Id   RoamingNetworkId,
                                                    EVSEOperator_Id     OperatorId,
                                                    ChargingSession_Id  SessionId,
                                                    Auth_Token          AuthToken,
                                                    TimeSpan?           QueryTimeout,
                                                    AuthStopResult      Result);


    // ----------------------------------------------------------------------------------------------------------


    /// <summary>
    /// An event fired whenever an authentication token will be verified to stop a charging process at the given EVSE.
    /// </summary>
    /// <param name="Timestamp">The timestamp of the request.</param>
    /// <param name="Sender">The sender of the request.</param>
    /// <param name="EventTrackingId">An unique event tracking identification for correlating this request with other events.</param>
    /// <param name="RoamingNetworkId">The unique identification for the roaming network.</param>
    /// <param name="OperatorId">An EVSE operator identification.</param>
    /// <param name="EVSEId">The unique identification of an EVSE.</param>
    /// <param name="SessionId">The unique identification for this charging session.</param>
    /// <param name="AuthToken">A (RFID) user identification.</param>
    /// <param name="QueryTimeout">An optional timeout for this request.</param>
    public delegate void OnAuthorizeEVSEStopDelegate(DateTime            Timestamp,
                                                     Object              Sender,
                                                     EventTracking_Id    EventTrackingId,
                                                     RoamingNetwork_Id   RoamingNetworkId,
                                                     EVSEOperator_Id     OperatorId,
                                                     EVSE_Id             EVSEId,
                                                     ChargingSession_Id  SessionId,
                                                     Auth_Token          AuthToken,
                                                     TimeSpan?           QueryTimeout);

    /// <summary>
    /// An event fired whenever an authentication token had been verified to stop a charging process at the given EVSE.
    /// </summary>
    /// <param name="Timestamp">The timestamp of the request.</param>
    /// <param name="Sender">The sender of the request.</param>
    /// <param name="EventTrackingId">An unique event tracking identification for correlating this request with other events.</param>
    /// <param name="RoamingNetworkId">The unique identification for the roaming network.</param>
    /// <param name="OperatorId">An EVSE operator identification.</param>
    /// <param name="EVSEId">The unique identification of an EVSE.</param>
    /// <param name="SessionId">The unique identification for this charging session.</param>
    /// <param name="AuthToken">A (RFID) user identification.</param>
    /// <param name="QueryTimeout">An optional timeout for this request.</param>
    /// <param name="Result">The authorize stop result.</param>
    public delegate void OnAuthorizeEVSEStoppedDelegate(DateTime            Timestamp,
                                                        Object              Sender,
                                                        EventTracking_Id    EventTrackingId,
                                                        RoamingNetwork_Id   RoamingNetworkId,
                                                        EVSEOperator_Id     OperatorId,
                                                        EVSE_Id             EVSEId,
                                                        ChargingSession_Id  SessionId,
                                                        Auth_Token          AuthToken,
                                                        TimeSpan?           QueryTimeout,
                                                        AuthStopEVSEResult  Result);



    /// <summary>
    /// An event fired whenever an authentication token had been verified to stop a charging process at the given EVSE.
    /// </summary>
    /// <param name="Timestamp">The timestamp of the request.</param>
    /// <param name="CancellationToken">A token to cancel this request.</param>
    /// <param name="EventTrackingId">An unique event tracking identification for correlating this request with other events.</param>
    /// <param name="OperatorId">An EVSE operator identification.</param>
    /// <param name="EVSEId">The unique identification of an EVSE.</param>
    /// <param name="SessionId">The unique identification for this charging session.</param>
    /// <param name="AuthToken">A (RFID) user identification.</param>
    /// <param name="QueryTimeout">An optional timeout for this request.</param>
    public delegate Task<AuthStopEVSEResult> OnAuthorizeStopEVSEDelegate(DateTime            Timestamp,
                                                                         CancellationToken   CancellationToken,
                                                                         EventTracking_Id    EventTrackingId,
                                                                         EVSEOperator_Id     OperatorId,
                                                                         EVSE_Id             EVSEId,
                                                                         ChargingSession_Id  SessionId,
                                                                         Auth_Token          AuthToken,
                                                                         TimeSpan?           QueryTimeout);

    // ----------------------------------------------------------------------------------------------------------



    /// <summary>
    /// An event fired whenever an authentication token will be verified to stop a charging process at the given charging station.
    /// </summary>
    /// <param name="Timestamp">The timestamp of the request.</param>
    /// <param name="Sender">The sender of the request.</param>
    /// <param name="EventTrackingId">An unique event tracking identification for correlating this request with other events.</param>
    /// <param name="RoamingNetworkId">The unique identification for the roaming network.</param>
    /// <param name="OperatorId">An EVSE operator identification.</param>
    /// <param name="ChargingStationId">The unique identification of a charging station.</param>
    /// <param name="SessionId">The unique identification for this charging session.</param>
    /// <param name="AuthToken">A (RFID) user identification.</param>
    /// <param name="QueryTimeout">An optional timeout for this request.</param>
    public delegate void OnAuthorizeChargingStationStopDelegate(DateTime            Timestamp,
                                                                Object              Sender,
                                                                EventTracking_Id    EventTrackingId,
                                                                RoamingNetwork_Id   RoamingNetworkId,
                                                                EVSEOperator_Id     OperatorId,
                                                                ChargingStation_Id  ChargingStationId,
                                                                ChargingSession_Id  SessionId,
                                                                Auth_Token          AuthToken,
                                                                TimeSpan?           QueryTimeout);

    /// <summary>
    /// An event fired whenever an authentication token had been verified to stop a charging process at the given charging station.
    /// </summary>
    /// <param name="Timestamp">The timestamp of the request.</param>
    /// <param name="Sender">The sender of the request.</param>
    /// <param name="EventTrackingId">An unique event tracking identification for correlating this request with other events.</param>
    /// <param name="RoamingNetworkId">The unique identification for the roaming network.</param>
    /// <param name="OperatorId">An EVSE operator identification.</param>
    /// <param name="ChargingStationId">The unique identification of a charging station.</param>
    /// <param name="SessionId">The unique identification for this charging session.</param>
    /// <param name="AuthToken">A (RFID) user identification.</param>
    /// <param name="QueryTimeout">An optional timeout for this request.</param>
    /// <param name="Result">The authorize stop result.</param>
    public delegate void OnAuthorizeChargingStationStoppedDelegate(DateTime                       Timestamp,
                                                                   Object                         Sender,
                                                                   EventTracking_Id               EventTrackingId,
                                                                   RoamingNetwork_Id              RoamingNetworkId,
                                                                   EVSEOperator_Id                OperatorId,
                                                                   ChargingStation_Id             ChargingStationId,
                                                                   ChargingSession_Id             SessionId,
                                                                   Auth_Token                     AuthToken,
                                                                   TimeSpan?                      QueryTimeout,
                                                                   AuthStopChargingStationResult  Result);

}
