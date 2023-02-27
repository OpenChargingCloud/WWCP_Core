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

using org.GraphDefined.Vanaheimr.Illias;

#endregion

namespace cloud.charging.open.protocols.WWCP
{

    /// <summary>
    /// Extension methods for socket outlet identifications.
    /// </summary>
    public static class SocketOutletIdExtensions
    {

        /// <summary>
        /// Indicates whether this socket outlet identification is null or empty.
        /// </summary>
        /// <param name="SocketOutletId">A socket outlet identification.</param>
        public static Boolean IsNullOrEmpty(this SocketOutlet_Id? SocketOutletId)
            => !SocketOutletId.HasValue || SocketOutletId.Value.IsNullOrEmpty;

        /// <summary>
        /// Indicates whether this socket outlet identification is NOT null or empty.
        /// </summary>
        /// <param name="SocketOutletId">A socket outlet identification.</param>
        public static Boolean IsNotNullOrEmpty(this SocketOutlet_Id? SocketOutletId)
            => SocketOutletId.HasValue && SocketOutletId.Value.IsNotNullOrEmpty;

    }


    /// <summary>
    /// The unique identification of a socket outlet.
    /// CiString(3)
    /// </summary>
    public readonly struct SocketOutlet_Id : IId<SocketOutlet_Id>
    {

        #region Data

        /// <summary>
        /// The internal identification.
        /// </summary>
        private readonly String InternalId;

        #endregion

        #region Properties

        /// <summary>
        /// Indicates whether this socket outlet identification is null or empty.
        /// </summary>
        public Boolean IsNullOrEmpty
            => InternalId.IsNullOrEmpty();

        /// <summary>
        /// Indicates whether this socket outlet identification is NOT null or empty.
        /// </summary>
        public Boolean IsNotNullOrEmpty
            => InternalId.IsNotNullOrEmpty();

        /// <summary>
        /// The length of the socket outlet identification.
        /// </summary>
        public UInt64 Length
            => (UInt64) InternalId.Length;

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new socket outlet identification based on the given text.
        /// </summary>
        /// <param name="Text">A text representation of a socket outlet identification.</param>
        private SocketOutlet_Id(String Text)
        {
            this.InternalId = Text;
        }

        #endregion


        #region (static) Parse   (Text)

        /// <summary>
        /// Parse the given text as a socket outlet identification.
        /// </summary>
        /// <param name="Text">A text representation of a socket outlet identification.</param>
        public static SocketOutlet_Id Parse(String Text)
        {

            if (TryParse(Text, out var socketOutletId))
                return socketOutletId;

            throw new ArgumentException("Invalid text representation of a socket outlet identification: '" + Text + "'!",
                                        nameof(Text));

        }

        #endregion

        #region (static) TryParse(Text)

        /// <summary>
        /// Try to parse the given text as a socket outlet identification.
        /// </summary>
        /// <param name="Text">A text representation of a socket outlet identification.</param>
        public static SocketOutlet_Id? TryParse(String Text)
        {

            if (TryParse(Text, out var socketOutletId))
                return socketOutletId;

            return null;

        }

        #endregion

        #region (static) TryParse(Text, out SocketOutletId)

        /// <summary>
        /// Try to parse the given text as a socket outlet identification.
        /// </summary>
        /// <param name="Text">A text representation of a socket outlet identification.</param>
        /// <param name="SocketOutletId">The parsed socket outlet identification.</param>
        public static Boolean TryParse(String Text, out SocketOutlet_Id SocketOutletId)
        {

            Text = Text.Trim();

            if (Text.IsNotNullOrEmpty() &&
                Text.Length >= 1        &&
                Text.Length <= 3)
            {
                try
                {
                    SocketOutletId = new SocketOutlet_Id(Text);
                    return true;
                }
                catch (Exception)
                { }
            }

            SocketOutletId = default;
            return false;

        }

        #endregion

        #region Clone

        /// <summary>
        /// Clone this socket outlet identification.
        /// </summary>
        public SocketOutlet_Id Clone

            => new (
                   new String(InternalId?.ToCharArray())
               );

        #endregion


        #region Operator overloading

        #region Operator == (SocketOutletId1, SocketOutletId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="SocketOutletId1">A socket outlet identification.</param>
        /// <param name="SocketOutletId2">Another socket outlet identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator == (SocketOutlet_Id SocketOutletId1,
                                           SocketOutlet_Id SocketOutletId2)

