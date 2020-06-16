/*
 * Copyright (c) 2014-2020 GraphDefined GmbH <achim.friedland@graphdefined.com>
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

using org.GraphDefined.Vanaheimr.Illias;

#endregion

namespace org.GraphDefined.WWCP
{

    /// <summary>
    /// The unique identification of a charge detail record.
    /// </summary>
    public struct ChargeDetailRecord_Id : IId,
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
        /// Indicates whether this identification is null or empty.
        /// </summary>
        public Boolean IsNullOrEmpty
            => InternalId.IsNullOrEmpty();

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


        #region New

        /// <summary>
        /// Returns a new charge detail record identification.
        /// </summary>
        public static ChargeDetailRecord_Id New

            => ChargeDetailRecord_Id.Parse(Guid.NewGuid().ToString());

        #endregion

        #region Parse(Text)

        /// <summary>
        /// Parse the given string as a charge detail record identification.
        /// </summary>
        /// <param name="Text">A text representation of a charge detail record identification.</param>
        public static ChargeDetailRecord_Id Parse(String Text)

            => new ChargeDetailRecord_Id(Text);

        #endregion

        #region TryParse(Text, out ChargeDetailRecordId)

        /// <summary>
        /// Parse the given string as a charge detail record identification.
        /// </summary>
        /// <param name="Text">A text representation of a charge detail record identification.</param>
        /// <param name="ChargeDetailRecordId">The parsed charge detail record identification.</param>
        public static Boolean TryParse(String Text, out ChargeDetailRecord_Id ChargeDetailRecordId)
        {
            try
            {

                ChargeDetailRecordId = new ChargeDetailRecord_Id(Text);

                return true;

            }
            catch (Exception)
            {
                ChargeDetailRecordId = default(ChargeDetailRecord_Id);
                return false;
            }
        }

        #endregion

        #region Clone

        /// <summary>
        /// Clone this charge detail record identification.
        /// </summary>
        public ChargeDetailRecord_Id Clone

            => new ChargeDetailRecord_Id(
                   new String(InternalId.ToCharArray())
               );

        #endregion


        #region Operator overloading

        #region Operator == (ChargeDetailRecordId1, ChargeDetailRecordId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargeDetailRecordId1">A ChargeDetailRecordId.</param>
        /// <param name="ChargeDetailRecordId2">Another ChargeDetailRecordId.</param>
        /// <returns>true|false</returns>
        public static Boolean operator == (ChargeDetailRecord_Id ChargeDetailRecordId1, ChargeDetailRecord_Id ChargeDetailRecordId2)
        {

            // If both are null, or both are same instance, return true.
            if (ReferenceEquals(ChargeDetailRecordId1, ChargeDetailRecordId2))
                return true;

            // If one is null, but not both, return false.
            if (((Object) ChargeDetailRecordId1 == null) || ((Object) ChargeDetailRecordId2 == null))
                return false;

            return ChargeDetailRecordId1.Equals(ChargeDetailRecordId2);

        }

        #endregion

        #region Operator != (ChargeDetailRecordId1, ChargeDetailRecordId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargeDetailRecordId1">A ChargeDetailRecordId.</param>
        /// <param name="ChargeDetailRecordId2">Another ChargeDetailRecordId.</param>
        /// <returns>true|false</returns>
        public static Boolean operator != (ChargeDetailRecord_Id ChargeDetailRecordId1, ChargeDetailRecord_Id ChargeDetailRecordId2)
            => !(ChargeDetailRecordId1 == ChargeDetailRecordId2);

        #endregion

        #region Operator <  (ChargeDetailRecordId1, ChargeDetailRecordId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargeDetailRecordId1">A ChargeDetailRecordId.</param>
        /// <param name="ChargeDetailRecordId2">Another ChargeDetailRecordId.</param>
        /// <returns>true|false</returns>
        public static Boolean operator < (ChargeDetailRecord_Id ChargeDetailRecordId1, ChargeDetailRecord_Id ChargeDetailRecordId2)
        {

            if ((Object) ChargeDetailRecordId1 == null)
                throw new ArgumentNullException(nameof(ChargeDetailRecordId1), "The given ChargeDetailRecordId1 must not be null!");

            return ChargeDetailRecordId1.CompareTo(ChargeDetailRecordId2) < 0;

        }

        #endregion

        #region Operator <= (ChargeDetailRecordId1, ChargeDetailRecordId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargeDetailRecordId1">A ChargeDetailRecordId.</param>
        /// <param name="ChargeDetailRecordId2">Another ChargeDetailRecordId.</param>
        /// <returns>true|false</returns>
        public static Boolean operator <= (ChargeDetailRecord_Id ChargeDetailRecordId1, ChargeDetailRecord_Id ChargeDetailRecordId2)
            => !(ChargeDetailRecordId1 > ChargeDetailRecordId2);

        #endregion

        #region Operator >  (ChargeDetailRecordId1, ChargeDetailRecordId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargeDetailRecordId1">A ChargeDetailRecordId.</param>
        /// <param name="ChargeDetailRecordId2">Another ChargeDetailRecordId.</param>
        /// <returns>true|false</returns>
        public static Boolean operator > (ChargeDetailRecord_Id ChargeDetailRecordId1, ChargeDetailRecord_Id ChargeDetailRecordId2)
        {

            if ((Object) ChargeDetailRecordId1 == null)
                throw new ArgumentNullException(nameof(ChargeDetailRecordId1), "The given ChargeDetailRecordId1 must not be null!");

            return ChargeDetailRecordId1.CompareTo(ChargeDetailRecordId2) > 0;

        }

        #endregion

        #region Operator >= (ChargeDetailRecordId1, ChargeDetailRecordId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargeDetailRecordId1">A ChargeDetailRecordId.</param>
        /// <param name="ChargeDetailRecordId2">Another ChargeDetailRecordId.</param>
        /// <returns>true|false</returns>
        public static Boolean operator >= (ChargeDetailRecord_Id ChargeDetailRecordId1, ChargeDetailRecord_Id ChargeDetailRecordId2)
            => !(ChargeDetailRecordId1 < ChargeDetailRecordId2);

        #endregion

        #endregion

        #region IComparable<ChargeDetailRecordId> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        public Int32 CompareTo(Object Object)
        {

            if (Object == null)
                throw new ArgumentNullException(nameof(Object), "The given object must not be null!");

            if (!(Object is ChargeDetailRecord_Id))
                throw new ArgumentException("The given object is not a charge detail record identification!",
                                            nameof(Object));

            return CompareTo((ChargeDetailRecord_Id) Object);

        }

        #endregion

        #region CompareTo(ChargeDetailRecordId)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargeDetailRecordId">An object to compare with.</param>
        public Int32 CompareTo(ChargeDetailRecord_Id ChargeDetailRecordId)
        {

            if ((Object) ChargeDetailRecordId == null)
                throw new ArgumentNullException(nameof(ChargeDetailRecordId),  "The given charge detail record identification must not be null!");

            // Compare the length of the ChargeDetailRecordIds
            var _Result = this.Length.CompareTo(ChargeDetailRecordId.Length);

            if (_Result == 0)
                _Result = String.Compare(InternalId, ChargeDetailRecordId.InternalId, StringComparison.Ordinal);

            return _Result;

        }

        #endregion

        #endregion

        #region IEquatable<ChargeDetailRecordId> Members

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

            if (!(Object is ChargeDetailRecord_Id))
                return false;

            return Equals((ChargeDetailRecord_Id) Object);

        }

        #endregion

        #region Equals(ChargeDetailRecordId)

        /// <summary>
        /// Compares two ChargeDetailRecordIds for equality.
        /// </summary>
        /// <param name="ChargeDetailRecordId">A ChargeDetailRecordId to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public Boolean Equals(ChargeDetailRecord_Id ChargeDetailRecordId)
        {

            if ((Object) ChargeDetailRecordId == null)
                return false;

            return InternalId.Equals(ChargeDetailRecordId.InternalId);

        }

        #endregion

        #endregion

        #region GetHashCode()

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
