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

using org.GraphDefined.Vanaheimr.Illias;

#endregion

namespace cloud.charging.open.protocols.WWCP
{

    /// <summary>
    /// A 4-byte RFID UID.
    /// </summary>
    public struct RFIDUID4
    {

        #region Data

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
        /// Generate a new 4-byte RFID UID based on the given string.
        /// </summary>
        /// <param name="Text">The text representation of the 4-byte RFID UID.</param>
        private RFIDUID4(String Text)
        {
            InternalId = Text;
        }

        #endregion


        #region (static) Random

        /// <summary>
        /// Return a random 4-byte RFID UID.
        /// </summary>
        public static RFIDUID4 Random

            => new (RandomExtensions.RandomHexString(8));

        #endregion


        #region (static) Parse   (Text)

        /// <summary>
        /// Parse the given string as a 4-byte RFID UID.
        /// </summary>
        /// <param name="Text">A text representation of a 4-byte RFID UID.</param>
        public static RFIDUID4 Parse(String Text)
        {

            #region Initial checks

            if (Text != null)
                Text = Text.Trim();

            if (Text.IsNullOrEmpty())
                throw new ArgumentNullException(nameof(Text), "The given text representation of a 4-byte RFID UID must not be null or empty!");

            #endregion

            if (TryParse(Text, out RFIDUID4 RFIDUID4))
                return RFIDUID4;

            throw new ArgumentNullException(nameof(Text), "The given text representation of a 4-byte RFID UID is invalid!");

        }

        #endregion

        #region (static) TryParse(Text)

        /// <summary>
        /// Try to parse the given string as a 4-byte RFID UID.
        /// </summary>
        /// <param name="Text">A text representation of a 4-byte RFID UID.</param>
        public static RFIDUID4? TryParse(String Text)
        {

            if (TryParse(Text, out RFIDUID4 RFIDUID4))
                return RFIDUID4;

            return null;

        }

        #endregion

        #region (static) TryParse(Text, out RFIDUID4)

        /// <summary>
        /// Try to parse the given string as a 4-byte RFID UID.
        /// </summary>
        /// <param name="Text">A text representation of a 4-byte RFID UID.</param>
        /// <param name="RFIDUID4">The parsed 4-byte RFID UID.</param>
        public static Boolean TryParse(String Text, out RFIDUID4 RFIDUID4)
        {

            #region Initial checks

            if (Text != null)
                Text = Text.Trim();

            if (Text.IsNullOrEmpty())
            {
                RFIDUID4 = default;
                return false;
            }

            #endregion

            try
            {
                RFIDUID4 = new RFIDUID4(Text);
                return true;
            }
            catch (Exception)
            { }

            RFIDUID4 = default;
            return false;

        }

        #endregion

        #region Clone

        /// <summary>
        /// Clone this 4-byte RFID UID.
        /// </summary>
        public RFIDUID4 Clone

            => new (
                   new String(InternalId?.ToCharArray())
               );

        #endregion


        #region (implicit) => Auth_Token

        /// <summary>
        /// Convert to an Auth_Token.
        /// </summary>
        /// <param name="RFIDUID4">A 4-byte RFID UID.</param>
        public static implicit operator Auth_Token(RFIDUID4 RFIDUID4)

            => new Auth_Token(RFIDUID4.InternalId);

        #endregion


        #region Operator overloading

        #region Operator == (RFIDUID41, RFIDUID42)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="RFIDUID41">A 4-byte RFID UID.</param>
        /// <param name="RFIDUID42">Another 4-byte RFID UID.</param>
        /// <returns>true|false</returns>
        public static Boolean operator == (RFIDUID4 RFIDUID41, RFIDUID4 RFIDUID42)
        {

            // If both are null, or both are same instance, return true.
            if (ReferenceEquals(RFIDUID41, RFIDUID42))
                return true;

            // If one is null, but not both, return false.
            if (((Object) RFIDUID41 == null) || ((Object) RFIDUID42 == null))
                return false;

            return RFIDUID41.Equals(RFIDUID42);

        }

        #endregion

