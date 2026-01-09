/*
 * Copyright (c) 2014-2026 GraphDefined GmbH <achim.friedland@graphdefined.com>
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
using System.Diagnostics.CodeAnalysis;

using org.GraphDefined.Vanaheimr.Illias;
using org.GraphDefined.Vanaheimr.Illias.Votes;
using org.GraphDefined.Vanaheimr.Styx.Arrows;

using cloud.charging.open.protocols.WWCP.Networking;

#endregion

namespace cloud.charging.open.protocols.WWCP
{

    public static class WWCPCoreExtensions
    {

        #region CreateNewRoamingNetwork(RoamingNetworkId, AuthorizatorId = null, Description = null, Configurator = null)

        /// <summary>
        /// Create and register a new roaming network having the given
        /// unique roaming network identification.
        /// </summary>
        /// <param name="RoamingNetworkId">The unique identification of the new roaming network.</param>
        /// <param name="Name">The multi-language name of the roaming network.</param>
        /// <param name="Description">A multilanguage description of the roaming networks object.</param>
        /// <param name="Configurator">An optional delegate to configure the new roaming network after its creation.</param>
        /// <param name="InitialAdminStatus">The initial admin status of the roaming network.</param>
        /// <param name="InitialStatus">The initial status of the roaming network.</param>
        /// <param name="MaxAdminStatusScheduleSize">The maximum number of entries in the admin status history.</param>
        /// <param name="MaxStatusScheduleSize">The maximum number of entries in the status history.</param>
        /// <param name="ChargingStationSignatureGenerator">A delegate to sign a charging station.</param>
        /// <param name="ChargingPoolSignatureGenerator">A delegate to sign a charging pool.</param>
        /// <param name="ChargingStationOperatorSignatureGenerator">A delegate to sign a charging station operator.</param>
        public static IRoamingNetwork CreateNewRoamingNetwork(this IWWCPCore                             WWCPCore,
                                                              RoamingNetwork_Id                          RoamingNetworkId,
                                                              I18NString                                 Name,
                                                              I18NString?                                Description                                  = null,
                                                              Action<RoamingNetwork>?                    Configurator                                 = null,
                                                              RoamingNetworkAdminStatusTypes?            InitialAdminStatus                           = null,
                                                              RoamingNetworkStatusTypes?                 InitialStatus                                = null,
                                                              UInt16?                                    MaxAdminStatusScheduleSize                   = null,
                                                              UInt16?                                    MaxStatusScheduleSize                        = null,

                                                              Boolean?                                   DisableAuthenticationCache                   = false,
                                                              TimeSpan?                                  AuthenticationCacheTimeout                   = null,
                                                              UInt32?                                    MaxAuthStartResultCacheElements              = null,
                                                              UInt32?                                    MaxAuthStopResultCacheElements               = null,

                                                              Boolean?                                   DisableAuthenticationRateLimit               = true,
                                                              TimeSpan?                                  AuthenticationRateLimitTimeSpan              = null,
                                                              UInt16?                                    AuthenticationRateLimitPerChargingLocation   = null,

                                                              ChargingStationSignatureDelegate?          ChargingStationSignatureGenerator            = null,
                                                              ChargingPoolSignatureDelegate?             ChargingPoolSignatureGenerator               = null,
                                                              ChargingStationOperatorSignatureDelegate?  ChargingStationOperatorSignatureGenerator    = null,

                                                              IEnumerable<RoamingNetworkInfo>?           RoamingNetworkInfos                          = null,
                                                              Boolean                                    DisableNetworkSync                           = false,
                                                              String?                                    LoggingPath                                  = null)

        {

            var roamingNetwork = new RoamingNetwork(
                                     RoamingNetworkId,
                                     Name,
                                     Description,
                                     InitialAdminStatus,
                                     InitialStatus,
                                     MaxAdminStatusScheduleSize,
                                     MaxStatusScheduleSize,

                                     DisableAuthenticationCache,
                                     AuthenticationCacheTimeout,
                                     MaxAuthStartResultCacheElements,
                                     MaxAuthStopResultCacheElements,

                                     DisableAuthenticationRateLimit,
                                     AuthenticationRateLimitTimeSpan,
                                     AuthenticationRateLimitPerChargingLocation,

                                     ChargingStationSignatureGenerator,
                                     ChargingPoolSignatureGenerator,
                                     ChargingStationOperatorSignatureGenerator,

                                     RoamingNetworkInfos,
                                     DisableNetworkSync,
                                     LoggingPath
                                 );

            return WWCPCore.AddRoamingNetwork(roamingNetwork);

        }

        #endregion

    }


    /// <summary>
    /// A collection of roaming networks, which simplifies the handling of multiple roaming networks.
    /// </summary>
    public class WWCPCore : IWWCPCore
    {

        #region Data

        private readonly ConcurrentDictionary<RoamingNetwork_Id, IRoamingNetwork> roamingNetworks = [];

        #endregion

        #region Events

        #region OnRoamingNetworkAddition

        private readonly IVotingNotificator<WWCPCore, IRoamingNetwork, Boolean> roamingNetworkAddition;

        /// <summary>
        /// Called whenever a roaming network will be or was added.
        /// </summary>
        public IVotingSender<WWCPCore, IRoamingNetwork, Boolean> OnRoamingNetworkAddition
            => roamingNetworkAddition;

        #endregion

        #region OnRoamingNetworkRemoval

        private readonly IVotingNotificator<WWCPCore, IRoamingNetwork, Boolean> roamingNetworkRemoval;

        /// <summary>
        /// Called whenever a roaming network will be or was removed.
        /// </summary>
        public IVotingSender<WWCPCore, IRoamingNetwork, Boolean> OnRoamingNetworkRemoval
            => roamingNetworkRemoval;

        #endregion

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new roaming network collection.
        /// </summary>
        /// <param name="RoamingNetworks">Initial roaming networks to be added.</param>
        public WWCPCore(params RoamingNetwork[] RoamingNetworks)
        {

            this.roamingNetworkAddition  = new VotingNotificator<WWCPCore, IRoamingNetwork, Boolean>(() => new VetoVote(), true);
            this.roamingNetworkRemoval   = new VotingNotificator<WWCPCore, IRoamingNetwork, Boolean>(() => new VetoVote(), true);

            if (RoamingNetworks is not null && RoamingNetworks.Length > 0)
                AddRoamingNetworks(RoamingNetworks);

        }

        #endregion


        #region AddRoamingNetwork(RoamingNetwork)

        /// <summary>
        /// Register the given roaming network.
        /// </summary>
        /// <param name="RoamingNetwork">The roaming network to add.</param>
        public IRoamingNetwork AddRoamingNetwork(IRoamingNetwork RoamingNetwork)
        {

            #region Initial checks

            if (roamingNetworks.ContainsKey(RoamingNetwork.Id))
                throw new RoamingNetworkAlreadyExists(RoamingNetwork.Id);

            #endregion

            if (roamingNetworkAddition.SendVoting(EventTracking_Id.New, this, RoamingNetwork) &&
                roamingNetworks.TryAdd(RoamingNetwork.Id, RoamingNetwork))
            {
                roamingNetworkAddition.SendNotification(EventTracking_Id.New, this, RoamingNetwork);
                return RoamingNetwork;
            }

            throw new Exception();

        }

        #endregion

        #region AddRoamingNetworks(RoamingNetworks)

        /// <summary>
        /// Register the given roaming networks.
        /// </summary>
        /// <param name="RoamingNetworks">An enumeration of roaming networks to add.</param>
        public void AddRoamingNetworks(IEnumerable<IRoamingNetwork> RoamingNetworks)
        {

            foreach (var roamingNetwork in RoamingNetworks)
                AddRoamingNetwork(roamingNetwork);

        }

        #endregion

        #region Contains(RoamingNetworkId)

        /// <summary>
        /// Return the roaming network identified by the given unique roaming network identification.
        /// </summary>
        /// <param name="RoamingNetworkId">The unique identification of a roaming network.</param>
        public Boolean Contains(RoamingNetwork_Id RoamingNetworkId)

            => roamingNetworks.ContainsKey(RoamingNetworkId);

        #endregion

        #region GetRoamingNetwork(RoamingNetworkId)

        /// <summary>
        /// Return the roaming network identified by the given unique roaming network identification.
        /// </summary>
        /// <param name="RoamingNetworkId">The unique identification of a roaming network.</param>
        public IRoamingNetwork? GetRoamingNetwork(RoamingNetwork_Id RoamingNetworkId)
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
        public Boolean TryGetRoamingNetwork(RoamingNetwork_Id                         RoamingNetworkId,
                                            [NotNullWhen(true)] out IRoamingNetwork?  RoamingNetwork)

            => roamingNetworks.TryGetValue(
                   RoamingNetworkId,
                   out RoamingNetwork
               );

        #endregion

        #region RemoveRoamingNetwork(RoamingNetworkId)

        /// <summary>
        /// Try to return the roaming network identified by the given unique roaming network identification.
        /// </summary>
        /// <param name="RoamingNetworkId">The unique identification of a roaming network.</param>
        /// <returns>True, when the roaming network was found; false else.</returns>
        public IRoamingNetwork? RemoveRoamingNetwork(RoamingNetwork_Id RoamingNetworkId)
        {

            if (roamingNetworks.TryRemove(RoamingNetworkId, out var roamingNetwork))
                return roamingNetwork;

            return null;

        }

        #endregion

        #region TryRemoveRoamingNetwork(RoamingNetworkId, out RoamingNetwork)

        /// <summary>
        /// Try to return the roaming network identified by the given unique roaming network identification.
        /// </summary>
        /// <param name="RoamingNetworkId">The unique identification of a roaming network.</param>
        /// <param name="RoamingNetwork">The roaming network.</param>
        public Boolean TryRemoveRoamingNetwork(RoamingNetwork_Id                         RoamingNetworkId,
                                               [NotNullWhen(true)] out IRoamingNetwork?  RoamingNetwork)

            => roamingNetworks.TryRemove(
                   RoamingNetworkId,
                   out RoamingNetwork
               );

        #endregion


        #region IEnumerable<RoamingNetwork> Members

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()

            => roamingNetworks.Values.GetEnumerator();

        public IEnumerator<IRoamingNetwork> GetEnumerator()

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
