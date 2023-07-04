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

using org.GraphDefined.Vanaheimr.Illias;
using org.GraphDefined.Vanaheimr.Hermod.DNS;

#endregion

namespace cloud.charging.open.protocols.WWCP
{

    /// <summary>
    /// A charging node.
    /// </summary>
    public class ChargingNode
    {

        #region Data

        

        #endregion

        #region Properties

        /// <summary>
        /// The unique identification of this charging node.
        /// </summary>
        public ChargingNode_Id  Id             { get; }

        /// <summary>
        /// The multi-language name of this charging node.
        /// </summary>
        public I18NString       Name           { get; }

        /// <summary>
        /// The multi-language description of this charging node.
        /// </summary>
        public I18NString       Description    { get; }


        /// <summary>
        /// The DNS client used by the charging node.
        /// </summary>
        public DNSClient        DNSClient      { get; }

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new charging node.
        /// </summary>
        /// <param name="Id">An unique identification of this charging node.</param>
        /// <param name="Name">A multi-language name of this charging node.</param>
        /// <param name="Description">A multi-language description of this charging node.</param>
        /// 
        /// <param name="DNSClient">The DNS client used by the charging node.</param>
        public ChargingNode(ChargingNode_Id  Id,
                            I18NString?      Name          = null,
                            I18NString?      Description   = null,

                            DNSClient?       DNSClient     = null)
        {

            #region Initial checks

            if (Id.IsNullOrEmpty)
                throw new ArgumentNullException(nameof(Id), "The given unique charging node identification must not be null or empty!");

            #endregion

            this.Id               = Id;
            this.Name             = Name        ?? I18NString.Empty;
            this.Description      = Description ?? I18NString.Empty;

            this.DNSClient        = DNSClient   ?? new DNSClient();

            unchecked
            {

                hashCode = this.Id.         GetHashCode() * 5 ^
                           this.Name.       GetHashCode() * 3 ^
                           this.Description.GetHashCode();

            }

        }

        #endregion


        //ToDo: Add Trackers

        #region Roaming Networks

        #region Data

        private readonly RoamingNetworks roamingNetworks = new ();

        /// <summary>
        /// An enumeration of all roaming networks.
        /// </summary>
        public IEnumerable<RoamingNetwork> RoamingNetworks
            => roamingNetworks;

        #endregion


        #endregion


        //ToDo: Add HTTP Servers
        //ToDo: Add HTTP WebSocket Servers
        //ToDo: Add Overlay Networks


        #region Clone

        /// <summary>
        /// Clone this charging node.
        /// </summary>
        public ChargingNode Clone

            => new (Id.Clone);

        #endregion


        #region Operator overloading

        #region Operator == (Node1, Node2)

        /// <summary>
        /// Compares two node identifications for equality.
        /// </summary>
        /// <param name="Node1">A charging node.</param>
        /// <param name="Node2">Another charging node.</param>
        /// <returns>true|false</returns>
        public static Boolean operator == (ChargingNode Node1,
                                           ChargingNode Node2)
        {

            // If both are null, or both are same instance, return true.
            if (ReferenceEquals(Node1, Node2))
                return true;

            // If one is null, but not both, return false.
            if (Node1 is null || Node2 is null)
                return false;

            return Node1.Equals(Node2);

        }

        #endregion

        #region Operator != (Node1, Node2)

        /// <summary>
        /// Compares two node identifications for inequality.
        /// </summary>
        /// <param name="Node1">A charging node.</param>
        /// <param name="Node2">Another charging node.</param>
        /// <returns>true|false</returns>
        public static Boolean operator != (ChargingNode Node1,
                                           ChargingNode Node2)

            => !(Node1 == Node2);

        #endregion

        #region Operator <  (Node1, Node2)

        /// <summary>
        /// Compares two node identifications.
        /// </summary>
        /// <param name="Node1">A charging node.</param>
        /// <param name="Node2">Another charging node.</param>
        /// <returns>true|false</returns>
        public static Boolean operator < (ChargingNode Node1,
                                          ChargingNode Node2)
        {

            if (Node1 is null)
                throw new ArgumentNullException(nameof(Node1), "The given charging node 1 must not be null!");

            return Node1.CompareTo(Node2) < 0;

        }

        #endregion

        #region Operator <= (Node1, Node2)

        /// <summary>
        /// Compares two node identifications.
        /// </summary>
        /// <param name="Node1">A charging node.</param>
        /// <param name="Node2">Another charging node.</param>
        /// <returns>true|false</returns>
        public static Boolean operator <= (ChargingNode Node1,
                                           ChargingNode Node2)

            => !(Node1 > Node2);

        #endregion

        #region Operator >  (Node1, Node2)

        /// <summary>
        /// Compares two node identifications.
        /// </summary>
        /// <param name="Node1">A charging node.</param>
        /// <param name="Node2">Another charging node.</param>
        /// <returns>true|false</returns>
        public static Boolean operator > (ChargingNode Node1,
                                          ChargingNode Node2)
        {

            if (Node1 is null)
                throw new ArgumentNullException(nameof(Node1), "The given charging node 1 must not be null!");

            return Node1.CompareTo(Node2) > 0;

        }

        #endregion

        #region Operator >= (Node1, Node2)

        /// <summary>
        /// Compares two node identifications.
        /// </summary>
        /// <param name="Node1">A charging node.</param>
        /// <param name="Node2">Another charging node.</param>
        /// <returns>true|false</returns>
        public static Boolean operator >= (ChargingNode Node1,
                                           ChargingNode Node2)

            => !(Node1 < Node2);

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
