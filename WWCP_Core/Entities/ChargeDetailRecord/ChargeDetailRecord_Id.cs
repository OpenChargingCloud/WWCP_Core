/*
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

using System.Text.RegularExpressions;

using org.GraphDefined.Vanaheimr.Illias;

#endregion

namespace cloud.charging.open.protocols.WWCP
{

    /// <summary>
    /// Extension methods for charge detail record identifications.
    /// </summary>
    public static class ChargeDetailRecordIdExtensions
    {

        /// <summary>
        /// Indicates whether this charge detail record identification is null or empty.
        /// </summary>
        /// <param name="ChargeDetailRecordId">A charge detail record identification.</param>
        public static Boolean IsNullOrEmpty(this ChargeDetailRecord_Id? ChargeDetailRecordId)
            => !ChargeDetailRecordId.HasValue || ChargeDetailRecordId.Value.IsNullOrEmpty;

        /// <summary>
        /// Indicates whether this charge detail record identification is null or empty.
        /// </summary>
        /// <param name="ChargeDetailRecordId">A charge detail record identification.</param>
        public static Boolean IsNotNullOrEmpty(this ChargeDetailRecord_Id? ChargeDetailRecordId)
            => ChargeDetailRecordId.HasValue && ChargeDetailRecordId.Value.IsNotNullOrEmpty;

    }


    /// <summary>
    /// The unique identification of a charge detail record.
    /// </summary>
    public readonly struct ChargeDetailRecord_Id : IId,
                                                   IEquatable<ChargeDetailRecord_Id>,
                                                   IComparable<ChargeDetailRecord_Id>
    {

        #region Data

        /// <summary>
        /// The regular expression for parsing a charge detail record identification.
        /// </summary>
        public static readonly Regex ChargingReservationId_RegEx = new (@"^([A-Z]{2}\*?[A-Z0-9]{3})\*?R([A-Za-z0-9][A-Za-z0-9\*\-]{0,250})$",
                                                                        RegexOptions.IgnorePatternWhitespace);

        #endregion

        #region Properties

        /// <summary>
        /// The charging station operator identification.
        /// </summary>
        public ChargingStationOperator_Id?  OperatorId    { get; }

        /// <summary>
        /// The suffix of the charge detail record identification.
        /// </summary>
        public String                       Suffix        { get; }


        /// <summary>
        /// Indicates whether this charge detail record identification is null or empty.
        /// </summary>
        public Boolean IsNullOrEmpty
            => Suffix.IsNullOrEmpty();

        /// <summary>
        /// Indicates whether this charge detail record identification is NOT null or empty.
        /// </summary>
        public Boolean IsNotNullOrEmpty
            => Suffix.IsNotNullOrEmpty();

        /// <summary>
        /// Returns the length of the identification.
        /// </summary>
        public UInt64 Length

            => OperatorId.HasValue
                   ? OperatorId.Value.Length + 2 + ((UInt64) Suffix.Length)
                   :                                (UInt64) Suffix.Length;

        #endregion

        #region Constructor(s)

        #region ChargeDetailRecord_Id(Text)

        /// <summary>
        /// Create a new charge detail record identification.
        /// based on the given string.
        /// </summary>
        private ChargeDetailRecord_Id(String Text)
        {
            this.Suffix = Text;
        }

        #endregion

        #region ChargeDetailRecord_Id(OperatorId, Suffix)

        /// <summary>
        /// Generate a new charge detail record identification
        /// based on the given charging station operator and identification suffix.
        /// </summary>
        /// <param name="OperatorId">The unique identification of a charging station operator.</param>
        /// <param name="Suffix">The suffix of the charge detail record identification.</param>
        private ChargeDetailRecord_Id(ChargingStationOperator_Id  OperatorId,
                                      String                      Suffix)
        {

            this.OperatorId  = OperatorId;
            this.Suffix      = Suffix;

        }

        #endregion

        #endregion


        #region (static) NewRandom(Length = 20)

        /// <summary>
        /// Create a new random charge detail record identification.
        /// </summary>
        /// <param name="Length">The expected length of the charge detail record identification suffix.</param>
        public static ChargeDetailRecord_Id NewRandom(Byte Length = 30)

            => new (RandomExtensions.RandomString(Length));

        #endregion

        #region (static) NewRandom(OperatorId, Length = 20)

        /// <summary>
        /// Create a new random charge detail record identification.
        /// </summary>
        /// <param name="OperatorId">The unique identification of a charging station operator.</param>
        /// <param name="Length">The expected length of the charge detail record identification suffix.</param>
        public static ChargeDetailRecord_Id NewRandom(ChargingStationOperator_Id  OperatorId,
                                                      Byte                        Length  = 20)

            => new (OperatorId,
                    RandomExtensions.RandomString(Length));

        #endregion

        #region (static) Parse   (Text)

        /// <summary>
        /// Parse the given string as a charge detail record identification.
        /// </summary>
        /// <param name="Text">A text representation of a charge detail record identification.</param>
        public static ChargeDetailRecord_Id Parse(String Text)
        {

            if (TryParse(Text, out var chargeDetailRecordId))
                return chargeDetailRecordId;

            throw new ArgumentException($"Invalid text representation of a charge detail record identification: '{Text}'!",
                                        nameof(Text));

        }

        #endregion

        #region (static) TryParse(Text)

        /// <summary>
        /// Try to parse the given string as a charge detail record identification.
        /// </summary>
        /// <param name="Text">A text representation of a charge detail record identification.</param>
        public static ChargeDetailRecord_Id? TryParse(String Text)
        {

            if (TryParse(Text, out var chargeDetailRecordId))
                return chargeDetailRecordId;

            return null;

        }

        #endregion

        #region TryParse(Text, out ChargeDetailRecordId)

        /// <summary>
        /// Parse the given string as a charge detail record identification.
        /// </summary>
        /// <param name="Text">A text representation of a charge detail record identification.</param>
        /// <param name="ChargeDetailRecordId">The parsed charge detail record identification.</param>
        public static Boolean TryParse(String Text, out ChargeDetailRecord_Id ChargeDetailRecordId)
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

                        ChargeDetailRecordId = new ChargeDetailRecord_Id(
                                                   chargingStationOperatorId,
                                                   matchCollection[0].Groups[2].Value
                                               );

                        return true;

                    }

                    // Use the whole string as charge detail record identification!
                    ChargeDetailRecordId = new ChargeDetailRecord_Id(Text);
                    return true;

                }
                catch
                { }
            }

            ChargeDetailRecordId = default;
            return false;

        }

        #endregion

        #region (static) TryParse (OperatorId, Suffix, out ChargeDetailRecordId)

        /// <summary>
        /// Try to parse the given string as a charge detail record identification.
        /// </summary>
        /// <param name="OperatorId">The unique identification of a charging station operator.</param>
        /// <param name="Suffix">The suffix of the charge detail record identification.</param>
        /// <param name="ChargingReservationId">The parsed charge detail record identification.</param>
        public static Boolean TryParse(ChargingStationOperator_Id  OperatorId,
                                       String                      Suffix,
                                       out ChargeDetailRecord_Id   ChargeDetailRecordId)

            => TryParse($"{OperatorId}*D{Suffix.Trim()}",
                        out ChargeDetailRecordId);

        #endregion

        #region Clone

        /// <summary>
        /// Clone this charge detail record identification.
        /// </summary>
        public ChargeDetailRecord_Id Clone
        {
            get
            {

                if (OperatorId.HasValue)
                    return new(OperatorId.Value.Clone,
                                new String(Suffix?.ToCharArray()));

                return new(new String(Suffix?.ToCharArray()));

            }
        }

        #endregion


        #region Operator overloading

        #region Operator == (ChargeDetailRecordId1, ChargeDetailRecordId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargeDetailRecordId1">A charge detail record identification.</param>
        /// <param name="ChargeDetailRecordId2">Another charge detail record identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator == (ChargeDetailRecord_Id ChargeDetailRecordId1,
                                           ChargeDetailRecord_Id ChargeDetailRecordId2)

            => ChargeDetailRecordId1.Equals(ChargeDetailRecordId2);

        #endregion

        #region Operator != (ChargeDetailRecordId1, ChargeDetailRecordId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargeDetailRecordId1">A charge detail record identification.</param>
        /// <param name="ChargeDetailRecordId2">Another charge detail record identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator != (ChargeDetailRecord_Id ChargeDetailRecordId1,
                                           ChargeDetailRecord_Id ChargeDetailRecordId2)

            => !ChargeDetailRecordId1.Equals(ChargeDetailRecordId2);

        #endregion

        #region Operator <  (ChargeDetailRecordId1, ChargeDetailRecordId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargeDetailRecordId1">A charge detail record identification.</param>
        /// <param name="ChargeDetailRecordId2">Another charge detail record identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator < (ChargeDetailRecord_Id ChargeDetailRecordId1,
                                          ChargeDetailRecord_Id ChargeDetailRecordId2)

            => ChargeDetailRecordId1.CompareTo(ChargeDetailRecordId2) < 0;

        #endregion

        #region Operator <= (ChargeDetailRecordId1, ChargeDetailRecordId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargeDetailRecordId1">A charge detail record identification.</param>
        /// <param name="ChargeDetailRecordId2">Another charge detail record identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator <= (ChargeDetailRecord_Id ChargeDetailRecordId1,
                                           ChargeDetailRecord_Id ChargeDetailRecordId2)

            => ChargeDetailRecordId1.CompareTo(ChargeDetailRecordId2) <= 0;

        #endregion

        #region Operator >  (ChargeDetailRecordId1, ChargeDetailRecordId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargeDetailRecordId1">A charge detail record identification.</param>
        /// <param name="ChargeDetailRecordId2">Another charge detail record identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator > (ChargeDetailRecord_Id ChargeDetailRecordId1,
                                          ChargeDetailRecord_Id ChargeDetailRecordId2)

            => ChargeDetailRecordId1.CompareTo(ChargeDetailRecordId2) > 0;

        #endregion

        #region Operator >= (ChargeDetailRecordId1, ChargeDetailRecordId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargeDetailRecordId1">A charge detail record identification.</param>
        /// <param name="ChargeDetailRecordId2">Another charge detail record identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator >= (ChargeDetailRecord_Id ChargeDetailRecordId1,
                                           ChargeDetailRecord_Id ChargeDetailRecordId2)

            => ChargeDetailRecordId1.CompareTo(ChargeDetailRecordId2) >= 0;

        #endregion

        #endregion

        #region IComparable<ChargeDetailRecordId> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two charge detail record identifications.
        /// </summary>
        /// <param name="Object">A charge detail record identification to compare with.</param>
        public Int32 CompareTo(Object? Object)

            => Object is ChargeDetailRecord_Id chargeDetailRecordId
                   ? CompareTo(chargeDetailRecordId)
                   : throw new ArgumentException("The given object is not a charge detail record identification!");

        #endregion

        #region CompareTo(ChargeDetailRecordId)

        /// <summary>
        /// Compares two charge detail record identifications.
        /// </summary>
        /// <param name="ChargeDetailRecordId">A charge detail record identification to compare with.</param>
        public Int32 CompareTo(ChargeDetailRecord_Id ChargeDetailRecordId)
        {

            var c = OperatorId.HasValue && ChargeDetailRecordId.OperatorId.HasValue
                        ? OperatorId.Value.CompareTo(ChargeDetailRecordId.OperatorId.Value)
                        : 0;

            if (c == 0)
                c = String.Compare(Suffix,
                                   ChargeDetailRecordId.Suffix,
                                   StringComparison.Ordinal);

            return c;

        }

        #endregion

        #endregion

        #region IEquatable<ChargeDetailRecordId> Members

        #region Equals(Object)

        /// <summary>
        /// Compares two charge detail record identifications for equality.
        /// </summary>
        /// <param name="Object">A charge detail record identification to compare with.</param>
        public override Boolean Equals(Object? Object)

            => Object is ChargeDetailRecord_Id chargeDetailRecordId &&
                   Equals(chargeDetailRecordId);

        #endregion

        #region Equals(ChargeDetailRecordId)

        /// <summary>
        /// Compares two charge detail record identifications for equality.
        /// </summary>
        /// <param name="ChargeDetailRecordId">A charge detail record identification to compare with.</param>
        public Boolean Equals(ChargeDetailRecord_Id ChargeDetailRecordId)
        {

            if (OperatorId.HasValue && ChargeDetailRecordId.OperatorId.HasValue && OperatorId != ChargeDetailRecordId.OperatorId)
                return false;

            return String.Equals(Suffix,
                                 ChargeDetailRecordId.Suffix,
                                 StringComparison.OrdinalIgnoreCase);

        }

        #endregion

        #endregion

        #region (override) GetHashCode()

        /// <summary>
        /// Return the hash code of this object.
        /// </summary>
        public override Int32 GetHashCode()
        {
            unchecked
            {
                return (OperatorId?.GetHashCode() ?? 0) * 3 ^
                       (Suffix?.    GetHashCode() ?? 0);
            }
        }

        #endregion

        #region (override) ToString()

        /// <summary>
        /// Return a text representation of this object.
        /// </summary>
        public override String ToString()

            => OperatorId.HasValue
                   ? $"{OperatorId}*N{Suffix}"
                   : Suffix;

        #endregion

    }

}
