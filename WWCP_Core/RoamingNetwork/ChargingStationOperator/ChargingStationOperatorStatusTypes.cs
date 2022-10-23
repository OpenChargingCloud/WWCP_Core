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
    /// Extension methods for charging station operator status types.
    /// </summary>
    public static class ChargingStationOperatorStatusTypesExtensions
    {

        /// <summary>
        /// Indicates whether this charging station operator status types is null or empty.
        /// </summary>
        /// <param name="ChargingStationOperatorStatusTypes">A charging station operator status type.</param>
        public static Boolean IsNullOrEmpty(this ChargingStationOperatorStatusTypes? ChargingStationOperatorStatusTypes)
            => !ChargingStationOperatorStatusTypes.HasValue || ChargingStationOperatorStatusTypes.Value.IsNullOrEmpty;

        /// <summary>
        /// Indicates whether this charging station operator status types is null or empty.
        /// </summary>
        /// <param name="ChargingStationOperatorStatusTypes">A charging station operator status type.</param>
        public static Boolean IsNotNullOrEmpty(this ChargingStationOperatorStatusTypes? ChargingStationOperatorStatusTypes)
            => ChargingStationOperatorStatusTypes.HasValue && ChargingStationOperatorStatusTypes.Value.IsNotNullOrEmpty;

    }


    /// <summary>
    /// The status of a charging station operator.
    /// </summary>
    public readonly struct ChargingStationOperatorStatusTypes : IId,
                                                                IEquatable<ChargingStationOperatorStatusTypes>,
                                                                IComparable<ChargingStationOperatorStatusTypes>
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
        /// The length of the charging station operator status.
        /// </summary>
        public UInt64 Length
            => (UInt64) InternalId.Length;

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new charging station operator status type based on the given string.
        /// </summary>
        private ChargingStationOperatorStatusTypes(String Text)
        {
            InternalId = Text;
        }

        #endregion


        #region (static) Parse   (Text)

        /// <summary>
        /// Parse the given string as a charging station operator status type.
        /// </summary>
        /// <param name="Text">A text representation of a charging station operator status type.</param>
        public static ChargingStationOperatorStatusTypes Parse(String Text)
        {

            if (TryParse(Text, out ChargingStationOperatorStatusTypes chargingStationOperatorStatusType))
                return chargingStationOperatorStatusType;

            throw new ArgumentException("Invalid text-representation of a charging station operator status type: '" + Text + "'!",
                                        nameof(Text));

        }

        #endregion

        #region (static) TryParse(Text)

        /// <summary>
        /// Try to parse the given string as a charging station operator status type.
        /// </summary>
        /// <param name="Text">A text representation of a charging station operator status type.</param>
        public static ChargingStationOperatorStatusTypes? TryParse(String Text)
        {

            if (TryParse(Text, out ChargingStationOperatorStatusTypes chargingStationOperatorStatusType))
                return chargingStationOperatorStatusType;

            return null;

        }

        #endregion

        #region (static) TryParse(Text, out ChargingStationOperatorStatusType)

        /// <summary>
        /// Parse the given string as a charging station operator status type.
        /// </summary>
        /// <param name="Text">A text representation of a charging station operator status type.</param>
        /// <param name="ChargingStationOperatorStatusType">The parsed charging station operator status type.</param>
        public static Boolean TryParse(String Text, out ChargingStationOperatorStatusTypes ChargingStationOperatorStatusType)
        {

            Text = Text.Trim();

            if (Text.IsNotNullOrEmpty())
            {
                try
                {
                    ChargingStationOperatorStatusType = new ChargingStationOperatorStatusTypes(Text);
                    return true;
                }
                catch
                { }
            }

            ChargingStationOperatorStatusType = default;
            return false;

        }

        #endregion

        #region Clone

        /// <summary>
        /// Clone this charging station operator status type.
        /// </summary>
        public ChargingStationOperatorStatusTypes Clone

            => new(
                   new String(InternalId?.ToCharArray())
               );

        #endregion


        #region Static members

        /// <summary>
        /// Unknown status of the charging station operator.
        /// </summary>
        public static readonly ChargingStationOperatorStatusTypes Unknown       = new("unknown");

        /// <summary>
        /// Unclear status of the charging station operator.
        /// </summary>
        public static readonly ChargingStationOperatorStatusTypes Unspecified   = new("unspecified");

        /// <summary>
        /// The charging station operator is currently offline.
        /// </summary>
        public static readonly ChargingStationOperatorStatusTypes Offline       = new("offline");

        /// <summary>
        /// The charging station operator is not fully operational yet.
        /// </summary>
        public static readonly ChargingStationOperatorStatusTypes InDeployment  = new("inDeployment");

        /// <summary>
        /// The charging station operator is available.
        /// </summary>
        public static readonly ChargingStationOperatorStatusTypes Available     = new("available");

        /// <summary>
        /// A fatal error has occured within the charging station operator.
        /// </summary>
        public static readonly ChargingStationOperatorStatusTypes Error         = new("error");

        #endregion


        #region Operator overloading

        #region Operator == (ChargingStationOperatorStatusType1, ChargingStationOperatorStatusType2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingStationOperatorStatusType1">A charging station operator status type.</param>
        /// <param name="ChargingStationOperatorStatusType2">Another charging station operator status type.</param>
        /// <returns>true|false</returns>
        public static Boolean operator == (ChargingStationOperatorStatusTypes ChargingStationOperatorStatusType1,
                                           ChargingStationOperatorStatusTypes ChargingStationOperatorStatusType2)

            => ChargingStationOperatorStatusType1.Equals(ChargingStationOperatorStatusType2);

        #endregion

        #region Operator != (ChargingStationOperatorStatusType1, ChargingStationOperatorStatusType2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingStationOperatorStatusType1">A charging station operator status type.</param>
        /// <param name="ChargingStationOperatorStatusType2">Another charging station operator status type.</param>
        /// <returns>true|false</returns>
        public static Boolean operator != (ChargingStationOperatorStatusTypes ChargingStationOperatorStatusType1,
                                           ChargingStationOperatorStatusTypes ChargingStationOperatorStatusType2)

            => !ChargingStationOperatorStatusType1.Equals(ChargingStationOperatorStatusType2);

        #endregion

        #region Operator <  (ChargingStationOperatorStatusType1, ChargingStationOperatorStatusType2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingStationOperatorStatusType1">A charging station operator status type.</param>
        /// <param name="ChargingStationOperatorStatusType2">Another charging station operator status type.</param>
        /// <returns>true|false</returns>
        public static Boolean operator < (ChargingStationOperatorStatusTypes ChargingStationOperatorStatusType1,
                                          ChargingStationOperatorStatusTypes ChargingStationOperatorStatusType2)

            => ChargingStationOperatorStatusType1.CompareTo(ChargingStationOperatorStatusType2) < 0;

        #endregion

        #region Operator <= (ChargingStationOperatorStatusType1, ChargingStationOperatorStatusType2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingStationOperatorStatusType1">A charging station operator status type.</param>
        /// <param name="ChargingStationOperatorStatusType2">Another charging station operator status type.</param>
        /// <returns>true|false</returns>
        public static Boolean operator <= (ChargingStationOperatorStatusTypes ChargingStationOperatorStatusType1,
                                           ChargingStationOperatorStatusTypes ChargingStationOperatorStatusType2)

            => ChargingStationOperatorStatusType1.CompareTo(ChargingStationOperatorStatusType2) <= 0;

        #endregion

        #region Operator >  (ChargingStationOperatorStatusType1, ChargingStationOperatorStatusType2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingStationOperatorStatusType1">A charging station operator status type.</param>
        /// <param name="ChargingStationOperatorStatusType2">Another charging station operator status type.</param>
        /// <returns>true|false</returns>
        public static Boolean operator > (ChargingStationOperatorStatusTypes ChargingStationOperatorStatusType1,
                                          ChargingStationOperatorStatusTypes ChargingStationOperatorStatusType2)

            => ChargingStationOperatorStatusType1.CompareTo(ChargingStationOperatorStatusType2) > 0;

        #endregion

        #region Operator >= (ChargingStationOperatorStatusType1, ChargingStationOperatorStatusType2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingStationOperatorStatusType1">A charging station operator status type.</param>
        /// <param name="ChargingStationOperatorStatusType2">Another charging station operator status type.</param>
        /// <returns>true|false</returns>
        public static Boolean operator >= (ChargingStationOperatorStatusTypes ChargingStationOperatorStatusType1,
                                           ChargingStationOperatorStatusTypes ChargingStationOperatorStatusType2)

            => ChargingStationOperatorStatusType1.CompareTo(ChargingStationOperatorStatusType2) >= 0;

        #endregion

        #endregion

        #region IComparable<ChargingStationOperatorStatusTypes> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two charging station operator status types.
        /// </summary>
        /// <param name="Object">A charging station operator status type to compare with.</param>
        public Int32 CompareTo(Object? Object)

            => Object is ChargingStationOperatorStatusTypes chargingStationOperatorStatusType
                   ? CompareTo(chargingStationOperatorStatusType)
                   : throw new ArgumentException("The given object is not a charging station operator status type!",
                                                 nameof(Object));

        #endregion

        #region CompareTo(ChargingStationOperatorStatusType)

        /// <summary>
        /// Compares two charging station operator status types.
        /// </summary>
        /// <param name="ChargingStationOperatorStatusType">A charging station operator status type to compare with.</param>
        public Int32 CompareTo(ChargingStationOperatorStatusTypes ChargingStationOperatorStatusType)

            => String.Compare(InternalId,
                              ChargingStationOperatorStatusType.InternalId,
                              StringComparison.OrdinalIgnoreCase);

        #endregion

        #endregion

        #region IEquatable<ChargingStationOperatorStatusTypes> Members

        #region Equals(Object)

        /// <summary>
        /// Compares two charging station operator status types for equality.
        /// </summary>
        /// <param name="Object">A charging station operator status type to compare with.</param>
        public override Boolean Equals(Object? Object)

            => Object is ChargingStationOperatorStatusTypes chargingStationOperatorStatusType &&
                   Equals(chargingStationOperatorStatusType);

        #endregion

        #region Equals(ChargingStationOperatorStatusType)

        /// <summary>
        /// Compares two charging station operator status types for equality.
        /// </summary>
        /// <param name="ChargingStationOperatorStatusType">A charging station operator status type to compare with.</param>
        public Boolean Equals(ChargingStationOperatorStatusTypes ChargingStationOperatorStatusType)

            => String.Equals(InternalId,
                             ChargingStationOperatorStatusType.InternalId,
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
