/*
 * Copyright (c) 2014-2016 GraphDefined GmbH <achim.friedland@graphdefined.com>
 * This file is part of WWCP Core <https://github.com/GraphDefined/WWCP_Core>
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

#endregion

namespace org.GraphDefined.WWCP
{

    /// <summary>
    /// A unique authentication token.
    /// </summary>
    public class Auth_Token : IId, IEquatable<Auth_Token>, IComparable<Auth_Token>
    {

        #region Data

        /// <summary>
        /// The internal value.
        /// </summary>
        protected readonly String _Value;

        #endregion

        #region Properties

        #region New

        /// <summary>
        /// Generate a new token.
        /// </summary>
        public static Auth_Token New
        {
            get
            {
                return new Auth_Token(Guid.NewGuid().ToString());
            }
        }

        #endregion

        #region Length

        /// <summary>
        /// Returns the length of the identificator.
        /// </summary>
        public UInt64 Length
        {
            get
            {
                return (UInt64) _Value.Length;
            }
        }

        #endregion

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Generate a new token based on the given string.
        /// </summary>
        private Auth_Token(String String)
        {
            _Value = String.Trim();
        }

        #endregion


        #region Parse(Text)

        /// <summary>
        /// Parse the given string as an authentication token.
        /// </summary>
        /// <param name="Text">A text representation of an authentication token.</param>
        public static Auth_Token Parse(String Text)
        {
            return new Auth_Token(Text);
        }

        #endregion

        #region TryParse(Text, out Token)

        /// <summary>
        /// Parse the given string as an authentication token.
        /// </summary>
        /// <param name="Text">A text representation of an authentication token.</param>
        /// <param name="Token">The parsed authentication token.</param>
        public static Boolean TryParse(String Text, out Auth_Token Token)
        {
            try
            {
                Token = new Auth_Token(Text);
                return true;
            }
            catch (Exception)
            {
                Token = null;
                return false;
            }
        }

        #endregion

        #region Clone

        /// <summary>
        /// Clone this authentication token.
        /// </summary>
        public Auth_Token Clone
        {
            get
            {
                return new Auth_Token(_Value);
            }
        }

        #endregion


        #region Operator overloading

        #region Operator == (TokenId1, TokenId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="TokenId1">A TokenId.</param>
        /// <param name="TokenId2">Another TokenId.</param>
        /// <returns>true|false</returns>
        public static Boolean operator == (Auth_Token TokenId1, Auth_Token TokenId2)
        {

            // If both are null, or both are same instance, return true.
            if (Object.ReferenceEquals(TokenId1, TokenId2))
                return true;

            // If one is null, but not both, return false.
            if (((Object) TokenId1 == null) || ((Object) TokenId2 == null))
                return false;

            return TokenId1.Equals(TokenId2);

        }

        #endregion

        #region Operator != (TokenId1, TokenId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="TokenId1">A TokenId.</param>
        /// <param name="TokenId2">Another TokenId.</param>
        /// <returns>true|false</returns>
        public static Boolean operator != (Auth_Token TokenId1, Auth_Token TokenId2)
        {
            return !(TokenId1 == TokenId2);
        }

        #endregion

        #region Operator <  (TokenId1, TokenId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="TokenId1">A TokenId.</param>
        /// <param name="TokenId2">Another TokenId.</param>
        /// <returns>true|false</returns>
        public static Boolean operator < (Auth_Token TokenId1, Auth_Token TokenId2)
        {

            if ((Object) TokenId1 == null)
                throw new ArgumentNullException("The given TokenId1 must not be null!");

            return TokenId1.CompareTo(TokenId2) < 0;

        }

        #endregion

        #region Operator <= (TokenId1, TokenId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="TokenId1">A TokenId.</param>
        /// <param name="TokenId2">Another TokenId.</param>
        /// <returns>true|false</returns>
        public static Boolean operator <= (Auth_Token TokenId1, Auth_Token TokenId2)
        {
            return !(TokenId1 > TokenId2);
        }

        #endregion

        #region Operator >  (TokenId1, TokenId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="TokenId1">A TokenId.</param>
        /// <param name="TokenId2">Another TokenId.</param>
        /// <returns>true|false</returns>
        public static Boolean operator > (Auth_Token TokenId1, Auth_Token TokenId2)
        {

            if ((Object) TokenId1 == null)
                throw new ArgumentNullException("The given TokenId1 must not be null!");

            return TokenId1.CompareTo(TokenId2) > 0;

        }

        #endregion

        #region Operator >= (TokenId1, TokenId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="TokenId1">A TokenId.</param>
        /// <param name="TokenId2">Another TokenId.</param>
        /// <returns>true|false</returns>
        public static Boolean operator >= (Auth_Token TokenId1, Auth_Token TokenId2)
        {
            return !(TokenId1 < TokenId2);
        }

        #endregion

        #endregion

        #region IComparable<Auth_Token> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        public Int32 CompareTo(Object Object)
        {

            if (Object == null)
                throw new ArgumentNullException("The given object must not be null!");

            // Check if the given object is an TokenId.
            var Token = Object as Auth_Token;
            if ((Object) Token == null)
                throw new ArgumentException("The given object is not an authentication token!");

            return CompareTo(Token);

        }

        #endregion

        #region CompareTo(Token)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Token">An object to compare with.</param>
        public Int32 CompareTo(Auth_Token Token)
        {

            if ((Object) Token == null)
                throw new ArgumentNullException("The given AuthToken must not be null!");

            return _Value.CompareTo(Token._Value);

        }

        #endregion

        #endregion

        #region IEquatable<Auth_Token> Members

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

            // Check if the given object is an Token.
            var Token = Object as Auth_Token;
            if ((Object) Token == null)
                return false;

            return this.Equals(Token);

        }

        #endregion

        #region Equals(Token)

        /// <summary>
        /// Compares two TokenIds for equality.
        /// </summary>
        /// <param name="Token">A TokenId to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public Boolean Equals(Auth_Token Token)
        {

            if ((Object) Token == null)
                return false;

            return _Value.Equals(Token._Value);

        }

        #endregion

        #endregion

        #region GetHashCode()

        /// <summary>
        /// Return the HashCode of this object.
        /// </summary>
        /// <returns>The HashCode of this object.</returns>
        public override Int32 GetHashCode()
        {
            return _Value.GetHashCode();
        }

        #endregion

        #region (override) ToString()

        /// <summary>
        /// Return a string representation of this object.
        /// </summary>
        public override String ToString()
        {
            return _Value.ToString();
        }

        #endregion

    }

}
