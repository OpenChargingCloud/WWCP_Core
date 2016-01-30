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


}
