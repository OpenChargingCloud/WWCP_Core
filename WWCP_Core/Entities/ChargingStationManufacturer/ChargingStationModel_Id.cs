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

namespace cloud.charging.open.protocols.WWCP.CSM
{

    /// <summary>
    /// Extension methods for charging station model identifications.
    /// </summary>
    public static class ChargingStationModelIdExtensions
    {

        /// <summary>
        /// Indicates whether this charging station model identification is null or empty.
        /// </summary>
        /// <param name="ChargingStationModelId">A charging station model identification.</param>
        public static Boolean IsNullOrEmpty(this ChargingStationModel_Id? ChargingStationModelId)
            => !ChargingStationModelId.HasValue || ChargingStationModelId.Value.IsNullOrEmpty;

        /// <summary>
        /// Indicates whether this charging station model identification is NOT null or empty.
        /// </summary>
        /// <param name="ChargingStationModelId">A charging station model identification.</param>
        public static Boolean IsNotNullOrEmpty(this ChargingStationModel_Id? ChargingStationModelId)
            => ChargingStationModelId.HasValue && ChargingStationModelId.Value.IsNotNullOrEmpty;

    }


    /// <summary>
    /// The unique identification of a charging station model.
    /// </summary>
    public readonly struct ChargingStationModel_Id : IId<ChargingStationModel_Id>
    {

        #region Data

        /// <summary>
        /// The internal identification.
        /// </summary>
        private readonly String InternalId;

        #endregion

        #region Properties

        /// <summary>
        /// Indicates whether this charging station model identification is null or empty.
        /// </summary>
        public Boolean IsNullOrEmpty
            => InternalId.IsNullOrEmpty();

        /// <summary>
        /// Indicates whether this charging station model identification is NOT null or empty.
        /// </summary>
        public Boolean IsNotNullOrEmpty
            => InternalId.IsNotNullOrEmpty();

        /// <summary>
        /// The length of the charging station model identification.
        /// </summary>
        public UInt64 Length
            => (UInt64) (InternalId?.Length ?? 0);

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new charging station model identification based on the given text.
        /// </summary>
        /// <param name="Text">The text representation of a charging station model identification.</param>
        private ChargingStationModel_Id(String Text)
        {
            this.InternalId = Text;
        }

        #endregion


        #region (static) NewRandom(Length = 50)

        /// <summary>
        /// Create a new random charging station model identification.
        /// </summary>
        /// <param name="Length">The expected length of the charging station model identification.</param>
        public static ChargingStationModel_Id NewRandom(Byte Length = 50)

            => new(RandomExtensions.RandomString(Length));

        #endregion

        #region (static) Parse   (Text)

        /// <summary>
        /// Parse the given text as a charging station model identification.
        /// </summary>
        /// <param name="Text">A text representation of a charging station model identification.</param>
        public static ChargingStationModel_Id Parse(String Text)
        {

            if (TryParse(Text, out var chargingStationModelId))
                return chargingStationModelId;

            throw new ArgumentException($"Invalid text representation of a charging station model identification: '{Text}'!",
                                        nameof(Text));

        }

        #endregion

        #region (static) TryParse(Text)

        /// <summary>
        /// Try to parse the given text as a charging station model identification.
        /// </summary>
        /// <param name="Text">A text representation of a charging station model identification.</param>
        public static ChargingStationModel_Id? TryParse(String Text)
        {

            if (TryParse(Text, out var chargingStationModelId))
                return chargingStationModelId;

            return null;

        }

        #endregion

        #region (static) TryParse(Text, out ChargingStationModelId)

        /// <summary>
        /// Try to parse the given text as a charging station model identification.
        /// </summary>
        /// <param name="Text">A text representation of a charging station model identification.</param>
        /// <param name="ChargingStationModelId">The parsed charging station model identification.</param>
        public static Boolean TryParse(String Text, out ChargingStationModel_Id ChargingStationModelId)
        {

            Text = Text.Trim();

            if (Text.IsNotNullOrEmpty())
            {
                try
                {
                    ChargingStationModelId = new ChargingStationModel_Id(Text);
                    return true;
                }
                catch
                { }
            }

            ChargingStationModelId = default;
            return false;

        }

        #endregion

        #region Clone()

        /// <summary>
        /// Clone this charging station model identification.
        /// </summary>
        public ChargingStationModel_Id Clone()

            => new (
                   InternalId.CloneString()
               );

        #endregion


        #region Operator overloading

        #region Operator == (ChargingStationModelId1, ChargingStationModelId2)

        /// <summary>
        /// Compares two charging station model identifications for equality.
        /// </summary>
        /// <param name="ChargingStationModelId1">A charging station model identification.</param>
        /// <param name="ChargingStationModelId2">Another charging station model identification.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator == (ChargingStationModel_Id ChargingStationModelId1,
                                           ChargingStationModel_Id ChargingStationModelId2)

