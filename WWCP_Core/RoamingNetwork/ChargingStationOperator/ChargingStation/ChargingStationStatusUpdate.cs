/*
 * Copyright (c) 2014-2021 GraphDefined GmbH <achim.friedland@graphdefined.com>
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
    /// A charging station status update.
    /// </summary>
    public struct ChargingStationStatusUpdate : IEquatable <ChargingStationStatusUpdate>,
                                                IComparable<ChargingStationStatusUpdate>
    {

        #region Properties

        /// <summary>
        /// The unique identification of the charging station.
        /// </summary>
        public ChargingStation_Id                       Id          { get; }

        /// <summary>
        /// The old timestamped status of the charging station.
        /// </summary>
        public Timestamped<ChargingStationStatusTypes>  OldStatus   { get; }

        /// <summary>
        /// The new timestamped status of the charging station.
        /// </summary>
        public Timestamped<ChargingStationStatusTypes>  NewStatus   { get; }

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new charging station status update.
        /// </summary>
        /// <param name="Id">The unique identification of the charging station.</param>
        /// <param name="OldStatus">The old timestamped status of the charging station.</param>
        /// <param name="NewStatus">The new timestamped status of the charging station.</param>
        public ChargingStationStatusUpdate(ChargingStation_Id                       Id,
                                           Timestamped<ChargingStationStatusTypes>  OldStatus,
                                           Timestamped<ChargingStationStatusTypes>  NewStatus)

        {

            this.Id         = Id;
            this.OldStatus  = OldStatus;
            this.NewStatus  = NewStatus;

        }

        #endregion


        #region (static) Snapshot(ChargingStation)

        /// <summary>
        /// Take a snapshot of the current charging station status.
        /// </summary>
        /// <param name="ChargingStation">A charging station.</param>
        public static ChargingStationStatusUpdate Snapshot(ChargingStation ChargingStation)

            => new ChargingStationStatusUpdate(ChargingStation.Id,
                                               ChargingStation.Status,
                                               ChargingStation.StatusSchedule().Skip(1).FirstOrDefault());

        #endregion


        #region Operator overloading

        #region Operator == (ChargingStationStatusUpdate1, ChargingStationStatusUpdate2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingStationStatusUpdate1">A charging station status update.</param>
        /// <param name="ChargingStationStatusUpdate2">Another charging station status update.</param>
        /// <returns>true|false</returns>
        public static Boolean operator == (ChargingStationStatusUpdate ChargingStationStatusUpdate1, ChargingStationStatusUpdate ChargingStationStatusUpdate2)
        {

            // If both are null, or both are same instance, return true.
            if (ReferenceEquals(ChargingStationStatusUpdate1, ChargingStationStatusUpdate2))
                return true;

            // If one is null, but not both, return false.
            if (((Object) ChargingStationStatusUpdate1 == null) || ((Object) ChargingStationStatusUpdate2 == null))
                return false;

            return ChargingStationStatusUpdate1.Equals(ChargingStationStatusUpdate2);

        }

        #endregion

        #region Operator != (ChargingStationStatusUpdate1, ChargingStationStatusUpdate2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingStationStatusUpdate1">A charging station status update.</param>
        /// <param name="ChargingStationStatusUpdate2">Another charging station status update.</param>
        /// <returns>true|false</returns>
        public static Boolean operator != (ChargingStationStatusUpdate ChargingStationStatusUpdate1, ChargingStationStatusUpdate ChargingStationStatusUpdate2)
        {
            return !(ChargingStationStatusUpdate1 == ChargingStationStatusUpdate2);
        }

        #endregion

        #region Operator <  (ChargingStationStatusUpdate1, ChargingStationStatusUpdate2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingStationStatusUpdate1">A charging station status update.</param>
        /// <param name="ChargingStationStatusUpdate2">Another charging station status update.</param>
        /// <returns>true|false</returns>
        public static Boolean operator < (ChargingStationStatusUpdate ChargingStationStatusUpdate1, ChargingStationStatusUpdate ChargingStationStatusUpdate2)
        {

            if ((Object) ChargingStationStatusUpdate1 == null)
                throw new ArgumentNullException("The given ChargingStationStatusUpdate1 must not be null!");

            return ChargingStationStatusUpdate1.CompareTo(ChargingStationStatusUpdate2) < 0;

        }

        #endregion

        #region Operator <= (ChargingStationStatusUpdate1, ChargingStationStatusUpdate2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingStationStatusUpdate1">A charging station status update.</param>
        /// <param name="ChargingStationStatusUpdate2">Another charging station status update.</param>
        /// <returns>true|false</returns>
        public static Boolean operator <= (ChargingStationStatusUpdate ChargingStationStatusUpdate1, ChargingStationStatusUpdate ChargingStationStatusUpdate2)
        {
            return !(ChargingStationStatusUpdate1 > ChargingStationStatusUpdate2);
        }

        #endregion

        #region Operator >  (ChargingStationStatusUpdate1, ChargingStationStatusUpdate2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingStationStatusUpdate1">A charging station status update.</param>
        /// <param name="ChargingStationStatusUpdate2">Another charging station status update.</param>
        /// <returns>true|false</returns>
        public static Boolean operator > (ChargingStationStatusUpdate ChargingStationStatusUpdate1, ChargingStationStatusUpdate ChargingStationStatusUpdate2)
        {

            if ((Object) ChargingStationStatusUpdate1 == null)
                throw new ArgumentNullException("The given ChargingStationStatusUpdate1 must not be null!");

            return ChargingStationStatusUpdate1.CompareTo(ChargingStationStatusUpdate2) > 0;

        }

        #endregion

        #region Operator >= (ChargingStationStatusUpdate1, ChargingStationStatusUpdate2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingStationStatusUpdate1">A charging station status update.</param>
        /// <param name="ChargingStationStatusUpdate2">Another charging station status update.</param>
        /// <returns>true|false</returns>
        public static Boolean operator >= (ChargingStationStatusUpdate ChargingStationStatusUpdate1, ChargingStationStatusUpdate ChargingStationStatusUpdate2)
        {
            return !(ChargingStationStatusUpdate1 < ChargingStationStatusUpdate2);
        }

        #endregion

        #endregion

        #region IComparable<ChargingStationStatusUpdate> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        public Int32 CompareTo(Object Object)
        {

            if (Object == null)
                throw new ArgumentNullException(nameof(Object), "The given object must not be null!");

            if (!(Object is ChargingStationStatusUpdate))
                throw new ArgumentException("The given object is not a ChargingStationStatus!",
                                            nameof(Object));

            return CompareTo((ChargingStationStatusUpdate) Object);

        }

        #endregion

        #region CompareTo(ChargingStationStatusUpdate)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingStationStatusUpdate">An object to compare with.</param>
        public Int32 CompareTo(ChargingStationStatusUpdate ChargingStationStatusUpdate)
        {

            if ((Object) ChargingStationStatusUpdate == null)
                throw new ArgumentNullException(nameof(ChargingStationStatusUpdate), "The given ChargingStation status update must not be null!");

            // Compare ChargingStation Ids
            var _Result = Id.CompareTo(ChargingStationStatusUpdate.Id);

            // If equal: Compare the new charging station status
            if (_Result == 0)
                _Result = NewStatus.CompareTo(ChargingStationStatusUpdate.NewStatus);

            // If equal: Compare the old ChargingStation status
            if (_Result == 0)
                _Result = OldStatus.CompareTo(ChargingStationStatusUpdate.OldStatus);

            return _Result;

        }

        #endregion

        #endregion

        #region IEquatable<ChargingStationStatusUpdate> Members

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

            if (!(Object is ChargingStationStatusUpdate))
                return false;

            return this.Equals((ChargingStationStatusUpdate) Object);

        }

        #endregion

        #region Equals(ChargingStationStatusUpdate)

        /// <summary>
        /// Compares two ChargingStation status updates for equality.
        /// </summary>
        /// <param name="ChargingStationStatusUpdate">A charging station status update to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public Boolean Equals(ChargingStationStatusUpdate ChargingStationStatusUpdate)
        {

            if ((Object) ChargingStationStatusUpdate == null)
                return false;

            return Id.       Equals(ChargingStationStatusUpdate.Id)        &&
                   OldStatus.Equals(ChargingStationStatusUpdate.OldStatus) &&
                   NewStatus.Equals(ChargingStationStatusUpdate.NewStatus);

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
