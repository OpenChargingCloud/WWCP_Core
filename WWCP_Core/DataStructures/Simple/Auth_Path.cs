/*
 * Copyright (c) 2014-2025 GraphDefined GmbH <achim.friedland@graphdefined.com>
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
    /// Extension methods for authentication paths.
    /// </summary>
    public static class AuthPathExtensions
    {

        /// <summary>
        /// Indicates whether this authentication path is null or empty.
        /// </summary>
        /// <param name="AuthPath">An authentication path.</param>
        public static Boolean IsNullOrEmpty(this Auth_Path? AuthPath)
            => !AuthPath.HasValue || AuthPath.Value.IsNullOrEmpty;

        /// <summary>
        /// Indicates whether this authentication path is NOT null or empty.
        /// </summary>
        /// <param name="AuthPath">An authentication path.</param>
        public static Boolean IsNotNullOrEmpty(this Auth_Path? AuthPath)
            => AuthPath.HasValue && AuthPath.Value.IsNotNullOrEmpty;

    }


    /// <summary>
    /// The unique identification of an authentication path.
    /// </summary>
    public readonly struct Auth_Path : IId<Auth_Path>
    {

        #region Data

        /// <summary>
        /// The internal identification.
        /// </summary>
        private readonly String InternalId;

        #endregion

        #region Properties

        /// <summary>
        /// Indicates whether this authentication path is null or empty.
        /// </summary>
        public Boolean IsNullOrEmpty
            => InternalId.IsNullOrEmpty();

        /// <summary>
        /// Indicates whether this authentication path is NOT null or empty.
        /// </summary>
        public Boolean IsNotNullOrEmpty
            => InternalId.IsNotNullOrEmpty();

        /// <summary>
        /// The length of the authentication path.
        /// </summary>
        public UInt64 Length
            => (UInt64) (InternalId?.Length ?? 0);

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new authentication path based on the given text.
        /// </summary>
        /// <param name="Text">The text representation of an authentication path.</param>
        private Auth_Path(String Text)
        {
            this.InternalId = Text;
        }

        #endregion


        #region (static) Parse   (Text)

        /// <summary>
        /// Parse the given text as an authentication path.
        /// </summary>
        /// <param name="Text">A text representation of an authentication path.</param>
        public static Auth_Path Parse(String Text)
        {

            if (TryParse(Text, out var authenticationPath))
                return authenticationPath;

            throw new ArgumentException($"Invalid text representation of an authentication path: '{Text}'!",
                                        nameof(Text));

        }

        #endregion

        #region (static) TryParse(Text)

        /// <summary>
        /// Try to parse the given text as an authentication path.
        /// </summary>
        /// <param name="Text">A text representation of an authentication path.</param>
        public static Auth_Path? TryParse(String Text)
        {

            if (TryParse(Text, out var authenticationPath))
                return authenticationPath;

            return null;

        }

        #endregion

        #region (static) TryParse(Text, out AuthPath)

        /// <summary>
        /// Try to parse the given text as an authentication path.
        /// </summary>
        /// <param name="Text">A text representation of an authentication path.</param>
        /// <param name="AuthPath">The parsed authentication path.</param>
        public static Boolean TryParse(String Text, out Auth_Path AuthPath)
        {

            Text = Text.Trim();

            if (Text.IsNotNullOrEmpty())
            {
                try
                {
                    AuthPath = new Auth_Path(Text);
                    return true;
                }
                catch
                { }
            }

            AuthPath = default;
            return false;

        }

        #endregion

        #region Clone()

        /// <summary>
        /// Clone this authentication path.
        /// </summary>
        public Auth_Path Clone()

            => new (
                   InternalId.CloneString()
               );

        #endregion


        #region Operator overloading

        #region Operator == (AuthPath1, AuthPath2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="AuthPath1">An authentication path.</param>
        /// <param name="AuthPath2">Another authentication path.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator == (Auth_Path AuthPath1,
                                           Auth_Path AuthPath2)

            => AuthPath1.Equals(AuthPath2);

        #endregion

        #region Operator != (AuthPath1, AuthPath2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="AuthPath1">An authentication path.</param>
        /// <param name="AuthPath2">Another authentication path.</param>
        /// <returns>False if both match; True otherwise.</returns>
        public static Boolean operator != (Auth_Path AuthPath1,
                                           Auth_Path AuthPath2)

            => !AuthPath1.Equals(AuthPath2);

        #endregion

        #region Operator <  (AuthPath1, AuthPath2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="AuthPath1">An authentication path.</param>
        /// <param name="AuthPath2">Another authentication path.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator < (Auth_Path AuthPath1,
                                          Auth_Path AuthPath2)

            => AuthPath1.CompareTo(AuthPath2) < 0;

        #endregion

        #region Operator <= (AuthPath1, AuthPath2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="AuthPath1">An authentication path.</param>
        /// <param name="AuthPath2">Another authentication path.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator <= (Auth_Path AuthPath1,
                                           Auth_Path AuthPath2)

            => AuthPath1.CompareTo(AuthPath2) <= 0;

        #endregion

        #region Operator >  (AuthPath1, AuthPath2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="AuthPath1">An authentication path.</param>
        /// <param name="AuthPath2">Another authentication path.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator > (Auth_Path AuthPath1,
                                          Auth_Path AuthPath2)

            => AuthPath1.CompareTo(AuthPath2) > 0;

        #endregion

        #region Operator >= (AuthPath1, AuthPath2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="AuthPath1">An authentication path.</param>
        /// <param name="AuthPath2">Another authentication path.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator >= (Auth_Path AuthPath1,
                                           Auth_Path AuthPath2)

            => AuthPath1.CompareTo(AuthPath2) >= 0;

        #endregion

        #endregion

        #region IComparable<AuthPath> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two authentication paths.
        /// </summary>
        /// <param name="Object">An authentication path to compare with.</param>
        public Int32 CompareTo(Object? Object)

            => Object is Auth_Path authenticationPath
                   ? CompareTo(authenticationPath)
                   : throw new ArgumentException("The given object is not an authentication path!",
                                                 nameof(Object));

        #endregion

        #region CompareTo(AuthPath)

        /// <summary>
        /// Compares two authentication paths.
        /// </summary>
        /// <param name="AuthPath">An authentication path to compare with.</param>
        public Int32 CompareTo(Auth_Path AuthPath)

            => String.Compare(InternalId,
                              AuthPath.InternalId,
                              StringComparison.OrdinalIgnoreCase);

        #endregion

        #endregion

        #region IEquatable<AuthPath> Members

        #region Equals(Object)

        /// <summary>
        /// Compares two authentication paths for equality.
        /// </summary>
        /// <param name="Object">An authentication path to compare with.</param>
        public override Boolean Equals(Object? Object)

            => Object is Auth_Path authenticationPath &&
                   Equals(authenticationPath);

        #endregion

        #region Equals(AuthPath)

        /// <summary>
        /// Compares two authentication paths for equality.
        /// </summary>
        /// <param name="AuthPath">An authentication path to compare with.</param>
        public Boolean Equals(Auth_Path AuthPath)

            => String.Equals(InternalId,
                             AuthPath.InternalId,
                             StringComparison.OrdinalIgnoreCase);

        #endregion

        #endregion

        #region (override) GetHashCode()

        /// <summary>
        /// Return the hash code of this object.
        /// </summary>
        public override Int32 GetHashCode()

            => InternalId?.ToLower().GetHashCode() ?? 0;

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
