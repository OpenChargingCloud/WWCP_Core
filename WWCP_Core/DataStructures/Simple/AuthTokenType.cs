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

using org.GraphDefined.Vanaheimr.Illias;

#endregion

namespace cloud.charging.open.protocols.WWCP
{

    /// <summary>
    /// Extension token types for authentication (verification) token types.
    /// </summary>
    public static class AuthTokenTypeExtensions
    {

        /// <summary>
        /// Indicates whether this authentication token type is null or empty.
        /// </summary>
        /// <param name="AuthTokenType">An authentication token type.</param>
        public static Boolean IsNullOrEmpty(this AuthTokenType? AuthTokenType)
            => !AuthTokenType.HasValue || AuthTokenType.Value.IsNullOrEmpty;

        /// <summary>
        /// Indicates whether this authentication token type is null or empty.
        /// </summary>
        /// <param name="AuthTokenType">An authentication token type.</param>
        public static Boolean IsNotNullOrEmpty(this AuthTokenType? AuthTokenType)
            => AuthTokenType.HasValue && AuthTokenType.Value.IsNotNullOrEmpty;

    }


    /// <summary>
    /// Authentication (verification) token types.
    /// </summary>
    public readonly struct AuthTokenType : IId<AuthTokenType>
    {

        #region Data

        /// <summary>
        /// The internal identification.
        /// </summary>
        private readonly String InternalId;

        #endregion

        #region Properties

        /// <summary>
        /// Indicates whether this authentication token type is null or empty.
        /// </summary>
        public Boolean IsNullOrEmpty
            => InternalId.IsNullOrEmpty();

        /// <summary>
        /// Indicates whether this authentication token type is NOT null or empty.
        /// </summary>
        public Boolean IsNotNullOrEmpty
            => InternalId.IsNotNullOrEmpty();

        /// <summary>
        /// The length of the authentication token type.
        /// </summary>
        public UInt64  Length
            => (UInt64) (InternalId?.Length ?? 0);

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new authentication token type.
        /// based on the given string.
        /// </summary>
        private AuthTokenType(String Text)
        {
            InternalId = Text;
        }

        #endregion


        #region (static) Parse   (Text)

        /// <summary>
        /// Parse the given string as an authentication token type.
        /// </summary>
        /// <param name="Text">A text representation of an authentication token type.</param>
        public static AuthTokenType Parse(String Text)
        {

            if (TryParse(Text, out var authTokenType))
                return authTokenType;

            throw new ArgumentException($"Invalid text representation of an authentication token type: '" + Text + "'!",
                                        nameof(Text));

        }

        #endregion

        #region (static) TryParse(Text)

        /// <summary>
        /// Try to parse the given string as an authentication token type.
        /// </summary>
        /// <param name="Text">A text representation of an authentication token type.</param>
        public static AuthTokenType? TryParse(String Text)
        {

            if (TryParse(Text, out var authTokenType))
                return authTokenType;

            return null;

        }

        #endregion

        #region (static) TryParse(Text, out AuthTokenType)

        /// <summary>
        /// Parse the given string as an authentication token type.
        /// </summary>
        /// <param name="Text">A text representation of an authentication token type.</param>
        /// <param name="AuthTokenType">The parsed authentication token type.</param>
        public static Boolean TryParse(String Text, out AuthTokenType AuthTokenType)
        {

            if (Text.IsNotNullOrEmpty())
            {
                try
                {
                    AuthTokenType = new AuthTokenType(Text.Trim());
                    return true;
                }
                catch
                { }
            }

            AuthTokenType = default;
            return false;

        }

        #endregion

        #region Clone()

        /// <summary>
        /// Clone this authentication token type.
        /// </summary>
        public AuthTokenType Clone()

            => new (
                   InternalId.CloneString()
               );

        #endregion


        #region Static Methods

        /// <summary>
        /// An RFID card was used for authentication.
        /// </summary>
        public static AuthTokenType  RFID           { get; }
            = Parse("RFID");

        /// <summary>
        /// An ISO 15118 compliant authentication token was used for authentication.
        /// </summary>
        public static AuthTokenType  ISO15118       { get; }
            = Parse("ISO15118");

        /// <summary>
        /// An ethernet MAC address was used for authentication.
        /// </summary>
        public static AuthTokenType  MACAddress     { get; }
            = Parse("MACAddress");

        /// <summary>
        /// A one time use token identification generated by a server or app.
        /// The eMSP uses this to bind a charging session to a customer, probably an app user.
        /// </summary>
        public static AuthTokenType  AD_HOC_USER    { get; }
            = Parse("AD_HOC_USER");

        /// <summary>
        /// An unknown authentication token type was used for authentication.
        /// </summary>
        public static AuthTokenType  OTHER          { get; }
            = Parse("OTHER");

        #endregion


        #region Operator overloading

        #region Operator == (AuthTokenType1, AuthTokenType2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="AuthTokenType1">An authentication token type.</param>
        /// <param name="AuthTokenType2">Another authentication token type.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator == (AuthTokenType AuthTokenType1,
                                           AuthTokenType AuthTokenType2)

