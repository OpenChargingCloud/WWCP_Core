/*
 * Copyright (c) 2014-2023 GraphDefined GmbH <achim.friedland@graphdefined.com>
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
    /// Extension methods for roaming network status typess.
    /// </summary>
    public static class RoamingNetworkStatusTypesExtensions
    {

        /// <summary>
        /// Indicates whether this roaming network status types is null or empty.
        /// </summary>
        /// <param name="RoamingNetworkStatusType">A roaming network status type.</param>
        public static Boolean IsNullOrEmpty(this RoamingNetworkStatusTypes? RoamingNetworkStatusType)
            => !RoamingNetworkStatusType.HasValue || RoamingNetworkStatusType.Value.IsNullOrEmpty;

        /// <summary>
        /// Indicates whether this roaming network status types is null or empty.
        /// </summary>
        /// <param name="RoamingNetworkStatusType">A roaming network status type.</param>
        public static Boolean IsNotNullOrEmpty(this RoamingNetworkStatusTypes? RoamingNetworkStatusType)
            => RoamingNetworkStatusType.HasValue && RoamingNetworkStatusType.Value.IsNotNullOrEmpty;

    }


    /// <summary>
    /// The status type of a roaming network.
    /// </summary>
    public readonly struct RoamingNetworkStatusTypes : IId,
                                                       IEquatable<RoamingNetworkStatusTypes>,
                                                       IComparable<RoamingNetworkStatusTypes>
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
        /// The length of the roaming network status.
        /// </summary>
        public UInt64 Length
            => (UInt64) (InternalId?.Length ?? 0);

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new roaming network status types based on the given string.
        /// </summary>
        private RoamingNetworkStatusTypes(String Text)
        {
            InternalId = Text;
        }

        #endregion


        #region (static) Parse   (Text)

        /// <summary>
        /// Parse the given string as a roaming network status type.
        /// </summary>
        /// <param name="Text">A text representation of a roaming network status type.</param>
        public static RoamingNetworkStatusTypes Parse(String Text)
        {

            if (TryParse(Text, out RoamingNetworkStatusTypes roamingNetworkStatusType))
                return roamingNetworkStatusType;

            throw new ArgumentException($"Invalid text representation of a roaming network status type: '" + Text + "'!",
                                        nameof(Text));

        }

        #endregion

        #region (static) TryParse(Text)

        /// <summary>
        /// Try to parse the given string as a roaming network status type.
        /// </summary>
        /// <param name="Text">A text representation of a roaming network status type.</param>
        public static RoamingNetworkStatusTypes? TryParse(String Text)
        {

            if (TryParse(Text, out RoamingNetworkStatusTypes roamingNetworkStatusType))
                return roamingNetworkStatusType;

            return null;

        }

        #endregion

        #region (static) TryParse(Text, out RoamingNetworkStatusType)

        /// <summary>
        /// Parse the given string as a roaming network status type.
        /// </summary>
        /// <param name="Text">A text representation of a roaming network status type.</param>
        /// <param name="RoamingNetworkStatusType">The parsed roaming network status type.</param>
        public static Boolean TryParse(String Text, out RoamingNetworkStatusTypes RoamingNetworkStatusType)
        {

            Text = Text.Trim();

            if (Text.IsNotNullOrEmpty())
            {
                try
                {
                    RoamingNetworkStatusType = new RoamingNetworkStatusTypes(Text);
                    return true;
                }
                catch
                { }
            }

            RoamingNetworkStatusType = default;
            return false;

        }

        #endregion

        #region Clone

        /// <summary>
        /// Clone this roaming network status type.
        /// </summary>
        public RoamingNetworkStatusTypes Clone

            => new(
                   new String(InternalId?.ToCharArray())
               );

        #endregion


        #region Static members

        /// <summary>
        /// Unknown status of the roaming network.
        /// </summary>
        public static readonly RoamingNetworkStatusTypes Unknown       = new("unknown");

        /// <summary>
        /// Unclear status of the roaming network.
        /// </summary>
        public static readonly RoamingNetworkStatusTypes Unspecified   = new("unspecified");

        /// <summary>
        /// The roaming network is currently offline.
        /// </summary>
        public static readonly RoamingNetworkStatusTypes Offline       = new("offline");

        /// <summary>
        /// The roaming network is not fully operational yet.
        /// </summary>
        public static readonly RoamingNetworkStatusTypes InDeployment  = new("inDeployment");

        /// <summary>
        /// The roaming network is available.
        /// </summary>
        public static readonly RoamingNetworkStatusTypes Available     = new("available");

        /// <summary>
        /// A fatal error has occured within the roaming network.
        /// </summary>
        public static readonly RoamingNetworkStatusTypes Error         = new("error");

        #endregion


        #region Operator overloading

        #region Operator == (RoamingNetworkStatusType1, RoamingNetworkStatusType2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="RoamingNetworkStatusType1">A roaming network status type.</param>
        /// <param name="RoamingNetworkStatusType2">Another roaming network status type.</param>
        /// <returns>true|false</returns>
        public static Boolean operator == (RoamingNetworkStatusTypes RoamingNetworkStatusType1,
                                           RoamingNetworkStatusTypes RoamingNetworkStatusType2)

            => RoamingNetworkStatusType1.Equals(RoamingNetworkStatusType2);

        #endregion

        #region Operator != (RoamingNetworkStatusType1, RoamingNetworkStatusType2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="RoamingNetworkStatusType1">A roaming network status type.</param>
        /// <param name="RoamingNetworkStatusType2">Another roaming network status type.</param>
        /// <returns>true|false</returns>
        public static Boolean operator != (RoamingNetworkStatusTypes RoamingNetworkStatusType1,
                                           RoamingNetworkStatusTypes RoamingNetworkStatusType2)

            => !RoamingNetworkStatusType1.Equals(RoamingNetworkStatusType2);

        #endregion

        #region Operator <  (RoamingNetworkStatusType1, RoamingNetworkStatusType2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="RoamingNetworkStatusType1">A roaming network status type.</param>
        /// <param name="RoamingNetworkStatusType2">Another roaming network status type.</param>
        /// <returns>true|false</returns>
        public static Boolean operator < (RoamingNetworkStatusTypes RoamingNetworkStatusType1,
                                          RoamingNetworkStatusTypes RoamingNetworkStatusType2)

            => RoamingNetworkStatusType1.CompareTo(RoamingNetworkStatusType2) < 0;

        #endregion

        #region Operator <= (RoamingNetworkStatusType1, RoamingNetworkStatusType2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="RoamingNetworkStatusType1">A roaming network status type.</param>
        /// <param name="RoamingNetworkStatusType2">Another roaming network status type.</param>
        /// <returns>true|false</returns>
        public static Boolean operator <= (RoamingNetworkStatusTypes RoamingNetworkStatusType1,
                                           RoamingNetworkStatusTypes RoamingNetworkStatusType2)

            => RoamingNetworkStatusType1.CompareTo(RoamingNetworkStatusType2) <= 0;

        #endregion

        #region Operator >  (RoamingNetworkStatusType1, RoamingNetworkStatusType2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="RoamingNetworkStatusType1">A roaming network status type.</param>
        /// <param name="RoamingNetworkStatusType2">Another roaming network status type.</param>
        /// <returns>true|false</returns>
        public static Boolean operator > (RoamingNetworkStatusTypes RoamingNetworkStatusType1,
                                          RoamingNetworkStatusTypes RoamingNetworkStatusType2)

            => RoamingNetworkStatusType1.CompareTo(RoamingNetworkStatusType2) > 0;

        #endregion

        #region Operator >= (RoamingNetworkStatusType1, RoamingNetworkStatusType2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="RoamingNetworkStatusType1">A roaming network status type.</param>
        /// <param name="RoamingNetworkStatusType2">Another roaming network status type.</param>
        /// <returns>true|false</returns>
        public static Boolean operator >= (RoamingNetworkStatusTypes RoamingNetworkStatusType1,
                                           RoamingNetworkStatusTypes RoamingNetworkStatusType2)

            => RoamingNetworkStatusType1.CompareTo(RoamingNetworkStatusType2) >= 0;

        #endregion

        #endregion

        #region IComparable<RoamingNetworkStatusTypes> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two roaming network status types.
        /// </summary>
        /// <param name="Object">A roaming network status type to compare with.</param>
        public Int32 CompareTo(Object? Object)

            => Object is RoamingNetworkStatusTypes roamingNetworkStatusType
                   ? CompareTo(roamingNetworkStatusType)
                   : throw new ArgumentException("The given object is not a roaming network status type!",
                                                 nameof(Object));

        #endregion

        #region CompareTo(RoamingNetworkStatusType)

        /// <summary>
        /// Compares two roaming network status types.
        /// </summary>
        /// <param name="RoamingNetworkStatusType">A roaming network status type to compare with.</param>
        public Int32 CompareTo(RoamingNetworkStatusTypes RoamingNetworkStatusType)

            => String.Compare(InternalId,
                              RoamingNetworkStatusType.InternalId,
                              StringComparison.OrdinalIgnoreCase);

        #endregion

        #endregion

        #region IEquatable<RoamingNetworkStatusTypes> Members

        #region Equals(Object)

        /// <summary>
        /// Compares two roaming network status types for equality.
        /// </summary>
        /// <param name="Object">A roaming network status type to compare with.</param>
        public override Boolean Equals(Object? Object)

            => Object is RoamingNetworkStatusTypes roamingNetworkStatusType &&
                   Equals(roamingNetworkStatusType);

        #endregion

        #region Equals(RoamingNetworkStatusType)

        /// <summary>
        /// Compares two roaming network status types for equality.
        /// </summary>
        /// <param name="RoamingNetworkStatusType">A roaming network status type to compare with.</param>
        public Boolean Equals(RoamingNetworkStatusTypes RoamingNetworkStatusType)

            => String.Equals(InternalId,
                             RoamingNetworkStatusType.InternalId,
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
