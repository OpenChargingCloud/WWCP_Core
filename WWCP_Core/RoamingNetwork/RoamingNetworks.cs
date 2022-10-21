/*
 * Copyright (c) 2014-2022 GraphDefined GmbH <achim.friedland@graphdefined.com>
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

using System.Collections.Concurrent;

using org.GraphDefined.Vanaheimr.Illias;
using org.GraphDefined.Vanaheimr.Illias.Votes;
using org.GraphDefined.Vanaheimr.Styx.Arrows;

using cloud.charging.open.protocols.WWCP.Networking;

#endregion

namespace cloud.charging.open.protocols.WWCP
{

    /// <summary>
    /// A collection of roaming networks, which simpifies the handling of multiple roadming networks.
    /// </summary>
    public class RoamingNetworks : IEnumerable<RoamingNetwork>
    {

        #region Data

        private readonly ConcurrentDictionary<RoamingNetwork_Id, RoamingNetwork> roamingNetworks;

        #endregion

        #region Events

        #region OnRoamingNetworkAddition

        private readonly IVotingNotificator<RoamingNetworks, RoamingNetwork, Boolean> roamingNetworkAddition;

        /// <summary>
        /// Called whenever a roaming network will be or was added.
        /// </summary>
        public IVotingSender<RoamingNetworks, RoamingNetwork, Boolean> OnRoamingNetworkAddition
            => roamingNetworkAddition;

        #endregion

        #region OnRoamingNetworkRemoval

        private readonly IVotingNotificator<RoamingNetworks, RoamingNetwork, Boolean> roamingNetworkRemoval;

        /// <summary>
        /// Called whenever a roaming network will be or was removed.
        /// </summary>
        public IVotingSender<RoamingNetworks, RoamingNetwork, Boolean> OnRoamingNetworkRemoval
            => roamingNetworkRemoval;

        #endregion

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new roaming network collection.
        /// </summary>
        /// <param name="RoamingNetworks">Initial roaming networks to be added.</param>
        public RoamingNetworks(params RoamingNetwork[] RoamingNetworks)
        {

            this.roamingNetworks         = new ConcurrentDictionary<RoamingNetwork_Id, RoamingNetwork>();

            this.roamingNetworkAddition  = new VotingNotificator<RoamingNetworks, RoamingNetwork, Boolean>(() => new VetoVote(), true);
            this.roamingNetworkRemoval   = new VotingNotificator<RoamingNetworks, RoamingNetwork, Boolean>(() => new VetoVote(), true);

            if (RoamingNetworks is not null && RoamingNetworks.Any())
                AddRoamingNetworks(RoamingNetworks);

        }

        #endregion


        #region CreateNewRoamingNetwork(RoamingNetworkId, AuthorizatorId = null, Description = null, Configurator = null)

        /// <summary>
        /// Create and register a new roaming network having the given
        /// unique roaming network identification.
        /// </summary>
        /// <param name="RoamingNetworkId">The unique identification of the new roaming network.</param>
        /// <param name="Name">The multi-language name of the roaming network.</param>
        /// <param name="Description">A multilanguage description of the roaming networks object.</param>
        /// <param name="Configurator">An optional delegate to configure the new roaming network after its creation.</param>
        /// <param name="AdminStatus">The initial admin status of the roaming network.</param>
        /// <param name="Status">The initial status of the roaming network.</param>
        /// <param name="MaxAdminStatusListSize">The maximum number of entries in the admin status history.</param>
        /// <param name="MaxStatusListSize">The maximum number of entries in the status history.</param>
        /// <param name="ChargingStationSignatureGenerator">A delegate to sign a charging station.</param>
        /// <param name="ChargingPoolSignatureGenerator">A delegate to sign a charging pool.</param>
        /// <param name="ChargingStationOperatorSignatureGenerator">A delegate to sign a charging station operator.</param>
        public RoamingNetwork CreateNewRoamingNetwork(RoamingNetwork_Id                          RoamingNetworkId,
                                                      I18NString                                 Name,
                                                      I18NString?                                Description                                 = null,
                                                      Action<RoamingNetwork>?                    Configurator                                = null,
                                                      RoamingNetworkAdminStatusTypes?            AdminStatus                                 = null,
                                                      RoamingNetworkStatusTypes?                 Status                                      = null,
                                                      UInt16?                                    MaxAdminStatusListSize                      = null,
                                                      UInt16?                                    MaxStatusListSize                           = null,

                                                      ChargingStationSignatureDelegate?          ChargingStationSignatureGenerator           = null,
                                                      ChargingPoolSignatureDelegate?             ChargingPoolSignatureGenerator              = null,
                                                      ChargingStationOperatorSignatureDelegate?  ChargingStationOperatorSignatureGenerator   = null,

                                                      IEnumerable<RoamingNetworkInfo>?           RoamingNetworkInfos                         = null,
                                                      Boolean                                    DisableNetworkSync                          = false,
                                                      String?                                    LoggingPath                                 = null)

        {

            #region Initial checks

            if (roamingNetworks.ContainsKey(RoamingNetworkId))
                throw new RoamingNetworkAlreadyExists(RoamingNetworkId);

            #endregion

            var roamingNetwork = new RoamingNetwork(RoamingNetworkId,
                                                    Name,
                                                    Description,
                                                    AdminStatus,
                                                    Status,
                                                    MaxAdminStatusListSize,
                                                    MaxStatusListSize,

                                                    ChargingStationSignatureGenerator,
                                                    ChargingPoolSignatureGenerator,
                                                    ChargingStationOperatorSignatureGenerator,

                                                    RoamingNetworkInfos,
                                                    DisableNetworkSync,
                                                    LoggingPath);

            Configurator?.Invoke(roamingNetwork);

            if (roamingNetworkAddition.SendVoting(this, roamingNetwork) &&
                roamingNetworks.TryAdd(RoamingNetworkId, roamingNetwork))
            {
                roamingNetworkAddition.SendNotification(this, roamingNetwork);
                return roamingNetwork;
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

            if (RoamingNetwork is null)
                throw new ArgumentNullException(nameof(RoamingNetwork),  "The given roaming network must not be null!");

            if (roamingNetworks.ContainsKey(RoamingNetwork.Id))
                throw new RoamingNetworkAlreadyExists(RoamingNetwork.Id);

            #endregion

            if (roamingNetworkAddition.SendVoting(this, RoamingNetwork) &&
                roamingNetworks.TryAdd(RoamingNetwork.Id, RoamingNetwork))
            {
                roamingNetworkAddition.SendNotification(this, RoamingNetwork);
                return RoamingNetwork;
            }

            throw new Exception();

        }

        #endregion

        #region AddRoamingNetwork(RoamingNetworks)

        /// <summary>
        /// Register the given roaming networks.
        /// </summary>
        /// <param name="RoamingNetworks">An enumeration of roaming networks to add.</param>
        public void AddRoamingNetworks(IEnumerable<RoamingNetwork>  RoamingNetworks)
        {

            #region Initial checks

            if (RoamingNetworks is null)
                throw new ArgumentNullException(nameof(RoamingNetworks),  "The given enumeration of roaming networks must not be null!");

            #endregion

            foreach (var roamingNetwork in RoamingNetworks)
                AddRoamingNetwork(roamingNetwork);

        }

        #endregion

        #region GetRoamingNetwork(RoamingNetworkId)

        /// <summary>
        /// Return the roaming network identified by the given unique roaming network identification.
        /// </summary>
        /// <param name="RoamingNetworkId">The unique identification of a roaming network.</param>
        public RoamingNetwork? GetRoamingNetwork(RoamingNetwork_Id RoamingNetworkId)
        {

            if (roamingNetworks.TryGetValue(RoamingNetworkId, out var roamingNetwork))
                return roamingNetwork;

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
        public Boolean TryGetRoamingNetwork(RoamingNetwork_Id RoamingNetworkId, out RoamingNetwork? RoamingNetwork)

            => roamingNetworks.TryGetValue(RoamingNetworkId, out RoamingNetwork);

        #endregion

        #region RemoveRoamingNetwork(RoamingNetworkId)

        /// <summary>
        /// Try to return the roaming network identified by the given unique roaming network identification.
        /// </summary>
        /// <param name="RoamingNetworkId">The unique identification of a roaming network.</param>
        /// <returns>True, when the roaming network was found; false else.</returns>
        public RoamingNetwork? RemoveRoamingNetwork(RoamingNetwork_Id RoamingNetworkId)
        {

            if (roamingNetworks.TryRemove(RoamingNetworkId, out var roamingNetwork))
                return roamingNetwork;

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
        public Boolean RemoveRoamingNetwork(RoamingNetwork_Id RoamingNetworkId, out RoamingNetwork? RoamingNetwork)

            => roamingNetworks.TryRemove(RoamingNetworkId, out RoamingNetwork);

        #endregion


        #region IEnumerable<RoamingNetwork> Members

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()

            => roamingNetworks.Values.GetEnumerator();

        public IEnumerator<RoamingNetwork> GetEnumerator()

            => roamingNetworks.Values.GetEnumerator();

        #endregion

        #region (override) ToString()

        /// <summary>
        /// Return a text representation of this object.
        /// </summary>
        public override String ToString()

            => roamingNetworks.Count + " registered roaming networks";

        #endregion

    }

}
