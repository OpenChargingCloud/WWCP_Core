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

namespace cloud.charging.open.protocols.WWCP.CSM
{

    /// <summary>
    /// Extension methods for charging station manufacturer identifications.
    /// </summary>
    public static class ChargingStationManufacturerIdExtensions
    {

        /// <summary>
        /// Indicates whether this charging station manufacturer identification is null or empty.
        /// </summary>
        /// <param name="ChargingStationManufacturerId">A charging station manufacturer identification.</param>
        public static Boolean IsNullOrEmpty(this ChargingStationManufacturer_Id? ChargingStationManufacturerId)
            => !ChargingStationManufacturerId.HasValue || ChargingStationManufacturerId.Value.IsNullOrEmpty;

        /// <summary>
        /// Indicates whether this charging station manufacturer identification is NOT null or empty.
        /// </summary>
        /// <param name="ChargingStationManufacturerId">A charging station manufacturer identification.</param>
        public static Boolean IsNotNullOrEmpty(this ChargingStationManufacturer_Id? ChargingStationManufacturerId)
            => ChargingStationManufacturerId.HasValue && ChargingStationManufacturerId.Value.IsNotNullOrEmpty;

    }


    /// <summary>
    /// The unique identification of a charging station manufacturer.
    /// </summary>
    public readonly struct ChargingStationManufacturer_Id : IId<ChargingStationManufacturer_Id>
    {

        #region Data

        /// <summary>
        /// The internal identification.
        /// </summary>
        private readonly String InternalId;

        #endregion

        #region Properties

        /// <summary>
        /// Indicates whether this charging station manufacturer identification is null or empty.
        /// </summary>
        public Boolean IsNullOrEmpty
            => InternalId.IsNullOrEmpty();

        /// <summary>
        /// Indicates whether this charging station manufacturer identification is NOT null or empty.
        /// </summary>
        public Boolean IsNotNullOrEmpty
            => InternalId.IsNotNullOrEmpty();

        /// <summary>
        /// The length of the charging station manufacturer identification.
        /// </summary>
        public UInt64 Length
            => (UInt64) (InternalId?.Length ?? 0);

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new charging station manufacturer identification based on the given text.
        /// </summary>
        /// <param name="Text">The text representation of a charging station manufacturer identification.</param>
        private ChargingStationManufacturer_Id(String Text)
        {
            this.InternalId = Text;
        }

        #endregion


        #region (static) NewRandom(Length = 50)

        /// <summary>
        /// Create a new random charging station manufacturer identification.
        /// </summary>
        /// <param name="Length">The expected length of the charging station manufacturer identification.</param>
        public static ChargingStationManufacturer_Id NewRandom(Byte Length = 50)

            => new(RandomExtensions.RandomString(Length));

        #endregion

        #region (static) Parse   (Text)

        /// <summary>
        /// Parse the given text as a charging station manufacturer identification.
        /// </summary>
        /// <param name="Text">A text representation of a charging station manufacturer identification.</param>
        public static ChargingStationManufacturer_Id Parse(String Text)
        {

            if (TryParse(Text, out var chargingStationManufacturerId))
                return chargingStationManufacturerId;

            throw new ArgumentException($"Invalid text representation of a charging station manufacturer identification: '{Text}'!",
                                        nameof(Text));

        }

        #endregion

        #region (static) TryParse(Text)

        /// <summary>
        /// Try to parse the given text as a charging station manufacturer identification.
        /// </summary>
        /// <param name="Text">A text representation of a charging station manufacturer identification.</param>
        public static ChargingStationManufacturer_Id? TryParse(String Text)
        {

            if (TryParse(Text, out var chargingStationManufacturerId))
                return chargingStationManufacturerId;

            return null;

        }

        #endregion

        #region (static) TryParse(Text, out ChargingStationManufacturerId)

        /// <summary>
        /// Try to parse the given text as a charging station manufacturer identification.
        /// </summary>
        /// <param name="Text">A text representation of a charging station manufacturer identification.</param>
        /// <param name="ChargingStationManufacturerId">The parsed charging station manufacturer identification.</param>
        public static Boolean TryParse(String Text, out ChargingStationManufacturer_Id ChargingStationManufacturerId)
        {

            Text = Text.Trim();

            if (Text.IsNotNullOrEmpty())
            {
                try
                {
                    ChargingStationManufacturerId = new ChargingStationManufacturer_Id(Text);
                    return true;
                }
                catch
                { }
            }

            ChargingStationManufacturerId = default;
            return false;

        }

        #endregion

        #region Clone

        /// <summary>
        /// Clone this charging station manufacturer identification.
        /// </summary>
        public ChargingStationManufacturer_Id Clone

            => new (
                   new String(InternalId?.ToCharArray())
               );

        #endregion


        #region Operator overloading

        #region Operator == (ChargingStationManufacturerId1, ChargingStationManufacturerId2)

        /// <summary>
        /// Compares two charging station manufacturer identifications for equality.
        /// </summary>
        /// <param name="ChargingStationManufacturerId1">A charging station manufacturer identification.</param>
        /// <param name="ChargingStationManufacturerId2">Another charging station manufacturer identification.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator == (ChargingStationManufacturer_Id ChargingStationManufacturerId1,
                                           ChargingStationManufacturer_Id ChargingStationManufacturerId2)

            => ChargingStationManufacturerId1.Equals(ChargingStationManufacturerId2);

        #endregion

