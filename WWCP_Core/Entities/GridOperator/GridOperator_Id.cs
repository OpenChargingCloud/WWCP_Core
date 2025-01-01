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

using System;
using System.Text.RegularExpressions;

using org.GraphDefined.Vanaheimr.Illias;

#endregion

namespace cloud.charging.open.protocols.WWCP
{

    /// <summary>
    /// The unique identification of an Electric Vehicle Service Provider (EVSP Id).
    /// </summary>
    public class GridOperator_Id : IId,
                                        IEquatable <GridOperator_Id>,
                                        IComparable<GridOperator_Id>

    {

        #region Data

        /// <summary>
        /// The regular expression for parsing a charging station operator identification.
        /// </summary>
        public static readonly Regex  OperatorId_RegEx  = new Regex(@"^([A-Z]{2})(\*?)([A-Z0-9]{3})$ | "  +
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
        {
            get
            {

                switch (Format)
                {

                    case OperatorIdFormats.DIN:
                        return (UInt64) (CountryCode.TelefonCode.ToString().Length + 1 + Suffix.Length);

                    case OperatorIdFormats.ISO_STAR:
                        return (UInt64) (CountryCode.Alpha2Code.Length             + 1 + Suffix.Length);

                    default:  // ISO
                        return (UInt64) (CountryCode.Alpha2Code.Length                 + Suffix.Length);

                }

            }
        }

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new charging station operator identification.
        /// </summary>
        /// <param name="CountryCode">The country code.</param>
        /// <param name="Suffix">The suffix of the charging station operator identification.</param>
        /// <param name="Format">The format of the charging station operator identification.</param>
        private GridOperator_Id(Country            CountryCode,
                                           String             Suffix,
                                           OperatorIdFormats  Format = OperatorIdFormats.ISO)
        {

            #region Initial checks

            if (Suffix.IsNullOrEmpty())
                throw new ArgumentNullException(nameof(Suffix),  "The charging station operator identification suffix must not be null or empty!");

            #endregion

            this.CountryCode  = CountryCode;
            this.Suffix       = Suffix;
            this.Format       = Format;

        }

        #endregion


        #region Parse(Text)

        /// <summary>
        /// Parse the given text representation of a charging station operator identification.
        /// </summary>
        /// <param name="Text">A text representation of a charging station operator identification.</param>
        public static GridOperator_Id Parse(String Text)
        {

            #region Initial checks

            if (Text.IsNullOrEmpty())
                throw new ArgumentNullException(nameof(Text), "The given text representation of a charging station operator identification must not be null or empty!");

            #endregion

            var MatchCollection = OperatorId_RegEx.Matches(Text);

            if (MatchCollection.Count != 1)
                throw new ArgumentException("Illegal text representation of a charging station operator identification: '{Text}'!",
                                            nameof(Text));

            Country _CountryCode;

            // DE...
            if (Country.TryParseAlpha2Code(MatchCollection[0]. Groups[1].Value.ToUpper(), out _CountryCode))
                return new GridOperator_Id(_CountryCode,
                                                      MatchCollection[0].Groups[3].Value,
                                                      MatchCollection[0].Groups[2].Value == "*" ? OperatorIdFormats.ISO_STAR : OperatorIdFormats.ISO);

            // +49*...
            if (Country.TryParseTelefonCode(MatchCollection[0].Groups[4].Value.ToUpper(), out _CountryCode))
                return new GridOperator_Id(_CountryCode,
                                                      MatchCollection[0].Groups[5].Value,
                                                      OperatorIdFormats.DIN);

            // Just e.g. "822"...
            return new GridOperator_Id(Country.Germany,
                                                  MatchCollection[0].Groups[6].Value,
                                                  OperatorIdFormats.DIN);

        }

        #endregion

        #region Parse(CountryCode, Suffix, IdFormat = IdFormatType.ISO)

        /// <summary>
        /// Parse the given string as an charging station operator identification.
        /// </summary>
        /// <param name="CountryCode">A country code.</param>
        /// <param name="Suffix">The suffix of an charging station operator identification.</param>
        /// <param name="IdFormat">The format of the charging station operator identification [old|new].</param>
        public static GridOperator_Id Parse(Country            CountryCode,
                                                       String             Suffix,
                                                       OperatorIdFormats  IdFormat = OperatorIdFormats.ISO)
        {

            #region Initial checks

            if (CountryCode == null)
                throw new ArgumentNullException(nameof(CountryCode),  "The given country must not be null!");

            if (Suffix.IsNullOrEmpty())
                throw new ArgumentNullException(nameof(Suffix),       "The given charging station operator identification suffix must not be null or empty!");

            #endregion

            switch (IdFormat)
            {

                case OperatorIdFormats.ISO:
                    return Parse(CountryCode.Alpha2Code + Suffix);

                case OperatorIdFormats.ISO_STAR:
                    return Parse(CountryCode.Alpha2Code + "*" + Suffix);

                default: // DIN:
                    return Parse("+" + CountryCode.TelefonCode.ToString() + "*" + Suffix);

            }

        }

        #endregion

        #region TryParse(Text, out ChargingStationOperatorId)

        /// <summary>
        /// Try to parse the given text representation of a charging station operator identification.
        /// </summary>
        /// <param name="Text">A text representation of a charging station operator identification.</param>
        /// <param name="ChargingStationOperatorId">The parsed charging station operator identification.</param>
        public static Boolean TryParse(String                          Text,
                                       out GridOperator_Id  ChargingStationOperatorId)
        {

            #region Initial checks

            if (Text.IsNullOrEmpty())
            {
                ChargingStationOperatorId = default(GridOperator_Id);
                return false;
            }

            #endregion

            try
            {

                var MatchCollection = OperatorId_RegEx.Matches(Text);

                if (MatchCollection.Count != 1)
                {
                    ChargingStationOperatorId = default(GridOperator_Id);
                    return false;
                }

                Country _CountryCode;

                // DE...
                if (Country.TryParseAlpha2Code(MatchCollection[0].Groups[1].Value, out _CountryCode))
                {

                    ChargingStationOperatorId = new GridOperator_Id(_CountryCode,
                                                                               MatchCollection[0].Groups[3].Value,
                                                                               MatchCollection[0].Groups[2].Value == "*" ? OperatorIdFormats.ISO_STAR : OperatorIdFormats.ISO);

                    return true;

                }

                // +49*...
                if (Country.TryParseTelefonCode(MatchCollection[0].Groups[4].Value, out _CountryCode))
                {

                    ChargingStationOperatorId = new GridOperator_Id(_CountryCode,
                                                                               MatchCollection[0].Groups[5].Value,
                                                                               OperatorIdFormats.DIN);

                    return true;

                }


                // Just e.g. "822"...
                ChargingStationOperatorId = new GridOperator_Id(Country.Germany,
                                                                           MatchCollection[0].Groups[6].Value,
                                                                           OperatorIdFormats.DIN);

                return true;

            }

#pragma warning disable RCS1075  // Avoid empty catch clause that catches System.Exception.
#pragma warning disable RECS0022 // A catch clause that catches System.Exception and has an empty body
            catch
#pragma warning restore RECS0022 // A catch clause that catches System.Exception and has an empty body
#pragma warning restore RCS1075  // Avoid empty catch clause that catches System.Exception.
            { }

            ChargingStationOperatorId = default(GridOperator_Id);
            return false;

        }

        #endregion

        #region ChangeFormat(NewFormat)

        /// <summary>
        /// Return a new charging station operator identification in the given format.
        /// </summary>
        /// <param name="NewFormat">The new charging station operator identification format.</param>
        public GridOperator_Id ChangeFormat(OperatorIdFormats NewFormat)

            => new GridOperator_Id(CountryCode,
                                              Suffix,
                                              NewFormat);

        #endregion

        #region Clone

        /// <summary>
        /// Clone this charging station operator identification.
        /// </summary>
        public GridOperator_Id Clone

            => new GridOperator_Id(CountryCode,
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
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator == (GridOperator_Id ChargingStationOperatorId1, GridOperator_Id ChargingStationOperatorId2)
        {

            // If both are null, or both are same instance, return true.
            if (ReferenceEquals(ChargingStationOperatorId1, ChargingStationOperatorId2))
                return true;

            // If one is null, but not both, return false.
            if (((Object) ChargingStationOperatorId1 == null) || ((Object) ChargingStationOperatorId2 == null))
                return false;

            return ChargingStationOperatorId1.Equals(ChargingStationOperatorId2);

        }

        #endregion

        #region Operator != (ChargingStationOperatorId1, ChargingStationOperatorId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingStationOperatorId1">A charging station operator identification.</param>
        /// <param name="ChargingStationOperatorId2">Another charging station operator identification.</param>
        /// <returns>False if both match; True otherwise.</returns>
        public static Boolean operator != (GridOperator_Id ChargingStationOperatorId1, GridOperator_Id ChargingStationOperatorId2)
        {
            return !(ChargingStationOperatorId1 == ChargingStationOperatorId2);
        }

        #endregion

        #region Operator <  (ChargingStationOperatorId1, ChargingStationOperatorId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingStationOperatorId1">A charging station operator identification.</param>
        /// <param name="ChargingStationOperatorId2">Another charging station operator identification.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator < (GridOperator_Id ChargingStationOperatorId1, GridOperator_Id ChargingStationOperatorId2)
        {

            if ((Object) ChargingStationOperatorId1 == null)
                throw new ArgumentNullException("The given ChargingStationOperatorId1 must not be null!");

            return ChargingStationOperatorId1.CompareTo(ChargingStationOperatorId2) < 0;

        }

        #endregion

        #region Operator <= (ChargingStationOperatorId1, ChargingStationOperatorId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingStationOperatorId1">A charging station operator identification.</param>
        /// <param name="ChargingStationOperatorId2">Another charging station operator identification.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator <= (GridOperator_Id ChargingStationOperatorId1, GridOperator_Id ChargingStationOperatorId2)
        {
            return !(ChargingStationOperatorId1 > ChargingStationOperatorId2);
        }

        #endregion

        #region Operator >  (ChargingStationOperatorId1, ChargingStationOperatorId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingStationOperatorId1">A charging station operator identification.</param>
        /// <param name="ChargingStationOperatorId2">Another charging station operator identification.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator > (GridOperator_Id ChargingStationOperatorId1, GridOperator_Id ChargingStationOperatorId2)
        {

            if ((Object) ChargingStationOperatorId1 == null)
                throw new ArgumentNullException("The given ChargingStationOperatorId1 must not be null!");

            return ChargingStationOperatorId1.CompareTo(ChargingStationOperatorId2) > 0;

        }

        #endregion

        #region Operator >= (ChargingStationOperatorId1, ChargingStationOperatorId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingStationOperatorId1">A charging station operator identification.</param>
        /// <param name="ChargingStationOperatorId2">Another charging station operator identification.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator >= (GridOperator_Id ChargingStationOperatorId1, GridOperator_Id ChargingStationOperatorId2)
        {
            return !(ChargingStationOperatorId1 < ChargingStationOperatorId2);
        }

        #endregion

        #endregion

        #region IComparable<ChargingStationOperatorId> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        public Int32 CompareTo(Object Object)
        {

            if (Object == null)
                throw new ArgumentNullException(nameof(Object), "The given object must not be null!");

            if (!(Object is GridOperator_Id))
                throw new ArgumentException("The given object is not a charging station operator identification!", nameof(Object));

            return CompareTo((GridOperator_Id) Object);

        }

        #endregion

        #region CompareTo(ChargingStationOperatorId)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingStationOperatorId">An object to compare with.</param>
        public Int32 CompareTo(GridOperator_Id ChargingStationOperatorId)
        {

            if ((Object) ChargingStationOperatorId == null)
                throw new ArgumentNullException(nameof(ChargingStationOperatorId), "The given charging station operator identification must not be null!");

            // Compare the length of the ChargingStationOperatorIds
            var _Result = Length.CompareTo(ChargingStationOperatorId.Length);

            // If equal: Compare country codes
            if (_Result == 0)
                _Result = CountryCode.CompareTo(ChargingStationOperatorId.CountryCode);

            // If equal: Compare operator ids
            if (_Result == 0)
                _Result = String.Compare(Suffix, ChargingStationOperatorId.Suffix, StringComparison.Ordinal);

            return _Result;

        }

        #endregion

        #endregion

        #region IEquatable<ChargingStationOperatorId> Members

        #region Equals(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public override Boolean Equals(Object Object)
        {

            if (Object == null)
                return false;

            if (!(Object is GridOperator_Id))
                return false;

            return this.Equals((GridOperator_Id) Object);

        }

        #endregion

        #region Equals(ChargingStationOperatorId)

        /// <summary>
        /// Compares two ChargingStationOperatorIds for equality.
        /// </summary>
        /// <param name="ChargingStationOperatorId">A ChargingStationOperatorId to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public Boolean Equals(GridOperator_Id ChargingStationOperatorId)
        {

            if ((Object) ChargingStationOperatorId == null)
                return false;

            return CountryCode.Equals(ChargingStationOperatorId.CountryCode) &&
                   Suffix.     Equals(ChargingStationOperatorId.Suffix);

        }

        #endregion

        #endregion

        #region (override) GetHashCode()

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
        {

            switch (Format)
            {

                case OperatorIdFormats.DIN:
                    return "+" + CountryCode.TelefonCode.ToString() + "*" + Suffix;

                case OperatorIdFormats.ISO_STAR:
                    return CountryCode.Alpha2Code + "*" + Suffix;

                default: // ISO
                    return CountryCode.Alpha2Code       + Suffix;

            }

        }

        #endregion

        #region ToString(Format)

        /// <summary>
        /// Return the identification in the given format.
        /// </summary>
        /// <param name="Format">The format of the identification.</param>
        public String ToString(OperatorIdFormats Format)
        {

            switch (Format)
            {

                case OperatorIdFormats.ISO:
                    return String.Concat(CountryCode.Alpha2Code,
                                         Suffix);

                case OperatorIdFormats.ISO_STAR:
                    return String.Concat(CountryCode.Alpha2Code,
                                         "*",
                                         Suffix);

                default: // DIN
                    return String.Concat("+",
                                         CountryCode.TelefonCode,
                                         "*",
                                         Suffix);

            }

        }

        #endregion

    }

}
