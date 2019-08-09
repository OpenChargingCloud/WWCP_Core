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
    /// A unique authentication token.
    /// </summary>
    public struct Auth_Token : IId,
                               IEquatable<Auth_Token>,
                               IComparable<Auth_Token>
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
        /// Indicates whether this identification is null or empty.
        /// </summary>
        public Boolean IsNullOrEmpty
            => InternalId.IsNullOrEmpty();

        /// <summary>
        /// The length of the service session identificator.
        /// </summary>
        public UInt64 Length
            => (UInt64) InternalId.Length;

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new authentication token based on the given string.
        /// </summary>
        /// <param name="Text">The text representation of an authentication token.</param>
        internal Auth_Token(String Text)
        {
            InternalId = Text;
        }

        #endregion


        #region (static) Random(Length = 20)

        public static Auth_Token Random(Byte Length = 20)
            => new Auth_Token(_Random.RandomString(Length));

        #endregion


        #region (static) Parse   (Text)

        /// <summary>
        /// Parse the given string as an authentication token.
        /// </summary>
        /// <param name="Text">A text representation of an authentication token.</param>
        public static Auth_Token Parse(String Text)
        {

            #region Initial checks

            if (Text != null)
                Text = Text.Trim();

            if (Text.IsNullOrEmpty())
                throw new ArgumentNullException(nameof(Text), "The given text representation of an authentication token must not be null or empty!");

            #endregion

            if (TryParse(Text, out Auth_Token AuthToken))
                return AuthToken;

            throw new ArgumentNullException(nameof(Text), "The given text representation of an authentication token is invalid!");

        }

        #endregion

        #region (static) TryParse(Text)

        /// <summary>
        /// Try to parse the given string as an authentication token.
        /// </summary>
        /// <param name="Text">A text representation of an authentication token.</param>
        public static Auth_Token? TryParse(String Text)
        {

            if (TryParse(Text, out Auth_Token AuthToken))
                return AuthToken;

            return new Auth_Token?();

        }

        #endregion

        #region (static) TryParse(Text, out AuthToken)

        /// <summary>
        /// Try to parse the given string as an authentication token.
        /// </summary>
        /// <param name="Text">A text representation of an authentication token.</param>
        /// <param name="AuthToken">The parsed authentication token.</param>
        public static Boolean TryParse(String Text, out Auth_Token AuthToken)
        {

            #region Initial checks

            if (Text != null)
                Text = Text.Trim();

            if (Text.IsNullOrEmpty())
            {
                AuthToken = default;
                return false;
            }

            #endregion

            try
            {
                AuthToken = new Auth_Token(Text);
                return true;
            }
            catch (Exception)
            { }

            AuthToken = default;
            return false;

        }

        #endregion

        #region Clone

        /// <summary>
        /// Clone this authentication token.
        /// </summary>
        public Auth_Token Clone

            => new Auth_Token(
                   new String(InternalId.ToCharArray())
               );

        #endregion


        #region Operator overloading

        #region Operator == (AuthToken1, AuthToken2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="AuthToken1">An authentication token.</param>
        /// <param name="AuthToken2">Another authentication token.</param>
        /// <returns>true|false</returns>
        public static Boolean operator == (Auth_Token AuthToken1, Auth_Token AuthToken2)
        {

            // If both are null, or both are same instance, return true.
            if (Object.ReferenceEquals(AuthToken1, AuthToken2))
                return true;

            // If one is null, but not both, return false.
            if (((Object) AuthToken1 == null) || ((Object) AuthToken2 == null))
                return false;

            return AuthToken1.Equals(AuthToken2);

        }

        #endregion

        #region Operator != (AuthToken1, AuthToken2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="AuthToken1">An authentication token.</param>
        /// <param name="AuthToken2">Another authentication token.</param>
        /// <returns>true|false</returns>
        public static Boolean operator != (Auth_Token AuthToken1, Auth_Token AuthToken2)
            => !(AuthToken1 == AuthToken2);

        #endregion

        #region Operator <  (AuthToken1, AuthToken2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="AuthToken1">An authentication token.</param>
        /// <param name="AuthToken2">Another authentication token.</param>
        /// <returns>true|false</returns>
        public static Boolean operator < (Auth_Token AuthToken1, Auth_Token AuthToken2)
        {

            if ((Object) AuthToken1 == null)
                throw new ArgumentNullException(nameof(AuthToken1), "The given AuthToken1 must not be null!");

            return AuthToken1.CompareTo(AuthToken2) < 0;

        }

        #endregion

        #region Operator <= (AuthToken1, AuthToken2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="AuthToken1">An authentication token.</param>
        /// <param name="AuthToken2">Another authentication token.</param>
        /// <returns>true|false</returns>
        public static Boolean operator <= (Auth_Token AuthToken1, Auth_Token AuthToken2)
            => !(AuthToken1 > AuthToken2);

        #endregion

        #region Operator >  (AuthToken1, AuthToken2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="AuthToken1">An authentication token.</param>
        /// <param name="AuthToken2">Another authentication token.</param>
        /// <returns>true|false</returns>
        public static Boolean operator > (Auth_Token AuthToken1, Auth_Token AuthToken2)
        {

            if ((Object) AuthToken1 == null)
                throw new ArgumentNullException(nameof(AuthToken1), "The given AuthToken1 must not be null!");

            return AuthToken1.CompareTo(AuthToken2) > 0;

        }

        #endregion

        #region Operator >= (AuthToken1, AuthToken2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="AuthToken1">An authentication token.</param>
        /// <param name="AuthToken2">Another authentication token.</param>
        /// <returns>true|false</returns>
        public static Boolean operator >= (Auth_Token AuthToken1, Auth_Token AuthToken2)
            => !(AuthToken1 < AuthToken2);

        #endregion

        #endregion

        #region IComparable<AuthToken> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        public Int32 CompareTo(Object Object)
        {

            if (Object is null)
                throw new ArgumentNullException(nameof(Object), "The given object must not be null!");

            if (!(Object is Auth_Token AuthToken))
                throw new ArgumentException("The given object is not an authentication token!",
                                            nameof(Object));

            return CompareTo(AuthToken);

        }

        #endregion

        #region CompareTo(AuthToken)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="AuthToken">An object to compare with.</param>
        public Int32 CompareTo(Auth_Token AuthToken)
        {

            if ((Object) AuthToken == null)
                throw new ArgumentNullException(nameof(AuthToken),  "The given authentication token must not be null!");

            return String.Compare(InternalId, AuthToken.InternalId, StringComparison.OrdinalIgnoreCase);

        }

        #endregion

        #endregion

        #region IEquatable<AuthToken> Members

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

            if (!(Object is Auth_Token AuthToken))
                return false;

            return Equals(AuthToken);

        }

        #endregion

        #region Equals(AuthToken)

        /// <summary>
        /// Compares two AuthTokens for equality.
        /// </summary>
        /// <param name="AuthToken">An authentication token to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public Boolean Equals(Auth_Token AuthToken)
        {

            if ((Object) AuthToken == null)
                return false;

            return InternalId.ToLower().Equals(AuthToken.InternalId.ToLower());

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
