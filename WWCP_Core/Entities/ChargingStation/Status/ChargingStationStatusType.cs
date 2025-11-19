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
    /// Extension methods for charging station status typess.
    /// </summary>
    public static class ChargingStationStatusTypesExtensions
    {

        /// <summary>
        /// Indicates whether this charging station status type is null or empty.
        /// </summary>
        /// <param name="ChargingStationStatusType">A charging station status type.</param>
        public static Boolean IsNullOrEmpty(this ChargingStationStatusType? ChargingStationStatusType)
            => !ChargingStationStatusType.HasValue || ChargingStationStatusType.Value.IsNullOrEmpty;

        /// <summary>
        /// Indicates whether this charging station status type is null or empty.
        /// </summary>
        /// <param name="ChargingStationStatusType">A charging station status type.</param>
        public static Boolean IsNotNullOrEmpty(this ChargingStationStatusType? ChargingStationStatusType)
            => ChargingStationStatusType.HasValue && ChargingStationStatusType.Value.IsNotNullOrEmpty;

    }


    /// <summary>
    /// The status type of a charging station.
    /// </summary>
    public readonly struct ChargingStationStatusType : IId,
                                                       IEquatable<ChargingStationStatusType>,
                                                       IComparable<ChargingStationStatusType>
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
        /// The length of the charging station status.
        /// </summary>
        public UInt64 Length
            => (UInt64) (InternalId?.Length ?? 0);

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new charging station status types based on the given string.
        /// </summary>
        private ChargingStationStatusType(String Text)
        {
            InternalId = Text;
        }

        #endregion


        #region (static) Parse   (Text)

        /// <summary>
        /// Parse the given string as a charging station status type.
        /// </summary>
        /// <param name="Text">A text representation of a charging station status type.</param>
        public static ChargingStationStatusType Parse(String Text)
        {

            if (TryParse(Text, out ChargingStationStatusType chargingStationStatusTypes))
                return chargingStationStatusTypes;

            throw new ArgumentException($"Invalid text representation of a charging station status type: '" + Text + "'!",
                                        nameof(Text));

        }

        #endregion

        #region (static) TryParse(Text)

        /// <summary>
        /// Try to parse the given string as a charging station status type.
        /// </summary>
        /// <param name="Text">A text representation of a charging station status type.</param>
        public static ChargingStationStatusType? TryParse(String Text)
        {

            if (TryParse(Text, out ChargingStationStatusType chargingStationStatusTypes))
                return chargingStationStatusTypes;

            return null;

        }

        #endregion

        #region (static) TryParse(Text, out ChargingStationStatusType)

        /// <summary>
        /// Parse the given string as a charging station status type.
        /// </summary>
        /// <param name="Text">A text representation of a charging station status type.</param>
        /// <param name="ChargingStationStatusType">The parsed charging station status type.</param>
        public static Boolean TryParse(String Text, out ChargingStationStatusType ChargingStationStatusType)
        {

            Text = Text.Trim();

            if (Text.IsNotNullOrEmpty())
            {
                try
                {
                    ChargingStationStatusType = new ChargingStationStatusType(Text);
                    return true;
                }
                catch
                { }
            }

            ChargingStationStatusType = default;
            return false;

        }

        #endregion

        #region Clone()

        /// <summary>
        /// Clone this charging station status type.
        /// </summary>
        public ChargingStationStatusType Clone()

            => new (
                   InternalId.CloneString()
               );

        #endregion


        #region Static members

        /// <summary>
        /// Unknown status of the ChargingStation.
        /// </summary>
        public static ChargingStationStatusType Unknown             { get; }
            = new ("unknown");

        /// <summary>
        /// Unclear status of the ChargingStation.
        /// </summary>
        public static ChargingStationStatusType Unspecified         { get; }
            = new ("unspecified");

        /// <summary>
        /// Currently no communication with the ChargingStation possible,
        /// but charging in offline mode might be available.
        /// </summary>
        public static ChargingStationStatusType Offline             { get; }
            = new ("offline");

        /// <summary>
        /// The ChargingStation is not fully operational yet.
        /// </summary>
        public static ChargingStationStatusType InDeployment        { get; }
            = new ("inDeployment");

        /// <summary>
        /// The ChargingStation is reserved for future charging.
        /// </summary>
        public static ChargingStationStatusType Reserved            { get; }
            = new ("reserved");

        /// <summary>
        /// The ChargingStation is available for charging.
        /// </summary>
        public static ChargingStationStatusType Available           { get; }
            = new ("available");

        /// <summary>
        /// Some ongoing charging sessions or reservations, but still ready to charge.
        /// </summary>
        public static ChargingStationStatusType PartialAvailable    { get; }
            = new ("partialAvailable");

        /// <summary>
        /// The door of a charging locker is open, the ChargingStation is unlocked
        /// and is waiting for the customer to plugin.
        /// </summary>
        public static ChargingStationStatusType WaitingForPlugin    { get; }
            = new ("waitingForPlugin");

        /// <summary>
        /// A cable is plugged into the socket or a vehicle is connected
        /// to the cable, but both without any further action.
        /// </summary>
        public static ChargingStationStatusType PluggedIn           { get; }
            = new ("pluggedIn");

        /// <summary>
        /// An ongoing charging process.
        /// </summary>
        public static ChargingStationStatusType Charging            { get; }
            = new ("charging");

        /// <summary>
        /// The ChargingStation has a mechanical door, e.g. an e-bike charging locker,
        /// which was not closed after the customer took the battery out.
        /// </summary>
        public static ChargingStationStatusType DoorNotClosed       { get; }
            = new ("doorNotClosed");

        /// <summary>
        /// A fatal error has occurred within the ChargingStation.
        /// </summary>
        public static ChargingStationStatusType Error               { get; }
            = new ("error");

        /// <summary>
        /// The charging station was removed.
        /// </summary>
        public static ChargingStationStatusType  Removed            { get; }
            = new ("removed");

        /// <summary>
        /// The charging station is not ready for charging because it is under maintenance
        /// or was disabled by the charging station operator.
        /// </summary>
        public static ChargingStationStatusType OutOfService        { get; }
            = new ("outOfService");

        #endregion


        #region Operator overloading

        #region Operator == (ChargingStationStatusType1, ChargingStationStatusType2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingStationStatusType1">A charging station status type.</param>
        /// <param name="ChargingStationStatusType2">Another charging station status type.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator == (ChargingStationStatusType ChargingStationStatusType1,
                                           ChargingStationStatusType ChargingStationStatusType2)

            => ChargingStationStatusType1.Equals(ChargingStationStatusType2);

        #endregion

        #region Operator != (ChargingStationStatusType1, ChargingStationStatusType2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingStationStatusType1">A charging station status type.</param>
        /// <param name="ChargingStationStatusType2">Another charging station status type.</param>
        /// <returns>False if both match; True otherwise.</returns>
        public static Boolean operator != (ChargingStationStatusType ChargingStationStatusType1,
                                           ChargingStationStatusType ChargingStationStatusType2)

            => !ChargingStationStatusType1.Equals(ChargingStationStatusType2);

        #endregion

        #region Operator <  (ChargingStationStatusType1, ChargingStationStatusType2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingStationStatusType1">A charging station status type.</param>
        /// <param name="ChargingStationStatusType2">Another charging station status type.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator < (ChargingStationStatusType ChargingStationStatusType1,
                                          ChargingStationStatusType ChargingStationStatusType2)

            => ChargingStationStatusType1.CompareTo(ChargingStationStatusType2) < 0;

        #endregion

        #region Operator <= (ChargingStationStatusType1, ChargingStationStatusType2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingStationStatusType1">A charging station status type.</param>
        /// <param name="ChargingStationStatusType2">Another charging station status type.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator <= (ChargingStationStatusType ChargingStationStatusType1,
                                           ChargingStationStatusType ChargingStationStatusType2)

            => ChargingStationStatusType1.CompareTo(ChargingStationStatusType2) <= 0;

        #endregion

        #region Operator >  (ChargingStationStatusType1, ChargingStationStatusType2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingStationStatusType1">A charging station status type.</param>
        /// <param name="ChargingStationStatusType2">Another charging station status type.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator > (ChargingStationStatusType ChargingStationStatusType1,
                                          ChargingStationStatusType ChargingStationStatusType2)

            => ChargingStationStatusType1.CompareTo(ChargingStationStatusType2) > 0;

        #endregion

        #region Operator >= (ChargingStationStatusType1, ChargingStationStatusType2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingStationStatusType1">A charging station status type.</param>
        /// <param name="ChargingStationStatusType2">Another charging station status type.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator >= (ChargingStationStatusType ChargingStationStatusType1,
                                           ChargingStationStatusType ChargingStationStatusType2)

            => ChargingStationStatusType1.CompareTo(ChargingStationStatusType2) >= 0;

        #endregion

        #endregion

        #region IComparable<ChargingStationStatusTypes> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two charging station status types.
        /// </summary>
        /// <param name="Object">A charging station status type to compare with.</param>
        public Int32 CompareTo(Object? Object)

            => Object is ChargingStationStatusType chargingStationStatusTypes
                   ? CompareTo(chargingStationStatusTypes)
                   : throw new ArgumentException("The given object is not a charging station status type!",
                                                 nameof(Object));

        #endregion

        #region CompareTo(ChargingStationStatusType)

        /// <summary>
        /// Compares two charging station status types.
        /// </summary>
        /// <param name="ChargingStationStatusType">A charging station status type to compare with.</param>
        public Int32 CompareTo(ChargingStationStatusType ChargingStationStatusType)

            => String.Compare(InternalId,
                              ChargingStationStatusType.InternalId,
                              StringComparison.OrdinalIgnoreCase);

        #endregion

        #endregion

        #region IEquatable<ChargingStationStatusTypes> Members

        #region Equals(Object)

        /// <summary>
        /// Compares two charging station status types for equality.
        /// </summary>
        /// <param name="Object">A charging station status type to compare with.</param>
        public override Boolean Equals(Object? Object)

            => Object is ChargingStationStatusType chargingStationStatusTypes &&
                   Equals(chargingStationStatusTypes);

        #endregion

        #region Equals(ChargingStationStatusType)

        /// <summary>
        /// Compares two charging station status types for equality.
        /// </summary>
        /// <param name="ChargingStationStatusType">A charging station status type to compare with.</param>
        public Boolean Equals(ChargingStationStatusType ChargingStationStatusType)

            => String.Equals(InternalId,
                             ChargingStationStatusType.InternalId,
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
