/*
 * Copyright (c) 2014-2024 GraphDefined GmbH <achim.friedland@graphdefined.com> <achim.friedland@graphdefined.com>
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
    /// A charging station operator status update.
    /// </summary>
    public readonly struct ChargingStationOperatorStatusUpdate : IEquatable<ChargingStationOperatorStatusUpdate>,
                                                                 IComparable<ChargingStationOperatorStatusUpdate>,
                                                                 IComparable
    {

        #region Properties

        /// <summary>
        /// The unique identification of the charging station operator.
        /// </summary>
        public ChargingStationOperator_Id                        Id            { get; }

        /// <summary>
        /// The new timestamped status of the charging station operator.
        /// </summary>
        public Timestamped<ChargingStationOperatorStatusTypes>   NewStatus     { get; }

        /// <summary>
        /// The old timestamped status of the charging station operator.
        /// </summary>
        public Timestamped<ChargingStationOperatorStatusTypes>?  OldStatus     { get; }

        /// <summary>
        /// An optional data source or context for this charging station operator status update.
        /// </summary>
        public String?                                           DataSource    { get; }

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new charging station operator status update.
        /// </summary>
        /// <param name="Id">The unique identification of the charging station operator.</param>
        /// <param name="NewStatus">The new timestamped status of the charging station operator.</param>
        /// <param name="OldStatus">The optional old timestamped status of the charging station operator.</param>
        /// <param name="DataSource">An optional data source or context for the charging station operator status update.</param>
        public ChargingStationOperatorStatusUpdate(ChargingStationOperator_Id                        Id,
                                                   Timestamped<ChargingStationOperatorStatusTypes>   NewStatus,
                                                   Timestamped<ChargingStationOperatorStatusTypes>?  OldStatus    = null,
                                                   String?                                           DataSource   = null)

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


        #region (static) Snapshot(ChargingStationOperator, DataSource = null)

        /// <summary>
        /// Take a snapshot of the current charging station operator status.
        /// </summary>
        /// <param name="ChargingStationOperator">A charging station operator.</param>
        /// <param name="DataSource">An optional data source or context for the charging station operator status update.</param>
        public static ChargingStationOperatorStatusUpdate Snapshot(IChargingStationOperator  ChargingStationOperator,
                                                                   String?                   DataSource   = null)

            => new (ChargingStationOperator.Id,
                    ChargingStationOperator.Status,
                    ChargingStationOperator.StatusSchedule().Skip(1).FirstOrDefault(),
                    DataSource);

        #endregion


        #region Operator overloading

        #region Operator == (ChargingStationOperatorStatusUpdate1, ChargingStationOperatorStatusUpdate2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingStationOperatorStatusUpdate1">A charging station operator status update.</param>
        /// <param name="ChargingStationOperatorStatusUpdate2">Another charging station operator status update.</param>
        /// <returns>true|false</returns>
        public static Boolean operator == (ChargingStationOperatorStatusUpdate ChargingStationOperatorStatusUpdate1,
                                           ChargingStationOperatorStatusUpdate ChargingStationOperatorStatusUpdate2)

            => ChargingStationOperatorStatusUpdate1.Equals(ChargingStationOperatorStatusUpdate2);

        #endregion

        #region Operator != (ChargingStationOperatorStatusUpdate1, ChargingStationOperatorStatusUpdate2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingStationOperatorStatusUpdate1">A charging station operator status update.</param>
        /// <param name="ChargingStationOperatorStatusUpdate2">Another charging station operator status update.</param>
        /// <returns>true|false</returns>
        public static Boolean operator != (ChargingStationOperatorStatusUpdate ChargingStationOperatorStatusUpdate1,
                                           ChargingStationOperatorStatusUpdate ChargingStationOperatorStatusUpdate2)

            => !ChargingStationOperatorStatusUpdate1.Equals(ChargingStationOperatorStatusUpdate2);

        #endregion

        #region Operator <  (ChargingStationOperatorStatusUpdate1, ChargingStationOperatorStatusUpdate2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingStationOperatorStatusUpdate1">A charging station operator status update.</param>
        /// <param name="ChargingStationOperatorStatusUpdate2">Another charging station operator status update.</param>
        /// <returns>true|false</returns>
        public static Boolean operator < (ChargingStationOperatorStatusUpdate ChargingStationOperatorStatusUpdate1,
                                          ChargingStationOperatorStatusUpdate ChargingStationOperatorStatusUpdate2)

            => ChargingStationOperatorStatusUpdate1.CompareTo(ChargingStationOperatorStatusUpdate2) < 0;

        #endregion

        #region Operator <= (ChargingStationOperatorStatusUpdate1, ChargingStationOperatorStatusUpdate2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingStationOperatorStatusUpdate1">A charging station operator status update.</param>
        /// <param name="ChargingStationOperatorStatusUpdate2">Another charging station operator status update.</param>
        /// <returns>true|false</returns>
        public static Boolean operator <= (ChargingStationOperatorStatusUpdate ChargingStationOperatorStatusUpdate1,
                                           ChargingStationOperatorStatusUpdate ChargingStationOperatorStatusUpdate2)

            => ChargingStationOperatorStatusUpdate1.CompareTo(ChargingStationOperatorStatusUpdate2) <= 0;

        #endregion

        #region Operator >  (ChargingStationOperatorStatusUpdate1, ChargingStationOperatorStatusUpdate2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingStationOperatorStatusUpdate1">A charging station operator status update.</param>
        /// <param name="ChargingStationOperatorStatusUpdate2">Another charging station operator status update.</param>
        /// <returns>true|false</returns>
        public static Boolean operator > (ChargingStationOperatorStatusUpdate ChargingStationOperatorStatusUpdate1,
                                          ChargingStationOperatorStatusUpdate ChargingStationOperatorStatusUpdate2)

            => ChargingStationOperatorStatusUpdate1.CompareTo(ChargingStationOperatorStatusUpdate2) > 0;

        #endregion

        #region Operator >= (ChargingStationOperatorStatusUpdate1, ChargingStationOperatorStatusUpdate2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingStationOperatorStatusUpdate1">A charging station operator status update.</param>
        /// <param name="ChargingStationOperatorStatusUpdate2">Another charging station operator status update.</param>
        /// <returns>true|false</returns>
        public static Boolean operator >= (ChargingStationOperatorStatusUpdate ChargingStationOperatorStatusUpdate1,
                                           ChargingStationOperatorStatusUpdate ChargingStationOperatorStatusUpdate2)

            => ChargingStationOperatorStatusUpdate1.CompareTo(ChargingStationOperatorStatusUpdate2) >= 0;

        #endregion

        #endregion

        #region IComparable<ChargingStationOperatorStatusUpdate> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two charging station operator status updates.
        /// </summary>
        /// <param name="Object">A charging station operator status update to compare with.</param>
        public Int32 CompareTo(Object? Object)

            => Object is ChargingStationOperatorStatusUpdate chargingStationOperatorStatusUpdate
                   ? CompareTo(chargingStationOperatorStatusUpdate)
                   : throw new ArgumentException("The given object is not a charging station operator status update!",
                                                 nameof(Object));

        #endregion

        #region CompareTo(ChargingStationOperatorStatusUpdate)

        /// <summary>
        /// Compares two charging station operator status updates.
        /// </summary>
        /// <param name="ChargingStationOperatorStatusUpdate">A charging station operator status update to compare with.</param>
        public Int32 CompareTo(ChargingStationOperatorStatusUpdate ChargingStationOperatorStatusUpdate)
        {

            var c = Id.             CompareTo(ChargingStationOperatorStatusUpdate.Id);

            if (c == 0)
                c = NewStatus.      CompareTo(ChargingStationOperatorStatusUpdate.NewStatus);

            if (c == 0 && OldStatus.HasValue && ChargingStationOperatorStatusUpdate.OldStatus.HasValue)
                c = OldStatus.Value.CompareTo(ChargingStationOperatorStatusUpdate.OldStatus.Value);

            if (c == 0 && DataSource is not null && ChargingStationOperatorStatusUpdate.DataSource is not null)
                c = DataSource.     CompareTo(ChargingStationOperatorStatusUpdate.DataSource);

            return c;

        }

        #endregion

        #endregion

        #region IEquatable<ChargingStationOperatorStatusUpdate> Members

        #region Equals(Object)

        /// <summary>
        /// Compares two charging station operator status updates for equality.
        /// </summary>
        /// <param name="Object">A charging station operator status update to compare with.</param>
        public override Boolean Equals(Object? Object)

            => Object is ChargingStationOperatorStatusUpdate chargingStationOperatorStatusUpdate &&
                   Equals(chargingStationOperatorStatusUpdate);

        #endregion

        #region Equals(ChargingStationOperatorStatusUpdate)

        /// <summary>
        /// Compares two charging station operator status updates for equality.
        /// </summary>
        /// <param name="ChargingStationOperatorStatusUpdate">A charging station operator status update to compare with.</param>
        public Boolean Equals(ChargingStationOperatorStatusUpdate ChargingStationOperatorStatusUpdate)

            => Id.       Equals(ChargingStationOperatorStatusUpdate.Id)        &&
               NewStatus.Equals(ChargingStationOperatorStatusUpdate.NewStatus) &&

            ((!OldStatus.HasValue     && !ChargingStationOperatorStatusUpdate.OldStatus.HasValue) ||
              (OldStatus.HasValue     &&  ChargingStationOperatorStatusUpdate.OldStatus.HasValue     && OldStatus.Value.Equals(ChargingStationOperatorStatusUpdate.OldStatus.Value))) &&

            (( DataSource is null     &&  ChargingStationOperatorStatusUpdate.DataSource is null) ||
              (DataSource is not null &&  ChargingStationOperatorStatusUpdate.DataSource is not null && DataSource.     Equals(ChargingStationOperatorStatusUpdate.DataSource)));

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
