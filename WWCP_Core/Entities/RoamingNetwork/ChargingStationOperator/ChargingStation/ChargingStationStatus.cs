/*
 * Copyright (c) 2014-2017 GraphDefined GmbH <achim.friedland@graphdefined.com>
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

using System;

using org.GraphDefined.Vanaheimr.Illias;

#endregion

namespace org.GraphDefined.WWCP
{

    /// <summary>
    /// The current status of a charging station.
    /// </summary>
    public struct ChargingStationStatus : IEquatable <ChargingStationStatus>,
                                          IComparable<ChargingStationStatus>
    {

        #region Properties

        /// <summary>
        /// The unique identification of the charging station.
        /// </summary>
        public ChargingStation_Id         Id          { get; }

        /// <summary>
        /// The current status of the charging station.
        /// </summary>
        public ChargingStationStatusTypes  Status      { get; }

        /// <summary>
        /// The timestamp of the current status of the charging station.
        /// </summary>
        public DateTime                   Timestamp   { get; }

        /// <summary>
        /// The timestamped status of the charging station.
        /// </summary>
        public Timestamped<ChargingStationStatusTypes> Combined
            => new Timestamped<ChargingStationStatusTypes>(Timestamp, Status);

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new charging station status.
        /// </summary>
        /// <param name="Id">The unique identification of the charging station.</param>
        /// <param name="Status">The current status of the charging station.</param>
        /// <param name="Timestamp">The timestamp of the current status of the charging station.</param>
        public ChargingStationStatus(ChargingStation_Id         Id,
                                     ChargingStationStatusTypes  Status,
                                     DateTime                   Timestamp)

        {

            this.Id         = Id;
            this.Status     = Status;
            this.Timestamp  = Timestamp;

        }

        #endregion


        #region (static) Snapshot(ChargingStation)

        /// <summary>
        /// Take a snapshot of the current charging station status.
        /// </summary>
        /// <param name="ChargingStation">A charging station.</param>
        public static ChargingStationStatus Snapshot(ChargingStation ChargingStation)

            => new ChargingStationStatus(ChargingStation.Id,
                                         ChargingStation.Status.Value,
                                         ChargingStation.Status.Timestamp);

        #endregion


        #region Operator overloading

        #region Operator == (ChargingStationStatus1, ChargingStationStatus2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingStationStatus1">A charging station status.</param>
        /// <param name="ChargingStationStatus2">Another charging station status.</param>
        /// <returns>true|false</returns>
        public static Boolean operator == (ChargingStationStatus ChargingStationStatus1, ChargingStationStatus ChargingStationStatus2)
        {

            // If both are null, or both are same instance, return true.
            if (Object.ReferenceEquals(ChargingStationStatus1, ChargingStationStatus2))
                return true;

            // If one is null, but not both, return false.
            if (((Object) ChargingStationStatus1 == null) || ((Object) ChargingStationStatus2 == null))
                return false;

            return ChargingStationStatus1.Equals(ChargingStationStatus2);

        }

        #endregion

        #region Operator != (ChargingStationStatus1, ChargingStationStatus2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingStationStatus1">A charging station status.</param>
        /// <param name="ChargingStationStatus2">Another charging station status.</param>
        /// <returns>true|false</returns>
        public static Boolean operator != (ChargingStationStatus ChargingStationStatus1, ChargingStationStatus ChargingStationStatus2)
            => !(ChargingStationStatus1 == ChargingStationStatus2);

        #endregion

        #region Operator <  (ChargingStationStatus1, ChargingStationStatus2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingStationStatus1">A charging station status.</param>
        /// <param name="ChargingStationStatus2">Another charging station status.</param>
        /// <returns>true|false</returns>
        public static Boolean operator < (ChargingStationStatus ChargingStationStatus1, ChargingStationStatus ChargingStationStatus2)
        {

            if ((Object) ChargingStationStatus1 == null)
                throw new ArgumentNullException(nameof(ChargingStationStatus1), "The given ChargingStationStatus1 must not be null!");

            return ChargingStationStatus1.CompareTo(ChargingStationStatus2) < 0;

        }

        #endregion

        #region Operator <= (ChargingStationStatus1, ChargingStationStatus2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingStationStatus1">A charging station status.</param>
        /// <param name="ChargingStationStatus2">Another charging station status.</param>
        /// <returns>true|false</returns>
        public static Boolean operator <= (ChargingStationStatus ChargingStationStatus1, ChargingStationStatus ChargingStationStatus2)
            => !(ChargingStationStatus1 > ChargingStationStatus2);

        #endregion

        #region Operator >  (ChargingStationStatus1, ChargingStationStatus2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingStationStatus1">A charging station status.</param>
        /// <param name="ChargingStationStatus2">Another charging station status.</param>
        /// <returns>true|false</returns>
        public static Boolean operator > (ChargingStationStatus ChargingStationStatus1, ChargingStationStatus ChargingStationStatus2)
        {

            if ((Object) ChargingStationStatus1 == null)
                throw new ArgumentNullException(nameof(ChargingStationStatus1), "The given ChargingStationStatus1 must not be null!");

            return ChargingStationStatus1.CompareTo(ChargingStationStatus2) > 0;

        }

        #endregion

        #region Operator >= (ChargingStationStatus1, ChargingStationStatus2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingStationStatus1">A charging station status.</param>
        /// <param name="ChargingStationStatus2">Another charging station status.</param>
        /// <returns>true|false</returns>
        public static Boolean operator >= (ChargingStationStatus ChargingStationStatus1, ChargingStationStatus ChargingStationStatus2)
            => !(ChargingStationStatus1 < ChargingStationStatus2);

        #endregion

        #endregion

        #region IComparable<ChargingStationStatus> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        public Int32 CompareTo(Object Object)
        {

            if (Object == null)
                throw new ArgumentNullException(nameof(Object), "The given object must not be null!");

            if (!(Object is ChargingStationStatus))
                throw new ArgumentException("The given object is not a ChargingStationStatus!",
                                            nameof(Object));

            return CompareTo((ChargingStationStatus) Object);

        }

        #endregion

        #region CompareTo(ChargingStationStatus)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingStationStatus">An object to compare with.</param>
        public Int32 CompareTo(ChargingStationStatus ChargingStationStatus)
        {

            if ((Object) ChargingStationStatus == null)
                throw new ArgumentNullException(nameof(ChargingStationStatus), "The given ChargingStationStatus must not be null!");

            // Compare ChargingStation Ids
            var _Result = Id.CompareTo(ChargingStationStatus.Id);

            // If equal: Compare ChargingStation status
            if (_Result == 0)
                _Result = Status.CompareTo(ChargingStationStatus.Status);

            return _Result;

        }

        #endregion

        #endregion

        #region IEquatable<ChargingStationStatus> Members

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

            if (!(Object is ChargingStationStatus))
                return false;

            return this.Equals((ChargingStationStatus) Object);

        }

        #endregion

        #region Equals(ChargingStationStatus)

        /// <summary>
        /// Compares two ChargingStation identifications for equality.
        /// </summary>
        /// <param name="ChargingStationStatus">A charging station identification to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public Boolean Equals(ChargingStationStatus ChargingStationStatus)
        {

            if ((Object) ChargingStationStatus == null)
                return false;

            return Id.       Equals(ChargingStationStatus.Id)     &&
                   Status.   Equals(ChargingStationStatus.Status) &&
                   Timestamp.Equals(ChargingStationStatus.Timestamp);

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

                return Id.       GetHashCode() * 7 ^
                       Status.   GetHashCode() * 5 ^
                       Timestamp.GetHashCode();

            }
        }

        #endregion

        #region (override) ToString()

        /// <summary>
        /// Return a string representation of this object.
        /// </summary>
        public override String ToString()

            => String.Concat(Id, " -> ",
                             Status,
                             " since ",
                             Timestamp.ToIso8601());

        #endregion

    }

}
