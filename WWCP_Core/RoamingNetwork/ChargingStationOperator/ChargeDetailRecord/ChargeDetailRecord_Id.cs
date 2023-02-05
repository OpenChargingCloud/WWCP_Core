/*
 * Copyright (c) 2014-2023 GraphDefined GmbH <achim.friedland@graphdefined.com>
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

using org.GraphDefined.Vanaheimr.Illias;

#endregion

namespace cloud.charging.open.protocols.WWCP
{

    /// <summary>
    /// Extension methods for charge detail record identifications.
    /// </summary>
    public static class ChargeDetailRecordIdExtensions
    {

        /// <summary>
        /// Indicates whether this charge detail record identification is null or empty.
        /// </summary>
        /// <param name="ChargeDetailRecordId">A charge detail record identification.</param>
        public static Boolean IsNullOrEmpty(this ChargeDetailRecord_Id? ChargeDetailRecordId)
            => !ChargeDetailRecordId.HasValue || ChargeDetailRecordId.Value.IsNullOrEmpty;

        /// <summary>
        /// Indicates whether this charge detail record identification is null or empty.
        /// </summary>
        /// <param name="ChargeDetailRecordId">A charge detail record identification.</param>
        public static Boolean IsNotNullOrEmpty(this ChargeDetailRecord_Id? ChargeDetailRecordId)
            => ChargeDetailRecordId.HasValue && ChargeDetailRecordId.Value.IsNotNullOrEmpty;

    }


    /// <summary>
    /// The unique identification of a charge detail record.
    /// </summary>
    public readonly struct ChargeDetailRecord_Id : IId,
                                                   IEquatable<ChargeDetailRecord_Id>,
                                                   IComparable<ChargeDetailRecord_Id>
    {

        #region Data

        /// <summary>
        /// The internal identification.
        /// </summary>
        private readonly String InternalId;

        #endregion

        #region Properties

        /// <summary>
        /// Indicates whether this charge detail record identification is null or empty.
        /// </summary>
        public Boolean IsNullOrEmpty
            => InternalId.IsNullOrEmpty();

        /// <summary>
        /// Indicates whether this charge detail record identification is NOT null or empty.
        /// </summary>
        public Boolean IsNotNullOrEmpty
            => InternalId.IsNotNullOrEmpty();

        /// <summary>
        /// Returns the length of the identification.
        /// </summary>
        public UInt64 Length
            => (UInt64) InternalId.Length;

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new charge detail record identification.
        /// based on the given string.
        /// </summary>
        private ChargeDetailRecord_Id(String Text)
        {
            InternalId = Text;
        }

        #endregion


        #region (static) NewRandom

        /// <summary>
        /// Create a new random charge detail record identification.
        /// </summary>
        public static ChargeDetailRecord_Id NewRandom
            => Parse(Guid.NewGuid().ToString());

        #endregion

        #region (static) Parse   (Text)

        /// <summary>
        /// Parse the given string as a charge detail record identification.
        /// </summary>
        /// <param name="Text">A text representation of a charge detail record identification.</param>
        public static ChargeDetailRecord_Id Parse(String Text)
        {

            if (TryParse(Text, out ChargeDetailRecord_Id cdrId))
                return cdrId;

            throw new ArgumentException("Invalid text representation of a charge detail record identification: '" + Text + "'!",
                                        nameof(Text));

        }

        #endregion

        #region (static) TryParse(Text)

        /// <summary>
        /// Try to parse the given string as a charge detail record identification.
        /// </summary>
        /// <param name="Text">A text representation of a charge detail record identification.</param>
        public static ChargeDetailRecord_Id? TryParse(String Text)
        {

            if (TryParse(Text, out ChargeDetailRecord_Id cdrId))
                return cdrId;

            return null;

        }

        #endregion

        #region TryParse(Text, out ChargeDetailRecordId)

        /// <summary>
        /// Parse the given string as a charge detail record identification.
        /// </summary>
        /// <param name="Text">A text representation of a charge detail record identification.</param>
        /// <param name="ChargeDetailRecordId">The parsed charge detail record identification.</param>
        public static Boolean TryParse(String Text, out ChargeDetailRecord_Id ChargeDetailRecordId)
        {

            Text = Text?.Trim();

            if (Text.IsNotNullOrEmpty())
            {
                try
                {
                    ChargeDetailRecordId = new ChargeDetailRecord_Id(Text.SubstringMax(250));
                    return true;
                }
                catch
                { }
            }

            ChargeDetailRecordId = default;
            return false;

        }

        #endregion

        #region Clone

        /// <summary>
        /// Clone this charge detail record identification.
        /// </summary>
        public ChargeDetailRecord_Id Clone

            => new (
                   new String(InternalId?.ToCharArray())
               );

        #endregion


        #region Operator overloading

        #region Operator == (ChargeDetailRecordId1, ChargeDetailRecordId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargeDetailRecordId1">A charge detail record identification.</param>
        /// <param name="ChargeDetailRecordId2">Another charge detail record identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator == (ChargeDetailRecord_Id ChargeDetailRecordId1,
                                           ChargeDetailRecord_Id ChargeDetailRecordId2)

            => ChargeDetailRecordId1.Equals(ChargeDetailRecordId2);

        #endregion

        #region Operator != (ChargeDetailRecordId1, ChargeDetailRecordId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargeDetailRecordId1">A charge detail record identification.</param>
        /// <param name="ChargeDetailRecordId2">Another charge detail record identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator != (ChargeDetailRecord_Id ChargeDetailRecordId1,
                                           ChargeDetailRecord_Id ChargeDetailRecordId2)

            => !ChargeDetailRecordId1.Equals(ChargeDetailRecordId2);

        #endregion

        #region Operator <  (ChargeDetailRecordId1, ChargeDetailRecordId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargeDetailRecordId1">A charge detail record identification.</param>
        /// <param name="ChargeDetailRecordId2">Another charge detail record identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator < (ChargeDetailRecord_Id ChargeDetailRecordId1,
                                          ChargeDetailRecord_Id ChargeDetailRecordId2)

            => ChargeDetailRecordId1.CompareTo(ChargeDetailRecordId2) < 0;

        #endregion

        #region Operator <= (ChargeDetailRecordId1, ChargeDetailRecordId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargeDetailRecordId1">A charge detail record identification.</param>
        /// <param name="ChargeDetailRecordId2">Another charge detail record identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator <= (ChargeDetailRecord_Id ChargeDetailRecordId1,
                                           ChargeDetailRecord_Id ChargeDetailRecordId2)

            => ChargeDetailRecordId1.CompareTo(ChargeDetailRecordId2) <= 0;

        #endregion

        #region Operator >  (ChargeDetailRecordId1, ChargeDetailRecordId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargeDetailRecordId1">A charge detail record identification.</param>
        /// <param name="ChargeDetailRecordId2">Another charge detail record identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator > (ChargeDetailRecord_Id ChargeDetailRecordId1,
                                          ChargeDetailRecord_Id ChargeDetailRecordId2)

            => ChargeDetailRecordId1.CompareTo(ChargeDetailRecordId2) > 0;

        #endregion

        #region Operator >= (ChargeDetailRecordId1, ChargeDetailRecordId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargeDetailRecordId1">A charge detail record identification.</param>
        /// <param name="ChargeDetailRecordId2">Another charge detail record identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator >= (ChargeDetailRecord_Id ChargeDetailRecordId1,
                                           ChargeDetailRecord_Id ChargeDetailRecordId2)

            => ChargeDetailRecordId1.CompareTo(ChargeDetailRecordId2) >= 0;

        #endregion

        #endregion

        #region IComparable<ChargeDetailRecordId> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        public Int32 CompareTo(Object? Object)

            => Object is ChargeDetailRecord_Id cdrId
                   ? CompareTo(cdrId)
                   : throw new ArgumentException("The given object is not a charge detail record identification!");

        #endregion

        #region CompareTo(ChargeDetailRecordId)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargeDetailRecordId">An object to compare with.</param>
        public Int32 CompareTo(ChargeDetailRecord_Id ChargeDetailRecordId)

            => String.Compare(InternalId,
                              ChargeDetailRecordId.InternalId,
                              StringComparison.OrdinalIgnoreCase);

        #endregion

        #endregion

        #region IEquatable<ChargeDetailRecordId> Members

        #region Equals(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        /// <returns>true|false</returns>
        public override Boolean Equals(Object? Object)

            => Object is ChargeDetailRecord_Id cdrId &&
                   Equals(cdrId);

        #endregion

        #region Equals(ChargeDetailRecordId)

        /// <summary>
        /// Compares two ChargeDetailRecordIds for equality.
        /// </summary>
        /// <param name="ChargeDetailRecordId">A charge detail record identification to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public Boolean Equals(ChargeDetailRecord_Id ChargeDetailRecordId)

            => String.Equals(InternalId,
                             ChargeDetailRecordId.InternalId,
                             StringComparison.OrdinalIgnoreCase);

        #endregion

        #endregion

        #region GetHashCode()

        /// <summary>
        /// Return the hash code of this object.
        /// </summary>
        /// <returns>The hash code of this object.</returns>
        public override Int32 GetHashCode()

            => InternalId?.GetHashCode() ?? 0;

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
