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

using Newtonsoft.Json.Linq;

using org.GraphDefined.Vanaheimr.Illias;

#endregion

namespace cloud.charging.open.protocols.WWCP
{

    /// <summary>
    /// Extension methods for the grid operator admin status.
    /// </summary>
    public static class GridOperatorAdminStatusExtensions
    {

        #region ToJSON(this GridOperatorAdminStatus, Skip = null, Take = null)

        public static JObject ToJSON(this IEnumerable<GridOperatorAdminStatus>  GridOperatorAdminStatus,
                                     UInt64?                                               Skip   = null,
                                     UInt64?                                               Take   = null)
        {

            #region Initial checks

            if (GridOperatorAdminStatus is null || !GridOperatorAdminStatus.Any())
                return new JObject();

            #endregion

            #region Maybe there are duplicate grid operator identifications in the enumeration... take the newest one!

            var filteredStatus = new Dictionary<GridOperator_Id, GridOperatorAdminStatus>();

            foreach (var status in GridOperatorAdminStatus)
            {

                if (!filteredStatus.ContainsKey(status.Id))
                    filteredStatus.Add(status.Id, status);

                else if (filteredStatus[status.Id].Timestamp >= status.Timestamp)
                    filteredStatus[status.Id] = status;

            }

            #endregion


            return new JObject((Take.HasValue ? filteredStatus.OrderBy(status => status.Key).Skip(Skip).Take(Take)
                                              : filteredStatus.OrderBy(status => status.Key).Skip(Skip)).

                                   Select(kvp => new JProperty(kvp.Key.ToString(),
                                                               new JArray(kvp.Value.Timestamp.  ToISO8601(),
                                                                          kvp.Value.AdminStatus.ToString())
                                                              )));

        }

        #endregion

        #region Contains(this GridOperatorAdminStatus, Id, Status)

        /// <summary>
        /// Check if the given enumeration of grid operators and their current admin status
        /// contains the given pair of grid operator identification and admin status.
        /// </summary>
        /// <param name="GridOperatorAdminStatus">An enumeration of grid operators and their current admin status.</param>
        /// <param name="Id">A grid operator identification.</param>
        /// <param name="AdminStatus">A grid operator admin status.</param>
        public static Boolean Contains(this IEnumerable<GridOperatorAdminStatus>  GridOperatorAdminStatus,
                                       GridOperator_Id                            Id,
                                       GridOperatorAdminStatusTypes               AdminStatus)
        {

            foreach (var status in GridOperatorAdminStatus)
            {
                if (status.Id          == Id &&
                    status.AdminStatus == AdminStatus)
                {
                    return true;
                }
            }

            return false;

        }

        #endregion

    }


