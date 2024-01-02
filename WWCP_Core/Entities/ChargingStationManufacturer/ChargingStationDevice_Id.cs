/*
 * Copyright (c) 2014-2024 GraphDefined GmbH <achim.friedland@graphdefined.com> <achim.friedland@graphdefined.com>
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
    /// Extension methods for charging station device identifications.
    /// </summary>
    public static class ChargingStationDeviceIdExtensions
    {

        /// <summary>
        /// Indicates whether this charging station device identification is null or empty.
        /// </summary>
        /// <param name="ChargingStationDeviceId">A charging station device identification.</param>
        public static Boolean IsNullOrEmpty(this ChargingStationDevice_Id? ChargingStationDeviceId)
            => !ChargingStationDeviceId.HasValue || ChargingStationDeviceId.Value.IsNullOrEmpty;

        /// <summary>
        /// Indicates whether this charging station device identification is NOT null or empty.
        /// </summary>
        /// <param name="ChargingStationDeviceId">A charging station device identification.</param>
        public static Boolean IsNotNullOrEmpty(this ChargingStationDevice_Id? ChargingStationDeviceId)
            => ChargingStationDeviceId.HasValue && ChargingStationDeviceId.Value.IsNotNullOrEmpty;

    }


    /// <summary>
    /// The unique identification of a charging station device.
    /// </summary>
    public readonly struct ChargingStationDevice_Id : IId<ChargingStationDevice_Id>
    {

        #region Data

        /// <summary>
        /// The internal identification.
        /// </summary>
        private readonly String InternalId;

        #endregion

        #region Properties

        /// <summary>
        /// Indicates whether this charging station device identification is null or empty.
        /// </summary>
        public Boolean IsNullOrEmpty
            => InternalId.IsNullOrEmpty();

        /// <summary>
        /// Indicates whether this charging station device identification is NOT null or empty.
        /// </summary>
        public Boolean IsNotNullOrEmpty
            => InternalId.IsNotNullOrEmpty();

        /// <summary>
        /// The length of the charging station device identification.
        /// </summary>
        public UInt64 Length
            => (UInt64) (InternalId?.Length ?? 0);

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new charging station device identification based on the given text.
        /// </summary>
        /// <param name="Text">The text representation of a charging station device identification.</param>
        private ChargingStationDevice_Id(String Text)
        {
            this.InternalId = Text;
        }

        #endregion


        #region (static) NewRandom(Length = 50)

        /// <summary>
        /// Create a new random charging station device identification.
        /// </summary>
        /// <param name="Length">The expected length of the charging station device identification.</param>
        public static ChargingStationDevice_Id NewRandom(Byte Length = 50)

            => new(RandomExtensions.RandomString(Length));

        #endregion

        #region (static) Parse   (Text)

        /// <summary>
        /// Parse the given text as a charging station device identification.
        /// </summary>
        /// <param name="Text">A text representation of a charging station device identification.</param>
        public static ChargingStationDevice_Id Parse(String Text)
        {

            if (TryParse(Text, out var chargingStationDeviceId))
                return chargingStationDeviceId;

            throw new ArgumentException($"Invalid text representation of a charging station device identification: '{Text}'!",
                                        nameof(Text));

        }

        #endregion

        #region (static) TryParse(Text)

        /// <summary>
        /// Try to parse the given text as a charging station device identification.
        /// </summary>
        /// <param name="Text">A text representation of a charging station device identification.</param>
        public static ChargingStationDevice_Id? TryParse(String Text)
        {

            if (TryParse(Text, out var chargingStationDeviceId))
                return chargingStationDeviceId;

            return null;

        }

        #endregion

        #region (static) TryParse(Text, out ChargingStationDeviceId)

        /// <summary>
        /// Try to parse the given text as a charging station device identification.
        /// </summary>
        /// <param name="Text">A text representation of a charging station device identification.</param>
        /// <param name="ChargingStationDeviceId">The parsed charging station device identification.</param>
        public static Boolean TryParse(String Text, out ChargingStationDevice_Id ChargingStationDeviceId)
        {

            Text = Text.Trim();

            if (Text.IsNotNullOrEmpty())
            {
                try
                {
                    ChargingStationDeviceId = new ChargingStationDevice_Id(Text);
                    return true;
                }
                catch
                { }
            }

            ChargingStationDeviceId = default;
            return false;

        }

        #endregion

        #region Clone

        /// <summary>
        /// Clone this charging station device identification.
        /// </summary>
        public ChargingStationDevice_Id Clone

            => new (
                   new String(InternalId?.ToCharArray())
               );

        #endregion


        #region Operator overloading

        #region Operator == (ChargingStationDeviceId1, ChargingStationDeviceId2)

        /// <summary>
        /// Compares two charging station device identifications for equality.
        /// </summary>
        /// <param name="ChargingStationDeviceId1">A charging station device identification.</param>
        /// <param name="ChargingStationDeviceId2">Another charging station device identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator == (ChargingStationDevice_Id ChargingStationDeviceId1,
                                           ChargingStationDevice_Id ChargingStationDeviceId2)

            => ChargingStationDeviceId1.Equals(ChargingStationDeviceId2);

        #endregion

        #region Operator != (ChargingStationDeviceId1, ChargingStationDeviceId2)

        /// <summary>
        /// Compares two charging station device identifications for inequality.
        /// </summary>
        /// <param name="ChargingStationDeviceId1">A charging station device identification.</param>
        /// <param name="ChargingStationDeviceId2">Another charging station device identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator != (ChargingStationDevice_Id ChargingStationDeviceId1,
                                           ChargingStationDevice_Id ChargingStationDeviceId2)

            => !ChargingStationDeviceId1.Equals(ChargingStationDeviceId2);

        #endregion

        #region Operator <  (ChargingStationDeviceId1, ChargingStationDeviceId2)

        /// <summary>
        /// Compares two charging station device identifications.
        /// </summary>
        /// <param name="ChargingStationDeviceId1">A charging station device identification.</param>
        /// <param name="ChargingStationDeviceId2">Another charging station device identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator < (ChargingStationDevice_Id ChargingStationDeviceId1,
                                          ChargingStationDevice_Id ChargingStationDeviceId2)

            => ChargingStationDeviceId1.CompareTo(ChargingStationDeviceId2) < 0;

        #endregion

        #region Operator <= (ChargingStationDeviceId1, ChargingStationDeviceId2)

        /// <summary>
        /// Compares two charging station device identifications.
        /// </summary>
        /// <param name="ChargingStationDeviceId1">A charging station device identification.</param>
        /// <param name="ChargingStationDeviceId2">Another charging station device identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator <= (ChargingStationDevice_Id ChargingStationDeviceId1,
                                           ChargingStationDevice_Id ChargingStationDeviceId2)

            => ChargingStationDeviceId1.CompareTo(ChargingStationDeviceId2) <= 0;

        #endregion

        #region Operator >  (ChargingStationDeviceId1, ChargingStationDeviceId2)

        /// <summary>
        /// Compares two charging station device identifications.
        /// </summary>
        /// <param name="ChargingStationDeviceId1">A charging station device identification.</param>
        /// <param name="ChargingStationDeviceId2">Another charging station device identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator > (ChargingStationDevice_Id ChargingStationDeviceId1,
                                          ChargingStationDevice_Id ChargingStationDeviceId2)

            => ChargingStationDeviceId1.CompareTo(ChargingStationDeviceId2) > 0;

        #endregion

        #region Operator >= (ChargingStationDeviceId1, ChargingStationDeviceId2)

        /// <summary>
        /// Compares two charging station device identifications.
        /// </summary>
        /// <param name="ChargingStationDeviceId1">A charging station device identification.</param>
        /// <param name="ChargingStationDeviceId2">Another charging station device identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator >= (ChargingStationDevice_Id ChargingStationDeviceId1,
                                           ChargingStationDevice_Id ChargingStationDeviceId2)

            => ChargingStationDeviceId1.CompareTo(ChargingStationDeviceId2) >= 0;

        #endregion

        #endregion

        #region IComparable<ChargingStationDeviceId> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two charging station device identifications.
        /// </summary>
        /// <param name="Object">A charging station device identification to compare with.</param>
        public Int32 CompareTo(Object? Object)

            => Object is ChargingStationDevice_Id chargingStationDeviceId
                   ? CompareTo(chargingStationDeviceId)
                   : throw new ArgumentException("The given object is not a charging station device identification!",
                                                 nameof(Object));

        #endregion

        #region CompareTo(ChargingStationDeviceId)

        /// <summary>
        /// Compares two charging station device identifications.
        /// </summary>
        /// <param name="ChargingStationDeviceId">A charging station device identification to compare with.</param>
        public Int32 CompareTo(ChargingStationDevice_Id ChargingStationDeviceId)

            => String.Compare(InternalId,
                              ChargingStationDeviceId.InternalId,
                              StringComparison.OrdinalIgnoreCase);

        #endregion

        #endregion

        #region IEquatable<ChargingStationDeviceId> Members

        #region Equals(Object)

        /// <summary>
        /// Compares two charging station device identifications for equality.
        /// </summary>
        /// <param name="Object">A charging station device identification to compare with.</param>
        public override Boolean Equals(Object? Object)

            => Object is ChargingStationDevice_Id chargingStationDeviceId &&
                   Equals(chargingStationDeviceId);

        #endregion

        #region Equals(ChargingStationDeviceId)

        /// <summary>
        /// Compares two charging station device identifications for equality.
        /// </summary>
        /// <param name="ChargingStationDeviceId">A charging station device identification to compare with.</param>
        public Boolean Equals(ChargingStationDevice_Id ChargingStationDeviceId)

            => String.Equals(InternalId,
                             ChargingStationDeviceId.InternalId,
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