        #region Operator != (ChargingStationManufacturerId1, ChargingStationManufacturerId2)

        /// <summary>
        /// Compares two charging station manufacturer identifications for inequality.
        /// </summary>
        /// <param name="ChargingStationManufacturerId1">A charging station manufacturer identification.</param>
        /// <param name="ChargingStationManufacturerId2">Another charging station manufacturer identification.</param>
        /// <returns>False if both match; True otherwise.</returns>
        public static Boolean operator != (ChargingStationManufacturer_Id ChargingStationManufacturerId1,
                                           ChargingStationManufacturer_Id ChargingStationManufacturerId2)

            => !ChargingStationManufacturerId1.Equals(ChargingStationManufacturerId2);

        #endregion

        #region Operator <  (ChargingStationManufacturerId1, ChargingStationManufacturerId2)

        /// <summary>
        /// Compares two charging station manufacturer identifications.
        /// </summary>
        /// <param name="ChargingStationManufacturerId1">A charging station manufacturer identification.</param>
        /// <param name="ChargingStationManufacturerId2">Another charging station manufacturer identification.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator < (ChargingStationManufacturer_Id ChargingStationManufacturerId1,
                                          ChargingStationManufacturer_Id ChargingStationManufacturerId2)

            => ChargingStationManufacturerId1.CompareTo(ChargingStationManufacturerId2) < 0;

        #endregion

        #region Operator <= (ChargingStationManufacturerId1, ChargingStationManufacturerId2)

        /// <summary>
        /// Compares two charging station manufacturer identifications.
        /// </summary>
        /// <param name="ChargingStationManufacturerId1">A charging station manufacturer identification.</param>
        /// <param name="ChargingStationManufacturerId2">Another charging station manufacturer identification.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator <= (ChargingStationManufacturer_Id ChargingStationManufacturerId1,
                                           ChargingStationManufacturer_Id ChargingStationManufacturerId2)

            => ChargingStationManufacturerId1.CompareTo(ChargingStationManufacturerId2) <= 0;

        #endregion

        #region Operator >  (ChargingStationManufacturerId1, ChargingStationManufacturerId2)

        /// <summary>
        /// Compares two charging station manufacturer identifications.
        /// </summary>
        /// <param name="ChargingStationManufacturerId1">A charging station manufacturer identification.</param>
        /// <param name="ChargingStationManufacturerId2">Another charging station manufacturer identification.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator > (ChargingStationManufacturer_Id ChargingStationManufacturerId1,
                                          ChargingStationManufacturer_Id ChargingStationManufacturerId2)

            => ChargingStationManufacturerId1.CompareTo(ChargingStationManufacturerId2) > 0;

        #endregion

        #region Operator >= (ChargingStationManufacturerId1, ChargingStationManufacturerId2)

        /// <summary>
        /// Compares two charging station manufacturer identifications.
        /// </summary>
        /// <param name="ChargingStationManufacturerId1">A charging station manufacturer identification.</param>
        /// <param name="ChargingStationManufacturerId2">Another charging station manufacturer identification.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator >= (ChargingStationManufacturer_Id ChargingStationManufacturerId1,
                                           ChargingStationManufacturer_Id ChargingStationManufacturerId2)

            => ChargingStationManufacturerId1.CompareTo(ChargingStationManufacturerId2) >= 0;

        #endregion

        #endregion

        #region IComparable<ChargingStationManufacturerId> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two charging station manufacturer identifications.
        /// </summary>
        /// <param name="Object">A charging station manufacturer identification to compare with.</param>
        public Int32 CompareTo(Object? Object)

            => Object is ChargingStationManufacturer_Id chargingStationManufacturerId
                   ? CompareTo(chargingStationManufacturerId)
                   : throw new ArgumentException("The given object is not a charging station manufacturer identification!",
                                                 nameof(Object));

        #endregion

        #region CompareTo(ChargingStationManufacturerId)

        /// <summary>
        /// Compares two charging station manufacturer identifications.
        /// </summary>
        /// <param name="ChargingStationManufacturerId">A charging station manufacturer identification to compare with.</param>
        public Int32 CompareTo(ChargingStationManufacturer_Id ChargingStationManufacturerId)

            => String.Compare(InternalId,
                              ChargingStationManufacturerId.InternalId,
                              StringComparison.OrdinalIgnoreCase);

        #endregion

        #endregion

        #region IEquatable<ChargingStationManufacturerId> Members

        #region Equals(Object)

        /// <summary>
        /// Compares two charging station manufacturer identifications for equality.
        /// </summary>
        /// <param name="Object">A charging station manufacturer identification to compare with.</param>
        public override Boolean Equals(Object? Object)

            => Object is ChargingStationManufacturer_Id chargingStationManufacturerId &&
                   Equals(chargingStationManufacturerId);

        #endregion

        #region Equals(ChargingStationManufacturerId)

        /// <summary>
        /// Compares two charging station manufacturer identifications for equality.
        /// </summary>
        /// <param name="ChargingStationManufacturerId">A charging station manufacturer identification to compare with.</param>
        public Boolean Equals(ChargingStationManufacturer_Id ChargingStationManufacturerId)

            => String.Equals(InternalId,
                             ChargingStationManufacturerId.InternalId,
                             StringComparison.OrdinalIgnoreCase);

        #endregion

        #endregion

        #region (override) GetHashCode()

        /// <summary>
        /// Return the hash code of this object.
        /// </summary>
        /// <returns>The hash code of this object.</returns>
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
