/*
 * Copyright (c) 2014-2026 GraphDefined GmbH <achim.friedland@graphdefined.com>
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

using System.Diagnostics.CodeAnalysis;

using org.GraphDefined.Vanaheimr.Illias;

#endregion

namespace cloud.charging.open.protocols.WWCP
{

    /// <summary>
    /// A unique authentication token with an optional token type.
    /// </summary>
    public class AuthenticationToken : IEquatable<AuthenticationToken>,
                                       IComparable<AuthenticationToken>,
                                       IComparable
    {

        #region Properties

        /// <summary>
        /// The unique authentication token.
        /// </summary>
        public AuthenticationToken2  Token    { get; }

        /// <summary>
        /// The optional type of the authentication token.
        /// </summary>
        public AuthTokenType?        Type     { get; }

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new authentication token having an optional token type.
        /// </summary>
        /// <param name="Token">The value of the authentication token.</param>
        /// <param name="Type">The type of the authentication token.</param>
        public AuthenticationToken(AuthenticationToken2  Token,
                                   AuthTokenType?        Type = null)
        {

            this.Token  = Token;
            this.Type   = Type;

        }

        #endregion


        #region (static) NewRandomRFID4Bytes

        /// <summary>
        /// Create a new random 4-byte authentication token.
        /// </summary>
        public static AuthenticationToken NewRandomRFID4Bytes

            => ParseHEX(
                   RandomExtensions.RandomHexString(8),
                   AuthTokenType.RFID
               );

        #endregion

        #region (static) NewRandomRFID7Bytes

        /// <summary>
        /// Create a new random 7-byte authentication token.
        /// </summary>
        public static AuthenticationToken NewRandomRFID7Bytes

            => ParseHEX(
                   RandomExtensions.RandomHexString(14),
                   AuthTokenType.RFID
               );

        #endregion

        #region (static) NewRandomRFID10Bytes

        /// <summary>
        /// Create a new random 10-byte authentication token.
        /// </summary>
        public static AuthenticationToken NewRandomRFID10Bytes

            => ParseHEX(
                   RandomExtensions.RandomHexString(20),
                   AuthTokenType.RFID
               );

        #endregion


        #region (static) Parse       (Text                           Type = null)

        /// <summary>
        /// Parse the given string as an authentication token.
        /// </summary>
        /// <param name="Text">A text representation of an authentication token.</param>
        /// <param name="Type">An optional authentication token type.</param>
        public static AuthenticationToken Parse(String          Text,
                                                AuthTokenType?  Type = null)
        {

            if (TryParse(Text, out var authenticationToken, Type))
                return authenticationToken;

            throw new ArgumentException($"Invalid text representation of an authentication token: '{Text}'!",
                                        nameof(Text));

        }

        #endregion

        #region (static) TryParse    (Text, out AuthenticationToken, Type = null)

        // Note: The following is needed to satisfy pattern matching delegates! Do not refactor it!

        /// <summary>
        /// Try to parse the given string as an authentication token.
        /// </summary>
        /// <param name="Text">A text representation of an authentication token.</param>
        /// <param name="AuthenticationToken">The parsed authentication token.</param>
        public static Boolean TryParse(String                                        Text,
                                       [NotNullWhen(true)] out AuthenticationToken?  AuthenticationToken)

            => TryParse(Text,
                        out AuthenticationToken,
                        null);


        /// <summary>
        /// Try to parse the given string as an authentication token.
        /// </summary>
        /// <param name="Text">A text representation of an authentication token.</param>
        /// <param name="AuthenticationToken">The parsed authentication token.</param>
        /// <param name="Type">An optional authentication token type.</param>
        public static Boolean TryParse(String                                        Text,
                                       [NotNullWhen(true)] out AuthenticationToken?  AuthenticationToken,
                                       AuthTokenType?                                Type = null)
        {

            if (AuthenticationToken2.TryParse(Text, out var token))
            {

                AuthenticationToken = new AuthenticationToken(
                                          token,
                                          Type
                                      );

                return true;

            }

            AuthenticationToken = null;
            return false;

        }

        #endregion


        #region (static) ParseHEX    (HEX,                           Type = null)

        /// <summary>
        /// Parse the given hexadecimal string as an authentication token.
        /// </summary>
        /// <param name="HEX">A hexadecimal representation of an authentication token.</param>
        /// <param name="Type">An optional authentication token type.</param>
        public static AuthenticationToken ParseHEX(String          HEX,
                                                   AuthTokenType?  Type = null)
        {

            if (TryParseHEX(HEX, out var authenticationToken, Type))
                return authenticationToken;

            throw new ArgumentException($"Invalid text representation of an authentication token: '{HEX}'!",
                                        nameof(HEX));

        }

        #endregion

        #region (static) TryParseHEX (HEX,  out AuthenticationToken, Type = null)

        // Note: The following is needed to satisfy pattern matching delegates! Do not refactor it!

        /// <summary>
        /// Try to parse the given hexadecimal string as an authentication token.
        /// </summary>
        /// <param name="HEX">A hexadecimal representation of an authentication token.</param>
        /// <param name="AuthenticationToken">The parsed authentication token.</param>
        public static Boolean TryParseHEX(String                                        HEX,
                                          [NotNullWhen(true)] out AuthenticationToken?  AuthenticationToken)

            => TryParseHEX(HEX,
                           out AuthenticationToken,
                           null);


        /// <summary>
        /// Try to parse the given hexadecimal string as an authentication token.
        /// </summary>
        /// <param name="HEX">A hexadecimal representation of an authentication token.</param>
        /// <param name="AuthenticationToken">The parsed authentication token.</param>
        /// <param name="Type">An optional authentication token type.</param>
        public static Boolean TryParseHEX(String                                        HEX,
                                          [NotNullWhen(true)] out AuthenticationToken?  AuthenticationToken,
                                          AuthTokenType?                                Type = null)
        {

            if (AuthenticationToken2.TryParseHEX(HEX, out var authenticationToken))
            {

                AuthenticationToken = new AuthenticationToken(
                                          authenticationToken,
                                          Type
                                      );

                return true;

            }

            AuthenticationToken = null;
            return false;

        }

        #endregion


        #region Clone()

        /// <summary>
        /// Clone this authentication token.
        /// </summary>
        public AuthenticationToken Clone()

            => new (
                   Token.Clone(),
                   Type?.Clone()
               );

        #endregion


        #region Operator overloading

        #region Operator == (AuthenticationToken1, AuthenticationToken2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="AuthenticationToken1">An authentication token.</param>
        /// <param name="AuthenticationToken2">Another authentication token.</param>
        /// <returns>True if both match; False otherwise.</returns>
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
        /// <returns>False if both match; True otherwise.</returns>
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
        /// <returns>True if both match; False otherwise.</returns>
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
        /// <returns>True if both match; False otherwise.</returns>
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
        /// <returns>True if both match; False otherwise.</returns>
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
        /// <returns>True if both match; False otherwise.</returns>
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
        public Int32 CompareTo(AuthenticationToken? AuthenticationToken)
        {

            if (AuthenticationToken is null)
                throw new ArgumentNullException(nameof(AuthenticationToken), "The given authentication token must not be null!");

            var c = Token.CompareTo(AuthenticationToken.Token);

            if (c == 0 && Type.HasValue && AuthenticationToken.Type.HasValue)
                c = Type.Value.CompareTo(AuthenticationToken.Type.Value);

            return c;

        }

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
        public Boolean Equals(AuthenticationToken? AuthenticationToken)

            => AuthenticationToken is not null &&

               Token.Equals(AuthenticationToken.Token) &&

            ((!Type.HasValue && !AuthenticationToken.Type.HasValue) ||
              (Type.HasValue &&  AuthenticationToken.Type.HasValue && Type.Value.Equals(AuthenticationToken.Type.Value)));

        #endregion

        #endregion

        #region (override) GetHashCode()

        /// <summary>
        /// Return the hash code of this object.
        /// </summary>
        public override Int32 GetHashCode()

            => Token.GetHashCode() ^
               Type?.GetHashCode() ?? 0;

        #endregion

        #region (override) ToString()

        /// <summary>
        /// Return a text representation of this object.
        /// </summary>
        public override String ToString()

            => String.Concat(

                   Token.ToString(),

                   Type.HasValue
                       ? $" ({Type.Value})"
                       : ""

               );

        #endregion

    }



    /// <summary>
    /// A unique authentication token.
    /// </summary>
    public readonly struct AuthenticationToken2 : IId,
                                                  IEquatable<AuthenticationToken2>,
                                                  IComparable<AuthenticationToken2>
    {

        #region Data

        /// <summary>
        /// The internal identification.
        /// </summary>
        private readonly String InternalId;


        ///// <summary>
        ///// The regular expression for parsing an authentication token.
        ///// </summary>
        //public static readonly Regex AuthenticationToken2_RegEx = new ("^([a-fA-F0-9]{8})$ | ^([a-fA-F0-9]{14})$ | ^([a-fA-F0-9]{20})$",
        //                                                              RegexOptions.IgnorePatternWhitespace);

        #endregion

        #region Properties

        /// <summary>
        /// Indicates whether this authentication token is null or empty.
        /// </summary>
        public Boolean  IsNullOrEmpty
            => InternalId.IsNullOrEmpty();

        /// <summary>
        /// Indicates whether this authentication token is NOT null or empty.
        /// </summary>
        public Boolean  IsNotNullOrEmpty
            => InternalId.IsNotNullOrEmpty();

        /// <summary>
        /// The length of the authentication token.
        /// </summary>
        public UInt64   Length
            => (UInt64) (InternalId?.Length ?? 0);

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new authentication token based on the given string.
        /// </summary>
        /// <param name="Text">The value of the authentication token.</param>
        private AuthenticationToken2(String Text)
        {
            InternalId = Text;
        }

        #endregion


        #region (static) NewRandom(Length = 20)

        /// <summary>
        /// Create a new random authentication token.
        /// </summary>
        /// <param name="Length">The expected length of the authentication token.</param>
        public static AuthenticationToken2 NewRandom(Byte Length = 20)

            => Parse(RandomExtensions.RandomString(Length));

        #endregion

        #region (static) NewRandom4Bytes

        /// <summary>
        /// Create a new random 4-byte authentication token.
        /// </summary>
        public static AuthenticationToken2 NewRandom4Bytes

            => ParseHEX(RandomExtensions.RandomHexString(8));

        #endregion

        #region (static) NewRandom7Bytes

        /// <summary>
        /// Create a new random 7-byte authentication token.
        /// </summary>
        public static AuthenticationToken2 NewRandom7Bytes

            => ParseHEX(RandomExtensions.RandomHexString(14));

        #endregion

        #region (static) NewRandom10Bytes

        /// <summary>
        /// Create a new random 10-byte authentication token.
        /// </summary>
        public static AuthenticationToken2 NewRandom10Bytes

            => ParseHEX(RandomExtensions.RandomHexString(20));

        #endregion


        #region (static) Parse       (Text)

        /// <summary>
        /// Parse the given string as an authentication token.
        /// </summary>
        /// <param name="Text">A text representation of an authentication token.</param>
        public static AuthenticationToken2 Parse(String Text)
        {

            if (TryParse(Text, out var authenticationToken))
                return authenticationToken;

            throw new ArgumentException($"Invalid text representation of an authentication token: '" + Text + "'!",
                                        nameof(Text));

        }

        #endregion

        #region (static) TryParse    (Text)

        /// <summary>
        /// Try to parse the given string as an authentication token.
        /// </summary>
        /// <param name="Text">A text representation of an authentication token.</param>
        public static AuthenticationToken2? TryParse(String Text)
        {

            if (TryParse(Text, out var authenticationToken))
                return authenticationToken;

            return null;

        }

        #endregion

        #region (static) TryParse    (Text, out AuthenticationToken2)

        /// <summary>
        /// Try to parse the given string as an authentication token.
        /// </summary>
        /// <param name="Text">A text representation of an authentication token.</param>
        /// <param name="AuthenticationToken2">The parsed authentication token.</param>
        public static Boolean TryParse(String                    Text,
                                       out AuthenticationToken2  AuthenticationToken2)
        {

            if (!Text.IsNullOrEmpty())
            {

                Text = Text.Trim();

                //if (AuthenticationToken2_RegEx.IsMatch(Text))
                //{
                    try
                    {
                        AuthenticationToken2 = new AuthenticationToken2(Text);
                        return true;
                    }
                    catch
                    { }
                //}

            }

            AuthenticationToken2 = default;
            return false;

        }

        #endregion


        #region (static) ParseHEX    (HEX)

        /// <summary>
        /// Parse the given hexadecimal string as an authentication token.
        /// </summary>
        /// <param name="HEX">A hexadecimal representation of an authentication token.</param>
        public static AuthenticationToken2 ParseHEX(String HEX)
        {

            if (TryParseHEX(HEX, out var authenticationToken))
                return authenticationToken;

            throw new ArgumentException($"Invalid text representation of an authentication token: '{HEX}'!",
                                        nameof(HEX));

        }

        #endregion

        #region (static) TryParseHEX (HEX)

        /// <summary>
        /// Try to parse the given hexadecimal string as an authentication token.
        /// </summary>
        /// <param name="HEX">A hexadecimal representation of an authentication token.</param>
        public static AuthenticationToken2? TryParseHEX(String HEX)
        {

            if (TryParseHEX(HEX, out var authenticationToken))
                return authenticationToken;

            return null;

        }

        #endregion

        #region (static) TryParseHEX (HEX, out AuthenticationToken2)

        /// <summary>
        /// Try to parse the given hexadecimal string as an authentication token.
        /// </summary>
        /// <param name="HEX">A hexadecimal representation of an authentication token.</param>
        /// <param name="AuthenticationToken2">The parsed authentication token.</param>
        public static Boolean TryParseHEX(String                    HEX,
                                          out AuthenticationToken2  AuthenticationToken2)
        {

            if (!HEX.IsNullOrWhiteSpace())
            {

                HEX = HEX.Trim().
                          Replace("-", "").
                          Replace(":", "").
                          ToUpperInvariant();

                if (HEX.All(c => (c >= '0' && c <= '9') ||
                                 (c >= 'A' && c <= 'F')))
                {

                    AuthenticationToken2 = new AuthenticationToken2(HEX);

                    return true;

                }

            }

            AuthenticationToken2 = default;
            return false;

        }

        #endregion


        #region Clone()

        /// <summary>
        /// Clone this authentication token.
        /// </summary>
        public AuthenticationToken2 Clone()

            => new (
                   InternalId.CloneString()
               );

        #endregion


        #region Operator overloading

        #region Operator == (AuthenticationToken21, AuthenticationToken22)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="AuthenticationToken21">An authentication token.</param>
        /// <param name="AuthenticationToken22">Another authentication token.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator == (AuthenticationToken2 AuthenticationToken21,
                                           AuthenticationToken2 AuthenticationToken22)

            => AuthenticationToken21.Equals(AuthenticationToken22);

        #endregion

        #region Operator != (AuthenticationToken21, AuthenticationToken22)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="AuthenticationToken21">An authentication token.</param>
        /// <param name="AuthenticationToken22">Another authentication token.</param>
        /// <returns>False if both match; True otherwise.</returns>
        public static Boolean operator != (AuthenticationToken2 AuthenticationToken21,
                                           AuthenticationToken2 AuthenticationToken22)

            => !(AuthenticationToken21 == AuthenticationToken22);

        #endregion

        #region Operator <  (AuthenticationToken21, AuthenticationToken22)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="AuthenticationToken21">An authentication token.</param>
        /// <param name="AuthenticationToken22">Another authentication token.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator < (AuthenticationToken2 AuthenticationToken21,
                                          AuthenticationToken2 AuthenticationToken22)

            => AuthenticationToken21.CompareTo(AuthenticationToken22) < 0;

        #endregion

        #region Operator <= (AuthenticationToken21, AuthenticationToken22)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="AuthenticationToken21">An authentication token.</param>
        /// <param name="AuthenticationToken22">Another authentication token.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator <= (AuthenticationToken2 AuthenticationToken21,
                                           AuthenticationToken2 AuthenticationToken22)

            => !(AuthenticationToken21 > AuthenticationToken22);

        #endregion

        #region Operator >  (AuthenticationToken21, AuthenticationToken22)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="AuthenticationToken21">An authentication token.</param>
        /// <param name="AuthenticationToken22">Another authentication token.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator > (AuthenticationToken2 AuthenticationToken21,
                                          AuthenticationToken2 AuthenticationToken22)

            => AuthenticationToken21.CompareTo(AuthenticationToken22) > 0;

        #endregion

        #region Operator >= (AuthenticationToken21, AuthenticationToken22)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="AuthenticationToken21">An authentication token.</param>
        /// <param name="AuthenticationToken22">Another authentication token.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator >= (AuthenticationToken2 AuthenticationToken21,
                                           AuthenticationToken2 AuthenticationToken22)

            => !(AuthenticationToken21 < AuthenticationToken22);

        #endregion

        #endregion

        #region IComparable<AuthenticationToken2> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two authentication tokens.
        /// </summary>
        /// <param name="Object">An authentication token to compare with.</param>
        public Int32 CompareTo(Object? Object)

            => Object is AuthenticationToken2 authenticationToken
                   ? CompareTo(authenticationToken)
                   : throw new ArgumentException("The given object is not an authentication token!",
                                                 nameof(Object));

        #endregion

        #region CompareTo(AuthenticationToken2)

        /// <summary>
        /// Compares two authentication tokens.
        /// </summary>
        /// <param name="AuthenticationToken2">An authentication token to compare with.</param>
        public Int32 CompareTo(AuthenticationToken2 AuthenticationToken2)

            => String.Compare(InternalId,
                              AuthenticationToken2.InternalId,
                              StringComparison.OrdinalIgnoreCase);

        #endregion

        #endregion

        #region IEquatable<AuthenticationToken2> Members

        #region Equals(Object)

        /// <summary>
        /// Compares two authentication tokens for equality.
        /// </summary>
        /// <param name="Object">An authentication token to compare with.</param>
        public override Boolean Equals(Object? Object)

            => Object is AuthenticationToken2 authenticationToken &&
                   Equals(authenticationToken);

        #endregion

        #region Equals(AuthenticationToken2)

        /// <summary>
        /// Compares two authentication tokens for equality.
        /// </summary>
        /// <param name="AuthenticationToken2">An authentication token to compare with.</param>
        public Boolean Equals(AuthenticationToken2 AuthenticationToken2)

            => String.Equals(InternalId,
                             AuthenticationToken2.InternalId,
                             StringComparison.OrdinalIgnoreCase);

        #endregion

        #endregion

        #region (override) GetHashCode()

        /// <summary>
        /// Return the hash code of this object.
        /// </summary>
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
