/*
 * Copyright (c) 2014-2024 GraphDefined GmbH <achim.friedland@graphdefined.com>
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

using org.GraphDefined.Vanaheimr.Illias;

#endregion

namespace cloud.charging.open.protocols.WWCP
{

    /// <summary>
    /// A delegate for filtering roaming network identifications.
    /// </summary>
    /// <param name="RoamingNetworkId">A roaming network identification to include.</param>
    public delegate Boolean IncludeRoamingNetworkIdDelegate(RoamingNetwork_Id RoamingNetworkId);


    /// <summary>
    /// Extension methods for roaming network identifications.
    /// </summary>
    public static class RoamingNetworkIdExtensions
    {

        /// <summary>
        /// Indicates whether this roaming network identification is null or empty.
        /// </summary>
        /// <param name="RoamingNetworkId">A roaming network identification.</param>
        public static Boolean IsNullOrEmpty(this RoamingNetwork_Id? RoamingNetworkId)
            => !RoamingNetworkId.HasValue || RoamingNetworkId.Value.IsNullOrEmpty;

        /// <summary>
        /// Indicates whether this roaming network identification is NOT null or empty.
        /// </summary>
        /// <param name="RoamingNetworkId">A roaming network identification.</param>
        public static Boolean IsNotNullOrEmpty(this RoamingNetwork_Id? RoamingNetworkId)
            => RoamingNetworkId.HasValue && RoamingNetworkId.Value.IsNotNullOrEmpty;

    }


    /// <summary>
    /// The unique identification of a roaming network.
    /// </summary>
    public readonly struct RoamingNetwork_Id : IId,
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
        /// Indicates whether this roaming network identification is null or empty.
        /// </summary>
        public Boolean IsNullOrEmpty
            => InternalId.IsNullOrEmpty();

        /// <summary>
        /// Indicates whether this roaming network identification is NOT null or empty.
        /// </summary>
        public Boolean IsNotNullOrEmpty
            => InternalId.IsNotNullOrEmpty();

        /// <summary>
        /// The length of the roaming network identification.
        /// </summary>
        public UInt64 Length
            => (UInt64) (InternalId?.Length ?? 0);

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new roaming network identification based on the given string.
        /// </summary>
        /// <param name="Text">A text representation of a roaming network identification.</param>
        private RoamingNetwork_Id(String Text)
        {
            InternalId = Text;
        }

        #endregion


        #region (static) Parse   (Text)

        /// <summary>
        /// Parse the given string as a roaming network identification.
        /// </summary>
        /// <param name="Text">A text representation of a roaming network identification.</param>
        public static RoamingNetwork_Id Parse(String Text)
        {

            if (TryParse(Text, out var roamingNetworkId))
                return roamingNetworkId;

            throw new ArgumentException($"Invalid text representation of a roaming network identification: '{Text}'!",
                                        nameof(Text));

        }

        #endregion

        #region (static) TryParse(Text)

        /// <summary>
        /// Try to parse the given string as a roaming network identification.
        /// </summary>
        /// <param name="Text">A text representation of a roaming network identification.</param>
        public static RoamingNetwork_Id? TryParse(String Text)
        {

            if (TryParse(Text, out var roamingNetworkId))
                return roamingNetworkId;

            return null;

        }

        #endregion

        #region (static) TryParse(Text, out RoamingNetworkId)

        /// <summary>
        /// Parse the given text as a roaming network identification.
        /// </summary>
        /// <param name="Text">A text representation of a roaming network identification.</param>
        /// <param name="RoamingNetworkId">The parsed roaming network identification.</param>
        public static Boolean TryParse(String Text, out RoamingNetwork_Id RoamingNetworkId)
        {

            Text = Text.Trim();

            if (Text.IsNotNullOrEmpty())
            {
                RoamingNetworkId = new RoamingNetwork_Id(Text);
                return true;
            }

            RoamingNetworkId = default;
            return false;

        }

        #endregion

        #region Clone

        /// <summary>
        /// Clone this roaming network identification.
        /// </summary>
        public RoamingNetwork_Id Clone

            => new (
                   new String(InternalId?.ToCharArray())
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
        public static Boolean operator == (RoamingNetwork_Id RoamingNetworkId1,
                                           RoamingNetwork_Id RoamingNetworkId2)

            => RoamingNetworkId1.Equals(RoamingNetworkId2);

        #endregion

        #region Operator != (RoamingNetworkId1, RoamingNetworkId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="RoamingNetworkId1">A roaming network identification.</param>
        /// <param name="RoamingNetworkId2">Another roaming network identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator != (RoamingNetwork_Id RoamingNetworkId1,
                                           RoamingNetwork_Id RoamingNetworkId2)

            => !RoamingNetworkId1.Equals(RoamingNetworkId2);

        #endregion

        #region Operator <  (RoamingNetworkId1, RoamingNetworkId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="RoamingNetworkId1">A roaming network identification.</param>
        /// <param name="RoamingNetworkId2">Another roaming network identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator < (RoamingNetwork_Id RoamingNetworkId1,
                                          RoamingNetwork_Id RoamingNetworkId2)

            => RoamingNetworkId1.CompareTo(RoamingNetworkId2) < 0;

        #endregion

        #region Operator <= (RoamingNetworkId1, RoamingNetworkId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="RoamingNetworkId1">A roaming network identification.</param>
        /// <param name="RoamingNetworkId2">Another roaming network identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator <= (RoamingNetwork_Id RoamingNetworkId1,
                                           RoamingNetwork_Id RoamingNetworkId2)

            => RoamingNetworkId1.CompareTo(RoamingNetworkId2) <= 0;

        #endregion

        #region Operator >  (RoamingNetworkId1, RoamingNetworkId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="RoamingNetworkId1">A roaming network identification.</param>
        /// <param name="RoamingNetworkId2">Another roaming network identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator > (RoamingNetwork_Id RoamingNetworkId1,
                                          RoamingNetwork_Id RoamingNetworkId2)

            => RoamingNetworkId1.CompareTo(RoamingNetworkId2) > 0;

        #endregion

        #region Operator >= (RoamingNetworkId1, RoamingNetworkId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="RoamingNetworkId1">A roaming network identification.</param>
        /// <param name="RoamingNetworkId2">Another roaming network identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator >= (RoamingNetwork_Id RoamingNetworkId1,
                                           RoamingNetwork_Id RoamingNetworkId2)

            => RoamingNetworkId1.CompareTo(RoamingNetworkId2) >= 0;

        #endregion

        #endregion

        #region IComparable<RoamingNetwork_Id> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two roaming network identifications.
        /// </summary>
        /// <param name="Object">A roaming network identification to compare with.</param>
        public Int32 CompareTo(Object? Object)

            => Object is RoamingNetwork_Id roamingNetworkId
                   ? CompareTo(roamingNetworkId)
                   : throw new ArgumentException("The given object is not a roaming network identification!",
                                                 nameof(Object));

        #endregion

        #region CompareTo(RoamingNetworkId)

        /// <summary>
        /// Compares two roaming network identifications.
        /// </summary>
        /// <param name="RoamingNetworkId">A roaming network identification to compare with.</param>
        public Int32 CompareTo(RoamingNetwork_Id RoamingNetworkId)

            => String.Compare(InternalId,
                              RoamingNetworkId.InternalId,
                              StringComparison.OrdinalIgnoreCase);

        #endregion

        #endregion

        #region IEquatable<RoamingNetwork_Id> Members

        #region Equals(Object)

        /// <summary>
        /// Compares two roaming network identifications for equality.
        /// </summary>
        /// <param name="Object">A roaming network identification to compare with.</param>
        public override Boolean Equals(Object? Object)

            => Object is RoamingNetwork_Id roamingNetworkId &&
                   Equals(roamingNetworkId);

        #endregion

        #region Equals(RoamingNetworkId)

        /// <summary>
        /// Compares two roaming network identifications for equality.
        /// </summary>
        /// <param name="RoamingNetworkId">A roaming network identification to compare with.</param>
        public Boolean Equals(RoamingNetwork_Id RoamingNetworkId)

            => String.Equals(InternalId,
                             RoamingNetworkId.InternalId,
                             StringComparison.OrdinalIgnoreCase);

        #endregion

        #endregion

        #region (override) GetHashCode()

        /// <summary>
        /// Return the hash code of this object.
        /// </summary>
        /// <returns>The hash code of this object.</returns>
        public override Int32 GetHashCode()

            => InternalId?.GetHashCode() ?? 0;

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
