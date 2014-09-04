/*
 * Copyright (c) 2014 Achim Friedland <achim.friedland@graphdefined.com>
 * This file is part of eMI3 Core <http://www.github.com/GraphDefined/eMI3>
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *     http://www.apache.org/licenses/LICENSE-2.0
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

namespace com.graphdefined.eMI3
{

    /// <summary>
    /// The unique identification of a token.
    /// </summary>
    public class Token : IId, IEquatable<Token>, IComparable<Token>
    {

        #region Data

        /// <summary>
        /// The internal identification.
        /// </summary>
        protected readonly String _Id;

        #endregion

        #region Properties

        #region Length

        /// <summary>
        /// Returns the length of the identificator.
        /// </summary>
        public UInt64 Length
        {
            get
            {
                return (UInt64) _Id.Length;
            }
        }

        #endregion

        #endregion

        #region Constructor(s)

        #region TokenId()

        /// <summary>
        /// Generate a new token.
        /// </summary>
        public Token()
        {
            _Id = Guid.NewGuid().ToString();
        }

        #endregion

        #region TokenId(String)

        /// <summary>
        /// Generate a new token based on the given string.
        /// </summary>
        public Token(String String)
        {
            _Id = String.Trim();
        }

        #endregion

        #endregion


        #region New

        /// <summary>
        /// Generate a new token.
        /// </summary>
        public static Token New
        {
            get
            {
                return new Token(Guid.NewGuid().ToString());
            }
        }

        #endregion

        #region Parse(EVSEOperatorId)

        /// <summary>
        /// Parse the given string as a token.
        /// </summary>
        public static Token Parse(String EVSEOperatorId)
        {
            return new Token(EVSEOperatorId);
        }

        #endregion

        #region TryParse(Text, out EVSEOperatorId)

        /// <summary>
        /// Parse the given string as a token.
        /// </summary>
        public static Boolean TryParse(String Text, out Token EVSEOperatorId)
        {
            try
            {
                EVSEOperatorId = new Token(Text);
                return true;
            }
            catch (Exception e)
            {
                EVSEOperatorId = null;
                return false;
            }
        }

        #endregion

        #region Clone

        /// <summary>
        /// Clone a token.
        /// </summary>
        public Token Clone
        {
            get
            {
                return new Token(_Id);
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
        public static Boolean operator == (Token TokenId1, Token TokenId2)
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
        public static Boolean operator != (Token TokenId1, Token TokenId2)
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
        public static Boolean operator < (Token TokenId1, Token TokenId2)
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
        public static Boolean operator <= (Token TokenId1, Token TokenId2)
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
        public static Boolean operator > (Token TokenId1, Token TokenId2)
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
        public static Boolean operator >= (Token TokenId1, Token TokenId2)
        {
            return !(TokenId1 < TokenId2);
        }

        #endregion

        #endregion

        #region IComparable<TokenId> Members

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
            var TokenId = Object as Token;
            if ((Object) TokenId == null)
                throw new ArgumentException("The given object is not a TokenId!");

            return CompareTo(TokenId);

        }

        #endregion

        #region CompareTo(TokenId)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="TokenId">An object to compare with.</param>
        public Int32 CompareTo(Token TokenId)
        {

            if ((Object) TokenId == null)
                throw new ArgumentNullException("The given TokenId must not be null!");

            // Compare the length of the TokenIds
            var _Result = this.Length.CompareTo(TokenId.Length);

            // If equal: Compare Ids
            if (_Result == 0)
                _Result = _Id.CompareTo(TokenId._Id);

            return _Result;

        }

        #endregion

        #endregion

        #region IEquatable<TokenId> Members

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

            // Check if the given object is an TokenId.
            var TokenId = Object as Token;
            if ((Object) TokenId == null)
                return false;

            return this.Equals(TokenId);

        }

        #endregion

        #region Equals(TokenId)

        /// <summary>
        /// Compares two TokenIds for equality.
        /// </summary>
        /// <param name="TokenId">A TokenId to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public Boolean Equals(Token TokenId)
        {

            if ((Object) TokenId == null)
                return false;

            return _Id.Equals(TokenId._Id);

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
            return _Id.GetHashCode();
        }

        #endregion

        #region ToString()

        /// <summary>
        /// Return a string represtentation of this object.
        /// </summary>
        public override String ToString()
        {
            return _Id.ToString();
        }

        #endregion

    }

}
