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

using System.Text.RegularExpressions;

using org.GraphDefined.Vanaheimr.Illias;

#endregion

namespace cloud.charging.open.protocols.WWCP
{

    /// <summary>
    /// The different formats of charging station operator identifications.
    /// </summary>
    public enum OperatorIdFormats
    {

        /// <summary>
        /// The old DIN format.
        /// </summary>
        DIN,

        /// <summary>
        /// The new ISO format.
        /// </summary>
        ISO,

        /// <summary>
        /// The new ISO format with a '*' as separator.
        /// </summary>
        ISO_STAR

    }


    /// <summary>
    /// Extension methods for charging station operator identifications.
    /// </summary>
    public static class ChargingStationOperatorIdExtensions
    {

        /// <summary>
        /// Indicates whether this charging station operator identification is null or empty.
        /// </summary>
        /// <param name="ChargingStationOperatorId">A charging station operator identification.</param>
        public static Boolean IsNullOrEmpty(this ChargingStationOperator_Id? ChargingStationOperatorId)
            => !ChargingStationOperatorId.HasValue || ChargingStationOperatorId.Value.IsNullOrEmpty;

        /// <summary>
        /// Indicates whether this charging station operator identification is null or empty.
        /// </summary>
        /// <param name="ChargingStationOperatorId">A charging station operator identification.</param>
        public static Boolean IsNotNullOrEmpty(this ChargingStationOperator_Id? ChargingStationOperatorId)
            => ChargingStationOperatorId.HasValue && ChargingStationOperatorId.Value.IsNotNullOrEmpty;

    }


    /// <summary>
    /// The unique identification of a charging station operator (CSO).
    /// </summary>
    public readonly struct ChargingStationOperator_Id : IId,
                                                        IEquatable<ChargingStationOperator_Id>,
                                                        IComparable<ChargingStationOperator_Id>

