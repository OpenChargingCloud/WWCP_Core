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
using System.Collections.Generic;

#endregion

namespace org.GraphDefined.WWCP
{

    /// <summary>
    /// A list of charging pool identifications which are redirected
    /// from one roaming network to another roaming network.
    /// This might be usefull whenever some charging Pools
    /// shall be conntected to a testing/QA roaming network.
    /// </summary>
    public class RedirectedChargingPools
    {

        #region Data

        private readonly Dictionary<ChargingPool_Id, Tuple<RoamingNetwork_Id, RoamingNetwork_Id>> _RedirectedChargingPools;

        #endregion

        #region Constructor(s)

        private RedirectedChargingPools()
        {

            _RedirectedChargingPools = new Dictionary<ChargingPool_Id, Tuple<RoamingNetwork_Id, RoamingNetwork_Id>>();

        }

        #endregion


        #region Create()

        public static RedirectedChargingPools Create()
        {
            return new RedirectedChargingPools();
        }

        #endregion

        #region Add()

        public RedirectedChargingPools Add(ChargingPool_Id    ChargingPoolId,
                                           RoamingNetwork_Id  RoamingNetwork_From,
                                           RoamingNetwork_Id  RoamingNetwork_To)
        {

            if (!_RedirectedChargingPools.ContainsKey(ChargingPoolId))
                _RedirectedChargingPools.Add(ChargingPoolId, new Tuple<RoamingNetwork_Id, RoamingNetwork_Id>(RoamingNetwork_From, RoamingNetwork_To));

            return this;

        }

        #endregion

        #region Clear()

        public RedirectedChargingPools Clear()
        {

            _RedirectedChargingPools.Clear();

            return this;

        }

        #endregion


        public Boolean TryGet(ChargingPool_Id ChargingPoolId, out Tuple<RoamingNetwork_Id, RoamingNetwork_Id> Redirection)
        {
            return _RedirectedChargingPools.TryGetValue(ChargingPoolId, out Redirection);
        }

    }

}
