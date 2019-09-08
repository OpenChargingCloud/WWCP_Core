/*
 * Copyright (c) 2014-2019 GraphDefined GmbH <achim.friedland@graphdefined.com>
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
    /// The current status of a charging station operator.
    /// </summary>
    public struct ChargingStationOperatorStatus : IEquatable <ChargingStationOperatorStatus>,
                                                  IComparable<ChargingStationOperatorStatus>
    {

        #region Properties

        /// <summary>
        /// The unique identification of the charging station operator.
        /// </summary>
        public ChargingStationOperator_Id         Id          { get; }

        /// <summary>
        /// The current status of the charging station operator.
        /// </summary>
        public ChargingStationOperatorStatusTypes  Status      { get; }

        /// <summary>
        /// The timestamp of the current status of the charging station operator.
        /// </summary>
        public DateTime                           Timestamp   { get; }

        /// <summary>
        /// The timestamped status of the charging station operator.
        /// </summary>
        public Timestamped<ChargingStationOperatorStatusTypes> Combined
            => new Timestamped<ChargingStationOperatorStatusTypes>(Timestamp, Status);

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new charging station operator status.
        /// </summary>
        /// <param name="Id">The unique identification of the charging station operator.</param>
        /// <param name="Status">The current status of the charging station operator.</param>
        /// <param name="Timestamp">The timestamp of the current status of the charging station operator.</param>
        public ChargingStationOperatorStatus(ChargingStationOperator_Id         Id,
                                             ChargingStationOperatorStatusTypes  Status,
                                             DateTime                           Timestamp)

        {

            this.Id         = Id;
            this.Status     = Status;
            this.Timestamp  = Timestamp;

        }

        #endregion


        #region (static) Snapshot(ChargingStationOperator)

        /// <summary>
        /// Take a snapshot of the current charging station operator status.
        /// </summary>
        /// <param name="ChargingStationOperator">A charging station operator.</param>
        public static ChargingStationOperatorStatus Snapshot(ChargingStationOperator ChargingStationOperator)

            => new ChargingStationOperatorStatus(ChargingStationOperator.Id,
                                                 ChargingStationOperator.Status.Value,
                                                 ChargingStationOperator.Status.Timestamp);

        #endregion


        #region Operator overloading

        #region Operator == (ChargingStationOperatorStatus1, ChargingStationOperatorStatus2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingStationOperatorStatus1">A charging station operator status.</param>
        /// <param name="ChargingStationOperatorStatus2">Another charging station operator status.</param>
        /// <returns>true|false</returns>
        public static Boolean operator == (ChargingStationOperatorStatus ChargingStationOperatorStatus1, ChargingStationOperatorStatus ChargingStationOperatorStatus2)
        {

            // If both are null, or both are same instance, return true.
            if (Object.ReferenceEquals(ChargingStationOperatorStatus1, ChargingStationOperatorStatus2))
                return true;

            // If one is null, but not both, return false.
            if (((Object) ChargingStationOperatorStatus1 == null) || ((Object) ChargingStationOperatorStatus2 == null))
                return false;

            return ChargingStationOperatorStatus1.Equals(ChargingStationOperatorStatus2);

        }

        #endregion

        #region Operator != (ChargingStationOperatorStatus1, ChargingStationOperatorStatus2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingStationOperatorStatus1">A charging station operator status.</param>
        /// <param name="ChargingStationOperatorStatus2">Another charging station operator status.</param>
        /// <returns>true|false</returns>
        public static Boolean operator != (ChargingStationOperatorStatus ChargingStationOperatorStatus1, ChargingStationOperatorStatus ChargingStationOperatorStatus2)
            => !(ChargingStationOperatorStatus1 == ChargingStationOperatorStatus2);

        #endregion

        #region Operator <  (ChargingStationOperatorStatus1, ChargingStationOperatorStatus2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingStationOperatorStatus1">A charging station operator status.</param>
        /// <param name="ChargingStationOperatorStatus2">Another charging station operator status.</param>
        /// <returns>true|false</returns>
        public static Boolean operator < (ChargingStationOperatorStatus ChargingStationOperatorStatus1, ChargingStationOperatorStatus ChargingStationOperatorStatus2)
        {

            if ((Object) ChargingStationOperatorStatus1 == null)
                throw new ArgumentNullException(nameof(ChargingStationOperatorStatus1), "The given ChargingStationOperatorStatus1 must not be null!");

            return ChargingStationOperatorStatus1.CompareTo(ChargingStationOperatorStatus2) < 0;

        }

        #endregion

        #region Operator <= (ChargingStationOperatorStatus1, ChargingStationOperatorStatus2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingStationOperatorStatus1">A charging station operator status.</param>
        /// <param name="ChargingStationOperatorStatus2">Another charging station operator status.</param>
        /// <returns>true|false</returns>
        public static Boolean operator <= (ChargingStationOperatorStatus ChargingStationOperatorStatus1, ChargingStationOperatorStatus ChargingStationOperatorStatus2)
            => !(ChargingStationOperatorStatus1 > ChargingStationOperatorStatus2);

        #endregion

        #region Operator >  (ChargingStationOperatorStatus1, ChargingStationOperatorStatus2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingStationOperatorStatus1">A charging station operator status.</param>
        /// <param name="ChargingStationOperatorStatus2">Another charging station operator status.</param>
        /// <returns>true|false</returns>
        public static Boolean operator > (ChargingStationOperatorStatus ChargingStationOperatorStatus1, ChargingStationOperatorStatus ChargingStationOperatorStatus2)
        {

            if ((Object) ChargingStationOperatorStatus1 == null)
                throw new ArgumentNullException(nameof(ChargingStationOperatorStatus1), "The given ChargingStationOperatorStatus1 must not be null!");

            return ChargingStationOperatorStatus1.CompareTo(ChargingStationOperatorStatus2) > 0;

        }

        #endregion

        #region Operator >= (ChargingStationOperatorStatus1, ChargingStationOperatorStatus2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingStationOperatorStatus1">A charging station operator status.</param>
        /// <param name="ChargingStationOperatorStatus2">Another charging station operator status.</param>
        /// <returns>true|false</returns>
        public static Boolean operator >= (ChargingStationOperatorStatus ChargingStationOperatorStatus1, ChargingStationOperatorStatus ChargingStationOperatorStatus2)
            => !(ChargingStationOperatorStatus1 < ChargingStationOperatorStatus2);

        #endregion

        #endregion

        #region IComparable<ChargingStationOperatorStatus> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        public Int32 CompareTo(Object Object)
        {

            if (Object == null)
                throw new ArgumentNullException(nameof(Object), "The given object must not be null!");

            if (!(Object is ChargingStationOperatorStatus))
                throw new ArgumentException("The given object is not a ChargingStationOperatorStatus!",
                                            nameof(Object));

            return CompareTo((ChargingStationOperatorStatus) Object);

        }

        #endregion

        #region CompareTo(ChargingStationOperatorStatus)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingStationOperatorStatus">An object to compare with.</param>
        public Int32 CompareTo(ChargingStationOperatorStatus ChargingStationOperatorStatus)
        {

            if ((Object) ChargingStationOperatorStatus == null)
                throw new ArgumentNullException(nameof(ChargingStationOperatorStatus), "The given ChargingStationOperatorStatus must not be null!");

            // Compare ChargingStationOperator Ids
            var _Result = Id.CompareTo(ChargingStationOperatorStatus.Id);

            // If equal: Compare ChargingStationOperator status
            if (_Result == 0)
                _Result = Status.CompareTo(ChargingStationOperatorStatus.Status);

            return _Result;

        }

        #endregion

        #endregion

        #region IEquatable<ChargingStationOperatorStatus> Members

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

            if (!(Object is ChargingStationOperatorStatus))
                return false;

            return this.Equals((ChargingStationOperatorStatus) Object);

        }

        #endregion

        #region Equals(ChargingStationOperatorStatus)

        /// <summary>
        /// Compares two ChargingStationOperator identifications for equality.
        /// </summary>
        /// <param name="ChargingStationOperatorStatus">A charging station operator identification to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public Boolean Equals(ChargingStationOperatorStatus ChargingStationOperatorStatus)
        {

            if ((Object) ChargingStationOperatorStatus == null)
                return false;

            return Id.       Equals(ChargingStationOperatorStatus.Id)     &&
                   Status.   Equals(ChargingStationOperatorStatus.Status) &&
                   Timestamp.Equals(ChargingStationOperatorStatus.Timestamp);

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
        /// Return a text representation of this object.
        /// </summary>
        public override String ToString()

            => String.Concat(Id, " -> ",
                             Status,
                             " since ",
                             Timestamp.ToIso8601());

        #endregion

    }

}
