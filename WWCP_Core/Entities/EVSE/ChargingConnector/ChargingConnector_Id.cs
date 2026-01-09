/*
 * Copyright (c) 2014-2026 GraphDefined GmbH <achim.friedland@graphdefined.com>
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
    /// A delegate for filtering charging connector identifications.
    /// </summary>
    /// <param name="ChargingConnectorId">A charging connector identification to include.</param>
    public delegate Boolean IncludeChargingConnectorIdDelegate(ChargingConnector_Id ChargingConnectorId);


    /// <summary>
    /// Extension methods for charging connector identifications.
    /// </summary>
    public static class ChargingConnectorIdExtensions
    {

        /// <summary>
        /// Indicates whether this charging connector identification is null or empty.
        /// </summary>
        /// <param name="ChargingConnectorId">A charging connector identification.</param>
        public static Boolean IsNullOrEmpty(this ChargingConnector_Id? ChargingConnectorId)
            => !ChargingConnectorId.HasValue || ChargingConnectorId.Value.IsNullOrEmpty;

        /// <summary>
        /// Indicates whether this charging connector identification is NOT null or empty.
        /// </summary>
        /// <param name="ChargingConnectorId">A charging connector identification.</param>
        public static Boolean IsNotNullOrEmpty(this ChargingConnector_Id? ChargingConnectorId)
            => ChargingConnectorId.HasValue && ChargingConnectorId.Value.IsNotNullOrEmpty;

    }


    /// <summary>
    /// The unique identification of a charging connector.
    /// CiString(3)
    /// </summary>
    public readonly struct ChargingConnector_Id : IId<ChargingConnector_Id>
    {

        #region Data

        /// <summary>
        /// The internal identification.
        /// </summary>
        private readonly String InternalId;

        #endregion

        #region Properties

        /// <summary>
        /// Indicates whether this charging connector identification is null or empty.
        /// </summary>
        public Boolean IsNullOrEmpty
            => InternalId.IsNullOrEmpty();

        /// <summary>
        /// Indicates whether this charging connector identification is NOT null or empty.
        /// </summary>
        public Boolean IsNotNullOrEmpty
            => InternalId.IsNotNullOrEmpty();

        /// <summary>
        /// The length of the charging connector identification.
        /// </summary>
        public UInt64 Length
            => (UInt64) (InternalId?.Length ?? 0);

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new charging connector identification based on the given text.
        /// </summary>
        /// <param name="Text">A text representation of a charging connector identification.</param>
        private ChargingConnector_Id(String Text)
        {
            this.InternalId = Text;
        }

        #endregion


        #region (static) Parse   (Text)

        /// <summary>
        /// Parse the given text as a charging connector identification.
        /// </summary>
        /// <param name="Text">A text representation of a charging connector identification.</param>
        public static ChargingConnector_Id Parse(String Text)
        {

            if (TryParse(Text, out var chargingConnectorId))
                return chargingConnectorId;

            throw new ArgumentException($"Invalid text representation of a charging connector identification: '{Text}'!",
                                        nameof(Text));

        }

        #endregion

        #region (static) Parse   (Number)

        /// <summary>
        /// Parse the given number as a charging connector identification.
        /// </summary>
        /// <param name="Number">A numeric representation of a charging connector identification.</param>
        public static ChargingConnector_Id Parse(UInt16 Number)
        {

            if (TryParse(Number, out var chargingConnectorId))
                return chargingConnectorId;

            throw new ArgumentException("Invalid numeric representation of a charging connector identification: '" + Number + "'!",
                                        nameof(Number));

        }

        #endregion

        #region (static) TryParse(Text)

        /// <summary>
        /// Try to parse the given text as a charging connector identification.
        /// </summary>
        /// <param name="Text">A text representation of a charging connector identification.</param>
        public static ChargingConnector_Id? TryParse(String Text)
        {

            if (TryParse(Text, out var chargingConnectorId))
                return chargingConnectorId;

            return null;

        }

        #endregion

        #region (static) TryParse(Number)

        /// <summary>
        /// Try to parse the given number as a charging connector identification.
        /// </summary>
        /// <param name="Number">A numeric representation of a charging connector identification.</param>
        public static ChargingConnector_Id? TryParse(UInt16 Number)
        {

            if (TryParse(Number, out var chargingConnectorId))
                return chargingConnectorId;

            return null;

        }

        #endregion

        #region (static) TryParse(Text,   out ChargingConnectorId)

        /// <summary>
        /// Try to parse the given text as a charging connector identification.
        /// </summary>
        /// <param name="Text">A text representation of a charging connector identification.</param>
        /// <param name="ChargingConnectorId">The parsed charging connector identification.</param>
        public static Boolean TryParse(String Text, out ChargingConnector_Id ChargingConnectorId)
        {

            Text = Text.Trim();

            if (Text.IsNotNullOrEmpty() &&
                Text.Length >= 1        &&
                Text.Length <= 3)
            {
                try
                {
                    ChargingConnectorId = new ChargingConnector_Id(Text);
                    return true;
                }
                catch
                { }
            }

            ChargingConnectorId = default;
            return false;

        }

        #endregion

        #region (static) TryParse(Number, out ChargingConnectorId)

        /// <summary>
        /// Try to parse the given number as a charging connector identification.
        /// </summary>
        /// <param name="Number">A numeric representation of a charging connector identification.</param>
        /// <param name="ChargingConnectorId">The parsed charging connector identification.</param>
        public static Boolean TryParse(UInt16 Number, out ChargingConnector_Id ChargingConnectorId)
        {

            try
            {
                ChargingConnectorId = new ChargingConnector_Id(Number.ToString());
                return true;
            }
            catch
            { }

            ChargingConnectorId = default;
            return false;

        }

        #endregion

        #region Clone()

        /// <summary>
        /// Clone this charging connector identification.
        /// </summary>
        public ChargingConnector_Id Clone()

            => new (
                   InternalId.CloneString()
               );

        #endregion


        #region Operator overloading

        #region Operator == (ChargingConnectorId1, ChargingConnectorId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingConnectorId1">A charging connector identification.</param>
        /// <param name="ChargingConnectorId2">Another charging connector identification.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator == (ChargingConnector_Id ChargingConnectorId1,
                                           ChargingConnector_Id ChargingConnectorId2)

            => ChargingConnectorId1.Equals(ChargingConnectorId2);

        #endregion

        #region Operator != (ChargingConnectorId1, ChargingConnectorId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingConnectorId1">A charging connector identification.</param>
        /// <param name="ChargingConnectorId2">Another charging connector identification.</param>
        /// <returns>False if both match; True otherwise.</returns>
        public static Boolean operator != (ChargingConnector_Id ChargingConnectorId1,
                                           ChargingConnector_Id ChargingConnectorId2)

            => !ChargingConnectorId1.Equals(ChargingConnectorId2);

        #endregion

        #region Operator <  (ChargingConnectorId1, ChargingConnectorId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingConnectorId1">A charging connector identification.</param>
        /// <param name="ChargingConnectorId2">Another charging connector identification.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator < (ChargingConnector_Id ChargingConnectorId1,
                                          ChargingConnector_Id ChargingConnectorId2)

            => ChargingConnectorId1.CompareTo(ChargingConnectorId2) < 0;

        #endregion

        #region Operator <= (ChargingConnectorId1, ChargingConnectorId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingConnectorId1">A charging connector identification.</param>
        /// <param name="ChargingConnectorId2">Another charging connector identification.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator <= (ChargingConnector_Id ChargingConnectorId1,
                                           ChargingConnector_Id ChargingConnectorId2)

            => ChargingConnectorId1.CompareTo(ChargingConnectorId2) <= 0;

        #endregion

        #region Operator >  (ChargingConnectorId1, ChargingConnectorId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingConnectorId1">A charging connector identification.</param>
        /// <param name="ChargingConnectorId2">Another charging connector identification.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator > (ChargingConnector_Id ChargingConnectorId1,
                                          ChargingConnector_Id ChargingConnectorId2)

            => ChargingConnectorId1.CompareTo(ChargingConnectorId2) > 0;

        #endregion

        #region Operator >= (ChargingConnectorId1, ChargingConnectorId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingConnectorId1">A charging connector identification.</param>
        /// <param name="ChargingConnectorId2">Another charging connector identification.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator >= (ChargingConnector_Id ChargingConnectorId1,
                                           ChargingConnector_Id ChargingConnectorId2)

            => ChargingConnectorId1.CompareTo(ChargingConnectorId2) >= 0;

        #endregion

        #endregion

        #region IComparable<ChargingConnectorId> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two charging connector identifications.
        /// </summary>
        /// <param name="Object">A charging connector identification to compare with.</param>
        public Int32 CompareTo(Object? Object)

            => Object is ChargingConnector_Id chargingConnectorId
                   ? CompareTo(chargingConnectorId)
                   : throw new ArgumentException("The given object is not a charging connector identification!",
                                                 nameof(Object));

        #endregion

        #region CompareTo(ChargingConnectorId)

        /// <summary>
        /// Compares two charging connector identifications.
        /// </summary>
        /// <param name="ChargingConnectorId">A charging connector identification to compare with.</param>
        public Int32 CompareTo(ChargingConnector_Id ChargingConnectorId)

            => String.Compare(InternalId,
                              ChargingConnectorId.InternalId,
                              StringComparison.OrdinalIgnoreCase);

        #endregion

        #endregion

        #region IEquatable<ChargingConnectorId> Members

        #region Equals(Object)

        /// <summary>
        /// Compares two charging connector identifications for equality.
        /// </summary>
        /// <param name="Object">A charging connector identification to compare with.</param>
        public override Boolean Equals(Object? Object)

            => Object is ChargingConnector_Id chargingConnectorId &&
                   Equals(chargingConnectorId);

        #endregion

        #region Equals(ChargingConnectorId)

        /// <summary>
        /// Compares two charging connector identifications for equality.
        /// </summary>
        /// <param name="ChargingConnectorId">A charging connector identification to compare with.</param>
        public Boolean Equals(ChargingConnector_Id ChargingConnectorId)

            => String.Equals(InternalId,
                             ChargingConnectorId.InternalId,
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
