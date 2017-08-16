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
    /// The current admin status of a charging station operator.
    /// </summary>
    public struct ChargingStationOperatorAdminStatus : IEquatable <ChargingStationOperatorAdminStatus>,
                                                       IComparable<ChargingStationOperatorAdminStatus>
    {

        #region Properties

        /// <summary>
        /// The unique identification of the charging station operator.
        /// </summary>
        public ChargingStationOperator_Id              Id            { get; }

        /// <summary>
        /// The current admin status of the charging station operator.
        /// </summary>
        public ChargingStationOperatorAdminStatusTypes  AdminStatus   { get; }

        /// <summary>
        /// The timestamp of the current admin status of the charging station operator.
        /// </summary>
        public DateTime                                Timestamp     { get; }

        /// <summary>
        /// The timestamped admin status of the charging station operator.
        /// </summary>
        public Timestamped<ChargingStationOperatorAdminStatusTypes> Combined
            => new Timestamped<ChargingStationOperatorAdminStatusTypes>(Timestamp, AdminStatus);

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new charging station operator admin status.
        /// </summary>
        /// <param name="Id">The unique identification of the charging station operator.</param>
        /// <param name="Status">The current admin status of the charging station operator.</param>
        /// <param name="Timestamp">The timestamp of the current admin status of the charging station operator.</param>
        public ChargingStationOperatorAdminStatus(ChargingStationOperator_Id              Id,
                                                  ChargingStationOperatorAdminStatusTypes  Status,
                                                  DateTime                                Timestamp)

        {

            this.Id           = Id;
            this.AdminStatus  = Status;
            this.Timestamp    = Timestamp;

        }

        #endregion


        #region (static) Snapshot(ChargingStationOperator)

        /// <summary>
        /// Take a snapshot of the current charging station operator admin status.
        /// </summary>
        /// <param name="ChargingStationOperator">A charging station operator.</param>
        public static ChargingStationOperatorAdminStatus Snapshot(ChargingStationOperator ChargingStationOperator)

            => new ChargingStationOperatorAdminStatus(ChargingStationOperator.Id,
                                                      ChargingStationOperator.AdminStatus.Value,
                                                      ChargingStationOperator.AdminStatus.Timestamp);

        #endregion


        #region Operator overloading

        #region Operator == (ChargingStationOperatorAdminStatus1, ChargingStationOperatorAdminStatus2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingStationOperatorAdminStatus1">A charging station operator admin status.</param>
        /// <param name="ChargingStationOperatorAdminStatus2">Another charging station operator admin status.</param>
        /// <returns>true|false</returns>
        public static Boolean operator == (ChargingStationOperatorAdminStatus ChargingStationOperatorAdminStatus1, ChargingStationOperatorAdminStatus ChargingStationOperatorAdminStatus2)
        {

            // If both are null, or both are same instance, return true.
            if (Object.ReferenceEquals(ChargingStationOperatorAdminStatus1, ChargingStationOperatorAdminStatus2))
                return true;

            // If one is null, but not both, return false.
            if (((Object) ChargingStationOperatorAdminStatus1 == null) || ((Object) ChargingStationOperatorAdminStatus2 == null))
                return false;

            return ChargingStationOperatorAdminStatus1.Equals(ChargingStationOperatorAdminStatus2);

        }

        #endregion

        #region Operator != (ChargingStationOperatorAdminStatus1, ChargingStationOperatorAdminStatus2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingStationOperatorAdminStatus1">A charging station operator admin status.</param>
        /// <param name="ChargingStationOperatorAdminStatus2">Another charging station operator admin status.</param>
        /// <returns>true|false</returns>
        public static Boolean operator != (ChargingStationOperatorAdminStatus ChargingStationOperatorAdminStatus1, ChargingStationOperatorAdminStatus ChargingStationOperatorAdminStatus2)
            => !(ChargingStationOperatorAdminStatus1 == ChargingStationOperatorAdminStatus2);

        #endregion

        #region Operator <  (ChargingStationOperatorAdminStatus1, ChargingStationOperatorAdminStatus2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingStationOperatorAdminStatus1">A charging station operator admin status.</param>
        /// <param name="ChargingStationOperatorAdminStatus2">Another charging station operator admin status.</param>
        /// <returns>true|false</returns>
        public static Boolean operator < (ChargingStationOperatorAdminStatus ChargingStationOperatorAdminStatus1, ChargingStationOperatorAdminStatus ChargingStationOperatorAdminStatus2)
        {

            if ((Object) ChargingStationOperatorAdminStatus1 == null)
                throw new ArgumentNullException(nameof(ChargingStationOperatorAdminStatus1), "The given ChargingStationOperatorAdminStatus1 must not be null!");

            return ChargingStationOperatorAdminStatus1.CompareTo(ChargingStationOperatorAdminStatus2) < 0;

        }

        #endregion

        #region Operator <= (ChargingStationOperatorAdminStatus1, ChargingStationOperatorAdminStatus2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingStationOperatorAdminStatus1">A charging station operator admin status.</param>
        /// <param name="ChargingStationOperatorAdminStatus2">Another charging station operator admin status.</param>
        /// <returns>true|false</returns>
        public static Boolean operator <= (ChargingStationOperatorAdminStatus ChargingStationOperatorAdminStatus1, ChargingStationOperatorAdminStatus ChargingStationOperatorAdminStatus2)
            => !(ChargingStationOperatorAdminStatus1 > ChargingStationOperatorAdminStatus2);

        #endregion

        #region Operator >  (ChargingStationOperatorAdminStatus1, ChargingStationOperatorAdminStatus2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingStationOperatorAdminStatus1">A charging station operator admin status.</param>
        /// <param name="ChargingStationOperatorAdminStatus2">Another charging station operator admin status.</param>
        /// <returns>true|false</returns>
        public static Boolean operator > (ChargingStationOperatorAdminStatus ChargingStationOperatorAdminStatus1, ChargingStationOperatorAdminStatus ChargingStationOperatorAdminStatus2)
        {

            if ((Object) ChargingStationOperatorAdminStatus1 == null)
                throw new ArgumentNullException(nameof(ChargingStationOperatorAdminStatus1), "The given ChargingStationOperatorAdminStatus1 must not be null!");

            return ChargingStationOperatorAdminStatus1.CompareTo(ChargingStationOperatorAdminStatus2) > 0;

        }

        #endregion

        #region Operator >= (ChargingStationOperatorAdminStatus1, ChargingStationOperatorAdminStatus2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingStationOperatorAdminStatus1">A charging station operator admin status.</param>
        /// <param name="ChargingStationOperatorAdminStatus2">Another charging station operator admin status.</param>
        /// <returns>true|false</returns>
        public static Boolean operator >= (ChargingStationOperatorAdminStatus ChargingStationOperatorAdminStatus1, ChargingStationOperatorAdminStatus ChargingStationOperatorAdminStatus2)
            => !(ChargingStationOperatorAdminStatus1 < ChargingStationOperatorAdminStatus2);

        #endregion

        #endregion

        #region IComparable<ChargingStationOperatorAdminStatus> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        public Int32 CompareTo(Object Object)
        {

            if (Object == null)
                throw new ArgumentNullException(nameof(Object), "The given object must not be null!");

            if (!(Object is ChargingStationOperatorAdminStatus))
                throw new ArgumentException("The given object is not a ChargingStationOperatorAdminStatus!",
                                            nameof(Object));

            return CompareTo((ChargingStationOperatorAdminStatus) Object);

        }

        #endregion

        #region CompareTo(ChargingStationOperatorAdminStatus)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingStationOperatorAdminStatus">An object to compare with.</param>
        public Int32 CompareTo(ChargingStationOperatorAdminStatus ChargingStationOperatorAdminStatus)
        {

            if ((Object) ChargingStationOperatorAdminStatus == null)
                throw new ArgumentNullException(nameof(ChargingStationOperatorAdminStatus), "The given ChargingStationOperatorAdminStatus must not be null!");

            // Compare ChargingStationOperator Ids
            var _Result = Id.CompareTo(ChargingStationOperatorAdminStatus.Id);

            // If equal: Compare ChargingStationOperator status
            if (_Result == 0)
                _Result = AdminStatus.CompareTo(ChargingStationOperatorAdminStatus.AdminStatus);

            return _Result;

        }

        #endregion

        #endregion

        #region IEquatable<ChargingStationOperatorAdminStatus> Members

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

            if (!(Object is ChargingStationOperatorAdminStatus))
                return false;

            return this.Equals((ChargingStationOperatorAdminStatus) Object);

        }

        #endregion

        #region Equals(ChargingStationOperatorAdminStatus)

        /// <summary>
        /// Compares two ChargingStationOperator identifications for equality.
        /// </summary>
        /// <param name="ChargingStationOperatorAdminStatus">A charging station operator identification to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public Boolean Equals(ChargingStationOperatorAdminStatus ChargingStationOperatorAdminStatus)
        {

            if ((Object) ChargingStationOperatorAdminStatus == null)
                return false;

            return Id.         Equals(ChargingStationOperatorAdminStatus.Id)          &&
                   AdminStatus.Equals(ChargingStationOperatorAdminStatus.AdminStatus) &&
                   Timestamp.  Equals(ChargingStationOperatorAdminStatus.Timestamp);

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
