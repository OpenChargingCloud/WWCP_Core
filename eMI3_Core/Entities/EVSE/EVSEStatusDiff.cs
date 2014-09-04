/*
 * Copyright (c) 2014 Achim Friedland <achim.friedland@graphdefined.com>
 * This file is part of eMI3 Core <http://www.github.com/GraphDefined/eMI3>
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

using System.Collections.Generic;

#endregion

namespace com.graphdefined.eMI3
{

    public class EVSEStatusDiff
    {

        public List<KeyValuePair<EVSE_Id, EVSEStatusType>> NewEVSEStates        { get; private set; }
        public List<KeyValuePair<EVSE_Id, EVSEStatusType>> ChangedEVSEStates    { get; private set; }
        public List<EVSE_Id>                               RemovedEVSEIds       { get; private set; }

        public EVSEStatusDiff()
        {
            NewEVSEStates      = new List<KeyValuePair<EVSE_Id, EVSEStatusType>>();
            ChangedEVSEStates  = new List<KeyValuePair<EVSE_Id, EVSEStatusType>>();
            RemovedEVSEIds     = new List<EVSE_Id>();
        }

        public EVSEStatusDiff(List<KeyValuePair<EVSE_Id, EVSEStatusType>> _NewEVSEStates,
                              List<KeyValuePair<EVSE_Id, EVSEStatusType>> _ChangedEVSEStates,
                              List<EVSE_Id>                               _RemovedEVSEIds)
        {

            NewEVSEStates      = _NewEVSEStates;
            ChangedEVSEStates  = _ChangedEVSEStates;
            RemovedEVSEIds     = _RemovedEVSEIds;

        }

    }

}
