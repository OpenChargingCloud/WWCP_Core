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

    public enum ProviderIdFormats
    {
        DIN,
        DIN_STAR,
        DIN_HYPHEN,
        ISO,
        ISO_HYPHEN
    }


    /// <summary>
    /// The unique identification of an e-mobility service provider.
    /// </summary>
    public struct eMobilityProvider_Id : IId,
                                         IEquatable <eMobilityProvider_Id>,
                                         IComparable<eMobilityProvider_Id>

    {

        #region Data

        /// <summary>
        /// The regular expression for parsing an e-mobility service provider identification.
        /// </summary>
        public const String  ProviderId_RegEx            = @"^[A-Z0-9]{3}$";

        /// <summary>
        /// The regular expression for parsing an Alpha-2-CountryCode and an e-mobility service provider identification.
        /// The ISO format onyl allows '-' as a separator!
        /// </summary>
        public const String  CountryAndProviderId_RegEx  = @"^([A-Z]{2})([\*|\-]?)([A-Z0-9]{3})$";

        #endregion

        #region Properties

        /// <summary>
        /// The country code.
        /// </summary>
        public Country            CountryCode   { get; }

        /// <summary>
        /// The internal e-mobility service provider identification.
        /// </summary>
        public String             ProviderId    { get; }

        /// <summary>
        /// The format of the e-mobility service provider identification.
        /// </summary>
        public ProviderIdFormats  Format        { get; }

        /// <summary>
        /// Returns the original representation of the e-mobility service provider identification.
        /// </summary>
        public String             OriginId
            => ToFormat(Format);

        /// <summary>
        /// Returns the length of the identificator.
        /// </summary>
        public UInt64 Length
        {
            get
            {

                switch (Format)
                {

                    case ProviderIdFormats.DIN:
                        return (UInt64) (1 + CountryCode.TelefonCode.ToString().Length     + ProviderId.Length);

                    case ProviderIdFormats.DIN_STAR:
                    case ProviderIdFormats.DIN_HYPHEN:
                        return (UInt64) (1 + CountryCode.TelefonCode.ToString().Length + 1 + ProviderId.Length);


                    case ProviderIdFormats.ISO:
                        return (UInt64)     (CountryCode.Alpha2Code.Length                 + ProviderId.Length);

                    default:
                        return (UInt64)     (CountryCode.Alpha2Code.Length             + 1 + ProviderId.Length);

                }

            }
        }

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Generate a new e-mobility service provider identification.
        /// </summary>
        /// <param name="CountryCode">The country code.</param>
        /// <param name="ProviderId">The e-mobility service provider identification.</param>
        /// <param name="Format">The id format '-' (ISO) or '*|-' DIN to use.</param>
        private eMobilityProvider_Id(Country            CountryCode,
                                     String             ProviderId,
                                     ProviderIdFormats  Format = ProviderIdFormats.ISO_HYPHEN)
        {

            this.CountryCode  = CountryCode;
            this.Format       = Format;
            this.ProviderId   = ProviderId;

        }

        #endregion


        #region Parse(Text)

        /// <summary>
        /// Parse the given text representation of an e-mobility service provider identification.
        /// </summary>
        /// <param name="Text">A text representation of an e-mobility service provider identification.</param>
        public static eMobilityProvider_Id Parse(String Text)
        {

            #region Initial checks

            if (Text.Trim().IsNullOrEmpty())
                throw new ArgumentException("The given text representation of an e-mobility service provider identification must not be null or empty!", nameof(Text));

            #endregion

            var MatchCollection = Regex.Matches(Text.Trim().ToUpper(),
                                                CountryAndProviderId_RegEx,
                                                RegexOptions.IgnorePatternWhitespace);

            if (MatchCollection.Count != 1)
                throw new ArgumentException("Illegal text representation of an e-mobility service provider identification: '" + Text + "'!", nameof(Text));

            Country _CountryCode;

            if (Country.TryParseAlpha2Code(MatchCollection[0].Groups[1].Value, out _CountryCode))
            {

                ProviderIdFormats Separator = ProviderIdFormats.ISO_HYPHEN;

                switch (MatchCollection[0].Groups[2].Value)
                {

                    case ""  : Separator = ProviderIdFormats.DIN|ProviderIdFormats.ISO; break;
                    case "-" : Separator = ProviderIdFormats.DIN_HYPHEN|ProviderIdFormats.ISO_HYPHEN; break;
                    case "*" : Separator = ProviderIdFormats.DIN_STAR; break;

                    default: throw new ArgumentException("Illegal e-mobility service provider identification!", "CountryAndProviderId");

                }

                return new eMobilityProvider_Id(_CountryCode,
                                                MatchCollection[0].Groups[3].Value,
                                                Separator);
            }

            throw new ArgumentException("Unknown country code in the given text representation of an e-mobility service provider identification: '" + Text + "'!", nameof(Text));

        }

        #endregion

        #region Parse(CountryCode, ProviderId, IdFormat = ProviderIdFormats.ISO_HYPHEN)

        /// <summary>
        /// Parse the given string as an e-mobility service provider identification.
        /// </summary>
        /// <param name="CountryCode">A country code.</param>
        /// <param name="ProviderId">An e-mobility service provider identification as a string.</param>
        /// <param name="IdFormat">The optional format of the provider identification.</param>
        public static eMobilityProvider_Id Parse(Country            CountryCode,
                                                 String             ProviderId,
                                                 ProviderIdFormats  IdFormat = ProviderIdFormats.ISO_HYPHEN)
        {

            #region Initial checks

            if (CountryCode == null)
                throw new ArgumentNullException(nameof(CountryCode), "The given country must not be null!");

            if (ProviderId.IsNullOrEmpty())
                throw new ArgumentNullException(nameof(ProviderId),  "The given e-mobility service provider identification suffix must not be null or empty!");

            #endregion

            if (!Regex.IsMatch(ProviderId.Trim().ToUpper(),
                               ProviderId_RegEx,
                               RegexOptions.IgnorePatternWhitespace))

                throw new ArgumentException("Illegal e-mobility service provider identification '" + CountryCode + "' / '" + ProviderId + "'!",
                                            nameof(ProviderId));

            return new eMobilityProvider_Id(CountryCode,
                                            ProviderId,
                                            IdFormat);

        }

        #endregion

        #region TryParse(Text, out EVSEProviderId)

        /// <summary>
        /// Try to parse the given text representation of an e-mobility service provider identification.
        /// </summary>
        /// <param name="Text">A text representation of an e-mobility service provider identification.</param>
        /// <param name="EVSEProviderId">The parsed e-mobility service provider identification.</param>
        public static Boolean TryParse(String                    Text,
                                       out eMobilityProvider_Id  EVSEProviderId)
        {

            #region Initial checks

            if (Text.IsNullOrEmpty())
            {
                EVSEProviderId = default(eMobilityProvider_Id);
                return false;
            }

            #endregion

            try
            {

                var MatchCollection = Regex.Matches(Text.Trim().ToUpper(),
                                                    CountryAndProviderId_RegEx,
                                                    RegexOptions.IgnorePatternWhitespace);

                if (MatchCollection.Count != 1)
                {
                    EVSEProviderId = default(eMobilityProvider_Id);
                    return false;
                }

                Country _CountryCode;

                if (Country.TryParseAlpha2Code(MatchCollection[0].Groups[1].Value, out _CountryCode))
                {

                    var Separator = ProviderIdFormats.ISO_HYPHEN;

                    switch (MatchCollection[0].Groups[2].Value)
                    {

                        case ""  : Separator = ProviderIdFormats.DIN|ProviderIdFormats.ISO; break;
                        case "-" : Separator = ProviderIdFormats.ISO_HYPHEN;                break;
                        case "*" : Separator = ProviderIdFormats.DIN_STAR;                  break;

                        default: throw new ArgumentException("Illegal e-mobility service provider identification!", nameof(Text));

                    }

                    EVSEProviderId = new eMobilityProvider_Id(_CountryCode,
                                                 MatchCollection[0].Groups[3].Value,
                                                 Separator);

                    return true;

                }

            }

#pragma warning disable RCS1075  // Avoid empty catch clause that catches System.Exception.
#pragma warning disable RECS0022 // A catch clause that catches System.Exception and has an empty body
            catch (Exception)
#pragma warning restore RECS0022 // A catch clause that catches System.Exception and has an empty body
#pragma warning restore RCS1075  // Avoid empty catch clause that catches System.Exception.
            { }

            EVSEProviderId = default(eMobilityProvider_Id);
            return false;

        }

        #endregion

        #region TryParse(CountryCode, ProviderId, out EVSEProviderId, IdFormat = IdFormatType.NEW)

        /// <summary>
        /// Parse the given string as an EVSE Operator identification.
        /// </summary>
        /// <param name="CountryCode">A country code.</param>
        /// <param name="ProviderId">An Charging Station Operator identification as a string.</param>
        /// <param name="EVSEProviderId">The parsed EVSE Operator identification.</param>
        public static Boolean TryParse(Country      CountryCode,
                                       String       ProviderId,
                                       out eMobilityProvider_Id  EVSEProviderId)
        {

            #region Initial checks

            if (CountryCode == null || ProviderId.IsNullOrEmpty())
            {
                EVSEProviderId = default(eMobilityProvider_Id);
                return false;
            }

            #endregion

            try
            {

                var _MatchCollection = Regex.Matches(ProviderId.Trim().ToUpper(),
                                                     ProviderId_RegEx,
                                                     RegexOptions.IgnorePatternWhitespace);

                if (_MatchCollection.Count != 1)
                {
                    EVSEProviderId = default(eMobilityProvider_Id);
                    return false;
                }

                EVSEProviderId = new eMobilityProvider_Id(CountryCode,
                                             _MatchCollection[0].Value,
                                             ProviderIdFormats.DIN | ProviderIdFormats.ISO);

                return true;

            }

            catch (Exception)
            {
                EVSEProviderId = default(eMobilityProvider_Id);
                return false;
            }

        }

        #endregion


        #region ChangeFormat(NewFormat)

        /// <summary>
        /// Return a new e-mobility service provider identification in the given format.
        /// </summary>
        /// <param name="NewFormat">The new e-mobility service provider identification format.</param>
        public eMobilityProvider_Id ChangeFormat(ProviderIdFormats NewFormat)

            => new eMobilityProvider_Id(CountryCode,
                                        ProviderId,
                                        NewFormat);

        #endregion


        #region ToFormat(Format)

        /// <summary>
        /// Return the identification in the given format.
        /// </summary>
        /// <param name="Format">The format of the identification.</param>
        public String ToFormat(ProviderIdFormats Format)
        {

            switch (Format)
            {

                case ProviderIdFormats.DIN:
                    return String.Concat("+",
                                         CountryCode.TelefonCode,
                                         ProviderId);

                case ProviderIdFormats.DIN_STAR:
                    return String.Concat("+",
                                         CountryCode.TelefonCode,
                                         "*",
                                         ProviderId);

                case ProviderIdFormats.DIN_HYPHEN:
                    return String.Concat("+",
                                         CountryCode.TelefonCode,
                                         "-",
                                         ProviderId);


                case ProviderIdFormats.ISO:
                    return String.Concat(CountryCode.Alpha2Code,
                                         ProviderId);

                default:
                    return String.Concat(CountryCode.Alpha2Code,
                                         "-",
                                         ProviderId);

            }

        }

        #endregion


        #region Operator overloading

        #region Operator == (eMobilityProviderId1, eMobilityProviderId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="eMobilityProviderId1">An e-mobility service provider identification.</param>
        /// <param name="eMobilityProviderId2">Another e-mobility service provider identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator == (eMobilityProvider_Id eMobilityProviderId1, eMobilityProvider_Id eMobilityProviderId2)
        {

            // If both are null, or both are same instance, return true.
            if (Object.ReferenceEquals(eMobilityProviderId1, eMobilityProviderId2))
                return true;

            // If one is null, but not both, return false.
            if (((Object) eMobilityProviderId1 == null) || ((Object) eMobilityProviderId2 == null))
                return false;

            return eMobilityProviderId1.Equals(eMobilityProviderId2);

        }

        #endregion

        #region Operator != (eMobilityProviderId1, eMobilityProviderId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="eMobilityProviderId1">An e-mobility service provider identification.</param>
        /// <param name="eMobilityProviderId2">Another e-mobility service provider identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator != (eMobilityProvider_Id eMobilityProviderId1, eMobilityProvider_Id eMobilityProviderId2)
        {
            return !(eMobilityProviderId1 == eMobilityProviderId2);
        }

        #endregion

        #region Operator <  (eMobilityProviderId1, eMobilityProviderId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="eMobilityProviderId1">An e-mobility service provider identification.</param>
        /// <param name="eMobilityProviderId2">Another e-mobility service provider identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator < (eMobilityProvider_Id eMobilityProviderId1, eMobilityProvider_Id eMobilityProviderId2)
        {

            if ((Object) eMobilityProviderId1 == null)
                throw new ArgumentNullException("The given eMobilityProviderId1 must not be null!");

            return eMobilityProviderId1.CompareTo(eMobilityProviderId2) < 0;

        }

        #endregion

        #region Operator <= (eMobilityProviderId1, eMobilityProviderId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="eMobilityProviderId1">An e-mobility service provider identification.</param>
        /// <param name="eMobilityProviderId2">Another e-mobility service provider identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator <= (eMobilityProvider_Id eMobilityProviderId1, eMobilityProvider_Id eMobilityProviderId2)
        {
            return !(eMobilityProviderId1 > eMobilityProviderId2);
        }

        #endregion

        #region Operator >  (eMobilityProviderId1, eMobilityProviderId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="eMobilityProviderId1">An e-mobility service provider identification.</param>
        /// <param name="eMobilityProviderId2">Another e-mobility service provider identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator > (eMobilityProvider_Id eMobilityProviderId1, eMobilityProvider_Id eMobilityProviderId2)
        {

            if ((Object) eMobilityProviderId1 == null)
                throw new ArgumentNullException("The given eMobilityProviderId1 must not be null!");

            return eMobilityProviderId1.CompareTo(eMobilityProviderId2) > 0;

        }

        #endregion

        #region Operator >= (eMobilityProviderId1, eMobilityProviderId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="eMobilityProviderId1">An e-mobility service provider identification.</param>
        /// <param name="eMobilityProviderId2">Another e-mobility service provider identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator >= (eMobilityProvider_Id eMobilityProviderId1, eMobilityProvider_Id eMobilityProviderId2)
        {
            return !(eMobilityProviderId1 < eMobilityProviderId2);
        }

        #endregion

        #endregion

        #region IComparable<eMobilityProviderId> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        public Int32 CompareTo(Object Object)
        {

            if (Object == null)
                throw new ArgumentNullException(nameof(Object), "The given object must not be null!");

            if (!(Object is eMobilityProvider_Id))
                throw new ArgumentException("The given object is not an e-mobility provider identification!", nameof(Object));

            return CompareTo((eMobilityProvider_Id) Object);

        }

        #endregion

        #region CompareTo(eMobilityProviderId)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="eMobilityProviderId">An object to compare with.</param>
        public Int32 CompareTo(eMobilityProvider_Id eMobilityProviderId)
        {

            if ((Object) eMobilityProviderId == null)
                throw new ArgumentNullException(nameof(eMobilityProviderId), "The given e-mobility provider identification must not be null!");

            // Compare the length of the eMobilityProviderIds
            var _Result = this.Length.CompareTo(eMobilityProviderId.Length);

            // If equal: Compare country codes
            if (_Result == 0)
                _Result = CountryCode.CompareTo(eMobilityProviderId.CountryCode);

            // If equal: Compare provider ids
            if (_Result == 0)
                _Result = String.Compare(ProviderId, eMobilityProviderId.ProviderId, StringComparison.Ordinal);

            return _Result;

        }

        #endregion

        #endregion

        #region IEquatable<eMobilityProviderId> Members

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

            if (!(Object is eMobilityProvider_Id))
                return false;

            return this.Equals((eMobilityProvider_Id) Object);

        }

        #endregion

        #region Equals(eMobilityProviderId)

        /// <summary>
        /// Compares two eMobilityProviderIds for equality.
        /// </summary>
        /// <param name="eMobilityProviderId">A eMobilityProviderId to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public Boolean Equals(eMobilityProvider_Id eMobilityProviderId)
        {

            if ((Object) eMobilityProviderId == null)
                return false;

            return CountryCode.Equals(eMobilityProviderId.CountryCode) &&
                   ProviderId. Equals(eMobilityProviderId.ProviderId);

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
               ProviderId.            GetHashCode();


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