    /// <summary>
    /// The current admin status of a grid operator.
    /// </summary>
    public class GridOperatorAdminStatus : AInternalData,
                                                      IEquatable<GridOperatorAdminStatus>,
                                                      IComparable<GridOperatorAdminStatus>
    {

        #region Properties

        /// <summary>
        /// The unique identification of the grid operator.
        /// </summary>
        public GridOperator_Id               Id             { get; }

        /// <summary>
        /// The current admin status of the grid operator.
        /// </summary>
        public GridOperatorAdminStatusTypes  AdminStatus    { get; }

        /// <summary>
        /// The timestamp of the current admin status of the grid operator.
        /// </summary>
        public DateTimeOffset                Timestamp      { get; }

        /// <summary>
        /// The timestamped admin status of the grid operator.
        /// </summary>
        public Timestamped<GridOperatorAdminStatusTypes> TimestampedAdminStatus
            => new (Timestamp, AdminStatus);

        #endregion

        #region Constructor(s)

        #region GridOperatorAdminStatus(Id, AdminStatus,            CustomData = null, InternalData = null)

        /// <summary>
        /// Create a new grid operator admin status.
        /// </summary>
        /// <param name="Id">The unique identification of the grid operator.</param>
        /// <param name="AdminStatus">The current timestamped adminstatus of the grid operator.</param>
        /// <param name="CustomData">An optional dictionary of customer-specific data.</param>
        public GridOperatorAdminStatus(GridOperator_Id                            Id,
                                       Timestamped<GridOperatorAdminStatusTypes>  AdminStatus,
                                       JObject?                                   CustomData     = null,
                                       UserDefinedDictionary?                     InternalData   = null)

            : base(CustomData,
                   InternalData,
                   org.GraphDefined.Vanaheimr.Illias.Timestamp.Now)

        {

            this.Id           = Id;
            this.AdminStatus  = AdminStatus.Value;
            this.Timestamp    = AdminStatus.Timestamp;

        }

        #endregion

        #region GridOperatorAdminStatus(Id, AdminStatus, Timestamp, CustomData = null, InternalData = null)

        /// <summary>
        /// Create a new grid operator admin status.
        /// </summary>
        /// <param name="Id">The unique identification of the grid operator.</param>
        /// <param name="Status">The current admin status of the grid operator.</param>
        /// <param name="Timestamp">The timestamp of the status change of the grid operator.</param>
        /// <param name="CustomData">An optional dictionary of customer-specific data.</param>
        public GridOperatorAdminStatus(GridOperator_Id               Id,
                                       GridOperatorAdminStatusTypes  AdminStatus,
                                       DateTime                      Timestamp,
                                       JObject?                      CustomData     = null,
                                       UserDefinedDictionary?        InternalData   = null)

            : base(CustomData,
                   InternalData,
                   org.GraphDefined.Vanaheimr.Illias.Timestamp.Now)

        {

            this.Id           = Id;
            this.AdminStatus  = AdminStatus;
            this.Timestamp    = Timestamp;

        }

        #endregion

        #endregion


        #region (static) Snapshot(GridOperator)

        /// <summary>
        /// Take a snapshot of the current grid operator admin status.
        /// </summary>
        /// <param name="GridOperator">A grid operator.</param>
        public static GridOperatorAdminStatus Snapshot(GridOperator GridOperator)

            => new (GridOperator.Id,
                    GridOperator.AdminStatus);

        #endregion


        #region Operator overloading

        #region Operator == (GridOperatorAdminStatus1, GridOperatorAdminStatus2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="GridOperatorAdminStatus1">A grid operator admin status.</param>
        /// <param name="GridOperatorAdminStatus2">Another grid operator admin status.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator == (GridOperatorAdminStatus GridOperatorAdminStatus1,
                                           GridOperatorAdminStatus GridOperatorAdminStatus2)
        {

            // If both are null, or both are same instance, return true.
            if (ReferenceEquals(GridOperatorAdminStatus1, GridOperatorAdminStatus2))
                return true;

            // If one is null, but not both, return false.
            if (GridOperatorAdminStatus1 is null || GridOperatorAdminStatus2 is null)
                return false;

            return GridOperatorAdminStatus1.Equals(GridOperatorAdminStatus2);

        }

        #endregion

        #region Operator != (GridOperatorAdminStatus1, GridOperatorAdminStatus2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="GridOperatorAdminStatus1">A grid operator admin status.</param>
        /// <param name="GridOperatorAdminStatus2">Another grid operator admin status.</param>
        /// <returns>False if both match; True otherwise.</returns>
        public static Boolean operator != (GridOperatorAdminStatus GridOperatorAdminStatus1,
                                           GridOperatorAdminStatus GridOperatorAdminStatus2)

            => !(GridOperatorAdminStatus1 == GridOperatorAdminStatus2);

        #endregion

