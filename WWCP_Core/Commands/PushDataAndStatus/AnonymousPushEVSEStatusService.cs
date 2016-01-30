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

    public class AnonymousPushEVSEStatusService : IPushDataAndStatus
    {

        public event OnEVSEDataPushDelegate     OnEVSEDataPush;
        public event OnEVSEDataPushedDelegate   OnEVSEDataPushed;
        public event OnEVSEStatusPushDelegate   OnEVSEStatusPush;
        public event OnEVSEStatusPushedDelegate OnEVSEStatusPushed;

        private readonly Action<EVSEStatusDiff> _EVSEStatusDiffDelegate;

        public AnonymousPushEVSEStatusService(Action<EVSEStatusDiff> EVSEStatusDiffDelegate)
        {

            this._EVSEStatusDiffDelegate = EVSEStatusDiffDelegate;

        }


        public Task PushEVSEStatus(EVSEStatusDiff EVSEStatusDiff, TimeSpan? QueryTimeout = default(TimeSpan?))
        {
            return Task.FromResult(new Acknowledgement(true));
        }

        public Task<Acknowledgement> PushEVSEStatus(EVSE EVSE, ActionType ActionType = ActionType.update, EVSEOperator_Id OperatorId = null, string OperatorName = null, TimeSpan? QueryTimeout = default(TimeSpan?))
        {

            this._EVSEStatusDiffDelegate(new EVSEStatusDiff(DateTime.Now, EVSE.Operator.Id, null, new KeyValuePair<EVSE_Id, EVSEStatusType>[] { new KeyValuePair<EVSE_Id, EVSEStatusType>(EVSE.Id, EVSE.Status.Value) }, null));

            return Task.FromResult(new Acknowledgement(true));

        }

        public Task<Acknowledgement> PushEVSEStatus(ILookup<EVSEOperator, EVSE> GroupedEVSEs, ActionType ActionType = ActionType.update, EVSEOperator_Id OperatorId = null, string OperatorName = null, TimeSpan? QueryTimeout = default(TimeSpan?))
        {
            return Task.FromResult(new Acknowledgement(true));
        }

        public Task<Acknowledgement> PushEVSEStatus(IEnumerable<EVSEOperator> EVSEOperators, ActionType ActionType = ActionType.update, EVSEOperator_Id OperatorId = null, string OperatorName = null, Func<EVSE, bool> IncludeEVSEs = null, TimeSpan? QueryTimeout = default(TimeSpan?))
        {
            return Task.FromResult(new Acknowledgement(true));
        }

        public Task<Acknowledgement> PushEVSEStatus(RoamingNetwork RoamingNetwork, ActionType ActionType = ActionType.update, EVSEOperator_Id OperatorId = null, string OperatorName = null, Func<EVSE, bool> IncludeEVSEs = null, TimeSpan? QueryTimeout = default(TimeSpan?))
        {
            return Task.FromResult(new Acknowledgement(true));
        }

        public Task<Acknowledgement> PushEVSEStatus(EVSEOperator EVSEOperator, ActionType ActionType = ActionType.update, EVSEOperator_Id OperatorId = null, string OperatorName = null, Func<EVSE, bool> IncludeEVSEs = null, TimeSpan? QueryTimeout = default(TimeSpan?))
        {
            return Task.FromResult(new Acknowledgement(true));
        }

        public Task<Acknowledgement> PushEVSEStatus(IEnumerable<ChargingStation> ChargingStations, ActionType ActionType = ActionType.update, EVSEOperator_Id OperatorId = null, string OperatorName = null, Func<EVSE, bool> IncludeEVSEs = null, TimeSpan? QueryTimeout = default(TimeSpan?))
        {
            return Task.FromResult(new Acknowledgement(true));
        }

        public Task<Acknowledgement> PushEVSEStatus(IEnumerable<ChargingPool> ChargingPools, ActionType ActionType = ActionType.update, EVSEOperator_Id OperatorId = null, string OperatorName = null, Func<EVSE, bool> IncludeEVSEs = null, TimeSpan? QueryTimeout = default(TimeSpan?))
        {
            return Task.FromResult(new Acknowledgement(true));
        }

        public Task<Acknowledgement> PushEVSEStatus(ChargingPool ChargingPool, ActionType ActionType = ActionType.update, EVSEOperator_Id OperatorId = null, string OperatorName = null, Func<EVSE, bool> IncludeEVSEs = null, TimeSpan? QueryTimeout = default(TimeSpan?))
        {
            return Task.FromResult(new Acknowledgement(true));
        }

        public Task<Acknowledgement> PushEVSEStatus(ChargingStation ChargingStation, ActionType ActionType = ActionType.update, EVSEOperator_Id OperatorId = null, string OperatorName = null, Func<EVSE, bool> IncludeEVSEs = null, TimeSpan? QueryTimeout = default(TimeSpan?))
        {
            return Task.FromResult(new Acknowledgement(true));
        }

        public Task<Acknowledgement> PushEVSEStatus(IEnumerable<EVSE> EVSEs, ActionType ActionType = ActionType.update, EVSEOperator_Id OperatorId = null, string OperatorName = null, Func<EVSE, bool> IncludeEVSEs = null, TimeSpan? QueryTimeout = default(TimeSpan?))
        {
            return Task.FromResult(new Acknowledgement(true));
        }

        public Task<Acknowledgement> PushEVSEData(ILookup<EVSEOperator, EVSE> GroupedEVSEs, ActionType ActionType = ActionType.fullLoad, EVSEOperator_Id OperatorId = null, string OperatorName = null, TimeSpan? QueryTimeout = default(TimeSpan?))
        {
            throw new NotImplementedException();
        }

        public Task<Acknowledgement> PushEVSEData(EVSE EVSE, ActionType ActionType = ActionType.insert, EVSEOperator_Id OperatorId = null, string OperatorName = null, TimeSpan? QueryTimeout = default(TimeSpan?))
        {
            throw new NotImplementedException();
        }

        public Task<Acknowledgement> PushEVSEData(IEnumerable<EVSE> EVSEs, ActionType ActionType = ActionType.fullLoad, EVSEOperator_Id OperatorId = null, string OperatorName = null, Func<EVSE, bool> IncludeEVSEs = null, TimeSpan? QueryTimeout = default(TimeSpan?))
        {
            throw new NotImplementedException();
        }

        public Task<Acknowledgement> PushEVSEData(ChargingStation ChargingStation, ActionType ActionType = ActionType.fullLoad, EVSEOperator_Id OperatorId = null, string OperatorName = null, Func<EVSE, bool> IncludeEVSEs = null, TimeSpan? QueryTimeout = default(TimeSpan?))
        {
            throw new NotImplementedException();
        }

        public Task<Acknowledgement> PushEVSEData(IEnumerable<ChargingStation> ChargingStations, ActionType ActionType = ActionType.fullLoad, EVSEOperator_Id OperatorId = null, string OperatorName = null, Func<EVSE, bool> IncludeEVSEs = null, TimeSpan? QueryTimeout = default(TimeSpan?))
        {
            throw new NotImplementedException();
        }

        public Task<Acknowledgement> PushEVSEData(ChargingPool ChargingPool, ActionType ActionType = ActionType.fullLoad, EVSEOperator_Id OperatorId = null, string OperatorName = null, Func<EVSE, bool> IncludeEVSEs = null, TimeSpan? QueryTimeout = default(TimeSpan?))
        {
            throw new NotImplementedException();
        }

        public Task<Acknowledgement> PushEVSEData(IEnumerable<ChargingPool> ChargingPools, ActionType ActionType = ActionType.fullLoad, EVSEOperator_Id OperatorId = null, string OperatorName = null, Func<EVSE, bool> IncludeEVSEs = null, TimeSpan? QueryTimeout = default(TimeSpan?))
        {
            throw new NotImplementedException();
        }

        public Task<Acknowledgement> PushEVSEData(EVSEOperator EVSEOperator, ActionType ActionType = ActionType.fullLoad, EVSEOperator_Id OperatorId = null, string OperatorName = null, Func<EVSE, bool> IncludeEVSEs = null, TimeSpan? QueryTimeout = default(TimeSpan?))
        {
            throw new NotImplementedException();
        }

        public Task<Acknowledgement> PushEVSEData(IEnumerable<EVSEOperator> EVSEOperators, ActionType ActionType = ActionType.fullLoad, EVSEOperator_Id OperatorId = null, string OperatorName = null, Func<EVSE, bool> IncludeEVSEs = null, TimeSpan? QueryTimeout = default(TimeSpan?))
        {
            throw new NotImplementedException();
        }

        public Task<Acknowledgement> PushEVSEData(RoamingNetwork RoamingNetwork, ActionType ActionType = ActionType.fullLoad, EVSEOperator_Id OperatorId = null, string OperatorName = null, Func<EVSE, bool> IncludeEVSEs = null, TimeSpan? QueryTimeout = default(TimeSpan?))
        {
            throw new NotImplementedException();
        }
    }


}
