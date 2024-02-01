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
    /// A charging station admin status update.
    /// </summary>
    public readonly struct ChargingStationAdminStatusUpdate : IEquatable<ChargingStationAdminStatusUpdate>,
                                                              IComparable<ChargingStationAdminStatusUpdate>,
                                                              IComparable
    {

        #region Properties

        /// <summary>
        /// The unique identification of the charging station.
        /// </summary>
        public ChargingStation_Id                             Id            { get; }

        /// <summary>
        /// The new timestamped admin status of the charging station.
        /// </summary>
        public Timestamped<ChargingStationAdminStatusTypes>   NewStatus     { get; }

        /// <summary>
        /// The optional old timestamped admin status of the charging station.
        /// </summary>
        public Timestamped<ChargingStationAdminStatusTypes>?  OldStatus     { get; }

        /// <summary>
        /// An optional data source or context for this charging station status update.
        /// </summary>
        public Context?                                       DataSource    { get; }

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new charging station admin status update.
        /// </summary>
        /// <param name="Id">The unique identification of the charging station.</param>
        /// <param name="NewStatus">The new timestamped admin status of the charging station.</param>
        /// <param name="OldStatus">The optional old timestamped admin status of the charging station.</param>
        /// <param name="DataSource">An optional data source or context for the charging station status update.</param>
        public ChargingStationAdminStatusUpdate(ChargingStation_Id                             Id,
                                                Timestamped<ChargingStationAdminStatusTypes>   NewStatus,
                                                Timestamped<ChargingStationAdminStatusTypes>?  OldStatus    = null,
                                                Context?                                       DataSource   = null)

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


        #region (static) Snapshot(ChargingStation, DataSource = null)

        /// <summary>
        /// Take a snapshot of the current charging station admin status.
        /// </summary>
        /// <param name="ChargingStation">A charging station.</param>
        /// <param name="DataSource">An optional data source or context for the EVSE status update.</param>
        public static ChargingStationAdminStatusUpdate Snapshot(IChargingStation  ChargingStation,
                                                                Context?          DataSource   = null)

            => new (ChargingStation.Id,
                    ChargingStation.AdminStatus,
                    ChargingStation.AdminStatusSchedule().Skip(1).FirstOrDefault(),
                    DataSource);

        #endregion


        #region Operator overloading

        #region Operator == (ChargingStationAdminStatusUpdate1, ChargingStationAdminStatusUpdate2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingStationAdminStatusUpdate1">A charging station admin status update.</param>
        /// <param name="ChargingStationAdminStatusUpdate2">Another charging station admin status update.</param>
        /// <returns>true|false</returns>
        public static Boolean operator == (ChargingStationAdminStatusUpdate ChargingStationAdminStatusUpdate1,
                                           ChargingStationAdminStatusUpdate ChargingStationAdminStatusUpdate2)

            => ChargingStationAdminStatusUpdate1.Equals(ChargingStationAdminStatusUpdate2);

        #endregion

        #region Operator != (ChargingStationAdminStatusUpdate1, ChargingStationAdminStatusUpdate2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingStationAdminStatusUpdate1">A charging station admin status update.</param>
        /// <param name="ChargingStationAdminStatusUpdate2">Another charging station admin status update.</param>
        /// <returns>true|false</returns>
        public static Boolean operator != (ChargingStationAdminStatusUpdate ChargingStationAdminStatusUpdate1,
                                           ChargingStationAdminStatusUpdate ChargingStationAdminStatusUpdate2)

            => !ChargingStationAdminStatusUpdate1.Equals(ChargingStationAdminStatusUpdate2);

        #endregion

        #region Operator <  (ChargingStationAdminStatusUpdate1, ChargingStationAdminStatusUpdate2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingStationAdminStatusUpdate1">A charging station admin status update.</param>
        /// <param name="ChargingStationAdminStatusUpdate2">Another charging station admin status update.</param>
        /// <returns>true|false</returns>
        public static Boolean operator < (ChargingStationAdminStatusUpdate ChargingStationAdminStatusUpdate1,
                                          ChargingStationAdminStatusUpdate ChargingStationAdminStatusUpdate2)

            => ChargingStationAdminStatusUpdate1.CompareTo(ChargingStationAdminStatusUpdate2) < 0;

        #endregion

        #region Operator <= (ChargingStationAdminStatusUpdate1, ChargingStationAdminStatusUpdate2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingStationAdminStatusUpdate1">A charging station admin status update.</param>
        /// <param name="ChargingStationAdminStatusUpdate2">Another charging station admin status update.</param>
        /// <returns>true|false</returns>
        public static Boolean operator <= (ChargingStationAdminStatusUpdate ChargingStationAdminStatusUpdate1,
                                           ChargingStationAdminStatusUpdate ChargingStationAdminStatusUpdate2)

            => ChargingStationAdminStatusUpdate1.CompareTo(ChargingStationAdminStatusUpdate2) <= 0;

        #endregion

        #region Operator >  (ChargingStationAdminStatusUpdate1, ChargingStationAdminStatusUpdate2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingStationAdminStatusUpdate1">A charging station admin status update.</param>
        /// <param name="ChargingStationAdminStatusUpdate2">Another charging station admin status update.</param>
        /// <returns>true|false</returns>
        public static Boolean operator > (ChargingStationAdminStatusUpdate ChargingStationAdminStatusUpdate1,
                                          ChargingStationAdminStatusUpdate ChargingStationAdminStatusUpdate2)

            => ChargingStationAdminStatusUpdate1.CompareTo(ChargingStationAdminStatusUpdate2) > 0;

        #endregion

        #region Operator >= (ChargingStationAdminStatusUpdate1, ChargingStationAdminStatusUpdate2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingStationAdminStatusUpdate1">A charging station admin status update.</param>
        /// <param name="ChargingStationAdminStatusUpdate2">Another charging station admin status update.</param>
        /// <returns>true|false</returns>
        public static Boolean operator >= (ChargingStationAdminStatusUpdate ChargingStationAdminStatusUpdate1,
                                           ChargingStationAdminStatusUpdate ChargingStationAdminStatusUpdate2)

            => ChargingStationAdminStatusUpdate1.CompareTo(ChargingStationAdminStatusUpdate2) >= 0;

        #endregion

        #endregion

        #region IComparable<ChargingStationAdminStatusUpdate> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two charging station admin status updates.
        /// </summary>
        /// <param name="Object">A charging station admin status update to compare with.</param>
        public Int32 CompareTo(Object? Object)

            => Object is ChargingStationAdminStatusUpdate chargingStationAdminStatusUpdate
                   ? CompareTo(chargingStationAdminStatusUpdate)
                   : throw new ArgumentException("The given object is not a charging station admin status update!",
                                                 nameof(Object));

        #endregion

        #region CompareTo(ChargingStationAdminStatusUpdate)

        /// <summary>
        /// Compares two charging station admin status updates.
        /// </summary>
        /// <param name="ChargingStationAdminStatusUpdate">A charging station admin status update to compare with.</param>
        public Int32 CompareTo(ChargingStationAdminStatusUpdate ChargingStationAdminStatusUpdate)
        {

            var c = Id.             CompareTo(ChargingStationAdminStatusUpdate.Id);

            if (c == 0)
                c = NewStatus.      CompareTo(ChargingStationAdminStatusUpdate.NewStatus);

            if (c == 0 && OldStatus.HasValue && ChargingStationAdminStatusUpdate.OldStatus.HasValue)
                c = OldStatus.Value.CompareTo(ChargingStationAdminStatusUpdate.OldStatus.Value);

            if (c == 0 && DataSource is not null && ChargingStationAdminStatusUpdate.DataSource is not null)
                c = DataSource.     CompareTo(ChargingStationAdminStatusUpdate.DataSource);

            return c;

        }

        #endregion

        #endregion

        #region IEquatable<ChargingStationAdminStatusUpdate> Members

        #region Equals(Object)

        /// <summary>
        /// Compares two charging station admin status updates for equality.
        /// </summary>
        /// <param name="Object">A charging station admin status update to compare with.</param>
        public override Boolean Equals(Object? Object)

            => Object is ChargingStationAdminStatusUpdate chargingStationAdminStatusUpdate &&
                   Equals(chargingStationAdminStatusUpdate);

        #endregion

        #region Equals(ChargingStationAdminStatusUpdate)

        /// <summary>
        /// Compares two charging station admin status updates for equality.
        /// </summary>
        /// <param name="ChargingStationAdminStatusUpdate">A charging station admin status update to compare with.</param>
        public Boolean Equals(ChargingStationAdminStatusUpdate ChargingStationAdminStatusUpdate)

            => Id.       Equals(ChargingStationAdminStatusUpdate.Id)        &&
               NewStatus.Equals(ChargingStationAdminStatusUpdate.NewStatus) &&

            ((!OldStatus.HasValue     && !ChargingStationAdminStatusUpdate.OldStatus.HasValue) ||
              (OldStatus.HasValue     &&  ChargingStationAdminStatusUpdate.OldStatus.HasValue     && OldStatus.Value.Equals(ChargingStationAdminStatusUpdate.OldStatus.Value))) &&

            (( DataSource is null     &&  ChargingStationAdminStatusUpdate.DataSource is null) ||
              (DataSource is not null &&  ChargingStationAdminStatusUpdate.DataSource is not null && DataSource.     Equals(ChargingStationAdminStatusUpdate.DataSource)));

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