            => ChargingStationModelId1.Equals(ChargingStationModelId2);

        #endregion

        #region Operator != (ChargingStationModelId1, ChargingStationModelId2)

        /// <summary>
        /// Compares two charging station model identifications for inequality.
        /// </summary>
        /// <param name="ChargingStationModelId1">A charging station model identification.</param>
        /// <param name="ChargingStationModelId2">Another charging station model identification.</param>
        /// <returns>False if both match; True otherwise.</returns>
        public static Boolean operator != (ChargingStationModel_Id ChargingStationModelId1,
                                           ChargingStationModel_Id ChargingStationModelId2)

            => !ChargingStationModelId1.Equals(ChargingStationModelId2);

        #endregion

        #region Operator <  (ChargingStationModelId1, ChargingStationModelId2)

        /// <summary>
        /// Compares two charging station model identifications.
        /// </summary>
        /// <param name="ChargingStationModelId1">A charging station model identification.</param>
        /// <param name="ChargingStationModelId2">Another charging station model identification.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator < (ChargingStationModel_Id ChargingStationModelId1,
                                          ChargingStationModel_Id ChargingStationModelId2)

            => ChargingStationModelId1.CompareTo(ChargingStationModelId2) < 0;

        #endregion

        #region Operator <= (ChargingStationModelId1, ChargingStationModelId2)

        /// <summary>
        /// Compares two charging station model identifications.
        /// </summary>
        /// <param name="ChargingStationModelId1">A charging station model identification.</param>
        /// <param name="ChargingStationModelId2">Another charging station model identification.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator <= (ChargingStationModel_Id ChargingStationModelId1,
                                           ChargingStationModel_Id ChargingStationModelId2)

            => ChargingStationModelId1.CompareTo(ChargingStationModelId2) <= 0;

        #endregion

        #region Operator >  (ChargingStationModelId1, ChargingStationModelId2)

        /// <summary>
        /// Compares two charging station model identifications.
        /// </summary>
        /// <param name="ChargingStationModelId1">A charging station model identification.</param>
        /// <param name="ChargingStationModelId2">Another charging station model identification.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator > (ChargingStationModel_Id ChargingStationModelId1,
                                          ChargingStationModel_Id ChargingStationModelId2)

            => ChargingStationModelId1.CompareTo(ChargingStationModelId2) > 0;

        #endregion

        #region Operator >= (ChargingStationModelId1, ChargingStationModelId2)

        /// <summary>
        /// Compares two charging station model identifications.
        /// </summary>
        /// <param name="ChargingStationModelId1">A charging station model identification.</param>
        /// <param name="ChargingStationModelId2">Another charging station model identification.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator >= (ChargingStationModel_Id ChargingStationModelId1,
                                           ChargingStationModel_Id ChargingStationModelId2)

            => ChargingStationModelId1.CompareTo(ChargingStationModelId2) >= 0;

        #endregion

        #endregion

        #region IComparable<ChargingStationModelId> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two charging station model identifications.
        /// </summary>
        /// <param name="Object">A charging station model identification to compare with.</param>
        public Int32 CompareTo(Object? Object)

            => Object is ChargingStationModel_Id chargingStationModelId
                   ? CompareTo(chargingStationModelId)
                   : throw new ArgumentException("The given object is not a charging station model identification!",
                                                 nameof(Object));

        #endregion

        #region CompareTo(ChargingStationModelId)

        /// <summary>
        /// Compares two charging station model identifications.
        /// </summary>
        /// <param name="ChargingStationModelId">A charging station model identification to compare with.</param>
        public Int32 CompareTo(ChargingStationModel_Id ChargingStationModelId)

            => String.Compare(InternalId,
                              ChargingStationModelId.InternalId,
                              StringComparison.OrdinalIgnoreCase);

        #endregion

        #endregion

        #region IEquatable<ChargingStationModelId> Members

        #region Equals(Object)

        /// <summary>
        /// Compares two charging station model identifications for equality.
        /// </summary>
        /// <param name="Object">A charging station model identification to compare with.</param>
        public override Boolean Equals(Object? Object)

            => Object is ChargingStationModel_Id chargingStationModelId &&
                   Equals(chargingStationModelId);

        #endregion

        #region Equals(ChargingStationModelId)

        /// <summary>
        /// Compares two charging station model identifications for equality.
        /// </summary>
        /// <param name="ChargingStationModelId">A charging station model identification to compare with.</param>
        public Boolean Equals(ChargingStationModel_Id ChargingStationModelId)

            => String.Equals(InternalId,
                             ChargingStationModelId.InternalId,
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
