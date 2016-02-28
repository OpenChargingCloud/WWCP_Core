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
    /// The current status of an auth token.
    /// </summary>
    public class AuthTokenStatus : IEquatable<AuthTokenStatus>,
                                   IComparable<AuthTokenStatus>
    {

        #region Properties

        #region AuthToken

        private readonly Auth_Token _AuthToken;

        /// <summary>
        /// The unique identification of an auth token.
        /// </summary>
        public Auth_Token AuthToken
        {
            get
            {
                return _AuthToken;
            }
        }

        #endregion

        #region Status

        private readonly AuthTokenStatusType _Status;

        /// <summary>
        /// The current status of an auth token.
        /// </summary>
        public AuthTokenStatusType Status
        {
            get
            {
                return _Status;
            }
        }

        #endregion

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new auth token status.
        /// </summary>
        /// <param name="AuthToken">The unique identification of an auth token.</param>
        /// <param name="Status">The current status of an auth token.</param>
        public AuthTokenStatus(Auth_Token           AuthToken,
                               AuthTokenStatusType  Status)

        {

            #region Initial checks

            if (AuthToken == null)
                throw new ArgumentNullException(nameof(AuthToken), "The given unique identification of an auth token must not be null!");

            #endregion

            this._AuthToken  = AuthToken;
            this._Status     = Status;

        }

        #endregion


        #region Operator overloading

        #region Operator == (AuthTokenStatus1, AuthTokenStatus2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="AuthTokenStatus1">A AuthTokenStatus.</param>
        /// <param name="AuthTokenStatus2">Another AuthTokenStatus.</param>
        /// <returns>true|false</returns>
        public static Boolean operator ==(AuthTokenStatus AuthTokenStatus1, AuthTokenStatus AuthTokenStatus2)
        {

            // If both are null, or both are same instance, return true.
            if (Object.ReferenceEquals(AuthTokenStatus1, AuthTokenStatus2))
                return true;

            // If one is null, but not both, return false.
            if (((Object)AuthTokenStatus1 == null) || ((Object)AuthTokenStatus2 == null))
                return false;

            return AuthTokenStatus1.Equals(AuthTokenStatus2);

        }

        #endregion

        #region Operator != (AuthTokenStatus1, AuthTokenStatus2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="AuthTokenStatus1">A AuthTokenStatus.</param>
        /// <param name="AuthTokenStatus2">Another AuthTokenStatus.</param>
        /// <returns>true|false</returns>
        public static Boolean operator !=(AuthTokenStatus AuthTokenStatus1, AuthTokenStatus AuthTokenStatus2)
        {
            return !(AuthTokenStatus1 == AuthTokenStatus2);
        }

        #endregion

        #region Operator <  (AuthTokenStatus1, AuthTokenStatus2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="AuthTokenStatus1">A AuthTokenStatus.</param>
        /// <param name="AuthTokenStatus2">Another AuthTokenStatus.</param>
        /// <returns>true|false</returns>
        public static Boolean operator <(AuthTokenStatus AuthTokenStatus1, AuthTokenStatus AuthTokenStatus2)
        {

            if ((Object)AuthTokenStatus1 == null)
                throw new ArgumentNullException("The given AuthTokenStatus1 must not be null!");

            return AuthTokenStatus1.CompareTo(AuthTokenStatus2) < 0;

        }

        #endregion

        #region Operator <= (AuthTokenStatus1, AuthTokenStatus2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="AuthTokenStatus1">A AuthTokenStatus.</param>
        /// <param name="AuthTokenStatus2">Another AuthTokenStatus.</param>
        /// <returns>true|false</returns>
        public static Boolean operator <=(AuthTokenStatus AuthTokenStatus1, AuthTokenStatus AuthTokenStatus2)
        {
            return !(AuthTokenStatus1 > AuthTokenStatus2);
        }

        #endregion

        #region Operator >  (AuthTokenStatus1, AuthTokenStatus2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="AuthTokenStatus1">A AuthTokenStatus.</param>
        /// <param name="AuthTokenStatus2">Another AuthTokenStatus.</param>
        /// <returns>true|false</returns>
        public static Boolean operator >(AuthTokenStatus AuthTokenStatus1, AuthTokenStatus AuthTokenStatus2)
        {

            if ((Object)AuthTokenStatus1 == null)
                throw new ArgumentNullException("The given AuthTokenStatus1 must not be null!");

            return AuthTokenStatus1.CompareTo(AuthTokenStatus2) > 0;

        }

        #endregion

        #region Operator >= (AuthTokenStatus1, AuthTokenStatus2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="AuthTokenStatus1">A AuthTokenStatus.</param>
        /// <param name="AuthTokenStatus2">Another AuthTokenStatus.</param>
        /// <returns>true|false</returns>
        public static Boolean operator >=(AuthTokenStatus AuthTokenStatus1, AuthTokenStatus AuthTokenStatus2)
        {
            return !(AuthTokenStatus1 < AuthTokenStatus2);
        }

        #endregion

        #endregion

        #region IComparable<AuthTokenStatus> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        public Int32 CompareTo(Object Object)
        {

            if (Object == null)
                throw new ArgumentNullException("The given object must not be null!");

            // Check if the given object is an AuthTokenStatus.
            var AuthTokenStatus = Object as AuthTokenStatus;
            if ((Object)AuthTokenStatus == null)
                throw new ArgumentException("The given object is not a AuthTokenStatus!");

            return CompareTo(AuthTokenStatus);

        }

        #endregion

        #region CompareTo(AuthTokenStatus)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="AuthTokenStatus">An object to compare with.</param>
        public Int32 CompareTo(AuthTokenStatus AuthTokenStatus)
        {

            if ((Object)AuthTokenStatus == null)
                throw new ArgumentNullException("The given AuthTokenStatus must not be null!");

            // Compare EVSE Ids
            var _Result = _AuthToken.CompareTo(AuthTokenStatus._AuthToken);

            // If equal: Compare EVSE status
            if (_Result == 0)
                _Result = _Status.CompareTo(AuthTokenStatus._Status);

            return _Result;

        }

        #endregion

        #endregion

        #region IEquatable<AuthTokenStatus> Members

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

            // Check if the given object is an AuthTokenStatus.
            var AuthTokenStatus = Object as AuthTokenStatus;
            if ((Object)AuthTokenStatus == null)
                return false;

            return this.Equals(AuthTokenStatus);

        }

        #endregion

        #region Equals(AuthTokenStatus)

        /// <summary>
        /// Compares two EVSE identifications for equality.
        /// </summary>
        /// <param name="AuthTokenStatus">An EVSE identification to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public Boolean Equals(AuthTokenStatus AuthTokenStatus)
        {

            if ((Object)AuthTokenStatus == null)
                return false;

            return _AuthToken.Equals(AuthTokenStatus._AuthToken) &&
                   _Status.Equals(AuthTokenStatus._Status);

        }

        #endregion

        #endregion

        #region (override) GetHashCode()

        /// <summary>
        /// Return the HashCode of this object.
        /// </summary>
        /// <returns>The HashCode of this object.</returns>
        public override Int32 GetHashCode()
        {
            unchecked
            {
                return _AuthToken.GetHashCode() * 17 ^ _Status.GetHashCode();
            }
        }

        #endregion

        #region (override) ToString()

        /// <summary>
        /// Return a string representation of this object.
        /// ISO-IEC-15118 – Annex H "Specification of Identifiers"
        /// </summary>
        public override String ToString()
        {

            return String.Concat(_AuthToken, " -> ", _Status.ToString());

        }

        #endregion

    }


    /// <summary>
    /// The current status of an auth token.
    /// </summary>
    public enum AuthTokenStatusType
    {

        /// <summary>
        /// Unclear or unknown status of the auth token.
        /// </summary>
        Unspecified  = 0,

        /// <summary>
        /// The auth token is invalid.
        /// </summary>
        Invalid      = 1,

        /// <summary>
        /// The processing of the auth token led to an error.
        /// </summary>
        Error        = 2

    }

}