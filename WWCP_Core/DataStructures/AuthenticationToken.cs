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

using System.Text.RegularExpressions;

using org.GraphDefined.Vanaheimr.Illias;

#endregion

namespace cloud.charging.open.protocols.WWCP
{

    /// <summary>
    /// A unique authentication token.
    /// </summary>
    public readonly struct AuthenticationToken : IId,
                                                 IEquatable<AuthenticationToken>,
                                                 IComparable<AuthenticationToken>
    {

        #region Data

        /// <summary>
        /// The internal identification.
        /// </summary>
        private readonly String InternalId;


        /// <summary>
        /// The regular expression for parsing an authentication token.
        /// </summary>
        public static readonly Regex AuthenticationToken_RegEx = new ("^([a-fA-F0-9]{8})$ | ^([a-fA-F0-9]{14})$ | ^([a-fA-F0-9]{20})$",
                                                                      RegexOptions.IgnorePatternWhitespace);

        #endregion

        #region Properties

        /// <summary>
        /// Indicates whether this authentication token is null or empty.
        /// </summary>
        public Boolean IsNullOrEmpty
            => InternalId.IsNullOrEmpty();

        /// <summary>
        /// Indicates whether this authentication token is NOT null or empty.
        /// </summary>
        public Boolean IsNotNullOrEmpty
            => InternalId.IsNotNullOrEmpty();

        /// <summary>
        /// The length of the authentication token.
        /// </summary>
        public UInt64 Length
            => (UInt64)(InternalId?.Length ?? 0);

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new authentication token based on the given string.
        /// </summary>
        /// <param name="Text">The value of the authentication token.</param>
        private AuthenticationToken(String Text)
        {
            InternalId = Text;
        }

        #endregion


        #region (static) NewRandom(Length = 20)

        /// <summary>
        /// Create a new random authentication token.
        /// </summary>
        /// <param name="Length">The expected length of the authentication token.</param>
        public static AuthenticationToken NewRandom(Byte Length = 20)

            => Parse(RandomExtensions.RandomString(Length));

        #endregion

        #region (static) NewRandom4Bytes

        /// <summary>
        /// Create a new random 4-byte authentication token.
        /// </summary>
        public static AuthenticationToken NewRandom4Bytes

            => ParseHEX(RandomExtensions.RandomHexString(8));

        #endregion

        #region (static) NewRandom7Bytes

        /// <summary>
        /// Create a new random 7-byte authentication token.
        /// </summary>
        public static AuthenticationToken NewRandom7Bytes

            => ParseHEX(RandomExtensions.RandomHexString(14));

        #endregion

        #region (static) NewRandom10Bytes

        /// <summary>
        /// Create a new random 10-byte authentication token.
        /// </summary>
        public static AuthenticationToken NewRandom10Bytes

            => ParseHEX(RandomExtensions.RandomHexString(20));

        #endregion


        #region (static) Parse      (Text)

        /// <summary>
        /// Parse the given string as an authentication token.
        /// </summary>
        /// <param name="Text">A text representation of an authentication token.</param>
        public static AuthenticationToken Parse(String Text)
        {

            if (TryParse(Text, out var authenticationToken))
                return authenticationToken;

            throw new ArgumentException("Invalid text representation of an authentication token: '" + Text + "'!",
                                        nameof(Text));

        }

        #endregion

        #region (static) TryParse   (Text)

        /// <summary>
        /// Try to parse the given string as an authentication token.
        /// </summary>
        /// <param name="Text">A text representation of an authentication token.</param>
        public static AuthenticationToken? TryParse(String Text)
        {

            if (TryParse(Text, out var authenticationToken))
                return authenticationToken;

            return null;

        }

        #endregion

        #region (static) TryParse   (Text, out AuthenticationToken)

        /// <summary>
        /// Try to parse the given string as an authentication token.
        /// </summary>
        /// <param name="Text">A text representation of an authentication token.</param>
        /// <param name="AuthenticationToken">The parsed authentication token.</param>
        public static Boolean TryParse(String Text, out AuthenticationToken AuthenticationToken)
        {

            if (!Text.IsNullOrEmpty())
            {

                Text = Text.Trim();

                if (AuthenticationToken_RegEx.IsMatch(Text))
                {
                    try
                    {
                        AuthenticationToken = new AuthenticationToken(Text);
                        return true;
                    }
                    catch
                    { }
                }

            }

            AuthenticationToken = default;
            return false;

        }

        #endregion


        #region (static) ParseHEX   (HEX)

        /// <summary>
        /// Parse the given hexadecimal string as an authentication token.
        /// </summary>
        /// <param name="HEX">A hexadecimal representation of an authentication token.</param>
        public static AuthenticationToken ParseHEX(String HEX)
        {

            if (TryParseHEX(HEX, out var authenticationToken))
                return authenticationToken;

            throw new ArgumentException("Invalid text representation of an authentication token: '" + HEX + "'!",
                                        nameof(HEX));

        }

        #endregion

        #region (static) TryParseHEX(HEX)

        /// <summary>
        /// Try to parse the given hexadecimal string as an authentication token.
        /// </summary>
        /// <param name="HEX">A hexadecimal representation of an authentication token.</param>
        public static AuthenticationToken? TryParseHEX(String HEX)
        {

            if (TryParseHEX(HEX, out var authenticationToken))
                return authenticationToken;

            return null;

        }

        #endregion

        #region (static) TryParseHEX(HEX, out AuthenticationToken)

        /// <summary>
        /// Try to parse the given hexadecimal string as an authentication token.
        /// </summary>
        /// <param name="HEX">A hexadecimal representation of an authentication token.</param>
        /// <param name="AuthenticationToken">The parsed authentication token.</param>
        public static Boolean TryParseHEX(String HEX, out AuthenticationToken AuthenticationToken)
        {

            if (!HEX.IsNullOrEmpty())
            {

                HEX = HEX.Trim();

                if (AuthenticationToken_RegEx.IsMatch(HEX))
                {
                    try
                    {
                        AuthenticationToken = new AuthenticationToken(HEX);
                        return true;
                    }
                    catch
                    { }
                }

            }

            AuthenticationToken = default;
            return false;

        }

        #endregion


        #region Clone

        /// <summary>
        /// Clone this RFID card identification.
        /// </summary>
        public AuthenticationToken Clone

            => new(
                   new String(InternalId?.ToCharArray())
               );

        #endregion


        #region Operator overloading

        #region Operator == (AuthenticationToken1, AuthenticationToken2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="AuthenticationToken1">An authentication token.</param>
        /// <param name="AuthenticationToken2">Another authentication token.</param>
        /// <returns>true|false</returns>
        public static Boolean operator == (AuthenticationToken AuthenticationToken1,
                                           AuthenticationToken AuthenticationToken2)

            => AuthenticationToken1.Equals(AuthenticationToken2);

        #endregion

        #region Operator != (AuthenticationToken1, AuthenticationToken2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="AuthenticationToken1">An authentication token.</param>
        /// <param name="AuthenticationToken2">Another authentication token.</param>
        /// <returns>true|false</returns>
        public static Boolean operator != (AuthenticationToken AuthenticationToken1,
                                           AuthenticationToken AuthenticationToken2)

            => !(AuthenticationToken1 == AuthenticationToken2);

        #endregion

        #region Operator <  (AuthenticationToken1, AuthenticationToken2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="AuthenticationToken1">An authentication token.</param>
        /// <param name="AuthenticationToken2">Another authentication token.</param>
        /// <returns>true|false</returns>
        public static Boolean operator < (AuthenticationToken AuthenticationToken1,
                                          AuthenticationToken AuthenticationToken2)

            => AuthenticationToken1.CompareTo(AuthenticationToken2) < 0;

        #endregion

        #region Operator <= (AuthenticationToken1, AuthenticationToken2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="AuthenticationToken1">An authentication token.</param>
        /// <param name="AuthenticationToken2">Another authentication token.</param>
        /// <returns>true|false</returns>
        public static Boolean operator <= (AuthenticationToken AuthenticationToken1,
                                           AuthenticationToken AuthenticationToken2)

            => !(AuthenticationToken1 > AuthenticationToken2);

        #endregion

        #region Operator >  (AuthenticationToken1, AuthenticationToken2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="AuthenticationToken1">An authentication token.</param>
        /// <param name="AuthenticationToken2">Another authentication token.</param>
        /// <returns>true|false</returns>
        public static Boolean operator > (AuthenticationToken AuthenticationToken1,
                                          AuthenticationToken AuthenticationToken2)

            => AuthenticationToken1.CompareTo(AuthenticationToken2) > 0;

        #endregion

        #region Operator >= (AuthenticationToken1, AuthenticationToken2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="AuthenticationToken1">An authentication token.</param>
        /// <param name="AuthenticationToken2">Another authentication token.</param>
        /// <returns>true|false</returns>
        public static Boolean operator >= (AuthenticationToken AuthenticationToken1,
                                           AuthenticationToken AuthenticationToken2)

            => !(AuthenticationToken1 < AuthenticationToken2);

        #endregion

        #endregion

        #region IComparable<AuthenticationToken> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two authentication tokens.
        /// </summary>
        /// <param name="Object">An authentication token to compare with.</param>
        public Int32 CompareTo(Object? Object)

            => Object is AuthenticationToken authenticationToken
                   ? CompareTo(authenticationToken)
                   : throw new ArgumentException("The given object is not an authentication token!",
                                                 nameof(Object));

        #endregion

        #region CompareTo(AuthenticationToken)

        /// <summary>
        /// Compares two authentication tokens.
        /// </summary>
        /// <param name="AuthenticationToken">An authentication token to compare with.</param>
        public Int32 CompareTo(AuthenticationToken AuthenticationToken)

            => String.Compare(InternalId,
                              AuthenticationToken.InternalId,
                              StringComparison.OrdinalIgnoreCase);

        #endregion

        #endregion

        #region IEquatable<AuthenticationToken> Members

        #region Equals(Object)

        /// <summary>
        /// Compares two authentication tokens for equality.
        /// </summary>
        /// <param name="Object">An authentication token to compare with.</param>
        public override Boolean Equals(Object? Object)

            => Object is AuthenticationToken authenticationToken &&
                   Equals(authenticationToken);

        #endregion

        #region Equals(AuthenticationToken)

        /// <summary>
        /// Compares two authentication tokens for equality.
        /// </summary>
        /// <param name="AuthenticationToken">An authentication token to compare with.</param>
        public Boolean Equals(AuthenticationToken AuthenticationToken)

            => String.Equals(InternalId,
                             AuthenticationToken.InternalId,
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
