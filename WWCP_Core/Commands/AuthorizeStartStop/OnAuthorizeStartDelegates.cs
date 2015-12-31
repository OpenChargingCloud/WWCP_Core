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
using System.Threading;
using System.Threading.Tasks;

using org.GraphDefined.WWCP;

#endregion

namespace org.GraphDefined.WWCP
{

    /// <summary>
    /// Indicate an authorize start.
    /// </summary>
    /// <param name="Sender">The sender of the request.</param>
    /// <param name="Timestamp">The timestamp of the request.</param>
    /// <param name="RoamingNetworkId">The unique identification for the roaming network.</param>
    /// <param name="OperatorId">An EVSE operator identification.</param>
    /// <param name="AuthToken">A (RFID) user identification.</param>
    /// <param name="ChargingProductId">The unique identification of the choosen charging product at the given EVSE.</param>
    /// <param name="SessionId">The unique identification for this charging session.</param>
    public delegate void OnAuthorizeStartDelegate(Object              Sender,
                                                  DateTime            Timestamp,
                                                  RoamingNetwork_Id   RoamingNetworkId,
                                                  EVSEOperator_Id     OperatorId,
                                                  Auth_Token          AuthToken,
                                                  ChargingProduct_Id  ChargingProductId  = null,
                                                  ChargingSession_Id  SessionId          = null);

    /// <summary>
    /// Indicate an authorize start.
    /// </summary>
    /// <param name="Sender">The sender of the request.</param>
    /// <param name="Timestamp">The timestamp of the request.</param>
    /// <param name="RoamingNetworkId">The unique identification for the roaming network.</param>
    /// <param name="Result">The authorize start result.</param>
    public delegate void OnAuthorizeStartedDelegate(Object             Sender,
                                                    DateTime           Timestamp,
                                                    RoamingNetwork_Id  RoamingNetworkId,
                                                    AuthStartResult    Result);


    // ----------------------------------------------------------------------------------------------------------


    /// <summary>
    /// Indicate an authorize start at the given EVSE.
    /// </summary>
    /// <param name="Sender">The sender of the request.</param>
    /// <param name="Timestamp">The timestamp of the request.</param>
    /// <param name="RoamingNetworkId">The unique identification for the roaming network.</param>
    /// <param name="OperatorId">An EVSE operator identification.</param>
    /// <param name="AuthToken">A (RFID) user identification.</param>
    /// <param name="EVSEId">The unique identification of an EVSE.</param>
    /// <param name="ChargingProductId">The unique identification of the choosen charging product at the given EVSE.</param>
    /// <param name="SessionId">The unique identification for this charging session.</param>
    public delegate void OnAuthorizeEVSEStartDelegate(Object              Sender,
                                                      DateTime            Timestamp,
                                                      RoamingNetwork_Id   RoamingNetworkId,
                                                      EVSEOperator_Id     OperatorId,
                                                      Auth_Token          AuthToken,
                                                      EVSE_Id             EVSEId,
                                                      ChargingProduct_Id  ChargingProductId  = null,
                                                      ChargingSession_Id  SessionId          = null);

    /// <summary>
    /// Indicate an authorize start at the given EVSE.
    /// </summary>
    /// <param name="Sender">The sender of the request.</param>
    /// <param name="Timestamp">The timestamp of the request.</param>
    /// <param name="RoamingNetworkId">The unique identification for the roaming network.</param>
    /// <param name="EVSEId">The unique identification of an EVSE.</param>
    /// <param name="Result">The authorize start result.</param>
    public delegate void OnAuthorizeEVSEStartedDelegate(Object               Sender,
                                                        DateTime             Timestamp,
                                                        RoamingNetwork_Id    RoamingNetworkId,
                                                        EVSE_Id              EVSEId,
                                                        AuthStartEVSEResult  Result);


    // ----------------------------------------------------------------------------------------------------------



    /// <summary>
    /// Indicate an authorize start at the given charging station.
    /// </summary>
    /// <param name="Sender">The sender of the request.</param>
    /// <param name="Timestamp">The timestamp of the request.</param>
    /// <param name="RoamingNetworkId">The unique identification for the roaming network.</param>
    /// <param name="OperatorId">An EVSE operator identification.</param>
    /// <param name="AuthToken">A (RFID) user identification.</param>
    /// <param name="ChargingStationId">The unique identification of a charging station.</param>
    /// <param name="ChargingProductId">The unique identification of the choosen charging product at the given charging station.</param>
    /// <param name="SessionId">The unique identification for this charging session.</param>
    public delegate void OnAuthorizeChargingStationStartDelegate(Object              Sender,
                                                                 DateTime            Timestamp,
                                                                 RoamingNetwork_Id   RoamingNetworkId,
                                                                 EVSEOperator_Id     OperatorId,
                                                                 Auth_Token          AuthToken,
                                                                 ChargingStation_Id  ChargingStationId,
                                                                 ChargingProduct_Id  ChargingProductId  = null,
                                                                 ChargingSession_Id  SessionId          = null);

    /// <summary>
    /// Indicate an authorize start at the given charging station.
    /// </summary>
    /// <param name="Sender">The sender of the request.</param>
    /// <param name="Timestamp">The timestamp of the request.</param>
    /// <param name="RoamingNetworkId">The unique identification for the roaming network.</param>
    /// <param name="ChargingStationId">The unique identification of a charging station.</param>
    /// <param name="Result">The authorize start result.</param>
    public delegate void OnAuthorizeChargingStationStartedDelegate(Object                          Sender,
                                                                   DateTime                        Timestamp,
                                                                   RoamingNetwork_Id               RoamingNetworkId,
                                                                   ChargingStation_Id              ChargingStationId,
                                                                   AuthStartChargingStationResult  Result);




}


