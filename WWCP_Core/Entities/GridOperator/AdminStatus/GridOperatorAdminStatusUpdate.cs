﻿/*
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
    /// A grid operator admin status update.
    /// </summary>
    public readonly struct GridOperatorAdminStatusUpdate : IEquatable<GridOperatorAdminStatusUpdate>,
                                                                      IComparable<GridOperatorAdminStatusUpdate>,
                                                                      IComparable
    {

        #region Properties

        /// <summary>
        /// The unique identification of the grid operator.
        /// </summary>
        public GridOperator_Id                             Id            { get; }

        /// <summary>
        /// The new timestamped admin status of the grid operator.
        /// </summary>
        public Timestamped<GridOperatorAdminStatusTypes>   NewStatus     { get; }

        /// <summary>
        /// The optional old timestamped admin status of the grid operator.
        /// </summary>
        public Timestamped<GridOperatorAdminStatusTypes>?  OldStatus     { get; }

        /// <summary>
        /// An optional data source or context for this grid operator admin status update.
        /// </summary>
        public String?                                                DataSource    { get; }

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new grid operator admin status update.
        /// </summary>
        /// <param name="Id">The unique identification of the grid operator.</param>
        /// <param name="NewStatus">The new timestamped admin status of the grid operator.</param>
        /// <param name="OldStatus">The optional old timestamped admin status of the grid operator.</param>
        /// <param name="DataSource">An optional data source or context for the grid operator admin status update.</param>
        public GridOperatorAdminStatusUpdate(GridOperator_Id                             Id,
                                                        Timestamped<GridOperatorAdminStatusTypes>   NewStatus,
                                                        Timestamped<GridOperatorAdminStatusTypes>?  OldStatus    = null,
                                                        String?                                                DataSource   = null)

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
        /// Take a snapshot of the current grid operator admin status.
        /// </summary>
        /// <param name="GridOperator">A grid operator.</param>
        /// <param name="DataSource">An optional data source or context for the grid operator admin status update.</param>
        public static GridOperatorAdminStatusUpdate Snapshot(IGridOperator  GridOperator,
                                                                        String?                   DataSource   = null)

            => new (GridOperator.Id,
                    GridOperator.AdminStatus,
                    GridOperator.AdminStatusSchedule().Skip(1).FirstOrDefault(),
                    DataSource);

        #endregion


        #region Operator overloading

        #region Operator == (GridOperatorAdminStatusUpdate1, GridOperatorAdminStatusUpdate2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="GridOperatorAdminStatusUpdate1">A grid operator admin status update.</param>
        /// <param name="GridOperatorAdminStatusUpdate2">Another grid operator admin status update.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator == (GridOperatorAdminStatusUpdate GridOperatorAdminStatusUpdate1,
                                           GridOperatorAdminStatusUpdate GridOperatorAdminStatusUpdate2)

            => GridOperatorAdminStatusUpdate1.Equals(GridOperatorAdminStatusUpdate2);

        #endregion

        #region Operator != (GridOperatorAdminStatusUpdate1, GridOperatorAdminStatusUpdate2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="GridOperatorAdminStatusUpdate1">A grid operator admin status update.</param>
        /// <param name="GridOperatorAdminStatusUpdate2">Another grid operator admin status update.</param>
        /// <returns>False if both match; True otherwise.</returns>
        public static Boolean operator != (GridOperatorAdminStatusUpdate GridOperatorAdminStatusUpdate1,
                                           GridOperatorAdminStatusUpdate GridOperatorAdminStatusUpdate2)

            => !GridOperatorAdminStatusUpdate1.Equals(GridOperatorAdminStatusUpdate2);

        #endregion

        #region Operator <  (GridOperatorAdminStatusUpdate1, GridOperatorAdminStatusUpdate2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="GridOperatorAdminStatusUpdate1">A grid operator admin status update.</param>
        /// <param name="GridOperatorAdminStatusUpdate2">Another grid operator admin status update.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator < (GridOperatorAdminStatusUpdate GridOperatorAdminStatusUpdate1,
                                          GridOperatorAdminStatusUpdate GridOperatorAdminStatusUpdate2)

            => GridOperatorAdminStatusUpdate1.CompareTo(GridOperatorAdminStatusUpdate2) < 0;

        #endregion

        #region Operator <= (GridOperatorAdminStatusUpdate1, GridOperatorAdminStatusUpdate2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="GridOperatorAdminStatusUpdate1">A grid operator admin status update.</param>
        /// <param name="GridOperatorAdminStatusUpdate2">Another grid operator admin status update.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator <= (GridOperatorAdminStatusUpdate GridOperatorAdminStatusUpdate1,
                                           GridOperatorAdminStatusUpdate GridOperatorAdminStatusUpdate2)

            => GridOperatorAdminStatusUpdate1.CompareTo(GridOperatorAdminStatusUpdate2) <= 0;

        #endregion

        #region Operator >  (GridOperatorAdminStatusUpdate1, GridOperatorAdminStatusUpdate2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="GridOperatorAdminStatusUpdate1">A grid operator admin status update.</param>
        /// <param name="GridOperatorAdminStatusUpdate2">Another grid operator admin status update.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator > (GridOperatorAdminStatusUpdate GridOperatorAdminStatusUpdate1,
                                          GridOperatorAdminStatusUpdate GridOperatorAdminStatusUpdate2)

            => GridOperatorAdminStatusUpdate1.CompareTo(GridOperatorAdminStatusUpdate2) > 0;

        #endregion

        #region Operator >= (GridOperatorAdminStatusUpdate1, GridOperatorAdminStatusUpdate2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="GridOperatorAdminStatusUpdate1">A grid operator admin status update.</param>
        /// <param name="GridOperatorAdminStatusUpdate2">Another grid operator admin status update.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator >= (GridOperatorAdminStatusUpdate GridOperatorAdminStatusUpdate1,
                                           GridOperatorAdminStatusUpdate GridOperatorAdminStatusUpdate2)

            => GridOperatorAdminStatusUpdate1.CompareTo(GridOperatorAdminStatusUpdate2) >= 0;

        #endregion

        #endregion

        #region IComparable<GridOperatorAdminStatusUpdate> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two grid operator admin status updates.
        /// </summary>
        /// <param name="Object">A grid operator admin status update to compare with.</param>
        public Int32 CompareTo(Object? Object)

            => Object is GridOperatorAdminStatusUpdate chargingStationOperatorAdminStatusUpdate
                   ? CompareTo(chargingStationOperatorAdminStatusUpdate)
                   : throw new ArgumentException("The given object is not a grid operator admin status update!",
                                                 nameof(Object));

        #endregion

        #region CompareTo(GridOperatorAdminStatusUpdate)

        /// <summary>
        /// Compares two grid operator admin status updates.
        /// </summary>
        /// <param name="GridOperatorAdminStatusUpdate">A grid operator admin status update to compare with.</param>
        public Int32 CompareTo(GridOperatorAdminStatusUpdate GridOperatorAdminStatusUpdate)
        {

            var c = Id.             CompareTo(GridOperatorAdminStatusUpdate.Id);

            if (c == 0)
                c = NewStatus.      CompareTo(GridOperatorAdminStatusUpdate.NewStatus);

            if (c == 0 && OldStatus.HasValue && GridOperatorAdminStatusUpdate.OldStatus.HasValue)
                c = OldStatus.Value.CompareTo(GridOperatorAdminStatusUpdate.OldStatus.Value);

            if (c == 0 && DataSource is not null && GridOperatorAdminStatusUpdate.DataSource is not null)
                c = DataSource.     CompareTo(GridOperatorAdminStatusUpdate.DataSource);

            return c;

        }

        #endregion

        #endregion

        #region IEquatable<GridOperatorAdminStatusUpdate> Members

        #region Equals(Object)

        /// <summary>
        /// Compares two grid operator admin status updates for equality.
        /// </summary>
        /// <param name="Object">A grid operator admin status update to compare with.</param>
        public override Boolean Equals(Object? Object)

            => Object is GridOperatorAdminStatusUpdate chargingStationOperatorAdminStatusUpdate &&
                   Equals(chargingStationOperatorAdminStatusUpdate);

        #endregion

        #region Equals(GridOperatorAdminStatusUpdate)

        /// <summary>
        /// Compares two grid operator admin status updates for equality.
        /// </summary>
        /// <param name="GridOperatorAdminStatusUpdate">A grid operator admin status update to compare with.</param>
        public Boolean Equals(GridOperatorAdminStatusUpdate GridOperatorAdminStatusUpdate)

            => Id.       Equals(GridOperatorAdminStatusUpdate.Id)        &&
               NewStatus.Equals(GridOperatorAdminStatusUpdate.NewStatus) &&

            ((!OldStatus.HasValue     && !GridOperatorAdminStatusUpdate.OldStatus.HasValue) ||
              (OldStatus.HasValue     &&  GridOperatorAdminStatusUpdate.OldStatus.HasValue     && OldStatus.Value.Equals(GridOperatorAdminStatusUpdate.OldStatus.Value))) &&

            (( DataSource is null     &&  GridOperatorAdminStatusUpdate.DataSource is null) ||
              (DataSource is not null &&  GridOperatorAdminStatusUpdate.DataSource is not null && DataSource.     Equals(GridOperatorAdminStatusUpdate.DataSource)));

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
