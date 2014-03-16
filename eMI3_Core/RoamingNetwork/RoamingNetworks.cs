/*
 * Copyright (c) 2013-2014 Achim Friedland <achim.friedland@belectric.com>
 * This file is part of eMI3 Core <http://www.github.com/eMI3/Core>
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
using System.Collections.Concurrent;

using eu.Vanaheimr.Illias.Commons;
using eu.Vanaheimr.Illias.Commons.Votes;
using eu.Vanaheimr.Styx.Arrows;

using org.emi3group.LocalService;

#endregion

namespace org.emi3group
{

    /// <summary>
    /// A roaming network is the collection of all entities to provide electric vehicle charging.
    /// </summary>
    public class RoamingNetworks : IEnumerable<RoamingNetwork>
    {

        #region Data

        private readonly ConcurrentDictionary<RoamingNetwork_Id, RoamingNetwork> _RoamingNetworks;

        #endregion

        #region Events

        #region RoamingNetworkAddition

        private readonly IVotingNotificator<RoamingNetworks, RoamingNetwork, Boolean> RoamingNetworkAddition;

        /// <summary>
        /// Called whenever a roaming network will be or was added.
        /// </summary>
        public IVotingSender<RoamingNetworks, RoamingNetwork, Boolean> OnRoamingNetworkAddition
        {
            get
            {
                return RoamingNetworkAddition;
            }
        }

        #endregion

        #endregion

        #region Constructor(s)

        public RoamingNetworks()
        {
            this._RoamingNetworks        = new ConcurrentDictionary<RoamingNetwork_Id, RoamingNetwork>();
            this.RoamingNetworkAddition  = new VotingNotificator<RoamingNetworks, RoamingNetwork, Boolean>(() => new VetoVote(), true);
        }

        #endregion


        #region CreateNewRoamingNetwork(RoamingNetwork_Id, Description, Action = null)

        /// <summary>
        /// Create and register a new roaming network having the given
        /// unique roaming network identification.
        /// </summary>
        /// <param name="RoamingNetwork_Id">The unique identification of the new roaming network.</param>
        /// <param name="Description">A multilanguage description of the roaming network.</param>
        /// <param name="Action">An optional delegate to configure the new EVSE operator after its creation.</param>
        public RoamingNetwork CreateNewRoamingNetwork(RoamingNetwork_Id       RoamingNetwork_Id,
                                                      I8NString               Description,
                                                      Action<RoamingNetwork>  Action = null)
        {

            #region Initial checks

            if (RoamingNetwork_Id == null)
                throw new ArgumentNullException("RoamingNetwork_Id", "The given roaming network identification must not be null!");

            if (_RoamingNetworks.ContainsKey(RoamingNetwork_Id))
                throw new RoamingNetworkAlreadyExists(RoamingNetwork_Id);

            #endregion

            var _RoamingNetwork = new RoamingNetwork(RoamingNetwork_Id) {
                                      Description = Description
                                  };

            Action.FailSafeInvoke(_RoamingNetwork);

            if (RoamingNetworkAddition.SendVoting(this, _RoamingNetwork))
            {
                if (_RoamingNetworks.TryAdd(RoamingNetwork_Id, _RoamingNetwork))
                {
                    RoamingNetworkAddition.SendNotification(this, _RoamingNetwork);
                    return _RoamingNetwork;
                }
            }

            throw new Exception();

        }

        #endregion

        #region GetRoamingNetwork(RoamingNetwork_Id)

        public RoamingNetwork GetRoamingNetwork(RoamingNetwork_Id RoamingNetwork_Id)
        {

            if (_RoamingNetworks.ContainsKey(RoamingNetwork_Id))
                return _RoamingNetworks[RoamingNetwork_Id];

            return null;

        }

        #endregion

        #region TryGetRoamingNetwork(RoamingNetwork_Id, out RoamingNetwork)

        public Boolean TryGetRoamingNetwork(RoamingNetwork_Id RoamingNetwork_Id, out RoamingNetwork RoamingNetwork)
        {
            return _RoamingNetworks.TryGetValue(RoamingNetwork_Id, out RoamingNetwork);
        }

        #endregion


        #region IEnumerable<RoamingNetwork> Members

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return _RoamingNetworks.Values.GetEnumerator();
        }

        public IEnumerator<RoamingNetwork> GetEnumerator()
        {
            return _RoamingNetworks.Values.GetEnumerator();
        }

        #endregion

        #region ToString()

        /// <summary>
        /// Get a string representation of this object.
        /// </summary>
        public override String ToString()
        {
            return _RoamingNetworks.Count + " roaming networks registered";
        }

        #endregion

    }

}
