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

using System.Text.RegularExpressions;

using org.GraphDefined.Vanaheimr.Illias;

#endregion

namespace cloud.charging.open.protocols.WWCP
{

    /// <summary>
    /// The different formats of e-mobility provider identifications.
    /// </summary>
    public enum EMobilityProviderIdFormats
    {

        /// <summary>
        /// The new ISO format.
        /// </summary>
        ISO,

        /// <summary>
        /// The new ISO format with a '-' as separator.
        /// </summary>
        ISO_HYPHEN,

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
        DIN_HYPHEN

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
        public static readonly Regex eMobilityProviderId_RegEx = new Regex(@"^([A-Z]{2})([*-]?)([A-Z0-9]{3})$",
                                                                           RegexOptions.Compiled | RegexOptions.CultureInvariant);

        #endregion

        #region Properties

        /// <summary>
        /// The country code.
        /// </summary>
        public Country                     CountryCode   { get; }

        /// <summary>
        /// The identifier suffix.
        /// </summary>
        public String                      Suffix        { get; }

        /// <summary>
        /// The format of the e-mobility provider identification.
        /// </summary>
        public EMobilityProviderIdFormats  Format        { get; }

        /// <summary>
        /// Indicates whether this identification is null or empty.
        /// </summary>
        public Boolean  IsNullOrEmpty
            => Suffix.IsNullOrEmpty();

        /// <summary>
        /// Indicates whether this identification is NOT null or empty.
        /// </summary>
        public Boolean  IsNotNullOrEmpty
            => Suffix.IsNotNullOrEmpty();

        /// <summary>
        /// Returns the length of the identification.
        /// </summary>
        public UInt64   Length
            => Format switch {

                    EMobilityProviderIdFormats.DIN or
                    EMobilityProviderIdFormats.ISO
                        => (UInt64) (CountryCode.Alpha2Code.Length +     Suffix?.Length ?? 0),

                    _   => (UInt64) (CountryCode.Alpha2Code.Length + 1 + Suffix?.Length ?? 0)

               };

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Generate a new e-mobility provider identification.
        /// </summary>
        /// <param name="CountryCode">The country code.</param>
        /// <param name="Suffix">The suffix of the e-mobility provider identification.</param>
        /// <param name="Format">The format of the e-mobility provider identification.</param>
        private EMobilityProvider_Id(Country                     CountryCode,
                                     String                      Suffix,
                                     EMobilityProviderIdFormats  Format = EMobilityProviderIdFormats.ISO_HYPHEN)
        {

            this.CountryCode  = CountryCode;
            this.Suffix       = Suffix ?? String.Empty;
            this.Format       = Format;

        }

        #endregion


        #region Parse    (Text)

        /// <summary>
        /// Parse the given text representation of an e-mobility provider identification.
        /// </summary>
        /// <param name="Text">A text representation of an e-mobility provider identification.</param>
        public static EMobilityProvider_Id Parse(String Text)
        {

            if (TryParse(Text, out var eMobilityProviderId))
                return eMobilityProviderId;

            throw new ArgumentException($"Invalid text representation of an e-mobility provider identification: '{Text}'!",
                                        nameof(Text));

        }

        #endregion

        #region Parse    (CountryCode, Suffix, IdFormat = ISO_HYPHEN)

        /// <summary>
        /// Parse the given string as an e-mobility provider identification.
        /// </summary>
        /// <param name="CountryCode">A country code.</param>
        /// <param name="Suffix">The suffix of an e-mobility provider identification.</param>
        /// <param name="IdFormat">The optional format of the e-mobility provider identification.</param>
        public static EMobilityProvider_Id Parse(Country                     CountryCode,
                                                 String                      Suffix,
                                                 EMobilityProviderIdFormats  IdFormat = EMobilityProviderIdFormats.ISO_HYPHEN)
        {

            if (TryParse(CountryCode, Suffix, out var eMobilityProviderId, IdFormat))
                return eMobilityProviderId;

            throw new ArgumentException($"Invalid text representation of an e-mobility provider identification: '{CountryCode}{Suffix}'!");

        }

        #endregion

        #region TryParse (Text)

        /// <summary>
        /// Try to parse the given text representation of an e-mobility provider identification.
        /// </summary>
        /// <param name="Text">A text representation of an e-mobility provider identification.</param>
        public static EMobilityProvider_Id? TryParse(String? Text)
        {

            if (TryParse(Text, out var eMobilityProviderId))
                return eMobilityProviderId;

            return default;

        }