        #region Operator != (RFIDUID41, RFIDUID42)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="RFIDUID41">A 4-byte RFID UID.</param>
        /// <param name="RFIDUID42">Another 4-byte RFID UID.</param>
        /// <returns>true|false</returns>
        public static Boolean operator != (RFIDUID4 RFIDUID41, RFIDUID4 RFIDUID42)
            => !(RFIDUID41 == RFIDUID42);

        #endregion

        #region Operator <  (RFIDUID41, RFIDUID42)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="RFIDUID41">A 4-byte RFID UID.</param>
        /// <param name="RFIDUID42">Another 4-byte RFID UID.</param>
        /// <returns>true|false</returns>
        public static Boolean operator < (RFIDUID4 RFIDUID41, RFIDUID4 RFIDUID42)
        {

            if ((Object) RFIDUID41 == null)
                throw new ArgumentNullException(nameof(RFIDUID41), "The given RFIDUID41 must not be null!");

            return RFIDUID41.CompareTo(RFIDUID42) < 0;

        }

        #endregion

        #region Operator <= (RFIDUID41, RFIDUID42)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="RFIDUID41">A 4-byte RFID UID.</param>
        /// <param name="RFIDUID42">Another 4-byte RFID UID.</param>
        /// <returns>true|false</returns>
        public static Boolean operator <= (RFIDUID4 RFIDUID41, RFIDUID4 RFIDUID42)
            => !(RFIDUID41 > RFIDUID42);

        #endregion

        #region Operator >  (RFIDUID41, RFIDUID42)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="RFIDUID41">A 4-byte RFID UID.</param>
        /// <param name="RFIDUID42">Another 4-byte RFID UID.</param>
        /// <returns>true|false</returns>
        public static Boolean operator > (RFIDUID4 RFIDUID41, RFIDUID4 RFIDUID42)
        {

            if ((Object) RFIDUID41 == null)
                throw new ArgumentNullException(nameof(RFIDUID41), "The given RFIDUID41 must not be null!");

            return RFIDUID41.CompareTo(RFIDUID42) > 0;

        }

        #endregion

        #region Operator >= (RFIDUID41, RFIDUID42)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="RFIDUID41">A 4-byte RFID UID.</param>
        /// <param name="RFIDUID42">Another 4-byte RFID UID.</param>
        /// <returns>true|false</returns>
        public static Boolean operator >= (RFIDUID4 RFIDUID41, RFIDUID4 RFIDUID42)
            => !(RFIDUID41 < RFIDUID42);

        #endregion

        #endregion

        #region IComparable<RFIDUID4> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        public Int32 CompareTo(Object Object)
        {

            if (Object is null)
                throw new ArgumentNullException(nameof(Object), "The given object must not be null!");

            if (!(Object is RFIDUID4 RFIDUID4))
                throw new ArgumentException("The given object is not a 4-byte RFID UID!",
                                            nameof(Object));

            return CompareTo(RFIDUID4);

        }

        #endregion

        #region CompareTo(RFIDUID4)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="RFIDUID4">An object to compare with.</param>
        public Int32 CompareTo(RFIDUID4 RFIDUID4)
        {

            if ((Object) RFIDUID4 == null)
                throw new ArgumentNullException(nameof(RFIDUID4),  "The given 4-byte RFID UID must not be null!");

            return String.Compare(InternalId, RFIDUID4.InternalId, StringComparison.OrdinalIgnoreCase);

        }

        #endregion

        #endregion

        #region IEquatable<RFIDUID4> Members

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

            if (!(Object is RFIDUID4 RFIDUID4))
                return false;

            return Equals(RFIDUID4);

        }

        #endregion

        #region Equals(RFIDUID4)

        /// <summary>
        /// Compares two 4-byte RFID UIDs for equality.
        /// </summary>
        /// <param name="RFIDUID4">A 4-byte RFID UID to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public Boolean Equals(RFIDUID4 RFIDUID4)
        {

            if ((Object) RFIDUID4 == null)
                return false;

            return InternalId.ToLower().Equals(RFIDUID4.InternalId.ToLower());

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
