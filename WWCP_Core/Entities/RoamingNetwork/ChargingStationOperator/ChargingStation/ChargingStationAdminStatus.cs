/*
 * Copyright (c) 2014-2016 GraphDefined GmbH <achim.friedland@graphdefined.com>
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
    /// The current admin status of a charging station.
    /// </summary>
    public struct ChargingStationAdminStatus : IEquatable <ChargingStationAdminStatus>,
                                               IComparable<ChargingStationAdminStatus>
    {

        #region Properties

        /// <summary>
        /// The unique identification of the charging station.
        /// </summary>
        public ChargingStation_Id              Id            { get; }

        /// <summary>
        /// The current admin status of the charging station.
        /// </summary>
        public ChargingStationAdminStatusType  AdminStatus   { get; }

        /// <summary>
        /// The timestamp of the current admin status of the charging station.
        /// </summary>
        public DateTime                        Timestamp     { get; }

        /// <summary>
        /// The timestamped admin status of the charging station.
        /// </summary>
        public Timestamped<ChargingStationAdminStatusType> Combined
            => new Timestamped<ChargingStationAdminStatusType>(Timestamp, AdminStatus);

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new charging station admin status.
        /// </summary>
        /// <param name="Id">The unique identification of the charging station.</param>
        /// <param name="Status">The current admin status of the charging station.</param>
        /// <param name="Timestamp">The timestamp of the current admin status of the charging station.</param>
        public ChargingStationAdminStatus(ChargingStation_Id              Id,
                                          ChargingStationAdminStatusType  Status,
                                          DateTime                        Timestamp)

        {

            this.Id           = Id;
            this.AdminStatus  = Status;
            this.Timestamp    = Timestamp;

        }

        #endregion


        #region (static) Snapshot(ChargingStation)

        /// <summary>
        /// Take a snapshot of the current charging station admin status.
        /// </summary>
        /// <param name="ChargingStation">A charging station.</param>
        public static ChargingStationAdminStatus Snapshot(ChargingStation ChargingStation)

            => new ChargingStationAdminStatus(ChargingStation.Id,
                                              ChargingStation.AdminStatus.Value,
                                              ChargingStation.AdminStatus.Timestamp);

        #endregion


        #region Operator overloading

        #region Operator == (ChargingStationAdminStatus1, ChargingStationAdminStatus2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingStationAdminStatus1">A charging station admin status.</param>
        /// <param name="ChargingStationAdminStatus2">Another charging station admin status.</param>
        /// <returns>true|false</returns>
        public static Boolean operator == (ChargingStationAdminStatus ChargingStationAdminStatus1, ChargingStationAdminStatus ChargingStationAdminStatus2)
        {

            // If both are null, or both are same instance, return true.
            if (Object.ReferenceEquals(ChargingStationAdminStatus1, ChargingStationAdminStatus2))
                return true;

            // If one is null, but not both, return false.
            if (((Object) ChargingStationAdminStatus1 == null) || ((Object) ChargingStationAdminStatus2 == null))
                return false;

            return ChargingStationAdminStatus1.Equals(ChargingStationAdminStatus2);

        }

        #endregion

        #region Operator != (ChargingStationAdminStatus1, ChargingStationAdminStatus2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingStationAdminStatus1">A charging station admin status.</param>
        /// <param name="ChargingStationAdminStatus2">Another charging station admin status.</param>
        /// <returns>true|false</returns>
        public static Boolean operator != (ChargingStationAdminStatus ChargingStationAdminStatus1, ChargingStationAdminStatus ChargingStationAdminStatus2)
            => !(ChargingStationAdminStatus1 == ChargingStationAdminStatus2);

        #endregion

        #region Operator <  (ChargingStationAdminStatus1, ChargingStationAdminStatus2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingStationAdminStatus1">A charging station admin status.</param>
        /// <param name="ChargingStationAdminStatus2">Another charging station admin status.</param>
        /// <returns>true|false</returns>
        public static Boolean operator < (ChargingStationAdminStatus ChargingStationAdminStatus1, ChargingStationAdminStatus ChargingStationAdminStatus2)
        {

            if ((Object) ChargingStationAdminStatus1 == null)
                throw new ArgumentNullException(nameof(ChargingStationAdminStatus1), "The given ChargingStationAdminStatus1 must not be null!");

            return ChargingStationAdminStatus1.CompareTo(ChargingStationAdminStatus2) < 0;

        }

        #endregion

        #region Operator <= (ChargingStationAdminStatus1, ChargingStationAdminStatus2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingStationAdminStatus1">A charging station admin status.</param>
        /// <param name="ChargingStationAdminStatus2">Another charging station admin status.</param>
        /// <returns>true|false</returns>
        public static Boolean operator <= (ChargingStationAdminStatus ChargingStationAdminStatus1, ChargingStationAdminStatus ChargingStationAdminStatus2)
            => !(ChargingStationAdminStatus1 > ChargingStationAdminStatus2);

        #endregion

        #region Operator >  (ChargingStationAdminStatus1, ChargingStationAdminStatus2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingStationAdminStatus1">A charging station admin status.</param>
        /// <param name="ChargingStationAdminStatus2">Another charging station admin status.</param>
        /// <returns>true|false</returns>
        public static Boolean operator > (ChargingStationAdminStatus ChargingStationAdminStatus1, ChargingStationAdminStatus ChargingStationAdminStatus2)
        {

            if ((Object) ChargingStationAdminStatus1 == null)
                throw new ArgumentNullException(nameof(ChargingStationAdminStatus1), "The given ChargingStationAdminStatus1 must not be null!");

            return ChargingStationAdminStatus1.CompareTo(ChargingStationAdminStatus2) > 0;

        }

        #endregion

        #region Operator >= (ChargingStationAdminStatus1, ChargingStationAdminStatus2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingStationAdminStatus1">A charging station admin status.</param>
        /// <param name="ChargingStationAdminStatus2">Another charging station admin status.</param>
        /// <returns>true|false</returns>
        public static Boolean operator >= (ChargingStationAdminStatus ChargingStationAdminStatus1, ChargingStationAdminStatus ChargingStationAdminStatus2)
            => !(ChargingStationAdminStatus1 < ChargingStationAdminStatus2);

        #endregion

        #endregion

        #region IComparable<ChargingStationAdminStatus> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        public Int32 CompareTo(Object Object)
        {

            if (Object == null)
                throw new ArgumentNullException(nameof(Object), "The given object must not be null!");

            if (!(Object is ChargingStationAdminStatus))
                throw new ArgumentException("The given object is not a ChargingStationAdminStatus!",
                                            nameof(Object));

            return CompareTo((ChargingStationAdminStatus) Object);

        }

        #endregion

        #region CompareTo(ChargingStationAdminStatus)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingStationAdminStatus">An object to compare with.</param>
        public Int32 CompareTo(ChargingStationAdminStatus ChargingStationAdminStatus)
        {

            if ((Object) ChargingStationAdminStatus == null)
                throw new ArgumentNullException(nameof(ChargingStationAdminStatus), "The given ChargingStationAdminStatus must not be null!");

            // Compare ChargingStation Ids
            var _Result = Id.CompareTo(ChargingStationAdminStatus.Id);

            // If equal: Compare ChargingStation status
            if (_Result == 0)
                _Result = AdminStatus.CompareTo(ChargingStationAdminStatus.AdminStatus);

            return _Result;

        }

        #endregion

        #endregion

        #region IEquatable<ChargingStationAdminStatus> Members

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

            if (!(Object is ChargingStationAdminStatus))
                return false;

            return this.Equals((ChargingStationAdminStatus) Object);

        }

        #endregion

        #region Equals(ChargingStationAdminStatus)

        /// <summary>
        /// Compares two ChargingStation identifications for equality.
        /// </summary>
        /// <param name="ChargingStationAdminStatus">A charging station identification to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public Boolean Equals(ChargingStationAdminStatus ChargingStationAdminStatus)
        {

            if ((Object) ChargingStationAdminStatus == null)
                return false;

            return Id.         Equals(ChargingStationAdminStatus.Id)          &&
                   AdminStatus.Equals(ChargingStationAdminStatus.AdminStatus) &&
                   Timestamp.  Equals(ChargingStationAdminStatus.Timestamp);

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

                return Id.         GetHashCode() * 7 ^
                       AdminStatus.GetHashCode() * 5 ^
                       Timestamp.  GetHashCode();

            }
        }

        #endregion

        #region (override) ToString()

        /// <summary>
        /// Return a string representation of this object.
        /// </summary>
        public override String ToString()

            => String.Concat(Id, " -> ",
                             AdminStatus,
                             " since ",
                             Timestamp.ToIso8601());

        #endregion

    }

}
