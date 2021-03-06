﻿/*
 * Copyright (c) 2014-2021 GraphDefined GmbH <achim.friedland@graphdefined.com>
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
    /// How strictly to parse EVSE Ids.
    /// </summary>
    public enum EVSEIdParsingMode
    {

        /// <summary>
        /// Allow more EVSE Id variants.
        /// </summary>
        relaxed,

        /// <summary>
        /// Strict parsing of EVSE Ids.
        /// </summary>
        strict

    }


    /// <summary>
    /// The unique identification of an Electric Vehicle Supply Equipment (EVSE).
    /// </summary>
    public readonly struct EVSE_Id : IId,
                                     IEquatable<EVSE_Id>,
                                     IComparable<EVSE_Id>

    {

        #region Data

        /// <summary>
        /// The regular expression for relaxed parsing an EVSE identification.
        /// </summary>                                                  // new format:
        public static readonly Regex  EVSEId_relaxed_RegEx  = new Regex(@"^([A-Za-z]{2}\*?[A-Za-z0-9]{3})\*?E([A-Za-z0-9\*]{1,30})$" + " | " +

                                                                        // old format:
                                                                        @"^(\+?[0-9]{1,5}\*[0-9]{3,6})\*?([A-Za-z0-9\*]{1,32})$",
                                                                        // Hubject ([A-Za-z]{2}\*?[A-Za-z0-9]{3}\*?E[A-Za-z0-9\*]{1,30})  |  (\+?[0-9]{1,3}\*[0-9]{3,6}\*[0-9\*]{1,32})
                                                                        // OCHP.eu                                                           /^\+[0-9]{1,3}\*?[A-Z0-9]{3}\*?[A-Z0-9\*]{0,40}(?=$)/i;
                                                                        // var valid_evse_warning= /^(?=.*[a-z])(?=.*[A-Z])[a-zA-Z0-9\*]*/; // look ahead: at least one upper and one lower case letter

                                                                        RegexOptions.IgnorePatternWhitespace);

        /// <summary>
        /// The regular expression for strict parsing an EVSE identification.
        /// </summary>                                                  // new format:
        public static readonly Regex  EVSEId_strict_RegEx   = new Regex(@"^([A-Z]{2}\*?[A-Z0-9]{3})\*?E([A-Z0-9][A-Z0-9\*]{1,30})$",
                                                                        RegexOptions.IgnorePatternWhitespace);

        /// <summary>
        /// The regular expression for parsing an ISO EVSE identification suffix.
        /// </summary>
        public static readonly Regex IdSuffixISO_RegEx      = new Regex(@"^([A-Za-z0-9\*]{1,30})$",
                                                                        RegexOptions.IgnorePatternWhitespace);

        /// <summary>
        /// The regular expression for parsing a DIN EVSE identification suffix.
        /// </summary>
        public static readonly Regex IdSuffixDIN_RegEx      = new Regex(@"^([0-9\*]{1,32})$",
                                                                        RegexOptions.IgnorePatternWhitespace);

        private static readonly Random random = new Random();

        #endregion

        #region Properties

        /// <summary>
        /// The charging station operator identification.
        /// </summary>
        public ChargingStationOperator_Id  OperatorId   { get; }

        /// <summary>
        /// The suffix of the identification.
        /// </summary>
        public String                      Suffix       { get; }

        /// <summary>
        /// The detected format of the EVSE identification.
        /// </summary>
        public OperatorIdFormats           Format
            => OperatorId.Format;

        /// <summary>
        /// Indicates whether this identification is null or empty.
        /// </summary>
        public Boolean IsNullOrEmpty
            => Suffix.IsNullOrEmpty();

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
                        return OperatorId.Length + 1 + (UInt64) Suffix.Length;

                    case OperatorIdFormats.ISO_STAR:
                        return OperatorId.Length + 2 + (UInt64) Suffix.Length;

                    default:  // ISO
                        return OperatorId.Length + 1 + (UInt64) Suffix.Length;

                }

            }
        }

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Generate a new Electric Vehicle Supply Equipment (EVSE) identification
        /// based on the given charging station operator and identification suffix.
        /// </summary>
        /// <param name="OperatorId">The unique identification of a charging station operator.</param>
        /// <param name="Suffix">The suffix of the EVSE identification.</param>
        private EVSE_Id(ChargingStationOperator_Id  OperatorId,
                        String                      Suffix)
        {

            #region Initial checks

            if (Suffix.IsNullOrEmpty())
                throw new ArgumentNullException(nameof(Suffix),  "The EVSE identification suffix must not be null or empty!");

            #endregion

            this.OperatorId  = OperatorId;
            this.Suffix      = Suffix;

        }

        #endregion


        #region Random  (OperatorId, Length = 12, Mapper = null)

        /// <summary>
        /// Generate a new unique identification of an EVSE.
        /// </summary>
        /// <param name="OperatorId">The unique identification of a charging station operator.</param>
        /// <param name="Length">The expected length of the EVSE identification suffix</param>
        /// <param name="Mapper">A delegate to modify the newly generated EVSE identification.</param>
        public static EVSE_Id Random(ChargingStationOperator_Id  OperatorId,
                                     Byte                        Length  = 12,
                                     Func<String, String>        Mapper  = null)


            => new EVSE_Id(OperatorId,
                           Mapper != null
                               ? Mapper(random.RandomString(Length))
                               :        random.RandomString(Length));

        #endregion

        #region Parse   (OperatorId, Suffix)

        /// <summary>
        /// Parse the given string as an EVSE identification.
        /// </summary>
        /// <param name="OperatorId">The unique identification of a charging station operator.</param>
        /// <param name="Suffix">The suffix of the EVSE identification.</param>
        public static EVSE_Id Parse(ChargingStationOperator_Id  OperatorId,
                                    String                      Suffix)
        {

            switch (OperatorId.Format)
            {

                case OperatorIdFormats.ISO:
                case OperatorIdFormats.ISO_STAR:
                    if (!IdSuffixISO_RegEx.IsMatch(Suffix))
                        throw new ArgumentException("Illegal EVSE identification suffix '" + Suffix + "'!",
                                                    nameof(Suffix));
                    return new EVSE_Id(OperatorId,
                                       Suffix);

                case OperatorIdFormats.DIN:
                    if (!IdSuffixDIN_RegEx.IsMatch(Suffix))
                        throw new ArgumentException("Illegal EVSE identification suffix '" + Suffix + "'!",
                                                    nameof(Suffix));
                    return new EVSE_Id(OperatorId,
                                       Suffix);

            }

            throw new ArgumentException("Illegal EVSE identification suffix '" + Suffix + "'!",
                                        nameof(Suffix));

        }

        #endregion

        #region Parse   (Text,              ParsingMode = relaxed)

        /// <summary>
        /// Parse the given text-representation of an EVSE identification.
        /// </summary>
        /// <param name="Text">A text-representation of an EVSE identification.</param>
        /// <param name="ParsingMode">How strictly to parse the given EVSE identification.</param>
        public static EVSE_Id Parse(String              Text,
                                    EVSEIdParsingMode?  ParsingMode = EVSEIdParsingMode.relaxed)
        {

            if (TryParse(Text,
                         out EVSE_Id EVSEId,
                         ParsingMode))
            {
                return EVSEId;
            }

            throw new ArgumentException("Illegal EVSE identification '" + Text + "'!",
                                        nameof(Text));

        }

        #endregion

        #region TryParse(Text,              ParsingMode = relaxed)

        // Note: The following is needed to satisfy pattern matching delegates! Do not refactor it!

        /// <summary>
        /// Try to parse the given text-representation of an EVSE identification.
        /// </summary>
        /// <param name="Text">A text-representation of an EVSE identification.</param>
        public static EVSE_Id? TryParse(String Text)

            => TryParse(Text,
                        null);


        /// <summary>
        /// Try to parse the given text-representation of an EVSE identification.
        /// </summary>
        /// <param name="Text">A text-representation of an EVSE identification.</param>
        /// <param name="ParsingMode">How strictly to parse the given EVSE identification.</param>
        public static EVSE_Id? TryParse(String              Text,
                                        EVSEIdParsingMode?  ParsingMode)
        {

            if (TryParse(Text,
                         out EVSE_Id EVSEId,
                         ParsingMode))
            {
                return EVSEId;
            }

            return default;

        }

        #endregion

        #region TryParse(Text, out EVSE_Id, ParsingMode = relaxed)

        // Note: The following is needed to satisfy pattern matching delegates! Do not refactor it!

        /// <summary>
        /// Try to parse the given text-representation of an EVSE identification.
        /// </summary>
        /// <param name="Text">A text-representation of an EVSE identification.</param>
        /// <param name="EVSEId">The parsed EVSE identification.</param>
        public static Boolean TryParse(String       Text,
                                       out EVSE_Id  EVSEId)

            => TryParse(Text,
                        out EVSEId,
                        null);


        /// <summary>
        /// Try to parse the given text-representation of an EVSE identification.
        /// </summary>
        /// <param name="Text">A text-representation of an EVSE identification.</param>
        /// <param name="EVSEId">The parsed EVSE identification.</param>
        /// <param name="ParsingMode">How strictly to parse the given EVSE identification.</param>
        public static Boolean TryParse(String              Text,
                                       out EVSE_Id         EVSEId,
                                       EVSEIdParsingMode?  ParsingMode)
        {

            #region Initial checks

            EVSEId  = default;
            Text    = Text?.Trim();

            if (Text.IsNullOrEmpty())
                return false;

            #endregion

            try
            {

                var matchCollection = (ParsingMode == EVSEIdParsingMode.relaxed
                                           ? EVSEId_relaxed_RegEx
                                           : EVSEId_strict_RegEx).Matches(Text);

                if (matchCollection.Count != 1)
                    return false;


                // New format...
                if (ChargingStationOperator_Id.TryParse(matchCollection[0].Groups[1].Value,
                                                        out ChargingStationOperator_Id operatorId))
                {

                    EVSEId = new EVSE_Id(operatorId,
                                         matchCollection[0].Groups[2].Value);

                    return true;

                }

                // Old format...
                if (ChargingStationOperator_Id.TryParse(matchCollection[0].Groups[3].Value,
                                                        out operatorId))
                {

                    EVSEId = new EVSE_Id(operatorId,
                                         matchCollection[0].Groups[4].Value);

                    return true;

                }

            }
            catch (Exception)
            { }

            return false;

        }

        #endregion

        #region ChangeFormat(NewFormat)

        /// <summary>
        /// Return a new EVSE identification in the given format.
        /// </summary>
        /// <param name="NewFormat">An EVSE identification format.</param>
        public EVSE_Id ChangeFormat(OperatorIdFormats NewFormat)

            => new EVSE_Id(OperatorId.ChangeFormat(NewFormat),
                           Suffix);

        #endregion

        #region Clone

        /// <summary>
        /// Clone this EVSE identification.
        /// </summary>
        public EVSE_Id Clone

            => new EVSE_Id(OperatorId.Clone,
                           new String(Suffix.ToCharArray()));

        #endregion


        #region Replace(Old, New)

        /// <summary>
        /// Returns a new EVSE Id in which all occurrences of the specified
        /// old string value are replaced with the new value.
        /// </summary>
        /// <param name="OldValue">The string to be replaced.</param>
        /// <param name="NewValue">The new string value.</param>
        public EVSE_Id Replace(String  OldValue,
                               String  NewValue)

            => Parse(ToString().Replace(OldValue, NewValue));

        #endregion


        #region ToFormat(IdFormat)

        /// <summary>
        /// Return the identification in the given format.
        /// </summary>
        /// <param name="IdFormat">The format.</param>
        public String ToFormat(OperatorIdFormats IdFormat)

            => IdFormat == OperatorIdFormats.ISO
                   ? String.Concat(OperatorId.ToString(IdFormat), "*E", Suffix)
                   : String.Concat(OperatorId.ToString(IdFormat),  "*", Suffix);

        #endregion


        #region Operator overloading

        #region Operator == (EVSEId1, EVSEId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EVSEId1">A EVSE identification.</param>
        /// <param name="EVSEId2">Another EVSE identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator == (EVSE_Id EVSEId1, EVSE_Id EVSEId2)
            => EVSEId1.Equals(EVSEId2);

        #endregion

        #region Operator != (EVSEId1, EVSEId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EVSEId1">A EVSE identification.</param>
        /// <param name="EVSEId2">Another EVSE identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator != (EVSE_Id EVSEId1, EVSE_Id EVSEId2)
            => !EVSEId1.Equals(EVSEId2);

        #endregion

        #region Operator <  (EVSEId1, EVSEId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EVSEId1">A EVSE identification.</param>
        /// <param name="EVSEId2">Another EVSE identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator < (EVSE_Id EVSEId1, EVSE_Id EVSEId2)
            => EVSEId1.CompareTo(EVSEId2) < 0;

        #endregion

        #region Operator <= (EVSEId1, EVSEId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EVSEId1">A EVSE identification.</param>
        /// <param name="EVSEId2">Another EVSE identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator <= (EVSE_Id EVSEId1, EVSE_Id EVSEId2)
            => EVSEId1.CompareTo(EVSEId2) <= 0;

        #endregion

        #region Operator >  (EVSEId1, EVSEId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EVSEId1">A EVSE identification.</param>
        /// <param name="EVSEId2">Another EVSE identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator > (EVSE_Id EVSEId1, EVSE_Id EVSEId2)
            => EVSEId1.CompareTo(EVSEId2) > 0;

        #endregion

        #region Operator >= (EVSEId1, EVSEId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EVSEId1">A EVSE identification.</param>
        /// <param name="EVSEId2">Another EVSE identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator >= (EVSE_Id EVSEId1, EVSE_Id EVSEId2)
            => EVSEId1.CompareTo(EVSEId2) >= 0;

        #endregion

        #endregion

        #region IComparable<EVSEId> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        public Int32 CompareTo(Object Object)

            => Object is EVSE_Id EVSEId
                   ? CompareTo(EVSEId)
                   : throw new ArgumentException("The given object is not an EVSE identification!", nameof(Object));

        #endregion

        #region CompareTo(EVSEId)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EVSEId">An object to compare with.</param>
        public Int32 CompareTo(EVSE_Id EVSEId)
        {

            var result = OperatorId.CompareTo(EVSEId.OperatorId);

            return result == 0
                       ? String.Compare(Suffix, EVSEId.Suffix, StringComparison.OrdinalIgnoreCase)
                       : result;

        }

        #endregion

        #endregion

        #region IEquatable<EVSEId> Members

        #region Equals(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        /// <returns>true|false</returns>
        public override Boolean Equals(Object Object)

            => Object is EVSE_Id EVSEId &&
                   Equals(EVSEId);

        #endregion

        #region Equals(EVSEId)

        /// <summary>
        /// Compares two EVSE identifications for equality.
        /// </summary>
        /// <param name="EVSEId">An EVSE identification to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public Boolean Equals(EVSE_Id EVSEId)

            => OperatorId.Equals(EVSEId.OperatorId) &&
               String.Equals(Suffix, EVSEId.Suffix, StringComparison.OrdinalIgnoreCase);

        #endregion

        #endregion

        #region (override) GetHashCode()

        /// <summary>
        /// Return the HashCode of this object.
        /// </summary>
        /// <returns>The HashCode of this object.</returns>
        public override Int32 GetHashCode()

            => OperatorId.GetHashCode() ^
               Suffix.    GetHashCode();

        #endregion

        #region (override) ToString()

        /// <summary>
        /// Return a text representation of this object.
        /// ISO-IEC-15118 – Annex H "Specification of Identifiers"
        /// </summary>
        public override String ToString()

            => Format switch {
                   OperatorIdFormats.DIN       => String.Concat(OperatorId,  "*", Suffix),
                   OperatorIdFormats.ISO_STAR  => String.Concat(OperatorId, "*E", Suffix),
                   _                           => String.Concat(OperatorId,  "E", Suffix),
               };

        #endregion

    }

}
