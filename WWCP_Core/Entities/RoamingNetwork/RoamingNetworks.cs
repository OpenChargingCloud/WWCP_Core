/*
 * Copyright (c) 2014-2016 GraphDefined GmbH <achim.friedland@graphdefined.com>
 * This file is part of WWCP Net <https://github.com/GraphDefined/WWCP_Net>
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

#endregion

namespace org.GraphDefined.WWCP
{

    /// <summary>
    /// The EV roaming networks collection simpifies the
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
        /// <param name="RoamingNetwork">A initial roaming network to add after initialization.</param>
        public RoamingNetworks(RoamingNetwork RoamingNetwork = null)
        {

            _RoamingNetworks  = new ConcurrentDictionary<RoamingNetwork_Id, RoamingNetwork>();

            #region Init events

            this.RoamingNetworkAddition  = new VotingNotificator<RoamingNetworks, RoamingNetwork, Boolean>(() => new VetoVote(), true);
            this.RoamingNetworkRemoval   = new VotingNotificator<RoamingNetworks, RoamingNetwork, Boolean>(() => new VetoVote(), true);

            #endregion

            if (RoamingNetwork != null)
                AddRoamingNetwork(RoamingNetwork);

        }

        #endregion


        #region CreateNewRoamingNetwork(RoamingNetworkId, AuthorizatorId = null, Description = null, Configurator = null)

        /// <summary>
        /// Create and register a new roaming network having the given
        /// unique roaming network identification.
        /// </summary>
        /// <param name="RoamingNetworkId">The unique identification of the new roaming network.</param>
        /// <param name="AuthorizatorId">The unique identification for the Auth service.</param>
        /// <param name="Description">A multilanguage description of the roaming networks object.</param>
        /// <param name="Configurator">An optional delegate to configure the new roaming network after its creation.</param>
        /// <param name="AdminStatus">The initial admin status of the roaming network.</param>
        /// <param name="Status">The initial status of the roaming network.</param>
        /// <param name="MaxAdminStatusListSize">The maximum number of entries in the admin status history.</param>
        /// <param name="MaxStatusListSize">The maximum number of entries in the status history.</param>
        /// <param name="ChargingStationSignatureGenerator">A delegate to sign a charging station.</param>
        /// <param name="ChargingPoolSignatureGenerator">A delegate to sign a charging pool.</param>
        /// <param name="ChargingStationOperatorSignatureGenerator">A delegate to sign a charging station operator.</param>
        public RoamingNetwork CreateNewRoamingNetwork(RoamingNetwork_Id                         RoamingNetworkId,
                                                      I18NString                                Name,
                                                      //String                                    AuthorizatorId                             = null,
                                                      I18NString                                Description                                = null,
                                                      Action<RoamingNetwork>                    Configurator                               = null,
                                                      RoamingNetworkAdminStatusType             AdminStatus                                = RoamingNetworkAdminStatusType.Operational,
                                                      RoamingNetworkStatusType                  Status                                     = RoamingNetworkStatusType.Available,
                                                      UInt16                                    MaxAdminStatusListSize                     = RoamingNetwork.DefaultMaxAdminStatusListSize,
                                                      UInt16                                    MaxStatusListSize                          = RoamingNetwork.DefaultMaxStatusListSize,
                                                      ChargingStationSignatureDelegate          ChargingStationSignatureGenerator          = null,
                                                      ChargingPoolSignatureDelegate             ChargingPoolSignatureGenerator             = null,
                                                      ChargingStationOperatorSignatureDelegate  ChargingStationOperatorSignatureGenerator  = null)

        {

            #region Initial checks

            if (RoamingNetworkId == null)
                throw new ArgumentNullException(nameof(RoamingNetworkId),  "The given roaming network identification must not be null!");

            if (_RoamingNetworks.ContainsKey(RoamingNetworkId))
                throw new RoamingNetworkAlreadyExists(RoamingNetworkId);

            #endregion

            var _RoamingNetwork = new RoamingNetwork(RoamingNetworkId,
                                                     Name,
                                                     Description,
                                                     //AuthorizatorId,
                                                     AdminStatus,
                                                     Status,
                                                     MaxAdminStatusListSize,
                                                     MaxStatusListSize,
                                                     ChargingStationSignatureGenerator,
                                                     ChargingPoolSignatureGenerator,
                                                     ChargingStationOperatorSignatureGenerator);

            Configurator?.Invoke(_RoamingNetwork);

            if (RoamingNetworkAddition.SendVoting(this, _RoamingNetwork) &&
                _RoamingNetworks.TryAdd(RoamingNetworkId, _RoamingNetwork))
            {
                RoamingNetworkAddition.SendNotification(this, _RoamingNetwork);
                return _RoamingNetwork;
            }

            throw new Exception();

        }

        #endregion

        #region AddRoamingNetwork(RoamingNetwork)

        /// <summary>
        /// Register the given roaming network.
        /// </summary>
        /// <param name="RoamingNetwork">The roaming network to add.</param>
        public RoamingNetwork AddRoamingNetwork(RoamingNetwork  RoamingNetwork)
        {

            #region Initial checks

            if (RoamingNetwork == null)
                throw new ArgumentNullException(nameof(RoamingNetwork),  "The given roaming network must not be null!");

            if (_RoamingNetworks.ContainsKey(RoamingNetwork.Id))
                throw new RoamingNetworkAlreadyExists(RoamingNetwork.Id);

            #endregion

            if (RoamingNetworkAddition.SendVoting(this, RoamingNetwork) &&
                _RoamingNetworks.TryAdd(RoamingNetwork.Id, RoamingNetwork))
            {
                RoamingNetworkAddition.SendNotification(this, RoamingNetwork);
                return RoamingNetwork;
            }

            throw new Exception();

        }

        #endregion

        #region GetRoamingNetwork(RoamingNetworkId)

        /// <summary>
        /// Return the roaming network identified by the given unique roaming network identification.
        /// </summary>
        /// <param name="RoamingNetworkId">The unique identification of a roaming network.</param>
        public RoamingNetwork GetRoamingNetwork(RoamingNetwork_Id RoamingNetworkId)
        {

            RoamingNetwork _RoamingNetwork = null;

            if (_RoamingNetworks.TryGetValue(RoamingNetworkId, out _RoamingNetwork))
                return _RoamingNetwork;

            return null;

        }

        #endregion

        #region TryGetRoamingNetwork(RoamingNetworkId, out RoamingNetwork)

        /// <summary>
        /// Try to return the roaming network identified by the given unique roaming network identification.
        /// </summary>
        /// <param name="RoamingNetworkId">The unique identification of a roaming network.</param>
        /// <param name="RoamingNetwork">The roaming network.</param>
        /// <returns>True, when the roaming network was found; false else.</returns>
        public Boolean TryGetRoamingNetwork(RoamingNetwork_Id RoamingNetworkId, out RoamingNetwork RoamingNetwork)

            => _RoamingNetworks.TryGetValue(RoamingNetworkId, out RoamingNetwork);

        #endregion

        #region RemoveRoamingNetwork(RoamingNetworkId)

        /// <summary>
        /// Try to return the roaming network identified by the given unique roaming network identification.
        /// </summary>
        /// <param name="RoamingNetworkId">The unique identification of a roaming network.</param>
        /// <returns>True, when the roaming network was found; false else.</returns>
        public RoamingNetwork RemoveRoamingNetwork(RoamingNetwork_Id RoamingNetworkId)
        {

            RoamingNetwork _RoamingNetwork;

            if (_RoamingNetworks.TryRemove(RoamingNetworkId, out _RoamingNetwork))
                return _RoamingNetwork;

            return null;

        }

        #endregion

        #region RemoveRoamingNetwork(RoamingNetworkId, out RoamingNetwork)

        /// <summary>
        /// Try to return the roaming network identified by the given unique roaming network identification.
        /// </summary>
        /// <param name="RoamingNetworkId">The unique identification of a roaming network.</param>
        /// <param name="RoamingNetwork">The roaming network.</param>
        /// <returns>True, when the roaming network was found; false else.</returns>
        public Boolean RemoveRoamingNetwork(RoamingNetwork_Id   RoamingNetworkId,
                                            out RoamingNetwork  RoamingNetwork)

            => _RoamingNetworks.TryRemove(RoamingNetworkId, out RoamingNetwork);

        #endregion


        #region IEnumerable<RoamingNetwork> Members

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()

            => _RoamingNetworks.Values.GetEnumerator();

        public IEnumerator<RoamingNetwork> GetEnumerator()

            => _RoamingNetworks.Values.GetEnumerator();

        #endregion

        #region (override) ToString()

        /// <summary>
        /// Return a string representation of this object.
        /// </summary>
        public override String ToString()

            => _RoamingNetworks.Count + " roaming networks registered";

        #endregion

    }

}
