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

#endregion

namespace cloud.charging.open.protocols.WWCP
{

    /// <summary>
    /// Extension methods for node identifications.
    /// </summary>
    public static class ChargingNodeIdExtensions
    {

        /// <summary>
        /// Indicates whether this node identification is null or empty.
        /// </summary>
        /// <param name="NodeId">A node identification.</param>
        public static Boolean IsNullOrEmpty(this ChargingNode_Id? NodeId)
            => !NodeId.HasValue || NodeId.Value.IsNullOrEmpty;

        /// <summary>
        /// Indicates whether this node identification is null or empty.
        /// </summary>
        /// <param name="NodeId">A node identification.</param>
        public static Boolean IsNotNullOrEmpty(this ChargingNode_Id? NodeId)
            => NodeId.HasValue && NodeId.Value.IsNotNullOrEmpty;

    }


    /// <summary>
    /// The unique identification of a node.
    /// </summary>
    public readonly struct ChargingNode_Id : IId,
                                             IEquatable <ChargingNode_Id>,
                                             IComparable<ChargingNode_Id>
    {

        #region Data

        /// <summary>
        /// The internal identification.
        /// </summary>
        private readonly String InternalId;

        #endregion

        #region Properties

        /// <summary>
        /// Indicates whether this identification is null or empty.
        /// </summary>
        public Boolean IsNullOrEmpty
            => InternalId.IsNullOrEmpty();

        /// <summary>
        /// Indicates whether this identification is NOT null or empty.
        /// </summary>
        public Boolean IsNotNullOrEmpty
            => InternalId.IsNotNullOrEmpty();

        /// <summary>
        /// The length of the node identificator.
        /// </summary>
        public UInt64 Length
            => (UInt64) (InternalId?.Length ?? 0);

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new unique node identification based on the given text.
        /// </summary>
        /// <param name="Text">The text representation of a node identification.</param>
        private ChargingNode_Id(String Text)
        {
            InternalId = Text;
        }

        #endregion


        #region (static) NewRandom(Length = 50)

        /// <summary>
        /// Create a new random node identification.
        /// </summary>
        /// <param name="Length">The expected length of the node identification.</param>
        public static ChargingNode_Id NewRandom(Byte Length = 50)

            => new(RandomExtensions.RandomString(Length));

        #endregion

        #region (static) Parse    (Text)

        /// <summary>
        /// Parse the given text as a node identification.
        /// </summary>
        /// <param name="Text">A text representation of a node identification.</param>
        public static ChargingNode_Id Parse(String Text)
        {

            if (TryParse(Text, out var nodeId))
                return nodeId;

            throw new ArgumentException($"Invalid text representation of a node identification: '{Text}'!",
                                        nameof(Text));

        }

        #endregion

        #region (static) TryParse (Text)

        /// <summary>
        /// Try to parse the given text as a node identification.
        /// </summary>
        /// <param name="Text">A text representation of a node identification.</param>
        public static ChargingNode_Id? TryParse(String Text)
        {

            if (TryParse(Text, out var nodeId))
                return nodeId;

            return null;

        }

        #endregion

        #region (static) TryParse (Text, out NodeId)

        /// <summary>
        /// Parse the given string as a node identification.
        /// </summary>
        /// <param name="Text">A text representation of a node identification.</param>
        /// <param name="NodeId">The parsed node identification.</param>
        public static Boolean TryParse(String Text, out ChargingNode_Id NodeId)
        {

            Text = Text.Trim();

            if (Text.IsNotNullOrEmpty())
            {
                try
                {
                    NodeId = new ChargingNode_Id(Text);
                    return true;
                }
                catch
                { }
            }

            NodeId = default;
            return false;

        }

        #endregion

        #region Clone

        /// <summary>
        /// Clone this node identification.
        /// </summary>
        public ChargingNode_Id Clone

            => new (
                   new String(InternalId?.ToCharArray())
               );

        #endregion


        #region Operator overloading

        #region Operator == (NodeId1, NodeId2)

        /// <summary>
        /// Compares two node identifications for equality.
        /// </summary>
        /// <param name="NodeId1">A node identification.</param>
        /// <param name="NodeId2">Another node identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator == (ChargingNode_Id NodeId1,
                                           ChargingNode_Id NodeId2)

