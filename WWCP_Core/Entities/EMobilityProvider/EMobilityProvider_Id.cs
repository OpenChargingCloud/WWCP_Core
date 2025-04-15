﻿/*
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

using System.Text.RegularExpressions;

using org.GraphDefined.Vanaheimr.Illias;

#endregion

namespace cloud.charging.open.protocols.WWCP
{

    /// <summary>
    /// The different formats of e-mobility provider identifications.
    /// </summary>
    public enum ProviderIdFormats
    {

        /// <summary>
        /// The old DIN format.
        /// (Only used in combination with eMAIds!)
        /// </summary>
        DIN,

        /// <summary>
        /// The old DIN format with a '*' as separator.
        /// </summary>
        DIN_STAR,

        /// <summary>
        /// The old DIN format with a '-' as separator.
        /// (Only used in combination with eMAIds!)
        /// </summary>
        DIN_HYPHEN,


        /// <summary>
        /// The new ISO format.
        /// </summary>
        ISO,

        /// <summary>
        /// The new ISO format with a '-' as separator.
        /// </summary>
        ISO_HYPHEN

    }


    /// <summary>
    /// The unique identification of an e-mobility provider.
    /// </summary>
    public readonly struct EMobilityProvider_Id : IId,
                                                  IEquatable<EMobilityProvider_Id>,
                                                  IComparable<EMobilityProvider_Id>

    {

        #region Data

        /// <summary>
        /// The regular expression for parsing an Alpha-2-CountryCode and an e-mobility service provider identification.
        /// </summary>
        public static readonly Regex ProviderId_RegEx = new Regex(@"^([A-Z]{2})([\*|\-]?)([A-Z0-9]{3})$",
                                                                  RegexOptions.IgnorePatternWhitespace);

        #endregion

        #region Properties

        /// <summary>
        /// The country code.
        /// </summary>
        public Country            CountryCode   { get; }

        /// <summary>
        /// The identifier suffix.
        /// </summary>
        public String             Suffix        { get; }

        /// <summary>
        /// The format of the e-mobility provider identification.
        /// </summary>
        public ProviderIdFormats  Format        { get; }

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

                    case ProviderIdFormats.DIN_STAR:
                        return (UInt64) (CountryCode.Alpha2Code.Length + 1 + Suffix.Length);

                    case ProviderIdFormats.ISO:
                        return (UInt64) (CountryCode.Alpha2Code.Length     + Suffix.Length);

                    default: // ISO_HYPHEN
                        return (UInt64) (CountryCode.Alpha2Code.Length + 1 + Suffix.Length);

                }

            }
        }

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Generate a new e-mobility provider identification.
        /// </summary>
        /// <param name="CountryCode">The country code.</param>
        /// <param name="Suffix">The suffix of the e-mobility provider identification.</param>
        /// <param name="Format">The format of the e-mobility provider identification.</param>
        private EMobilityProvider_Id(Country            CountryCode,
                                     String             Suffix,
                                     ProviderIdFormats  Format = ProviderIdFormats.ISO_HYPHEN)
        {

            #region Initial checks

            if (Suffix.IsNullOrEmpty())
                throw new ArgumentNullException(nameof(Suffix), "The e-mobility provider identification suffix must not be null or empty!");

            #endregion

            this.CountryCode  = CountryCode;
            this.Suffix       = Suffix;
            this.Format       = Format;

        }

        #endregion


        #region Parse(Text)

        /// <summary>
        /// Parse the given text representation of an e-mobility provider identification.
        /// </summary>
        /// <param name="Text">A text representation of an e-mobility provider identification.</param>
        public static EMobilityProvider_Id Parse(String Text)
        {

            if (TryParse(Text, out var providerId))
                return providerId;

            throw new ArgumentException("Unknown country code in the given text representation of an e-mobility provider identification: '{Text}'!",
                                        nameof(Text));

        }

        #endregion

        #region Parse(CountryCode, Suffix, IdFormat = ProviderIdFormats.ISO_HYPHEN)

        /// <summary>
        /// Parse the given string as an e-mobility provider identification.
        /// </summary>
        /// <param name="CountryCode">A country code.</param>
        /// <param name="Suffix">The suffix of an e-mobility provider identification.</param>
        /// <param name="IdFormat">The optional format of the e-mobility provider identification.</param>
        public static EMobilityProvider_Id Parse(Country            CountryCode,
                                                 String             Suffix,
                                                 ProviderIdFormats  IdFormat = ProviderIdFormats.ISO_HYPHEN)
        {

            #region Initial checks

            if (Suffix.IsNullOrEmpty())
                throw new ArgumentNullException(nameof(Suffix), "The given e-mobility provider identification suffix must not be null or empty!");

            #endregion

            switch (IdFormat)
            {

                case ProviderIdFormats.DIN:
                    return Parse(CountryCode.Alpha2Code +       Suffix);

                case ProviderIdFormats.DIN_STAR:
                    return Parse(CountryCode.Alpha2Code + "*" + Suffix);

                case ProviderIdFormats.DIN_HYPHEN:
                    return Parse(CountryCode.Alpha2Code + "-" + Suffix);


                case ProviderIdFormats.ISO:
                    return Parse(CountryCode.Alpha2Code +       Suffix);

                default: // ISO_HYPHEN:
                    return Parse(CountryCode.Alpha2Code + "-" + Suffix);

            }

        }

        #endregion

        #region TryParse(Text)

        /// <summary>
        /// Try to parse the given text representation of an e-mobility provider identification.
        /// </summary>
        /// <param name="Text">A text representation of an e-mobility provider identification.</param>
        public static EMobilityProvider_Id? TryParse(String Text)
        {

            if (TryParse(Text, out var providerId))
                return providerId;

            return default;

        }

        #endregion

        #region TryParse(Text, out ProviderId)

        /// <summary>
        /// Try to parse the given text representation of an e-mobility provider identification.
        /// </summary>
        /// <param name="Text">A text representation of an e-mobility provider identification.</param>
        /// <param name="ProviderId">The parsed e-mobility provider identification.</param>
        public static Boolean TryParse(String                    Text,
                                       out EMobilityProvider_Id  ProviderId)
        {

            #region Initial checks

            ProviderId  = default;
            Text        = Text.Trim();

            if (Text.IsNullOrEmpty())
                return false;

            #endregion

            try
            {

                var MatchCollection = ProviderId_RegEx.Matches(Text);

                if (MatchCollection.Count != 1)
                    return false;


                if (Country.TryParseAlpha2Code(MatchCollection[0].Groups[1].Value, out var countryCode))
                {

                    ProviderId = new EMobilityProvider_Id(countryCode,
                                                          MatchCollection[0].Groups[3].Value,
                                                          MatchCollection[0].Groups[2].Value switch {
                                                              "-" => ProviderIdFormats.ISO_HYPHEN,
                                                              "*" => ProviderIdFormats.DIN_STAR,
                                                              _   => ProviderIdFormats.ISO,
                                                          });

                    return true;

                }

            }

            catch
            { }

            return false;

        }

        #endregion

        #region TryParse(CountryCode, Suffix, out ProviderId, IdFormat = ProviderIdFormats.ISO_HYPHEN)

        /// <summary>
        /// Try to parse the given text representation of an e-mobility provider identification.
        /// </summary>
        /// <param name="CountryCode">A country code.</param>
        /// <param name="Suffix">The suffix of an e-mobility provider identification.</param>
        /// <param name="ProviderId">The parsed e-mobility provider identification.</param>
        /// <param name="IdFormat">The optional format of the e-mobility provider identification.</param>
        public static Boolean TryParse(Country                   CountryCode,
                                       String                    Suffix,
                                       out EMobilityProvider_Id  ProviderId,
                                       ProviderIdFormats         IdFormat = ProviderIdFormats.ISO_HYPHEN)
        {

            #region Initial checks

            if (Suffix.IsNullOrEmpty())
            {
                ProviderId = default;
                return false;
            }

            #endregion

            switch (IdFormat)
            {

                case ProviderIdFormats.DIN_STAR:
                    return TryParse(CountryCode.Alpha2Code + "*" + Suffix,
                                    out ProviderId);

                case ProviderIdFormats.ISO:
                    return TryParse(CountryCode.Alpha2Code +       Suffix,
                                    out ProviderId);

                default: // ISO_HYPHEN:
                    return TryParse(CountryCode.Alpha2Code + "-" + Suffix,
                                    out ProviderId);

            }

        }

        #endregion

        #region ChangeFormat(NewFormat)

        /// <summary>
        /// Return a new e-mobility provider identification in the given format.
        /// </summary>
        /// <param name="NewFormat">The new e-mobility provider identification format.</param>
        public EMobilityProvider_Id ChangeFormat(ProviderIdFormats NewFormat)

            => new (CountryCode,
                    Suffix,
                    NewFormat);

        #endregion

        #region Clone()

        /// <summary>
        /// Clone this e-mobility provider identification.
        /// </summary>
        public EMobilityProvider_Id Clone()

            => new (
                   CountryCode.Clone(),
                   Suffix.     CloneString(),
                   Format
               );

        #endregion


        #region Operator overloading

        #region Operator == (ProviderId1, ProviderId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ProviderId1">An e-mobility provider identification.</param>
        /// <param name="ProviderId2">Another e-mobility provider identification.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator == (EMobilityProvider_Id ProviderId1, EMobilityProvider_Id ProviderId2)
        {

            // If both are null, or both are same instance, return true.
            if (ReferenceEquals(ProviderId1, ProviderId2))
                return true;

            // If one is null, but not both, return false.
            if (((Object) ProviderId1 == null) || ((Object) ProviderId2 == null))
                return false;

            return ProviderId1.Equals(ProviderId2);

        }

        #endregion

        #region Operator != (ProviderId1, ProviderId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ProviderId1">An e-mobility provider identification.</param>
        /// <param name="ProviderId2">Another e-mobility provider identification.</param>
        /// <returns>False if both match; True otherwise.</returns>
        public static Boolean operator != (EMobilityProvider_Id ProviderId1, EMobilityProvider_Id ProviderId2)
            => !(ProviderId1 == ProviderId2);

        #endregion

        #region Operator <  (ProviderId1, ProviderId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ProviderId1">An e-mobility provider identification.</param>
        /// <param name="ProviderId2">Another e-mobility provider identification.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator < (EMobilityProvider_Id ProviderId1, EMobilityProvider_Id ProviderId2)
        {

            if ((Object) ProviderId1 == null)
                throw new ArgumentNullException(nameof(ProviderId1), "The given ProviderId1 must not be null!");

            return ProviderId1.CompareTo(ProviderId2) < 0;

        }

        #endregion

        #region Operator <= (ProviderId1, ProviderId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ProviderId1">An e-mobility provider identification.</param>
        /// <param name="ProviderId2">Another e-mobility provider identification.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator <= (EMobilityProvider_Id ProviderId1, EMobilityProvider_Id ProviderId2)
            => !(ProviderId1 > ProviderId2);

        #endregion

        #region Operator >  (ProviderId1, ProviderId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ProviderId1">An e-mobility provider identification.</param>
        /// <param name="ProviderId2">Another e-mobility provider identification.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator > (EMobilityProvider_Id ProviderId1, EMobilityProvider_Id ProviderId2)
        {

            if ((Object) ProviderId1 == null)
                throw new ArgumentNullException(nameof(ProviderId1), "The given ProviderId1 must not be null!");

            return ProviderId1.CompareTo(ProviderId2) > 0;

        }

        #endregion

        #region Operator >= (ProviderId1, ProviderId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ProviderId1">An e-mobility provider identification.</param>
        /// <param name="ProviderId2">Another e-mobility provider identification.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator >= (EMobilityProvider_Id ProviderId1, EMobilityProvider_Id ProviderId2)
            => !(ProviderId1 < ProviderId2);

        #endregion

        #endregion

        #region IComparable<ProviderId> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        public Int32 CompareTo(Object Object)
        {

            if (Object == null)
                throw new ArgumentNullException(nameof(Object), "The given object must not be null!");

            if (!(Object is EMobilityProvider_Id))
                throw new ArgumentException("The given object is not an e-mobility provider identification!", nameof(Object));

            return CompareTo((EMobilityProvider_Id) Object);

        }

        #endregion

        #region CompareTo(ProviderId)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ProviderId">An object to compare with.</param>
        public Int32 CompareTo(EMobilityProvider_Id ProviderId)
        {

            if ((Object) ProviderId == null)
                throw new ArgumentNullException(nameof(ProviderId), "The given e-mobility provider identification must not be null!");

            // Compare the length of the ProviderIds
            var _Result = Length.CompareTo(ProviderId.Length);

            // If equal: Compare country codes
            if (_Result == 0)
                _Result = CountryCode.CompareTo(ProviderId.CountryCode);

            // If equal: Compare provider ids
            if (_Result == 0)
                _Result = String.Compare(Suffix, ProviderId.Suffix, StringComparison.Ordinal);

            return _Result;

        }

        #endregion

        #endregion

        #region IEquatable<ProviderId> Members

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

            if (!(Object is EMobilityProvider_Id))
                return false;

            return Equals((EMobilityProvider_Id) Object);

        }

        #endregion

        #region Equals(ProviderId)

        /// <summary>
        /// Compares two e-mobility provider identifications for equality.
        /// </summary>
        /// <param name="ProviderId">An e-mobility provider to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public Boolean Equals(EMobilityProvider_Id ProviderId)
        {

            if ((Object) ProviderId == null)
                return false;

            return CountryCode.Equals(ProviderId.CountryCode) &&
                   Suffix.     Equals(ProviderId.Suffix);

        }

        #endregion

        #endregion

        #region (override) GetHashCode()

        /// <summary>
        /// Return the HashCode of this object.
        /// </summary>
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

                case ProviderIdFormats.DIN:
                    return CountryCode.Alpha2Code + Suffix;

                case ProviderIdFormats.DIN_STAR:
                    return CountryCode.Alpha2Code + "*" + Suffix;

                case ProviderIdFormats.DIN_HYPHEN:
                    return CountryCode.Alpha2Code + "-" + Suffix;

                case ProviderIdFormats.ISO:
                    return CountryCode.Alpha2Code + Suffix;

                default: // ISO_HYPHEN
                    return CountryCode.Alpha2Code + "-" + Suffix;

            }

        }

        #endregion

        #region ToString(Format)

        /// <summary>
        /// Return the identification in the given format.
        /// </summary>
        /// <param name="Format">The format of the identification.</param>
        public String ToString(ProviderIdFormats Format)
        {

            switch (Format)
            {

                case ProviderIdFormats.DIN:
                    return String.Concat(CountryCode.Alpha2Code,
                                         Suffix);

                case ProviderIdFormats.DIN_STAR:
                    return String.Concat(CountryCode.Alpha2Code,
                                         "*",
                                         Suffix);

                case ProviderIdFormats.DIN_HYPHEN:
                    return String.Concat(CountryCode.Alpha2Code,
                                         "-",
                                         Suffix);

                case ProviderIdFormats.ISO:
                    return String.Concat(CountryCode.Alpha2Code,
                                         Suffix);

                default: // ISO_HYPHEN
                    return String.Concat(CountryCode.Alpha2Code,
                                         "-",
                                         Suffix);

            }

        }

        #endregion

    }

}
