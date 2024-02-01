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

using System.Text.RegularExpressions;

using org.GraphDefined.Vanaheimr.Illias;

#endregion

namespace cloud.charging.open.protocols.WWCP
{

    /// <summary>
    /// The unique identification of a charging reservation.
    /// </summary>
    public readonly struct ChargingReservation_Id : IId,
                                                    IEquatable<ChargingReservation_Id>,
                                                    IComparable<ChargingReservation_Id>
    {

        #region Data

        /// <summary>
        /// The regular expression for parsing a charging reservation identification.
        /// </summary>
        public static readonly Regex ChargingReservationId_RegEx = new (@"^([A-Z]{2}\*?[A-Z0-9]{3})\*?R([A-Za-z0-9][A-Za-z0-9\*\-]{0,250})$",
                                                                        RegexOptions.IgnorePatternWhitespace);

        #endregion

        #region Properties

        /// <summary>
        /// The charging station operator identification.
        /// </summary>
        public ChargingStationOperator_Id  OperatorId    { get; }

        /// <summary>
        /// The suffix of the charging reservation identification.
        /// </summary>
        public String                      Suffix        { get; }


        /// <summary>
        /// Indicates whether this charging reservation identification is null or empty.
        /// </summary>
        public Boolean IsNullOrEmpty
            => OperatorId.IsNullOrEmpty    || Suffix.IsNullOrEmpty();

        /// <summary>
        /// Indicates whether this charging reservation identification is NOT null or empty.
        /// </summary>
        public Boolean IsNotNullOrEmpty
            => OperatorId.IsNotNullOrEmpty && Suffix.IsNotNullOrEmpty();

        /// <summary>
        /// Returns the length of the identification.
        /// </summary>
        public UInt64 Length
            => OperatorId.Length + 2 + (UInt64) Suffix.Length;

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Generate a new charging reservation identification
        /// based on the given charging station operator and identification suffix.
        /// </summary>
        /// <param name="OperatorId">The unique identification of a charging station operator.</param>
        /// <param name="Suffix">The suffix of the charging reservation identification.</param>
        private ChargingReservation_Id(ChargingStationOperator_Id  OperatorId,
                                       String                      Suffix)
        {

            this.OperatorId  = OperatorId;
            this.Suffix      = Suffix;

        }

        #endregion


        #region (static) NewRandom(OperatorId, Length = 20)

        /// <summary>
        /// Create a new random charging reservation identification.
        /// </summary>
        /// <param name="OperatorId">The unique identification of a charging station operator.</param>
        /// <param name="Length">The expected length of the charging reservation identification suffix.</param>
        public static ChargingReservation_Id NewRandom(ChargingStationOperator_Id  OperatorId,
                                                       Byte                        Length  = 20)

            => new (OperatorId,
                    RandomExtensions.RandomString(Length));

        #endregion

        #region (static) Parse    (Text)

        /// <summary>
        /// Parse the given string as a charging reservation identification.
        /// </summary>
        /// <param name="Text">A text representation of a charging reservation identification.</param>
        public static ChargingReservation_Id Parse(String Text)
        {

            if (TryParse(Text, out var chargingReservationId))
                return chargingReservationId;

            throw new ArgumentException($"Invalid text representation of a charging reservation identification: '{Text}'!",
                                        nameof(Text));

        }

        #endregion

        #region (static) Parse    (OperatorId, Suffix)

        /// <summary>
        /// Parse the given string as a charging reservation identification.
        /// </summary>
        /// <param name="OperatorId">The unique identification of a charging station operator.</param>
        /// <param name="Suffix">The suffix of the charging reservation identification.</param>
        public static ChargingReservation_Id Parse(ChargingStationOperator_Id  OperatorId,
                                                   String                      Suffix)
        {

            if (TryParse(OperatorId, Suffix, out var chargingReservationId))
                return chargingReservationId;

            throw new ArgumentException($"Invalid text representation of a charging reservation identification: '{OperatorId}*R{Suffix}'!",
                                        nameof(Suffix));

        }

        #endregion

        #region (static) TryParse (Text)

        /// <summary>
        /// Try to parse the given string as a charging reservation identification.
        /// </summary>
        /// <param name="Text">A text representation of a charging reservation identification..</param>
        public static ChargingReservation_Id? TryParse(String Text)
        {

            if (TryParse(Text, out var chargingReservationId))
                return chargingReservationId;

            return null;

        }

        #endregion

        #region (static) TryParse (Text,               out ChargingReservationId)

        /// <summary>
        /// Try to parse the given string as a charging reservation identification.
        /// </summary>
        /// <param name="Text">A text representation of a charging reservation identification.</param>
        /// <param name="ChargingReservationId">The parsed charging reservation identification.</param>
        public static Boolean TryParse(String Text, out ChargingReservation_Id ChargingReservationId)
        {

            Text = Text.Trim();

            if (Text.IsNotNullOrEmpty())
            {
                try
                {

                    var matchCollection = ChargingReservationId_RegEx.Matches(Text);

                    if (matchCollection.Count == 1 &&
                        ChargingStationOperator_Id.TryParse(matchCollection[0].Groups[1].Value,
                                                            out var chargingStationOperatorId))
                    {

                        ChargingReservationId = new ChargingReservation_Id(
                                                    chargingStationOperatorId,
                                                    matchCollection[0].Groups[2].Value
                                                );

                        return true;

                    }

                    //if (ChargingStationOperator_Id.TryParse(matchCollection[0].Groups[3].Value, out chargingStationOperatorId))
                    //{

                    //    ChargingReservationId = new ChargingReservation_Id(chargingStationOperatorId,
                    //                                                       matchCollection[0].Groups[4].Value);

                    //    return true;

                    //}

                }
                catch
                { }
            }

            ChargingReservationId = default;
            return false;

        }

        #endregion

        #region (static) TryParse (OperatorId, Suffix, out ChargingReservationId)

        /// <summary>
        /// Try to parse the given string as a charging reservation identification.
        /// </summary>
        /// <param name="OperatorId">The unique identification of a charging station operator.</param>
        /// <param name="Suffix">The suffix of the charging reservation identification.</param>
        /// <param name="ChargingReservationId">The parsed charging reservation identification.</param>
        public static Boolean TryParse(ChargingStationOperator_Id  OperatorId,
                                       String                      Suffix,
                                       out ChargingReservation_Id  ChargingReservationId)

            => TryParse($"{OperatorId}*R{Suffix.Trim()}",
                        out ChargingReservationId);

        #endregion

        #region Clone

        /// <summary>
        /// Clone this charging reservation identification.
        /// </summary>
        public ChargingReservation_Id Clone

            => new (OperatorId.Clone,
                    new String(Suffix?.ToCharArray()));

        #endregion


        #region Operator overloading

        #region Operator == (ChargingReservationId1, ChargingReservationId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingReservationId1">A charging reservation identification.</param>
        /// <param name="ChargingReservationId2">Another charging reservation identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator == (ChargingReservation_Id ChargingReservationId1,
                                           ChargingReservation_Id ChargingReservationId2)

