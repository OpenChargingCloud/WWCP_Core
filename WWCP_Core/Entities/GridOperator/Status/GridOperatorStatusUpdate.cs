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
    /// A grid operator status update.
    /// </summary>
    public readonly struct GridOperatorStatusUpdate : IEquatable<GridOperatorStatusUpdate>,
                                                                 IComparable<GridOperatorStatusUpdate>,
                                                                 IComparable
    {

        #region Properties

        /// <summary>
        /// The unique identification of the grid operator.
        /// </summary>
        public GridOperator_Id                        Id            { get; }

        /// <summary>
        /// The new timestamped status of the grid operator.
        /// </summary>
        public Timestamped<GridOperatorStatusTypes>   NewStatus     { get; }

        /// <summary>
        /// The old timestamped status of the grid operator.
        /// </summary>
        public Timestamped<GridOperatorStatusTypes>?  OldStatus     { get; }

        /// <summary>
        /// An optional data source or context for this grid operator status update.
        /// </summary>
        public String?                                           DataSource    { get; }

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new grid operator status update.
        /// </summary>
        /// <param name="Id">The unique identification of the grid operator.</param>
        /// <param name="NewStatus">The new timestamped status of the grid operator.</param>
        /// <param name="OldStatus">The optional old timestamped status of the grid operator.</param>
        /// <param name="DataSource">An optional data source or context for the grid operator status update.</param>
        public GridOperatorStatusUpdate(GridOperator_Id                        Id,
                                                   Timestamped<GridOperatorStatusTypes>   NewStatus,
                                                   Timestamped<GridOperatorStatusTypes>?  OldStatus    = null,
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


        #region (static) Snapshot(GridOperator, DataSource = null)

        /// <summary>
        /// Take a snapshot of the current grid operator status.
        /// </summary>
        /// <param name="GridOperator">A grid operator.</param>
        /// <param name="DataSource">An optional data source or context for the grid operator status update.</param>
        public static GridOperatorStatusUpdate Snapshot(IGridOperator  GridOperator,
                                                                   String?                   DataSource   = null)

            => new (GridOperator.Id,
                    GridOperator.Status,
                    GridOperator.StatusSchedule().Skip(1).FirstOrDefault(),
                    DataSource);

        #endregion


        #region Operator overloading

        #region Operator == (GridOperatorStatusUpdate1, GridOperatorStatusUpdate2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="GridOperatorStatusUpdate1">A grid operator status update.</param>
        /// <param name="GridOperatorStatusUpdate2">Another grid operator status update.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator == (GridOperatorStatusUpdate GridOperatorStatusUpdate1,
                                           GridOperatorStatusUpdate GridOperatorStatusUpdate2)

            => GridOperatorStatusUpdate1.Equals(GridOperatorStatusUpdate2);

        #endregion

        #region Operator != (GridOperatorStatusUpdate1, GridOperatorStatusUpdate2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="GridOperatorStatusUpdate1">A grid operator status update.</param>
        /// <param name="GridOperatorStatusUpdate2">Another grid operator status update.</param>
        /// <returns>False if both match; True otherwise.</returns>
        public static Boolean operator != (GridOperatorStatusUpdate GridOperatorStatusUpdate1,
                                           GridOperatorStatusUpdate GridOperatorStatusUpdate2)

            => !GridOperatorStatusUpdate1.Equals(GridOperatorStatusUpdate2);

        #endregion

        #region Operator <  (GridOperatorStatusUpdate1, GridOperatorStatusUpdate2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="GridOperatorStatusUpdate1">A grid operator status update.</param>
        /// <param name="GridOperatorStatusUpdate2">Another grid operator status update.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator < (GridOperatorStatusUpdate GridOperatorStatusUpdate1,
                                          GridOperatorStatusUpdate GridOperatorStatusUpdate2)

            => GridOperatorStatusUpdate1.CompareTo(GridOperatorStatusUpdate2) < 0;

        #endregion

        #region Operator <= (GridOperatorStatusUpdate1, GridOperatorStatusUpdate2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="GridOperatorStatusUpdate1">A grid operator status update.</param>
        /// <param name="GridOperatorStatusUpdate2">Another grid operator status update.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator <= (GridOperatorStatusUpdate GridOperatorStatusUpdate1,
                                           GridOperatorStatusUpdate GridOperatorStatusUpdate2)

            => GridOperatorStatusUpdate1.CompareTo(GridOperatorStatusUpdate2) <= 0;

        #endregion

        #region Operator >  (GridOperatorStatusUpdate1, GridOperatorStatusUpdate2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="GridOperatorStatusUpdate1">A grid operator status update.</param>
        /// <param name="GridOperatorStatusUpdate2">Another grid operator status update.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator > (GridOperatorStatusUpdate GridOperatorStatusUpdate1,
                                          GridOperatorStatusUpdate GridOperatorStatusUpdate2)

            => GridOperatorStatusUpdate1.CompareTo(GridOperatorStatusUpdate2) > 0;

        #endregion

        #region Operator >= (GridOperatorStatusUpdate1, GridOperatorStatusUpdate2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="GridOperatorStatusUpdate1">A grid operator status update.</param>
        /// <param name="GridOperatorStatusUpdate2">Another grid operator status update.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator >= (GridOperatorStatusUpdate GridOperatorStatusUpdate1,
                                           GridOperatorStatusUpdate GridOperatorStatusUpdate2)

            => GridOperatorStatusUpdate1.CompareTo(GridOperatorStatusUpdate2) >= 0;

        #endregion

        #endregion

        #region IComparable<GridOperatorStatusUpdate> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two grid operator status updates.
        /// </summary>
        /// <param name="Object">A grid operator status update to compare with.</param>
        public Int32 CompareTo(Object? Object)

            => Object is GridOperatorStatusUpdate chargingStationOperatorStatusUpdate
                   ? CompareTo(chargingStationOperatorStatusUpdate)
                   : throw new ArgumentException("The given object is not a grid operator status update!",
                                                 nameof(Object));

        #endregion

        #region CompareTo(GridOperatorStatusUpdate)

        /// <summary>
        /// Compares two grid operator status updates.
        /// </summary>
        /// <param name="GridOperatorStatusUpdate">A grid operator status update to compare with.</param>
        public Int32 CompareTo(GridOperatorStatusUpdate GridOperatorStatusUpdate)
        {

            var c = Id.             CompareTo(GridOperatorStatusUpdate.Id);

            if (c == 0)
                c = NewStatus.      CompareTo(GridOperatorStatusUpdate.NewStatus);

            if (c == 0 && OldStatus.HasValue && GridOperatorStatusUpdate.OldStatus.HasValue)
                c = OldStatus.Value.CompareTo(GridOperatorStatusUpdate.OldStatus.Value);

            if (c == 0 && DataSource is not null && GridOperatorStatusUpdate.DataSource is not null)
                c = DataSource.     CompareTo(GridOperatorStatusUpdate.DataSource);

            return c;

        }

        #endregion

        #endregion

        #region IEquatable<GridOperatorStatusUpdate> Members

        #region Equals(Object)

        /// <summary>
        /// Compares two grid operator status updates for equality.
        /// </summary>
        /// <param name="Object">A grid operator status update to compare with.</param>
        public override Boolean Equals(Object? Object)

            => Object is GridOperatorStatusUpdate chargingStationOperatorStatusUpdate &&
                   Equals(chargingStationOperatorStatusUpdate);

        #endregion

        #region Equals(GridOperatorStatusUpdate)

        /// <summary>
        /// Compares two grid operator status updates for equality.
        /// </summary>
        /// <param name="GridOperatorStatusUpdate">A grid operator status update to compare with.</param>
        public Boolean Equals(GridOperatorStatusUpdate GridOperatorStatusUpdate)

            => Id.       Equals(GridOperatorStatusUpdate.Id)        &&
               NewStatus.Equals(GridOperatorStatusUpdate.NewStatus) &&

            ((!OldStatus.HasValue     && !GridOperatorStatusUpdate.OldStatus.HasValue) ||
              (OldStatus.HasValue     &&  GridOperatorStatusUpdate.OldStatus.HasValue     && OldStatus.Value.Equals(GridOperatorStatusUpdate.OldStatus.Value))) &&

            (( DataSource is null     &&  GridOperatorStatusUpdate.DataSource is null) ||
              (DataSource is not null &&  GridOperatorStatusUpdate.DataSource is not null && DataSource.     Equals(GridOperatorStatusUpdate.DataSource)));

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
