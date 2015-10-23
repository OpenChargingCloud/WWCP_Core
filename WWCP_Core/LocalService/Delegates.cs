/*
 * Copyright (c) 2014-2015 GraphDefined GmbH
 * This file is part of WWCP OICP <https://github.com/WorldWideCharging/WWCP_OICP>
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
using System.Xml.Linq;
using System.Collections.Generic;

using Newtonsoft.Json.Linq;

using org.GraphDefined.Vanaheimr.Illias;
using org.GraphDefined.Vanaheimr.Illias.ConsoleLog;
using org.GraphDefined.Vanaheimr.Hermod;
using org.GraphDefined.Vanaheimr.Hermod.HTTP;

using org.GraphDefined.WWCP.LocalService;

#endregion

namespace org.GraphDefined.WWCP
{

    /// <summary>
    /// A delegate fired whenever a remote start command was received.
    /// </summary>
    /// <param name="Timestamp">The timestamp of the request.</param>
    /// <param name="RoamingNetworkId">The unique identification for the roaming network.</param>
    /// <param name="SessionId">The unique identification for this charging session.</param>
    /// <param name="ProviderId">The unique identification of the e-mobility service provider.</param>
    /// <param name="eMAId">The unique identification of the e-mobility account.</param>
    /// <param name="EVSEId">The unique identification of an EVSE.</param>
    /// <param name="EventTrackingId">An optional unique identification for tracking related events.</param>
    /// <returns>A remote start result object.</returns>
    public delegate RemoteStartResult OnRemoteStartDelegate(DateTime            Timestamp,
                                                            RoamingNetwork_Id   RoamingNetworkId,
                                                            ChargingSession_Id  SessionId,
                                                            EVSP_Id             ProviderId,
                                                            eMA_Id              eMAId,
                                                            EVSE_Id             EVSEId,
                                                            EventTracking_Id    EventTrackingId  = null);


    /// <summary>
    /// A delegate fired whenever a remote stop command was received.
    /// </summary>
    /// <param name="Timestamp">The timestamp of the request.</param>
    /// <param name="RoamingNetworkId">The unique identification for the roaming network.</param>
    /// <param name="SessionId">The unique identification for this charging session.</param>
    /// <param name="ProviderId">The unique identification of the e-mobility service provider.</param>
    /// <param name="EVSEId">The unique identification of an EVSE.</param>
    /// <param name="EventTrackingId">An optional unique identification for tracking related events.</param>
    /// <returns>A remote stop result object.</returns>
    public delegate RemoteStopResult  OnRemoteStopDelegate (DateTime            Timestamp,
                                                            RoamingNetwork_Id   RoamingNetworkId,
                                                            ChargingSession_Id  SessionId,
                                                            EVSP_Id             ProviderId,
                                                            EVSE_Id             EVSEId,
                                                            EventTracking_Id    EventTrackingId  = null);

}
