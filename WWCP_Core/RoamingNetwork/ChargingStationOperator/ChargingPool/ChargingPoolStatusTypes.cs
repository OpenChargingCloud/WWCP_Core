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
    /// Extension methods for charging pool status types.
    /// </summary>
    public static class ChargingPoolStatusTypesExtensions
    {

        /// <summary>
        /// Indicates whether this charging pool status types is null or empty.
        /// </summary>
        /// <param name="ChargingPoolStatusTypes">A charging pool status type.</param>
        public static Boolean IsNullOrEmpty(this ChargingPoolStatusTypes? ChargingPoolStatusTypes)
            => !ChargingPoolStatusTypes.HasValue || ChargingPoolStatusTypes.Value.IsNullOrEmpty;

        /// <summary>
        /// Indicates whether this charging pool status types is null or empty.
        /// </summary>
        /// <param name="ChargingPoolStatusTypes">A charging pool status type.</param>
        public static Boolean IsNotNullOrEmpty(this ChargingPoolStatusTypes? ChargingPoolStatusTypes)
            => ChargingPoolStatusTypes.HasValue && ChargingPoolStatusTypes.Value.IsNotNullOrEmpty;

    }


    /// <summary>
    /// The status of a charging pool.
    /// </summary>
    public readonly struct ChargingPoolStatusTypes : IId,
                                                     IEquatable<ChargingPoolStatusTypes>,
                                                     IComparable<ChargingPoolStatusTypes>

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
        /// The length of the charging pool status.
        /// </summary>
        public UInt64 Length
            => (UInt64) InternalId.Length;

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new charging pool status type based on the given string.
        /// </summary>
        private ChargingPoolStatusTypes(String Text)
        {
            InternalId = Text;
        }

        #endregion


        #region (static) Parse   (Text)

        /// <summary>
        /// Parse the given string as a charging pool status type.
        /// </summary>
        /// <param name="Text">A text representation of a charging pool status type.</param>
        public static ChargingPoolStatusTypes Parse(String Text)
        {

            if (TryParse(Text, out ChargingPoolStatusTypes chargingPoolStatusTypes))
                return chargingPoolStatusTypes;

            throw new ArgumentException("Invalid text-representation of a charging pool status type: '" + Text + "'!",
                                        nameof(Text));

        }

        #endregion

        #region (static) TryParse(Text)

        /// <summary>
        /// Try to parse the given string as a charging pool status type.
        /// </summary>
        /// <param name="Text">A text representation of a charging pool status type.</param>
        public static ChargingPoolStatusTypes? TryParse(String Text)
        {

            if (TryParse(Text, out ChargingPoolStatusTypes chargingPoolStatusTypes))
                return chargingPoolStatusTypes;

            return null;

        }

        #endregion

        #region (static) TryParse(Text, out ChargingPoolStatusType)

        /// <summary>
        /// Parse the given string as a charging pool status type.
        /// </summary>
        /// <param name="Text">A text representation of a charging pool status type.</param>
        /// <param name="ChargingPoolStatusType">The parsed charging pool status type.</param>
        public static Boolean TryParse(String Text, out ChargingPoolStatusTypes ChargingPoolStatusType)
        {

            Text = Text.Trim();

            if (Text.IsNotNullOrEmpty())
            {
                try
                {
                    ChargingPoolStatusType = new ChargingPoolStatusTypes(Text);
                    return true;
                }
                catch
                { }
            }

            ChargingPoolStatusType = default;
            return false;

        }

        #endregion

        #region Clone

        /// <summary>
        /// Clone this charging pool status type.
        /// </summary>
        public ChargingPoolStatusTypes Clone

            => new(
                   new String(InternalId?.ToCharArray())
               );

        #endregion


        #region Static members

        /// <summary>
        /// Unknown status of the charging pool.
        /// </summary>
        public static readonly ChargingPoolStatusTypes Unknown           = new("unknown");

        /// <summary>
        /// Unclear status of the charging pool.
        /// </summary>
        public static readonly ChargingPoolStatusTypes Unspecified       = new("unspecified");

        /// <summary>
        /// The charging pool is currently offline.
        /// </summary>
        public static readonly ChargingPoolStatusTypes Offline           = new("offline");

        /// <summary>
        /// The charging pool is not fully operational yet.
        /// </summary>
        public static readonly ChargingPoolStatusTypes InDeployment      = new("inDeployment");

        /// <summary>
        /// Some ongoing charging sessions or reservations, but still ready to charge.
        /// </summary>
        public static readonly ChargingPoolStatusTypes PartialAvailable  = new("partialAvailable");

        /// <summary>
        /// The charging pool is available.
        /// </summary>
        public static readonly ChargingPoolStatusTypes Available         = new("available");

        /// <summary>
        /// The entire charging pool was reserved by an ev customer.
        /// </summary>
        public static readonly ChargingPoolStatusTypes Reserved          = new("reserved");

        /// <summary>
        /// The entire charging pool is charging. Currently no additional charging sessions are possible.
        /// </summary>
        public static readonly ChargingPoolStatusTypes Charging          = new("charging");

        /// <summary>
        /// A fatal error has occured within the charging pool.
        /// </summary>
        public static readonly ChargingPoolStatusTypes Error             = new("error");

        #endregion


        #region Operator overloading

        #region Operator == (ChargingPoolStatusType1, ChargingPoolStatusType2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingPoolStatusType1">A charging pool status type.</param>
        /// <param name="ChargingPoolStatusType2">Another charging pool status type.</param>
        /// <returns>true|false</returns>
        public static Boolean operator == (ChargingPoolStatusTypes ChargingPoolStatusType1,
                                           ChargingPoolStatusTypes ChargingPoolStatusType2)

            => ChargingPoolStatusType1.Equals(ChargingPoolStatusType2);

        #endregion

        #region Operator != (ChargingPoolStatusType1, ChargingPoolStatusType2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingPoolStatusType1">A charging pool status type.</param>
        /// <param name="ChargingPoolStatusType2">Another charging pool status type.</param>
        /// <returns>true|false</returns>
        public static Boolean operator != (ChargingPoolStatusTypes ChargingPoolStatusType1,
                                           ChargingPoolStatusTypes ChargingPoolStatusType2)

            => !ChargingPoolStatusType1.Equals(ChargingPoolStatusType2);

        #endregion

        #region Operator <  (ChargingPoolStatusType1, ChargingPoolStatusType2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingPoolStatusType1">A charging pool status type.</param>
        /// <param name="ChargingPoolStatusType2">Another charging pool status type.</param>
        /// <returns>true|false</returns>
        public static Boolean operator < (ChargingPoolStatusTypes ChargingPoolStatusType1,
                                          ChargingPoolStatusTypes ChargingPoolStatusType2)

            => ChargingPoolStatusType1.CompareTo(ChargingPoolStatusType2) < 0;

        #endregion

        #region Operator <= (ChargingPoolStatusType1, ChargingPoolStatusType2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingPoolStatusType1">A charging pool status type.</param>
        /// <param name="ChargingPoolStatusType2">Another charging pool status type.</param>
        /// <returns>true|false</returns>
        public static Boolean operator <= (ChargingPoolStatusTypes ChargingPoolStatusType1,
                                           ChargingPoolStatusTypes ChargingPoolStatusType2)

            => ChargingPoolStatusType1.CompareTo(ChargingPoolStatusType2) <= 0;

        #endregion

        #region Operator >  (ChargingPoolStatusType1, ChargingPoolStatusType2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingPoolStatusType1">A charging pool status type.</param>
        /// <param name="ChargingPoolStatusType2">Another charging pool status type.</param>
        /// <returns>true|false</returns>
        public static Boolean operator > (ChargingPoolStatusTypes ChargingPoolStatusType1,
                                          ChargingPoolStatusTypes ChargingPoolStatusType2)

            => ChargingPoolStatusType1.CompareTo(ChargingPoolStatusType2) > 0;

        #endregion

        #region Operator >= (ChargingPoolStatusType1, ChargingPoolStatusType2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingPoolStatusType1">A charging pool status type.</param>
        /// <param name="ChargingPoolStatusType2">Another charging pool status type.</param>
        /// <returns>true|false</returns>
        public static Boolean operator >= (ChargingPoolStatusTypes ChargingPoolStatusType1,
                                           ChargingPoolStatusTypes ChargingPoolStatusType2)

            => ChargingPoolStatusType1.CompareTo(ChargingPoolStatusType2) >= 0;

        #endregion

        #endregion

        #region IComparable<ChargingPoolStatusTypes> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two charging pool status types.
        /// </summary>
        /// <param name="Object">A charging pool status type to compare with.</param>
        public Int32 CompareTo(Object? Object)

            => Object is ChargingPoolStatusTypes chargingPoolStatusTypes
                   ? CompareTo(chargingPoolStatusTypes)
                   : throw new ArgumentException("The given object is not a charging pool status type!",
                                                 nameof(Object));

        #endregion

        #region CompareTo(ChargingPoolStatusType)

        /// <summary>
        /// Compares two charging pool status types.
        /// </summary>
        /// <param name="ChargingPoolStatusType">A charging pool status type to compare with.</param>
        public Int32 CompareTo(ChargingPoolStatusTypes ChargingPoolStatusType)

            => String.Compare(InternalId,
                              ChargingPoolStatusType.InternalId,
                              StringComparison.OrdinalIgnoreCase);

        #endregion

        #endregion

        #region IEquatable<ChargingPoolStatusTypes> Members

        #region Equals(Object)

        /// <summary>
        /// Compares two charging pool status types for equality.
        /// </summary>
        /// <param name="Object">A charging pool status type to compare with.</param>
        public override Boolean Equals(Object? Object)

            => Object is ChargingPoolStatusTypes chargingPoolStatusTypes &&
                   Equals(chargingPoolStatusTypes);

        #endregion

        #region Equals(ChargingPoolStatusType)

        /// <summary>
        /// Compares two charging pool status types for equality.
        /// </summary>
        /// <param name="ChargingPoolStatusType">A charging pool status type to compare with.</param>
        public Boolean Equals(ChargingPoolStatusTypes ChargingPoolStatusType)

            => String.Equals(InternalId,
                             ChargingPoolStatusType.InternalId,
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
