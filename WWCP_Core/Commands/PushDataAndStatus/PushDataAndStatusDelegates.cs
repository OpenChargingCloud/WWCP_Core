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

using org.GraphDefined.Vanaheimr.Illias;
using System;
using System.Linq;

#endregion

namespace org.GraphDefined.WWCP
{

    /// <summary>
    /// A delegate called whenever new EVSE data will be send upstream.
    /// </summary>
    public delegate void OnPushEVSEDataRequestDelegate (DateTime                      LogTimestamp,
                                                        DateTime                      RequestTimestamp,
                                                        Object                        Sender,
                                                        String                        SenderId,
                                                        EventTracking_Id              EventTrackingId,
                                                        RoamingNetwork_Id             RoamingNetworkId,
                                                        ActionType                    ActionType,
                                                        ILookup<EVSEOperator, EVSE>   EVSEData,
                                                        UInt32                        NumberOfEVSEs,
                                                        TimeSpan?                     RequestTimeout);


    /// <summary>
    /// A delegate called whenever new EVSE data had been send upstream.
    /// </summary>
    public delegate void OnPushEVSEDataResponseDelegate(DateTime                      LogTimestamp,
                                                        DateTime                      RequestTimestamp,
                                                        Object                        Sender,
                                                        String                        SenderId,
                                                        EventTracking_Id              EventTrackingId,
                                                        RoamingNetwork_Id             RoamingNetworkId,
                                                        ActionType                    ActionType,
                                                        ILookup<EVSEOperator, EVSE>   EVSEData,
                                                        UInt32                        NumberOfEVSEs,
                                                        TimeSpan?                     RequestTimeout,
                                                        Acknowledgement               Result,
                                                        TimeSpan                      Runtime);


    /// <summary>
    /// A delegate called whenever new EVSE status will be send upstream.
    /// </summary>
    public delegate void OnPushEVSEStatusRequestDelegate (DateTime                            LogTimestamp,
                                                          DateTime                            RequestTimestamp,
                                                          Object                              Sender,
                                                          String                              SenderId,
                                                          EventTracking_Id                    EventTrackingId,
                                                          RoamingNetwork_Id                   RoamingNetworkId,
                                                          ActionType                          ActionType,
                                                          ILookup<EVSEOperator, EVSEStatus>   EVSEStatus,
                                                          UInt32                              NumberOfEVSEs,
                                                          TimeSpan?                           RequestTimeout);


    /// <summary>
    /// A delegate called whenever new EVSE status had been send upstream.
    /// </summary>
    public delegate void OnPushEVSEStatusResponseDelegate(DateTime                            LogTimestamp,
                                                          DateTime                            RequestTimestamp,
                                                          Object                              Sender,
                                                          String                              SenderId,
                                                          EventTracking_Id                    EventTrackingId,
                                                          RoamingNetwork_Id                   RoamingNetworkId,
                                                          ActionType                          ActionType,
                                                          ILookup<EVSEOperator, EVSEStatus>   EVSEStatus,
                                                          UInt32                              NumberOfEVSEs,
                                                          TimeSpan?                           RequestTimeout,
                                                          Acknowledgement                     Result,
                                                          TimeSpan                            Runtime);

    public delegate Boolean IncludeEVSEDelegate(EVSE EVSE);

}
