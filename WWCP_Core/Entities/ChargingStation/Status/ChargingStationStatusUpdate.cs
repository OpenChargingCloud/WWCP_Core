/*
 * Copyright (c) 2014-2026 GraphDefined GmbH <achim.friedland@graphdefined.com>
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
    /// A charging station status update.
    /// </summary>
    public readonly struct ChargingStationStatusUpdate : IEquatable<ChargingStationStatusUpdate>,
                                                         IComparable<ChargingStationStatusUpdate>,
                                                         IComparable
    {

        #region Properties

        /// <summary>
        /// The unique identification of the charging station.
        /// </summary>
        public ChargingStation_Id                        Id            { get; }

        /// <summary>
        /// The new timestamped status of the charging station.
        /// </summary>
        public Timestamped<ChargingStationStatusType>   NewStatus     { get; }

        /// <summary>
        /// The old timestamped status of the charging station.
        /// </summary>
        public Timestamped<ChargingStationStatusType>?  OldStatus     { get; }

        /// <summary>
        /// An optional data source or context for this charging station status update.
        /// </summary>
        public Context?                                  DataSource    { get; }

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new charging station status update.
        /// </summary>
        /// <param name="Id">The unique identification of the charging station.</param>
        /// <param name="NewStatus">The new timestamped status of the charging station.</param>
        /// <param name="OldStatus">The optional old timestamped status of the charging station.</param>
        /// <param name="DataSource">An optional data source or context for the charging station status update.</param>
        public ChargingStationStatusUpdate(ChargingStation_Id                        Id,
                                           Timestamped<ChargingStationStatusType>   NewStatus,
                                           Timestamped<ChargingStationStatusType>?  OldStatus    = null,
                                           Context?                                  DataSource   = null)

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
        /// Take a snapshot of the current charging station status.
        /// </summary>
        /// <param name="ChargingStation">A charging station.</param>
        /// <param name="DataSource">An optional data source or context for the charging station status update.</param>
        public static ChargingStationStatusUpdate Snapshot(IChargingStation  ChargingStation,
                                                           Context?          DataSource   = null)

            => new (ChargingStation.Id,
                    ChargingStation.Status,
                    ChargingStation.StatusSchedule().Skip(1).FirstOrDefault(),
                    DataSource);

        #endregion


        #region Operator overloading

        #region Operator == (ChargingStationStatusUpdate1, ChargingStationStatusUpdate2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingStationStatusUpdate1">A charging station status update.</param>
        /// <param name="ChargingStationStatusUpdate2">Another charging station status update.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator == (ChargingStationStatusUpdate ChargingStationStatusUpdate1,
                                           ChargingStationStatusUpdate ChargingStationStatusUpdate2)

            => ChargingStationStatusUpdate1.Equals(ChargingStationStatusUpdate2);

        #endregion

        #region Operator != (ChargingStationStatusUpdate1, ChargingStationStatusUpdate2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingStationStatusUpdate1">A charging station status update.</param>
        /// <param name="ChargingStationStatusUpdate2">Another charging station status update.</param>
        /// <returns>False if both match; True otherwise.</returns>
        public static Boolean operator != (ChargingStationStatusUpdate ChargingStationStatusUpdate1,
                                           ChargingStationStatusUpdate ChargingStationStatusUpdate2)

            => !ChargingStationStatusUpdate1.Equals(ChargingStationStatusUpdate2);

        #endregion

        #region Operator <  (ChargingStationStatusUpdate1, ChargingStationStatusUpdate2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingStationStatusUpdate1">A charging station status update.</param>
        /// <param name="ChargingStationStatusUpdate2">Another charging station status update.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator < (ChargingStationStatusUpdate ChargingStationStatusUpdate1,
                                          ChargingStationStatusUpdate ChargingStationStatusUpdate2)

            => ChargingStationStatusUpdate1.CompareTo(ChargingStationStatusUpdate2) < 0;

        #endregion

        #region Operator <= (ChargingStationStatusUpdate1, ChargingStationStatusUpdate2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingStationStatusUpdate1">A charging station status update.</param>
        /// <param name="ChargingStationStatusUpdate2">Another charging station status update.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator <= (ChargingStationStatusUpdate ChargingStationStatusUpdate1,
                                           ChargingStationStatusUpdate ChargingStationStatusUpdate2)

            => ChargingStationStatusUpdate1.CompareTo(ChargingStationStatusUpdate2) <= 0;

        #endregion

        #region Operator >  (ChargingStationStatusUpdate1, ChargingStationStatusUpdate2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingStationStatusUpdate1">A charging station status update.</param>
        /// <param name="ChargingStationStatusUpdate2">Another charging station status update.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator > (ChargingStationStatusUpdate ChargingStationStatusUpdate1,
                                          ChargingStationStatusUpdate ChargingStationStatusUpdate2)

            => ChargingStationStatusUpdate1.CompareTo(ChargingStationStatusUpdate2) > 0;

        #endregion

        #region Operator >= (ChargingStationStatusUpdate1, ChargingStationStatusUpdate2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingStationStatusUpdate1">A charging station status update.</param>
        /// <param name="ChargingStationStatusUpdate2">Another charging station status update.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator >= (ChargingStationStatusUpdate ChargingStationStatusUpdate1,
                                           ChargingStationStatusUpdate ChargingStationStatusUpdate2)

            => ChargingStationStatusUpdate1.CompareTo(ChargingStationStatusUpdate2) >= 0;

        #endregion

        #endregion

        #region IComparable<ChargingStationStatusUpdate> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two charging station status updates.
        /// </summary>
        /// <param name="Object">A charging station status update to compare with.</param>
        public Int32 CompareTo(Object? Object)

            => Object is ChargingStationStatusUpdate chargingStationStatusUpdate
                   ? CompareTo(chargingStationStatusUpdate)
                   : throw new ArgumentException("The given object is not a charging station status update!",
                                                 nameof(Object));

        #endregion

        #region CompareTo(ChargingStationStatusUpdate)

        /// <summary>
        /// Compares two charging station status updates.
        /// </summary>
        /// <param name="ChargingStationStatusUpdate">A charging station status update to compare with.</param>
        public Int32 CompareTo(ChargingStationStatusUpdate ChargingStationStatusUpdate)
        {

            var c = Id.             CompareTo(ChargingStationStatusUpdate.Id);

            if (c == 0)
                c = NewStatus.      CompareTo(ChargingStationStatusUpdate.NewStatus);

            if (c == 0 && OldStatus.HasValue && ChargingStationStatusUpdate.OldStatus.HasValue)
                c = OldStatus.Value.CompareTo(ChargingStationStatusUpdate.OldStatus.Value);

            if (c == 0 && DataSource is not null && ChargingStationStatusUpdate.DataSource is not null)
                c = DataSource.     CompareTo(ChargingStationStatusUpdate.DataSource);

            return c;

        }

        #endregion

        #endregion

        #region IEquatable<ChargingStationStatusUpdate> Members

        #region Equals(Object)

        /// <summary>
        /// Compares two charging station status updates for equality.
        /// </summary>
        /// <param name="Object">A charging station status update to compare with.</param>
        public override Boolean Equals(Object? Object)

            => Object is ChargingStationStatusUpdate chargingStationStatusUpdate &&
                   Equals(chargingStationStatusUpdate);

        #endregion

        #region Equals(ChargingStationStatusUpdate)

        /// <summary>
        /// Compares two charging station status updates for equality.
        /// </summary>
        /// <param name="ChargingStationStatusUpdate">A charging station status update to compare with.</param>
        public Boolean Equals(ChargingStationStatusUpdate ChargingStationStatusUpdate)

            => Id.       Equals(ChargingStationStatusUpdate.Id)        &&
               NewStatus.Equals(ChargingStationStatusUpdate.NewStatus) &&

            ((!OldStatus.HasValue     && !ChargingStationStatusUpdate.OldStatus.HasValue) ||
              (OldStatus.HasValue     &&  ChargingStationStatusUpdate.OldStatus.HasValue     && OldStatus.Value.Equals(ChargingStationStatusUpdate.OldStatus.Value))) &&

            (( DataSource is null     &&  ChargingStationStatusUpdate.DataSource is null) ||
              (DataSource is not null &&  ChargingStationStatusUpdate.DataSource is not null && DataSource.     Equals(ChargingStationStatusUpdate.DataSource)));

        #endregion

        #endregion

        #region (override) GetHashCode()

        private readonly Int32 hashCode;

        /// <summary>
        /// Return the hash code of this object.
        /// </summary>
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
