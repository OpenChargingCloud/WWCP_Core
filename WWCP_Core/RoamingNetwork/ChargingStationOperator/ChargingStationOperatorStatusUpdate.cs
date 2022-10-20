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

using System;
using System.Linq;

using org.GraphDefined.Vanaheimr.Illias;

#endregion

namespace cloud.charging.open.protocols.WWCP
{

    /// <summary>
    /// A charging station operator status update.
    /// </summary>
    public struct ChargingStationOperatorStatusUpdate : IEquatable <ChargingStationOperatorStatusUpdate>,
                                                        IComparable<ChargingStationOperatorStatusUpdate>
    {

        #region Properties

        /// <summary>
        /// The unique identification of the charging station operator.
        /// </summary>
        public ChargingStationOperator_Id                      Id          { get; }

        /// <summary>
        /// The old timestamped status of the charging station operator.
        /// </summary>
        public Timestamped<ChargingStationOperatorStatusTypes>  OldStatus   { get; }

        /// <summary>
        /// The new timestamped status of the charging station operator.
        /// </summary>
        public Timestamped<ChargingStationOperatorStatusTypes>  NewStatus   { get; }

        #endregion

        #region Constructor(s)

        #region ChargingStationOperatorStatusUpdate(Id, OldStatus, NewStatus)

        /// <summary>
        /// Create a new charging station operator status update.
        /// </summary>
        /// <param name="Id">The unique identification of the charging station operator.</param>
        /// <param name="OldStatus">The old timestamped status of the charging station operator.</param>
        /// <param name="NewStatus">The new timestamped status of the charging station operator.</param>
        public ChargingStationOperatorStatusUpdate(ChargingStationOperator_Id                      Id,
                                                   Timestamped<ChargingStationOperatorStatusTypes>  OldStatus,
                                                   Timestamped<ChargingStationOperatorStatusTypes>  NewStatus)

        {

            this.Id         = Id;
            this.OldStatus  = OldStatus;
            this.NewStatus  = NewStatus;

        }

        #endregion

        #region ChargingStationOperatorStatusUpdate(Id, OldStatus, NewStatus)

        /// <summary>
        /// Create a new charging station operator status update.
        /// </summary>
        /// <param name="Id">The unique identification of the charging station operator.</param>
        /// <param name="OldStatus">The old timestamped status of the charging station operator.</param>
        /// <param name="NewStatus">The new timestamped status of the charging station operator.</param>
        public ChargingStationOperatorStatusUpdate(ChargingStationOperator_Id     Id,
                                                   ChargingStationOperatorStatus  OldStatus,
                                                   ChargingStationOperatorStatus  NewStatus)

        {

            this.Id         = Id;
            this.OldStatus  = OldStatus.TimestampedStatus;
            this.NewStatus  = NewStatus.TimestampedStatus;

        }

        #endregion

        #endregion


        #region (static) Snapshot(ChargingStationOperator)

        /// <summary>
        /// Take a snapshot of the current charging station operator status.
        /// </summary>
        /// <param name="ChargingStationOperator">A charging station operator.</param>
        public static ChargingStationOperatorStatusUpdate Snapshot(ChargingStationOperator ChargingStationOperator)

            => new ChargingStationOperatorStatusUpdate(ChargingStationOperator.Id,
                                                       ChargingStationOperator.Status,
                                                       ChargingStationOperator.StatusSchedule().Skip(1).FirstOrDefault());

        #endregion


        #region Operator overloading

        #region Operator == (ChargingStationOperatorStatusUpdate1, ChargingStationOperatorStatusUpdate2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingStationOperatorStatusUpdate1">A charging station operator status update.</param>
        /// <param name="ChargingStationOperatorStatusUpdate2">Another charging station operator status update.</param>
        /// <returns>true|false</returns>
        public static Boolean operator == (ChargingStationOperatorStatusUpdate ChargingStationOperatorStatusUpdate1, ChargingStationOperatorStatusUpdate ChargingStationOperatorStatusUpdate2)
        {

            // If both are null, or both are same instance, return true.
            if (ReferenceEquals(ChargingStationOperatorStatusUpdate1, ChargingStationOperatorStatusUpdate2))
                return true;

            // If one is null, but not both, return false.
            if (((Object) ChargingStationOperatorStatusUpdate1 == null) || ((Object) ChargingStationOperatorStatusUpdate2 == null))
                return false;

            return ChargingStationOperatorStatusUpdate1.Equals(ChargingStationOperatorStatusUpdate2);

        }

        #endregion

        #region Operator != (ChargingStationOperatorStatusUpdate1, ChargingStationOperatorStatusUpdate2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingStationOperatorStatusUpdate1">A charging station operator status update.</param>
        /// <param name="ChargingStationOperatorStatusUpdate2">Another charging station operator status update.</param>
        /// <returns>true|false</returns>
        public static Boolean operator != (ChargingStationOperatorStatusUpdate ChargingStationOperatorStatusUpdate1, ChargingStationOperatorStatusUpdate ChargingStationOperatorStatusUpdate2)
        {
            return !(ChargingStationOperatorStatusUpdate1 == ChargingStationOperatorStatusUpdate2);
        }

        #endregion

        #region Operator <  (ChargingStationOperatorStatusUpdate1, ChargingStationOperatorStatusUpdate2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingStationOperatorStatusUpdate1">A charging station operator status update.</param>
        /// <param name="ChargingStationOperatorStatusUpdate2">Another charging station operator status update.</param>
        /// <returns>true|false</returns>
        public static Boolean operator < (ChargingStationOperatorStatusUpdate ChargingStationOperatorStatusUpdate1, ChargingStationOperatorStatusUpdate ChargingStationOperatorStatusUpdate2)
        {

            if ((Object) ChargingStationOperatorStatusUpdate1 == null)
                throw new ArgumentNullException("The given ChargingStationOperatorStatusUpdate1 must not be null!");

            return ChargingStationOperatorStatusUpdate1.CompareTo(ChargingStationOperatorStatusUpdate2) < 0;

        }

        #endregion

        #region Operator <= (ChargingStationOperatorStatusUpdate1, ChargingStationOperatorStatusUpdate2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingStationOperatorStatusUpdate1">A charging station operator status update.</param>
        /// <param name="ChargingStationOperatorStatusUpdate2">Another charging station operator status update.</param>
        /// <returns>true|false</returns>
        public static Boolean operator <= (ChargingStationOperatorStatusUpdate ChargingStationOperatorStatusUpdate1, ChargingStationOperatorStatusUpdate ChargingStationOperatorStatusUpdate2)
        {
            return !(ChargingStationOperatorStatusUpdate1 > ChargingStationOperatorStatusUpdate2);
        }

        #endregion

        #region Operator >  (ChargingStationOperatorStatusUpdate1, ChargingStationOperatorStatusUpdate2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingStationOperatorStatusUpdate1">A charging station operator status update.</param>
        /// <param name="ChargingStationOperatorStatusUpdate2">Another charging station operator status update.</param>
        /// <returns>true|false</returns>
        public static Boolean operator > (ChargingStationOperatorStatusUpdate ChargingStationOperatorStatusUpdate1, ChargingStationOperatorStatusUpdate ChargingStationOperatorStatusUpdate2)
        {

            if ((Object) ChargingStationOperatorStatusUpdate1 == null)
                throw new ArgumentNullException("The given ChargingStationOperatorStatusUpdate1 must not be null!");

            return ChargingStationOperatorStatusUpdate1.CompareTo(ChargingStationOperatorStatusUpdate2) > 0;

        }

        #endregion

        #region Operator >= (ChargingStationOperatorStatusUpdate1, ChargingStationOperatorStatusUpdate2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingStationOperatorStatusUpdate1">A charging station operator status update.</param>
        /// <param name="ChargingStationOperatorStatusUpdate2">Another charging station operator status update.</param>
        /// <returns>true|false</returns>
        public static Boolean operator >= (ChargingStationOperatorStatusUpdate ChargingStationOperatorStatusUpdate1, ChargingStationOperatorStatusUpdate ChargingStationOperatorStatusUpdate2)
        {
            return !(ChargingStationOperatorStatusUpdate1 < ChargingStationOperatorStatusUpdate2);
        }

        #endregion

        #endregion

        #region IComparable<ChargingStationOperatorStatusUpdate> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        public Int32 CompareTo(Object Object)
        {

            if (Object == null)
                throw new ArgumentNullException(nameof(Object), "The given object must not be null!");

            if (!(Object is ChargingStationOperatorStatusUpdate))
                throw new ArgumentException("The given object is not a ChargingStationOperatorStatus!",
                                            nameof(Object));

            return CompareTo((ChargingStationOperatorStatusUpdate) Object);

        }

        #endregion

        #region CompareTo(ChargingStationOperatorStatusUpdate)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingStationOperatorStatusUpdate">An object to compare with.</param>
        public Int32 CompareTo(ChargingStationOperatorStatusUpdate ChargingStationOperatorStatusUpdate)
        {

            if ((Object) ChargingStationOperatorStatusUpdate == null)
                throw new ArgumentNullException(nameof(ChargingStationOperatorStatusUpdate), "The given ChargingStationOperator status update must not be null!");

            // Compare ChargingStationOperator Ids
            var _Result = Id.CompareTo(ChargingStationOperatorStatusUpdate.Id);

            // If equal: Compare the new charging station operator status
            if (_Result == 0)
                _Result = NewStatus.CompareTo(ChargingStationOperatorStatusUpdate.NewStatus);

            // If equal: Compare the old ChargingStationOperator status
            if (_Result == 0)
                _Result = OldStatus.CompareTo(ChargingStationOperatorStatusUpdate.OldStatus);

            return _Result;

        }

        #endregion

        #endregion

        #region IEquatable<ChargingStationOperatorStatusUpdate> Members

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

            if (!(Object is ChargingStationOperatorStatusUpdate))
                return false;

            return this.Equals((ChargingStationOperatorStatusUpdate) Object);

        }

        #endregion

        #region Equals(ChargingStationOperatorStatusUpdate)

        /// <summary>
        /// Compares two ChargingStationOperator status updates for equality.
        /// </summary>
        /// <param name="ChargingStationOperatorStatusUpdate">A charging station operator status update to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public Boolean Equals(ChargingStationOperatorStatusUpdate ChargingStationOperatorStatusUpdate)
        {

            if ((Object) ChargingStationOperatorStatusUpdate == null)
                return false;

            return Id.       Equals(ChargingStationOperatorStatusUpdate.Id)        &&
                   OldStatus.Equals(ChargingStationOperatorStatusUpdate.OldStatus) &&
                   NewStatus.Equals(ChargingStationOperatorStatusUpdate.NewStatus);

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
                       OldStatus.GetHashCode() * 5 ^
                       NewStatus.GetHashCode();

            }
        }

        #endregion

        #region (override) ToString()

        /// <summary>
        /// Return a text representation of this object.
        /// </summary>
        public override String ToString()

            => String.Concat(Id, ": ",
                             OldStatus,
                             " -> ",
                             NewStatus);

        #endregion

    }

}
