/*
 * Copyright (c) 2014-2025 GraphDefined GmbH <achim.friedland@graphdefined.com>
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
    /// Extension methods for charging station operator admin status types.
    /// </summary>
    public static class ChargingStationOperatorAdminStatusTypesExtensions
    {

        /// <summary>
        /// Indicates whether this charging station operator admin status types is null or empty.
        /// </summary>
        /// <param name="ChargingStationOperatorAdminStatusType">A charging station operator admin status type.</param>
        public static Boolean IsNullOrEmpty(this ChargingStationOperatorAdminStatusTypes? ChargingStationOperatorAdminStatusType)
            => !ChargingStationOperatorAdminStatusType.HasValue || ChargingStationOperatorAdminStatusType.Value.IsNullOrEmpty;

        /// <summary>
        /// Indicates whether this charging station operator admin status types is null or empty.
        /// </summary>
        /// <param name="ChargingStationOperatorAdminStatusType">A charging station operator admin status type.</param>
        public static Boolean IsNotNullOrEmpty(this ChargingStationOperatorAdminStatusTypes? ChargingStationOperatorAdminStatusType)
            => ChargingStationOperatorAdminStatusType.HasValue && ChargingStationOperatorAdminStatusType.Value.IsNotNullOrEmpty;

    }


    /// <summary>
    /// The admin status of a charging station operator.
    /// </summary>
    public readonly struct ChargingStationOperatorAdminStatusTypes : IId,
                                                                     IEquatable<ChargingStationOperatorAdminStatusTypes>,
                                                                     IComparable<ChargingStationOperatorAdminStatusTypes>
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
        /// The length of the charging station operator admin status.
        /// </summary>
        public UInt64 Length
            => (UInt64) (InternalId?.Length ?? 0);

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new charging station operator admin status type based on the given string.
        /// </summary>
        private ChargingStationOperatorAdminStatusTypes(String Text)
        {
            InternalId = Text;
        }

        #endregion


        #region (static) Parse   (Text)

        /// <summary>
        /// Parse the given string as a charging station operator admin status type.
        /// </summary>
        /// <param name="Text">A text representation of a charging station operator admin status type.</param>
        public static ChargingStationOperatorAdminStatusTypes Parse(String Text)
        {

            if (TryParse(Text, out ChargingStationOperatorAdminStatusTypes chargingStationOperatorAdminStatusType))
                return chargingStationOperatorAdminStatusType;

            throw new ArgumentException($"Invalid text representation of a charging station operator admin status type: '" + Text + "'!",
                                        nameof(Text));

        }

        #endregion

        #region (static) TryParse(Text)

        /// <summary>
        /// Try to parse the given string as a charging station operator admin status type.
        /// </summary>
        /// <param name="Text">A text representation of a charging station operator admin status type.</param>
        public static ChargingStationOperatorAdminStatusTypes? TryParse(String Text)
        {

            if (TryParse(Text, out ChargingStationOperatorAdminStatusTypes chargingStationOperatorAdminStatusType))
                return chargingStationOperatorAdminStatusType;

            return null;

        }

        #endregion

        #region (static) TryParse(Text, out ChargingStationOperatorAdminStatusType)

        /// <summary>
        /// Parse the given string as a charging station operator admin status type.
        /// </summary>
        /// <param name="Text">A text representation of a charging station operator admin status type.</param>
        /// <param name="ChargingStationOperatorAdminStatusType">The parsed charging station operator admin status type.</param>
        public static Boolean TryParse(String Text, out ChargingStationOperatorAdminStatusTypes ChargingStationOperatorAdminStatusType)
        {

            Text = Text.Trim();

            if (Text.IsNotNullOrEmpty())
            {
                try
                {
                    ChargingStationOperatorAdminStatusType = new ChargingStationOperatorAdminStatusTypes(Text);
                    return true;
                }
                catch
                { }
            }

            ChargingStationOperatorAdminStatusType = default;
            return false;

        }

        #endregion

        #region Clone()

        /// <summary>
        /// Clone this charging station operator admin status type.
        /// </summary>
        public ChargingStationOperatorAdminStatusTypes Clone()

            => new (
                   InternalId.CloneString()
               );

        #endregion


        #region Static members

        /// <summary>
        /// Unknown admin status of the charging station operator.
        /// </summary>
        public static readonly ChargingStationOperatorAdminStatusTypes Unknown        = new("unknown");

        /// <summary>
        /// Unclear admin status of the charging station operator.
        /// </summary>
        public static readonly ChargingStationOperatorAdminStatusTypes Unspecified   = new("unspecified");

        /// <summary>
        /// The charging station operator is under maintenance.
        /// </summary>
        public static readonly ChargingStationOperatorAdminStatusTypes OutOfService  = new("outOfService");

        /// <summary>
        /// The charging station operator is operational.
        /// </summary>
        public static readonly ChargingStationOperatorAdminStatusTypes Operational   = new("operational");

        /// <summary>
        /// Private or internal use.
        /// </summary>
        public static readonly ChargingStationOperatorAdminStatusTypes InternalUse   = new("internalUse");

        #endregion


        #region Operator overloading

        #region Operator == (ChargingStationOperatorAdminStatusType1, ChargingStationOperatorAdminStatusType2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingStationOperatorAdminStatusType1">A charging station operator admin status type.</param>
        /// <param name="ChargingStationOperatorAdminStatusType2">Another charging station operator admin status type.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator == (ChargingStationOperatorAdminStatusTypes ChargingStationOperatorAdminStatusType1,
                                           ChargingStationOperatorAdminStatusTypes ChargingStationOperatorAdminStatusType2)

            => ChargingStationOperatorAdminStatusType1.Equals(ChargingStationOperatorAdminStatusType2);

        #endregion

        #region Operator != (ChargingStationOperatorAdminStatusType1, ChargingStationOperatorAdminStatusType2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingStationOperatorAdminStatusType1">A charging station operator admin status type.</param>
        /// <param name="ChargingStationOperatorAdminStatusType2">Another charging station operator admin status type.</param>
        /// <returns>False if both match; True otherwise.</returns>
        public static Boolean operator != (ChargingStationOperatorAdminStatusTypes ChargingStationOperatorAdminStatusType1,
                                           ChargingStationOperatorAdminStatusTypes ChargingStationOperatorAdminStatusType2)

            => !ChargingStationOperatorAdminStatusType1.Equals(ChargingStationOperatorAdminStatusType2);

        #endregion

        #region Operator <  (ChargingStationOperatorAdminStatusType1, ChargingStationOperatorAdminStatusType2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingStationOperatorAdminStatusType1">A charging station operator admin status type.</param>
        /// <param name="ChargingStationOperatorAdminStatusType2">Another charging station operator admin status type.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator < (ChargingStationOperatorAdminStatusTypes ChargingStationOperatorAdminStatusType1,
                                          ChargingStationOperatorAdminStatusTypes ChargingStationOperatorAdminStatusType2)

            => ChargingStationOperatorAdminStatusType1.CompareTo(ChargingStationOperatorAdminStatusType2) < 0;

        #endregion

        #region Operator <= (ChargingStationOperatorAdminStatusType1, ChargingStationOperatorAdminStatusType2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingStationOperatorAdminStatusType1">A charging station operator admin status type.</param>
        /// <param name="ChargingStationOperatorAdminStatusType2">Another charging station operator admin status type.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator <= (ChargingStationOperatorAdminStatusTypes ChargingStationOperatorAdminStatusType1,
                                           ChargingStationOperatorAdminStatusTypes ChargingStationOperatorAdminStatusType2)

            => ChargingStationOperatorAdminStatusType1.CompareTo(ChargingStationOperatorAdminStatusType2) <= 0;

        #endregion

        #region Operator >  (ChargingStationOperatorAdminStatusType1, ChargingStationOperatorAdminStatusType2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingStationOperatorAdminStatusType1">A charging station operator admin status type.</param>
        /// <param name="ChargingStationOperatorAdminStatusType2">Another charging station operator admin status type.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator > (ChargingStationOperatorAdminStatusTypes ChargingStationOperatorAdminStatusType1,
                                          ChargingStationOperatorAdminStatusTypes ChargingStationOperatorAdminStatusType2)

            => ChargingStationOperatorAdminStatusType1.CompareTo(ChargingStationOperatorAdminStatusType2) > 0;

        #endregion

        #region Operator >= (ChargingStationOperatorAdminStatusType1, ChargingStationOperatorAdminStatusType2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingStationOperatorAdminStatusType1">A charging station operator admin status type.</param>
        /// <param name="ChargingStationOperatorAdminStatusType2">Another charging station operator admin status type.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator >= (ChargingStationOperatorAdminStatusTypes ChargingStationOperatorAdminStatusType1,
                                           ChargingStationOperatorAdminStatusTypes ChargingStationOperatorAdminStatusType2)

            => ChargingStationOperatorAdminStatusType1.CompareTo(ChargingStationOperatorAdminStatusType2) >= 0;

        #endregion

        #endregion

        #region IComparable<ChargingStationOperatorAdminStatusTypes> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two charging station operator admin status types.
        /// </summary>
        /// <param name="Object">A charging station operator admin status type to compare with.</param>
        public Int32 CompareTo(Object? Object)

            => Object is ChargingStationOperatorAdminStatusTypes chargingStationOperatorAdminStatusType
                   ? CompareTo(chargingStationOperatorAdminStatusType)
                   : throw new ArgumentException("The given object is not a charging station operator admin status type!",
                                                 nameof(Object));

        #endregion

        #region CompareTo(ChargingStationOperatorAdminStatusType)

        /// <summary>
        /// Compares two charging station operator admin status types.
        /// </summary>
        /// <param name="ChargingStationOperatorAdminStatusType">A charging station operator admin status type to compare with.</param>
        public Int32 CompareTo(ChargingStationOperatorAdminStatusTypes ChargingStationOperatorAdminStatusType)

            => String.Compare(InternalId,
                              ChargingStationOperatorAdminStatusType.InternalId,
                              StringComparison.OrdinalIgnoreCase);

        #endregion

        #endregion

        #region IEquatable<ChargingStationOperatorAdminStatusTypes> Members

        #region Equals(Object)

        /// <summary>
        /// Compares two charging station operator admin status types for equality.
        /// </summary>
        /// <param name="Object">A charging station operator admin status type to compare with.</param>
        public override Boolean Equals(Object? Object)

            => Object is ChargingStationOperatorAdminStatusTypes chargingStationOperatorAdminStatusType &&
                   Equals(chargingStationOperatorAdminStatusType);

        #endregion

        #region Equals(ChargingStationOperatorAdminStatusType)

        /// <summary>
        /// Compares two charging station operator admin status types for equality.
        /// </summary>
        /// <param name="ChargingStationOperatorAdminStatusType">A charging station operator admin status type to compare with.</param>
        public Boolean Equals(ChargingStationOperatorAdminStatusTypes ChargingStationOperatorAdminStatusType)

            => String.Equals(InternalId,
                             ChargingStationOperatorAdminStatusType.InternalId,
                             StringComparison.OrdinalIgnoreCase);

        #endregion

        #endregion

        #region (override) GetHashCode()

        /// <summary>
        /// Return the hash code of this object.
        /// </summary>
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
