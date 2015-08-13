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
using System.Collections.Generic;

#endregion

namespace org.GraphDefined.WWCP
{

    /// <summary>
    /// A list of EVSE Ids which must be redirected from one
    /// roaming network to another roaming network.
    /// This might be usefull whenever some charging stations
    /// shall be conntected to a testing/QA roaming network.
    /// </summary>
    public class RedirectedEVSEList
    {

        #region Data

        private readonly Dictionary<EVSE_Id, Tuple<RoamingNetwork_Id, RoamingNetwork_Id>> _RedirectedEVSEList;

        #endregion

        #region Constructor(s)

        private RedirectedEVSEList()
        {

            _RedirectedEVSEList = new Dictionary<EVSE_Id, Tuple<RoamingNetwork_Id, RoamingNetwork_Id>>();

        }

        #endregion


        #region Create()

        public static RedirectedEVSEList Create()
        {
            return new RedirectedEVSEList();
        }

        #endregion

        #region Add()

        public RedirectedEVSEList Add(EVSE_Id            EVSEId,
                                      RoamingNetwork_Id  RoamingNetwork_From,
                                      RoamingNetwork_Id  RoamingNetwork_To)
        {

            if (!_RedirectedEVSEList.ContainsKey(EVSEId))
                _RedirectedEVSEList.Add(EVSEId, new Tuple<RoamingNetwork_Id, RoamingNetwork_Id>(RoamingNetwork_From, RoamingNetwork_To));

            return this;

        }

        #endregion

        #region Clear()

        public RedirectedEVSEList Clear()
        {

            _RedirectedEVSEList.Clear();

            return this;

        }

        #endregion


        public Boolean TryGet(EVSE_Id EVSEId, out Tuple<RoamingNetwork_Id, RoamingNetwork_Id> Redirection)
        {
            return _RedirectedEVSEList.TryGetValue(EVSEId, out Redirection);
        }

    }

}
