/*
 * Copyright (c) 2014-2019 GraphDefined GmbH <achim.friedland@graphdefined.com>
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
    /// A 5-byte RFID UID.
    /// </summary>
    public struct RFIDUID5
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
        /// Generate a new 5-byte RFID UID based on the given string.
        /// </summary>
        /// <param name="Text">The text representation of the 5-byte RFID UID.</param>
        private RFIDUID5(String Text)
        {
            InternalId = Text;
        }

        #endregion


        #region (static) Random

        public static RFIDUID5 Random
            => new RFIDUID5(_Random.RandomString(10));

        #endregion


        #region (static) Parse   (Text)

        /// <summary>
        /// Parse the given string as a 5-byte RFID UID.
        /// </summary>
        /// <param name="Text">A text representation of a 5-byte RFID UID.</param>
        public static RFIDUID5 Parse(String Text)
        {

            #region Initial checks

            if (Text != null)
                Text = Text.Trim();

            if (Text.IsNullOrEmpty())
                throw new ArgumentNullException(nameof(Text), "The given text representation of a 5-byte RFID UID must not be null or empty!");

            #endregion

            if (TryParse(Text, out RFIDUID5 RFIDUID5))
                return RFIDUID5;

            throw new ArgumentNullException(nameof(Text), "The given text representation of a 5-byte RFID UID is invalid!");

        }

        #endregion

        #region (static) TryParse(Text)

        /// <summary>
        /// Try to parse the given string as a 5-byte RFID UID.
        /// </summary>
        /// <param name="Text">A text representation of a 5-byte RFID UID.</param>
        public static RFIDUID5? TryParse(String Text)
        {

            if (TryParse(Text, out RFIDUID5 RFIDUID5))
                return RFIDUID5;

            return new RFIDUID5?();

        }

        #endregion

        #region (static) TryParse(Text, out RFIDUID5)

        /// <summary>
        /// Try to parse the given string as a 5-byte RFID UID.
        /// </summary>
        /// <param name="Text">A text representation of a 5-byte RFID UID.</param>
        /// <param name="RFIDUID5">The parsed 5-byte RFID UID.</param>
        public static Boolean TryParse(String Text, out RFIDUID5 RFIDUID5)
        {

            #region Initial checks

            if (Text != null)
                Text = Text.Trim();

            if (Text.IsNullOrEmpty())
            {
                RFIDUID5 = default;
                return false;
            }

            #endregion

            try
            {
                RFIDUID5 = new RFIDUID5(Text);
                return true;
            }
            catch (Exception)
            { }

            RFIDUID5 = default;
            return false;

        }

        #endregion

        #region Clone

        /// <summary>
        /// Clone this 5-byte RFID UID.
        /// </summary>
        public RFIDUID5 Clone

            => new RFIDUID5(
                   new String(InternalId.ToCharArray())
               );

        #endregion


        #region (implicit) => Auth_Token

        /// <summary>
        /// Convert to an Auth_Token.
        /// </summary>
        /// <param name="RFIDUID5">A 5-byte RFID UID.</param>
        public static implicit operator Auth_Token(RFIDUID5 RFIDUID5)

            => new Auth_Token(RFIDUID5.InternalId);

        #endregion


        #region Operator overloading

        #region Provider == (RFIDUID51, RFIDUID52)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="RFIDUID51">A 5-byte RFID UID.</param>
        /// <param name="RFIDUID52">Another 5-byte RFID UID.</param>
        /// <returns>true|false</returns>
        public static Boolean operator == (RFIDUID5 RFIDUID51, RFIDUID5 RFIDUID52)
        {

            // If both are null, or both are same instance, return true.
            if (Object.ReferenceEquals(RFIDUID51, RFIDUID52))
                return true;

            // If one is null, but not both, return false.
            if (((Object) RFIDUID51 == null) || ((Object) RFIDUID52 == null))
                return false;

            return RFIDUID51.Equals(RFIDUID52);

        }

        #endregion

        #region Provider != (RFIDUID51, RFIDUID52)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="RFIDUID51">A 5-byte RFID UID.</param>
        /// <param name="RFIDUID52">Another 5-byte RFID UID.</param>
        /// <returns>true|false</returns>
        public static Boolean operator != (RFIDUID5 RFIDUID51, RFIDUID5 RFIDUID52)
            => !(RFIDUID51 == RFIDUID52);

        #endregion

        #region Provider <  (RFIDUID51, RFIDUID52)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="RFIDUID51">A 5-byte RFID UID.</param>
        /// <param name="RFIDUID52">Another 5-byte RFID UID.</param>
        /// <returns>true|false</returns>
        public static Boolean operator < (RFIDUID5 RFIDUID51, RFIDUID5 RFIDUID52)
        {

            if ((Object) RFIDUID51 == null)
                throw new ArgumentNullException(nameof(RFIDUID51), "The given RFIDUID51 must not be null!");

            return RFIDUID51.CompareTo(RFIDUID52) < 0;

        }

        #endregion

        #region Provider <= (RFIDUID51, RFIDUID52)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="RFIDUID51">A 5-byte RFID UID.</param>
        /// <param name="RFIDUID52">Another 5-byte RFID UID.</param>
        /// <returns>true|false</returns>
        public static Boolean operator <= (RFIDUID5 RFIDUID51, RFIDUID5 RFIDUID52)
            => !(RFIDUID51 > RFIDUID52);

        #endregion

        #region Provider >  (RFIDUID51, RFIDUID52)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="RFIDUID51">A 5-byte RFID UID.</param>
        /// <param name="RFIDUID52">Another 5-byte RFID UID.</param>
        /// <returns>true|false</returns>
        public static Boolean operator > (RFIDUID5 RFIDUID51, RFIDUID5 RFIDUID52)
        {

            if ((Object) RFIDUID51 == null)
                throw new ArgumentNullException(nameof(RFIDUID51), "The given RFIDUID51 must not be null!");

            return RFIDUID51.CompareTo(RFIDUID52) > 0;

        }

        #endregion

        #region Provider >= (RFIDUID51, RFIDUID52)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="RFIDUID51">A 5-byte RFID UID.</param>
        /// <param name="RFIDUID52">Another 5-byte RFID UID.</param>
        /// <returns>true|false</returns>
        public static Boolean operator >= (RFIDUID5 RFIDUID51, RFIDUID5 RFIDUID52)
            => !(RFIDUID51 < RFIDUID52);

        #endregion

        #endregion

        #region IComparable<RFIDUID5> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        public Int32 CompareTo(Object Object)
        {

            if (Object is null)
                throw new ArgumentNullException(nameof(Object), "The given object must not be null!");

            if (!(Object is RFIDUID5 RFIDUID5))
                throw new ArgumentException("The given object is not a 5-byte RFID UID!",
                                            nameof(Object));

            return CompareTo(RFIDUID5);

        }

        #endregion

        #region CompareTo(RFIDUID5)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="RFIDUID5">An object to compare with.</param>
        public Int32 CompareTo(RFIDUID5 RFIDUID5)
        {

            if ((Object) RFIDUID5 == null)
                throw new ArgumentNullException(nameof(RFIDUID5),  "The given 5-byte RFID UID must not be null!");

            return String.Compare(InternalId, RFIDUID5.InternalId, StringComparison.OrdinalIgnoreCase);

        }

        #endregion

        #endregion

        #region IEquatable<RFIDUID5> Members

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

            if (!(Object is RFIDUID5 RFIDUID5))
                return false;

            return Equals(RFIDUID5);

        }

        #endregion

        #region Equals(RFIDUID5)

        /// <summary>
        /// Compares two 5-byte RFID UIDs for equality.
        /// </summary>
        /// <param name="RFIDUID5">A 5-byte RFID UID to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public Boolean Equals(RFIDUID5 RFIDUID5)
        {

            if ((Object) RFIDUID5 == null)
                return false;

            return InternalId.ToLower().Equals(RFIDUID5.InternalId.ToLower());

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
