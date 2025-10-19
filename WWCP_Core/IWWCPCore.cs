/*
 * Copyright (c) 2014-2025 GraphDefined GmbH <achim.friedland@graphdefined.com>
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

using System.Diagnostics.CodeAnalysis;

using org.GraphDefined.Vanaheimr.Styx.Arrows;

#endregion

namespace cloud.charging.open.protocols.WWCP
{

    public interface IWWCPCore : IEnumerable<IRoamingNetwork>
    {

        IVotingSender<WWCPCore, IRoamingNetwork, Boolean>  OnRoamingNetworkAddition    { get; }

        IVotingSender<WWCPCore, IRoamingNetwork, Boolean>  OnRoamingNetworkRemoval     { get; }


        IRoamingNetwork AddRoamingNetwork(IRoamingNetwork RoamingNetwork);

        void AddRoamingNetworks(IEnumerable<IRoamingNetwork> RoamingNetworks);

        Boolean Contains(RoamingNetwork_Id RoamingNetworkId);

        IRoamingNetwork? GetRoamingNetwork(RoamingNetwork_Id RoamingNetworkId);

        Boolean TryGetRoamingNetwork(RoamingNetwork_Id                         RoamingNetworkId,
                                     [NotNullWhen(true)] out IRoamingNetwork?  RoamingNetwork);

        IRoamingNetwork? RemoveRoamingNetwork(RoamingNetwork_Id RoamingNetworkId);

        Boolean RemoveRoamingNetwork(RoamingNetwork_Id RoamingNetworkId, out IRoamingNetwork? RoamingNetwork);

    }

}