        #endregion

        #region TryParse (Text, out EMobilityProviderId)

        /// <summary>
        /// Try to parse the given text representation of an e-mobility provider identification.
        /// </summary>
        /// <param name="Text">A text representation of an e-mobility provider identification.</param>
        /// <param name="EMobilityProviderId">The parsed e-mobility provider identification.</param>
        public static Boolean TryParse(String?                   Text,
                                       out EMobilityProvider_Id  EMobilityProviderId)
        {

            #region Initial checks

            EMobilityProviderId = default;

            Text = Text?.Trim();

            if (Text.IsNullOrWhiteSpace())
                return false;

            #endregion

            var match = eMobilityProviderId_RegEx.Match(Text);

            if (!match.Success)
                return false;

            if (Country.TryParseAlpha2Code(match.Groups[1].Value, out var countryCode))
            {

                EMobilityProviderId = new EMobilityProvider_Id(
                                          countryCode,
                                          match.Groups[3].Value,
                                          match.Groups[2].Value switch {
                                              "-" => EMobilityProviderIdFormats.ISO_HYPHEN,
                                              "*" => EMobilityProviderIdFormats.DIN_STAR,
                                              _   => EMobilityProviderIdFormats.ISO,
                                          }
                                      );

                return true;

            }

            return false;

        }

        #endregion

        #region TryParse (CountryCode, Suffix, out eMobilityProviderId, IdFormat = ISO_HYPHEN)

        /// <summary>
        /// Try to parse the given text representation of an e-mobility provider identification.
        /// </summary>
        /// <param name="CountryCode">A country code.</param>
        /// <param name="Suffix">The suffix of an e-mobility provider identification.</param>
        /// <param name="eMobilityProviderId">The parsed e-mobility provider identification.</param>
        /// <param name="IdFormat">The optional format of the e-mobility provider identification.</param>
        public static Boolean TryParse(Country                     CountryCode,
                                       String                      Suffix,
                                       out EMobilityProvider_Id    EMobilityProviderId,
                                       EMobilityProviderIdFormats  IdFormat = EMobilityProviderIdFormats.ISO_HYPHEN)
        {

            #region Initial checks

            EMobilityProviderId = default;

            if (Suffix.IsNullOrWhiteSpace())
            {
                EMobilityProviderId = default;
                return false;
            }

            Suffix = Suffix.Trim().ToUpperInvariant();

            #endregion

            if (Suffix.Length != 3 || !Suffix.All(Char.IsAsciiLetterOrDigit))
                return false;

            EMobilityProviderId = new EMobilityProvider_Id(
                                      CountryCode,
                                      Suffix,
                                      IdFormat
                                  );

            return true;

        }

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


        #region ChangeFormat (NewFormat)

        /// <summary>
        /// Return a new e-mobility provider identification in the given format.
        /// </summary>
        /// <param name="NewFormat">The new e-mobility provider identification format.</param>
        public EMobilityProvider_Id ChangeFormat(EMobilityProviderIdFormats NewFormat)

            => new (CountryCode,
                    Suffix,
                    NewFormat);

        #endregion


        #region Operator overloading

        #region Operator == (eMobilityProviderId1, eMobilityProviderId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="eMobilityProviderId1">An e-mobility provider identification.</param>
        /// <param name="eMobilityProviderId2">Another e-mobility provider identification.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator == (EMobilityProvider_Id eMobilityProviderId1,
                                           EMobilityProvider_Id eMobilityProviderId2)

            => eMobilityProviderId1.Equals(eMobilityProviderId2);

        #endregion

        #region Operator != (eMobilityProviderId1, eMobilityProviderId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="eMobilityProviderId1">An e-mobility provider identification.</param>
        /// <param name="eMobilityProviderId2">Another e-mobility provider identification.</param>
        /// <returns>False if both match; True otherwise.</returns>
        public static Boolean operator != (EMobilityProvider_Id eMobilityProviderId1,
                                           EMobilityProvider_Id eMobilityProviderId2)

            => !eMobilityProviderId1.Equals(eMobilityProviderId2);

        #endregion