        #region Operator <  (GridOperatorAdminStatus1, GridOperatorAdminStatus2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="GridOperatorAdminStatus1">A grid operator admin status.</param>
        /// <param name="GridOperatorAdminStatus2">Another grid operator admin status.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator < (GridOperatorAdminStatus GridOperatorAdminStatus1,
                                          GridOperatorAdminStatus GridOperatorAdminStatus2)
        {

            if (GridOperatorAdminStatus1 is null)
                throw new ArgumentNullException(nameof(GridOperatorAdminStatus1), "The given GridOperatorAdminStatus1 must not be null!");

            return GridOperatorAdminStatus1.CompareTo(GridOperatorAdminStatus2) < 0;

        }

        #endregion

        #region Operator <= (GridOperatorAdminStatus1, GridOperatorAdminStatus2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="GridOperatorAdminStatus1">A grid operator admin status.</param>
        /// <param name="GridOperatorAdminStatus2">Another grid operator admin status.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator <= (GridOperatorAdminStatus GridOperatorAdminStatus1,
                                           GridOperatorAdminStatus GridOperatorAdminStatus2)

            => !(GridOperatorAdminStatus1 > GridOperatorAdminStatus2);

        #endregion

        #region Operator >  (GridOperatorAdminStatus1, GridOperatorAdminStatus2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="GridOperatorAdminStatus1">A grid operator admin status.</param>
        /// <param name="GridOperatorAdminStatus2">Another grid operator admin status.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator > (GridOperatorAdminStatus GridOperatorAdminStatus1,
                                          GridOperatorAdminStatus GridOperatorAdminStatus2)
        {

            if (GridOperatorAdminStatus1 is null)
                throw new ArgumentNullException(nameof(GridOperatorAdminStatus1), "The given GridOperatorAdminStatus1 must not be null!");

            return GridOperatorAdminStatus1.CompareTo(GridOperatorAdminStatus2) > 0;

        }

        #endregion

        #region Operator >= (GridOperatorAdminStatus1, GridOperatorAdminStatus2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="GridOperatorAdminStatus1">A grid operator admin status.</param>
        /// <param name="GridOperatorAdminStatus2">Another grid operator admin status.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator >= (GridOperatorAdminStatus GridOperatorAdminStatus1,
                                           GridOperatorAdminStatus GridOperatorAdminStatus2)

            => !(GridOperatorAdminStatus1 < GridOperatorAdminStatus2);

        #endregion

        #endregion

        #region IComparable<GridOperatorAdminStatus> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        public Int32 CompareTo(Object? Object)

            => Object is GridOperatorAdminStatus chargingStationOperatorAdminStatus
                   ? CompareTo(chargingStationOperatorAdminStatus)
                   : throw new ArgumentException("The given object is not a grid operator admin status!",
                                                 nameof(Object));

        #endregion

        #region CompareTo(GridOperatorAdminStatus)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="GridOperatorAdminStatus">An object to compare with.</param>
        public Int32 CompareTo(GridOperatorAdminStatus? GridOperatorAdminStatus)
        {

            if (GridOperatorAdminStatus is null)
                throw new ArgumentNullException(nameof(GridOperatorAdminStatus), "The given grid operator admin status must not be null!");

            var c = Id.         CompareTo(GridOperatorAdminStatus.Id);

            if (c == 0)
                c = AdminStatus.CompareTo(GridOperatorAdminStatus.AdminStatus);

            if (c == 0)
                c = Timestamp.  CompareTo(GridOperatorAdminStatus.Timestamp);

            return c;

        }

        #endregion

        #endregion

        #region IEquatable<GridOperatorAdminStatus> Members

        #region Equals(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public override Boolean Equals(Object? Object)

            => Object is GridOperatorAdminStatus chargingStationOperatorAdminStatus &&
                   Equals(chargingStationOperatorAdminStatus);

        #endregion

        #region Equals(GridOperatorAdminStatus)

        /// <summary>
        /// Compares two GridOperator identifications for equality.
        /// </summary>
        /// <param name="GridOperatorAdminStatus">A grid operator identification to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public Boolean Equals(GridOperatorAdminStatus? GridOperatorAdminStatus)

            => GridOperatorAdminStatus is not null                     &&
               Id.         Equals(GridOperatorAdminStatus.Id)          &&
               AdminStatus.Equals(GridOperatorAdminStatus.AdminStatus) &&
               Timestamp.  Equals(GridOperatorAdminStatus.Timestamp);

        #endregion

        #endregion

        #region (override) GetHashCode()

        /// <summary>
        /// Return the HashCode of this object.
        /// </summary>
        public override Int32 GetHashCode()
        {
            unchecked
            {

                return Id.         GetHashCode() * 5 ^
                       AdminStatus.GetHashCode() * 3 ^
                       Timestamp.  GetHashCode();

            }
        }

        #endregion

        #region (override) ToString()

        /// <summary>
        /// Return a text representation of this object.
        /// </summary>
        public override String ToString()

            => String.Concat(Id, " -> ",
                             AdminStatus,
                             " since ",
                             Timestamp.ToISO8601());

        #endregion

    }

}
