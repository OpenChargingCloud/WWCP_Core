/*
 * Copyright (c) 2014-2015 GraphDefined GmbH
 * This file is part of WWCP Core <https://github.com/WorldWideCharging/WWCP_Core>
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
using System.Text;

#endregion

namespace org.GraphDefined.WWCP.LocalService
{

    /// <summary>
    /// The EVSE operator provided E-Mobility services interface.
    /// </summary>
    public interface IRemoteStartStop
    {

        Authorizator_Id AuthorizatorId { get; }


        /// <summary>
        /// Initiate a remote start of a charging station socket outlet.
        /// </summary>
        /// <param name="EVSEId">The unique identification of an EVSE.</param>
        /// <param name="SessionId">The unique identification for this charging session.</param>
        /// <param name="ProviderId">The unique identification of the e-mobility service provider.</param>
        /// <param name="eMAId">The unique identification of the e-mobility account.</param>
        /// <param name="EventTrackingId">An optional unique identification for tracking related events.</param>
        RemoteStartResult RemoteStart(EVSE_Id             EVSEId,
                                      ChargingSession_Id  SessionId,
                                      EVSP_Id             ProviderId,
                                      eMA_Id              eMAId,
                                      EventTracking_Id    EventTrackingId  = null);

        /// <summary>
        /// Initiate a remote stop of a charging station socket outlet.
        /// </summary>
        /// <param name="EVSEId">The unique identification of an EVSE.</param>
        /// <param name="SessionId">The unique identification for this charging session.</param>
        /// <param name="ProviderId">The unique identification of the e-mobility service provider.</param>
        /// <param name="EventTrackingId">An optional unique identification for tracking related events.</param>
        RemoteStopResult  RemoteStop (EVSE_Id             EVSEId,
                                      ChargingSession_Id  SessionId,
                                      EVSP_Id             ProviderId,
                                      EventTracking_Id    EventTrackingId  = null);

    }

}
