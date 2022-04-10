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

namespace org.GraphDefined.WWCP
{

    /// <summary>
    /// A charging pool admin status update.
    /// </summary>
    public struct ChargingPoolAdminStatusUpdate : IEquatable <ChargingPoolAdminStatusUpdate>,
                                                     IComparable<ChargingPoolAdminStatusUpdate>
    {

        #region Properties

        /// <summary>
        /// The unique identification of the charging pool.
        /// </summary>
        public ChargingPool_Id                            Id          { get; }

        /// <summary>
        /// The old timestamped status of the charging pool.
        /// </summary>
        public Timestamped<ChargingPoolAdminStatusTypes>  OldStatus   { get; }

        /// <summary>
        /// The new timestamped status of the charging pool.
        /// </summary>
        public Timestamped<ChargingPoolAdminStatusTypes>  NewStatus   { get; }

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new charging pool admin status update.
        /// </summary>
        /// <param name="Id">The unique identification of the charging pool.</param>
        /// <param name="OldStatus">The old timestamped admin status of the charging pool.</param>
        /// <param name="NewStatus">The new timestamped admin status of the charging pool.</param>
        public ChargingPoolAdminStatusUpdate(ChargingPool_Id                           Id,
                                             Timestamped<ChargingPoolAdminStatusTypes>  OldStatus,
                                             Timestamped<ChargingPoolAdminStatusTypes>  NewStatus)

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
        public static ChargingPoolAdminStatusUpdate Snapshot(ChargingPool ChargingPool)

            => new ChargingPoolAdminStatusUpdate(ChargingPool.Id,
                                                 ChargingPool.AdminStatus,
                                                 ChargingPool.AdminStatusSchedule().Skip(1).FirstOrDefault());

        #endregion


        #region Operator overloading

        #region Operator == (ChargingPoolAdminStatusUpdate1, ChargingPoolAdminStatusUpdate2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingPoolAdminStatusUpdate1">A charging pool status update.</param>
        /// <param name="ChargingPoolAdminStatusUpdate2">Another charging pool status update.</param>
        /// <returns>true|false</returns>
        public static Boolean operator == (ChargingPoolAdminStatusUpdate ChargingPoolAdminStatusUpdate1, ChargingPoolAdminStatusUpdate ChargingPoolAdminStatusUpdate2)
        {

            // If both are null, or both are same instance, return true.
            if (ReferenceEquals(ChargingPoolAdminStatusUpdate1, ChargingPoolAdminStatusUpdate2))
                return true;

            // If one is null, but not both, return false.
            if (((Object) ChargingPoolAdminStatusUpdate1 == null) || ((Object) ChargingPoolAdminStatusUpdate2 == null))
                return false;

            return ChargingPoolAdminStatusUpdate1.Equals(ChargingPoolAdminStatusUpdate2);

        }

        #endregion

        #region Operator != (ChargingPoolAdminStatusUpdate1, ChargingPoolAdminStatusUpdate2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingPoolAdminStatusUpdate1">A charging pool status update.</param>
        /// <param name="ChargingPoolAdminStatusUpdate2">Another charging pool status update.</param>
        /// <returns>true|false</returns>
        public static Boolean operator != (ChargingPoolAdminStatusUpdate ChargingPoolAdminStatusUpdate1, ChargingPoolAdminStatusUpdate ChargingPoolAdminStatusUpdate2)
            => !(ChargingPoolAdminStatusUpdate1 == ChargingPoolAdminStatusUpdate2);

        #endregion

        #region Operator <  (ChargingPoolAdminStatusUpdate1, ChargingPoolAdminStatusUpdate2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingPoolAdminStatusUpdate1">A charging pool status update.</param>
        /// <param name="ChargingPoolAdminStatusUpdate2">Another charging pool status update.</param>
        /// <returns>true|false</returns>
        public static Boolean operator < (ChargingPoolAdminStatusUpdate ChargingPoolAdminStatusUpdate1, ChargingPoolAdminStatusUpdate ChargingPoolAdminStatusUpdate2)
        {

            if ((Object) ChargingPoolAdminStatusUpdate1 == null)
                throw new ArgumentNullException(nameof(ChargingPoolAdminStatusUpdate1), "The given ChargingPoolAdminStatusUpdate1 must not be null!");

            return ChargingPoolAdminStatusUpdate1.CompareTo(ChargingPoolAdminStatusUpdate2) < 0;

        }

        #endregion

        #region Operator <= (ChargingPoolAdminStatusUpdate1, ChargingPoolAdminStatusUpdate2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingPoolAdminStatusUpdate1">A charging pool status update.</param>
        /// <param name="ChargingPoolAdminStatusUpdate2">Another charging pool status update.</param>
        /// <returns>true|false</returns>
        public static Boolean operator <= (ChargingPoolAdminStatusUpdate ChargingPoolAdminStatusUpdate1, ChargingPoolAdminStatusUpdate ChargingPoolAdminStatusUpdate2)
            => !(ChargingPoolAdminStatusUpdate1 > ChargingPoolAdminStatusUpdate2);

        #endregion

        #region Operator >  (ChargingPoolAdminStatusUpdate1, ChargingPoolAdminStatusUpdate2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingPoolAdminStatusUpdate1">A charging pool status update.</param>
        /// <param name="ChargingPoolAdminStatusUpdate2">Another charging pool status update.</param>
        /// <returns>true|false</returns>
        public static Boolean operator > (ChargingPoolAdminStatusUpdate ChargingPoolAdminStatusUpdate1, ChargingPoolAdminStatusUpdate ChargingPoolAdminStatusUpdate2)
        {

            if ((Object) ChargingPoolAdminStatusUpdate1 == null)
                throw new ArgumentNullException(nameof(ChargingPoolAdminStatusUpdate1), "The given ChargingPoolAdminStatusUpdate1 must not be null!");

            return ChargingPoolAdminStatusUpdate1.CompareTo(ChargingPoolAdminStatusUpdate2) > 0;

        }

        #endregion

        #region Operator >= (ChargingPoolAdminStatusUpdate1, ChargingPoolAdminStatusUpdate2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingPoolAdminStatusUpdate1">A charging pool status update.</param>
        /// <param name="ChargingPoolAdminStatusUpdate2">Another charging pool status update.</param>
        /// <returns>true|false</returns>
        public static Boolean operator >= (ChargingPoolAdminStatusUpdate ChargingPoolAdminStatusUpdate1, ChargingPoolAdminStatusUpdate ChargingPoolAdminStatusUpdate2)
            => !(ChargingPoolAdminStatusUpdate1 < ChargingPoolAdminStatusUpdate2);

        #endregion

        #endregion

        #region IComparable<ChargingPoolAdminStatusUpdate> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        public Int32 CompareTo(Object Object)
        {

            if (Object == null)
                throw new ArgumentNullException(nameof(Object), "The given object must not be null!");

            if (!(Object is ChargingPoolAdminStatusUpdate))
                throw new ArgumentException("The given object is not a ChargingPoolStatus!",
                                            nameof(Object));

            return CompareTo((ChargingPoolAdminStatusUpdate) Object);

        }

        #endregion

        #region CompareTo(ChargingPoolAdminStatusUpdate)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingPoolAdminStatusUpdate">An object to compare with.</param>
        public Int32 CompareTo(ChargingPoolAdminStatusUpdate ChargingPoolAdminStatusUpdate)
        {

            if ((Object) ChargingPoolAdminStatusUpdate == null)
                throw new ArgumentNullException(nameof(ChargingPoolAdminStatusUpdate), "The given ChargingPool status update must not be null!");

            // Compare ChargingPool Ids
            var _Result = Id.CompareTo(ChargingPoolAdminStatusUpdate.Id);

            // If equal: Compare the new charging pool status
            if (_Result == 0)
                _Result = NewStatus.CompareTo(ChargingPoolAdminStatusUpdate.NewStatus);

            // If equal: Compare the old ChargingPool status
            if (_Result == 0)
                _Result = OldStatus.CompareTo(ChargingPoolAdminStatusUpdate.OldStatus);

            return _Result;

        }

        #endregion

        #endregion

        #region IEquatable<ChargingPoolAdminStatusUpdate> Members

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

            if (!(Object is ChargingPoolAdminStatusUpdate))
                return false;

            return this.Equals((ChargingPoolAdminStatusUpdate) Object);

        }

        #endregion

        #region Equals(ChargingPoolAdminStatusUpdate)

        /// <summary>
        /// Compares two ChargingPool status updates for equality.
        /// </summary>
        /// <param name="ChargingPoolAdminStatusUpdate">A charging pool status update to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public Boolean Equals(ChargingPoolAdminStatusUpdate ChargingPoolAdminStatusUpdate)
        {

            if ((Object) ChargingPoolAdminStatusUpdate == null)
                return false;

            return Id.       Equals(ChargingPoolAdminStatusUpdate.Id)        &&
                   OldStatus.Equals(ChargingPoolAdminStatusUpdate.OldStatus) &&
                   NewStatus.Equals(ChargingPoolAdminStatusUpdate.NewStatus);

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
