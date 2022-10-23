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
    /// Extension methods for charging station status typess.
    /// </summary>
    public static class ChargingStationStatusTypesExtensions
    {

        /// <summary>
        /// Indicates whether this charging station status types is null or empty.
        /// </summary>
        /// <param name="ChargingStationStatusType">A charging station status type.</param>
        public static Boolean IsNullOrEmpty(this ChargingStationStatusTypes? ChargingStationStatusType)
            => !ChargingStationStatusType.HasValue || ChargingStationStatusType.Value.IsNullOrEmpty;

        /// <summary>
        /// Indicates whether this charging station status types is null or empty.
        /// </summary>
        /// <param name="ChargingStationStatusType">A charging station status type.</param>
        public static Boolean IsNotNullOrEmpty(this ChargingStationStatusTypes? ChargingStationStatusType)
            => ChargingStationStatusType.HasValue && ChargingStationStatusType.Value.IsNotNullOrEmpty;

    }


    /// <summary>
    /// The status type of a charging station.
    /// </summary>
    public readonly struct ChargingStationStatusTypes : IId,
                                                        IEquatable<ChargingStationStatusTypes>,
                                                        IComparable<ChargingStationStatusTypes>
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
            => (UInt64) InternalId.Length;

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new charging station status types based on the given string.
        /// </summary>
        private ChargingStationStatusTypes(String Text)
        {
            InternalId = Text;
        }

        #endregion


        #region (static) Parse   (Text)

        /// <summary>
        /// Parse the given string as a charging station status type.
        /// </summary>
        /// <param name="Text">A text representation of a charging station status type.</param>
        public static ChargingStationStatusTypes Parse(String Text)
        {

            if (TryParse(Text, out ChargingStationStatusTypes chargingStationStatusTypes))
                return chargingStationStatusTypes;

            throw new ArgumentException("Invalid text-representation of a charging station status type: '" + Text + "'!",
                                        nameof(Text));

        }

        #endregion

        #region (static) TryParse(Text)

        /// <summary>
        /// Try to parse the given string as a charging station status type.
        /// </summary>
        /// <param name="Text">A text representation of a charging station status type.</param>
        public static ChargingStationStatusTypes? TryParse(String Text)
        {

            if (TryParse(Text, out ChargingStationStatusTypes chargingStationStatusTypes))
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
        public static Boolean TryParse(String Text, out ChargingStationStatusTypes ChargingStationStatusType)
        {

            Text = Text.Trim();

            if (Text.IsNotNullOrEmpty())
            {
                try
                {
                    ChargingStationStatusType = new ChargingStationStatusTypes(Text);
                    return true;
                }
                catch
                { }
            }

            ChargingStationStatusType = default;
            return false;

        }

        #endregion

        #region Clone

        /// <summary>
        /// Clone this charging station status type.
        /// </summary>
        public ChargingStationStatusTypes Clone

            => new(
                   new String(InternalId?.ToCharArray())
               );

        #endregion


        #region Static members

        /// <summary>
        /// Unknown status of the ChargingStation.
        /// </summary>
        public static readonly ChargingStationStatusTypes Unknown           = new("unknown");

        /// <summary>
        /// Unclear status of the ChargingStation.
        /// </summary>
        public static readonly ChargingStationStatusTypes Unspecified       = new("unspecified");

        /// <summary>
        /// Currently no communication with the ChargingStation possible,
        /// but charging in offline mode might be available.
        /// </summary>
        public static readonly ChargingStationStatusTypes Offline           = new("offline");

        /// <summary>
        /// The ChargingStation is not fully operational yet.
        /// </summary>
        public static readonly ChargingStationStatusTypes InDeployment      = new("inDeployment");

        /// <summary>
        /// The ChargingStation is reserved for future charging.
        /// </summary>
        public static readonly ChargingStationStatusTypes Reserved          = new("reserved");

        /// <summary>
        /// The ChargingStation is available for charging.
        /// </summary>
        public static readonly ChargingStationStatusTypes Available         = new("available");

        /// <summary>
        /// Some ongoing charging sessions or reservations, but still ready to charge.
        /// </summary>
        public static readonly ChargingStationStatusTypes PartialAvailable  = new("partialAvailable");

        /// <summary>
        /// The door of a charging locker is open, the ChargingStation is unlocked
        /// and is waiting for the customer to plugin.
        /// </summary>
        public static readonly ChargingStationStatusTypes WaitingForPlugin  = new("waitingForPlugin");

        /// <summary>
        /// A cable is plugged into the socket or a vehicle is connected
        /// to the cable, but both without any further action.
        /// </summary>
        public static readonly ChargingStationStatusTypes PluggedIn         = new("pluggedIn");

        /// <summary>
        /// An ongoing charging process.
        /// </summary>
        public static readonly ChargingStationStatusTypes Charging          = new("charging");

        /// <summary>
        /// The ChargingStation has a mechanical door, e.g. an e-bike charging locker,
        /// which was not closed after the customer took the battery out.
        /// </summary>
        public static readonly ChargingStationStatusTypes DoorNotClosed     = new("doorNotClosed");

        /// <summary>
        /// A fatal error has occured within the ChargingStation.
        /// </summary>
        public static readonly ChargingStationStatusTypes Error             = new("error");

        /// <summary>
        /// The ChargingStation is not ready for charging because it is under maintenance
        /// or was disabled by the charging station operator.
        /// </summary>
        public static readonly ChargingStationStatusTypes OutOfService      = new("outOfService");

        #endregion


        #region Operator overloading

        #region Operator == (ChargingStationStatusType1, ChargingStationStatusType2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingStationStatusType1">A charging station status type.</param>
        /// <param name="ChargingStationStatusType2">Another charging station status type.</param>
        /// <returns>true|false</returns>
        public static Boolean operator == (ChargingStationStatusTypes ChargingStationStatusType1,
                                           ChargingStationStatusTypes ChargingStationStatusType2)

            => ChargingStationStatusType1.Equals(ChargingStationStatusType2);

        #endregion

        #region Operator != (ChargingStationStatusType1, ChargingStationStatusType2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingStationStatusType1">A charging station status type.</param>
        /// <param name="ChargingStationStatusType2">Another charging station status type.</param>
        /// <returns>true|false</returns>
        public static Boolean operator != (ChargingStationStatusTypes ChargingStationStatusType1,
                                           ChargingStationStatusTypes ChargingStationStatusType2)

            => !ChargingStationStatusType1.Equals(ChargingStationStatusType2);

        #endregion

        #region Operator <  (ChargingStationStatusType1, ChargingStationStatusType2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingStationStatusType1">A charging station status type.</param>
        /// <param name="ChargingStationStatusType2">Another charging station status type.</param>
        /// <returns>true|false</returns>
        public static Boolean operator < (ChargingStationStatusTypes ChargingStationStatusType1,
                                          ChargingStationStatusTypes ChargingStationStatusType2)

            => ChargingStationStatusType1.CompareTo(ChargingStationStatusType2) < 0;

        #endregion

        #region Operator <= (ChargingStationStatusType1, ChargingStationStatusType2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingStationStatusType1">A charging station status type.</param>
        /// <param name="ChargingStationStatusType2">Another charging station status type.</param>
        /// <returns>true|false</returns>
        public static Boolean operator <= (ChargingStationStatusTypes ChargingStationStatusType1,
                                           ChargingStationStatusTypes ChargingStationStatusType2)

            => ChargingStationStatusType1.CompareTo(ChargingStationStatusType2) <= 0;

        #endregion

        #region Operator >  (ChargingStationStatusType1, ChargingStationStatusType2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingStationStatusType1">A charging station status type.</param>
        /// <param name="ChargingStationStatusType2">Another charging station status type.</param>
        /// <returns>true|false</returns>
        public static Boolean operator > (ChargingStationStatusTypes ChargingStationStatusType1,
                                          ChargingStationStatusTypes ChargingStationStatusType2)

            => ChargingStationStatusType1.CompareTo(ChargingStationStatusType2) > 0;

        #endregion

        #region Operator >= (ChargingStationStatusType1, ChargingStationStatusType2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingStationStatusType1">A charging station status type.</param>
        /// <param name="ChargingStationStatusType2">Another charging station status type.</param>
        /// <returns>true|false</returns>
        public static Boolean operator >= (ChargingStationStatusTypes ChargingStationStatusType1,
                                           ChargingStationStatusTypes ChargingStationStatusType2)

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

            => Object is ChargingStationStatusTypes chargingStationStatusTypes
                   ? CompareTo(chargingStationStatusTypes)
                   : throw new ArgumentException("The given object is not a charging station status type!",
                                                 nameof(Object));

        #endregion

        #region CompareTo(ChargingStationStatusType)

        /// <summary>
        /// Compares two charging station status types.
        /// </summary>
        /// <param name="ChargingStationStatusType">A charging station status type to compare with.</param>
        public Int32 CompareTo(ChargingStationStatusTypes ChargingStationStatusType)

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

            => Object is ChargingStationStatusTypes chargingStationStatusTypes &&
                   Equals(chargingStationStatusTypes);

        #endregion

        #region Equals(ChargingStationStatusType)

        /// <summary>
        /// Compares two charging station status types for equality.
        /// </summary>
        /// <param name="ChargingStationStatusType">A charging station status type to compare with.</param>
        public Boolean Equals(ChargingStationStatusTypes ChargingStationStatusType)

            => String.Equals(InternalId,
                             ChargingStationStatusType.InternalId,
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
