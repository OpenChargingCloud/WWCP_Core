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

using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;

using org.GraphDefined.Vanaheimr.Illias;

#endregion

namespace cloud.charging.open.protocols.WWCP
{

    /// <summary>
    /// Extension methods for electric mobility account (eMAId) or electric vehicle contract identifications.
    /// </summary>
    public static class EMobilityAccountIdExtensions
    {

        /// <summary>
        /// Indicates whether this electric mobility account identification is null or empty.
        /// </summary>
        /// <param name="EMobilityAccountId">An electric mobility account identification.</param>
        public static Boolean IsNullOrEmpty(this EMobilityAccount_Id? EMobilityAccountId)
            => !EMobilityAccountId.HasValue || EMobilityAccountId.Value.IsNullOrEmpty;

        /// <summary>
        /// Indicates whether this electric mobility account identification is NOT null or empty.
        /// </summary>
        /// <param name="EMobilityAccountId">An electric mobility account identification.</param>
        public static Boolean IsNotNullOrEmpty(this EMobilityAccount_Id? EMobilityAccountId)
            => EMobilityAccountId.HasValue && EMobilityAccountId.Value.IsNotNullOrEmpty;

    }


    /// <summary>
    /// The unique identification of an electric mobility account (eMAId) or electric vehicle contract.
    /// </summary>
    public readonly struct EMobilityAccount_Id : IId,
                                                 IEquatable<EMobilityAccount_Id>,
                                                 IComparable<EMobilityAccount_Id>
    {

        #region Data

        private readonly String InternalId;

        /// <summary>
        /// The regular expression for parsing an electric mobility account identification.
        /// </summary>
        public static readonly Regex eMobilityAccountId_RegEx  = new (@"^([A-Za-z]{2}\-?[A-Za-z0-9]{3})\-?C([A-Za-z0-9]{8})\-?([\d|A-Za-z])$|" +         // ISO
                                                                      @"^([A-Za-z]{2}[\*|\-]?[A-Za-z0-9]{3})[\*|\-]?([A-Za-z0-9]{6})[\*|\-]?([\d|X])$",  // DIN
                                                                      RegexOptions.IgnorePatternWhitespace);
                                                                 //new Regex(@"^([A-Za-z]{2}\*[A-Za-z0-9]{3})\*([A-Za-z0-9]{6})\*([0-9|X])$ |"  +   // OICP DIN STAR:  DE*BMW*0010LY*3
                                                                 //          @"^([A-Za-z]{2}-[A-Za-z0-9]{3})-([A-Za-z0-9]{6})-([0-9|X])$ |"     +   // OICP DIN HYPEN: DE-BMW-0010LY-3
                                                                 //          @"^([A-Za-z]{2}[A-Za-z0-9]{3})([A-Za-z0-9]{6})([0-9|X])$ |"        +   // OICP DIN:       DEBMW0010LY3
                                                                 //
                                                                 //          @"^([A-Za-z]{2}\*[A-Za-z0-9]{3})\*C([A-Za-z0-9]{9})$ |"            +   //                 FR*MSP*C0001000LY
                                                                 //          @"^([A-Za-z]{2}-[A-Za-z0-9]{3})-C([A-Za-z0-9]{8})-([0-9|X])$ |"    +   // OICP ISO Hypen: DE-BMW-C001000LY-3
                                                                 //          @"^([A-Za-z]{2}[A-Za-z0-9]{3})C([A-Za-z0-9]{8})([0-9|X])$ |"       +   // OICP ISO:       DEBMWC001000LY3
                                                                 //
                                                                 //          @"^([A-Za-z]{2}-[A-Za-z0-9]{3})-([A-Za-z0-9]{9})-([A-Za-z0-9])$ |" +   // OCHP:
                                                                 //          @"^([A-Za-z]{2}[A-Za-z0-9]{3})([A-Za-z0-9]{9})([A-Za-z0-9])$ |" +
                                                                 //          @"^([A-Za-z]{2}-[A-Za-z0-9]{3})-([A-Za-z0-9]{9})$ |" +
                                                                 //          @"^([A-Za-z]{2}[A-Za-z0-9]{3})([A-Za-z0-9]{9})$",
                                                                 //
                                                                 //      //    @"^([A-Za-z]{2}[\*|-]?[A-Za-z0-9]{3})[\*|-]?([A-Za-z0-9]{6})[\*|-]?[\*|-]?[\d|X])$",   // PlugSurfing OIOI 4.x
                                                                 //          RegexOptions.IgnorePatternWhitespace);

        #endregion

        #region Properties

        /// <summary>
        /// The e-mobility provider identification.
        /// </summary>
        public EMobilityProvider_Id  ProviderId    { get; }

        /// <summary>
        /// The suffix of the identification.
        /// </summary>
        public String                Suffix        { get; }

        /// <summary>
        /// An optional check digit of the electric mobility account identification.
        /// </summary>
        public Char?                 CheckDigit    { get; }

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

            => ProviderId.Length +
               1UL +
               (UInt64) Suffix.Length +
               (CheckDigit.HasValue ? 2UL : 0UL);

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Generate a new electric mobility account identification.
        /// </summary>
        /// <param name="ProviderId">The unique identification of an e-mobility provider.</param>
        /// <param name="Suffix">The suffix of the electric mobility account identification.</param>
        /// <param name="CheckDigit">An optional check digit of the electric mobility account identification.</param>
        private EMobilityAccount_Id(String                InternalId,
                                    EMobilityProvider_Id  ProviderId,
                                    String                Suffix,
                                    Char?                 CheckDigit = null)
        {

            this.InternalId  = InternalId;
            this.ProviderId  = ProviderId;
            this.Suffix      = Suffix;
            this.CheckDigit  = CheckDigit;

        }

        #endregion


        #region (static) Parse   (Text)

        /// <summary>
        /// Parse the given text representation of an electric mobility account identification.
        /// </summary>
        /// <param name="Text">A text representation of an electric mobility account identification.</param>
        public static EMobilityAccount_Id Parse(String Text)
        {

            if (TryParse(Text, out var eMobilityAccountId))
                return eMobilityAccountId;

            throw new ArgumentException($"Invalid electric mobility account identification '{Text}'!");

        }

        #endregion

        #region (static) Parse   (ProviderId, Suffix)

        /// <summary>
        /// Parse the given electric mobility account identification.
        /// </summary>
        /// <param name="ProviderId">The unique identification of an e-mobility provider.</param>
        /// <param name="Suffix">The suffix of the electric mobility account identification.</param>
        public static EMobilityAccount_Id Parse(EMobilityProvider_Id  ProviderId,
                                                String                Suffix)
        {

            #region Initial checks

            Suffix = Suffix.Trim();

            if (Suffix.IsNullOrEmpty())
                throw new ArgumentNullException(nameof(Suffix), "The given electric vehicle contract identification suffix must not be null or empty!");

            #endregion

            switch (ProviderId.Format)
            {

                case ProviderIdFormats.DIN:
                    return Parse(ProviderId +       Suffix);

                case ProviderIdFormats.DIN_STAR:
                    return Parse(ProviderId + "*" + Suffix);

                case ProviderIdFormats.DIN_HYPHEN:
                    return Parse(ProviderId + "-" + Suffix);


                case ProviderIdFormats.ISO:
                    return Parse(ProviderId +       Suffix);

                case ProviderIdFormats.ISO_HYPHEN:
                    return Parse(ProviderId + "-" + Suffix);

                default:
                    return Parse(ProviderId + "-" + Suffix);

            }

        }

        #endregion

        #region (static) TryParse(Text)

        /// <summary>
        /// Try to parse the given string as an electric mobility account identification.
        /// </summary>
        /// <param name="Text">A text representation of an electric mobility account identification.</param>
        public static EMobilityAccount_Id? TryParse(String Text)
        {

            if (TryParse(Text, out var eMobilityAccountId))
                return eMobilityAccountId;

            return null;

        }

        #endregion

        #region (static) TryParse(Text, out eMobilityAccountId)

        /// <summary>
        /// Try to parse the given string as an electric mobility account identification.
        /// </summary>
        /// <param name="Text">A text representation of an electric mobility account identification.</param>
        /// <param name="eMobilityAccountId">The parsed electric mobility account identification.</param>
        public static Boolean TryParse(String                                       Text,
                                       [NotNullWhen(true)] out EMobilityAccount_Id  eMobilityAccountId)
        {

            #region Initial checks

            Text = Text.Trim();

            if (Text.IsNullOrEmpty())
            {
                eMobilityAccountId = default;
                return false;
            }

            #endregion

            try
            {

                var matchCollection = eMobilityAccountId_RegEx.Matches(Text);

                if (matchCollection.Count == 1)
                {

                    // ISO: DE-GDF-C12022187-X, DEGDFC12022187X
                    if (EMobilityProvider_Id.TryParse(matchCollection[0].Groups[1].Value, out EMobilityProvider_Id providerId))
                    {

                        eMobilityAccountId = new EMobilityAccount_Id(
                                                     Text,
                                                     providerId,
                                                     matchCollection[0].Groups[2].Value,
                                                     matchCollection[0].Groups[3].Value[0]
                                                 );

                        return true;

                    }


                    // DIN: DE*GDF*0010LY*3, DE-GDF-0010LY-3, DEGDF0010LY3
                    if (EMobilityProvider_Id.TryParse(matchCollection[0].Groups[4].Value,  out providerId))
                    {

                        if (providerId.Format == ProviderIdFormats.ISO_HYPHEN)
                            providerId = providerId.ChangeFormat(ProviderIdFormats.DIN_HYPHEN);

                        eMobilityAccountId = new EMobilityAccount_Id(
                                                     Text,
                                                     providerId.ChangeFormat(ProviderIdFormats.DIN_HYPHEN),
                                                     matchCollection[0].Groups[5].Value,
                                                     matchCollection[0].Groups[6].Value[0]
                                                 );

                        return true;

                    }

                }

            }
            catch
            { }

            eMobilityAccountId = default;
            return false;

        }

        #endregion

        #region Clone

        /// <summary>
        /// Clone this electric mobility account identification identification.
        /// </summary>
        public EMobilityAccount_Id Clone

            => new (
                   new String(InternalId?.ToCharArray()),
                   ProviderId.Clone,
                   new String(Suffix?.ToCharArray())
               );

        #endregion


        #region Operator overloading

        #region Operator == (eMobilityAccountId1, eMobilityAccountId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="eMobilityAccountId1">A contract identification.</param>
        /// <param name="eMobilityAccountId2">Another contract identification.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator == (EMobilityAccount_Id eMobilityAccountId1,
                                           EMobilityAccount_Id eMobilityAccountId2)

