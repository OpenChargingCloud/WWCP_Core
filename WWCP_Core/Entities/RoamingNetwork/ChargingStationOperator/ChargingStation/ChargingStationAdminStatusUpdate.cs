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
using System.Linq;

using org.GraphDefined.Vanaheimr.Illias;

#endregion

namespace org.GraphDefined.WWCP
{

    /// <summary>
    /// A charging station admin status update.
    /// </summary>
    public struct ChargingStationAdminStatusUpdate : IEquatable <ChargingStationAdminStatusUpdate>,
                                                     IComparable<ChargingStationAdminStatusUpdate>
    {

        #region Properties

        /// <summary>
        /// The unique identification of the charging station.
        /// </summary>
        public ChargingStation_Id                           Id          { get; }

        /// <summary>
        /// The old timestamped status of the charging station.
        /// </summary>
        public Timestamped<ChargingStationAdminStatusTypes>  OldStatus   { get; }

        /// <summary>
        /// The new timestamped status of the charging station.
        /// </summary>
        public Timestamped<ChargingStationAdminStatusTypes>  NewStatus   { get; }

        #endregion

        #region Constructor(s)

        #region ChargingStationAdminStatusUpdate(Id, OldStatus, NewStatus)

        /// <summary>
        /// Create a new charging station admin status update.
        /// </summary>
        /// <param name="Id">The unique identification of the charging station.</param>
        /// <param name="OldStatus">The old timestamped admin status of the charging station.</param>
        /// <param name="NewStatus">The new timestamped admin status of the charging station.</param>
        public ChargingStationAdminStatusUpdate(ChargingStation_Id                           Id,
                                                Timestamped<ChargingStationAdminStatusTypes>  OldStatus,
                                                Timestamped<ChargingStationAdminStatusTypes>  NewStatus)

        {

            this.Id         = Id;
            this.OldStatus  = OldStatus;
            this.NewStatus  = NewStatus;

        }

        #endregion

        #region ChargingStationAdminStatusUpdate(Id, OldStatus, NewStatus)

        /// <summary>
        /// Create a new charging station admin status update.
        /// </summary>
        /// <param name="Id">The unique identification of the charging station.</param>
        /// <param name="OldStatus">The old timestamped admin status of the charging station.</param>
        /// <param name="NewStatus">The new timestamped admin status of the charging station.</param>
        public ChargingStationAdminStatusUpdate(ChargingStation_Id          Id,
                                                ChargingStationAdminStatus  OldStatus,
                                                ChargingStationAdminStatus  NewStatus)

        {

            this.Id         = Id;
            this.OldStatus  = OldStatus.Combined;
            this.NewStatus  = NewStatus.Combined;

        }

        #endregion

        #endregion


        #region (static) Snapshot(ChargingStation)

        /// <summary>
        /// Take a snapshot of the current charging station status.
        /// </summary>
        /// <param name="ChargingStation">A charging station.</param>
        public static ChargingStationAdminStatusUpdate Snapshot(ChargingStation ChargingStation)

            => new ChargingStationAdminStatusUpdate(ChargingStation.Id,
                                                    ChargingStation.AdminStatus,
                                                    ChargingStation.AdminStatusSchedule().Skip(1).FirstOrDefault());

        #endregion


        #region Operator overloading

        #region Operator == (ChargingStationAdminStatusUpdate1, ChargingStationAdminStatusUpdate2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingStationAdminStatusUpdate1">A charging station status update.</param>
        /// <param name="ChargingStationAdminStatusUpdate2">Another charging station status update.</param>
        /// <returns>true|false</returns>
        public static Boolean operator == (ChargingStationAdminStatusUpdate ChargingStationAdminStatusUpdate1, ChargingStationAdminStatusUpdate ChargingStationAdminStatusUpdate2)
        {

            // If both are null, or both are same instance, return true.
            if (Object.ReferenceEquals(ChargingStationAdminStatusUpdate1, ChargingStationAdminStatusUpdate2))
                return true;

            // If one is null, but not both, return false.
            if (((Object) ChargingStationAdminStatusUpdate1 == null) || ((Object) ChargingStationAdminStatusUpdate2 == null))
                return false;

            return ChargingStationAdminStatusUpdate1.Equals(ChargingStationAdminStatusUpdate2);

        }

        #endregion

        #region Operator != (ChargingStationAdminStatusUpdate1, ChargingStationAdminStatusUpdate2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingStationAdminStatusUpdate1">A charging station status update.</param>
        /// <param name="ChargingStationAdminStatusUpdate2">Another charging station status update.</param>
        /// <returns>true|false</returns>
        public static Boolean operator != (ChargingStationAdminStatusUpdate ChargingStationAdminStatusUpdate1, ChargingStationAdminStatusUpdate ChargingStationAdminStatusUpdate2)
            => !(ChargingStationAdminStatusUpdate1 == ChargingStationAdminStatusUpdate2);

        #endregion

        #region Operator <  (ChargingStationAdminStatusUpdate1, ChargingStationAdminStatusUpdate2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingStationAdminStatusUpdate1">A charging station status update.</param>
        /// <param name="ChargingStationAdminStatusUpdate2">Another charging station status update.</param>
        /// <returns>true|false</returns>
        public static Boolean operator < (ChargingStationAdminStatusUpdate ChargingStationAdminStatusUpdate1, ChargingStationAdminStatusUpdate ChargingStationAdminStatusUpdate2)
        {

            if ((Object) ChargingStationAdminStatusUpdate1 == null)
                throw new ArgumentNullException(nameof(ChargingStationAdminStatusUpdate1), "The given ChargingStationAdminStatusUpdate1 must not be null!");

            return ChargingStationAdminStatusUpdate1.CompareTo(ChargingStationAdminStatusUpdate2) < 0;

        }

        #endregion

        #region Operator <= (ChargingStationAdminStatusUpdate1, ChargingStationAdminStatusUpdate2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingStationAdminStatusUpdate1">A charging station status update.</param>
        /// <param name="ChargingStationAdminStatusUpdate2">Another charging station status update.</param>
        /// <returns>true|false</returns>
        public static Boolean operator <= (ChargingStationAdminStatusUpdate ChargingStationAdminStatusUpdate1, ChargingStationAdminStatusUpdate ChargingStationAdminStatusUpdate2)
            => !(ChargingStationAdminStatusUpdate1 > ChargingStationAdminStatusUpdate2);

        #endregion

        #region Operator >  (ChargingStationAdminStatusUpdate1, ChargingStationAdminStatusUpdate2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingStationAdminStatusUpdate1">A charging station status update.</param>
        /// <param name="ChargingStationAdminStatusUpdate2">Another charging station status update.</param>
        /// <returns>true|false</returns>
        public static Boolean operator > (ChargingStationAdminStatusUpdate ChargingStationAdminStatusUpdate1, ChargingStationAdminStatusUpdate ChargingStationAdminStatusUpdate2)
        {

            if ((Object) ChargingStationAdminStatusUpdate1 == null)
                throw new ArgumentNullException(nameof(ChargingStationAdminStatusUpdate1), "The given ChargingStationAdminStatusUpdate1 must not be null!");

            return ChargingStationAdminStatusUpdate1.CompareTo(ChargingStationAdminStatusUpdate2) > 0;

        }

        #endregion

        #region Operator >= (ChargingStationAdminStatusUpdate1, ChargingStationAdminStatusUpdate2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingStationAdminStatusUpdate1">A charging station status update.</param>
        /// <param name="ChargingStationAdminStatusUpdate2">Another charging station status update.</param>
        /// <returns>true|false</returns>
        public static Boolean operator >= (ChargingStationAdminStatusUpdate ChargingStationAdminStatusUpdate1, ChargingStationAdminStatusUpdate ChargingStationAdminStatusUpdate2)
            => !(ChargingStationAdminStatusUpdate1 < ChargingStationAdminStatusUpdate2);

        #endregion

        #endregion

        #region IComparable<ChargingStationAdminStatusUpdate> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        public Int32 CompareTo(Object Object)
        {

            if (Object == null)
                throw new ArgumentNullException(nameof(Object), "The given object must not be null!");

            if (!(Object is ChargingStationAdminStatusUpdate))
                throw new ArgumentException("The given object is not a ChargingStationStatus!",
                                            nameof(Object));

            return CompareTo((ChargingStationAdminStatusUpdate) Object);

        }

        #endregion

        #region CompareTo(ChargingStationAdminStatusUpdate)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingStationAdminStatusUpdate">An object to compare with.</param>
        public Int32 CompareTo(ChargingStationAdminStatusUpdate ChargingStationAdminStatusUpdate)
        {

            if ((Object) ChargingStationAdminStatusUpdate == null)
                throw new ArgumentNullException(nameof(ChargingStationAdminStatusUpdate), "The given ChargingStation status update must not be null!");

            // Compare ChargingStation Ids
            var _Result = Id.CompareTo(ChargingStationAdminStatusUpdate.Id);

            // If equal: Compare the new charging station status
            if (_Result == 0)
                _Result = NewStatus.CompareTo(ChargingStationAdminStatusUpdate.NewStatus);

            // If equal: Compare the old ChargingStation status
            if (_Result == 0)
                _Result = OldStatus.CompareTo(ChargingStationAdminStatusUpdate.OldStatus);

            return _Result;

        }

        #endregion

        #endregion

        #region IEquatable<ChargingStationAdminStatusUpdate> Members

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

            if (!(Object is ChargingStationAdminStatusUpdate))
                return false;

            return this.Equals((ChargingStationAdminStatusUpdate) Object);

        }

        #endregion

        #region Equals(ChargingStationAdminStatusUpdate)

        /// <summary>
        /// Compares two ChargingStation status updates for equality.
        /// </summary>
        /// <param name="ChargingStationAdminStatusUpdate">A charging station status update to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public Boolean Equals(ChargingStationAdminStatusUpdate ChargingStationAdminStatusUpdate)
        {

            if ((Object) ChargingStationAdminStatusUpdate == null)
                return false;

            return Id.       Equals(ChargingStationAdminStatusUpdate.Id)        &&
                   OldStatus.Equals(ChargingStationAdminStatusUpdate.OldStatus) &&
                   NewStatus.Equals(ChargingStationAdminStatusUpdate.NewStatus);

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
        /// Return a string representation of this object.
        /// </summary>
        public override String ToString()

            => String.Concat(Id, ": ",
                             OldStatus,
                             " -> ",
                             NewStatus);

        #endregion

    }

}
