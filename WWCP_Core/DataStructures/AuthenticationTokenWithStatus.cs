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

namespace cloud.charging.open.protocols.WWCP
{

    /// <summary>
    /// An authentication token with its current status.
    /// </summary>
    public readonly struct AuthenticationTokenWithStatus : IEquatable<AuthenticationTokenWithStatus>,
                                                           IComparable<AuthenticationTokenWithStatus>
    {

        #region Properties

        /// <summary>
        /// The authentication token.
        /// </summary>
        public AuthenticationToken        AuthenticationToken    { get; }

        /// <summary>
        /// The current status of the authentication token.
        /// </summary>
        public AuthenticationTokenStatus  Status                 { get; }

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new authentication token with its current status.
        /// </summary>
        /// <param name="AuthenticationToken">An authentication token.</param>
        /// <param name="Status">The current status of the authentication token.</param>
        public AuthenticationTokenWithStatus(AuthenticationToken        AuthenticationToken,
                                             AuthenticationTokenStatus  Status)

        {

            this.AuthenticationToken  = AuthenticationToken;
            this.Status               = Status;

        }

        #endregion


        #region Operator overloading

        #region Operator == (AuthenticationTokenWithStatus1, AuthenticationTokenWithStatus2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="AuthenticationTokenWithStatus1">An authentication token with status.</param>
        /// <param name="AuthenticationTokenWithStatus2">Another authentication token with status.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator == (AuthenticationTokenWithStatus AuthenticationTokenWithStatus1,
                                           AuthenticationTokenWithStatus AuthenticationTokenWithStatus2)

            => AuthenticationTokenWithStatus1.Equals(AuthenticationTokenWithStatus2);

        #endregion

        #region Operator != (AuthenticationTokenWithStatus1, AuthenticationTokenWithStatus2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="AuthenticationTokenWithStatus1">An authentication token with status.</param>
        /// <param name="AuthenticationTokenWithStatus2">Another authentication token with status.</param>
        /// <returns>False if both match; True otherwise.</returns>
        public static Boolean operator != (AuthenticationTokenWithStatus AuthenticationTokenWithStatus1,
                                           AuthenticationTokenWithStatus AuthenticationTokenWithStatus2)

            => !(AuthenticationTokenWithStatus1 == AuthenticationTokenWithStatus2);

        #endregion

        #region Operator <  (AuthenticationTokenWithStatus1, AuthenticationTokenWithStatus2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="AuthenticationTokenWithStatus1">An authentication token with status.</param>
        /// <param name="AuthenticationTokenWithStatus2">Another authentication token with status.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator < (AuthenticationTokenWithStatus AuthenticationTokenWithStatus1,
                                          AuthenticationTokenWithStatus AuthenticationTokenWithStatus2)

            => AuthenticationTokenWithStatus1.CompareTo(AuthenticationTokenWithStatus2) < 0;

        #endregion

        #region Operator <= (AuthenticationTokenWithStatus1, AuthenticationTokenWithStatus2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="AuthenticationTokenWithStatus1">An authentication token with status.</param>
        /// <param name="AuthenticationTokenWithStatus2">Another authentication token with status.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator <= (AuthenticationTokenWithStatus AuthenticationTokenWithStatus1,
                                           AuthenticationTokenWithStatus AuthenticationTokenWithStatus2)

            => !(AuthenticationTokenWithStatus1 > AuthenticationTokenWithStatus2);

        #endregion

        #region Operator >  (AuthenticationTokenWithStatus1, AuthenticationTokenWithStatus2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="AuthenticationTokenWithStatus1">An authentication token with status.</param>
        /// <param name="AuthenticationTokenWithStatus2">Another authentication token with status.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator > (AuthenticationTokenWithStatus AuthenticationTokenWithStatus1,
                                          AuthenticationTokenWithStatus AuthenticationTokenWithStatus2)

            => AuthenticationTokenWithStatus1.CompareTo(AuthenticationTokenWithStatus2) > 0;

        #endregion

        #region Operator >= (AuthenticationTokenWithStatus1, AuthenticationTokenWithStatus2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="AuthenticationTokenWithStatus1">An authentication token with status.</param>
        /// <param name="AuthenticationTokenWithStatus2">Another authentication token with status.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator >= (AuthenticationTokenWithStatus AuthenticationTokenWithStatus1,
                                           AuthenticationTokenWithStatus AuthenticationTokenWithStatus2)

            => !(AuthenticationTokenWithStatus1 < AuthenticationTokenWithStatus2);

        #endregion

        #endregion

        #region IComparable<AuthenticationTokenWithStatus> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two authentication token with its current status.
        /// </summary>
        /// <param name="Object">An authentication token with its current status to compare with.</param>
        public Int32 CompareTo(Object? Object)

            => Object is AuthenticationTokenWithStatus authenticationTokenWithStatus
                   ? CompareTo(authenticationTokenWithStatus)
                   : throw new ArgumentException("The given object is not an authentication token with its current status!",
                                                 nameof(Object));

        #endregion

        #region CompareTo(AuthenticationTokenWithStatus)

        /// <summary>
        /// Compares two authentication token with its current status.
        /// </summary>
        /// <param name="AuthenticationTokenWithStatus">An authentication token with its current status to compare with.</param>
        public Int32 CompareTo(AuthenticationTokenWithStatus AuthenticationTokenWithStatus)
        {

            var c = AuthenticationToken.CompareTo(AuthenticationTokenWithStatus.AuthenticationToken);

            if (c == 0)
                c = Status.             CompareTo(AuthenticationTokenWithStatus.Status);

            return c;

        }

        #endregion

        #endregion

        #region IEquatable<AuthenticationTokenWithStatus> Members

        #region Equals(Object)

        /// <summary>
        /// Compares two authentication token with its current status for equality.
        /// </summary>
        /// <param name="Object">An authentication token with its current status to compare with.</param>
        public override Boolean Equals(Object? Object)

            => Object is AuthenticationTokenWithStatus authenticationTokenWithStatus &&
                   Equals(authenticationTokenWithStatus);

        #endregion

        #region Equals(AuthenticationTokenWithStatus)

        /// <summary>
        /// Compares two authentication token with its current status for equality.
        /// </summary>
        /// <param name="AuthenticationTokenWithStatus">An authentication token with its current status to compare with.</param>
        public Boolean Equals(AuthenticationTokenWithStatus AuthenticationTokenWithStatus)

            => AuthenticationToken.Equals(AuthenticationTokenWithStatus.AuthenticationToken) &&
               Status.             Equals(AuthenticationTokenWithStatus.Status);

        #endregion

        #endregion

        #region (override) GetHashCode()

        /// <summary>
        /// Return the HashCode of this object.
        /// </summary>
        public override Int32 GetHashCode()
        {
            unchecked
            {
                return AuthenticationToken.GetHashCode() * 3 ^
                       Status.             GetHashCode();
            }
        }

        #endregion

        #region (override) ToString()

        /// <summary>
        /// Return a text representation of this object.
        /// ISO-IEC-15118 – Annex H "Specification of Identifiers"
        /// </summary>
        public override String ToString()

            => String.Concat(

                   AuthenticationToken,
                   " -> ",
                   Status

               );

        #endregion

    }

}
