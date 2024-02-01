/*
 * Copyright (c) 2014-2024 GraphDefined GmbH <achim.friedland@graphdefined.com>
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
    /// A charging pool status update.
    /// </summary>
    public struct ChargingPoolStatusUpdate : IEquatable <ChargingPoolStatusUpdate>,
                                             IComparable<ChargingPoolStatusUpdate>,
                                             IComparable
    {

        #region Properties

        /// <summary>
        /// The unique identification of the charging pool.
        /// </summary>
        public ChargingPool_Id                        Id            { get; }

        /// <summary>
        /// The new timestamped status of the charging pool.
        /// </summary>
        public Timestamped<ChargingPoolStatusTypes>   NewStatus     { get; }

        /// <summary>
        /// The optional old timestamped status of the charging pool.
        /// </summary>
        public Timestamped<ChargingPoolStatusTypes>?  OldStatus     { get; }

        /// <summary>
        /// An optional data source or context for this charging pool status update.
        /// </summary>
        public Context?                               DataSource    { get; }

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new charging pool status update.
        /// </summary>
        /// <param name="Id">The unique identification of the charging pool.</param>
        /// <param name="NewStatus">The new timestamped status of the charging pool.</param>
        /// <param name="OldStatus">The optional old timestamped status of the charging pool.</param>
        /// <param name="DataSource">An optional data source or context for the charging pool status update.</param>
        public ChargingPoolStatusUpdate(ChargingPool_Id                        Id,
                                        Timestamped<ChargingPoolStatusTypes>   NewStatus,
                                        Timestamped<ChargingPoolStatusTypes>?  OldStatus    = null,
                                        Context?                               DataSource   = null)

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
        /// Take a snapshot of the current charging pool status.
        /// </summary>
        /// <param name="ChargingPool">A charging pool.</param>
        /// <param name="DataSource">An optional data source or context for the EVSE admin status update.</param>
        public static ChargingPoolStatusUpdate Snapshot(IChargingPool  ChargingPool,
                                                        Context?       DataSource   = null)

            => new (ChargingPool.Id,
                    ChargingPool.Status,
                    ChargingPool.StatusSchedule().Skip(1).FirstOrDefault(),
                    DataSource);

        #endregion


        #region Operator overloading

        #region Operator == (ChargingPoolStatusUpdate1, ChargingPoolStatusUpdate2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingPoolStatusUpdate1">A charging pool status update.</param>
        /// <param name="ChargingPoolStatusUpdate2">Another charging pool status update.</param>
        /// <returns>true|false</returns>
        public static Boolean operator == (ChargingPoolStatusUpdate ChargingPoolStatusUpdate1,
                                           ChargingPoolStatusUpdate ChargingPoolStatusUpdate2)

            => ChargingPoolStatusUpdate1.Equals(ChargingPoolStatusUpdate2);

        #endregion

        #region Operator != (ChargingPoolStatusUpdate1, ChargingPoolStatusUpdate2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingPoolStatusUpdate1">A charging pool status update.</param>
        /// <param name="ChargingPoolStatusUpdate2">Another charging pool status update.</param>
        /// <returns>true|false</returns>
        public static Boolean operator != (ChargingPoolStatusUpdate ChargingPoolStatusUpdate1,
                                           ChargingPoolStatusUpdate ChargingPoolStatusUpdate2)

            => !ChargingPoolStatusUpdate1.Equals(ChargingPoolStatusUpdate2);

        #endregion

        #region Operator <  (ChargingPoolStatusUpdate1, ChargingPoolStatusUpdate2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingPoolStatusUpdate1">A charging pool status update.</param>
        /// <param name="ChargingPoolStatusUpdate2">Another charging pool status update.</param>
        /// <returns>true|false</returns>
        public static Boolean operator < (ChargingPoolStatusUpdate ChargingPoolStatusUpdate1,
                                          ChargingPoolStatusUpdate ChargingPoolStatusUpdate2)

            => ChargingPoolStatusUpdate1.CompareTo(ChargingPoolStatusUpdate2) < 0;

        #endregion

        #region Operator <= (ChargingPoolStatusUpdate1, ChargingPoolStatusUpdate2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingPoolStatusUpdate1">A charging pool status update.</param>
        /// <param name="ChargingPoolStatusUpdate2">Another charging pool status update.</param>
        /// <returns>true|false</returns>
        public static Boolean operator <= (ChargingPoolStatusUpdate ChargingPoolStatusUpdate1,
                                           ChargingPoolStatusUpdate ChargingPoolStatusUpdate2)

            => ChargingPoolStatusUpdate1.CompareTo(ChargingPoolStatusUpdate2) <= 0;

        #endregion

        #region Operator >  (ChargingPoolStatusUpdate1, ChargingPoolStatusUpdate2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingPoolStatusUpdate1">A charging pool status update.</param>
        /// <param name="ChargingPoolStatusUpdate2">Another charging pool status update.</param>
        /// <returns>true|false</returns>
        public static Boolean operator > (ChargingPoolStatusUpdate ChargingPoolStatusUpdate1,
                                          ChargingPoolStatusUpdate ChargingPoolStatusUpdate2)

            => ChargingPoolStatusUpdate1.CompareTo(ChargingPoolStatusUpdate2) > 0;

        #endregion

        #region Operator >= (ChargingPoolStatusUpdate1, ChargingPoolStatusUpdate2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingPoolStatusUpdate1">A charging pool status update.</param>
        /// <param name="ChargingPoolStatusUpdate2">Another charging pool status update.</param>
        /// <returns>true|false</returns>
        public static Boolean operator >= (ChargingPoolStatusUpdate ChargingPoolStatusUpdate1,
                                           ChargingPoolStatusUpdate ChargingPoolStatusUpdate2)

            => ChargingPoolStatusUpdate1.CompareTo(ChargingPoolStatusUpdate2) >= 0;

        #endregion

        #endregion

        #region IComparable<ChargingPoolStatusUpdate> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two charging pool status updates.
        /// </summary>
        /// <param name="Object">A charging pool status update to compare with.</param>
        public Int32 CompareTo(Object? Object)

            => Object is ChargingPoolStatusUpdate chargingPoolStatusUpdate
                   ? CompareTo(chargingPoolStatusUpdate)
                   : throw new ArgumentException("The given object is not a charging pool status update!",
                                                 nameof(Object));

        #endregion

        #region CompareTo(ChargingPoolStatusUpdate)

        /// <summary>
        /// Compares two charging pool status updates.
        /// </summary>
        /// <param name="ChargingPoolStatusUpdate">A charging pool status update to compare with.</param>
        public Int32 CompareTo(ChargingPoolStatusUpdate ChargingPoolStatusUpdate)
        {

            var c = Id.             CompareTo(ChargingPoolStatusUpdate.Id);

            if (c == 0)
                c = NewStatus.      CompareTo(ChargingPoolStatusUpdate.NewStatus);

            if (c == 0 && OldStatus.HasValue && ChargingPoolStatusUpdate.OldStatus.HasValue)
                c = OldStatus.Value.CompareTo(ChargingPoolStatusUpdate.OldStatus.Value);

            if (c == 0 && DataSource is not null && ChargingPoolStatusUpdate.DataSource is not null)
                c = DataSource.     CompareTo(ChargingPoolStatusUpdate.DataSource);

            return c;

        }

        #endregion

        #endregion

        #region IEquatable<ChargingPoolStatusUpdate> Members

        #region Equals(Object)

        /// <summary>
        /// Compares two charging pool status updates for equality.
        /// </summary>
        /// <param name="Object">A charging pool status update to compare with.</param>
        public override Boolean Equals(Object? Object)

            => Object is ChargingPoolStatusUpdate chargingPoolStatusUpdate &&
                   Equals(chargingPoolStatusUpdate);

        #endregion

        #region Equals(ChargingPoolStatusUpdate)

        /// <summary>
        /// Compares two charging pool status updates for equality.
        /// </summary>
        /// <param name="ChargingPoolStatusUpdate">A charging pool status update to compare with.</param>
        public Boolean Equals(ChargingPoolStatusUpdate ChargingPoolStatusUpdate)

            => Id.       Equals(ChargingPoolStatusUpdate.Id)        &&
               NewStatus.Equals(ChargingPoolStatusUpdate.NewStatus) &&

            ((!OldStatus.HasValue     && !ChargingPoolStatusUpdate.OldStatus.HasValue) ||
              (OldStatus.HasValue     &&  ChargingPoolStatusUpdate.OldStatus.HasValue     && OldStatus.Value.Equals(ChargingPoolStatusUpdate.OldStatus.Value))) &&

            (( DataSource is null     &&  ChargingPoolStatusUpdate.DataSource is null) ||
              (DataSource is not null &&  ChargingPoolStatusUpdate.DataSource is not null && DataSource.     Equals(ChargingPoolStatusUpdate.DataSource)));

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
