﻿/*
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
    /// A delegate for filtering authorizator identifications.
    /// </summary>
    /// <param name="AuthorizatorId">An authorizator identification to include.</param>
    public delegate Boolean IncludeAuthorizatorIdDelegate(Authorizator_Id AuthorizatorId);


    /// <summary>
    /// Extension methods for authorizator identifications.
    /// </summary>
    public static class AuthorizatorIdExtensions
    {

        /// <summary>
        /// Indicates whether this authorizator identification is null or empty.
        /// </summary>
        /// <param name="AuthorizatorId">An authorizator identification.</param>
        public static Boolean IsNullOrEmpty(this Authorizator_Id? AuthorizatorId)
            => !AuthorizatorId.HasValue || AuthorizatorId.Value.IsNullOrEmpty;

        /// <summary>
        /// Indicates whether this authorizator identification is NOT null or empty.
        /// </summary>
        /// <param name="AuthorizatorId">An authorizator identification.</param>
        public static Boolean IsNotNullOrEmpty(this Authorizator_Id? AuthorizatorId)
            => AuthorizatorId.HasValue && AuthorizatorId.Value.IsNotNullOrEmpty;

    }


    /// <summary>
    /// The unique identification of an authorizator.
    /// </summary>
    public readonly struct Authorizator_Id : IId,
                                             IEquatable <Authorizator_Id>,
                                             IComparable<Authorizator_Id>

    {

        #region Data

        /// <summary>
        /// The internal identification.
        /// </summary>
        private readonly String InternalId;

        #endregion

        #region Properties

        /// <summary>
        /// Indicates whether this authorizator identification is null or empty.
        /// </summary>
        public Boolean IsNullOrEmpty
            => InternalId.IsNullOrEmpty();

        /// <summary>
        /// Indicates whether this authorizator identification is NOT null or empty.
        /// </summary>
        public Boolean IsNotNullOrEmpty
            => InternalId.IsNotNullOrEmpty();

        /// <summary>
        /// The length of the authorizator identification.
        /// </summary>
        public UInt64 Length
            => (UInt64) (InternalId?.Length ?? 0);

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new authorizator identification based on the given string.
        /// </summary>
        /// <param name="Text">A text representation of an authorizator identification.</param>
        private Authorizator_Id(String Text)
        {
            InternalId = Text;
        }

        #endregion


        #region (static) Parse   (Text)

        /// <summary>
        /// Parse the given string as an authorizator identification.
        /// </summary>
        /// <param name="Text">A text representation of an authorizator identification.</param>
        public static Authorizator_Id Parse(String Text)
        {

            if (TryParse(Text, out var authorizatorId))
                return authorizatorId;

            throw new ArgumentException($"Invalid text representation of an authorizator identification: '{Text}'!",
                                        nameof(Text));

        }

        #endregion

        #region (static) TryParse(Text)

        /// <summary>
        /// Try to parse the given string as an authorizator identification.
        /// </summary>
        /// <param name="Text">A text representation of an authorizator identification.</param>
        public static Authorizator_Id? TryParse(String Text)
        {

            if (TryParse(Text, out var authorizatorId))
                return authorizatorId;

            return null;

        }

        #endregion

        #region (static) TryParse(Text, out AuthorizatorId)

        /// <summary>
        /// Parse the given text as an authorizator identification.
        /// </summary>
        /// <param name="Text">A text representation of an authorizator identification.</param>
        /// <param name="AuthorizatorId">The parsed authorizator identification.</param>
        public static Boolean TryParse(String Text, out Authorizator_Id AuthorizatorId)
        {

            Text = Text.Trim();

            if (Text.IsNotNullOrEmpty())
            {
                AuthorizatorId = new Authorizator_Id(Text);
                return true;
            }

            AuthorizatorId = default;
            return false;

        }

        #endregion

        #region Clone()

        /// <summary>
        /// Clone this authorizator identification.
        /// </summary>
        public Authorizator_Id Clone()

            => new (
                   InternalId.CloneString()
               );

        #endregion


        #region Operator overloading

        #region Operator == (AuthorizatorId1, AuthorizatorId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="AuthorizatorId1">An authorizator identification.</param>
        /// <param name="AuthorizatorId2">Another authorizator identification.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator == (Authorizator_Id AuthorizatorId1,
                                           Authorizator_Id AuthorizatorId2)

            => AuthorizatorId1.Equals(AuthorizatorId2);

        #endregion

        #region Operator != (AuthorizatorId1, AuthorizatorId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="AuthorizatorId1">An authorizator identification.</param>
        /// <param name="AuthorizatorId2">Another authorizator identification.</param>
        /// <returns>False if both match; True otherwise.</returns>
        public static Boolean operator != (Authorizator_Id AuthorizatorId1,
                                           Authorizator_Id AuthorizatorId2)

            => !AuthorizatorId1.Equals(AuthorizatorId2);

        #endregion

        #region Operator <  (AuthorizatorId1, AuthorizatorId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="AuthorizatorId1">An authorizator identification.</param>
        /// <param name="AuthorizatorId2">Another authorizator identification.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator < (Authorizator_Id AuthorizatorId1,
                                          Authorizator_Id AuthorizatorId2)

            => AuthorizatorId1.CompareTo(AuthorizatorId2) < 0;

        #endregion

        #region Operator <= (AuthorizatorId1, AuthorizatorId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="AuthorizatorId1">An authorizator identification.</param>
        /// <param name="AuthorizatorId2">Another authorizator identification.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator <= (Authorizator_Id AuthorizatorId1,
                                           Authorizator_Id AuthorizatorId2)

            => AuthorizatorId1.CompareTo(AuthorizatorId2) <= 0;

        #endregion

        #region Operator >  (AuthorizatorId1, AuthorizatorId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="AuthorizatorId1">An authorizator identification.</param>
        /// <param name="AuthorizatorId2">Another authorizator identification.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator > (Authorizator_Id AuthorizatorId1,
                                          Authorizator_Id AuthorizatorId2)

            => AuthorizatorId1.CompareTo(AuthorizatorId2) > 0;

        #endregion

        #region Operator >= (AuthorizatorId1, AuthorizatorId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="AuthorizatorId1">An authorizator identification.</param>
        /// <param name="AuthorizatorId2">Another authorizator identification.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator >= (Authorizator_Id AuthorizatorId1,
                                           Authorizator_Id AuthorizatorId2)

            => AuthorizatorId1.CompareTo(AuthorizatorId2) >= 0;

        #endregion

        #endregion

        #region IComparable<Authorizator_Id> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two authorizator identifications.
        /// </summary>
        /// <param name="Object">An authorizator identification to compare with.</param>
        public Int32 CompareTo(Object? Object)

            => Object is Authorizator_Id authorizatorId
                   ? CompareTo(authorizatorId)
                   : throw new ArgumentException("The given object is not an authorizator identification!",
                                                 nameof(Object));

        #endregion

        #region CompareTo(AuthorizatorId)

        /// <summary>
        /// Compares two authorizator identifications.
        /// </summary>
        /// <param name="AuthorizatorId">An authorizator identification to compare with.</param>
        public Int32 CompareTo(Authorizator_Id AuthorizatorId)

            => String.Compare(InternalId,
                              AuthorizatorId.InternalId,
                              StringComparison.OrdinalIgnoreCase);

        #endregion

        #endregion

        #region IEquatable<Authorizator_Id> Members

        #region Equals(Object)

        /// <summary>
        /// Compares two authorizator identifications for equality.
        /// </summary>
        /// <param name="Object">An authorizator identification to compare with.</param>
        public override Boolean Equals(Object? Object)

            => Object is Authorizator_Id authorizatorId &&
                   Equals(authorizatorId);

        #endregion

        #region Equals(AuthorizatorId)

        /// <summary>
        /// Compares two authorizator identifications for equality.
        /// </summary>
        /// <param name="AuthorizatorId">An authorizator identification to compare with.</param>
        public Boolean Equals(Authorizator_Id AuthorizatorId)

            => String.Equals(InternalId,
                             AuthorizatorId.InternalId,
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