    {

        #region Data

        /// <summary>
        /// The regular expression for parsing a charging station operator identification.
        /// </summary>
        public static readonly Regex  OperatorId_RegEx  = new (@"^([A-Z]{2})(\*?)([A-Z0-9]{3})$ | "  +
                                                               @"^\+?([0-9]{1,5})\*([0-9]{3,6})$ | " +
                                                               @"^([0-9]{1,5})$",
                                                               RegexOptions.IgnorePatternWhitespace);

        #endregion

        #region Properties

        /// <summary>
        /// The country code.
        /// </summary>
        public Country            CountryCode   { get; }

        /// <summary>
        /// The identificator suffix.
        /// </summary>
        public String             Suffix        { get; }

        /// <summary>
        /// The format of the charging station operator identification.
        /// </summary>
        public OperatorIdFormats  Format        { get; }

        /// <summary>
        /// Indicates whether this identification is null or empty.
        /// </summary>
        public Boolean IsNullOrEmpty
            => Suffix.IsNullOrEmpty();

        /// <summary>
        /// Indicates whether this identification is NOT null or empty.
        /// </summary>
        public Boolean IsNotNullOrEmpty
            => Suffix.IsNotNullOrEmpty();

        /// <summary>
        /// Returns the length of the identification.
        /// </summary>
        public UInt64 Length

            => Format switch {
                   OperatorIdFormats.DIN       => (UInt64) (CountryCode.TelefonCode.ToString().Length + 1 + Suffix.Length),
                   OperatorIdFormats.ISO_STAR  => (UInt64) (CountryCode.Alpha2Code.Length             + 1 + Suffix.Length),
                   _                           => (UInt64) (CountryCode.Alpha2Code.Length             +     Suffix.Length),
               };

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new charging station operator identification.
        /// </summary>
        /// <param name="CountryCode">The country code.</param>
        /// <param name="Suffix">The suffix of the charging station operator identification.</param>
        /// <param name="Format">The format of the charging station operator identification.</param>
        private ChargingStationOperator_Id(Country            CountryCode,
                                           String             Suffix,
                                           OperatorIdFormats  Format = OperatorIdFormats.ISO)
        {

            if (Suffix.IsNullOrEmpty())
                throw new ArgumentNullException(nameof(Suffix),  "The charging station operator identification suffix must not be null or empty!");

            this.CountryCode  = CountryCode;
            this.Suffix       = Suffix.Trim();
            this.Format       = Format;

        }

        #endregion


        #region Parse(Text)

        /// <summary>
        /// Parse the given text representation of a charging station operator identification.
        /// </summary>
        /// <param name="Text">A text representation of a charging station operator identification.</param>
        public static ChargingStationOperator_Id Parse(String Text)
        {

            #region Initial checks

            if (Text.IsNullOrEmpty())
                throw new ArgumentNullException(nameof(Text), "The given text representation of a charging station operator identification must not be null or empty!");

            #endregion

            var matchCollection = OperatorId_RegEx.Matches(Text.ToUpper());

            if (matchCollection.Count != 1)
                throw new ArgumentException("Illegal text representation of a charging station operator identification: '" + Text + "'!",
                                            nameof(Text));

            // DE...
            if (Country.TryParseAlpha2Code(matchCollection[0]. Groups[1].Value.ToUpper(), out Country _CountryCode))
                return new ChargingStationOperator_Id(_CountryCode,
                                                      matchCollection[0].Groups[3].Value,
                                                      matchCollection[0].Groups[2].Value == "*" ? OperatorIdFormats.ISO_STAR : OperatorIdFormats.ISO);

            // +49*...
            if (Country.TryParseTelefonCode(matchCollection[0].Groups[4].Value.ToUpper(), out _CountryCode))
                return new ChargingStationOperator_Id(_CountryCode,
                                                      matchCollection[0].Groups[5].Value,
                                                      OperatorIdFormats.DIN);

            // Just e.g. "822"...
            return new ChargingStationOperator_Id(Country.Germany,
                                                  matchCollection[0].Groups[6].Value,
                                                  OperatorIdFormats.DIN);

        }

        #endregion

        #region Parse(CountryCode, Suffix, IdFormat = IdFormatType.ISO_STAR)

        /// <summary>
        /// Parse the given string as an charging station operator identification.
        /// </summary>
        /// <param name="CountryCode">A country code.</param>
        /// <param name="Suffix">The suffix of an charging station operator identification.</param>
        /// <param name="IdFormat">The format of the charging station operator identification [old|new].</param>
        public static ChargingStationOperator_Id Parse(Country            CountryCode,
                                                       String             Suffix,
                                                       OperatorIdFormats  IdFormat = OperatorIdFormats.ISO_STAR)
        {

            #region Initial checks

            if (CountryCode is null)
                throw new ArgumentNullException(nameof(CountryCode),  "The given country must not be null!");

            if (Suffix.IsNullOrEmpty())
                throw new ArgumentNullException(nameof(Suffix),       "The given charging station operator identification suffix must not be null or empty!");

            #endregion

            switch (IdFormat)
            {

                case OperatorIdFormats.ISO:
                    return Parse(CountryCode.Alpha2Code + Suffix.Trim());

                case OperatorIdFormats.ISO_STAR:
                    return Parse(CountryCode.Alpha2Code + "*" + Suffix.Trim());

                default:
                    return Parse("+" + CountryCode.TelefonCode.ToString() + "*" + Suffix.Trim());

            }

        }

        #endregion

        #region TryParse(Text)

        /// <summary>
        /// Try to parse the given text representation of a charging station operator identification.
        /// </summary>
        /// <param name="Text">A text representation of a charging station operator identification.</param>
        public static ChargingStationOperator_Id? TryParse(String Text)
        {

            if (TryParse(Text, out ChargingStationOperator_Id chargingStationOperatorId))
                return chargingStationOperatorId;

            return null;

        }

        #endregion

        #region TryParse(Text, out ChargingStationOperatorId)

        /// <summary>
        /// Try to parse the given text representation of a charging station operator identification.
        /// </summary>
        /// <param name="Text">A text representation of a charging station operator identification.</param>
        /// <param name="ChargingStationOperatorId">The parsed charging station operator identification.</param>
        public static Boolean TryParse(String                          Text,
                                       out ChargingStationOperator_Id  ChargingStationOperatorId)
        {

            #region Initial checks

            ChargingStationOperatorId = default;

            if (Text.IsNullOrEmpty())
                return false;

            #endregion

            try
            {

                var matchCollection = OperatorId_RegEx.Matches(Text.ToUpper());

                if (matchCollection.Count != 1)
                    return false;

                // DE...
                if (Country.TryParseAlpha2Code(matchCollection[0].Groups[1].Value, out Country countryCode))
                {

                    ChargingStationOperatorId = new ChargingStationOperator_Id(countryCode,
                                                                               matchCollection[0].Groups[3].Value,
                                                                               matchCollection[0].Groups[2].Value == "*" ? OperatorIdFormats.ISO_STAR : OperatorIdFormats.ISO);

                    return true;

                }

                // +49*...
                if (Country.TryParseTelefonCode(matchCollection[0].Groups[4].Value, out countryCode))
                {

                    ChargingStationOperatorId = new ChargingStationOperator_Id(countryCode,
                                                                               matchCollection[0].Groups[5].Value,
                                                                               OperatorIdFormats.DIN);

                    return true;

                }


                // Just e.g. "822"...
                ChargingStationOperatorId = new ChargingStationOperator_Id(Country.Germany,
                                                                           matchCollection[0].Groups[6].Value,
                                                                           OperatorIdFormats.DIN);

                return true;

            }

            catch (Exception)
            { }

            return false;

        }

        #endregion

        #region ChangeFormat(NewFormat)

        /// <summary>
        /// Return a new charging station operator identification in the given format.
        /// </summary>
        /// <param name="NewFormat">The new charging station operator identification format.</param>
        public ChargingStationOperator_Id ChangeFormat(OperatorIdFormats NewFormat)

            => new ChargingStationOperator_Id(CountryCode,
                                              Suffix,
                                              NewFormat);

        #endregion

        #region Clone

        /// <summary>
        /// Clone this charging station operator identification.
        /// </summary>
        public ChargingStationOperator_Id Clone

            => new ChargingStationOperator_Id(CountryCode,
                                              Suffix,
                                              Format);

        #endregion


        #region Operator overloading

        #region Operator == (ChargingStationOperatorId1, ChargingStationOperatorId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingStationOperatorId1">A charging station operator identification.</param>
        /// <param name="ChargingStationOperatorId2">Another charging station operator identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator == (ChargingStationOperator_Id ChargingStationOperatorId1,
                                           ChargingStationOperator_Id ChargingStationOperatorId2)

