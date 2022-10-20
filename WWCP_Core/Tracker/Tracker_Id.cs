/*
 * Copyright (c) 2014-2022 GraphDefined GmbH
 * This file is part of WWCP Tracker <https://github.com/OpenChargingCloud/WWCP_Tracker>
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

namespace cloud.charging.open.protocols.WWCP.Networking
{

    /// <summary>
    /// Extension methods for tracker identifications.
    /// </summary>
    public static class TrackerIdExtensions
    {

        /// <summary>
        /// Indicates whether this tracker identification is null or empty.
        /// </summary>
        /// <param name="TrackerId">A tracker identification.</param>
        public static Boolean IsNullOrEmpty(this Tracker_Id? TrackerId)
            => !TrackerId.HasValue || TrackerId.Value.IsNullOrEmpty;

        /// <summary>
        /// Indicates whether this tracker identification is null or empty.
        /// </summary>
        /// <param name="TrackerId">A tracker identification.</param>
        public static Boolean IsNotNullOrEmpty(this Tracker_Id? TrackerId)
            => TrackerId.HasValue && TrackerId.Value.IsNotNullOrEmpty;

    }


    /// <summary>
    /// The unique identification of a tracker.
    /// </summary>
    public readonly struct Tracker_Id : IId,
                                               IEquatable <Tracker_Id>,
                                               IComparable<Tracker_Id>

    {

        #region Data

        /// <summary>
        /// The internal identification.
        /// </summary>
        private readonly String InternalId;

        #endregion

        #region Properties

        /// <summary>
        /// Indicates whether this identification is null or empty.
        /// </summary>
        public Boolean IsNullOrEmpty
            => InternalId.IsNullOrEmpty();

        /// <summary>
        /// Indicates whether this identification is NOT null or empty.
        /// </summary>
        public Boolean IsNotNullOrEmpty
            => InternalId.IsNotNullOrEmpty();

        /// <summary>
        /// The length of the tracker identificator.
        /// </summary>
        public UInt64 Length
            => (UInt64) InternalId.Length;

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new tracker identification.
        /// based on the given string.
        /// </summary>
        private Tracker_Id(String Text)
        {
            InternalId = Text;
        }

        #endregion


        #region (static) Parse   (Text)

        /// <summary>
        /// Parse the given string as a tracker identification.
        /// </summary>
        /// <param name="Text">A text representation of a tracker identification.</param>
        public static Tracker_Id Parse(String Text)
        {

            if (TryParse(Text, out Tracker_Id trackerId))
                return trackerId;

            throw new ArgumentException("Invalid text-representation of a tracker identification: '" + Text + "'!",
                                        nameof(Text));

        }

        #endregion

        #region (static) TryParse(Text)

        /// <summary>
        /// Try to parse the given string as a tracker identification.
        /// </summary>
        /// <param name="Text">A text representation of a tracker identification.</param>
        public static Tracker_Id? TryParse(String Text)
        {

            if (TryParse(Text, out Tracker_Id trackerId))
                return trackerId;

            return null;

        }

        #endregion

        #region (static) TryParse(Text, out TrackerId)

        /// <summary>
        /// Parse the given string as a tracker identification.
        /// </summary>
        /// <param name="Text">A text representation of a tracker identification.</param>
        /// <param name="TrackerId">The parsed tracker identification.</param>
        public static Boolean TryParse(String Text, out Tracker_Id TrackerId)
        {

            Text = Text.Trim();

            if (Text.IsNotNullOrEmpty())
            {
                try
                {
                    TrackerId = new Tracker_Id(Text);
                    return true;
                }
                catch
                { }
            }

            TrackerId = default;
            return false;

        }

        #endregion

        #region Clone

        /// <summary>
        /// Clone this tracker identification.
        /// </summary>
        public Tracker_Id Clone

            => new (
                   new String(InternalId?.ToCharArray())
               );

        #endregion


        #region Operator overloading

        #region Operator == (TrackerId1, TrackerId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="TrackerId1">A tracker identification.</param>
        /// <param name="TrackerId2">Another tracker identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator == (Tracker_Id TrackerId1,
                                           Tracker_Id TrackerId2)

            => TrackerId1.Equals(TrackerId2);

        #endregion

        #region Operator != (TrackerId1, TrackerId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="TrackerId1">A tracker identification.</param>
        /// <param name="TrackerId2">Another tracker identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator != (Tracker_Id TrackerId1,
                                           Tracker_Id TrackerId2)

            => !TrackerId1.Equals(TrackerId2);

        #endregion

        #region Operator <  (TrackerId1, TrackerId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="TrackerId1">A tracker identification.</param>
        /// <param name="TrackerId2">Another tracker identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator < (Tracker_Id TrackerId1,
                                          Tracker_Id TrackerId2)

            => TrackerId1.CompareTo(TrackerId2) < 0;

        #endregion

        #region Operator <= (TrackerId1, TrackerId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="TrackerId1">A tracker identification.</param>
        /// <param name="TrackerId2">Another tracker identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator <= (Tracker_Id TrackerId1,
                                           Tracker_Id TrackerId2)

            => TrackerId1.CompareTo(TrackerId2) <= 0;

        #endregion

        #region Operator >  (TrackerId1, TrackerId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="TrackerId1">A tracker identification.</param>
        /// <param name="TrackerId2">Another tracker identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator > (Tracker_Id TrackerId1,
                                          Tracker_Id TrackerId2)

            => TrackerId1.CompareTo(TrackerId2) > 0;

        #endregion

        #region Operator >= (TrackerId1, TrackerId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="TrackerId1">A tracker identification.</param>
        /// <param name="TrackerId2">Another tracker identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator >= (Tracker_Id TrackerId1,
                                           Tracker_Id TrackerId2)

            => TrackerId1.CompareTo(TrackerId2) >= 0;

        #endregion

        #endregion

        #region IComparable<Tracker_Id> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        public Int32 CompareTo(Object? Object)

            => Object is Tracker_Id trackerId
                   ? CompareTo(trackerId)
                   : throw new ArgumentException("The given object is not a tracker identification!",
                                                 nameof(Object));

        #endregion

        #region CompareTo(TrackerId)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="TrackerId">An object to compare with.</param>
        public Int32 CompareTo(Tracker_Id TrackerId)

            => String.Compare(InternalId,
                              TrackerId.InternalId,
                              StringComparison.OrdinalIgnoreCase);

        #endregion

        #endregion

        #region IEquatable<Tracker_Id> Members

        #region Equals(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        /// <returns>true|false</returns>
        public override Boolean Equals(Object? Object)

            => Object is Tracker_Id trackerId &&
                   Equals(trackerId);

        #endregion

        #region Equals(TrackerId)

        /// <summary>
        /// Compares two tracker identifications for equality.
        /// </summary>
        /// <param name="TrackerId">A tracker identification to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public Boolean Equals(Tracker_Id TrackerId)

            => String.Equals(InternalId,
                             TrackerId.InternalId,
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
        /// Return a text-representation of this object.
        /// </summary>
        public override String ToString()

            => InternalId ?? "";

        #endregion

    }

}
