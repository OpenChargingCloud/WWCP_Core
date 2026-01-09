/*
 * Copyright (c) 2014-2026 GraphDefined GmbH <achim.friedland@graphdefined.com>
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
    /// Extension methods for accessibility types.
    /// </summary>
    public static class AccessibilityTypeExtensions
    {

        /// <summary>
        /// Indicates whether this accessibility type is null or empty.
        /// </summary>
        /// <param name="AccessibilityType">A accessibility type.</param>
        public static Boolean IsNullOrEmpty(this AccessibilityType? AccessibilityType)
            => !AccessibilityType.HasValue || AccessibilityType.Value.IsNullOrEmpty;

        /// <summary>
        /// Indicates whether this accessibility type is NOT null or empty.
        /// </summary>
        /// <param name="AccessibilityType">A accessibility type.</param>
        public static Boolean IsNotNullOrEmpty(this AccessibilityType? AccessibilityType)
            => AccessibilityType.HasValue && AccessibilityType.Value.IsNotNullOrEmpty;

    }


    /// <summary>
    /// The accessibility type.
    /// </summary>
    public readonly struct AccessibilityType : IId<AccessibilityType>
    {

        #region Data

        /// <summary>
        /// The internal identification.
        /// </summary>
        private readonly String InternalId;

        #endregion

        #region Properties

        /// <summary>
        /// Indicates whether this accessibility type is null or empty.
        /// </summary>
        public Boolean IsNullOrEmpty
            => InternalId.IsNullOrEmpty();

        /// <summary>
        /// Indicates whether this accessibility type is NOT null or empty.
        /// </summary>
        public Boolean IsNotNullOrEmpty
            => InternalId.IsNotNullOrEmpty();

        /// <summary>
        /// The length of the accessibility type.
        /// </summary>
        public UInt64 Length
            => (UInt64) (InternalId?.Length ?? 0);

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new accessibility type based on the given text.
        /// </summary>
        /// <param name="Text">The text representation of a accessibility type.</param>
        private AccessibilityType(String Text)
        {
            this.InternalId = Text;
        }

        #endregion


        #region (static) Parse   (Text)

        /// <summary>
        /// Parse the given text as a accessibility type.
        /// </summary>
        /// <param name="Text">A text representation of a accessibility type.</param>
        public static AccessibilityType Parse(String Text)
        {

            if (TryParse(Text, out var accessibilityType))
                return accessibilityType;

            throw new ArgumentException($"Invalid text representation of a accessibility type: '" + Text + "'!",
                                        nameof(Text));

        }

        #endregion

        #region (static) TryParse(Text)

        /// <summary>
        /// Try to parse the given text as a accessibility type.
        /// </summary>
        /// <param name="Text">A text representation of a accessibility type.</param>
        public static AccessibilityType? TryParse(String Text)
        {

            if (TryParse(Text, out var accessibilityType))
                return accessibilityType;

            return null;

        }

        #endregion

        #region (static) TryParse(Text, out AccessibilityType)

        /// <summary>
        /// Try to parse the given text as a accessibility type.
        /// </summary>
        /// <param name="Text">A text representation of a accessibility type.</param>
        /// <param name="AccessibilityType">The parsed accessibility type.</param>
        public static Boolean TryParse(String Text, out AccessibilityType AccessibilityType)
        {

            Text = Text.Trim();

            if (Text.IsNotNullOrEmpty())
            {
                try
                {
                    AccessibilityType = new AccessibilityType(Text);
                    return true;
                }
                catch
                { }
            }

            AccessibilityType = default;
            return false;

        }

        #endregion

        #region Clone()

        /// <summary>
        /// Clone this accessibility type.
        /// </summary>
        public AccessibilityType Clone()

            => new (
                   InternalId.CloneString()
               );

        #endregion


        #region Static definitions

        /// <summary>
        /// Unspecified
        /// </summary>
        public static AccessibilityType Unspecified
            => new("Unspecified");

        /// <summary>
        /// Free Publicly Accessible
        /// </summary>
        public static AccessibilityType FreePubliclyAccessible
            => new("FreePubliclyAccessible");

        /// <summary>
        /// Restricted Access
        /// </summary>
        public static AccessibilityType RestrictedAccess
            => new("RestrictedAccess");

        /// <summary>
        /// Paying Publicly Accessible
        /// </summary>
        public static AccessibilityType PayingPubliclyAccessible
            => new("PayingPubliclyAccessible");

        /// <summary>
        /// Test Station
        /// </summary>
        public static AccessibilityType TestStation
            => new("TestStation");

        /// <summary>
        /// Test Pool
        /// </summary>
        public static AccessibilityType TestPool
            => new("TestPool");

        #endregion


        #region Operator overloading

        #region Operator == (AccessibilityType1, AccessibilityType2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="AccessibilityType1">A accessibility type.</param>
        /// <param name="AccessibilityType2">Another accessibility type.</param>
        /// <returns>true|false</returns>
        public static Boolean operator == (AccessibilityType AccessibilityType1,
                                           AccessibilityType AccessibilityType2)

            => AccessibilityType1.Equals(AccessibilityType2);

        #endregion

        #region Operator != (AccessibilityType1, AccessibilityType2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="AccessibilityType1">A accessibility type.</param>
        /// <param name="AccessibilityType2">Another accessibility type.</param>
        /// <returns>true|false</returns>
        public static Boolean operator != (AccessibilityType AccessibilityType1,
                                           AccessibilityType AccessibilityType2)

            => !AccessibilityType1.Equals(AccessibilityType2);

        #endregion

        #region Operator <  (AccessibilityType1, AccessibilityType2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="AccessibilityType1">A accessibility type.</param>
        /// <param name="AccessibilityType2">Another accessibility type.</param>
        /// <returns>true|false</returns>
        public static Boolean operator < (AccessibilityType AccessibilityType1,
                                          AccessibilityType AccessibilityType2)

            => AccessibilityType1.CompareTo(AccessibilityType2) < 0;

        #endregion

        #region Operator <= (AccessibilityType1, AccessibilityType2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="AccessibilityType1">A accessibility type.</param>
        /// <param name="AccessibilityType2">Another accessibility type.</param>
        /// <returns>true|false</returns>
        public static Boolean operator <= (AccessibilityType AccessibilityType1,
                                           AccessibilityType AccessibilityType2)

            => AccessibilityType1.CompareTo(AccessibilityType2) <= 0;

        #endregion

        #region Operator >  (AccessibilityType1, AccessibilityType2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="AccessibilityType1">A accessibility type.</param>
        /// <param name="AccessibilityType2">Another accessibility type.</param>
        /// <returns>true|false</returns>
        public static Boolean operator > (AccessibilityType AccessibilityType1,
                                          AccessibilityType AccessibilityType2)

            => AccessibilityType1.CompareTo(AccessibilityType2) > 0;

        #endregion

        #region Operator >= (AccessibilityType1, AccessibilityType2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="AccessibilityType1">A accessibility type.</param>
        /// <param name="AccessibilityType2">Another accessibility type.</param>
        /// <returns>true|false</returns>
        public static Boolean operator >= (AccessibilityType AccessibilityType1,
                                           AccessibilityType AccessibilityType2)

            => AccessibilityType1.CompareTo(AccessibilityType2) >= 0;

        #endregion

        #endregion

        #region IComparable<AccessibilityType> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two accessibility types.
        /// </summary>
        /// <param name="Object">A accessibility type to compare with.</param>
        public Int32 CompareTo(Object? Object)

            => Object is AccessibilityType accessibilityType
                   ? CompareTo(accessibilityType)
                   : throw new ArgumentException("The given object is not a accessibility type!",
                                                 nameof(Object));

        #endregion

        #region CompareTo(AccessibilityType)

        /// <summary>
        /// Compares two accessibility types.
        /// </summary>
        /// <param name="AccessibilityType">A accessibility type to compare with.</param>
        public Int32 CompareTo(AccessibilityType AccessibilityType)

            => String.Compare(InternalId,
                              AccessibilityType.InternalId,
                              StringComparison.OrdinalIgnoreCase);

        #endregion

        #endregion

        #region IEquatable<AccessibilityType> Members

        #region Equals(Object)

        /// <summary>
        /// Compares two accessibility types for equality.
        /// </summary>
        /// <param name="Object">A accessibility type to compare with.</param>
        public override Boolean Equals(Object? Object)

            => Object is AccessibilityType accessibilityType &&
                   Equals(accessibilityType);

        #endregion

        #region Equals(AccessibilityType)

        /// <summary>
        /// Compares two accessibility types for equality.
        /// </summary>
        /// <param name="AccessibilityType">A accessibility type to compare with.</param>
        public Boolean Equals(AccessibilityType AccessibilityType)

            => String.Equals(InternalId,
                             AccessibilityType.InternalId,
                             StringComparison.OrdinalIgnoreCase);

        #endregion

        #endregion

        #region (override) GetHashCode()

        /// <summary>
        /// Return the hash code of this object.
        /// </summary>
        public override Int32 GetHashCode()

            => InternalId?.ToUpper().GetHashCode() ?? 0;

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
