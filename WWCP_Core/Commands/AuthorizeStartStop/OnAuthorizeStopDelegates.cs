/*
 * Copyright (c) 2014-2021 GraphDefined GmbH <achim.friedland@graphdefined.com>
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
using System.Threading.Tasks;
using System.Collections.Generic;

using org.GraphDefined.Vanaheimr.Illias;

#endregion

namespace org.GraphDefined.WWCP
{

    /// <summary>
    /// An event fired whenever an authentication token will be verified to stop a charging process at the given EVSE.
    /// </summary>
    /// <param name="Timestamp">The timestamp of the request.</param>
    /// <param name="Sender">The sender of the request.</param>
    /// <param name="EventTrackingId">An unique event tracking identification for correlating this request with other events.</param>
    /// <param name="RoamingNetworkId">The unique identification for the roaming network.</param>
    /// <param name="OperatorId">An Charging Station Operator identification.</param>
    /// <param name="ChargingLocation">The charging location.</param>
    /// <param name="SessionId">The unique identification for this charging session.</param>
    /// <param name="LocalAuthentication">An user identification.</param>
    /// <param name="RequestTimeout">An optional timeout for this request.</param>
    public delegate Task OnAuthorizeStopRequestDelegate(DateTime                      LogTimestamp,
                                                        DateTime                      RequestTimestamp,
                                                        Object                        Sender,
                                                        String                        SenderId,
                                                        EventTracking_Id              EventTrackingId,
                                                        RoamingNetwork_Id             RoamingNetworkId,
                                                        CSORoamingProvider_Id?        EMPRoamingProviderId,
                                                        EMPRoamingProvider_Id?        CSORoamingProviderId,
                                                        ChargingStationOperator_Id?   OperatorId,
                                                        ChargingLocation              ChargingLocation,
                                                        ChargingSession_Id?           SessionId,
                                                        LocalAuthentication           LocalAuthentication,
                                                        TimeSpan?                     RequestTimeout);

    /// <summary>
    /// An event fired whenever an authentication token had been verified to stop a charging process at the given EVSE.
    /// </summary>
    /// <param name="Timestamp">The timestamp of the request.</param>
    /// <param name="Sender">The sender of the request.</param>
    /// <param name="EventTrackingId">An unique event tracking identification for correlating this request with other events.</param>
    /// <param name="RoamingNetworkId">The unique identification for the roaming network.</param>
    /// <param name="OperatorId">An Charging Station Operator identification.</param>
    /// <param name="ChargingLocation">The charging location.</param>
    /// <param name="SessionId">The unique identification for this charging session.</param>
    /// <param name="LocalAuthentication">An user identification.</param>
    /// <param name="RequestTimeout">An optional timeout for this request.</param>
    /// <param name="Result">The authorize stop result.</param>
    public delegate Task OnAuthorizeStopResponseDelegate(DateTime                      LogTimestamp,
                                                         DateTime                      RequestTimestamp,
                                                         Object                        Sender,
                                                         String                        SenderId,
                                                         EventTracking_Id              EventTrackingId,
                                                         RoamingNetwork_Id             RoamingNetworkId,
                                                         CSORoamingProvider_Id?        EMPRoamingProviderId,
                                                         EMPRoamingProvider_Id?        CSORoamingProviderId,
                                                         ChargingStationOperator_Id?   OperatorId,
                                                         ChargingLocation              ChargingLocation,
                                                         ChargingSession_Id?           SessionId,
                                                         LocalAuthentication           LocalAuthentication,
                                                         TimeSpan?                     RequestTimeout,
                                                         AuthStopResult                Result,
                                                         TimeSpan                      Runtime);

}
