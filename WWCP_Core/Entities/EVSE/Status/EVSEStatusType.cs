/*
 * Copyright (c) 2014-2024 GraphDefined GmbH <achim.friedland@graphdefined.com>
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
    /// Extension methods for EVSE status types.
    /// </summary>
    public static class EVSEStatusTypeExtensions
    {

        /// <summary>
        /// Indicates whether this EVSE status types is null or empty.
        /// </summary>
        /// <param name="EVSEStatusType">An EVSE status type.</param>
        public static Boolean IsNullOrEmpty(this EVSEStatusType? EVSEStatusType)
            => !EVSEStatusType.HasValue || EVSEStatusType.Value.IsNullOrEmpty;

        /// <summary>
        /// Indicates whether this EVSE status types is null or empty.
        /// </summary>
        /// <param name="EVSEStatusType">An EVSE status type.</param>
        public static Boolean IsNotNullOrEmpty(this EVSEStatusType? EVSEStatusType)
            => EVSEStatusType.HasValue && EVSEStatusType.Value.IsNotNullOrEmpty;

    }


    /// <summary>
    /// The status type of an EVSE.
    /// </summary>
    public readonly struct EVSEStatusType : IId,
                                            IEquatable<EVSEStatusType>,
                                            IComparable<EVSEStatusType>
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
        /// The length of the EVSE status.
        /// </summary>
        public UInt64   Length
            => (UInt64) (InternalId?.Length ?? 0);

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new EVSE status types based on the given string.
        /// </summary>
        private EVSEStatusType(String Text)
        {
            InternalId = Text;
        }

        #endregion


        #region (static) Parse   (Text)

        /// <summary>
        /// Parse the given string as an EVSE status type.
        /// </summary>
        /// <param name="Text">A text representation of an EVSE status type.</param>
        public static EVSEStatusType Parse(String Text)
        {

            if (TryParse(Text, out var evseStatusType))
                return evseStatusType;

            throw new ArgumentException($"Invalid text representation of an EVSE status type: '" + Text + "'!",
                                        nameof(Text));

        }

        #endregion

        #region (static) TryParse(Text)

        /// <summary>
        /// Try to parse the given string as an EVSE status type.
        /// </summary>
        /// <param name="Text">A text representation of an EVSE status type.</param>
        public static EVSEStatusType? TryParse(String Text)
        {

            if (TryParse(Text, out var evseStatusType))
                return evseStatusType;

            return null;

        }

        #endregion

        #region (static) TryParse(Text, out EVSEStatusType)

        /// <summary>
        /// Parse the given string as an EVSE status type.
        /// </summary>
        /// <param name="Text">A text representation of an EVSE status type.</param>
        /// <param name="EVSEStatusType">The parsed EVSE status type.</param>
        public static Boolean TryParse(String Text, out EVSEStatusType EVSEStatusType)
        {

            Text = Text.Trim();

            if (Text.IsNotNullOrEmpty())
            {
                try
                {
                    EVSEStatusType = new EVSEStatusType(Text);
                    return true;
                }
                catch
                { }
            }

            EVSEStatusType = default;
            return false;

        }

        #endregion

        #region Clone

        /// <summary>
        /// Clone this EVSE status type.
        /// </summary>
        public EVSEStatusType Clone

            => new(
                   new String(InternalId?.ToCharArray())
               );

        #endregion


        #region Static definitions

        /// <summary>
        /// Unknown status of the EVSE.
        /// </summary>
        public static EVSEStatusType Unknown             { get; }
            = new ("unknown");

        /// <summary>
        /// Unclear status of the EVSE.
        /// </summary>
        public static EVSEStatusType Unspecified         { get; }
            = new ("unspecified");

        /// <summary>
        /// Currently no communication with the EVSE possible,
        /// but charging in offline mode might be available.
        /// </summary>
        public static EVSEStatusType Offline             { get; }
            = new ("offline");

        /// <summary>
        /// The EVSE is not fully operational yet.
        /// </summary>
        public static EVSEStatusType InDeployment        { get; }
            = new ("inDeployment");

        /// <summary>
        /// The EVSE is reserved for future charging.
        /// </summary>
        public static EVSEStatusType Reserved            { get; }
            = new ("reserved");

        /// <summary>
        /// The EVSE is available for charging.
        /// </summary>
        public static EVSEStatusType Available           { get; }
            = new ("available");

        /// <summary>
        /// The door of a charging locker is open, the EVSE is unlocked
        /// and is waiting for the customer to plugin.
        /// </summary>
        public static EVSEStatusType WaitingForPlugin    { get; }
            = new ("waitingForPlugin");

        /// <summary>
        /// A cable is plugged into the socket or a vehicle is connected
        /// to the cable, but both without any further action.
        /// </summary>
        public static EVSEStatusType PluggedIn           { get; }
            = new ("pluggedIn");

        /// <summary>
        /// The EVSE is currently charging a vehicle.
        /// </summary>
        public static EVSEStatusType Charging            { get; }
            = new ("charging");

        /// <summary>
        /// The EVSE is currently occupied and not accessible.
        /// </summary>
        public static EVSEStatusType Occupied            { get; }
            = new ("occupied");

        /// <summary>
        /// The EVSE has a mechanical door, e.g. an e-bike charging locker,
        /// which was not closed after the customer took the battery out.
        /// </summary>
        public static EVSEStatusType DoorNotClosed       { get; }
            = new ("doorNotClosed");

        /// <summary>
        /// The EVSE is not ready for charging because an error has occured.
        /// </summary>
        public static EVSEStatusType Error               { get; }
            = new ("error");

        /// <summary>
        /// The EVSE is not ready for charging because a fault has occured.
        /// </summary>
        public static EVSEStatusType Faulted             { get; }
            = new ("faulted");

        /// <summary>
        /// The EVSE is not ready for charging because it is under maintenance.
        /// </summary>
        public static EVSEStatusType Unavailable         { get; }
            = new ("unavailable");

        /// <summary>
        /// The EVSE is not ready for charging because it is under maintenance.
        /// </summary>
        public static EVSEStatusType Inoperative         { get; }
            = new ("inoperative");

        /// <summary>
        /// The EVSE is not ready for charging because it is under maintenance
        /// or was disabled by the charging station operator.
        /// </summary>
        public static EVSEStatusType OutOfService        { get; }
            = new ("outOfService");

        /// <summary>
        /// The EVSE is not ready for charging because it is under maintenance.
        /// </summary>
        public static EVSEStatusType OutOfOrder          { get; }
            = new ("outOfOrder");

        /// <summary>
        /// The EVSE is not accessible because of a physical barrier, i.e. a car.
        /// </summary>
        public static EVSEStatusType Blocked             { get; }
            = new ("blocked");

        /// <summary>
        /// The EVSE was removed.
        /// </summary>
        public static EVSEStatusType Removed             { get; }
            = new ("removed");

        /// <summary>
        /// The EVSE is in an unknown state.
        /// </summary>
        public static EVSEStatusType Other               { get; }
            = new ("other");

        #endregion


        #region Operator overloading

        #region Operator == (EVSEStatusType1, EVSEStatusType2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EVSEStatusType1">An EVSE status type.</param>
        /// <param name="EVSEStatusType2">Another EVSE status type.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator == (EVSEStatusType EVSEStatusType1,
                                           EVSEStatusType EVSEStatusType2)

            => EVSEStatusType1.Equals(EVSEStatusType2);

        #endregion

        #region Operator != (EVSEStatusType1, EVSEStatusType2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EVSEStatusType1">An EVSE status type.</param>
        /// <param name="EVSEStatusType2">Another EVSE status type.</param>
        /// <returns>False if both match; True otherwise.</returns>
        public static Boolean operator != (EVSEStatusType EVSEStatusType1,
                                           EVSEStatusType EVSEStatusType2)

            => !EVSEStatusType1.Equals(EVSEStatusType2);

        #endregion

        #region Operator <  (EVSEStatusType1, EVSEStatusType2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EVSEStatusType1">An EVSE status type.</param>
        /// <param name="EVSEStatusType2">Another EVSE status type.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator < (EVSEStatusType EVSEStatusType1,
                                          EVSEStatusType EVSEStatusType2)

            => EVSEStatusType1.CompareTo(EVSEStatusType2) < 0;

        #endregion

        #region Operator <= (EVSEStatusType1, EVSEStatusType2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EVSEStatusType1">An EVSE status type.</param>
        /// <param name="EVSEStatusType2">Another EVSE status type.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator <= (EVSEStatusType EVSEStatusType1,
                                           EVSEStatusType EVSEStatusType2)

            => EVSEStatusType1.CompareTo(EVSEStatusType2) <= 0;

        #endregion

        #region Operator >  (EVSEStatusType1, EVSEStatusType2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EVSEStatusType1">An EVSE status type.</param>
        /// <param name="EVSEStatusType2">Another EVSE status type.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator > (EVSEStatusType EVSEStatusType1,
                                          EVSEStatusType EVSEStatusType2)

            => EVSEStatusType1.CompareTo(EVSEStatusType2) > 0;

        #endregion

        #region Operator >= (EVSEStatusType1, EVSEStatusType2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EVSEStatusType1">An EVSE status type.</param>
        /// <param name="EVSEStatusType2">Another EVSE status type.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator >= (EVSEStatusType EVSEStatusType1,
                                           EVSEStatusType EVSEStatusType2)

            => EVSEStatusType1.CompareTo(EVSEStatusType2) >= 0;

        #endregion

        #endregion

        #region IComparable<EVSEStatusType> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two EVSE status types.
        /// </summary>
        /// <param name="Object">An EVSE status type to compare with.</param>
        public Int32 CompareTo(Object? Object)

            => Object is EVSEStatusType evseStatusType
                   ? CompareTo(evseStatusType)
                   : throw new ArgumentException("The given object is not an EVSE status type!",
                                                 nameof(Object));

        #endregion

        #region CompareTo(EVSEStatusType)

        /// <summary>
        /// Compares two EVSE status types.
        /// </summary>
        /// <param name="EVSEStatusType">An EVSE status type to compare with.</param>
        public Int32 CompareTo(EVSEStatusType EVSEStatusType)

            => String.Compare(InternalId,
                              EVSEStatusType.InternalId,
                              StringComparison.OrdinalIgnoreCase);

        #endregion

        #endregion

        #region IEquatable<EVSEStatusType> Members

        #region Equals(Object)

        /// <summary>
        /// Compares two EVSE status types for equality.
        /// </summary>
        /// <param name="Object">An EVSE status type to compare with.</param>
        public override Boolean Equals(Object? Object)

            => Object is EVSEStatusType evseStatusType &&
                   Equals(evseStatusType);

        #endregion

        #region Equals(EVSEStatusType)

        /// <summary>
        /// Compares two EVSE status types for equality.
        /// </summary>
        /// <param name="EVSEStatusType">An EVSE status type to compare with.</param>
        public Boolean Equals(EVSEStatusType EVSEStatusType)

            => String.Equals(InternalId,
                             EVSEStatusType.InternalId,
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
