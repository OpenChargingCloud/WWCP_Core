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
    /// Extension methods for charging station admin status types.
    /// </summary>
    public static class ChargingStationAdminStatusTypesExtensions
    {

        /// <summary>
        /// Indicates whether this charging station admin status type is null or empty.
        /// </summary>
        /// <param name="ChargingStationAdminStatusType">A charging station admin status type.</param>
        public static Boolean IsNullOrEmpty(this ChargingStationAdminStatusType? ChargingStationAdminStatusType)
            => !ChargingStationAdminStatusType.HasValue || ChargingStationAdminStatusType.Value.IsNullOrEmpty;

        /// <summary>
        /// Indicates whether this charging station admin status type is null or empty.
        /// </summary>
        /// <param name="ChargingStationAdminStatusType">A charging station admin status type.</param>
        public static Boolean IsNotNullOrEmpty(this ChargingStationAdminStatusType? ChargingStationAdminStatusType)
            => ChargingStationAdminStatusType.HasValue && ChargingStationAdminStatusType.Value.IsNotNullOrEmpty;

    }


    /// <summary>
    /// The admin status of a charging station.
    /// </summary>
    public readonly struct ChargingStationAdminStatusType : IId,
                                                            IEquatable<ChargingStationAdminStatusType>,
                                                            IComparable<ChargingStationAdminStatusType>
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
        /// The length of the charging station admin status.
        /// </summary>
        public UInt64 Length
            => (UInt64) (InternalId?.Length ?? 0);

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new charging station admin status type based on the given string.
        /// </summary>
        private ChargingStationAdminStatusType(String Text)
        {
            InternalId = Text;
        }

        #endregion


        #region (static) Parse   (Text)

        /// <summary>
        /// Parse the given string as a charging station admin status type.
        /// </summary>
        /// <param name="Text">A text representation of a charging station admin status type.</param>
        public static ChargingStationAdminStatusType Parse(String Text)
        {

            if (TryParse(Text, out ChargingStationAdminStatusType chargingStationAdminStatusTypes))
                return chargingStationAdminStatusTypes;

            throw new ArgumentException($"Invalid text representation of a charging station admin status type: '" + Text + "'!",
                                        nameof(Text));

        }

        #endregion

        #region (static) TryParse(Text)

        /// <summary>
        /// Try to parse the given string as a charging station admin status type.
        /// </summary>
        /// <param name="Text">A text representation of a charging station admin status type.</param>
        public static ChargingStationAdminStatusType? TryParse(String Text)
        {

            if (TryParse(Text, out ChargingStationAdminStatusType chargingStationAdminStatusTypes))
                return chargingStationAdminStatusTypes;

            return null;

        }

        #endregion

        #region (static) TryParse(Text, out ChargingStationAdminStatusType)

        /// <summary>
        /// Parse the given string as a charging station admin status type.
        /// </summary>
        /// <param name="Text">A text representation of a charging station admin status type.</param>
        /// <param name="ChargingStationAdminStatusType">The parsed charging station admin status type.</param>
        public static Boolean TryParse(String Text, out ChargingStationAdminStatusType ChargingStationAdminStatusType)
        {

            Text = Text.Trim();

            if (Text.IsNotNullOrEmpty())
            {
                try
                {
                    ChargingStationAdminStatusType = new ChargingStationAdminStatusType(Text);
                    return true;
                }
                catch
                { }
            }

            ChargingStationAdminStatusType = default;
            return false;

        }

        #endregion

        #region Clone()

        /// <summary>
        /// Clone this charging station admin status type.
        /// </summary>
        public ChargingStationAdminStatusType Clone()

            => new (
                   InternalId.CloneString()
               );

        #endregion


        #region Static members

        /// <summary>
        /// Unclear admin status of the charging station.
        /// </summary>
        public static ChargingStationAdminStatusType  Unspecified     { get; }
            = new ("unspecified");

        /// <summary>
        /// Unknown admin status of the charging station.
        /// </summary>
        public static ChargingStationAdminStatusType  Unknown         { get; }
            = new ("unknown");

        /// <summary>
        /// The charging station is planned for the future.
        /// </summary>
        public static ChargingStationAdminStatusType  Planned         { get; }
            = new ("planned");

        /// <summary>
        /// The charging station is currently in deployment, but not fully operational yet.
        /// </summary>
        public static ChargingStationAdminStatusType  InDeployment    { get; }
            = new ("inDeployment");

        /// <summary>
        /// Private or internal use.
        /// </summary>
        public static ChargingStationAdminStatusType  InternalUse     { get; }
            = new ("internalUse");

        /// <summary>
        /// The charging station is under maintenance.
        /// </summary>
        public static ChargingStationAdminStatusType  OutOfService    { get; }
            = new ("outOfService");

        /// <summary>
        /// The charging station is operational.
        /// </summary>
        public static ChargingStationAdminStatusType  Operational     { get; }
            = new ("operational");

        #endregion


        #region Operator overloading

        #region Operator == (ChargingStationAdminStatusType1, ChargingStationAdminStatusType2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingStationAdminStatusType1">A charging station admin status type.</param>
        /// <param name="ChargingStationAdminStatusType2">Another charging station admin status type.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator == (ChargingStationAdminStatusType ChargingStationAdminStatusType1,
                                           ChargingStationAdminStatusType ChargingStationAdminStatusType2)

            => ChargingStationAdminStatusType1.Equals(ChargingStationAdminStatusType2);

        #endregion

        #region Operator != (ChargingStationAdminStatusType1, ChargingStationAdminStatusType2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingStationAdminStatusType1">A charging station admin status type.</param>
        /// <param name="ChargingStationAdminStatusType2">Another charging station admin status type.</param>
        /// <returns>False if both match; True otherwise.</returns>
        public static Boolean operator != (ChargingStationAdminStatusType ChargingStationAdminStatusType1,
                                           ChargingStationAdminStatusType ChargingStationAdminStatusType2)

            => !ChargingStationAdminStatusType1.Equals(ChargingStationAdminStatusType2);

        #endregion

        #region Operator <  (ChargingStationAdminStatusType1, ChargingStationAdminStatusType2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingStationAdminStatusType1">A charging station admin status type.</param>
        /// <param name="ChargingStationAdminStatusType2">Another charging station admin status type.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator < (ChargingStationAdminStatusType ChargingStationAdminStatusType1,
                                          ChargingStationAdminStatusType ChargingStationAdminStatusType2)

            => ChargingStationAdminStatusType1.CompareTo(ChargingStationAdminStatusType2) < 0;

        #endregion

        #region Operator <= (ChargingStationAdminStatusType1, ChargingStationAdminStatusType2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingStationAdminStatusType1">A charging station admin status type.</param>
        /// <param name="ChargingStationAdminStatusType2">Another charging station admin status type.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator <= (ChargingStationAdminStatusType ChargingStationAdminStatusType1,
                                           ChargingStationAdminStatusType ChargingStationAdminStatusType2)

            => ChargingStationAdminStatusType1.CompareTo(ChargingStationAdminStatusType2) <= 0;

        #endregion

        #region Operator >  (ChargingStationAdminStatusType1, ChargingStationAdminStatusType2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingStationAdminStatusType1">A charging station admin status type.</param>
        /// <param name="ChargingStationAdminStatusType2">Another charging station admin status type.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator > (ChargingStationAdminStatusType ChargingStationAdminStatusType1,
                                          ChargingStationAdminStatusType ChargingStationAdminStatusType2)

            => ChargingStationAdminStatusType1.CompareTo(ChargingStationAdminStatusType2) > 0;

        #endregion

        #region Operator >= (ChargingStationAdminStatusType1, ChargingStationAdminStatusType2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingStationAdminStatusType1">A charging station admin status type.</param>
        /// <param name="ChargingStationAdminStatusType2">Another charging station admin status type.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator >= (ChargingStationAdminStatusType ChargingStationAdminStatusType1,
                                           ChargingStationAdminStatusType ChargingStationAdminStatusType2)

            => ChargingStationAdminStatusType1.CompareTo(ChargingStationAdminStatusType2) >= 0;

        #endregion

        #endregion

        #region IComparable<ChargingStationAdminStatusTypes> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two charging station admin status types.
        /// </summary>
        /// <param name="Object">A charging station admin status type to compare with.</param>
        public Int32 CompareTo(Object? Object)

            => Object is ChargingStationAdminStatusType chargingStationAdminStatusTypes
                   ? CompareTo(chargingStationAdminStatusTypes)
                   : throw new ArgumentException("The given object is not a charging station admin status type!",
                                                 nameof(Object));

        #endregion

        #region CompareTo(ChargingStationAdminStatusType)

        /// <summary>
        /// Compares two charging station admin status types.
        /// </summary>
        /// <param name="ChargingStationAdminStatusType">A charging station admin status type to compare with.</param>
        public Int32 CompareTo(ChargingStationAdminStatusType ChargingStationAdminStatusType)

            => String.Compare(InternalId,
                              ChargingStationAdminStatusType.InternalId,
                              StringComparison.OrdinalIgnoreCase);

        #endregion

        #endregion

        #region IEquatable<ChargingStationAdminStatusTypes> Members

        #region Equals(Object)

        /// <summary>
        /// Compares two charging station admin status types for equality.
        /// </summary>
        /// <param name="Object">A charging station admin status type to compare with.</param>
        public override Boolean Equals(Object? Object)

            => Object is ChargingStationAdminStatusType chargingStationAdminStatusTypes &&
                   Equals(chargingStationAdminStatusTypes);

        #endregion

        #region Equals(ChargingStationAdminStatusType)

        /// <summary>
        /// Compares two charging station admin status types for equality.
        /// </summary>
        /// <param name="ChargingStationAdminStatusType">A charging station admin status type to compare with.</param>
        public Boolean Equals(ChargingStationAdminStatusType ChargingStationAdminStatusType)

            => String.Equals(InternalId,
                             ChargingStationAdminStatusType.InternalId,
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
