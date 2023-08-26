﻿/*
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
    /// Extension methods for charging pool admin status types.
    /// </summary>
    public static class ChargingPoolAdminStatusTypesExtensions
    {

        /// <summary>
        /// Indicates whether this charging pool admin status types is null or empty.
        /// </summary>
        /// <param name="ChargingPoolAdminStatusTypes">A charging pool admin status type.</param>
        public static Boolean IsNullOrEmpty(this ChargingPoolAdminStatusTypes? ChargingPoolAdminStatusTypes)
            => !ChargingPoolAdminStatusTypes.HasValue || ChargingPoolAdminStatusTypes.Value.IsNullOrEmpty;

        /// <summary>
        /// Indicates whether this charging pool admin status types is null or empty.
        /// </summary>
        /// <param name="ChargingPoolAdminStatusTypes">A charging pool admin status type.</param>
        public static Boolean IsNotNullOrEmpty(this ChargingPoolAdminStatusTypes? ChargingPoolAdminStatusTypes)
            => ChargingPoolAdminStatusTypes.HasValue && ChargingPoolAdminStatusTypes.Value.IsNotNullOrEmpty;

    }


    /// <summary>
    /// The admin status of a charging pool.
    /// </summary>
    public readonly struct ChargingPoolAdminStatusTypes : IId,
                                                          IEquatable<ChargingPoolAdminStatusTypes>,
                                                          IComparable<ChargingPoolAdminStatusTypes>
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
        /// The length of the charging pool admin status.
        /// </summary>
        public UInt64 Length
            => (UInt64) (InternalId?.Length ?? 0);

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new charging pool admin status type based on the given string.
        /// </summary>
        private ChargingPoolAdminStatusTypes(String Text)
        {
            InternalId = Text;
        }

        #endregion


        #region (static) Parse   (Text)

        /// <summary>
        /// Parse the given string as a charging pool admin status type.
        /// </summary>
        /// <param name="Text">A text representation of a charging pool admin status type.</param>
        public static ChargingPoolAdminStatusTypes Parse(String Text)
        {

            if (TryParse(Text, out ChargingPoolAdminStatusTypes chargingPoolAdminStatusTypes))
                return chargingPoolAdminStatusTypes;

            throw new ArgumentException($"Invalid text representation of a charging pool admin status type: '" + Text + "'!",
                                        nameof(Text));

        }

        #endregion

        #region (static) TryParse(Text)

        /// <summary>
        /// Try to parse the given string as a charging pool admin status type.
        /// </summary>
        /// <param name="Text">A text representation of a charging pool admin status type.</param>
        public static ChargingPoolAdminStatusTypes? TryParse(String Text)
        {

            if (TryParse(Text, out ChargingPoolAdminStatusTypes chargingPoolAdminStatusTypes))
                return chargingPoolAdminStatusTypes;

            return null;

        }

        #endregion

        #region (static) TryParse(Text, out ChargingPoolAdminStatusType)

        /// <summary>
        /// Parse the given string as a charging pool admin status type.
        /// </summary>
        /// <param name="Text">A text representation of a charging pool admin status type.</param>
        /// <param name="ChargingPoolAdminStatusType">The parsed charging pool admin status type.</param>
        public static Boolean TryParse(String Text, out ChargingPoolAdminStatusTypes ChargingPoolAdminStatusType)
        {

            Text = Text.Trim();

            if (Text.IsNotNullOrEmpty())
            {
                try
                {
                    ChargingPoolAdminStatusType = new ChargingPoolAdminStatusTypes(Text);
                    return true;
                }
                catch
                { }
            }

            ChargingPoolAdminStatusType = default;
            return false;

        }

        #endregion

        #region Clone

        /// <summary>
        /// Clone this charging pool admin status type.
        /// </summary>
        public ChargingPoolAdminStatusTypes Clone

            => new(
                   new String(InternalId?.ToCharArray())
               );

        #endregion


        #region Static members

        /// <summary>
        /// Unclear admin status of the charging pool.
        /// </summary>
        public static readonly ChargingPoolAdminStatusTypes Unspecified   = new("unspecified");

        /// <summary>
        /// Unkown admin status of the charging pool.
        /// </summary>
        public static readonly ChargingPoolAdminStatusTypes Unkown        = new("unkown");

        /// <summary>
        /// The charging pool is planned for the future.
        /// </summary>
        public static readonly ChargingPoolAdminStatusTypes Planned       = new("planned");

        /// <summary>
        /// The charging pool is currently in deployment, but not fully operational yet.
        /// </summary>
        public static readonly ChargingPoolAdminStatusTypes InDeployment  = new("inDeployment");

        /// <summary>
        /// Private or internal use.
        /// </summary>
        public static readonly ChargingPoolAdminStatusTypes InternalUse   = new("internalUse");

        /// <summary>
        /// The charging pool is under maintenance.
        /// </summary>
        public static readonly ChargingPoolAdminStatusTypes OutOfService  = new("outOfService");

        /// <summary>
        /// The charging pool is operational.
        /// </summary>
        public static readonly ChargingPoolAdminStatusTypes Operational   = new("operational");

        #endregion


        #region Operator overloading

        #region Operator == (ChargingPoolAdminStatusType1, ChargingPoolAdminStatusType2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingPoolAdminStatusType1">A charging pool admin status type.</param>
        /// <param name="ChargingPoolAdminStatusType2">Another charging pool admin status type.</param>
        /// <returns>true|false</returns>
        public static Boolean operator == (ChargingPoolAdminStatusTypes ChargingPoolAdminStatusType1,
                                           ChargingPoolAdminStatusTypes ChargingPoolAdminStatusType2)

            => ChargingPoolAdminStatusType1.Equals(ChargingPoolAdminStatusType2);

        #endregion

        #region Operator != (ChargingPoolAdminStatusType1, ChargingPoolAdminStatusType2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingPoolAdminStatusType1">A charging pool admin status type.</param>
        /// <param name="ChargingPoolAdminStatusType2">Another charging pool admin status type.</param>
        /// <returns>true|false</returns>
        public static Boolean operator != (ChargingPoolAdminStatusTypes ChargingPoolAdminStatusType1,
                                           ChargingPoolAdminStatusTypes ChargingPoolAdminStatusType2)

            => !ChargingPoolAdminStatusType1.Equals(ChargingPoolAdminStatusType2);

        #endregion

        #region Operator <  (ChargingPoolAdminStatusType1, ChargingPoolAdminStatusType2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingPoolAdminStatusType1">A charging pool admin status type.</param>
        /// <param name="ChargingPoolAdminStatusType2">Another charging pool admin status type.</param>
        /// <returns>true|false</returns>
        public static Boolean operator < (ChargingPoolAdminStatusTypes ChargingPoolAdminStatusType1,
                                          ChargingPoolAdminStatusTypes ChargingPoolAdminStatusType2)

            => ChargingPoolAdminStatusType1.CompareTo(ChargingPoolAdminStatusType2) < 0;

        #endregion

        #region Operator <= (ChargingPoolAdminStatusType1, ChargingPoolAdminStatusType2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingPoolAdminStatusType1">A charging pool admin status type.</param>
        /// <param name="ChargingPoolAdminStatusType2">Another charging pool admin status type.</param>
        /// <returns>true|false</returns>
        public static Boolean operator <= (ChargingPoolAdminStatusTypes ChargingPoolAdminStatusType1,
                                           ChargingPoolAdminStatusTypes ChargingPoolAdminStatusType2)

            => ChargingPoolAdminStatusType1.CompareTo(ChargingPoolAdminStatusType2) <= 0;

        #endregion

        #region Operator >  (ChargingPoolAdminStatusType1, ChargingPoolAdminStatusType2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingPoolAdminStatusType1">A charging pool admin status type.</param>
        /// <param name="ChargingPoolAdminStatusType2">Another charging pool admin status type.</param>
        /// <returns>true|false</returns>
        public static Boolean operator > (ChargingPoolAdminStatusTypes ChargingPoolAdminStatusType1,
                                          ChargingPoolAdminStatusTypes ChargingPoolAdminStatusType2)

            => ChargingPoolAdminStatusType1.CompareTo(ChargingPoolAdminStatusType2) > 0;

        #endregion

        #region Operator >= (ChargingPoolAdminStatusType1, ChargingPoolAdminStatusType2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingPoolAdminStatusType1">A charging pool admin status type.</param>
        /// <param name="ChargingPoolAdminStatusType2">Another charging pool admin status type.</param>
        /// <returns>true|false</returns>
        public static Boolean operator >= (ChargingPoolAdminStatusTypes ChargingPoolAdminStatusType1,
                                           ChargingPoolAdminStatusTypes ChargingPoolAdminStatusType2)

            => ChargingPoolAdminStatusType1.CompareTo(ChargingPoolAdminStatusType2) >= 0;

        #endregion

        #endregion

        #region IComparable<ChargingPoolAdminStatusTypes> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two charging pool admin status types.
        /// </summary>
        /// <param name="Object">A charging pool admin status type to compare with.</param>
        public Int32 CompareTo(Object? Object)

            => Object is ChargingPoolAdminStatusTypes chargingPoolAdminStatusTypes
                   ? CompareTo(chargingPoolAdminStatusTypes)
                   : throw new ArgumentException("The given object is not a charging pool admin status type!",
                                                 nameof(Object));

        #endregion

        #region CompareTo(ChargingPoolAdminStatusType)

        /// <summary>
        /// Compares two charging pool admin status types.
        /// </summary>
        /// <param name="ChargingPoolAdminStatusType">A charging pool admin status type to compare with.</param>
        public Int32 CompareTo(ChargingPoolAdminStatusTypes ChargingPoolAdminStatusType)

            => String.Compare(InternalId,
                              ChargingPoolAdminStatusType.InternalId,
                              StringComparison.OrdinalIgnoreCase);

        #endregion

        #endregion

        #region IEquatable<ChargingPoolAdminStatusTypes> Members

        #region Equals(Object)

        /// <summary>
        /// Compares two charging pool admin status types for equality.
        /// </summary>
        /// <param name="Object">A charging pool admin status type to compare with.</param>
        public override Boolean Equals(Object? Object)

            => Object is ChargingPoolAdminStatusTypes chargingPoolAdminStatusTypes &&
                   Equals(chargingPoolAdminStatusTypes);

        #endregion

        #region Equals(ChargingPoolAdminStatusType)

        /// <summary>
        /// Compares two charging pool admin status types for equality.
        /// </summary>
        /// <param name="ChargingPoolAdminStatusType">A charging pool admin status type to compare with.</param>
        public Boolean Equals(ChargingPoolAdminStatusTypes ChargingPoolAdminStatusType)

            => String.Equals(InternalId,
                             ChargingPoolAdminStatusType.InternalId,
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