            => eMobilityAccountId1.Equals(eMobilityAccountId2);

        #endregion

        #region Operator != (eMobilityAccountId1, eMobilityAccountId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="eMobilityAccountId1">A contract identification.</param>
        /// <param name="eMobilityAccountId2">Another contract identification.</param>
        /// <returns>False if both match; True otherwise.</returns>
        public static Boolean operator != (EMobilityAccount_Id eMobilityAccountId1,
                                           EMobilityAccount_Id eMobilityAccountId2)

            => !eMobilityAccountId1.Equals(eMobilityAccountId2);

        #endregion

        #region Operator <  (eMobilityAccountId1, eMobilityAccountId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="eMobilityAccountId1">A contract identification.</param>
        /// <param name="eMobilityAccountId2">Another contract identification.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator < (EMobilityAccount_Id eMobilityAccountId1,
                                          EMobilityAccount_Id eMobilityAccountId2)

            => eMobilityAccountId1.CompareTo(eMobilityAccountId2) < 0;

        #endregion

        #region Operator <= (eMobilityAccountId1, eMobilityAccountId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="eMobilityAccountId1">A contract identification.</param>
        /// <param name="eMobilityAccountId2">Another contract identification.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator <= (EMobilityAccount_Id eMobilityAccountId1,
                                           EMobilityAccount_Id eMobilityAccountId2)

            => eMobilityAccountId1.CompareTo(eMobilityAccountId2) <= 0;

        #endregion

        #region Operator >  (eMobilityAccountId1, eMobilityAccountId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="eMobilityAccountId1">A contract identification.</param>
        /// <param name="eMobilityAccountId2">Another contract identification.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator > (EMobilityAccount_Id eMobilityAccountId1,
                                          EMobilityAccount_Id eMobilityAccountId2)

            => eMobilityAccountId1.CompareTo(eMobilityAccountId2) > 0;

        #endregion

        #region Operator >= (eMobilityAccountId1, eMobilityAccountId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="eMobilityAccountId1">A contract identification.</param>
        /// <param name="eMobilityAccountId2">Another contract identification.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator >= (EMobilityAccount_Id eMobilityAccountId1,
                                           EMobilityAccount_Id eMobilityAccountId2)

            => eMobilityAccountId1.CompareTo(eMobilityAccountId2) >= 0;

        #endregion

        #endregion

        #region IComparable<eMobilityAccountId> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two electric mobility accounts.
        /// </summary>
        /// <param name="Object">An electric mobility account to compare with.</param>
        public Int32 CompareTo(Object? Object)

            => Object is EMobilityAccount_Id eMobilityAccountId
                   ? CompareTo(eMobilityAccountId)
                   : throw new ArgumentException("The given object is not an electric mobility account identification!",
                                                 nameof(Object));

        #endregion

        #region CompareTo(eMobilityAccountId)

        /// <summary>
        /// Compares two electric mobility accounts.
        /// </summary>
        /// <param name="eMobilityAccountId">An electric mobility account to compare with.</param>
        public Int32 CompareTo(EMobilityAccount_Id eMobilityAccountId)

            => String.Compare(InternalId,
                              eMobilityAccountId.InternalId,
                              StringComparison.OrdinalIgnoreCase);

        #endregion

        #endregion

        #region IEquatable<eMobilityAccountId> Members

        #region Equals(Object)

        /// <summary>
        /// Compares two electric mobility accounts for equality.
        /// </summary>
        /// <param name="Object">An electric mobility account to compare with.</param>
        public override Boolean Equals(Object? Object)

            => Object is EMobilityAccount_Id eMobilityAccountId &&
                   Equals(eMobilityAccountId);

        #endregion

        #region Equals(eMobilityAccountId)

        /// <summary>
        /// Compares two electric mobility accounts for equality.
        /// </summary>
        /// <param name="eMobilityAccountId">An electric mobility account to compare with.</param>
        public Boolean Equals(EMobilityAccount_Id eMobilityAccountId)

            => String.Equals(InternalId,
                             eMobilityAccountId.InternalId,
                             StringComparison.OrdinalIgnoreCase);

        #endregion

        #endregion

        #region (override) GetHashCode()

        /// <summary>
        /// Return the HashCode of this object.
        /// </summary>
        /// <returns>The HashCode of this object.</returns>
        public override Int32 GetHashCode()

            => InternalId.GetHashCode();

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