            => SocketOutletId1.Equals(SocketOutletId2);

        #endregion

        #region Operator != (SocketOutletId1, SocketOutletId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="SocketOutletId1">A socket outlet identification.</param>
        /// <param name="SocketOutletId2">Another socket outlet identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator != (SocketOutlet_Id SocketOutletId1,
                                           SocketOutlet_Id SocketOutletId2)

            => !SocketOutletId1.Equals(SocketOutletId2);

        #endregion

        #region Operator <  (SocketOutletId1, SocketOutletId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="SocketOutletId1">A socket outlet identification.</param>
        /// <param name="SocketOutletId2">Another socket outlet identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator < (SocketOutlet_Id SocketOutletId1,
                                          SocketOutlet_Id SocketOutletId2)

            => SocketOutletId1.CompareTo(SocketOutletId2) < 0;

        #endregion

        #region Operator <= (SocketOutletId1, SocketOutletId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="SocketOutletId1">A socket outlet identification.</param>
        /// <param name="SocketOutletId2">Another socket outlet identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator <= (SocketOutlet_Id SocketOutletId1,
                                           SocketOutlet_Id SocketOutletId2)

            => SocketOutletId1.CompareTo(SocketOutletId2) <= 0;

        #endregion

        #region Operator >  (SocketOutletId1, SocketOutletId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="SocketOutletId1">A socket outlet identification.</param>
        /// <param name="SocketOutletId2">Another socket outlet identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator > (SocketOutlet_Id SocketOutletId1,
                                          SocketOutlet_Id SocketOutletId2)

            => SocketOutletId1.CompareTo(SocketOutletId2) > 0;

        #endregion

        #region Operator >= (SocketOutletId1, SocketOutletId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="SocketOutletId1">A socket outlet identification.</param>
        /// <param name="SocketOutletId2">Another socket outlet identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator >= (SocketOutlet_Id SocketOutletId1,
                                           SocketOutlet_Id SocketOutletId2)

            => SocketOutletId1.CompareTo(SocketOutletId2) >= 0;

        #endregion

        #endregion

        #region IComparable<SocketOutletId> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two socket outlet identifications.
        /// </summary>
        /// <param name="Object">A socket outlet identification to compare with.</param>
        public Int32 CompareTo(Object? Object)

            => Object is SocketOutlet_Id socketOutletId
                   ? CompareTo(socketOutletId)
                   : throw new ArgumentException("The given object is not a socket outlet identification!",
                                                 nameof(Object));

        #endregion

        #region CompareTo(SocketOutletId)

        /// <summary>
        /// Compares two socket outlet identifications.
        /// </summary>
        /// <param name="SocketOutletId">A socket outlet identification to compare with.</param>
        public Int32 CompareTo(SocketOutlet_Id SocketOutletId)

            => String.Compare(InternalId,
                              SocketOutletId.InternalId,
                              StringComparison.OrdinalIgnoreCase);

        #endregion

        #endregion

        #region IEquatable<SocketOutletId> Members

        #region Equals(Object)

        /// <summary>
        /// Compares two socket outlet identifications for equality.
        /// </summary>
        /// <param name="Object">A socket outlet identification to compare with.</param>
        public override Boolean Equals(Object? Object)

            => Object is SocketOutlet_Id socketOutletId &&
                   Equals(socketOutletId);

        #endregion

        #region Equals(SocketOutletId)

        /// <summary>
        /// Compares two socket outlet identifications for equality.
        /// </summary>
        /// <param name="SocketOutletId">A socket outlet identification to compare with.</param>
        public Boolean Equals(SocketOutlet_Id SocketOutletId)

            => String.Equals(InternalId,
                             SocketOutletId.InternalId,
                             StringComparison.OrdinalIgnoreCase);

        #endregion

        #endregion

        #region GetHashCode()

        /// <summary>
        /// Return the hash code of this object.
        /// </summary>
        /// <returns>The hash code of this object.</returns>
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
