﻿/*
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

using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;

using org.GraphDefined.Vanaheimr.Illias;

#endregion

namespace org.GraphDefined.WWCP
{

    public class AnonymousPushEVSEStatusService : IPushStatus
    {

        public event OnPushEVSEStatusRequestDelegate   OnPushEVSEStatusRequest;
        public event OnPushEVSEStatusResponseDelegate  OnPushEVSEStatusResponse;

        private readonly Action<EVSEStatusDiff> _EVSEStatusDiffDelegate;

        public AnonymousPushEVSEStatusService(Action<EVSEStatusDiff> EVSEStatusDiffDelegate)
        {

            this._EVSEStatusDiffDelegate = EVSEStatusDiffDelegate;

        }

        public async Task<Acknowledgement> PushEVSEStatus(ILookup<ChargingStationOperator, EVSEStatus> GroupedEVSEStatus, ActionType ActionType = ActionType.update, DateTime? Timestamp = default(DateTime?), CancellationToken? CancellationToken = default(CancellationToken?), EventTracking_Id EventTrackingId = null, TimeSpan? RequestTimeout = default(TimeSpan?))
        {
            return new Acknowledgement(ResultType.True);
        }

        public async Task<Acknowledgement> PushEVSEStatus(EVSEStatus EVSEStatus, ActionType ActionType = ActionType.update, DateTime? Timestamp = default(DateTime?), CancellationToken? CancellationToken = default(CancellationToken?), EventTracking_Id EventTrackingId = null, TimeSpan? RequestTimeout = default(TimeSpan?))
        {
            return new Acknowledgement(ResultType.True);
        }

        public async Task<Acknowledgement> PushEVSEStatus(IEnumerable<EVSEStatus> EVSEStatus, ActionType ActionType = ActionType.update, DateTime? Timestamp = default(DateTime?), CancellationToken? CancellationToken = default(CancellationToken?), EventTracking_Id EventTrackingId = null, TimeSpan? RequestTimeout = default(TimeSpan?))
        {
            return new Acknowledgement(ResultType.True);
        }

        public async Task<Acknowledgement> PushEVSEStatus(EVSE EVSE, ActionType ActionType = ActionType.update, IncludeEVSEDelegate IncludeEVSEs = null, DateTime? Timestamp = default(DateTime?), CancellationToken? CancellationToken = default(CancellationToken?), EventTracking_Id EventTrackingId = null, TimeSpan? RequestTimeout = default(TimeSpan?))
        {
            return new Acknowledgement(ResultType.True);
        }

        public async Task<Acknowledgement> PushEVSEStatus(IEnumerable<EVSE> EVSEs, ActionType ActionType = ActionType.update, IncludeEVSEDelegate IncludeEVSEs = null, DateTime? Timestamp = default(DateTime?), CancellationToken? CancellationToken = default(CancellationToken?), EventTracking_Id EventTrackingId = null, TimeSpan? RequestTimeout = default(TimeSpan?))
        {
            return new Acknowledgement(ResultType.True);
        }

        public async Task<Acknowledgement> PushEVSEStatus(ChargingStation ChargingStation, ActionType ActionType = ActionType.update, IncludeEVSEDelegate IncludeEVSEs = null, DateTime? Timestamp = default(DateTime?), CancellationToken? CancellationToken = default(CancellationToken?), EventTracking_Id EventTrackingId = null, TimeSpan? RequestTimeout = default(TimeSpan?))
        {
            return new Acknowledgement(ResultType.True);
        }

        public async Task<Acknowledgement> PushEVSEStatus(IEnumerable<ChargingStation> ChargingStations, ActionType ActionType = ActionType.update, IncludeEVSEDelegate IncludeEVSEs = null, DateTime? Timestamp = default(DateTime?), CancellationToken? CancellationToken = default(CancellationToken?), EventTracking_Id EventTrackingId = null, TimeSpan? RequestTimeout = default(TimeSpan?))
        {
            return new Acknowledgement(ResultType.True);
        }

        public async Task<Acknowledgement> PushEVSEStatus(ChargingPool ChargingPool, ActionType ActionType = ActionType.update, IncludeEVSEDelegate IncludeEVSEs = null, DateTime? Timestamp = default(DateTime?), CancellationToken? CancellationToken = default(CancellationToken?), EventTracking_Id EventTrackingId = null, TimeSpan? RequestTimeout = default(TimeSpan?))
        {
            return new Acknowledgement(ResultType.True);
        }

        public async Task<Acknowledgement> PushEVSEStatus(IEnumerable<ChargingPool> ChargingPools, ActionType ActionType = ActionType.update, IncludeEVSEDelegate IncludeEVSEs = null, DateTime? Timestamp = default(DateTime?), CancellationToken? CancellationToken = default(CancellationToken?), EventTracking_Id EventTrackingId = null, TimeSpan? RequestTimeout = default(TimeSpan?))
        {
            return new Acknowledgement(ResultType.True);
        }

        public async Task<Acknowledgement> PushEVSEStatus(ChargingStationOperator EVSEOperator, ActionType ActionType = ActionType.update, IncludeEVSEDelegate IncludeEVSEs = null, DateTime? Timestamp = default(DateTime?), CancellationToken? CancellationToken = default(CancellationToken?), EventTracking_Id EventTrackingId = null, TimeSpan? RequestTimeout = default(TimeSpan?))
        {
            return new Acknowledgement(ResultType.True);
        }

        public async Task<Acknowledgement> PushEVSEStatus(IEnumerable<ChargingStationOperator> EVSEOperators, ActionType ActionType = ActionType.update, IncludeEVSEDelegate IncludeEVSEs = null, DateTime? Timestamp = default(DateTime?), CancellationToken? CancellationToken = default(CancellationToken?), EventTracking_Id EventTrackingId = null, TimeSpan? RequestTimeout = default(TimeSpan?))
        {
            return new Acknowledgement(ResultType.True);
        }

        public async Task<Acknowledgement> PushEVSEStatus(RoamingNetwork RoamingNetwork, ActionType ActionType = ActionType.update, IncludeEVSEDelegate IncludeEVSEs = null, DateTime? Timestamp = default(DateTime?), CancellationToken? CancellationToken = default(CancellationToken?), EventTracking_Id EventTrackingId = null, TimeSpan? RequestTimeout = default(TimeSpan?))
        {
            return new Acknowledgement(ResultType.True);
        }

        public async Task PushEVSEStatus(EVSEStatusDiff EVSEStatusDiff, DateTime? Timestamp = default(DateTime?), CancellationToken? CancellationToken = default(CancellationToken?), EventTracking_Id EventTrackingId = null, TimeSpan? RequestTimeout = default(TimeSpan?))
        { }

    }

}
