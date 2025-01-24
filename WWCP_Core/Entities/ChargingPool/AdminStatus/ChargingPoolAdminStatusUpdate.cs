/*
 * Copyright (c) 2014-2025 GraphDefined GmbH <achim.friedland@graphdefined.com>
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

using org.GraphDefined.Vanaheimr.Illias;

#endregion

namespace cloud.charging.open.protocols.WWCP
{

    /// <summary>
    /// A charging pool admin status update.
    /// </summary>
    public struct ChargingPoolAdminStatusUpdate : IEquatable<ChargingPoolAdminStatusUpdate>,
                                                  IComparable<ChargingPoolAdminStatusUpdate>,
                                                  IComparable
    {

        #region Properties

        /// <summary>
        /// The unique identification of the charging pool.
        /// </summary>
        public ChargingPool_Id                             Id           { get; }

        /// <summary>
        /// The new timestamped admin status of the charging pool.
        /// </summary>
        public Timestamped<ChargingPoolAdminStatusType>   NewStatus    { get; }

        /// <summary>
        /// The optional old timestamped admin status of the charging pool.
        /// </summary>
        public Timestamped<ChargingPoolAdminStatusType>?  OldStatus    { get; }

        /// <summary>
        /// An optional data source or context for this charging pool admin status change.
        /// </summary>
        public Context?                                    DataSource    { get; }

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new charging pool admin status update.
        /// </summary>
        /// <param name="Id">The unique identification of the charging pool.</param>
        /// <param name="OldStatus">The old timestamped admin status of the charging pool.</param>
        /// <param name="NewStatus">The new timestamped admin status of the charging pool.</param>
        /// <param name="DataSource">An optional data source or context for the charging pool admin status update.</param>
        public ChargingPoolAdminStatusUpdate(ChargingPool_Id                             Id,
                                             Timestamped<ChargingPoolAdminStatusType>   NewStatus,
                                             Timestamped<ChargingPoolAdminStatusType>?  OldStatus    = null,
                                             Context?                                    DataSource   = null)

        {

            this.Id          = Id;
            this.NewStatus   = NewStatus;
            this.OldStatus   = OldStatus;
            this.DataSource  = DataSource;

            unchecked
            {

                hashCode = Id.         GetHashCode()       * 7 ^
                           NewStatus.  GetHashCode()       * 5 ^
                          (OldStatus?. GetHashCode() ?? 0) * 3 ^
                          (DataSource?.GetHashCode() ?? 0);

            }

        }

        #endregion


        #region (static) Snapshot(ChargingPool, DataSource = null)

        /// <summary>
        /// Take a snapshot of the current charging pool admin status.
        /// </summary>
        /// <param name="ChargingPool">A charging pool.</param>
        /// <param name="DataSource">An optional data source or context for the charging pool admin status update.</param>
        public static ChargingPoolAdminStatusUpdate Snapshot(IChargingPool  ChargingPool,
                                                             Context?       DataSource   = null)

            => new (ChargingPool.Id,
                    ChargingPool.AdminStatus,
                    ChargingPool.AdminStatusSchedule().Skip(1).FirstOrDefault(),
                    DataSource);

        #endregion


        #region Operator overloading

        #region Operator == (ChargingPoolAdminStatusUpdate1, ChargingPoolAdminStatusUpdate2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingPoolAdminStatusUpdate1">A charging pool admin status update.</param>
        /// <param name="ChargingPoolAdminStatusUpdate2">Another charging pool admin status update.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator == (ChargingPoolAdminStatusUpdate ChargingPoolAdminStatusUpdate1,
                                           ChargingPoolAdminStatusUpdate ChargingPoolAdminStatusUpdate2)

            => ChargingPoolAdminStatusUpdate1.Equals(ChargingPoolAdminStatusUpdate2);

        #endregion

        #region Operator != (ChargingPoolAdminStatusUpdate1, ChargingPoolAdminStatusUpdate2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingPoolAdminStatusUpdate1">A charging pool admin status update.</param>
        /// <param name="ChargingPoolAdminStatusUpdate2">Another charging pool admin status update.</param>
        /// <returns>False if both match; True otherwise.</returns>
        public static Boolean operator != (ChargingPoolAdminStatusUpdate ChargingPoolAdminStatusUpdate1,
                                           ChargingPoolAdminStatusUpdate ChargingPoolAdminStatusUpdate2)

            => !ChargingPoolAdminStatusUpdate1.Equals(ChargingPoolAdminStatusUpdate2);

        #endregion

        #region Operator <  (ChargingPoolAdminStatusUpdate1, ChargingPoolAdminStatusUpdate2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingPoolAdminStatusUpdate1">A charging pool admin status update.</param>
        /// <param name="ChargingPoolAdminStatusUpdate2">Another charging pool admin status update.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator < (ChargingPoolAdminStatusUpdate ChargingPoolAdminStatusUpdate1,
                                          ChargingPoolAdminStatusUpdate ChargingPoolAdminStatusUpdate2)

            => ChargingPoolAdminStatusUpdate1.CompareTo(ChargingPoolAdminStatusUpdate2) < 0;

        #endregion

        #region Operator <= (ChargingPoolAdminStatusUpdate1, ChargingPoolAdminStatusUpdate2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingPoolAdminStatusUpdate1">A charging pool admin status update.</param>
        /// <param name="ChargingPoolAdminStatusUpdate2">Another charging pool admin status update.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator <= (ChargingPoolAdminStatusUpdate ChargingPoolAdminStatusUpdate1,
                                           ChargingPoolAdminStatusUpdate ChargingPoolAdminStatusUpdate2)

            => ChargingPoolAdminStatusUpdate1.CompareTo(ChargingPoolAdminStatusUpdate2) <= 0;

        #endregion

        #region Operator >  (ChargingPoolAdminStatusUpdate1, ChargingPoolAdminStatusUpdate2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingPoolAdminStatusUpdate1">A charging pool admin status update.</param>
        /// <param name="ChargingPoolAdminStatusUpdate2">Another charging pool admin status update.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator > (ChargingPoolAdminStatusUpdate ChargingPoolAdminStatusUpdate1,
                                          ChargingPoolAdminStatusUpdate ChargingPoolAdminStatusUpdate2)

            => ChargingPoolAdminStatusUpdate1.CompareTo(ChargingPoolAdminStatusUpdate2) > 0;

        #endregion

        #region Operator >= (ChargingPoolAdminStatusUpdate1, ChargingPoolAdminStatusUpdate2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingPoolAdminStatusUpdate1">A charging pool admin status update.</param>
        /// <param name="ChargingPoolAdminStatusUpdate2">Another charging pool admin status update.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator >= (ChargingPoolAdminStatusUpdate ChargingPoolAdminStatusUpdate1,
                                           ChargingPoolAdminStatusUpdate ChargingPoolAdminStatusUpdate2)

            => ChargingPoolAdminStatusUpdate1.CompareTo(ChargingPoolAdminStatusUpdate2) >= 0;

        #endregion

        #endregion

        #region IComparable<ChargingPoolAdminStatusUpdate> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two charging pool admin status updates.
        /// </summary>
        /// <param name="Object">A charging pool admin status update to compare with.</param>
        public Int32 CompareTo(Object? Object)

            => Object is ChargingPoolAdminStatusUpdate chargingPoolAdminStatusUpdate
                   ? CompareTo(chargingPoolAdminStatusUpdate)
                   : throw new ArgumentException("The given object is not a charging pool admin status update!",
                                                 nameof(Object));

        #endregion

        #region CompareTo(ChargingPoolAdminStatusUpdate)

        /// <summary>
        /// Compares two charging pool admin status updates.
        /// </summary>
        /// <param name="ChargingPoolAdminStatusUpdate">A charging pool admin status update to compare with.</param>
        public Int32 CompareTo(ChargingPoolAdminStatusUpdate ChargingPoolAdminStatusUpdate)
        {

            var c = Id.             CompareTo(ChargingPoolAdminStatusUpdate.Id);

            if (c == 0)
                c = NewStatus.      CompareTo(ChargingPoolAdminStatusUpdate.NewStatus);

            if (c == 0 && OldStatus.HasValue && ChargingPoolAdminStatusUpdate.OldStatus.HasValue)
                c = OldStatus.Value.CompareTo(ChargingPoolAdminStatusUpdate.OldStatus.Value);

            if (c == 0 && DataSource is not null && ChargingPoolAdminStatusUpdate.DataSource is not null)
                c = DataSource.     CompareTo(ChargingPoolAdminStatusUpdate.DataSource);

            return c;

        }

        #endregion

        #endregion

        #region IEquatable<ChargingPoolAdminStatusUpdate> Members

        #region Equals(Object)

        /// <summary>
        /// Compares two charging pool admin status updates for equality.
        /// </summary>
        /// <param name="Object">A charging pool admin status update to compare with.</param>
        public override Boolean Equals(Object? Object)

            => Object is ChargingPoolAdminStatusUpdate chargingPoolAdminStatusUpdate &&
                   Equals(chargingPoolAdminStatusUpdate);

        #endregion

        #region Equals(ChargingPoolAdminStatusUpdate)

        /// <summary>
        /// Compares two charging pool admin status updates for equality.
        /// </summary>
        /// <param name="ChargingPoolAdminStatusUpdate">A charging pool admin status update to compare with.</param>
        public Boolean Equals(ChargingPoolAdminStatusUpdate ChargingPoolAdminStatusUpdate)

            => Id.       Equals(ChargingPoolAdminStatusUpdate.Id)        &&
               NewStatus.Equals(ChargingPoolAdminStatusUpdate.NewStatus) &&

            ((!OldStatus.HasValue     && !ChargingPoolAdminStatusUpdate.OldStatus.HasValue) ||
              (OldStatus.HasValue     &&  ChargingPoolAdminStatusUpdate.OldStatus.HasValue     && OldStatus.Value.Equals(ChargingPoolAdminStatusUpdate.OldStatus.Value))) &&

            (( DataSource is null     &&  ChargingPoolAdminStatusUpdate.DataSource is null) ||
              (DataSource is not null &&  ChargingPoolAdminStatusUpdate.DataSource is not null && DataSource.     Equals(ChargingPoolAdminStatusUpdate.DataSource)));

        #endregion

        #endregion

        #region (override) GetHashCode()

        private readonly Int32 hashCode;

        /// <summary>
        /// Return the hash code of this object.
        /// </summary>
        /// <returns>The hash code of this object.</returns>
        public override Int32 GetHashCode()
            => hashCode;

        #endregion

        #region (override) ToString()

        /// <summary>
        /// Return a text representation of this object.
        /// </summary>
        public override String ToString()

            => $"{Id}: {(OldStatus.HasValue ? $"'{OldStatus.Value}' -> " : "")}'{NewStatus}'{(DataSource is not null ? $" ({DataSource})" : "")}";

        #endregion

    }

}
