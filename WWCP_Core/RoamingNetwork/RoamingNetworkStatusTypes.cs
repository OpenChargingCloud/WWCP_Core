/*
 * Copyright (c) 2014-2022 GraphDefined GmbH <achim.friedland@graphdefined.com>
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
        /// <param name="RoamingNetworkStatusTypes">A roaming network status type.</param>
        public static Boolean IsNullOrEmpty(this RoamingNetworkStatusTypes? RoamingNetworkStatusTypes)
            => !RoamingNetworkStatusTypes.HasValue || RoamingNetworkStatusTypes.Value.IsNullOrEmpty;

        /// <summary>
        /// Indicates whether this roaming network status types is null or empty.
        /// </summary>
        /// <param name="RoamingNetworkStatusTypes">A roaming network status type.</param>
        public static Boolean IsNotNullOrEmpty(this RoamingNetworkStatusTypes? RoamingNetworkStatusTypes)
            => RoamingNetworkStatusTypes.HasValue && RoamingNetworkStatusTypes.Value.IsNotNullOrEmpty;

    }


    /// <summary>
    /// The unique identification of a roaming network.
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
        /// The length of the roaming network identificator.
        /// </summary>
        public UInt64 Length
            => (UInt64) InternalId.Length;

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
        /// Parse the given string as a roaming network status types.
        /// </summary>
        /// <param name="Text">A text representation of a roaming network status types.</param>
        public static RoamingNetworkStatusTypes Parse(String Text)
        {

            if (TryParse(Text, out RoamingNetworkStatusTypes roamingNetworkStatusTypes))
                return roamingNetworkStatusTypes;

            throw new ArgumentException("Invalid text-representation of a roaming network status type: '" + Text + "'!",
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

            if (TryParse(Text, out RoamingNetworkStatusTypes roamingNetworkStatusTypes))
                return roamingNetworkStatusTypes;

            return null;

        }

        #endregion

        #region (static) TryParse(Text, out RoamingNetworkStatusTypes)

        /// <summary>
        /// Parse the given string as a roaming network status type.
        /// </summary>
        /// <param name="Text">A text representation of a roaming network status type.</param>
        /// <param name="RoamingNetworkStatusTypes">The parsed roaming network status type.</param>
        public static Boolean TryParse(String Text, out RoamingNetworkStatusTypes RoamingNetworkStatusTypes)
        {

            Text = Text.Trim();

            if (Text.IsNotNullOrEmpty())
            {
                try
                {
                    RoamingNetworkStatusTypes = new RoamingNetworkStatusTypes(Text);
                    return true;
                }
                catch
                { }
            }

            RoamingNetworkStatusTypes = default;
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

        private static readonly RoamingNetworkStatusTypes unknown       = new("unknown");
        private static readonly RoamingNetworkStatusTypes unspecified   = new("unspecified");
        private static readonly RoamingNetworkStatusTypes offline       = new("offline");
        private static readonly RoamingNetworkStatusTypes inDeployment  = new("inDeployment");
        private static readonly RoamingNetworkStatusTypes available     = new("available");
        private static readonly RoamingNetworkStatusTypes error         = new("error");


        /// <summary>
        /// Unknown status of the roaming network.
        /// </summary>
        public static RoamingNetworkStatusTypes Unknown        = unknown;

        /// <summary>
        /// Unclear status of the roaming network.
        /// </summary>
        public static RoamingNetworkStatusTypes Unspecified    = unspecified;

        /// <summary>
        /// The roaming network is currently offline.
        /// </summary>
        public static RoamingNetworkStatusTypes Offline        = offline;

        /// <summary>
        /// The roaming network is not fully operational yet.
        /// </summary>
        public static RoamingNetworkStatusTypes InDeployment   = inDeployment;

        /// <summary>
        /// The roaming network is available.
        /// </summary>
        public static RoamingNetworkStatusTypes Available      = available;

        /// <summary>
        /// A fatal error has occured within the roaming network.
        /// </summary>
        public static RoamingNetworkStatusTypes Error          = error;

        #endregion


        #region Operator overloading

        #region Operator == (RoamingNetworkStatusTypes1, RoamingNetworkStatusTypes2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="RoamingNetworkStatusTypes1">A roaming network status type.</param>
        /// <param name="RoamingNetworkStatusTypes2">Another roaming network status type.</param>
        /// <returns>true|false</returns>
        public static Boolean operator == (RoamingNetworkStatusTypes RoamingNetworkStatusTypes1,
                                           RoamingNetworkStatusTypes RoamingNetworkStatusTypes2)

            => RoamingNetworkStatusTypes1.Equals(RoamingNetworkStatusTypes2);

        #endregion

        #region Operator != (RoamingNetworkStatusTypes1, RoamingNetworkStatusTypes2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="RoamingNetworkStatusTypes1">A roaming network status type.</param>
        /// <param name="RoamingNetworkStatusTypes2">Another roaming network status type.</param>
        /// <returns>true|false</returns>
        public static Boolean operator != (RoamingNetworkStatusTypes RoamingNetworkStatusTypes1,
                                           RoamingNetworkStatusTypes RoamingNetworkStatusTypes2)

            => !RoamingNetworkStatusTypes1.Equals(RoamingNetworkStatusTypes2);

        #endregion

        #region Operator <  (RoamingNetworkStatusTypes1, RoamingNetworkStatusTypes2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="RoamingNetworkStatusTypes1">A roaming network status type.</param>
        /// <param name="RoamingNetworkStatusTypes2">Another roaming network status type.</param>
        /// <returns>true|false</returns>
        public static Boolean operator < (RoamingNetworkStatusTypes RoamingNetworkStatusTypes1,
                                          RoamingNetworkStatusTypes RoamingNetworkStatusTypes2)

            => RoamingNetworkStatusTypes1.CompareTo(RoamingNetworkStatusTypes2) < 0;

        #endregion

        #region Operator <= (RoamingNetworkStatusTypes1, RoamingNetworkStatusTypes2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="RoamingNetworkStatusTypes1">A roaming network status type.</param>
        /// <param name="RoamingNetworkStatusTypes2">Another roaming network status type.</param>
        /// <returns>true|false</returns>
        public static Boolean operator <= (RoamingNetworkStatusTypes RoamingNetworkStatusTypes1,
                                           RoamingNetworkStatusTypes RoamingNetworkStatusTypes2)

            => RoamingNetworkStatusTypes1.CompareTo(RoamingNetworkStatusTypes2) <= 0;

        #endregion

        #region Operator >  (RoamingNetworkStatusTypes1, RoamingNetworkStatusTypes2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="RoamingNetworkStatusTypes1">A roaming network status type.</param>
        /// <param name="RoamingNetworkStatusTypes2">Another roaming network status type.</param>
        /// <returns>true|false</returns>
        public static Boolean operator > (RoamingNetworkStatusTypes RoamingNetworkStatusTypes1,
                                          RoamingNetworkStatusTypes RoamingNetworkStatusTypes2)

            => RoamingNetworkStatusTypes1.CompareTo(RoamingNetworkStatusTypes2) > 0;

        #endregion

        #region Operator >= (RoamingNetworkStatusTypes1, RoamingNetworkStatusTypes2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="RoamingNetworkStatusTypes1">A roaming network status type.</param>
        /// <param name="RoamingNetworkStatusTypes2">Another roaming network status type.</param>
        /// <returns>true|false</returns>
        public static Boolean operator >= (RoamingNetworkStatusTypes RoamingNetworkStatusTypes1,
                                           RoamingNetworkStatusTypes RoamingNetworkStatusTypes2)

            => RoamingNetworkStatusTypes1.CompareTo(RoamingNetworkStatusTypes2) >= 0;

        #endregion

        #endregion

        #region IComparable<RoamingNetworkStatusTypes> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        public Int32 CompareTo(Object? Object)

            => Object is RoamingNetworkStatusTypes roamingNetworkStatusTypes
                   ? CompareTo(roamingNetworkStatusTypes)
                   : throw new ArgumentException("The given object is not a roaming network status type!",
                                                 nameof(Object));

        #endregion

        #region CompareTo(RoamingNetworkStatusTypes)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="RoamingNetworkStatusTypes">An object to compare with.</param>
        public Int32 CompareTo(RoamingNetworkStatusTypes RoamingNetworkStatusTypes)

            => String.Compare(InternalId,
                              RoamingNetworkStatusTypes.InternalId,
                              StringComparison.OrdinalIgnoreCase);

        #endregion

        #endregion

        #region IEquatable<RoamingNetworkStatusTypes> Members

        #region Equals(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        /// <returns>true|false</returns>
        public override Boolean Equals(Object? Object)

            => Object is RoamingNetworkStatusTypes roamingNetworkStatusTypes &&
                   Equals(roamingNetworkStatusTypes);

        #endregion

        #region Equals(RoamingNetworkStatusTypes)

        /// <summary>
        /// Compares two roaming network status types for equality.
        /// </summary>
        /// <param name="RoamingNetworkStatusTypes">A roaming network status type to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public Boolean Equals(RoamingNetworkStatusTypes RoamingNetworkStatusTypes)

            => String.Equals(InternalId,
                             RoamingNetworkStatusTypes.InternalId,
                             StringComparison.OrdinalIgnoreCase);

        #endregion

        #endregion

        #region GetHashCode()

        /// <summary>
        /// Return the hash code of this object.
        /// </summary>
        /// <returns>The hash code of this object.</returns>
        public override Int32 GetHashCode()

            => InternalId?.GetHashCode() ?? 0;

        #endregion

        #region (override) ToString()

        /// <summary>
        /// Return a text-representation of this object.
        /// </summary>
        public override String ToString()

            => InternalId ?? "";

        #endregion

    }

}
