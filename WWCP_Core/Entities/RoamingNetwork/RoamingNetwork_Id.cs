/*
 * Copyright (c) 2014-2017 GraphDefined GmbH <achim.friedland@graphdefined.com>
 * This file is part of WWCP Core <https://github.com/OpenChargingCloud/WWCP_Core>
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

using org.GraphDefined.Vanaheimr.Illias;

#endregion

namespace org.GraphDefined.WWCP
{

    /// <summary>
    /// The unique identification of an e-mobility roaming network.
    /// </summary>
    public struct RoamingNetwork_Id : IId,
                                      IEquatable <RoamingNetwork_Id>,
                                      IComparable<RoamingNetwork_Id>

    {

        #region Data

        /// <summary>
        /// The internal identification.
        /// </summary>
        private readonly String InternalId;

        #endregion

        #region Properties

        /// <summary>
        /// The length of the roaming network identificator.
        /// </summary>
        public UInt64 Length
            => (UInt64) InternalId.Length;

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new roaming network identification.
        /// based on the given string.
        /// </summary>
        private RoamingNetwork_Id(String Text)
        {
            InternalId = Text;
        }

        #endregion


        #region Parse(Text)

        /// <summary>
        /// Parse the given string as a roaming network identification.
        /// </summary>
        /// <param name="Text">A text representation of a roaming network identification.</param>
        public static RoamingNetwork_Id Parse(String Text)
        {

            #region Initial checks

            if (Text != null)
                Text = Text.Trim();

            if (Text.IsNullOrEmpty())
                throw new ArgumentNullException(nameof(Text), "The given text representation of a roaming network identification must not be null or empty!");

            #endregion

            return new RoamingNetwork_Id(Text);

        }

        #endregion

        #region TryParse(Text, out RoamingNetworkId)

        /// <summary>
        /// Parse the given string as a roaming network identification.
        /// </summary>
        /// <param name="Text">A text representation of a roaming network identification.</param>
        /// <param name="RoamingNetworkId">The parsed roaming network identification.</param>
        public static Boolean TryParse(String Text, out RoamingNetwork_Id RoamingNetworkId)
        {

            #region Initial checks

            if (Text != null)
                Text = Text.Trim();

            if (Text.IsNullOrEmpty())
            {
                RoamingNetworkId = default(RoamingNetwork_Id);
                return false;
            }

            #endregion

            try
            {

                RoamingNetworkId = new RoamingNetwork_Id(Text);

                return true;

            }

#pragma warning disable RCS1075  // Avoid empty catch clause that catches System.Exception.
#pragma warning disable RECS0022 // A catch clause that catches System.Exception and has an empty body
            catch (Exception)
#pragma warning restore RECS0022 // A catch clause that catches System.Exception and has an empty body
#pragma warning restore RCS1075  // Avoid empty catch clause that catches System.Exception.
            { }

            RoamingNetworkId = default(RoamingNetwork_Id);
            return false;

        }

        #endregion

        #region Clone

        /// <summary>
        /// Clone this roaming network identification.
        /// </summary>
        public RoamingNetwork_Id Clone

            => new RoamingNetwork_Id(
                   new String(InternalId.ToCharArray())
               );

        #endregion


        #region Operator overloading

        #region Operator == (RoamingNetworkId1, RoamingNetworkId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="RoamingNetworkId1">A roaming network identification.</param>
        /// <param name="RoamingNetworkId2">Another roaming network identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator == (RoamingNetwork_Id RoamingNetworkId1, RoamingNetwork_Id RoamingNetworkId2)
        {

            // If both are null, or both are same instance, return true.
            if (Object.ReferenceEquals(RoamingNetworkId1, RoamingNetworkId2))
                return true;

            // If one is null, but not both, return false.
            if (((Object) RoamingNetworkId1 == null) || ((Object) RoamingNetworkId2 == null))
                return false;

            return RoamingNetworkId1.Equals(RoamingNetworkId2);

        }

        #endregion

        #region Operator != (RoamingNetworkId1, RoamingNetworkId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="RoamingNetworkId1">A roaming network identification.</param>
        /// <param name="RoamingNetworkId2">Another roaming network identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator != (RoamingNetwork_Id RoamingNetworkId1, RoamingNetwork_Id RoamingNetworkId2)
            => !(RoamingNetworkId1 == RoamingNetworkId2);

        #endregion

        #region Operator <  (RoamingNetworkId1, RoamingNetworkId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="RoamingNetworkId1">A roaming network identification.</param>
        /// <param name="RoamingNetworkId2">Another roaming network identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator < (RoamingNetwork_Id RoamingNetworkId1, RoamingNetwork_Id RoamingNetworkId2)
        {

            if ((Object) RoamingNetworkId1 == null)
                throw new ArgumentNullException(nameof(RoamingNetworkId1), "The given RoamingNetworkId1 must not be null!");

            return RoamingNetworkId1.CompareTo(RoamingNetworkId2) < 0;

        }

        #endregion

        #region Operator <= (RoamingNetworkId1, RoamingNetworkId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="RoamingNetworkId1">A roaming network identification.</param>
        /// <param name="RoamingNetworkId2">Another roaming network identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator <= (RoamingNetwork_Id RoamingNetworkId1, RoamingNetwork_Id RoamingNetworkId2)
            => !(RoamingNetworkId1 > RoamingNetworkId2);

        #endregion

        #region Operator >  (RoamingNetworkId1, RoamingNetworkId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="RoamingNetworkId1">A roaming network identification.</param>
        /// <param name="RoamingNetworkId2">Another roaming network identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator > (RoamingNetwork_Id RoamingNetworkId1, RoamingNetwork_Id RoamingNetworkId2)
        {

            if ((Object) RoamingNetworkId1 == null)
                throw new ArgumentNullException(nameof(RoamingNetworkId1), "The given RoamingNetworkId1 must not be null!");

            return RoamingNetworkId1.CompareTo(RoamingNetworkId2) > 0;

        }

        #endregion

        #region Operator >= (RoamingNetworkId1, RoamingNetworkId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="RoamingNetworkId1">A roaming network identification.</param>
        /// <param name="RoamingNetworkId2">Another roaming network identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator >= (RoamingNetwork_Id RoamingNetworkId1, RoamingNetwork_Id RoamingNetworkId2)
            => !(RoamingNetworkId1 < RoamingNetworkId2);

        #endregion

        #endregion

        #region IComparable<RoamingNetworkId> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        public Int32 CompareTo(Object Object)
        {

            if (Object == null)
                throw new ArgumentNullException(nameof(Object), "The given object must not be null!");

            if (!(Object is RoamingNetwork_Id))
                throw new ArgumentException("The given object is not a roaming network identification!",
                                            nameof(Object));

            return CompareTo((RoamingNetwork_Id) Object);

        }

        #endregion

        #region CompareTo(RoamingNetworkId)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="RoamingNetworkId">An object to compare with.</param>
        public Int32 CompareTo(RoamingNetwork_Id RoamingNetworkId)
        {

            if ((Object) RoamingNetworkId == null)
                throw new ArgumentNullException(nameof(RoamingNetworkId),  "The given roaming network identification must not be null!");

            // Compare the length of the RoamingNetworkIds
            var _Result = this.Length.CompareTo(RoamingNetworkId.Length);

            if (_Result == 0)
                _Result = String.Compare(InternalId, RoamingNetworkId.InternalId, StringComparison.Ordinal);

            return _Result;

        }

        #endregion

        #endregion

        #region IEquatable<RoamingNetworkId> Members

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

            if (!(Object is RoamingNetwork_Id))
                return false;

            return Equals((RoamingNetwork_Id) Object);

        }

        #endregion

        #region Equals(RoamingNetworkId)

        /// <summary>
        /// Compares two RoamingNetworkIds for equality.
        /// </summary>
        /// <param name="RoamingNetworkId">A roaming network identification to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public Boolean Equals(RoamingNetwork_Id RoamingNetworkId)
        {

            if ((Object) RoamingNetworkId == null)
                return false;

            return InternalId.Equals(RoamingNetworkId.InternalId);

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
        /// Return a string representation of this object.
        /// </summary>
        public override String ToString()
            => InternalId;

        #endregion

    }

}