            => NodeId1.Equals(NodeId2);

        #endregion

        #region Operator != (NodeId1, NodeId2)

        /// <summary>
        /// Compares two node identifications for inequality.
        /// </summary>
        /// <param name="NodeId1">A node identification.</param>
        /// <param name="NodeId2">Another node identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator != (ChargingNode_Id NodeId1,
                                           ChargingNode_Id NodeId2)

            => !NodeId1.Equals(NodeId2);

        #endregion

        #region Operator <  (NodeId1, NodeId2)

        /// <summary>
        /// Compares two node identifications.
        /// </summary>
        /// <param name="NodeId1">A node identification.</param>
        /// <param name="NodeId2">Another node identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator < (ChargingNode_Id NodeId1,
                                          ChargingNode_Id NodeId2)

            => NodeId1.CompareTo(NodeId2) < 0;

        #endregion

        #region Operator <= (NodeId1, NodeId2)

        /// <summary>
        /// Compares two node identifications.
        /// </summary>
        /// <param name="NodeId1">A node identification.</param>
        /// <param name="NodeId2">Another node identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator <= (ChargingNode_Id NodeId1,
                                           ChargingNode_Id NodeId2)

            => NodeId1.CompareTo(NodeId2) <= 0;

        #endregion

        #region Operator >  (NodeId1, NodeId2)

        /// <summary>
        /// Compares two node identifications.
        /// </summary>
        /// <param name="NodeId1">A node identification.</param>
        /// <param name="NodeId2">Another node identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator > (ChargingNode_Id NodeId1,
                                          ChargingNode_Id NodeId2)

            => NodeId1.CompareTo(NodeId2) > 0;

        #endregion

        #region Operator >= (NodeId1, NodeId2)

        /// <summary>
        /// Compares two node identifications.
        /// </summary>
        /// <param name="NodeId1">A node identification.</param>
        /// <param name="NodeId2">Another node identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator >= (ChargingNode_Id NodeId1,
                                           ChargingNode_Id NodeId2)

            => NodeId1.CompareTo(NodeId2) >= 0;

        #endregion

        #endregion

        #region IComparable<Node_Id> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two node identifications.
        /// </summary>
        /// <param name="Object">A node identification to compare with.</param>
        public Int32 CompareTo(Object? Object)

            => Object is ChargingNode_Id nodeId
                   ? CompareTo(nodeId)
                   : throw new ArgumentException("The given object is not a node identification!",
                                                 nameof(Object));

        #endregion

        #region CompareTo(NodeId)

        /// <summary>
        /// Compares two node identifications.
        /// </summary>
        /// <param name="NodeId">A node identification to compare with.</param>
        public Int32 CompareTo(ChargingNode_Id NodeId)

            => String.Compare(InternalId,
                              NodeId.InternalId,
                              StringComparison.OrdinalIgnoreCase);

        #endregion

        #endregion

        #region IEquatable<Node_Id> Members

        #region Equals(Object)

        /// <summary>
        /// Compares two node identifications for equality.
        /// </summary>
        /// <param name="Object">A node identification to compare with.</param>
        public override Boolean Equals(Object? Object)

            => Object is ChargingNode_Id nodeId &&
                   Equals(nodeId);

        #endregion

        #region Equals(NodeId)

        /// <summary>
        /// Compares two node identifications for equality.
        /// </summary>
        /// <param name="NodeId">A node identification to compare with.</param>
        public Boolean Equals(ChargingNode_Id NodeId)

            => String.Equals(InternalId,
                             NodeId.InternalId,
                             StringComparison.OrdinalIgnoreCase);

        #endregion

        #endregion

        #region GetHashCode()

        /// <summary>
        /// Return the hash code of this object.
        /// </summary>
        /// <returns>The hash code of this object.</returns>
        public override Int32 GetHashCode()

            => InternalId?.ToLower().GetHashCode() ?? 0;

        #endregion

        #region (override) ToString()

        /// <summary>
        /// Return a text representation of this object.
        /// </summary>
        public override String ToString()

            => InternalId ?? "";

        #endregion

    }

}