        #region Operator <  (eMobilityProviderId1, eMobilityProviderId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="eMobilityProviderId1">An e-mobility provider identification.</param>
        /// <param name="eMobilityProviderId2">Another e-mobility provider identification.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator < (EMobilityProvider_Id eMobilityProviderId1,
                                          EMobilityProvider_Id eMobilityProviderId2)

            => eMobilityProviderId1.CompareTo(eMobilityProviderId2) < 0;

        #endregion

        #region Operator <= (eMobilityProviderId1, eMobilityProviderId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="eMobilityProviderId1">An e-mobility provider identification.</param>
        /// <param name="eMobilityProviderId2">Another e-mobility provider identification.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator <= (EMobilityProvider_Id eMobilityProviderId1,
                                           EMobilityProvider_Id eMobilityProviderId2)

            => eMobilityProviderId1.CompareTo(eMobilityProviderId2) <= 0;

        #endregion

        #region Operator >  (eMobilityProviderId1, eMobilityProviderId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="eMobilityProviderId1">An e-mobility provider identification.</param>
        /// <param name="eMobilityProviderId2">Another e-mobility provider identification.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator > (EMobilityProvider_Id eMobilityProviderId1,
                                          EMobilityProvider_Id eMobilityProviderId2)

            => eMobilityProviderId1.CompareTo(eMobilityProviderId2) > 0;

        #endregion

        #region Operator >= (eMobilityProviderId1, eMobilityProviderId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="eMobilityProviderId1">An e-mobility provider identification.</param>
        /// <param name="eMobilityProviderId2">Another e-mobility provider identification.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator >= (EMobilityProvider_Id eMobilityProviderId1,
                                           EMobilityProvider_Id eMobilityProviderId2)

            => eMobilityProviderId1.CompareTo(eMobilityProviderId2) >= 0;

        #endregion

        #endregion

        #region IComparable<EMobilityProvider_Id> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two e-mobility provider identifications.
        /// </summary>
        /// <param name="Object">An e-mobility provider identification to compare with.</param>
        public Int32 CompareTo(Object? Object)

            => Object is EMobilityProvider_Id eMobilityeMobilityProviderId
                   ? CompareTo(eMobilityeMobilityProviderId)
                   : throw new ArgumentException("The given object is not an e-mobility provider identification!",
                                                 nameof(Object));

        #endregion

        #region CompareTo(EMobilityProvider_Id)

        /// <summary>
        /// Compares two e-mobility provider identifications.
        /// </summary>
        /// <param name="eMobilityProviderId">An e-mobility provider identification to compare with.</param>
        public Int32 CompareTo(EMobilityProvider_Id eMobilityProviderId)
        {

            var c = CountryCode.CompareTo(eMobilityProviderId.CountryCode);

            if (c == 0)
                c = String.Compare(
                        Suffix,
                        eMobilityProviderId.Suffix,
                        StringComparison.Ordinal
                    );

            return c;

        }

        #endregion

        #endregion

        #region IEquatable<eMobilityProviderId> Members

        #region Equals(Object)

        /// <summary>
        /// Compares two e-mobility provider identifications.
        /// </summary>
        /// <param name="Object">An e-mobility provider identification to compare with.</param>
        public override Boolean Equals(Object? Object)

            => Object is EMobilityProvider_Id eMobilityProviderId &&
                   Equals(eMobilityProviderId);

        #endregion

        #region Equals(eMobilityProviderId)

        /// <summary>
        /// Compares two e-mobility provider identifications for equality.
        /// </summary>
        /// <param name="eMobilityProviderId">An e-mobility provider to compare with.</param>
        public Boolean Equals(EMobilityProvider_Id eMobilityProviderId)

            => CountryCode.Equals(eMobilityProviderId.CountryCode) &&
               StringComparer.Ordinal.Equals(Suffix, eMobilityProviderId.Suffix);

        #endregion

        #endregion

        #region (override) GetHashCode()

        public override Int32 GetHashCode()

            => HashCode.Combine(
                   CountryCode,
                   StringComparer.Ordinal.GetHashCode(Suffix ?? String.Empty)
               );

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
        public String ToString(EMobilityProviderIdFormats Format)

            => Format switch {
                   EMobilityProviderIdFormats.DIN         => $"{CountryCode.Alpha2Code}{Suffix}",
                   EMobilityProviderIdFormats.DIN_STAR    => $"{CountryCode.Alpha2Code}*{Suffix}",
                   EMobilityProviderIdFormats.DIN_HYPHEN  => $"{CountryCode.Alpha2Code}-{Suffix}",
                   EMobilityProviderIdFormats.ISO         => $"{CountryCode.Alpha2Code}{Suffix}",
                   // ISO_HYPHEN
                   _                                      => $"{CountryCode.Alpha2Code}-{Suffix}"
               };

        #endregion

    }

}
