/*
 * Copyright (c) 2014-2024 GraphDefined GmbH <achim.friedland@graphdefined.com>
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
    /// A unique authentication token status.
    /// </summary>
    public readonly struct AuthenticationTokenStatus : IId,
                                                       IEquatable<AuthenticationTokenStatus>,
                                                       IComparable<AuthenticationTokenStatus>
    {

        #region Data

        /// <summary>
        /// The internal identification.
        /// </summary>
        private readonly String InternalId;

        #endregion

        #region Properties

        /// <summary>
        /// Indicates whether this authentication token status is null or empty.
        /// </summary>
        public Boolean IsNullOrEmpty
            => InternalId.IsNullOrEmpty();

        /// <summary>
        /// Indicates whether this authentication token status is NOT null or empty.
        /// </summary>
        public Boolean IsNotNullOrEmpty
            => InternalId.IsNotNullOrEmpty();

        /// <summary>
        /// The length of the authentication token status.
        /// </summary>
        public UInt64 Length
            => (UInt64)(InternalId?.Length ?? 0);

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new authentication token status based on the given string.
        /// </summary>
        /// <param name="Text">The value of the authentication token status.</param>
        private AuthenticationTokenStatus(String Text)
        {
            InternalId = Text;
        }

        #endregion


        #region (static) Parse   (Text)

        /// <summary>
        /// Parse the given string as an authentication token status.
        /// </summary>
        /// <param name="Text">A text representation of an authentication token status.</param>
        public static AuthenticationTokenStatus Parse(String Text)
        {

            if (TryParse(Text, out var authenticationToken))
                return authenticationToken;

            throw new ArgumentException($"Invalid text representation of an authentication token status: '" + Text + "'!",
                                        nameof(Text));

        }

        #endregion

        #region (static) TryParse(Text)

        /// <summary>
        /// Try to parse the given string as an authentication token status.
        /// </summary>
        /// <param name="Text">A text representation of an authentication token status.</param>
        public static AuthenticationTokenStatus? TryParse(String Text)
        {

            if (TryParse(Text, out var authenticationToken))
                return authenticationToken;

            return null;

        }

        #endregion

        #region (static) TryParse(Text, out AuthenticationTokenStatus)

        /// <summary>
        /// Try to parse the given string as an authentication token status.
        /// </summary>
        /// <param name="Text">A text representation of an authentication token status.</param>
        /// <param name="AuthenticationTokenStatus">The parsed authentication token status.</param>
        public static Boolean TryParse(String Text, out AuthenticationTokenStatus AuthenticationTokenStatus)
        {

            Text = Text.Trim();

            if (!Text.IsNullOrEmpty())
            {
                try
                {
                    AuthenticationTokenStatus = new AuthenticationTokenStatus(Text);
                    return true;
                }
                catch
                { }
            }

            AuthenticationTokenStatus = default;
            return false;

        }

        #endregion

        #region Clone

        /// <summary>
        /// Clone this authentication token status.
        /// </summary>
        public AuthenticationTokenStatus Clone

            => new(
                   new String(InternalId?.ToCharArray())
               );

        #endregion


        #region Static Definitions

        /// <summary>
        /// Unclear or unknown status of the authentication token.
        /// </summary>
        public static AuthenticationTokenStatus Unspecified
            => new ("Unspecified");

        /// <summary>
        /// The authentication token is invalid.
        /// </summary>
        public static AuthenticationTokenStatus Invalid
            => new ("Invalid");

        /// <summary>
        /// The processing of the authentication token led to an error.
        /// </summary>
        public static AuthenticationTokenStatus Error
            => new ("Error");

        #endregion


        #region Operator overloading

        #region Operator == (AuthenticationTokenStatus1, AuthenticationTokenStatus2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="AuthenticationTokenStatus1">An authentication token status.</param>
        /// <param name="AuthenticationTokenStatus2">Another authentication token status.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator == (AuthenticationTokenStatus AuthenticationTokenStatus1,
                                           AuthenticationTokenStatus AuthenticationTokenStatus2)

            => AuthenticationTokenStatus1.Equals(AuthenticationTokenStatus2);

        #endregion

        #region Operator != (AuthenticationTokenStatus1, AuthenticationTokenStatus2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="AuthenticationTokenStatus1">An authentication token status.</param>
        /// <param name="AuthenticationTokenStatus2">Another authentication token status.</param>
        /// <returns>False if both match; True otherwise.</returns>
        public static Boolean operator != (AuthenticationTokenStatus AuthenticationTokenStatus1,
                                           AuthenticationTokenStatus AuthenticationTokenStatus2)

            => !(AuthenticationTokenStatus1 == AuthenticationTokenStatus2);

        #endregion

        #region Operator <  (AuthenticationTokenStatus1, AuthenticationTokenStatus2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="AuthenticationTokenStatus1">An authentication token status.</param>
        /// <param name="AuthenticationTokenStatus2">Another authentication token status.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator < (AuthenticationTokenStatus AuthenticationTokenStatus1,
                                          AuthenticationTokenStatus AuthenticationTokenStatus2)

            => AuthenticationTokenStatus1.CompareTo(AuthenticationTokenStatus2) < 0;

        #endregion

        #region Operator <= (AuthenticationTokenStatus1, AuthenticationTokenStatus2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="AuthenticationTokenStatus1">An authentication token status.</param>
        /// <param name="AuthenticationTokenStatus2">Another authentication token status.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator <= (AuthenticationTokenStatus AuthenticationTokenStatus1,
                                           AuthenticationTokenStatus AuthenticationTokenStatus2)

            => !(AuthenticationTokenStatus1 > AuthenticationTokenStatus2);

        #endregion

        #region Operator >  (AuthenticationTokenStatus1, AuthenticationTokenStatus2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="AuthenticationTokenStatus1">An authentication token status.</param>
        /// <param name="AuthenticationTokenStatus2">Another authentication token status.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator > (AuthenticationTokenStatus AuthenticationTokenStatus1,
                                          AuthenticationTokenStatus AuthenticationTokenStatus2)

            => AuthenticationTokenStatus1.CompareTo(AuthenticationTokenStatus2) > 0;

        #endregion

        #region Operator >= (AuthenticationTokenStatus1, AuthenticationTokenStatus2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="AuthenticationTokenStatus1">An authentication token status.</param>
        /// <param name="AuthenticationTokenStatus2">Another authentication token status.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator >= (AuthenticationTokenStatus AuthenticationTokenStatus1,
                                           AuthenticationTokenStatus AuthenticationTokenStatus2)

            => !(AuthenticationTokenStatus1 < AuthenticationTokenStatus2);

        #endregion

        #endregion

        #region IComparable<AuthenticationTokenStatus> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two authentication token status.
        /// </summary>
        /// <param name="Object">An authentication token status to compare with.</param>
        public Int32 CompareTo(Object? Object)

            => Object is AuthenticationTokenStatus authenticationToken
                   ? CompareTo(authenticationToken)
                   : throw new ArgumentException("The given object is not an authentication token status!",
                                                 nameof(Object));

        #endregion

        #region CompareTo(AuthenticationTokenStatus)

        /// <summary>
        /// Compares two authentication token status.
        /// </summary>
        /// <param name="AuthenticationTokenStatus">An authentication token status to compare with.</param>
        public Int32 CompareTo(AuthenticationTokenStatus AuthenticationTokenStatus)

            => String.Compare(InternalId,
                              AuthenticationTokenStatus.InternalId,
                              StringComparison.OrdinalIgnoreCase);

        #endregion

        #endregion

        #region IEquatable<AuthenticationTokenStatus> Members

        #region Equals(Object)

        /// <summary>
        /// Compares two authentication token status for equality.
        /// </summary>
        /// <param name="Object">An authentication token status to compare with.</param>
        public override Boolean Equals(Object? Object)

            => Object is AuthenticationTokenStatus authenticationToken &&
                   Equals(authenticationToken);

        #endregion

        #region Equals(AuthenticationTokenStatus)

        /// <summary>
        /// Compares two authentication token status for equality.
        /// </summary>
        /// <param name="AuthenticationTokenStatus">An authentication token status to compare with.</param>
        public Boolean Equals(AuthenticationTokenStatus AuthenticationTokenStatus)

            => String.Equals(InternalId,
                             AuthenticationTokenStatus.InternalId,
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
