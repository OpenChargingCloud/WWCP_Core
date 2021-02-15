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
    /// A 10-byte RFID UID.
    /// </summary>
    public struct RFIDUID10
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
        /// Generate a new 10-byte RFID UID based on the given string.
        /// </summary>
        /// <param name="Text">The text representation of the 10-byte RFID UID.</param>
        private RFIDUID10(String Text)
        {
            InternalId = Text;
        }

        #endregion


        #region (static) Random

        /// <summary>
        /// Return a random 10-byte RFID UID.
        /// </summary>
        public static RFIDUID10 Random
            => new RFIDUID10(_Random.RandomHexString(20));

        #endregion


        #region (static) Parse   (Text)

        /// <summary>
        /// Parse the given string as a 10-byte RFID UID.
        /// </summary>
        /// <param name="Text">A text representation of a 10-byte RFID UID.</param>
        public static RFIDUID10 Parse(String Text)
        {

            #region Initial checks

            if (Text != null)
                Text = Text.Trim();

            if (Text.IsNullOrEmpty())
                throw new ArgumentNullException(nameof(Text), "The given text representation of a 10-byte RFID UID must not be null or empty!");

            #endregion

            if (TryParse(Text, out RFIDUID10 RFIDUID10))
                return RFIDUID10;

            throw new ArgumentNullException(nameof(Text), "The given text representation of a 10-byte RFID UID is invalid!");

        }

        #endregion

        #region (static) TryParse(Text)

        /// <summary>
        /// Try to parse the given string as a 10-byte RFID UID.
        /// </summary>
        /// <param name="Text">A text representation of a 10-byte RFID UID.</param>
        public static RFIDUID10? TryParse(String Text)
        {

            if (TryParse(Text, out RFIDUID10 RFIDUID10))
                return RFIDUID10;

            return null;

        }

        #endregion

        #region (static) TryParse(Text, out RFIDUID10)

        /// <summary>
        /// Try to parse the given string as a 10-byte RFID UID.
        /// </summary>
        /// <param name="Text">A text representation of a 10-byte RFID UID.</param>
        /// <param name="RFIDUID10">The parsed 10-byte RFID UID.</param>
        public static Boolean TryParse(String Text, out RFIDUID10 RFIDUID10)
        {

            #region Initial checks

            if (Text != null)
                Text = Text.Trim();

            if (Text.IsNullOrEmpty())
            {
                RFIDUID10 = default;
                return false;
            }

            #endregion

            try
            {
                RFIDUID10 = new RFIDUID10(Text);
                return true;
            }
            catch (Exception)
            { }

            RFIDUID10 = default;
            return false;

        }

        #endregion

        #region Clone

        /// <summary>
        /// Clone this 10-byte RFID UID.
        /// </summary>
        public RFIDUID10 Clone

            => new RFIDUID10(
                   new String(InternalId.ToCharArray())
               );

        #endregion


        #region (implicit) => Auth_Token

        /// <summary>
        /// Convert to an Auth_Token.
        /// </summary>
        /// <param name="RFIDUID10">A 10-byte RFID UID.</param>
        public static implicit operator Auth_Token(RFIDUID10 RFIDUID10)

            => new Auth_Token(RFIDUID10.InternalId);

        #endregion


        #region Operator overloading

        #region Operator == (RFIDUID101, RFIDUID102)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="RFIDUID101">A 10-byte RFID UID.</param>
        /// <param name="RFIDUID102">Another 10-byte RFID UID.</param>
        /// <returns>true|false</returns>
        public static Boolean operator == (RFIDUID10 RFIDUID101, RFIDUID10 RFIDUID102)
        {

            // If both are null, or both are same instance, return true.
            if (ReferenceEquals(RFIDUID101, RFIDUID102))
                return true;

            // If one is null, but not both, return false.
            if (((Object) RFIDUID101 == null) || ((Object) RFIDUID102 == null))
                return false;

            return RFIDUID101.Equals(RFIDUID102);

        }

        #endregion

        #region Operator != (RFIDUID101, RFIDUID102)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="RFIDUID101">A 10-byte RFID UID.</param>
        /// <param name="RFIDUID102">Another 10-byte RFID UID.</param>
        /// <returns>true|false</returns>
        public static Boolean operator != (RFIDUID10 RFIDUID101, RFIDUID10 RFIDUID102)
            => !(RFIDUID101 == RFIDUID102);

        #endregion

        #region Operator <  (RFIDUID101, RFIDUID102)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="RFIDUID101">A 10-byte RFID UID.</param>
        /// <param name="RFIDUID102">Another 10-byte RFID UID.</param>
        /// <returns>true|false</returns>
        public static Boolean operator < (RFIDUID10 RFIDUID101, RFIDUID10 RFIDUID102)
        {

            if ((Object) RFIDUID101 == null)
                throw new ArgumentNullException(nameof(RFIDUID101), "The given RFIDUID101 must not be null!");

            return RFIDUID101.CompareTo(RFIDUID102) < 0;

        }

        #endregion

        #region Operator <= (RFIDUID101, RFIDUID102)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="RFIDUID101">A 10-byte RFID UID.</param>
        /// <param name="RFIDUID102">Another 10-byte RFID UID.</param>
        /// <returns>true|false</returns>
        public static Boolean operator <= (RFIDUID10 RFIDUID101, RFIDUID10 RFIDUID102)
            => !(RFIDUID101 > RFIDUID102);

        #endregion

        #region Operator >  (RFIDUID101, RFIDUID102)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="RFIDUID101">A 10-byte RFID UID.</param>
        /// <param name="RFIDUID102">Another 10-byte RFID UID.</param>
        /// <returns>true|false</returns>
        public static Boolean operator > (RFIDUID10 RFIDUID101, RFIDUID10 RFIDUID102)
        {

            if ((Object) RFIDUID101 == null)
                throw new ArgumentNullException(nameof(RFIDUID101), "The given RFIDUID101 must not be null!");

            return RFIDUID101.CompareTo(RFIDUID102) > 0;

        }

        #endregion

        #region Operator >= (RFIDUID101, RFIDUID102)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="RFIDUID101">A 10-byte RFID UID.</param>
        /// <param name="RFIDUID102">Another 10-byte RFID UID.</param>
        /// <returns>true|false</returns>
        public static Boolean operator >= (RFIDUID10 RFIDUID101, RFIDUID10 RFIDUID102)
            => !(RFIDUID101 < RFIDUID102);

        #endregion

        #endregion

        #region IComparable<RFIDUID10> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        public Int32 CompareTo(Object Object)
        {

            if (Object is null)
                throw new ArgumentNullException(nameof(Object), "The given object must not be null!");

            if (!(Object is RFIDUID10 RFIDUID10))
                throw new ArgumentException("The given object is not a 10-byte RFID UID!",
                                            nameof(Object));

            return CompareTo(RFIDUID10);

        }

        #endregion

        #region CompareTo(RFIDUID10)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="RFIDUID10">An object to compare with.</param>
        public Int32 CompareTo(RFIDUID10 RFIDUID10)
        {

            if ((Object) RFIDUID10 == null)
                throw new ArgumentNullException(nameof(RFIDUID10),  "The given 10-byte RFID UID must not be null!");

            return String.Compare(InternalId, RFIDUID10.InternalId, StringComparison.OrdinalIgnoreCase);

        }

        #endregion

        #endregion

        #region IEquatable<RFIDUID10> Members

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

            if (!(Object is RFIDUID10 RFIDUID10))
                return false;

            return Equals(RFIDUID10);

        }

        #endregion

        #region Equals(RFIDUID10)

        /// <summary>
        /// Compares two 10-byte RFID UIDs for equality.
        /// </summary>
        /// <param name="RFIDUID10">A 10-byte RFID UID to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public Boolean Equals(RFIDUID10 RFIDUID10)
        {

            if ((Object) RFIDUID10 == null)
                return false;

            return InternalId.ToLower().Equals(RFIDUID10.InternalId.ToLower());

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
