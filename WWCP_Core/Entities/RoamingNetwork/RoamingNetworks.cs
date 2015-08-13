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
using System.Collections.Concurrent;

using org.GraphDefined.Vanaheimr.Illias;
using org.GraphDefined.Vanaheimr.Illias.Votes;
using org.GraphDefined.Vanaheimr.Styx.Arrows;

using org.GraphDefined.WWCP.LocalService;

#endregion

namespace org.GraphDefined.WWCP
{

    /// <summary>
    /// The EV Roaming Networks collection simpifies the
    /// handling of multiple roadming networks.
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

        #region RoamingNetworkRemoval

        private readonly IVotingNotificator<RoamingNetworks, RoamingNetwork, Boolean> RoamingNetworkRemoval;

        /// <summary>
        /// Called whenever a roaming network will be or was removed.
        /// </summary>
        public IVotingSender<RoamingNetworks, RoamingNetwork, Boolean> OnRoamingNetworkRemoval
        {
            get
            {
                return RoamingNetworkRemoval;
            }
        }

        #endregion

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new EV Roaming Network collection.
        /// </summary>
        public RoamingNetworks()
        {

            this._RoamingNetworks = new ConcurrentDictionary<RoamingNetwork_Id, RoamingNetwork>();

            #region Init events

            this.RoamingNetworkAddition  = new VotingNotificator<RoamingNetworks, RoamingNetwork, Boolean>(() => new VetoVote(), true);
            this.RoamingNetworkRemoval   = new VotingNotificator<RoamingNetworks, RoamingNetwork, Boolean>(() => new VetoVote(), true);

            #endregion

        }

        #endregion


        #region CreateNewRoamingNetwork(RoamingNetworkId, AuthorizatorId = null, Description = null, Action = null)

        /// <summary>
        /// Create and register a new roaming network having the given
        /// unique roaming network identification.
        /// </summary>
        /// <param name="RoamingNetworkId">The unique identification of the new roaming network.</param>
        /// <param name="AuthorizatorId">The unique identification for the Auth service.</param>
        /// <param name="Description">A multilanguage description of the roaming network.</param>
        /// <param name="Action">An optional delegate to configure the new EVSE operator after its creation.</param>
        public RoamingNetwork CreateNewRoamingNetwork(RoamingNetwork_Id       RoamingNetworkId,
                                                      Authorizator_Id         AuthorizatorId  = null,
                                                      I18NString              Description     = null,
                                                      Action<RoamingNetwork>  Action          = null)
        {

            #region Initial checks

            if (RoamingNetworkId == null)
                throw new ArgumentNullException("RoamingNetworkId", "The given roaming network identification must not be null!");

            if (_RoamingNetworks.ContainsKey(RoamingNetworkId))
                throw new RoamingNetworkAlreadyExists(RoamingNetworkId);

            #endregion

            var _RoamingNetwork = new RoamingNetwork(RoamingNetworkId, AuthorizatorId) {
                                      Description = Description
                                  };

            Action.FailSafeInvoke(_RoamingNetwork);

            if (RoamingNetworkAddition.SendVoting(this, _RoamingNetwork))
            {
                if (_RoamingNetworks.TryAdd(RoamingNetworkId, _RoamingNetwork))
                {
                    RoamingNetworkAddition.SendNotification(this, _RoamingNetwork);
                    return _RoamingNetwork;
                }
            }

            throw new Exception();

        }

        #endregion

        #region GetRoamingNetwork(RoamingNetworkId)

        public RoamingNetwork GetRoamingNetwork(RoamingNetwork_Id RoamingNetworkId)
        {

            RoamingNetwork _RoamingNetwork = null;

            if (_RoamingNetworks.TryGetValue(RoamingNetworkId, out _RoamingNetwork))
                return _RoamingNetwork;

            return null;

        }

        #endregion

        #region TryGetRoamingNetwork(RoamingNetworkId, out RoamingNetwork)

        public Boolean TryGetRoamingNetwork(RoamingNetwork_Id RoamingNetworkId, out RoamingNetwork RoamingNetwork)
        {
            return _RoamingNetworks.TryGetValue(RoamingNetworkId, out RoamingNetwork);
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
