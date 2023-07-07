/*
 * Copyright (c) 2014-2023 GraphDefined GmbH
 * This file is part of WWCP Node <https://github.com/OpenChargingCloud/WWCP_Node>
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

using System.Collections;

using org.GraphDefined.Vanaheimr.Illias;
using org.GraphDefined.Vanaheimr.Styx.Arrows;
using org.GraphDefined.Vanaheimr.Hermod;
using org.GraphDefined.Vanaheimr.Hermod.DNS;
using org.GraphDefined.Vanaheimr.Hermod.HTTP;

#endregion

namespace cloud.charging.open.protocols.WWCP
{

    /// <summary>
    /// A charging node.
    /// </summary>
    public class ChargingNode : NetworkServiceNode,
                                IRoamingNetworks
    {

        #region Data

        #endregion

        #region Properties

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new charging node.
        /// </summary>
        /// <param name="Id">An unique identification of this charging node.</param>
        /// <param name="Name">A multi-language name of this charging node.</param>
        /// <param name="Description">A multi-language description of this charging node.</param>
        /// 
        /// <param name="DefaultHTTPAPI">An optional default HTTP API.</param>
        /// 
        /// <param name="DNSClient">The DNS client used by the charging node.</param>
        public ChargingNode(NetworkServiceNode_Id?  Id               = null,
                            I18NString?             Name             = null,
                            I18NString?             Description      = null,

                            HTTPAPI?                DefaultHTTPAPI   = null,

                            DNSClient?              DNSClient        = null)

            : base(Id,
                   Name,
                   Description,

                   DefaultHTTPAPI,

                   DNSClient)

        {

        }

        #endregion


        #region Roaming Networks

        #region Data

        private readonly RoamingNetworks roamingNetworks = new ();

        /// <summary>
        /// An enumeration of all roaming networks.
        /// </summary>
        public IEnumerable<RoamingNetwork> RoamingNetworks
            => roamingNetworks;

        #endregion

        #region Events

        public IVotingSender<RoamingNetworks, RoamingNetwork, Boolean> OnRoamingNetworkAddition
            => roamingNetworks.OnRoamingNetworkAddition;

        public IVotingSender<RoamingNetworks, RoamingNetwork, Boolean> OnRoamingNetworkRemoval
            => roamingNetworks.OnRoamingNetworkRemoval;

        #endregion


        public RoamingNetwork AddRoamingNetwork(RoamingNetwork RoamingNetwork)
            => roamingNetworks.AddRoamingNetwork(RoamingNetwork);

        public void AddRoamingNetworks(IEnumerable<RoamingNetwork> RoamingNetworks)
            => roamingNetworks.AddRoamingNetworks(RoamingNetworks);

        public RoamingNetwork? GetRoamingNetwork(RoamingNetwork_Id RoamingNetworkId)
            => roamingNetworks.GetRoamingNetwork(RoamingNetworkId);

        public Boolean TryGetRoamingNetwork(RoamingNetwork_Id RoamingNetworkId, out RoamingNetwork? RoamingNetwork)
            => roamingNetworks.TryGetRoamingNetwork(RoamingNetworkId,
                                                    out RoamingNetwork);
        public RoamingNetwork? RemoveRoamingNetwork(RoamingNetwork_Id RoamingNetworkId)
            => roamingNetworks.RemoveRoamingNetwork(RoamingNetworkId);

        public Boolean RemoveRoamingNetwork(RoamingNetwork_Id RoamingNetworkId, out RoamingNetwork? RoamingNetwork)
            => roamingNetworks.RemoveRoamingNetwork(RoamingNetworkId,
                                                    out RoamingNetwork);


        #region IEnumerable<RoamingNetwork>

        IEnumerator<RoamingNetwork> IEnumerable<RoamingNetwork>.GetEnumerator()
        {
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }

        #endregion

        #endregion


        #region Clone

        /// <summary>
        /// Clone this charging node.
        /// </summary>
        public new ChargingNode Clone

            => new (Id.Clone);

        #endregion


        #region Operator overloading

        #region Operator == (ChargingNode1, ChargingNode2)

        /// <summary>
        /// Compares two node identifications for equality.
        /// </summary>
        /// <param name="ChargingNode1">A charging node.</param>
        /// <param name="ChargingNode2">Another charging node.</param>
        /// <returns>true|false</returns>
        public static Boolean operator == (ChargingNode ChargingNode1,
                                           ChargingNode ChargingNode2)
        {

            // If both are null, or both are same instance, return true.
            if (ReferenceEquals(ChargingNode1, ChargingNode2))
                return true;

            // If one is null, but not both, return false.
            if (ChargingNode1 is null || ChargingNode2 is null)
                return false;

            return ChargingNode1.Equals(ChargingNode2);

        }

        #endregion

        #region Operator != (ChargingNode1, ChargingNode2)

        /// <summary>
        /// Compares two node identifications for inequality.
        /// </summary>
        /// <param name="ChargingNode1">A charging node.</param>
        /// <param name="ChargingNode2">Another charging node.</param>
        /// <returns>true|false</returns>
        public static Boolean operator != (ChargingNode ChargingNode1,
                                           ChargingNode ChargingNode2)

            => !(ChargingNode1 == ChargingNode2);

        #endregion

        #region Operator <  (ChargingNode1, ChargingNode2)

        /// <summary>
        /// Compares two node identifications.
        /// </summary>
        /// <param name="ChargingNode1">A charging node.</param>
        /// <param name="ChargingNode2">Another charging node.</param>
        /// <returns>true|false</returns>
        public static Boolean operator < (ChargingNode ChargingNode1,
                                          ChargingNode ChargingNode2)
        {

            if (ChargingNode1 is null)
                throw new ArgumentNullException(nameof(ChargingNode1), "The given charging node 1 must not be null!");

            return ChargingNode1.CompareTo(ChargingNode2) < 0;

        }

        #endregion

        #region Operator <= (ChargingNode1, ChargingNode2)

        /// <summary>
        /// Compares two node identifications.
        /// </summary>
        /// <param name="ChargingNode1">A charging node.</param>
        /// <param name="ChargingNode2">Another charging node.</param>
        /// <returns>true|false</returns>
        public static Boolean operator <= (ChargingNode ChargingNode1,
                                           ChargingNode ChargingNode2)

            => !(ChargingNode1 > ChargingNode2);

        #endregion

        #region Operator >  (ChargingNode1, ChargingNode2)

        /// <summary>
        /// Compares two node identifications.
        /// </summary>
        /// <param name="ChargingNode1">A charging node.</param>
        /// <param name="ChargingNode2">Another charging node.</param>
        /// <returns>true|false</returns>
        public static Boolean operator > (ChargingNode ChargingNode1,
                                          ChargingNode ChargingNode2)
        {

            if (ChargingNode1 is null)
                throw new ArgumentNullException(nameof(ChargingNode1), "The given charging node 1 must not be null!");

            return ChargingNode1.CompareTo(ChargingNode2) > 0;

        }

        #endregion

        #region Operator >= (ChargingNode1, ChargingNode2)

        /// <summary>
        /// Compares two node identifications.
        /// </summary>
        /// <param name="ChargingNode1">A charging node.</param>
        /// <param name="ChargingNode2">Another charging node.</param>
        /// <returns>true|false</returns>
        public static Boolean operator >= (ChargingNode ChargingNode1,
                                           ChargingNode ChargingNode2)

            => !(ChargingNode1 < ChargingNode2);

        #endregion

        #endregion

        #region IComparable<Node> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two charging nodes.
        /// </summary>
        /// <param name="Object">A charging node to compare with.</param>
        public Int32 CompareTo(Object? Object)

            => Object is ChargingNode chargingNode
                   ? CompareTo(chargingNode)
                   : throw new ArgumentException("The given object is not a charging node!",
                                                 nameof(Object));

        #endregion

        #region CompareTo(Node)

        /// <summary>
        /// Compares two charging nodes.
        /// </summary>
        /// <param name="Node">A charging node to compare with.</param>
        public Int32 CompareTo(ChargingNode Node)
        {

            if (Node is null)
                throw new ArgumentNullException(nameof(Node), "The given charging node must not be null!");

            var c = Id.         CompareTo(Node.Id);

            //if (c == 0)
            //    c = Name.       CompareTo(Node.Name);

            //if (c == 0)
            //    c = Description.CompareTo(Node.Description);

            return c;

        }

        #endregion

        #endregion

        #region IEquatable<Node> Members

        #region Equals(Object)

        /// <summary>
        /// Compares two charging nodes for equality.
        /// </summary>
        /// <param name="Object">A charging node to compare with.</param>
        public override Boolean Equals(Object? Object)

            => Object is ChargingNode chargingNode &&
                   Equals(chargingNode);

        #endregion

        #region Equals(Node)

        /// <summary>
        /// Compares two charging nodes for equality.
        /// </summary>
        /// <param name="Node">A charging node to compare with.</param>
        public Boolean Equals(ChargingNode Node)

            => Node is not null &&

               Id.         Equals(Node.Id)   &&
               Name.       Equals(Node.Name) &&
               Description.Equals(Node.Description);

        #endregion

        #endregion

        #region GetHashCode()

        private readonly Int32 hashCode;

        /// <summary>
        /// Return the hash code of this object.
        /// </summary>
        public override Int32 GetHashCode()
            => hashCode;

        #endregion

        #region (override) ToString()

        /// <summary>
        /// Return a text representation of this object.
        /// </summary>
        public override String ToString()

            => Id.ToString();

        #endregion

    }

}
