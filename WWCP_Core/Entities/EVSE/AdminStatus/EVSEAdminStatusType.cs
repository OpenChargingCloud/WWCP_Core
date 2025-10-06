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
    /// Extension methods for EVSE admin status types.
    /// </summary>
    public static class EVSEAdminStatusTypesExtensions
    {

        /// <summary>
        /// Indicates whether this EVSE admin status types is null or empty.
        /// </summary>
        /// <param name="EVSEAdminStatusType">An EVSE admin status type.</param>
        public static Boolean IsNullOrEmpty(this EVSEAdminStatusType? EVSEAdminStatusType)
            => !EVSEAdminStatusType.HasValue || EVSEAdminStatusType.Value.IsNullOrEmpty;

        /// <summary>
        /// Indicates whether this EVSE admin status types is null or empty.
        /// </summary>
        /// <param name="EVSEAdminStatusType">An EVSE admin status type.</param>
        public static Boolean IsNotNullOrEmpty(this EVSEAdminStatusType? EVSEAdminStatusType)
            => EVSEAdminStatusType.HasValue && EVSEAdminStatusType.Value.IsNotNullOrEmpty;

    }


    /// <summary>
    /// The admin status type of an EVSE.
    /// </summary>
    public readonly struct EVSEAdminStatusType : IId,
                                                 IEquatable <EVSEAdminStatusType>,
                                                 IComparable<EVSEAdminStatusType>
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
        public Boolean  IsNullOrEmpty
            => InternalId.IsNullOrEmpty();

        /// <summary>
        /// Indicates whether this identification is NOT null or empty.
        /// </summary>
        public Boolean  IsNotNullOrEmpty
            => InternalId.IsNotNullOrEmpty();

        /// <summary>
        /// The length of the EVSE admin status.
        /// </summary>
        public UInt64   Length
            => (UInt64) (InternalId?.Length ?? 0);

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new EVSE admin status type based on the given string.
        /// </summary>
        private EVSEAdminStatusType(String Text)
        {
            InternalId = Text;
        }

        #endregion


        #region (static) Parse   (Text)

        /// <summary>
        /// Parse the given string as an EVSE admin status type.
        /// </summary>
        /// <param name="Text">A text representation of an EVSE admin status type.</param>
        public static EVSEAdminStatusType Parse(String Text)
        {

            if (TryParse(Text, out var evseAdminStatusType))
                return evseAdminStatusType;

            throw new ArgumentException($"Invalid text representation of an EVSE admin status type: '" + Text + "'!",
                                        nameof(Text));

        }

        #endregion

        #region (static) TryParse(Text)

        /// <summary>
        /// Try to parse the given string as an EVSE admin status type.
        /// </summary>
        /// <param name="Text">A text representation of an EVSE admin status type.</param>
        public static EVSEAdminStatusType? TryParse(String Text)
        {

            if (TryParse(Text, out var evseAdminStatusType))
                return evseAdminStatusType;

            return null;

        }

        #endregion

        #region (static) TryParse(Text, out EVSEAdminStatusType)

        /// <summary>
        /// Parse the given string as an EVSE admin status type.
        /// </summary>
        /// <param name="Text">A text representation of an EVSE admin status type.</param>
        /// <param name="EVSEAdminStatusType">The parsed EVSE admin status type.</param>
        public static Boolean TryParse(String Text, out EVSEAdminStatusType EVSEAdminStatusType)
        {

            Text = Text.Trim();

            if (Text.IsNotNullOrEmpty())
            {
                try
                {
                    EVSEAdminStatusType = new EVSEAdminStatusType(Text);
                    return true;
                }
                catch
                { }
            }

            EVSEAdminStatusType = default;
            return false;

        }

        #endregion

        #region Clone()

        /// <summary>
        /// Clone this EVSE admin status type.
        /// </summary>
        public EVSEAdminStatusType Clone()

            => new (
                   InternalId.CloneString()
               );

        #endregion


        #region Static definitions

        /// <summary>
        /// Unknown admin status of the EVSE.
        /// </summary>
        public static readonly EVSEAdminStatusType Unknown        = new("unknown");

        /// <summary>
        /// Unclear admin status of the EVSE.
        /// </summary>
        public static readonly EVSEAdminStatusType Unspecified   = new("unspecified");

        /// <summary>
        /// Planned for the future.
        /// </summary>
        public static readonly EVSEAdminStatusType Planned       = new("planned");

        /// <summary>
        /// The EVSE is currently in deployment, but not fully operational yet.
        /// </summary>
        public static readonly EVSEAdminStatusType InDeployment  = new("inDeployment");

        /// <summary>
        /// The EVSE is under maintenance.
        /// </summary>
        public static readonly EVSEAdminStatusType OutOfService  = new("outOfService");

        /// <summary>
        /// The EVSE not accessible because of a physical barrier,
        /// i.e. a car, a construction area or a city festival in front
        /// of the EVSE.
        /// </summary>
        public static readonly EVSEAdminStatusType Blocked       = new("blocked");

        /// <summary>
        /// The EVSE is operational.
        /// </summary>
        public static readonly EVSEAdminStatusType Operational   = new("operational");

        /// <summary>
        /// The EVSE does no longer exist.
        /// </summary>
        public static readonly EVSEAdminStatusType Deleted       = new("deleted");

        /// <summary>
        /// Private or internal use.
        /// </summary>
        public static readonly EVSEAdminStatusType InternalUse   = new("internalUse");

        /// <summary>
        /// The EVSE was not found!
        /// (Only valid within batch-processing)
        /// </summary>
        public static readonly EVSEAdminStatusType UnknownEVSE   = new("unknownEVSE");

        #endregion


        #region Operator overloading

        #region Operator == (EVSEAdminStatusType1, EVSEAdminStatusType2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EVSEAdminStatusType1">An EVSE admin status type.</param>
        /// <param name="EVSEAdminStatusType2">Another EVSE admin status type.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator == (EVSEAdminStatusType EVSEAdminStatusType1,
                                           EVSEAdminStatusType EVSEAdminStatusType2)

            => EVSEAdminStatusType1.Equals(EVSEAdminStatusType2);

        #endregion

        #region Operator != (EVSEAdminStatusType1, EVSEAdminStatusType2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EVSEAdminStatusType1">An EVSE admin status type.</param>
        /// <param name="EVSEAdminStatusType2">Another EVSE admin status type.</param>
        /// <returns>False if both match; True otherwise.</returns>
        public static Boolean operator != (EVSEAdminStatusType EVSEAdminStatusType1,
                                           EVSEAdminStatusType EVSEAdminStatusType2)

            => !EVSEAdminStatusType1.Equals(EVSEAdminStatusType2);

        #endregion

        #region Operator <  (EVSEAdminStatusType1, EVSEAdminStatusType2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EVSEAdminStatusType1">An EVSE admin status type.</param>
        /// <param name="EVSEAdminStatusType2">Another EVSE admin status type.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator < (EVSEAdminStatusType EVSEAdminStatusType1,
                                          EVSEAdminStatusType EVSEAdminStatusType2)

            => EVSEAdminStatusType1.CompareTo(EVSEAdminStatusType2) < 0;

        #endregion

        #region Operator <= (EVSEAdminStatusType1, EVSEAdminStatusType2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EVSEAdminStatusType1">An EVSE admin status type.</param>
        /// <param name="EVSEAdminStatusType2">Another EVSE admin status type.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator <= (EVSEAdminStatusType EVSEAdminStatusType1,
                                           EVSEAdminStatusType EVSEAdminStatusType2)

            => EVSEAdminStatusType1.CompareTo(EVSEAdminStatusType2) <= 0;

        #endregion

        #region Operator >  (EVSEAdminStatusType1, EVSEAdminStatusType2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EVSEAdminStatusType1">An EVSE admin status type.</param>
        /// <param name="EVSEAdminStatusType2">Another EVSE admin status type.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator > (EVSEAdminStatusType EVSEAdminStatusType1,
                                          EVSEAdminStatusType EVSEAdminStatusType2)

            => EVSEAdminStatusType1.CompareTo(EVSEAdminStatusType2) > 0;

        #endregion

        #region Operator >= (EVSEAdminStatusType1, EVSEAdminStatusType2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EVSEAdminStatusType1">An EVSE admin status type.</param>
        /// <param name="EVSEAdminStatusType2">Another EVSE admin status type.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator >= (EVSEAdminStatusType EVSEAdminStatusType1,
                                           EVSEAdminStatusType EVSEAdminStatusType2)

            => EVSEAdminStatusType1.CompareTo(EVSEAdminStatusType2) >= 0;

        #endregion

        #endregion

        #region IComparable<EVSEAdminStatusTypes> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two EVSE admin status types.
        /// </summary>
        /// <param name="Object">An EVSE admin status type to compare with.</param>
        public Int32 CompareTo(Object? Object)

            => Object is EVSEAdminStatusType evseAdminStatusType
                   ? CompareTo(evseAdminStatusType)
                   : throw new ArgumentException("The given object is not an EVSE admin status type!",
                                                 nameof(Object));

        #endregion

        #region CompareTo(EVSEAdminStatusType)

        /// <summary>
        /// Compares two EVSE admin status types.
        /// </summary>
        /// <param name="EVSEAdminStatusType">An EVSE admin status type to compare with.</param>
        public Int32 CompareTo(EVSEAdminStatusType EVSEAdminStatusType)

            => String.Compare(InternalId,
                              EVSEAdminStatusType.InternalId,
                              StringComparison.OrdinalIgnoreCase);

        #endregion

        #endregion

        #region IEquatable<EVSEAdminStatusTypes> Members

        #region Equals(Object)

        /// <summary>
        /// Compares two EVSE admin status types for equality.
        /// </summary>
        /// <param name="Object">An EVSE admin status type to compare with.</param>
        public override Boolean Equals(Object? Object)

            => Object is EVSEAdminStatusType evseAdminStatusType &&
                   Equals(evseAdminStatusType);

        #endregion

        #region Equals(EVSEAdminStatusType)

        /// <summary>
        /// Compares two EVSE admin status types for equality.
        /// </summary>
        /// <param name="EVSEAdminStatusType">An EVSE admin status type to compare with.</param>
        public Boolean Equals(EVSEAdminStatusType EVSEAdminStatusType)

            => String.Equals(InternalId,
                             EVSEAdminStatusType.InternalId,
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
