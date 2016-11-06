/*
 * Copyright (c) 2014-2016 GraphDefined GmbH <achim.friedland@graphdefined.com>
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

namespace org.GraphDefined.WWCP
{

    public enum OperatorIdFormats
    {
        OLD,
        NEW
    }

    public enum IdFormatOriginType
    {
        OLD,
        NEW,
        Origin
    }


    /// <summary>
    /// The unique identification of a charging station operator (CSO).
    /// </summary>
    public struct ChargingStationOperator_Id : IId,
                                               IEquatable<ChargingStationOperator_Id>,
                                               IComparable<ChargingStationOperator_Id>

    {

        #region Data

        /// <summary>
        /// The regular expression for parsing a charging station operator identification.
        /// </summary>
        public const String  OperatorId_RegEx            = @"^[A-Z0-9]{3}$";

        /// <summary>
        /// The regular expression for parsing an Alpha-2-CountryCode and a charging station operator identification.
        /// </summary>
        public const String  CountryAndOperatorId_RegEx  = @"^([A-Z]{2})\*?([A-Z0-9]{3})$ | ^\+?([0-9]{1,5})\*([0-9]{3})$ | ^([0-9]{3})$";

        #endregion

        #region Properties

        /// <summary>
        /// The country code.
        /// </summary>
        public Country            CountryCode    { get; }

        /// <summary>
        /// The internal charging station operator identification.
        /// </summary>
        public String             OperatorId     { get; }

        /// <summary>
        /// The format of the charging station operator identification.
        /// </summary>
        public OperatorIdFormats  Format         { get; }

        /// <summary>
        /// Returns the original representation of the charging station operator identification.
        /// </summary>
        public String             OriginId
            => ToFormat(Format);

        /// <summary>
        /// Returns the length of the identificator.
        /// </summary>
        public UInt64 Length
            => (UInt64) (CountryCode.Alpha2Code.Length + OperatorId.Length);

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new charging station operator identification.
        /// </summary>
        /// <param name="CountryCode">The country code.</param>
        /// <param name="OperatorId">The charging station operator identification.</param>
        /// <param name="Format">The format of the charging station operator identification [old|new].</param>
        private ChargingStationOperator_Id(Country            CountryCode,
                                           String             OperatorId,
                                           OperatorIdFormats  Format = OperatorIdFormats.NEW)
        {

            this.CountryCode  = CountryCode;
            this.OperatorId   = OperatorId;
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

            if (Text.Trim().IsNullOrEmpty())
                throw new ArgumentException("The given text representation of a charging station operator identification must not be null or empty!", nameof(Text));

            #endregion

            var MatchCollection = Regex.Matches(Text.Trim().ToUpper(),
                                                CountryAndOperatorId_RegEx,
                                                RegexOptions.IgnorePatternWhitespace);

            if (MatchCollection.Count != 1)
                throw new ArgumentException("Illegal text representation of a charging station operator identification: '" + Text + "'!", nameof(Text));

            Country _CountryCode;

            // DE...
            if (Country.TryParseAlpha2Code(MatchCollection[0].Groups[1].Value, out _CountryCode))
                return new ChargingStationOperator_Id(_CountryCode,
                                                      MatchCollection[0].Groups[2].Value,
                                                      OperatorIdFormats.NEW);

            // +49...
            if (Country.TryParseTelefonCode(MatchCollection[0].Groups[3].Value, out _CountryCode))
                return new ChargingStationOperator_Id(_CountryCode,
                                                      MatchCollection[0].Groups[4].Value,
                                                      OperatorIdFormats.OLD);

            throw new ArgumentException("Unknown country code in the given text representation of a charging station operator identification: '" + Text + "'!", nameof(Text));

        }

        #endregion

        #region Parse(CountryCode, OperatorId, IdFormat = IdFormatType.NEW)

        /// <summary>
        /// Parse the given string as an charging station operator identification.
        /// </summary>
        /// <param name="CountryCode">A country code.</param>
        /// <param name="OperatorId">An charging station operator identification as a string.</param>
        /// <param name="IdFormat">The format of the charging station operator identification [old|new].</param>
        public static ChargingStationOperator_Id Parse(Country            CountryCode,
                                                       String             OperatorId,
                                                       OperatorIdFormats  IdFormat = OperatorIdFormats.NEW)
        {

            #region Initial checks

            if (CountryCode == null)
                throw new ArgumentNullException(nameof(CountryCode), "The given country must not be null!");

            if (OperatorId.IsNullOrEmpty())
                throw new ArgumentNullException(nameof(OperatorId),  "The given charging station operator identification suffix must not be null or empty!");

            #endregion

            if (!Regex.IsMatch(OperatorId.Trim().ToUpper(),
                               OperatorId_RegEx,
                               RegexOptions.IgnorePatternWhitespace))

                throw new ArgumentException("Illegal charging station operator identification '" + CountryCode + "' / '" + OperatorId + "'!",
                                            nameof(OperatorId));

            return new ChargingStationOperator_Id(CountryCode,
                                                  OperatorId,
                                                  IdFormat);

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

            if (Text.IsNullOrEmpty())
            {
                ChargingStationOperatorId = default(ChargingStationOperator_Id);
                return false;
            }

            #endregion

            try
            {

                var MatchCollection = Regex.Matches(Text.Trim().ToUpper(),
                                                    CountryAndOperatorId_RegEx,
                                                    RegexOptions.IgnorePatternWhitespace);

                if (MatchCollection.Count != 1)
                {
                    ChargingStationOperatorId = default(ChargingStationOperator_Id);
                    return false;
                }

                Country _CountryCode;

                // DE...
                if (Country.TryParseAlpha2Code(MatchCollection[0].Groups[1].Value, out _CountryCode))
                {

                    ChargingStationOperatorId = new ChargingStationOperator_Id(_CountryCode,
                                                                               MatchCollection[0].Groups[2].Value,
                                                                               OperatorIdFormats.NEW);

                    return true;

                }

                // +49...
                if (Country.TryParseTelefonCode(MatchCollection[0].Groups[3].Value, out _CountryCode))
                {

                    ChargingStationOperatorId = new ChargingStationOperator_Id(_CountryCode,
                                                                               MatchCollection[0].Groups[4].Value,
                                                                               OperatorIdFormats.OLD);

                    return true;

                }


                // Just e.g. "822"...
                ChargingStationOperatorId = Parse(Country.Germany,
                                                  MatchCollection[0].Groups[5].Value).ChangeFormat(OperatorIdFormats.OLD);

                return true;

            }

#pragma warning disable RCS1075  // Avoid empty catch clause that catches System.Exception.
#pragma warning disable RECS0022 // A catch clause that catches System.Exception and has an empty body
            catch (Exception)
#pragma warning restore RECS0022 // A catch clause that catches System.Exception and has an empty body
#pragma warning restore RCS1075  // Avoid empty catch clause that catches System.Exception.
            { }

            ChargingStationOperatorId = default(ChargingStationOperator_Id);
            return false;

        }

        #endregion

        #region TryParse(CountryCode, OperatorId, out ChargingStationOperatorId, IdFormat = IdFormatType.NEW)

        /// <summary>
        /// Parse the given string as an charging station operator identification.
        /// </summary>
        /// <param name="CountryCode">A country code.</param>
        /// <param name="OperatorId">An charging station operator identification as a string.</param>
        /// <param name="ChargingStationOperatorId">The parsed charging station operator identification.</param>
        /// <param name="IdFormat">The format of the charging station operator identification [old|new].</param>
        public static Boolean TryParse(Country              CountryCode,
                                       String               OperatorId,
                                       out ChargingStationOperator_Id  ChargingStationOperatorId,
                                       OperatorIdFormats         IdFormat = OperatorIdFormats.NEW)
        {

            #region Initial checks

            if (CountryCode == null || OperatorId.IsNullOrEmpty())
            {
                ChargingStationOperatorId = default(ChargingStationOperator_Id);
                return false;
            }

            #endregion

            try
            {

                var _MatchCollection = Regex.Matches(OperatorId.Trim().ToUpper(),
                                                     OperatorId_RegEx,
                                                     RegexOptions.IgnorePatternWhitespace);

                if (_MatchCollection.Count != 1)
                {
                    ChargingStationOperatorId = default(ChargingStationOperator_Id);
                    return false;
                }

                ChargingStationOperatorId = new ChargingStationOperator_Id(CountryCode, _MatchCollection[0].Value, IdFormat);
                return true;

            }

            catch (Exception e)
            {
                ChargingStationOperatorId = default(ChargingStationOperator_Id);
                return false;
            }

        }

        #endregion

        #region ChangeFormat(NewFormat)

        /// <summary>
        /// Return a new charging station operator identification in the given format.
        /// </summary>
        /// <param name="NewFormat">The new charging station operator identification format.</param>
        public ChargingStationOperator_Id ChangeFormat(OperatorIdFormats NewFormat)

            => new ChargingStationOperator_Id(CountryCode,
                                              OperatorId,
                                              NewFormat);

        #endregion

        #region Clone

        /// <summary>
        /// Clone an ChargingStationOperatorId.
        /// </summary>
        public ChargingStationOperator_Id Clone
        {
            get
            {

                return new ChargingStationOperator_Id(CountryCode,
                                           new String(OperatorId.ToCharArray()),
                                           Format);

            }
        }

        #endregion


        #region ToFormat(Format)

        /// <summary>
        /// Return the identification in the given format.
        /// </summary>
        /// <param name="Format">The format of the identification.</param>
        public String ToFormat(OperatorIdFormats Format)
        {

            switch (Format)
            {

                case OperatorIdFormats.OLD:
                    return String.Concat("+",
                                         CountryCode.TelefonCode,
                                         "*",
                                         OperatorId);

                default:
                    return String.Concat(CountryCode.Alpha2Code,
                                         "*",
                                         OperatorId);

            }

        }

        ///// <summary>
        ///// Return the identification in the given format.
        ///// </summary>
        ///// <param name="IdFormat">The format.</param>
        //public String ToFormat(IdFormatOriginType IdFormat)
        //{

        //    if (IdFormat == IdFormatOriginType.Origin)
        //        return ToFormat(this.Format);

        //    return ToFormat((OperatorIdFormats) IdFormat);

        //}

        #endregion


        #region Operator overloading

        #region Operator == (ChargingStationOperatorId1, ChargingStationOperatorId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingStationOperatorId1">A charging station operator identification.</param>
        /// <param name="ChargingStationOperatorId2">Another charging station operator identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator == (ChargingStationOperator_Id ChargingStationOperatorId1, ChargingStationOperator_Id ChargingStationOperatorId2)
        {

            // If both are null, or both are same instance, return true.
            if (Object.ReferenceEquals(ChargingStationOperatorId1, ChargingStationOperatorId2))
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
        /// <returns>true|false</returns>
        public static Boolean operator != (ChargingStationOperator_Id ChargingStationOperatorId1, ChargingStationOperator_Id ChargingStationOperatorId2)
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
        /// <returns>true|false</returns>
        public static Boolean operator < (ChargingStationOperator_Id ChargingStationOperatorId1, ChargingStationOperator_Id ChargingStationOperatorId2)
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
        /// <returns>true|false</returns>
        public static Boolean operator <= (ChargingStationOperator_Id ChargingStationOperatorId1, ChargingStationOperator_Id ChargingStationOperatorId2)
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
        /// <returns>true|false</returns>
        public static Boolean operator > (ChargingStationOperator_Id ChargingStationOperatorId1, ChargingStationOperator_Id ChargingStationOperatorId2)
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
        /// <returns>true|false</returns>
        public static Boolean operator >= (ChargingStationOperator_Id ChargingStationOperatorId1, ChargingStationOperator_Id ChargingStationOperatorId2)
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

            if (!(Object is ChargingStationOperator_Id))
                throw new ArgumentException("The given object is not a charging station operator identification!", nameof(Object));

            return CompareTo((ChargingStationOperator_Id) Object);

        }

        #endregion

        #region CompareTo(ChargingStationOperatorId)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingStationOperatorId">An object to compare with.</param>
        public Int32 CompareTo(ChargingStationOperator_Id ChargingStationOperatorId)
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
                _Result = String.Compare(OperatorId, ChargingStationOperatorId.OperatorId, StringComparison.Ordinal);

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
        /// <returns>true|false</returns>
        public override Boolean Equals(Object Object)
        {

            if (Object == null)
                return false;

            if (!(Object is ChargingStationOperator_Id))
                return false;

            return this.Equals((ChargingStationOperator_Id) Object);

        }

        #endregion

        #region Equals(ChargingStationOperatorId)

        /// <summary>
        /// Compares two ChargingStationOperatorIds for equality.
        /// </summary>
        /// <param name="ChargingStationOperatorId">A ChargingStationOperatorId to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public Boolean Equals(ChargingStationOperator_Id ChargingStationOperatorId)
        {

            if ((Object) ChargingStationOperatorId == null)
                return false;

            return CountryCode.Equals(ChargingStationOperatorId.CountryCode) &&
                   OperatorId. Equals(ChargingStationOperatorId.OperatorId);

        }

        #endregion

        #endregion

        #region GetHashCode()

        /// <summary>
        /// Return the HashCode of this object.
        /// </summary>
        /// <returns>The HashCode of this object.</returns>
        public override Int32 GetHashCode()

            => CountryCode.Alpha2Code.GetHashCode() ^
               OperatorId.            GetHashCode();

        #endregion

        #region (override) ToString()

        /// <summary>
        /// Return a string representation of this object.
        /// </summary>
        public override String ToString()
            => OriginId;

        #endregion


    }

}