            => AuthTokenType1.Equals(AuthTokenType2);

        #endregion

        #region Operator != (AuthTokenType1, AuthTokenType2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="AuthTokenType1">An authentication token type.</param>
        /// <param name="AuthTokenType2">Another authentication token type.</param>
        /// <returns>False if both match; True otherwise.</returns>
        public static Boolean operator != (AuthTokenType AuthTokenType1,
                                           AuthTokenType AuthTokenType2)

            => !AuthTokenType1.Equals(AuthTokenType2);

        #endregion

        #region Operator <  (AuthTokenType1, AuthTokenType2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="AuthTokenType1">An authentication token type.</param>
        /// <param name="AuthTokenType2">Another authentication token type.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator < (AuthTokenType AuthTokenType1,
                                          AuthTokenType AuthTokenType2)

            => AuthTokenType1.CompareTo(AuthTokenType2) < 0;

        #endregion

        #region Operator <= (AuthTokenType1, AuthTokenType2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="AuthTokenType1">An authentication token type.</param>
        /// <param name="AuthTokenType2">Another authentication token type.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator <= (AuthTokenType AuthTokenType1,
                                           AuthTokenType AuthTokenType2)

            => AuthTokenType1.CompareTo(AuthTokenType2) <= 0;

        #endregion

        #region Operator >  (AuthTokenType1, AuthTokenType2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="AuthTokenType1">An authentication token type.</param>
        /// <param name="AuthTokenType2">Another authentication token type.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator > (AuthTokenType AuthTokenType1,
                                          AuthTokenType AuthTokenType2)

            => AuthTokenType1.CompareTo(AuthTokenType2) > 0;

        #endregion

        #region Operator >= (AuthTokenType1, AuthTokenType2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="AuthTokenType1">An authentication token type.</param>
        /// <param name="AuthTokenType2">Another authentication token type.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator >= (AuthTokenType AuthTokenType1,
                                           AuthTokenType AuthTokenType2)

            => AuthTokenType1.CompareTo(AuthTokenType2) >= 0;

        #endregion

        #endregion

        #region IComparable<AuthTokenType> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two authentication token types.
        /// </summary>
        /// <param name="Object">An authentication token type to compare with.</param>
        public Int32 CompareTo(Object? Object)

            => Object is AuthTokenType authTokenType
                   ? CompareTo(authTokenType)
                   : throw new ArgumentException("The given object is not an authentication token type!",
                                                 nameof(Object));

        #endregion

        #region CompareTo(AuthTokenType)

        /// <summary>
        /// Compares two authentication token types.
        /// </summary>
        /// <param name="AuthTokenType">An authentication token type to compare with.</param>
        public Int32 CompareTo(AuthTokenType AuthTokenType)

            => String.Compare(InternalId,
                              AuthTokenType.InternalId,
                              StringComparison.OrdinalIgnoreCase);

        #endregion

        #endregion

        #region IEquatable<AuthTokenType> Members

        #region Equals(Object)

        /// <summary>
        /// Compares two authentication token types for equality.
        /// </summary>
        /// <param name="AuthTokenType">An authentication token type to compare with.</param>
        public override Boolean Equals(Object? Object)

            => Object is AuthTokenType authTokenType &&
                   Equals(authTokenType);

        #endregion

        #region Equals(AuthTokenType)

        /// <summary>
        /// Compares two authentication token types for equality.
        /// </summary>
        /// <param name="AuthTokenType">An authentication token type to compare with.</param>
        public Boolean Equals(AuthTokenType AuthTokenType)

            => String.Equals(InternalId,
                             AuthTokenType.InternalId,
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
