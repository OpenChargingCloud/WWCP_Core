/*
 * Copyright (c) 2014-2024 GraphDefined GmbH <achim.friedland@graphdefined.com> <achim.friedland@graphdefined.com>
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
    /// The current status of an auth info.
    /// </summary>
    public class AuthInfoStatus : IEquatable<AuthInfoStatus>,
                                  IComparable<AuthInfoStatus>
    {

        #region Properties

        /// <summary>
        /// The unique identification of an auth info.
        /// </summary>
        public AAuthentication     Id        { get; }

        /// <summary>
        /// The current status of an auth info.
        /// </summary>
        public AuthInfoStatusType  Status    { get; }

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new auth info status.
        /// </summary>
        /// <param name="AuthInfo">The unique identification of an auth info.</param>
        /// <param name="Status">The current status of an auth info.</param>
        public AuthInfoStatus(AAuthentication     AuthInfo,
                              AuthInfoStatusType  Status)

        {

            this.Id      = AuthInfo ?? throw new ArgumentNullException(nameof(AuthInfo), "The given unique identification of an auth info must not be null!");
            this.Status  = Status;

        }

        #endregion


        #region Operator overloading

        #region Operator == (AuthInfoStatus1, AuthInfoStatus2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="AuthInfoStatus1">A AuthInfoStatus.</param>
        /// <param name="AuthInfoStatus2">Another AuthInfoStatus.</param>
        /// <returns>true|false</returns>
        public static Boolean operator ==(AuthInfoStatus AuthInfoStatus1, AuthInfoStatus AuthInfoStatus2)
        {

            // If both are null, or both are same instance, return true.
            if (ReferenceEquals(AuthInfoStatus1, AuthInfoStatus2))
                return true;

            // If one is null, but not both, return false.
            if (((Object)AuthInfoStatus1 == null) || ((Object)AuthInfoStatus2 == null))
                return false;

            return AuthInfoStatus1.Equals(AuthInfoStatus2);

        }

        #endregion

        #region Operator != (AuthInfoStatus1, AuthInfoStatus2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="AuthInfoStatus1">A AuthInfoStatus.</param>
        /// <param name="AuthInfoStatus2">Another AuthInfoStatus.</param>
        /// <returns>true|false</returns>
        public static Boolean operator !=(AuthInfoStatus AuthInfoStatus1, AuthInfoStatus AuthInfoStatus2)
        {
            return !(AuthInfoStatus1 == AuthInfoStatus2);
        }

        #endregion

        #region Operator <  (AuthInfoStatus1, AuthInfoStatus2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="AuthInfoStatus1">A AuthInfoStatus.</param>
        /// <param name="AuthInfoStatus2">Another AuthInfoStatus.</param>
        /// <returns>true|false</returns>
        public static Boolean operator <(AuthInfoStatus AuthInfoStatus1, AuthInfoStatus AuthInfoStatus2)
        {

            if ((Object)AuthInfoStatus1 == null)
                throw new ArgumentNullException("The given AuthInfoStatus1 must not be null!");

            return AuthInfoStatus1.CompareTo(AuthInfoStatus2) < 0;

        }

        #endregion

        #region Operator <= (AuthInfoStatus1, AuthInfoStatus2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="AuthInfoStatus1">A AuthInfoStatus.</param>
        /// <param name="AuthInfoStatus2">Another AuthInfoStatus.</param>
        /// <returns>true|false</returns>
        public static Boolean operator <=(AuthInfoStatus AuthInfoStatus1, AuthInfoStatus AuthInfoStatus2)
        {
            return !(AuthInfoStatus1 > AuthInfoStatus2);
        }

        #endregion

        #region Operator >  (AuthInfoStatus1, AuthInfoStatus2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="AuthInfoStatus1">A AuthInfoStatus.</param>
        /// <param name="AuthInfoStatus2">Another AuthInfoStatus.</param>
        /// <returns>true|false</returns>
        public static Boolean operator >(AuthInfoStatus AuthInfoStatus1, AuthInfoStatus AuthInfoStatus2)
        {

            if ((Object)AuthInfoStatus1 == null)
                throw new ArgumentNullException("The given AuthInfoStatus1 must not be null!");

            return AuthInfoStatus1.CompareTo(AuthInfoStatus2) > 0;

        }

        #endregion

        #region Operator >= (AuthInfoStatus1, AuthInfoStatus2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="AuthInfoStatus1">A AuthInfoStatus.</param>
        /// <param name="AuthInfoStatus2">Another AuthInfoStatus.</param>
        /// <returns>true|false</returns>
        public static Boolean operator >=(AuthInfoStatus AuthInfoStatus1, AuthInfoStatus AuthInfoStatus2)
        {
            return !(AuthInfoStatus1 < AuthInfoStatus2);
        }

        #endregion

        #endregion

        #region IComparable<AuthInfoStatus> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        public Int32 CompareTo(Object Object)
        {

            if (Object == null)
                throw new ArgumentNullException("The given object must not be null!");

            // Check if the given object is an AuthInfoStatus.
            var AuthInfoStatus = Object as AuthInfoStatus;
            if ((Object)AuthInfoStatus == null)
                throw new ArgumentException("The given object is not a AuthInfoStatus!");

            return CompareTo(AuthInfoStatus);

        }

        #endregion

        #region CompareTo(AuthInfoStatus)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="AuthInfoStatus">An object to compare with.</param>
        public Int32 CompareTo(AuthInfoStatus AuthInfoStatus)
        {

            if ((Object)AuthInfoStatus == null)
                throw new ArgumentNullException("The given AuthInfoStatus must not be null!");

            // Compare EVSE Ids
            var _Result = Id.CompareTo(AuthInfoStatus.Id);

            // If equal: Compare EVSE status
            if (_Result == 0)
                _Result = Status.CompareTo(AuthInfoStatus.Status);

            return _Result;

        }

        #endregion

        #endregion

        #region IEquatable<AuthInfoStatus> Members

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

            // Check if the given object is an AuthInfoStatus.
            var AuthInfoStatus = Object as AuthInfoStatus;
            if ((Object)AuthInfoStatus == null)
                return false;

            return this.Equals(AuthInfoStatus);

        }

        #endregion

        #region Equals(AuthInfoStatus)

        /// <summary>
        /// Compares two EVSE identifications for equality.
        /// </summary>
        /// <param name="AuthInfoStatus">An EVSE identification to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public Boolean Equals(AuthInfoStatus AuthInfoStatus)
        {

            if ((Object)AuthInfoStatus == null)
                return false;

            return Id.Equals(AuthInfoStatus.Id) &&
                   Status.Equals(AuthInfoStatus.Status);

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
                return Id.GetHashCode() * 17 ^ Status.GetHashCode();
            }
        }

        #endregion

        #region (override) ToString()

        /// <summary>
        /// Return a text representation of this object.
        /// ISO-IEC-15118 – Annex H "Specification of Identifiers"
        /// </summary>
        public override String ToString()
        {

            return String.Concat(Id, " -> ", Status.ToString());

        }

        #endregion

    }


    /// <summary>
    /// The current status of an auth info.
    /// </summary>
    public enum AuthInfoStatusType
    {

        /// <summary>
        /// Unclear or unknown status of the auth info.
        /// </summary>
        Unspecified  = 0,

        /// <summary>
        /// The auth info is invalid.
        /// </summary>
        Invalid      = 1,

        /// <summary>
        /// The processing of the auth info led to an error.
        /// </summary>
        Error        = 2,

        /// <summary>
        /// The auth info was not found.
        /// </summary>
        NotFound     = 3

    }

}