/*
 * Copyright (c) 2014-2018 GraphDefined GmbH <achim.friedland@graphdefined.com>
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
    /// A charging pool status update.
    /// </summary>
    public struct ChargingPoolStatusUpdate : IEquatable <ChargingPoolStatusUpdate>,
                                             IComparable<ChargingPoolStatusUpdate>
    {

        #region Properties

        /// <summary>
        /// The unique identification of the charging pool.
        /// </summary>
        public ChargingPool_Id                       Id          { get; }

        /// <summary>
        /// The old timestamped status of the charging pool.
        /// </summary>
        public Timestamped<ChargingPoolStatusTypes>  OldStatus   { get; }

        /// <summary>
        /// The new timestamped status of the charging pool.
        /// </summary>
        public Timestamped<ChargingPoolStatusTypes>  NewStatus   { get; }

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new charging pool status update.
        /// </summary>
        /// <param name="Id">The unique identification of the charging pool.</param>
        /// <param name="OldStatus">The old timestamped status of the charging pool.</param>
        /// <param name="NewStatus">The new timestamped status of the charging pool.</param>
        public ChargingPoolStatusUpdate(ChargingPool_Id                       Id,
                                        Timestamped<ChargingPoolStatusTypes>  OldStatus,
                                        Timestamped<ChargingPoolStatusTypes>  NewStatus)

        {

            this.Id         = Id;
            this.OldStatus  = OldStatus;
            this.NewStatus  = NewStatus;

        }

        #endregion


        #region (static) Snapshot(ChargingPool)

        /// <summary>
        /// Take a snapshot of the current charging pool status.
        /// </summary>
        /// <param name="ChargingPool">A charging pool.</param>
        public static ChargingPoolStatusUpdate Snapshot(ChargingPool ChargingPool)

            => new ChargingPoolStatusUpdate(ChargingPool.Id,
                                            ChargingPool.Status,
                                            ChargingPool.StatusSchedule().Skip(1).FirstOrDefault());

        #endregion


        #region Operator overloading

        #region Operator == (ChargingPoolStatusUpdate1, ChargingPoolStatusUpdate2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingPoolStatusUpdate1">A charging pool status update.</param>
        /// <param name="ChargingPoolStatusUpdate2">Another charging pool status update.</param>
        /// <returns>true|false</returns>
        public static Boolean operator == (ChargingPoolStatusUpdate ChargingPoolStatusUpdate1, ChargingPoolStatusUpdate ChargingPoolStatusUpdate2)
        {

            // If both are null, or both are same instance, return true.
            if (Object.ReferenceEquals(ChargingPoolStatusUpdate1, ChargingPoolStatusUpdate2))
                return true;

            // If one is null, but not both, return false.
            if (((Object) ChargingPoolStatusUpdate1 == null) || ((Object) ChargingPoolStatusUpdate2 == null))
                return false;

            return ChargingPoolStatusUpdate1.Equals(ChargingPoolStatusUpdate2);

        }

        #endregion

        #region Operator != (ChargingPoolStatusUpdate1, ChargingPoolStatusUpdate2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingPoolStatusUpdate1">A charging pool status update.</param>
        /// <param name="ChargingPoolStatusUpdate2">Another charging pool status update.</param>
        /// <returns>true|false</returns>
        public static Boolean operator != (ChargingPoolStatusUpdate ChargingPoolStatusUpdate1, ChargingPoolStatusUpdate ChargingPoolStatusUpdate2)
        {
            return !(ChargingPoolStatusUpdate1 == ChargingPoolStatusUpdate2);
        }

        #endregion

        #region Operator <  (ChargingPoolStatusUpdate1, ChargingPoolStatusUpdate2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingPoolStatusUpdate1">A charging pool status update.</param>
        /// <param name="ChargingPoolStatusUpdate2">Another charging pool status update.</param>
        /// <returns>true|false</returns>
        public static Boolean operator < (ChargingPoolStatusUpdate ChargingPoolStatusUpdate1, ChargingPoolStatusUpdate ChargingPoolStatusUpdate2)
        {

            if ((Object) ChargingPoolStatusUpdate1 == null)
                throw new ArgumentNullException("The given ChargingPoolStatusUpdate1 must not be null!");

            return ChargingPoolStatusUpdate1.CompareTo(ChargingPoolStatusUpdate2) < 0;

        }

        #endregion

        #region Operator <= (ChargingPoolStatusUpdate1, ChargingPoolStatusUpdate2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingPoolStatusUpdate1">A charging pool status update.</param>
        /// <param name="ChargingPoolStatusUpdate2">Another charging pool status update.</param>
        /// <returns>true|false</returns>
        public static Boolean operator <= (ChargingPoolStatusUpdate ChargingPoolStatusUpdate1, ChargingPoolStatusUpdate ChargingPoolStatusUpdate2)
        {
            return !(ChargingPoolStatusUpdate1 > ChargingPoolStatusUpdate2);
        }

        #endregion

        #region Operator >  (ChargingPoolStatusUpdate1, ChargingPoolStatusUpdate2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingPoolStatusUpdate1">A charging pool status update.</param>
        /// <param name="ChargingPoolStatusUpdate2">Another charging pool status update.</param>
        /// <returns>true|false</returns>
        public static Boolean operator > (ChargingPoolStatusUpdate ChargingPoolStatusUpdate1, ChargingPoolStatusUpdate ChargingPoolStatusUpdate2)
        {

            if ((Object) ChargingPoolStatusUpdate1 == null)
                throw new ArgumentNullException("The given ChargingPoolStatusUpdate1 must not be null!");

            return ChargingPoolStatusUpdate1.CompareTo(ChargingPoolStatusUpdate2) > 0;

        }

        #endregion

        #region Operator >= (ChargingPoolStatusUpdate1, ChargingPoolStatusUpdate2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingPoolStatusUpdate1">A charging pool status update.</param>
        /// <param name="ChargingPoolStatusUpdate2">Another charging pool status update.</param>
        /// <returns>true|false</returns>
        public static Boolean operator >= (ChargingPoolStatusUpdate ChargingPoolStatusUpdate1, ChargingPoolStatusUpdate ChargingPoolStatusUpdate2)
        {
            return !(ChargingPoolStatusUpdate1 < ChargingPoolStatusUpdate2);
        }

        #endregion

        #endregion

        #region IComparable<ChargingPoolStatusUpdate> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        public Int32 CompareTo(Object Object)
        {

            if (Object == null)
                throw new ArgumentNullException(nameof(Object), "The given object must not be null!");

            if (!(Object is ChargingPoolStatusUpdate))
                throw new ArgumentException("The given object is not a ChargingPoolStatus!",
                                            nameof(Object));

            return CompareTo((ChargingPoolStatusUpdate) Object);

        }

        #endregion

        #region CompareTo(ChargingPoolStatusUpdate)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingPoolStatusUpdate">An object to compare with.</param>
        public Int32 CompareTo(ChargingPoolStatusUpdate ChargingPoolStatusUpdate)
        {

            if ((Object) ChargingPoolStatusUpdate == null)
                throw new ArgumentNullException(nameof(ChargingPoolStatusUpdate), "The given ChargingPool status update must not be null!");

            // Compare ChargingPool Ids
            var _Result = Id.CompareTo(ChargingPoolStatusUpdate.Id);

            // If equal: Compare the new charging pool status
            if (_Result == 0)
                _Result = NewStatus.CompareTo(ChargingPoolStatusUpdate.NewStatus);

            // If equal: Compare the old ChargingPool status
            if (_Result == 0)
                _Result = OldStatus.CompareTo(ChargingPoolStatusUpdate.OldStatus);

            return _Result;

        }

        #endregion

        #endregion

        #region IEquatable<ChargingPoolStatusUpdate> Members

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

            if (!(Object is ChargingPoolStatusUpdate))
                return false;

            return this.Equals((ChargingPoolStatusUpdate) Object);

        }

        #endregion

        #region Equals(ChargingPoolStatusUpdate)

        /// <summary>
        /// Compares two ChargingPool status updates for equality.
        /// </summary>
        /// <param name="ChargingPoolStatusUpdate">A charging pool status update to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public Boolean Equals(ChargingPoolStatusUpdate ChargingPoolStatusUpdate)
        {

            if ((Object) ChargingPoolStatusUpdate == null)
                return false;

            return Id.       Equals(ChargingPoolStatusUpdate.Id)        &&
                   OldStatus.Equals(ChargingPoolStatusUpdate.OldStatus) &&
                   NewStatus.Equals(ChargingPoolStatusUpdate.NewStatus);

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
