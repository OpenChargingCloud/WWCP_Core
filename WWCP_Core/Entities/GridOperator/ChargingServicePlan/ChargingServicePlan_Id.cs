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
    /// Extension methods for charging service plan identifications.
    /// </summary>
    public static class ChargingServicePlanIdExtensions
    {

        /// <summary>
        /// Indicates whether this charging service plan identification is null or empty.
        /// </summary>
        /// <param name="ChargingServicePlanId">A charging service plan identification.</param>
        public static Boolean IsNullOrEmpty(this ChargingServicePlan_Id? ChargingServicePlanId)
            => !ChargingServicePlanId.HasValue || ChargingServicePlanId.Value.IsNullOrEmpty;

        /// <summary>
        /// Indicates whether this charging service plan identification is null or empty.
        /// </summary>
        /// <param name="ChargingServicePlanId">A charging service plan identification.</param>
        public static Boolean IsNotNullOrEmpty(this ChargingServicePlan_Id? ChargingServicePlanId)
            => ChargingServicePlanId.HasValue && ChargingServicePlanId.Value.IsNotNullOrEmpty;

    }


    /// <summary>
    /// The unique identification of an Electric Vehicle Charging Service Plan (EVCSP Id).
    /// </summary>
    public readonly struct ChargingServicePlan_Id : IId,
                                                    IEquatable<ChargingServicePlan_Id>,
                                                    IComparable<ChargingServicePlan_Id>

    {

        #region Data

        /// <summary>
        /// The internal identification.
        /// </summary>
        private readonly String InternalId;

        #endregion

        #region Properties

        /// <summary>
        /// Indicates whether this charging service plan identification is null or empty.
        /// </summary>
        public Boolean IsNullOrEmpty
            => InternalId.IsNullOrEmpty();

        /// <summary>
        /// Indicates whether this charging service plan identification is NOT null or empty.
        /// </summary>
        public Boolean IsNotNullOrEmpty
            => InternalId.IsNotNullOrEmpty();

        /// <summary>
        /// The length of the charging service plan identificator.
        /// </summary>
        public UInt64 Length
            => (UInt64) (InternalId?.Length ?? 0);

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new charging service plan identification.
        /// based on the given string.
        /// </summary>
        private ChargingServicePlan_Id(String Text)
        {
            InternalId = Text;
        }

        #endregion


        #region (static) NewRandom

        /// <summary>
        /// Create a new random charging service plan identification.
        /// </summary>
        public static ChargingServicePlan_Id NewRandom
            => Parse(Guid.NewGuid().ToString());

        #endregion

        #region (static) Parse   (Text)

        /// <summary>
        /// Parse the given string as a charging service plan identification.
        /// </summary>
        /// <param name="Text">A text representation of a charging service plan identification.</param>
        public static ChargingServicePlan_Id Parse(String Text)
        {

            if (TryParse(Text, out ChargingServicePlan_Id sessionId))
                return sessionId;

            throw new ArgumentException($"Invalid text representation of a charging service plan identification: '{Text}'!",
                                        nameof(Text));

        }

        #endregion

        #region (static) TryParse(Text)

        /// <summary>
        /// Try to parse the given string as a charging service plan identification.
        /// </summary>
        /// <param name="Text">A text representation of a charging service plan identification.</param>
        public static ChargingServicePlan_Id? TryParse(String Text)
        {

            if (TryParse(Text, out ChargingServicePlan_Id sessionId))
                return sessionId;

            return null;

        }

        #endregion

        #region (static) TryParse(Text, out SessionId)

        /// <summary>
        /// Try to parse the given string as a charging service plan identification.
        /// </summary>
        /// <param name="Text">A text representation of a charging service plan identification.</param>
        /// <param name="SessionId">The parsed charging service plan identification.</param>
        public static Boolean TryParse(String Text, out ChargingServicePlan_Id SessionId)
        {

            if (Text.IsNotNullOrEmpty())
            {
                try
                {
                    SessionId = new ChargingServicePlan_Id(Text.Trim().SubstringMax(250));
                    return true;
                }
                catch
                { }
            }

            SessionId = default;
            return false;

        }

        #endregion

        #region Clone

        /// <summary>
        /// Clone this charging service plan identification.
        /// </summary>
        public ChargingServicePlan_Id Clone

            => new (
                   new String(InternalId?.ToCharArray())
               );

        #endregion


        #region Operator overloading

        #region Operator == (SessionId1, SessionId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="SessionId1">A charging service plan identification.</param>
        /// <param name="SessionId2">Another charging service plan identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator == (ChargingServicePlan_Id SessionId1,
                                           ChargingServicePlan_Id SessionId2)

            => SessionId1.Equals(SessionId2);

        #endregion

        #region Operator != (SessionId1, SessionId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="SessionId1">A charging service plan identification.</param>
        /// <param name="SessionId2">Another charging service plan identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator != (ChargingServicePlan_Id SessionId1,
                                           ChargingServicePlan_Id SessionId2)

            => !SessionId1.Equals(SessionId2);

        #endregion

        #region Operator <  (SessionId1, SessionId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="SessionId1">A charging service plan identification.</param>
        /// <param name="SessionId2">Another charging service plan identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator < (ChargingServicePlan_Id SessionId1,
                                          ChargingServicePlan_Id SessionId2)

            => SessionId1.CompareTo(SessionId2) < 0;

        #endregion

        #region Operator <= (SessionId1, SessionId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="SessionId1">A charging service plan identification.</param>
        /// <param name="SessionId2">Another charging service plan identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator <= (ChargingServicePlan_Id SessionId1,
                                           ChargingServicePlan_Id SessionId2)

            => SessionId1.CompareTo(SessionId2) <= 0;

        #endregion

        #region Operator >  (SessionId1, SessionId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="SessionId1">A charging service plan identification.</param>
        /// <param name="SessionId2">Another charging service plan identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator > (ChargingServicePlan_Id SessionId1,
                                          ChargingServicePlan_Id SessionId2)

            => SessionId1.CompareTo(SessionId2) > 0;

        #endregion

        #region Operator >= (SessionId1, SessionId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="SessionId1">A charging service plan identification.</param>
        /// <param name="SessionId2">Another charging service plan identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator >= (ChargingServicePlan_Id SessionId1,
                                           ChargingServicePlan_Id SessionId2)

            => SessionId1.CompareTo(SessionId2) >= 0;

        #endregion

        #endregion

        #region IComparable<ChargingServicePlan_Id> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        public Int32 CompareTo(Object? Object)

            => Object is ChargingServicePlan_Id sessionId
                   ? CompareTo(sessionId)
                   : throw new ArgumentException("The given object is not a charging service plan identification!");

        #endregion

        #region CompareTo(SessionId)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="SessionId">An object to compare with.</param>
        public Int32 CompareTo(ChargingServicePlan_Id SessionId)

            => String.Compare(InternalId,
                              SessionId.InternalId,
                              StringComparison.OrdinalIgnoreCase);

        #endregion

        #endregion

        #region IEquatable<ChargingServicePlan_Id> Members

        #region Equals(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        /// <returns>true|false</returns>
        public override Boolean Equals(Object? Object)

            => Object is ChargingServicePlan_Id sessionId
                   ? Equals(sessionId)
                   : false;

        #endregion

        #region Equals(SessionId)

        /// <summary>
        /// Compares two SessionIds for equality.
        /// </summary>
        /// <param name="SessionId">A charging service plan identification to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public Boolean Equals(ChargingServicePlan_Id SessionId)

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
