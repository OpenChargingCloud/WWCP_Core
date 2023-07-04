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
    /// Extension methods for roaming network admin status types.
    /// </summary>
    public static class RoamingNetworkAdminStatusTypesExtensions
    {

        /// <summary>
        /// Indicates whether this roaming network admin status types is null or empty.
        /// </summary>
        /// <param name="RoamingNetworkAdminStatusType">A roaming network admin status type.</param>
        public static Boolean IsNullOrEmpty(this RoamingNetworkAdminStatusTypes? RoamingNetworkAdminStatusType)
            => !RoamingNetworkAdminStatusType.HasValue || RoamingNetworkAdminStatusType.Value.IsNullOrEmpty;

        /// <summary>
        /// Indicates whether this roaming network admin status types is null or empty.
        /// </summary>
        /// <param name="RoamingNetworkAdminStatusType">A roaming network admin status type.</param>
        public static Boolean IsNotNullOrEmpty(this RoamingNetworkAdminStatusTypes? RoamingNetworkAdminStatusType)
            => RoamingNetworkAdminStatusType.HasValue && RoamingNetworkAdminStatusType.Value.IsNotNullOrEmpty;

    }


    /// <summary>
    /// The admin status type of a roaming network.
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
        /// The length of the roaming network admin status.
        /// </summary>
        public UInt64 Length
            => (UInt64) (InternalId?.Length ?? 0);

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

            if (TryParse(Text, out RoamingNetworkAdminStatusTypes roamingNetworkAdminStatusType))
                return roamingNetworkAdminStatusType;

            throw new ArgumentException($"Invalid text representation of a roaming network admin status type: '" + Text + "'!",
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

            if (TryParse(Text, out RoamingNetworkAdminStatusTypes roamingNetworkAdminStatusType))
                return roamingNetworkAdminStatusType;

            return null;

        }

        #endregion

        #region (static) TryParse(Text, out RoamingNetworkAdminStatusType)

        /// <summary>
        /// Parse the given string as a roaming network admin status type.
        /// </summary>
        /// <param name="Text">A text representation of a roaming network admin status type.</param>
        /// <param name="RoamingNetworkAdminStatusType">The parsed roaming network admin status type.</param>
        public static Boolean TryParse(String Text, out RoamingNetworkAdminStatusTypes RoamingNetworkAdminStatusType)
        {

            Text = Text.Trim();

            if (Text.IsNotNullOrEmpty())
            {
                try
                {
                    RoamingNetworkAdminStatusType = new RoamingNetworkAdminStatusTypes(Text);
                    return true;
                }
                catch
                { }
            }

            RoamingNetworkAdminStatusType = default;
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

        /// <summary>
        /// Unkown admin status of the roaming network.
        /// </summary>
        public static readonly RoamingNetworkAdminStatusTypes Unkown        = new("unkown");

        /// <summary>
        /// Unclear admin status of the roaming network.
        /// </summary>
        public static readonly RoamingNetworkAdminStatusTypes Unspecified   = new("unspecified");

        /// <summary>
        /// The roaming network is under maintenance.
        /// </summary>
        public static readonly RoamingNetworkAdminStatusTypes OutOfService  = new("outOfService");

        /// <summary>
        /// The roaming network is operational.
        /// </summary>
        public static readonly RoamingNetworkAdminStatusTypes Operational   = new("operational");

        /// <summary>
        /// Private or internal use.
        /// </summary>
        public static readonly RoamingNetworkAdminStatusTypes InternalUse   = new("internalUse");

        #endregion


        #region Operator overloading

        #region Operator == (RoamingNetworkAdminStatusType1, RoamingNetworkAdminStatusType2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="RoamingNetworkAdminStatusType1">A roaming network admin status type.</param>
        /// <param name="RoamingNetworkAdminStatusType2">Another roaming network admin status type.</param>
        /// <returns>true|false</returns>
        public static Boolean operator == (RoamingNetworkAdminStatusTypes RoamingNetworkAdminStatusType1,
                                           RoamingNetworkAdminStatusTypes RoamingNetworkAdminStatusType2)

            => RoamingNetworkAdminStatusType1.Equals(RoamingNetworkAdminStatusType2);

        #endregion

        #region Operator != (RoamingNetworkAdminStatusType1, RoamingNetworkAdminStatusType2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="RoamingNetworkAdminStatusType1">A roaming network admin status type.</param>
        /// <param name="RoamingNetworkAdminStatusType2">Another roaming network admin status type.</param>
        /// <returns>true|false</returns>
        public static Boolean operator != (RoamingNetworkAdminStatusTypes RoamingNetworkAdminStatusType1,
                                           RoamingNetworkAdminStatusTypes RoamingNetworkAdminStatusType2)

            => !RoamingNetworkAdminStatusType1.Equals(RoamingNetworkAdminStatusType2);

        #endregion

        #region Operator <  (RoamingNetworkAdminStatusType1, RoamingNetworkAdminStatusType2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="RoamingNetworkAdminStatusType1">A roaming network admin status type.</param>
        /// <param name="RoamingNetworkAdminStatusType2">Another roaming network admin status type.</param>
        /// <returns>true|false</returns>
        public static Boolean operator < (RoamingNetworkAdminStatusTypes RoamingNetworkAdminStatusType1,
                                          RoamingNetworkAdminStatusTypes RoamingNetworkAdminStatusType2)

            => RoamingNetworkAdminStatusType1.CompareTo(RoamingNetworkAdminStatusType2) < 0;

        #endregion

        #region Operator <= (RoamingNetworkAdminStatusType1, RoamingNetworkAdminStatusType2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="RoamingNetworkAdminStatusType1">A roaming network admin status type.</param>
        /// <param name="RoamingNetworkAdminStatusType2">Another roaming network admin status type.</param>
        /// <returns>true|false</returns>
        public static Boolean operator <= (RoamingNetworkAdminStatusTypes RoamingNetworkAdminStatusType1,
                                           RoamingNetworkAdminStatusTypes RoamingNetworkAdminStatusType2)

            => RoamingNetworkAdminStatusType1.CompareTo(RoamingNetworkAdminStatusType2) <= 0;

        #endregion

        #region Operator >  (RoamingNetworkAdminStatusType1, RoamingNetworkAdminStatusType2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="RoamingNetworkAdminStatusType1">A roaming network admin status type.</param>
        /// <param name="RoamingNetworkAdminStatusType2">Another roaming network admin status type.</param>
        /// <returns>true|false</returns>
        public static Boolean operator > (RoamingNetworkAdminStatusTypes RoamingNetworkAdminStatusType1,
                                          RoamingNetworkAdminStatusTypes RoamingNetworkAdminStatusType2)

            => RoamingNetworkAdminStatusType1.CompareTo(RoamingNetworkAdminStatusType2) > 0;

        #endregion

        #region Operator >= (RoamingNetworkAdminStatusType1, RoamingNetworkAdminStatusType2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="RoamingNetworkAdminStatusType1">A roaming network admin status type.</param>
        /// <param name="RoamingNetworkAdminStatusType2">Another roaming network admin status type.</param>
        /// <returns>true|false</returns>
        public static Boolean operator >= (RoamingNetworkAdminStatusTypes RoamingNetworkAdminStatusType1,
                                           RoamingNetworkAdminStatusTypes RoamingNetworkAdminStatusType2)

            => RoamingNetworkAdminStatusType1.CompareTo(RoamingNetworkAdminStatusType2) >= 0;

        #endregion

        #endregion

        #region IComparable<RoamingNetworkAdminStatusTypes> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two roaming network admin status types.
        /// </summary>
        /// <param name="Object">A roaming network admin status type to compare with.</param>
        public Int32 CompareTo(Object? Object)

            => Object is RoamingNetworkAdminStatusTypes roamingNetworkAdminStatusType
                   ? CompareTo(roamingNetworkAdminStatusType)
                   : throw new ArgumentException("The given object is not a roaming network admin status type!",
                                                 nameof(Object));

        #endregion

        #region CompareTo(RoamingNetworkAdminStatusType)

        /// <summary>
        /// Compares two roaming network admin status types.
        /// </summary>
        /// <param name="RoamingNetworkAdminStatusType">A roaming network admin status type to compare with.</param>
        public Int32 CompareTo(RoamingNetworkAdminStatusTypes RoamingNetworkAdminStatusType)

            => String.Compare(InternalId,
                              RoamingNetworkAdminStatusType.InternalId,
                              StringComparison.OrdinalIgnoreCase);

        #endregion

        #endregion

        #region IEquatable<RoamingNetworkAdminStatusTypes> Members

        #region Equals(Object)

        /// <summary>
        /// Compares two roaming network admin status types for equality.
        /// </summary>
        /// <param name="Object">A roaming network admin status type to compare with.</param>
        public override Boolean Equals(Object? Object)

            => Object is RoamingNetworkAdminStatusTypes roamingNetworkAdminStatusType &&
                   Equals(roamingNetworkAdminStatusType);

        #endregion

        #region Equals(RoamingNetworkAdminStatusType)

        /// <summary>
        /// Compares two roaming network admin status types for equality.
        /// </summary>
        /// <param name="RoamingNetworkAdminStatusType">A roaming network admin status type to compare with.</param>
        public Boolean Equals(RoamingNetworkAdminStatusTypes RoamingNetworkAdminStatusType)

            => String.Equals(InternalId,
                             RoamingNetworkAdminStatusType.InternalId,
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
        /// Return a text representation of this object.
        /// </summary>
        public override String ToString()

            => InternalId ?? "";

        #endregion

    }

}