            => ChargingReservationId1.Equals(ChargingReservationId2);

        #endregion

        #region Operator != (ChargingReservationId1, ChargingReservationId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingReservationId1">A charging reservation identification.</param>
        /// <param name="ChargingReservationId2">Another charging reservation identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator != (ChargingReservation_Id ChargingReservationId1,
                                           ChargingReservation_Id ChargingReservationId2)

            => !ChargingReservationId1.Equals(ChargingReservationId2);

        #endregion

        #region Operator <  (ChargingReservationId1, ChargingReservationId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingReservationId1">A charging reservation identification.</param>
        /// <param name="ChargingReservationId2">Another charging reservation identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator < (ChargingReservation_Id ChargingReservationId1,
                                          ChargingReservation_Id ChargingReservationId2)

            => ChargingReservationId1.CompareTo(ChargingReservationId2) < 0;

        #endregion

        #region Operator <= (ChargingReservationId1, ChargingReservationId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingReservationId1">A charging reservation identification.</param>
        /// <param name="ChargingReservationId2">Another charging reservation identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator <= (ChargingReservation_Id ChargingReservationId1,
                                           ChargingReservation_Id ChargingReservationId2)

            => ChargingReservationId1.CompareTo(ChargingReservationId2) <= 0;

        #endregion

        #region Operator >  (ChargingReservationId1, ChargingReservationId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingReservationId1">A charging reservation identification.</param>
        /// <param name="ChargingReservationId2">Another charging reservation identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator > (ChargingReservation_Id ChargingReservationId1,
                                          ChargingReservation_Id ChargingReservationId2)

            => ChargingReservationId1.CompareTo(ChargingReservationId2) > 0;

        #endregion

        #region Operator >= (ChargingReservationId1, ChargingReservationId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingReservationId1">A charging reservation identification.</param>
        /// <param name="ChargingReservationId2">Another charging reservation identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator >= (ChargingReservation_Id ChargingReservationId1,
                                           ChargingReservation_Id ChargingReservationId2)

            => ChargingReservationId1.CompareTo(ChargingReservationId2) >= 0;

        #endregion

        #endregion

        #region IComparable<ChargingReservationId> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two charging reservation identifications.
        /// </summary>
        /// <param name="Object">A charging reservation identification to compare with.</param>
        public Int32 CompareTo(Object? Object)

            => Object is ChargingReservation_Id chargingReservationId
                   ? CompareTo(chargingReservationId)
                   : throw new ArgumentException("The given object is not a charging reservation identification!");

        #endregion

        #region CompareTo(ChargingReservationId)

        /// <summary>
        /// Compares two charging reservation identifications.
        /// </summary>
        /// <param name="ChargingReservationId">A charging reservation identification to compare with.</param>
        public Int32 CompareTo(ChargingReservation_Id ChargingReservationId)
        {

            var c = OperatorId.CompareTo(ChargingReservationId.OperatorId);

            if (c == 0)
                c = String.Compare(Suffix,
                                   ChargingReservationId.Suffix,
                                   StringComparison.Ordinal);

            return c;

        }

        #endregion

        #endregion

        #region IEquatable<ChargingReservationId> Members

        #region Equals(Object)

        /// <summary>
        /// Compares two charging reservation identifications for equality.
        /// </summary>
        /// <param name="ChargingReservationId">A charging reservation identification to compare with.</param>
        public override Boolean Equals(Object? Object)

            => Object is ChargingReservation_Id chargingReservationId &&
                   Equals(chargingReservationId);

        #endregion

        #region Equals(ChargingReservationId)

        /// <summary>
        /// Compares two charging reservation identifications for equality.
        /// </summary>
        /// <param name="ChargingReservationId">A charging reservation identification to compare with.</param>
        public Boolean Equals(ChargingReservation_Id ChargingReservationId)

            => OperatorId.Equals(ChargingReservationId.OperatorId) &&

               String.    Equals(Suffix,
                                 ChargingReservationId.Suffix,
                                 StringComparison.OrdinalIgnoreCase);

        #endregion

        #endregion

        #region (override) GetHashCode()

        /// <summary>
        /// Return the HashCode of this object.
        /// </summary>
        /// <returns>The HashCode of this object.</returns>
        public override Int32 GetHashCode()

            => OperatorId.GetHashCode() ^
              (Suffix?.   GetHashCode() ?? 0);

        #endregion

        #region (override) ToString()

        /// <summary>
        /// Return a text representation of this object.
        /// ISO-IEC-15118 – Annex H "Specification of Identifiers"
        /// </summary>
        public override String ToString()

            => $"{OperatorId}*R{Suffix}";

        #endregion

    }

}