            => ChargingStationOperatorId1.Equals(ChargingStationOperatorId2);

        #endregion

        #region Operator != (ChargingStationOperatorId1, ChargingStationOperatorId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingStationOperatorId1">A charging station operator identification.</param>
        /// <param name="ChargingStationOperatorId2">Another charging station operator identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator != (ChargingStationOperator_Id ChargingStationOperatorId1,
                                           ChargingStationOperator_Id ChargingStationOperatorId2)

            => !ChargingStationOperatorId1.Equals(ChargingStationOperatorId2);

        #endregion

        #region Operator <  (ChargingStationOperatorId1, ChargingStationOperatorId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingStationOperatorId1">A charging station operator identification.</param>
        /// <param name="ChargingStationOperatorId2">Another charging station operator identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator < (ChargingStationOperator_Id ChargingStationOperatorId1,
                                          ChargingStationOperator_Id ChargingStationOperatorId2)

            => ChargingStationOperatorId1.CompareTo(ChargingStationOperatorId2) < 0;

        #endregion

        #region Operator <= (ChargingStationOperatorId1, ChargingStationOperatorId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingStationOperatorId1">A charging station operator identification.</param>
        /// <param name="ChargingStationOperatorId2">Another charging station operator identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator <= (ChargingStationOperator_Id ChargingStationOperatorId1,
                                           ChargingStationOperator_Id ChargingStationOperatorId2)

            => ChargingStationOperatorId1.CompareTo(ChargingStationOperatorId2) <= 0;

        #endregion

        #region Operator >  (ChargingStationOperatorId1, ChargingStationOperatorId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingStationOperatorId1">A charging station operator identification.</param>
        /// <param name="ChargingStationOperatorId2">Another charging station operator identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator > (ChargingStationOperator_Id ChargingStationOperatorId1,
                                          ChargingStationOperator_Id ChargingStationOperatorId2)

            => ChargingStationOperatorId1.CompareTo(ChargingStationOperatorId2) > 0;

        #endregion

        #region Operator >= (ChargingStationOperatorId1, ChargingStationOperatorId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingStationOperatorId1">A charging station operator identification.</param>
        /// <param name="ChargingStationOperatorId2">Another charging station operator identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator >= (ChargingStationOperator_Id ChargingStationOperatorId1,
                                           ChargingStationOperator_Id ChargingStationOperatorId2)

            => ChargingStationOperatorId1.CompareTo(ChargingStationOperatorId2) >= 0;

        #endregion

        #endregion

        #region IComparable<ChargingStationOperatorId> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        public Int32 CompareTo(Object? Object)

            => Object is ChargingStationOperator_Id chargingStationOperatorId
                   ? CompareTo(chargingStationOperatorId)
                   : throw new ArgumentException("The given object is not a charging station operator identification!",
                                                 nameof(Object));

        #endregion

        #region CompareTo(ChargingStationOperatorId)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingStationOperatorId">An object to compare with.</param>
        public Int32 CompareTo(ChargingStationOperator_Id ChargingStationOperatorId)
        {

            var c = CountryCode.CompareTo(ChargingStationOperatorId.CountryCode);

            if (c == 0)
                c = String.Compare(Suffix,
                                   ChargingStationOperatorId.Suffix,
                                   StringComparison.OrdinalIgnoreCase);

            return c;

        }

        #endregion

        #endregion

        #region IEquatable<ChargingStationOperatorId> Members

        #region Equals(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        /// <returns>true|false</returns>
        public override Boolean Equals(Object? Object)

            => Object is ChargingStationOperator_Id chargingStationOperatorId &&
                   Equals(chargingStationOperatorId);

        #endregion

        #region Equals(ChargingStationOperatorId)

        /// <summary>
        /// Compares two ChargingStationOperatorIds for equality.
        /// </summary>
        /// <param name="ChargingStationOperatorId">A ChargingStationOperatorId to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public Boolean Equals(ChargingStationOperator_Id ChargingStationOperatorId)

            => CountryCode.Equals(ChargingStationOperatorId.CountryCode) &&

               String.Equals(Suffix,
                             ChargingStationOperatorId.Suffix,
                             StringComparison.OrdinalIgnoreCase);

        #endregion

        #endregion

        #region GetHashCode()

        /// <summary>
        /// Return the HashCode of this object.
        /// </summary>
        /// <returns>The HashCode of this object.</returns>
        public override Int32 GetHashCode()

            => CountryCode.GetHashCode() ^
               Suffix.     GetHashCode();

        #endregion

        #region (override) ToString()

        /// <summary>
        /// Return a text representation of this object.
        /// </summary>
        public override String ToString()

            => ToString(Format);

        #endregion

        #region ToString(Format)

        /// <summary>
        /// Return the identification in the given format.
        /// </summary>
        /// <param name="Format">The format of the identification.</param>
        public String ToString(OperatorIdFormats Format)

            => Format switch {
                   OperatorIdFormats.DIN       => String.Concat("+", CountryCode.TelefonCode.ToString(), "*", Suffix),
                   OperatorIdFormats.ISO_STAR  => String.Concat(     CountryCode.Alpha2Code,             "*", Suffix),
                   _                           => String.Concat(     CountryCode.Alpha2Code,                  Suffix)
               };

        #endregion

    }

}
