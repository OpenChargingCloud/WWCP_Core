/*
 * Copyright (c) 2014-2022 GraphDefined GmbH
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

using System;
using System.Linq;
using System.Threading;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Collections.Generic;

using org.GraphDefined.Vanaheimr.Illias;
using org.GraphDefined.Vanaheimr.Hermod;
using org.GraphDefined.Vanaheimr.Hermod.DNS;

#endregion

namespace org.GraphDefined.WWCP.Networking
{

    /// <summary>
    /// The unique identification of a node.
    /// </summary>
    public class Node_Id : IId,
                           IEquatable<Node_Id>,
                           IComparable<Node_Id>

    {

        #region Data

        private readonly String InternalId;

        //ToDo: Replace with better randomness!
        private static readonly Random _Random = new Random();

        #endregion

        #region Properties

        /// <summary>
        /// Indicates whether this identification is null or empty.
        /// </summary>
        public Boolean IsNullOrEmpty
            => InternalId.IsNullOrEmpty();

        /// <summary>
        /// Returns the length of the identification.
        /// </summary>
        public UInt64 Length
            => (UInt64) InternalId.Length;

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Generate a new Node identification based on the given string.
        /// </summary>
        private Node_Id(String  Text)
        {

            #region Initial checks

            if (Text.IsNullOrEmpty())
                throw new ArgumentNullException(nameof(Text),  "The given text must not be null or empty!");

            #endregion

            this.InternalId = Text;

        }

        #endregion


        #region Parse(Text)

        /// <summary>
        /// Parse the given string as a charging station identification (EVCS Id).
        /// </summary>
        /// <param name="Text">A text representation of a charging station identification.</param>
        public static Node_Id Parse(String Text)
        {

            #region Initial checks

            if (Text.IsNullOrEmpty())
                throw new ArgumentNullException(nameof(Text), "The given text must not be null or empty!");

            #endregion

            return new Node_Id(Text);

        }

        #endregion

        #region TryParse(Text, out ChargingStationId)

        /// <summary>
        /// Parse the given string as a charging station identification (EVCS Id).
        /// </summary>
        /// <param name="Text">A text representation of a charging station identification.</param>
        /// <param name="ChargingStationId">The parsed charging station identification.</param>
        public static Boolean TryParse(String Text, out Node_Id ChargingStationId)
        {

            #region Initial checks

            if (Text.IsNullOrEmpty())
            {
                ChargingStationId = null;
                return false;
            }

            #endregion

            try
            {

                ChargingStationId = new Node_Id(Text);

                return true;

            }
            catch (Exception)
            { }

            ChargingStationId = null;
            return false;

        }

        #endregion

        #region Random

        /// <summary>
        /// Generate a new unique identification of a Node.
        /// </summary>
        public static Node_Id Random
            => new Node_Id(_Random.RandomString(23));

        #endregion

        #region Clone

        /// <summary>
        /// Clone this Electric Vehicle Charging Station identification.
        /// </summary>
        public Node_Id Clone
            => new Node_Id(InternalId);

        #endregion


        #region Operator overloading

        #region Operator == (WWCPNodeClientId1, WWCPNodeClientId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="WWCPNodeClientId1">A WWCPNodeClient_Id.</param>
        /// <param name="WWCPNodeClientId2">Another WWCPNodeClient_Id.</param>
        /// <returns>true|false</returns>
        public static Boolean operator == (Node_Id WWCPNodeClientId1, Node_Id WWCPNodeClientId2)
        {

            // If both are null, or both are same instance, return true.
            if (ReferenceEquals(WWCPNodeClientId1, WWCPNodeClientId2))
                return true;

            // If one is null, but not both, return false.
            if (((Object) WWCPNodeClientId1 == null) || ((Object) WWCPNodeClientId2 == null))
                return false;

            return WWCPNodeClientId1.Equals(WWCPNodeClientId2);

        }

        #endregion

        #region Operator != (WWCPNodeClientId1, WWCPNodeClientId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="WWCPNodeClientId1">A WWCPNodeClient_Id.</param>
        /// <param name="WWCPNodeClientId2">Another WWCPNodeClient_Id.</param>
        /// <returns>true|false</returns>
        public static Boolean operator != (Node_Id WWCPNodeClientId1, Node_Id WWCPNodeClientId2)
        {
            return !(WWCPNodeClientId1 == WWCPNodeClientId2);
        }

        #endregion

        #region Operator <  (WWCPNodeClientId1, WWCPNodeClientId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="WWCPNodeClientId1">A WWCPNodeClient_Id.</param>
        /// <param name="WWCPNodeClientId2">Another WWCPNodeClient_Id.</param>
        /// <returns>true|false</returns>
        public static Boolean operator < (Node_Id WWCPNodeClientId1, Node_Id WWCPNodeClientId2)
        {

            if ((Object) WWCPNodeClientId1 == null)
                throw new ArgumentNullException("The given WWCPNodeClientId1 must not be null!");

            return WWCPNodeClientId1.CompareTo(WWCPNodeClientId2) < 0;

        }

        #endregion

        #region Operator <= (WWCPNodeClientId1, WWCPNodeClientId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="WWCPNodeClientId1">A WWCPNodeClient_Id.</param>
        /// <param name="WWCPNodeClientId2">Another WWCPNodeClient_Id.</param>
        /// <returns>true|false</returns>
        public static Boolean operator <= (Node_Id WWCPNodeClientId1, Node_Id WWCPNodeClientId2)
        {
            return !(WWCPNodeClientId1 > WWCPNodeClientId2);
        }

        #endregion

        #region Operator >  (WWCPNodeClientId1, WWCPNodeClientId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="WWCPNodeClientId1">A WWCPNodeClient_Id.</param>
        /// <param name="WWCPNodeClientId2">Another WWCPNodeClient_Id.</param>
        /// <returns>true|false</returns>
        public static Boolean operator > (Node_Id WWCPNodeClientId1, Node_Id WWCPNodeClientId2)
        {

            if ((Object) WWCPNodeClientId1 == null)
                throw new ArgumentNullException("The given WWCPNodeClientId1 must not be null!");

            return WWCPNodeClientId1.CompareTo(WWCPNodeClientId2) > 0;

        }

        #endregion

        #region Operator >= (WWCPNodeClientId1, WWCPNodeClientId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="WWCPNodeClientId1">A WWCPNodeClient_Id.</param>
        /// <param name="WWCPNodeClientId2">Another WWCPNodeClient_Id.</param>
        /// <returns>true|false</returns>
        public static Boolean operator >= (Node_Id WWCPNodeClientId1, Node_Id WWCPNodeClientId2)
        {
            return !(WWCPNodeClientId1 < WWCPNodeClientId2);
        }

        #endregion

        #endregion

        #region IComparable<WWCPNodeClient_Id> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        public Int32 CompareTo(Object Object)
        {

            if (Object == null)
                throw new ArgumentNullException("The given object must not be null!");

            // Check if the given object is an WWCPNodeClientId.
            var WWCPNodeClientId = Object as Node_Id;
            if ((Object) WWCPNodeClientId == null)
                throw new ArgumentException("The given object is not a WWCPNodeClientId!");

            return CompareTo(WWCPNodeClientId);

        }

        #endregion

        #region CompareTo(WWCPNodeClientId)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="WWCPNodeClientId">An object to compare with.</param>
        public Int32 CompareTo(Node_Id WWCPNodeClientId)
        {

            if ((Object) WWCPNodeClientId == null)
                throw new ArgumentNullException("The given WWCPNodeClientId must not be null!");

            return InternalId.CompareTo(WWCPNodeClientId.InternalId);

        }

        #endregion

        #endregion

        #region IEquatable<WWCPNodeClient_Id> Members

        #region Equals(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        /// <returns>true|false</returns>
        public override Boolean Equals(Object Object)
        {

            if (Object == null)
                return false;

            // Check if the given object is an WWCPNodeClientId.
            var WWCPNodeClientId = Object as Node_Id;
            if ((Object) WWCPNodeClientId == null)
                return false;

            return this.Equals(WWCPNodeClientId);

        }

        #endregion

        #region Equals(WWCPNodeClientId)

        /// <summary>
        /// Compares two WWCPNodeClientIds for equality.
        /// </summary>
        /// <param name="WWCPNodeClientId">A WWCPNodeClientId to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public Boolean Equals(Node_Id WWCPNodeClientId)
        {

            if ((Object) WWCPNodeClientId == null)
                return false;

            return InternalId.Equals(WWCPNodeClientId.InternalId);

        }

        #endregion

        #endregion

        #region GetHashCode()

        /// <summary>
        /// Return the HashCode of this object.
        /// </summary>
        /// <returns>The HashCode of this object.</returns>
        public override Int32 GetHashCode()
            => InternalId.GetHashCode();

        #endregion

        #region (override) ToString()

        /// <summary>
        /// Return a text representation of this object.
        /// </summary>
        public override String ToString()
            => InternalId;

        #endregion

    }

}
