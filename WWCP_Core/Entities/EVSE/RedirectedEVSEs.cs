﻿/*
 * Copyright (c) 2014-2025 GraphDefined GmbH <achim.friedland@graphdefined.com>
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
using System.Collections.Generic;

#endregion

namespace cloud.charging.open.protocols.WWCP
{

    /// <summary>
    /// A list of EVSE identifications which are redirected
    /// from one roaming network to another roaming network.
    /// This might be usefull whenever some charging stations
    /// shall be conntected to a testing/QA roaming network.
    /// </summary>
    public class RedirectedEVSEs
    {

        #region Data

        private          Object                                                           Lock = new Object();
        private readonly Dictionary<EVSE_Id, Tuple<RoamingNetwork_Id, RoamingNetwork_Id>> _RedirectedEVSEs;

        #endregion

        #region Constructor(s)

        private RedirectedEVSEs()
        {

            _RedirectedEVSEs = new Dictionary<EVSE_Id, Tuple<RoamingNetwork_Id, RoamingNetwork_Id>>();

        }

        #endregion


        #region Create()

        public static RedirectedEVSEs Create()
        {
            return new RedirectedEVSEs();
        }

        #endregion

        #region Add()

        public RedirectedEVSEs Add(EVSE_Id            EVSEId,
                                   RoamingNetwork_Id  RoamingNetwork_From,
                                   RoamingNetwork_Id  RoamingNetwork_To)
        {
            lock (Lock)
            {

                if (!_RedirectedEVSEs.ContainsKey(EVSEId))
                    _RedirectedEVSEs.Add(EVSEId, new Tuple<RoamingNetwork_Id, RoamingNetwork_Id>(RoamingNetwork_From, RoamingNetwork_To));

                return this;

            }
        }

        #endregion

        #region Clear()

        public RedirectedEVSEs Clear()
        {
            lock (Lock)
            {

                _RedirectedEVSEs.Clear();

                return this;

            }
        }

        #endregion


        public Boolean TryGet(EVSE_Id EVSEId, out Tuple<RoamingNetwork_Id, RoamingNetwork_Id> Redirection)
        {
            return _RedirectedEVSEs.TryGetValue(EVSEId, out Redirection);
        }

    }

}
