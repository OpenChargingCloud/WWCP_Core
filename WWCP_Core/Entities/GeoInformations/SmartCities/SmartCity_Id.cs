/*
 * Copyright (c) 2014-2018 GraphDefined GmbH <achim.friedland@graphdefined.com>
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

    /// <summary>
    /// The unique identification of a smart city.
    /// </summary>
    public class SmartCity_Id : IId,
                                IEquatable <SmartCity_Id>,
                                IComparable<SmartCity_Id>

    {

        #region Data

        /// <summary>
        /// The regular expression for parsing a smart city identification.
        /// </summary>
        public const String SmartCityId_RegEx  = @"^([A-Za-z]{2}\-?(.{0,60})$";

        #endregion

        #region Properties

        /// <summary>
        /// The internal Alpha-2-CountryCode.
        /// </summary>
        public Country CountryCode  { get; }

        /// <summary>
        /// The internal smart city identification.
        /// </summary>
        public String  Suffix       { get; }

        /// <summary>
        /// Returns the length of the identification.
        /// </summary>
        public UInt64 Length
            => 2 + 2 + (UInt64) Suffix.Length;

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Generate a new smart city identification
        /// based on the given string.
        /// </summary>
        /// <param name="CountryCode">The country code.</param>
        /// <param name="Suffix">The suffix of the smart city identification.</param>
        private SmartCity_Id(Country  CountryCode,
                             String   Suffix)
        {

            this.CountryCode  = CountryCode;
            this.Suffix       = Suffix;

        }

        #endregion


        #region Parse(Text)

        /// <summary>
        /// Parse the given string as a smart city identification.
        /// </summary>
        /// <param name="Text">The smart city identification as a string.</param>
        public static SmartCity_Id Parse(String Text)
        {

            #region Initial checks

            if (Text.Trim().IsNullOrEmpty())
                throw new ArgumentNullException(nameof(Text),  "The given text must not be null or empty!");

            #endregion

            var _MatchCollection = Regex.Matches(Text.Trim().ToUpper(),
                                                 SmartCityId_RegEx,
                                                 RegexOptions.IgnorePatternWhitespace);

            if (_MatchCollection.Count != 1)
                throw new ArgumentException("Illegal smart city identification '" + Text + "'!", nameof(Text));

            Country __CountryCode;

            if (Country.TryParseAlpha2Code(_MatchCollection[0].Groups[1].Value, out __CountryCode))
                return new SmartCity_Id(__CountryCode,
                                        _MatchCollection[0].Groups[2].Value);

            throw new ArgumentException("Illegal smart city identification!", nameof(Text));

        }

        #endregion

        #region Parse(CountryCode, Text)

        /// <summary>
        /// Parse the given string as a smart city identification.
        /// </summary>
        /// <param name="CountryCode">A country code.</param>
        /// <param name="Text">A smart city identification as a string.</param>
        public static SmartCity_Id Parse(Country  CountryCode,
                                         String   Text)
        {

            #region Initial checks

            if (CountryCode == null)
                throw new ArgumentNullException(nameof(CountryCode),  "The country code must not be null!");

            if (Text.IsNullOrEmpty())
                throw new ArgumentNullException(nameof(Text),   "The provider identification must not be null or empty!");

            #endregion

            var _MatchCollection = Regex.Matches(Text.Trim().ToUpper(),
                                                 SmartCityId_RegEx,
                                                 RegexOptions.IgnorePatternWhitespace);

            if (_MatchCollection.Count != 1)
                throw new ArgumentException("Illegal smart city identification '" + CountryCode + " / " + Text + "'!", nameof(Text));

            return new SmartCity_Id(CountryCode,
                                    _MatchCollection[0].Value);

        }

        #endregion

        #region TryParse(Text, out SmartCityId)

        /// <summary>
        /// Parse the given string as a smart city identification.
        /// </summary>
        /// <param name="Text">The country code and smart city identification as a string.</param>
        /// <param name="SmartCityId">The parsed smart city identification.</param>
        public static Boolean TryParse(String            Text,
                                       out SmartCity_Id  SmartCityId)
        {

            #region Initial checks

            if (Text.IsNullOrEmpty())
            {
                SmartCityId = null;
                return false;
            }

            #endregion

            try
            {

                var _MatchCollection = Regex.Matches(Text.Trim().ToUpper(),
                                                     SmartCityId_RegEx,
                                                     RegexOptions.IgnorePatternWhitespace);

                if (_MatchCollection.Count != 1)
                {
                    SmartCityId = null;
                    return false;
                }

                Country _CountryCode;

                if (Country.TryParseAlpha2Code(_MatchCollection[0].Groups[1].Value, out _CountryCode))
                {

                    SmartCityId = new SmartCity_Id(_CountryCode,
                                                   _MatchCollection[0].Groups[2].Value);

                    return true;

                }

            }

            catch (Exception)
            {
                SmartCityId = null;
                return false;
            }

            SmartCityId = null;
            return false;

        }

        #endregion

        #region TryParse(CountryCode, Text, out EVSEProviderId, IdFormat = IdFormatType.NEW)

        /// <summary>
        /// Parse the given string as a smart city identification.
        /// </summary>
        /// <param name="CountryCode">A country code.</param>
        /// <param name="Text">A smart city identification as a string.</param>
        /// <param name="SmartCityId">The parsed smart city identification.</param>
        public static Boolean TryParse(Country           CountryCode,
                                       String            Text,
                                       out SmartCity_Id  SmartCityId)
        {

            #region Initial checks

            if (CountryCode == null || Text.IsNullOrEmpty())
            {
                SmartCityId = null;
                return false;
            }

            #endregion

            try
            {

                var _MatchCollection = Regex.Matches(Text.Trim().ToUpper(),
                                                     SmartCityId_RegEx,
                                                     RegexOptions.IgnorePatternWhitespace);

                if (_MatchCollection.Count != 1)
                {
                    SmartCityId = null;
                    return false;
                }

                SmartCityId = new SmartCity_Id(CountryCode,
                                               _MatchCollection[0].Value);

                return true;

            }

            catch (Exception)
            {
                SmartCityId = null;
                return false;
            }

        }

        #endregion

        #region Clone

        /// <summary>
        /// Clone this smart city identification.
        /// </summary>

        public SmartCity_Id Clone

            => new SmartCity_Id(CountryCode,
                                new String(Suffix.ToCharArray()));

        #endregion


        #region Operator overloading

        #region Operator == (SmartCityId1, SmartCityId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="SmartCityId1">A smart city.</param>
        /// <param name="SmartCityId2">Another smart city.</param>
        /// <returns>true|false</returns>
        public static Boolean operator == (SmartCity_Id  SmartCityId1,
                                           SmartCity_Id  SmartCityId2)
        {

            // If both are null, or both are same instance, return true.
            if (Object.ReferenceEquals(SmartCityId1, SmartCityId2))
                return true;

            // If one is null, but not both, return false.
            if (((Object) SmartCityId1 == null) || ((Object) SmartCityId2 == null))
                return false;

            return SmartCityId1.Equals(SmartCityId2);

        }

        #endregion

        #region Operator != (SmartCityId1, SmartCityId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="SmartCityId1">A smart city.</param>
        /// <param name="SmartCityId2">Another smart city.</param>
        /// <returns>true|false</returns>
        public static Boolean operator != (SmartCity_Id  SmartCityId1,
                                           SmartCity_Id  SmartCityId2)

            => !(SmartCityId1 == SmartCityId2);

        #endregion

        #region Operator <  (SmartCityId1, SmartCityId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="SmartCityId1">A smart city.</param>
        /// <param name="SmartCityId2">Another smart city.</param>
        /// <returns>true|false</returns>
        public static Boolean operator < (SmartCity_Id  SmartCityId1,
                                          SmartCity_Id  SmartCityId2)
        {

            if ((Object) SmartCityId1 == null)
                throw new ArgumentNullException(nameof(SmartCityId1),  "The given smart city identification must not be null!");

            return SmartCityId1.CompareTo(SmartCityId2) < 0;

        }

        #endregion

        #region Operator <= (SmartCityId1, SmartCityId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="SmartCityId1">A smart city.</param>
        /// <param name="SmartCityId2">Another smart city.</param>
        /// <returns>true|false</returns>
        public static Boolean operator <= (SmartCity_Id  SmartCityId1,
                                           SmartCity_Id  SmartCityId2)

            => !(SmartCityId1 > SmartCityId2);

        #endregion

        #region Operator >  (SmartCityId1, SmartCityId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="SmartCityId1">A smart city.</param>
        /// <param name="SmartCityId2">Another smart city.</param>
        /// <returns>true|false</returns>
        public static Boolean operator > (SmartCity_Id  SmartCityId1,
                                          SmartCity_Id  SmartCityId2)
        {

            if ((Object) SmartCityId1 == null)
                throw new ArgumentNullException(nameof(SmartCityId1),  "The given smart city identification must not be null!");

            return SmartCityId1.CompareTo(SmartCityId2) > 0;

        }

        #endregion

        #region Operator >= (SmartCityId1, SmartCityId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="SmartCityId1">A smart city.</param>
        /// <param name="SmartCityId2">Another smart city.</param>
        /// <returns>true|false</returns>
        public static Boolean operator >= (SmartCity_Id  SmartCityId1,
                                           SmartCity_Id  SmartCityId2)

            => !(SmartCityId1 < SmartCityId2);

        #endregion

        #endregion

        #region IComparable<SmartCityId> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        public Int32 CompareTo(Object Object)
        {

            if (Object == null)
                throw new ArgumentNullException(nameof(Object),  "The given object must not be null!");

            // Check if the given object is an smart city.
            var SmartCityId = Object as SmartCity_Id;
            if ((Object) SmartCityId == null)
                throw new ArgumentException("The given object is not a smart city identification!");

            return CompareTo(SmartCityId);

        }

        #endregion

        #region CompareTo(SmartCityId)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="SmartCityId">An object to compare with.</param>
        public Int32 CompareTo(SmartCity_Id SmartCityId)
        {

            if ((Object) SmartCityId == null)
                throw new ArgumentNullException(nameof(SmartCityId),  "The given smart city identification must not be null!");

            // Compare the length of the smart city identifications
            var _Result = this.Length.CompareTo(SmartCityId.Length);

            // If equal: Compare country codes
            if (_Result == 0)
                _Result = CountryCode.CompareTo(SmartCityId.CountryCode);

            if (_Result == 0)
                _Result = String.Compare(Suffix, SmartCityId.Suffix, StringComparison.Ordinal);

            return _Result;

        }

        #endregion

        #endregion

        #region IEquatable<SmartCityId> Members

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

            // Check if the given object is an smart city.
            var SmartCityId = Object as SmartCity_Id;
            if ((Object) SmartCityId == null)
                return false;

            return this.Equals(SmartCityId);

        }

        #endregion

        #region Equals(SmartCityId)

        /// <summary>
        /// Compares two SmartCityIds for equality.
        /// </summary>
        /// <param name="SmartCityId">A SmartCityId to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public Boolean Equals(SmartCity_Id SmartCityId)
        {

            if ((Object) SmartCityId == null)
                return false;

            if (!CountryCode.Equals(SmartCityId.CountryCode))
                return false;

            return Suffix.Equals(SmartCityId.Suffix);

        }

        #endregion

        #endregion

        #region GetHashCode()

        /// <summary>
        /// Return the HashCode of this object.
        /// </summary>
        /// <returns>The HashCode of this object.</returns>
        public override Int32 GetHashCode()
        {
            unchecked
            {
                return CountryCode.GetHashCode() * 17 ^ Suffix.GetHashCode();
            }
        }

        #endregion

        #region (override) ToString()

        /// <summary>
        /// Return a string representation of this object.
        /// </summary>
        public override String ToString()
            => String.Concat(CountryCode.ToString(), "-", Suffix);

        #endregion

    }

}
