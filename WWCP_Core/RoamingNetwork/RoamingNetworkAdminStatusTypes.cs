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
    /// Extension methods for roaming network admin status typess.
    /// </summary>
    public static class RoamingNetworkAdminStatusTypesExtensions
    {

        /// <summary>
        /// Indicates whether this roaming network admin status types is null or empty.
        /// </summary>
        /// <param name="RoamingNetworkAdminStatusTypes">A roaming network admin status type.</param>
        public static Boolean IsNullOrEmpty(this RoamingNetworkAdminStatusTypes? RoamingNetworkAdminStatusTypes)
            => !RoamingNetworkAdminStatusTypes.HasValue || RoamingNetworkAdminStatusTypes.Value.IsNullOrEmpty;

        /// <summary>
        /// Indicates whether this roaming network admin status types is null or empty.
        /// </summary>
        /// <param name="RoamingNetworkAdminStatusTypes">A roaming network admin status type.</param>
        public static Boolean IsNotNullOrEmpty(this RoamingNetworkAdminStatusTypes? RoamingNetworkAdminStatusTypes)
            => RoamingNetworkAdminStatusTypes.HasValue && RoamingNetworkAdminStatusTypes.Value.IsNotNullOrEmpty;

    }


    /// <summary>
    /// The unique identification of a roaming network.
    /// </summary>
    public readonly struct RoamingNetworkAdminStatusTypes : IId,
                                                            IEquatable <RoamingNetworkAdminStatusTypes>,
                                                            IComparable<RoamingNetworkAdminStatusTypes>

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
        /// Create a new roaming network admin status type based on the given string.
        /// </summary>
        private RoamingNetworkAdminStatusTypes(String Text)
        {
            InternalId = Text;
        }

        #endregion


        #region (static) Parse   (Text)

        /// <summary>
        /// Parse the given string as a roaming network admin status type.
        /// </summary>
        /// <param name="Text">A text representation of a roaming network admin status type.</param>
        public static RoamingNetworkAdminStatusTypes Parse(String Text)
        {

            if (TryParse(Text, out RoamingNetworkAdminStatusTypes roamingNetworkAdminStatusTypes))
                return roamingNetworkAdminStatusTypes;

            throw new ArgumentException("Invalid text-representation of a roaming network admin status types: '" + Text + "'!",
                                        nameof(Text));

        }

        #endregion

        #region (static) TryParse(Text)

        /// <summary>
        /// Try to parse the given string as a roaming network admin status type.
        /// </summary>
        /// <param name="Text">A text representation of a roaming network admin status type.</param>
        public static RoamingNetworkAdminStatusTypes? TryParse(String Text)
        {

            if (TryParse(Text, out RoamingNetworkAdminStatusTypes roamingNetworkAdminStatusTypes))
                return roamingNetworkAdminStatusTypes;

            return null;

        }

        #endregion

        #region (static) TryParse(Text, out RoamingNetworkAdminStatusTypes)

        /// <summary>
        /// Parse the given string as a roaming network admin status type.
        /// </summary>
        /// <param name="Text">A text representation of a roaming network admin status type.</param>
        /// <param name="RoamingNetworkAdminStatusTypes">The parsed roaming network admin status type.</param>
        public static Boolean TryParse(String Text, out RoamingNetworkAdminStatusTypes RoamingNetworkAdminStatusTypes)
        {

            Text = Text.Trim();

            if (Text.IsNotNullOrEmpty())
            {
                try
                {
                    RoamingNetworkAdminStatusTypes = new RoamingNetworkAdminStatusTypes(Text);
                    return true;
                }
                catch
                { }
            }

            RoamingNetworkAdminStatusTypes = default;
            return false;

        }

        #endregion

        #region Clone

        /// <summary>
        /// Clone this roaming network admin status type.
        /// </summary>
        public RoamingNetworkAdminStatusTypes Clone

            => new (
                   new String(InternalId?.ToCharArray())
               );

        #endregion


        #region Static members

        private static readonly RoamingNetworkAdminStatusTypes unspecified   = new("unspecified");
        private static readonly RoamingNetworkAdminStatusTypes unkown        = new("unkown");
        private static readonly RoamingNetworkAdminStatusTypes outOfService  = new("outOfService");
        private static readonly RoamingNetworkAdminStatusTypes operational   = new("operational");
        private static readonly RoamingNetworkAdminStatusTypes internalUse   = new("internalUse");


        /// <summary>
        /// Unkown admin status of the roaming network.
        /// </summary>
        public static RoamingNetworkAdminStatusTypes Unkown         = unkown;

        /// <summary>
        /// Unclear admin status of the roaming network.
        /// </summary>
        public static RoamingNetworkAdminStatusTypes Unspecified    = unspecified;

        /// <summary>
        /// The roaming network is under maintenance.
        /// </summary>
        public static RoamingNetworkAdminStatusTypes OutOfService   = outOfService;

        /// <summary>
        /// The roaming network is operational.
        /// </summary>
        public static RoamingNetworkAdminStatusTypes Operational    = operational;

        /// <summary>
        /// Private or internal use.
        /// </summary>
        public static RoamingNetworkAdminStatusTypes InternalUse    = internalUse;

        #endregion


        #region Operator overloading

        #region Operator == (RoamingNetworkAdminStatusTypes1, RoamingNetworkAdminStatusTypes2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="RoamingNetworkAdminStatusTypes1">A roaming network admin status type.</param>
        /// <param name="RoamingNetworkAdminStatusTypes2">Another roaming network admin status type.</param>
        /// <returns>true|false</returns>
        public static Boolean operator == (RoamingNetworkAdminStatusTypes RoamingNetworkAdminStatusTypes1,
                                           RoamingNetworkAdminStatusTypes RoamingNetworkAdminStatusTypes2)

            => RoamingNetworkAdminStatusTypes1.Equals(RoamingNetworkAdminStatusTypes2);

        #endregion

        #region Operator != (RoamingNetworkAdminStatusTypes1, RoamingNetworkAdminStatusTypes2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="RoamingNetworkAdminStatusTypes1">A roaming network admin status type.</param>
        /// <param name="RoamingNetworkAdminStatusTypes2">Another roaming network admin status type.</param>
        /// <returns>true|false</returns>
        public static Boolean operator != (RoamingNetworkAdminStatusTypes RoamingNetworkAdminStatusTypes1,
                                           RoamingNetworkAdminStatusTypes RoamingNetworkAdminStatusTypes2)

            => !RoamingNetworkAdminStatusTypes1.Equals(RoamingNetworkAdminStatusTypes2);

        #endregion

        #region Operator <  (RoamingNetworkAdminStatusTypes1, RoamingNetworkAdminStatusTypes2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="RoamingNetworkAdminStatusTypes1">A roaming network admin status type.</param>
        /// <param name="RoamingNetworkAdminStatusTypes2">Another roaming network admin status type.</param>
        /// <returns>true|false</returns>
        public static Boolean operator < (RoamingNetworkAdminStatusTypes RoamingNetworkAdminStatusTypes1,
                                          RoamingNetworkAdminStatusTypes RoamingNetworkAdminStatusTypes2)

            => RoamingNetworkAdminStatusTypes1.CompareTo(RoamingNetworkAdminStatusTypes2) < 0;

        #endregion

        #region Operator <= (RoamingNetworkAdminStatusTypes1, RoamingNetworkAdminStatusTypes2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="RoamingNetworkAdminStatusTypes1">A roaming network admin status type.</param>
        /// <param name="RoamingNetworkAdminStatusTypes2">Another roaming network admin status type.</param>
        /// <returns>true|false</returns>
        public static Boolean operator <= (RoamingNetworkAdminStatusTypes RoamingNetworkAdminStatusTypes1,
                                           RoamingNetworkAdminStatusTypes RoamingNetworkAdminStatusTypes2)

            => RoamingNetworkAdminStatusTypes1.CompareTo(RoamingNetworkAdminStatusTypes2) <= 0;

        #endregion

        #region Operator >  (RoamingNetworkAdminStatusTypes1, RoamingNetworkAdminStatusTypes2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="RoamingNetworkAdminStatusTypes1">A roaming network admin status type.</param>
        /// <param name="RoamingNetworkAdminStatusTypes2">Another roaming network admin status type.</param>
        /// <returns>true|false</returns>
        public static Boolean operator > (RoamingNetworkAdminStatusTypes RoamingNetworkAdminStatusTypes1,
                                          RoamingNetworkAdminStatusTypes RoamingNetworkAdminStatusTypes2)

            => RoamingNetworkAdminStatusTypes1.CompareTo(RoamingNetworkAdminStatusTypes2) > 0;

        #endregion

        #region Operator >= (RoamingNetworkAdminStatusTypes1, RoamingNetworkAdminStatusTypes2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="RoamingNetworkAdminStatusTypes1">A roaming network admin status type.</param>
        /// <param name="RoamingNetworkAdminStatusTypes2">Another roaming network admin status type.</param>
        /// <returns>true|false</returns>
        public static Boolean operator >= (RoamingNetworkAdminStatusTypes RoamingNetworkAdminStatusTypes1,
                                           RoamingNetworkAdminStatusTypes RoamingNetworkAdminStatusTypes2)

            => RoamingNetworkAdminStatusTypes1.CompareTo(RoamingNetworkAdminStatusTypes2) >= 0;

        #endregion

        #endregion

        #region IComparable<RoamingNetworkAdminStatusTypes> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        public Int32 CompareTo(Object? Object)

            => Object is RoamingNetworkAdminStatusTypes roamingNetworkAdminStatusTypes
                   ? CompareTo(roamingNetworkAdminStatusTypes)
                   : throw new ArgumentException("The given object is not a roaming network admin status type!",
                                                 nameof(Object));

        #endregion

        #region CompareTo(RoamingNetworkAdminStatusTypes)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="RoamingNetworkAdminStatusTypes">An object to compare with.</param>
        public Int32 CompareTo(RoamingNetworkAdminStatusTypes RoamingNetworkAdminStatusTypes)

            => String.Compare(InternalId,
                              RoamingNetworkAdminStatusTypes.InternalId,
                              StringComparison.OrdinalIgnoreCase);

        #endregion

        #endregion

        #region IEquatable<RoamingNetworkAdminStatusTypes> Members

        #region Equals(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        /// <returns>true|false</returns>
        public override Boolean Equals(Object? Object)

            => Object is RoamingNetworkAdminStatusTypes roamingNetworkAdminStatusTypes &&
                   Equals(roamingNetworkAdminStatusTypes);

        #endregion

        #region Equals(RoamingNetworkAdminStatusTypes)

        /// <summary>
        /// Compares two roaming network admin status typess for equality.
        /// </summary>
        /// <param name="RoamingNetworkAdminStatusTypes">A roaming network admin status types to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public Boolean Equals(RoamingNetworkAdminStatusTypes RoamingNetworkAdminStatusTypes)

            => String.Equals(InternalId,
                             RoamingNetworkAdminStatusTypes.InternalId,
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
