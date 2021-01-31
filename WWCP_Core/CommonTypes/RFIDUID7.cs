/*
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
using System.Linq;

using org.GraphDefined.Vanaheimr.Illias;

#endregion

namespace org.GraphDefined.WWCP
{

    /// <summary>
    /// A 7-byte RFID UID.
    /// </summary>
    public struct RFIDUID7
    {

        #region Data

        private readonly static Random _Random = new Random(Guid.NewGuid().GetHashCode());

        /// <summary>
        /// The internal identification.
        /// </summary>
        private readonly String InternalId;

        #endregion

        #region Properties

        /// <summary>
        /// The length of the service session identificator.
        /// </summary>
        public UInt64 Length
            => (UInt64) InternalId.Length;

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Generate a new 7-byte RFID UID based on the given string.
        /// </summary>
        /// <param name="Text">The text representation of the 7-byte RFID UID.</param>
        private RFIDUID7(String Text)
        {
            InternalId = Text;
        }

        #endregion


        #region (static) Random

        /// <summary>
        /// Return a random 7-byte RFID UID.
        /// </summary>
        public static RFIDUID7 Random
            => new RFIDUID7(_Random.RandomHexString(14));

        #endregion


        #region (static) Parse   (Text)

        /// <summary>
        /// Parse the given string as a 7-byte RFID UID.
        /// </summary>
        /// <param name="Text">A text representation of a 7-byte RFID UID.</param>
        public static RFIDUID7 Parse(String Text)
        {

            #region Initial checks

            if (Text != null)
                Text = Text.Trim();

            if (Text.IsNullOrEmpty())
                throw new ArgumentNullException(nameof(Text), "The given text representation of a 7-byte RFID UID must not be null or empty!");

            #endregion

            if (TryParse(Text, out RFIDUID7 RFIDUID7))
                return RFIDUID7;

            throw new ArgumentNullException(nameof(Text), "The given text representation of a 7-byte RFID UID is invalid!");

        }

        #endregion

        #region (static) TryParse(Text)

        /// <summary>
        /// Try to parse the given string as a 7-byte RFID UID.
        /// </summary>
        /// <param name="Text">A text representation of a 7-byte RFID UID.</param>
        public static RFIDUID7? TryParse(String Text)
        {

            if (TryParse(Text, out RFIDUID7 RFIDUID7))
                return RFIDUID7;

            return null;

        }

        #endregion

        #region (static) TryParse(Text, out RFIDUID7)

        /// <summary>
        /// Try to parse the given string as a 7-byte RFID UID.
        /// </summary>
        /// <param name="Text">A text representation of a 7-byte RFID UID.</param>
        /// <param name="RFIDUID7">The parsed 7-byte RFID UID.</param>
        public static Boolean TryParse(String Text, out RFIDUID7 RFIDUID7)
        {

            #region Initial checks

            if (Text != null)
                Text = Text.Trim();

            if (Text.IsNullOrEmpty())
            {
                RFIDUID7 = default;
                return false;
            }

            #endregion

            try
            {
                RFIDUID7 = new RFIDUID7(Text);
                return true;
            }
            catch (Exception)
            { }

            RFIDUID7 = default;
            return false;

        }

        #endregion

        #region Clone

        /// <summary>
        /// Clone this 7-byte RFID UID.
        /// </summary>
        public RFIDUID7 Clone

            => new RFIDUID7(
                   new String(InternalId.ToCharArray())
               );

        #endregion


        #region (implicit) => Auth_Token

        /// <summary>
        /// Convert to an Auth_Token.
        /// </summary>
        /// <param name="RFIDUID7">A 7-byte RFID UID.</param>
        public static implicit operator Auth_Token(RFIDUID7 RFIDUID7)

            => new Auth_Token(RFIDUID7.InternalId);

        #endregion


        #region Operator overloading

        #region Operator == (RFIDUID71, RFIDUID72)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="RFIDUID71">A 7-byte RFID UID.</param>
        /// <param name="RFIDUID72">Another 7-byte RFID UID.</param>
        /// <returns>true|false</returns>
        public static Boolean operator == (RFIDUID7 RFIDUID71, RFIDUID7 RFIDUID72)
        {

            // If both are null, or both are same instance, return true.
            if (ReferenceEquals(RFIDUID71, RFIDUID72))
                return true;

            // If one is null, but not both, return false.
            if (((Object) RFIDUID71 == null) || ((Object) RFIDUID72 == null))
                return false;

            return RFIDUID71.Equals(RFIDUID72);

        }

        #endregion

        #region Operator != (RFIDUID71, RFIDUID72)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="RFIDUID71">A 7-byte RFID UID.</param>
        /// <param name="RFIDUID72">Another 7-byte RFID UID.</param>
        /// <returns>true|false</returns>
        public static Boolean operator != (RFIDUID7 RFIDUID71, RFIDUID7 RFIDUID72)
            => !(RFIDUID71 == RFIDUID72);

        #endregion

        #region Operator <  (RFIDUID71, RFIDUID72)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="RFIDUID71">A 7-byte RFID UID.</param>
        /// <param name="RFIDUID72">Another 7-byte RFID UID.</param>
        /// <returns>true|false</returns>
        public static Boolean operator < (RFIDUID7 RFIDUID71, RFIDUID7 RFIDUID72)
        {

            if ((Object) RFIDUID71 == null)
                throw new ArgumentNullException(nameof(RFIDUID71), "The given RFIDUID71 must not be null!");

            return RFIDUID71.CompareTo(RFIDUID72) < 0;

        }

        #endregion

        #region Operator <= (RFIDUID71, RFIDUID72)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="RFIDUID71">A 7-byte RFID UID.</param>
        /// <param name="RFIDUID72">Another 7-byte RFID UID.</param>
        /// <returns>true|false</returns>
        public static Boolean operator <= (RFIDUID7 RFIDUID71, RFIDUID7 RFIDUID72)
            => !(RFIDUID71 > RFIDUID72);

        #endregion

        #region Operator >  (RFIDUID71, RFIDUID72)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="RFIDUID71">A 7-byte RFID UID.</param>
        /// <param name="RFIDUID72">Another 7-byte RFID UID.</param>
        /// <returns>true|false</returns>
        public static Boolean operator > (RFIDUID7 RFIDUID71, RFIDUID7 RFIDUID72)
        {

            if ((Object) RFIDUID71 == null)
                throw new ArgumentNullException(nameof(RFIDUID71), "The given RFIDUID71 must not be null!");

            return RFIDUID71.CompareTo(RFIDUID72) > 0;

        }

        #endregion

        #region Operator >= (RFIDUID71, RFIDUID72)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="RFIDUID71">A 7-byte RFID UID.</param>
        /// <param name="RFIDUID72">Another 7-byte RFID UID.</param>
        /// <returns>true|false</returns>
        public static Boolean operator >= (RFIDUID7 RFIDUID71, RFIDUID7 RFIDUID72)
            => !(RFIDUID71 < RFIDUID72);

        #endregion

        #endregion

        #region IComparable<RFIDUID7> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        public Int32 CompareTo(Object Object)
        {

            if (Object is null)
                throw new ArgumentNullException(nameof(Object), "The given object must not be null!");

            if (!(Object is RFIDUID7 RFIDUID7))
                throw new ArgumentException("The given object is not a 7-byte RFID UID!",
                                            nameof(Object));

            return CompareTo(RFIDUID7);

        }

        #endregion

        #region CompareTo(RFIDUID7)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="RFIDUID7">An object to compare with.</param>
        public Int32 CompareTo(RFIDUID7 RFIDUID7)
        {

            if ((Object) RFIDUID7 == null)
                throw new ArgumentNullException(nameof(RFIDUID7),  "The given 7-byte RFID UID must not be null!");

            return String.Compare(InternalId, RFIDUID7.InternalId, StringComparison.OrdinalIgnoreCase);

        }

        #endregion

        #endregion

        #region IEquatable<RFIDUID7> Members

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

            if (!(Object is RFIDUID7 RFIDUID7))
                return false;

            return Equals(RFIDUID7);

        }

        #endregion

        #region Equals(RFIDUID7)

        /// <summary>
        /// Compares two 7-byte RFID UIDs for equality.
        /// </summary>
        /// <param name="RFIDUID7">A 7-byte RFID UID to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public Boolean Equals(RFIDUID7 RFIDUID7)
        {

            if ((Object) RFIDUID7 == null)
                return false;

            return InternalId.ToLower().Equals(RFIDUID7.InternalId.ToLower());

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
