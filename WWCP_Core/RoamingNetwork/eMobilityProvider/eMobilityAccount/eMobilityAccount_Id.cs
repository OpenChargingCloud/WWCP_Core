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

using System;
using System.Text.RegularExpressions;

using org.GraphDefined.Vanaheimr.Illias;

#endregion

namespace cloud.charging.open.protocols.WWCP
{

    /// <summary>
    /// The unique identification of an electric mobility account (eMAId).
    /// </summary>
    public readonly struct eMobilityAccount_Id : IId,
                                                 IEquatable<eMobilityAccount_Id>,
                                                 IComparable<eMobilityAccount_Id>
    {

        #region Data

        private readonly String InternalId;

        /// <summary>
        /// The regular expression for parsing an electric mobility account identification.
        /// </summary>
        public static readonly Regex eMobilityAccountId_RegEx  = new Regex(@"^([A-Za-z]{2}\-?[A-Za-z0-9]{3})\-?C([A-Za-z0-9]{8})\-?([\d|A-Za-z])$|" +         // ISO
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
        public eMobilityProvider_Id  ProviderId    { get; }

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
        /// Generate a new electric mobility account identification
        /// based on the given string.
        /// </summary>
        /// <param name="ProviderId">The unique identification of an e-mobility provider.</param>
        /// <param name="Suffix">The suffix of the electric mobility account identification.</param>
        /// <param name="CheckDigit">An optional check digit of the electric mobility account identification.</param>
        private eMobilityAccount_Id(String                InternalId,
                                    eMobilityProvider_Id  ProviderId,
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
        /// Parse the given text representation of an electric vehicle contract identification.
        /// </summary>
        /// <param name="Text">A text representation of an electric vehicle contract identification.</param>
        public static eMobilityAccount_Id Parse(String Text)
        {

            if (TryParse(Text, out eMobilityAccount_Id eMobilityAccountId))
                return eMobilityAccountId;

            throw new ArgumentException("Illegal electric vehicle contract identification '" + Text + "'!");

        }

        #endregion

        #region (static) Parse   (ProviderId, Suffix)

        /// <summary>
        /// Parse the given electric vehicle contract identification.
        /// </summary>
        /// <param name="ProviderId">The unique identification of an e-mobility provider.</param>
        /// <param name="Suffix">The suffix of the electric vehicle contract identification.</param>
        public static eMobilityAccount_Id Parse(eMobilityProvider_Id  ProviderId,
                                    String       Suffix)
        {

            #region Initial checks

            if (Suffix != null)
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

                default: // ISO_HYPHEN
                    return Parse(ProviderId + "-" + Suffix);

            }

        }

        #endregion

        #region (static) TryParse(Text)

        /// <summary>
        /// Try to parse the given string as an electric vehicle contract identification.
        /// </summary>
        /// <param name="Text">A text representation of an electric vehicle contract identification.</param>
        public static eMobilityAccount_Id? TryParse(String Text)
        {

            if (TryParse(Text, out eMobilityAccount_Id eMobilityAccountId))
                return eMobilityAccountId;

            return new eMobilityAccount_Id?();

        }

        #endregion

        #region (static) TryParse(Text, out eMobilityAccountId)

        /// <summary>
        /// Try to parse the given string as an electric vehicle contract identification.
        /// </summary>
        /// <param name="Text">A text representation of an electric vehicle contract identification.</param>
        /// <param name="eMobilityAccountId">The parsed electric vehicle contract identification.</param>
        public static Boolean TryParse(String Text, out eMobilityAccount_Id eMobilityAccountId)
        {

            #region Initial checks

            if (Text != null)
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

                if (matchCollection.Count != 1)
                {
                    eMobilityAccountId = default;
                    return false;
                }


                // ISO: DE-GDF-C12022187-X, DEGDFC12022187X
                if (eMobilityProvider_Id.TryParse(matchCollection[0].Groups[1].Value, out eMobilityProvider_Id providerId))
                {

                    eMobilityAccountId = new eMobilityAccount_Id(Text,
                                         providerId,
                                         matchCollection[0].Groups[2].Value,
                                         matchCollection[0].Groups[3].Value[0]);

                    return true;

                }


                // DIN: DE*GDF*0010LY*3, DE-GDF-0010LY-3, DEGDF0010LY3
                if (eMobilityProvider_Id.TryParse(matchCollection[0].Groups[4].Value,  out providerId))
                {

                    if (providerId.Format == ProviderIdFormats.ISO_HYPHEN)
                        providerId = providerId.ChangeFormat(ProviderIdFormats.DIN_HYPHEN);

                    eMobilityAccountId = new eMobilityAccount_Id(Text,
                                         providerId.ChangeFormat(ProviderIdFormats.DIN_HYPHEN),
                                         matchCollection[0].Groups[5].Value,
                                         matchCollection[0].Groups[6].Value[0]);

                    return true;

                }

            }
            catch (Exception)
            {
                eMobilityAccountId = default;
                return false;
            }

            eMobilityAccountId = default;
            return false;

        }

        #endregion


//        #region Parse(Text)

//        /// <summary>
//        /// Parse the given string as an electric mobility account identification.
//        /// </summary>
//        /// <param name="Text">A text representation of an electric mobility account identification.</param>
//        public static eMobilityAccount_Id Parse(String Text)
//        {

//            #region Initial checks

//            if (Text.IsNullOrEmpty())
//                throw new ArgumentNullException(nameof(Text), "The text representation of the electric mobility account identification must not be null or empty!");

//            #endregion

//            var _MatchCollection = eMobilityAccountId_RegEx.Matches(Text);

//            if (_MatchCollection.Count != 1)
//                throw new ArgumentException("Illegal electric mobility account identification '" + Text + "'!");

//            eMobilityProvider_Id _ProviderId;

//            // OICP DIN STAR:  DE*BMW*0010LY*3
//            if (eMobilityProvider_Id.TryParse(_MatchCollection[0].Groups[1].Value,  out _ProviderId))
//                return new eMobilityAccount_Id(_ProviderId,
//                                               _MatchCollection[0].Groups[2].Value,
//                                               _MatchCollection[0].Groups[3].Value[0]);

//            // OICP DIN HYPEN: DE-BMW-0010LY-3
//            if (eMobilityProvider_Id.TryParse(_MatchCollection[0].Groups[4].Value,  out _ProviderId))
//                return new eMobilityAccount_Id(_ProviderId.ChangeFormat(ProviderIdFormats.DIN_HYPHEN),
//                                               _MatchCollection[0].Groups[5].Value,
//                                               _MatchCollection[0].Groups[6].Value[0]);

//            // OICP DIN:       DEBMW0010LY3
//            if (eMobilityProvider_Id.TryParse(_MatchCollection[0].Groups[7].Value,  out _ProviderId))
//                return new eMobilityAccount_Id(_ProviderId.ChangeFormat(ProviderIdFormats.DIN),
//                                               _MatchCollection[0].Groups[8].Value,
//                                               _MatchCollection[0].Groups[9].Value[0]);




//            //                 FR*MSP*C0001000LY
//            if (eMobilityProvider_Id.TryParse(_MatchCollection[0].Groups[10].Value, out _ProviderId))
//                return new eMobilityAccount_Id(_ProviderId,
//                                               _MatchCollection[0].Groups[11].Value);

//            // OICP ISO Hypen: DE-BMW-C001000LY-3
//            if (eMobilityProvider_Id.TryParse(_MatchCollection[0].Groups[12].Value, out _ProviderId))
//                return new eMobilityAccount_Id(_ProviderId.ChangeFormat(ProviderIdFormats.ISO_HYPHEN),
//                                               _MatchCollection[0].Groups[13].Value,
//                                               _MatchCollection[0].Groups[14].Value[0]);

//            // OICP ISO:       DEBMWC001000LY3
//            if (eMobilityProvider_Id.TryParse(_MatchCollection[0].Groups[15].Value, out _ProviderId))
//                return new eMobilityAccount_Id(_ProviderId.ChangeFormat(ProviderIdFormats.ISO_HYPHEN),
//                                               _MatchCollection[0].Groups[16].Value,
//                                               _MatchCollection[0].Groups[17].Value[0]);


//            // OCHP

//            throw new ArgumentException("Illegal electric mobility account identification '" + Text + "'!");

//        }

//        #endregion

//        #region Parse(ProviderId, Suffix)

//        /// <summary>
//        /// Parse the given electric mobility account identification.
//        /// </summary>
//        /// <param name="ProviderId">The unique identification of an e-mobility provider.</param>
//        /// <param name="Suffix">The suffix of the electric mobility account identification.</param>
//        public static eMobilityAccount_Id Parse(eMobilityProvider_Id  ProviderId,
//                                                String                Suffix)
//        {

//            #region Initial checks

//            if (Suffix.IsNullOrEmpty())
//                throw new ArgumentNullException(nameof(Suffix), "The given electric mobility account identification suffix must not be null or empty!");

//            #endregion

//            switch (ProviderId.Format)
//            {

//                case ProviderIdFormats.DIN:
//                    return Parse(ProviderId +       Suffix);

//                case ProviderIdFormats.DIN_STAR:
//                    return Parse(ProviderId + "*" + Suffix);

//                case ProviderIdFormats.DIN_HYPHEN:
//                    return Parse(ProviderId + "-" + Suffix);


//                case ProviderIdFormats.ISO:
//                    return Parse(ProviderId +       Suffix);

//                default: // ISO_HYPHEN
//                    return Parse(ProviderId + "-" + Suffix);

//            }

//        }

//        #endregion

//        #region TryParse(Text)

//        /// <summary>
//        /// Parse the given string as an electric mobility account identification.
//        /// </summary>
//        /// <param name="Text">A text representation of an electric mobility account identification.</param>
//        public static eMobilityAccount_Id? TryParse(String Text)
//        {

//            if (TryParse(Text, out eMobilityAccount_Id eMAId))
//                return eMAId;

//            return new eMobilityAccount_Id?();

//        }

//        #endregion

//        #region TryParse(Text, out eMobilityAccountId)

//        /// <summary>
//        /// Parse the given string as an electric mobility account identification.
//        /// </summary>
//        /// <param name="Text">A text representation of an electric mobility account identification.</param>
//        /// <param name="eMobilityAccountId">The parsed electric mobility account identification.</param>
//        public static Boolean TryParse(String Text, out eMobilityAccount_Id eMobilityAccountId)
//        {

//            #region Initial checks

//            if (Text.IsNullOrEmpty())
//            {
//                eMobilityAccountId = default(eMobilityAccount_Id);
//                return false;
//            }

//            #endregion

//            try
//            {

//                eMobilityAccountId = default(eMobilityAccount_Id);

//                var _MatchCollection = eMobilityAccountId_RegEx.Matches(Text.Trim().ToUpper());

//                if (_MatchCollection.Count != 1)
//                    return false;

//                eMobilityProvider_Id _ProviderId;

//                #region OICP DIN STAR:  DE*BMW*0010LY*3

//                if (eMobilityProvider_Id.TryParse(_MatchCollection[0].Groups[1].Value, out _ProviderId))
//                {

//                    eMobilityAccountId = new eMobilityAccount_Id(_ProviderId,
//                                                                 _MatchCollection[0].Groups[2].Value,
//                                                                 _MatchCollection[0].Groups[3].Value[0]);

//                    return true;

//                }

//                #endregion

//                #region OICP DIN HYPEN: DE-BMW-0010LY-3

//                if (eMobilityProvider_Id.TryParse(_MatchCollection[0].Groups[4].Value,  out _ProviderId))
//                {

//                    eMobilityAccountId = new eMobilityAccount_Id(_ProviderId.ChangeFormat(ProviderIdFormats.DIN_HYPHEN),
//                                                                 _MatchCollection[0].Groups[5].Value,
//                                                                 _MatchCollection[0].Groups[6].Value[0]);

//                    return true;

//                }

//                #endregion

//                #region OICP DIN:       DEBMW0010LY3

//                if (eMobilityProvider_Id.TryParse(_MatchCollection[0].Groups[7].Value,  out _ProviderId))
//                {

//                    eMobilityAccountId = new eMobilityAccount_Id(_ProviderId.ChangeFormat(ProviderIdFormats.DIN),
//                                                                 _MatchCollection[0].Groups[8].Value,
//                                                                 _MatchCollection[0].Groups[9].Value[0]);

//                    return true;

//                }

//                #endregion


//                #region                 FR*MSP*C0001000LY

//                if (eMobilityProvider_Id.TryParse(_MatchCollection[0].Groups[10].Value, out _ProviderId))
//                {

//                    eMobilityAccountId = new eMobilityAccount_Id(_ProviderId,
//                                                                 _MatchCollection[0].Groups[11].Value);

//                    return true;

//                }

//                #endregion

//                #region OICP ISO Hypen: DE-BMW-C001000LY-3

//                if (eMobilityProvider_Id.TryParse(_MatchCollection[0].Groups[12].Value, out _ProviderId))
//                {

//                    eMobilityAccountId = new eMobilityAccount_Id(_ProviderId,
//                                                                 _MatchCollection[0].Groups[13].Value,
//                                                                 _MatchCollection[0].Groups[14].Value[0]);

//                    return true;

//                }

//                #endregion

//                #region OICP ISO:       DEBMWC001000LY3

//                if (eMobilityProvider_Id.TryParse(_MatchCollection[0].Groups[15].Value, out _ProviderId))
//                {

//                    eMobilityAccountId = new eMobilityAccount_Id(_ProviderId.ChangeFormat(ProviderIdFormats.ISO_HYPHEN),
//                                                                 _MatchCollection[0].Groups[16].Value,
//                                                                 _MatchCollection[0].Groups[17].Value[0]);

//                    return true;

//                }

//                #endregion


//                // OCHP

//            }
//#pragma warning disable RCS1075  // Avoid empty catch clause that catches System.Exception.
//#pragma warning disable RECS0022 // A catch clause that catches System.Exception and has an empty body
//            catch (Exception)
//#pragma warning restore RECS0022 // A catch clause that catches System.Exception and has an empty body
//#pragma warning restore RCS1075  // Avoid empty catch clause that catches System.Exception.
//            { }

//            eMobilityAccountId = default(eMobilityAccount_Id);
//            return false;

//        }

//        #endregion

        #region Clone

        /// <summary>
        /// Clone this EVSE identification.
        /// </summary>
        public eMobilityAccount_Id Clone

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
        /// <returns>true|false</returns>
        public static Boolean operator == (eMobilityAccount_Id eMobilityAccountId1, eMobilityAccount_Id eMobilityAccountId2)
        {

            // If both are null, or both are same instance, return true.
            if (ReferenceEquals(eMobilityAccountId1, eMobilityAccountId2))
                return true;

            // If one is null, but not both, return false.
            if (((Object) eMobilityAccountId1 == null) || ((Object) eMobilityAccountId2 == null))
                return false;

            if ((Object) eMobilityAccountId1 == null)
                throw new ArgumentNullException(nameof(eMobilityAccountId1),  "The given contract identification must not be null!");

            return eMobilityAccountId1.Equals(eMobilityAccountId2);

        }

        #endregion

        #region Operator != (eMobilityAccountId1, eMobilityAccountId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="eMobilityAccountId1">A contract identification.</param>
        /// <param name="eMobilityAccountId2">Another contract identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator != (eMobilityAccount_Id eMobilityAccountId1, eMobilityAccount_Id eMobilityAccountId2)
            => !(eMobilityAccountId1 == eMobilityAccountId2);

        #endregion

        #region Operator <  (eMobilityAccountId1, eMobilityAccountId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="eMobilityAccountId1">A contract identification.</param>
        /// <param name="eMobilityAccountId2">Another contract identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator < (eMobilityAccount_Id eMobilityAccountId1, eMobilityAccount_Id eMobilityAccountId2)
        {

            if ((Object) eMobilityAccountId1 == null)
                throw new ArgumentNullException(nameof(eMobilityAccountId1),  "The given contract identification must not be null!");

            return eMobilityAccountId1.CompareTo(eMobilityAccountId2) < 0;

        }

        #endregion

        #region Operator <= (eMobilityAccountId1, eMobilityAccountId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="eMobilityAccountId1">A contract identification.</param>
        /// <param name="eMobilityAccountId2">Another contract identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator <= (eMobilityAccount_Id eMobilityAccountId1, eMobilityAccount_Id eMobilityAccountId2)
            => !(eMobilityAccountId1 > eMobilityAccountId2);

        #endregion

        #region Operator >  (eMobilityAccountId1, eMobilityAccountId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="eMobilityAccountId1">A contract identification.</param>
        /// <param name="eMobilityAccountId2">Another contract identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator > (eMobilityAccount_Id eMobilityAccountId1, eMobilityAccount_Id eMobilityAccountId2)
        {

            if ((Object) eMobilityAccountId1 == null)
                throw new ArgumentNullException(nameof(eMobilityAccountId1),  "The given contract identification must not be null!");

            return eMobilityAccountId1.CompareTo(eMobilityAccountId2) > 0;

        }

        #endregion

        #region Operator >= (eMobilityAccountId1, eMobilityAccountId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="eMobilityAccountId1">A contract identification.</param>
        /// <param name="eMobilityAccountId2">Another contract identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator >= (eMobilityAccount_Id eMobilityAccountId1, eMobilityAccount_Id eMobilityAccountId2)
            => !(eMobilityAccountId1 < eMobilityAccountId2);

        #endregion

        #endregion

        #region IComparable<eMobilityAccountId> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        public Int32 CompareTo(Object Object)
        {

            if (Object == null)
                throw new ArgumentNullException(nameof(Object),  "The given object must not be null!");

            if (!(Object is eMobilityAccount_Id eMobilityAccountId))
                throw new ArgumentException("The given object is not a eMobilityAccountId!");

            return CompareTo(eMobilityAccountId);

        }

        #endregion

        #region CompareTo(eMobilityAccountId)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="eMobilityAccountId">An object to compare with.</param>
        public Int32 CompareTo(eMobilityAccount_Id eMobilityAccountId)
        {

            if ((Object) eMobilityAccountId == null)
                throw new ArgumentNullException(nameof(eMobilityAccountId),  "The given contract identification must not be null!");

            return String.Compare(InternalId, eMobilityAccountId.InternalId, StringComparison.Ordinal);

        }

        #endregion

        #endregion

        #region IEquatable<eMobilityAccountId> Members

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

            if (!(Object is eMobilityAccount_Id eMobilityAccountId))
                return false;

            return Equals(eMobilityAccountId);

        }

        #endregion

        #region Equals(eMobilityAccountId)

        /// <summary>
        /// Compares two contract identifications for equality.
        /// </summary>
        /// <param name="eMobilityAccountId">A contract identification to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public Boolean Equals(eMobilityAccount_Id eMobilityAccountId)
        {

            if ((Object) eMobilityAccountId == null)
                return false;

            return InternalId.Equals(eMobilityAccountId.InternalId);

        }

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
            => InternalId;

        #endregion

    }

}
