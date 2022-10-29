/*
 * Copyright (c) 2014-2022 GraphDefined GmbH <achim.friedland@graphdefined.com>
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
    /// Extension methods for charging session identifications.
    /// </summary>
    public static class ChargingSessionIdExtensions
    {

        /// <summary>
        /// Indicates whether this charging session identification is null or empty.
        /// </summary>
        /// <param name="ChargingSessionId">A charging session identification.</param>
        public static Boolean IsNullOrEmpty(this ChargingSession_Id? ChargingSessionId)
            => !ChargingSessionId.HasValue || ChargingSessionId.Value.IsNullOrEmpty;

        /// <summary>
        /// Indicates whether this charging session identification is null or empty.
        /// </summary>
        /// <param name="ChargingSessionId">A charging session identification.</param>
        public static Boolean IsNotNullOrEmpty(this ChargingSession_Id? ChargingSessionId)
            => ChargingSessionId.HasValue && ChargingSessionId.Value.IsNotNullOrEmpty;

    }


    /// <summary>
    /// The unique identification of a charging session.
    /// </summary>
    public readonly struct ChargingSession_Id : IId,
                                                IEquatable<ChargingSession_Id>,
                                                IComparable<ChargingSession_Id>
    {

        #region Data

        /// <summary>
        /// The internal identification.
        /// </summary>
        private readonly String InternalId;

        #endregion

        #region Properties

        /// <summary>
        /// Indicates whether this charging session identification is null or empty.
        /// </summary>
        public Boolean IsNullOrEmpty
            => InternalId.IsNullOrEmpty();

        /// <summary>
        /// Indicates whether this charging session identification is NOT null or empty.
        /// </summary>
        public Boolean IsNotNullOrEmpty
            => InternalId.IsNotNullOrEmpty();

        /// <summary>
        /// The length of the charging session identificator.
        /// </summary>
        public UInt64 Length
            => (UInt64) (InternalId?.Length ?? 0);

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new charging session identification.
        /// based on the given string.
        /// </summary>
        private ChargingSession_Id(String Text)
        {
            InternalId = Text;
        }

        #endregion


        #region (static) NewRandom

        /// <summary>
        /// Create a new random charging session identification.
        /// </summary>
        public static ChargingSession_Id NewRandom

            => Parse(Guid.NewGuid().ToString());

        #endregion

        #region (static) Parse   (Text)

        /// <summary>
        /// Parse the given string as a charging session identification.
        /// </summary>
        /// <param name="Text">A text representation of a charging session identification.</param>
        public static ChargingSession_Id Parse(String Text)
        {

            if (TryParse(Text, out ChargingSession_Id chargingSessionId))
                return chargingSessionId;

            throw new ArgumentException("Invalid text representation of a charging session identification: '" + Text + "'!",
                                        nameof(Text));

        }

        #endregion

        #region (static) TryParse(Text)

        /// <summary>
        /// Try to parse the given string as a charging session identification.
        /// </summary>
        /// <param name="Text">A text representation of a charging session identification.</param>
        public static ChargingSession_Id? TryParse(String Text)
        {

            if (TryParse(Text, out ChargingSession_Id chargingSessionId))
                return chargingSessionId;

            return null;

        }

        #endregion

        #region (static) TryParse(Text, out SessionId)

        /// <summary>
        /// Try to parse the given string as a charging session identification.
        /// </summary>
        /// <param name="Text">A text representation of a charging session identification.</param>
        /// <param name="ChargingSessionId">The parsed charging session identification.</param>
        public static Boolean TryParse(String Text, out ChargingSession_Id ChargingSessionId)
        {

            if (Text.IsNotNullOrEmpty())
            {
                try
                {
                    ChargingSessionId = new ChargingSession_Id(Text.Trim().SubstringMax(250));
                    return true;
                }
                catch
                { }
            }

            ChargingSessionId = default;
            return false;

        }

        #endregion

        #region Clone

        /// <summary>
        /// Clone this charging session identification.
        /// </summary>
        public ChargingSession_Id Clone

            => new (
                   new String(InternalId?.ToCharArray())
               );

        #endregion


        #region Operator overloading

        #region Operator == (SessionId1, SessionId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="SessionId1">A charging session identification.</param>
        /// <param name="SessionId2">Another charging session identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator == (ChargingSession_Id SessionId1,
                                           ChargingSession_Id SessionId2)

            => SessionId1.Equals(SessionId2);

        #endregion

        #region Operator != (SessionId1, SessionId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="SessionId1">A charging session identification.</param>
        /// <param name="SessionId2">Another charging session identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator != (ChargingSession_Id SessionId1,
                                           ChargingSession_Id SessionId2)

            => !SessionId1.Equals(SessionId2);

        #endregion

        #region Operator <  (SessionId1, SessionId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="SessionId1">A charging session identification.</param>
        /// <param name="SessionId2">Another charging session identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator < (ChargingSession_Id SessionId1,
                                          ChargingSession_Id SessionId2)

            => SessionId1.CompareTo(SessionId2) < 0;

        #endregion

        #region Operator <= (SessionId1, SessionId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="SessionId1">A charging session identification.</param>
        /// <param name="SessionId2">Another charging session identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator <= (ChargingSession_Id SessionId1,
                                           ChargingSession_Id SessionId2)

            => SessionId1.CompareTo(SessionId2) <= 0;

        #endregion

        #region Operator >  (SessionId1, SessionId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="SessionId1">A charging session identification.</param>
        /// <param name="SessionId2">Another charging session identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator > (ChargingSession_Id SessionId1,
                                          ChargingSession_Id SessionId2)

            => SessionId1.CompareTo(SessionId2) > 0;

        #endregion

        #region Operator >= (SessionId1, SessionId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="SessionId1">A charging session identification.</param>
        /// <param name="SessionId2">Another charging session identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator >= (ChargingSession_Id SessionId1,
                                           ChargingSession_Id SessionId2)

            => SessionId1.CompareTo(SessionId2) >= 0;

        #endregion

        #endregion

        #region IComparable<ChargingSession_Id> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        public Int32 CompareTo(Object? Object)

            => Object is ChargingSession_Id sessionId
                   ? CompareTo(sessionId)
                   : throw new ArgumentException("The given object is not a charging session identification!");

        #endregion

        #region CompareTo(SessionId)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="SessionId">An object to compare with.</param>
        public Int32 CompareTo(ChargingSession_Id SessionId)

            => String.Compare(InternalId,
                              SessionId.InternalId,
                              StringComparison.OrdinalIgnoreCase);

        #endregion

        #endregion

        #region IEquatable<ChargingSession_Id> Members

        #region Equals(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        /// <returns>true|false</returns>
        public override Boolean Equals(Object? Object)

            => Object is ChargingSession_Id sessionId &&
                   Equals(sessionId);

        #endregion

        #region Equals(SessionId)

        /// <summary>
        /// Compares two SessionIds for equality.
        /// </summary>
        /// <param name="SessionId">A charging session identification to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public Boolean Equals(ChargingSession_Id SessionId)

            => String.Equals(InternalId,
                             SessionId.InternalId,
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
