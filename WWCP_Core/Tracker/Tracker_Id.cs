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
    /// The unique identification of a tracker.
    /// </summary>
    public class Tracker_Id : IId,
                              IEquatable<Tracker_Id>,
                              IComparable<Tracker_Id>

    {

        #region Data

        private readonly String InternalId;

        #endregion

        #region Properties

        /// <summary>
        /// Indicates whether this identification is null or empty.
        /// </summary>
        public Boolean IsNullOrEmpty
            => InternalId.IsNullOrEmpty();

        /// <summary>
        /// Returns the length of the identification.
        /// </summary>
        public UInt64 Length
            => (UInt64) InternalId.Length;

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Generate a new tracker identification based on the given string.
        /// </summary>
        private Tracker_Id(String  Text)
        {

            #region Initial checks

            if (Text.IsNullOrEmpty())
                throw new ArgumentNullException(nameof(Text),  "The given text must not be null or empty!");

            #endregion

            this.InternalId = Text;

        }

        #endregion


        #region Parse(Text)

        /// <summary>
        /// Parse the given string as a charging station identification (EVCS Id).
        /// </summary>
        /// <param name="Text">A text representation of a charging station identification.</param>
        public static Tracker_Id Parse(String Text)
        {

            #region Initial checks

            if (Text.IsNullOrEmpty())
                throw new ArgumentNullException(nameof(Text), "The given text must not be null or empty!");

            #endregion

            return new Tracker_Id(Text);

        }

        #endregion

        #region TryParse(Text, out ChargingStationId)

        /// <summary>
        /// Parse the given string as a charging station identification (EVCS Id).
        /// </summary>
        /// <param name="Text">A text representation of a charging station identification.</param>
        /// <param name="ChargingStationId">The parsed charging station identification.</param>
        public static Boolean TryParse(String Text, out Tracker_Id ChargingStationId)
        {

            #region Initial checks

            if (Text.IsNullOrEmpty())
            {
                ChargingStationId = null;
                return false;
            }

            #endregion

            try
            {

                ChargingStationId = new Tracker_Id(Text);

                return true;

            }
            catch (Exception)
            { }

            ChargingStationId = null;
            return false;

        }

        #endregion

        #region Random

        /// <summary>
        /// Generate a new unique identification of a tracker.
        /// </summary>
        public static Tracker_Id Random

            => new (RandomExtensions.RandomString(23));

        #endregion

        #region Clone

        /// <summary>
        /// Clone this Electric Vehicle Charging Station identification.
        /// </summary>
        public Tracker_Id Clone
            => new Tracker_Id(InternalId);

        #endregion


        #region Operator overloading

        #region Operator == (WWCPTrackerClientId1, WWCPTrackerClientId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="WWCPTrackerClientId1">A WWCPTrackerClient_Id.</param>
        /// <param name="WWCPTrackerClientId2">Another WWCPTrackerClient_Id.</param>
        /// <returns>true|false</returns>
        public static Boolean operator == (Tracker_Id WWCPTrackerClientId1, Tracker_Id WWCPTrackerClientId2)
        {

            // If both are null, or both are same instance, return true.
            if (ReferenceEquals(WWCPTrackerClientId1, WWCPTrackerClientId2))
                return true;

            // If one is null, but not both, return false.
            if (((Object) WWCPTrackerClientId1 == null) || ((Object) WWCPTrackerClientId2 == null))
                return false;

            return WWCPTrackerClientId1.Equals(WWCPTrackerClientId2);

        }

        #endregion

        #region Operator != (WWCPTrackerClientId1, WWCPTrackerClientId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="WWCPTrackerClientId1">A WWCPTrackerClient_Id.</param>
        /// <param name="WWCPTrackerClientId2">Another WWCPTrackerClient_Id.</param>
        /// <returns>true|false</returns>
        public static Boolean operator != (Tracker_Id WWCPTrackerClientId1, Tracker_Id WWCPTrackerClientId2)
        {
            return !(WWCPTrackerClientId1 == WWCPTrackerClientId2);
        }

        #endregion

        #region Operator <  (WWCPTrackerClientId1, WWCPTrackerClientId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="WWCPTrackerClientId1">A WWCPTrackerClient_Id.</param>
        /// <param name="WWCPTrackerClientId2">Another WWCPTrackerClient_Id.</param>
        /// <returns>true|false</returns>
        public static Boolean operator < (Tracker_Id WWCPTrackerClientId1, Tracker_Id WWCPTrackerClientId2)
        {

            if ((Object) WWCPTrackerClientId1 == null)
                throw new ArgumentNullException("The given WWCPTrackerClientId1 must not be null!");

            return WWCPTrackerClientId1.CompareTo(WWCPTrackerClientId2) < 0;

        }

        #endregion

        #region Operator <= (WWCPTrackerClientId1, WWCPTrackerClientId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="WWCPTrackerClientId1">A WWCPTrackerClient_Id.</param>
        /// <param name="WWCPTrackerClientId2">Another WWCPTrackerClient_Id.</param>
        /// <returns>true|false</returns>
        public static Boolean operator <= (Tracker_Id WWCPTrackerClientId1, Tracker_Id WWCPTrackerClientId2)
        {
            return !(WWCPTrackerClientId1 > WWCPTrackerClientId2);
        }

        #endregion

        #region Operator >  (WWCPTrackerClientId1, WWCPTrackerClientId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="WWCPTrackerClientId1">A WWCPTrackerClient_Id.</param>
        /// <param name="WWCPTrackerClientId2">Another WWCPTrackerClient_Id.</param>
        /// <returns>true|false</returns>
        public static Boolean operator > (Tracker_Id WWCPTrackerClientId1, Tracker_Id WWCPTrackerClientId2)
        {

            if ((Object) WWCPTrackerClientId1 == null)
                throw new ArgumentNullException("The given WWCPTrackerClientId1 must not be null!");

            return WWCPTrackerClientId1.CompareTo(WWCPTrackerClientId2) > 0;

        }

        #endregion

        #region Operator >= (WWCPTrackerClientId1, WWCPTrackerClientId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="WWCPTrackerClientId1">A WWCPTrackerClient_Id.</param>
        /// <param name="WWCPTrackerClientId2">Another WWCPTrackerClient_Id.</param>
        /// <returns>true|false</returns>
        public static Boolean operator >= (Tracker_Id WWCPTrackerClientId1, Tracker_Id WWCPTrackerClientId2)
        {
            return !(WWCPTrackerClientId1 < WWCPTrackerClientId2);
        }

        #endregion

        #endregion

        #region IComparable<WWCPTrackerClient_Id> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        public Int32 CompareTo(Object Object)
        {

            if (Object == null)
                throw new ArgumentNullException("The given object must not be null!");

            // Check if the given object is an WWCPTrackerClientId.
            var WWCPTrackerClientId = Object as Tracker_Id;
            if ((Object) WWCPTrackerClientId == null)
                throw new ArgumentException("The given object is not a WWCPTrackerClientId!");

            return CompareTo(WWCPTrackerClientId);

        }

        #endregion

        #region CompareTo(WWCPTrackerClientId)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="WWCPTrackerClientId">An object to compare with.</param>
        public Int32 CompareTo(Tracker_Id WWCPTrackerClientId)
        {

            if ((Object) WWCPTrackerClientId == null)
                throw new ArgumentNullException("The given WWCPTrackerClientId must not be null!");

            return InternalId.CompareTo(WWCPTrackerClientId.InternalId);

        }

        #endregion

        #endregion

        #region IEquatable<WWCPTrackerClient_Id> Members

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

            // Check if the given object is an WWCPTrackerClientId.
            var WWCPTrackerClientId = Object as Tracker_Id;
            if ((Object) WWCPTrackerClientId == null)
                return false;

            return this.Equals(WWCPTrackerClientId);

        }

        #endregion

        #region Equals(WWCPTrackerClientId)

        /// <summary>
        /// Compares two WWCPTrackerClientIds for equality.
        /// </summary>
        /// <param name="WWCPTrackerClientId">A WWCPTrackerClientId to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public Boolean Equals(Tracker_Id WWCPTrackerClientId)
        {

            if ((Object) WWCPTrackerClientId == null)
                return false;

            return InternalId.Equals(WWCPTrackerClientId.InternalId);

        }

        #endregion

        #endregion

        #region GetHashCode()

        /// <summary>
        /// Return the HashCode of this object.
        /// </summary>
        /// <returns>The HashCode of this object.</returns>
        public override Int32 GetHashCode()
            => InternalId.GetHashCode();

        #endregion

        #region (override) ToString()

        /// <summary>
        /// Return a text representation of this object.
        /// </summary>
        public override String ToString()
            => InternalId;

        #endregion

    }

}
