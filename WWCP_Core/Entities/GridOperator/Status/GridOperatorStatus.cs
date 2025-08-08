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

using Newtonsoft.Json.Linq;

using org.GraphDefined.Vanaheimr.Illias;

#endregion

namespace cloud.charging.open.protocols.WWCP
{

    /// <summary>
    /// Extension methods for the grid operator status.
    /// </summary>
    public static class GridOperatorStatusExtensions
    {

        #region ToJSON(this GridOperatorStatus, Skip = null, Take = null)

        public static JObject ToJSON(this IEnumerable<GridOperatorStatus>  GridOperatorStatus,
                                     UInt64?                                          Skip   = null,
                                     UInt64?                                          Take   = null)
        {

            #region Initial checks

            if (GridOperatorStatus is null || !GridOperatorStatus.Any())
                return new JObject();

            #endregion

            #region Maybe there are duplicate grid operator identifications in the enumeration... take the newest one!

            var filteredStatus = new Dictionary<GridOperator_Id, GridOperatorStatus>();

            foreach (var status in GridOperatorStatus)
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
                                                                          kvp.Value.Status.ToString())
                                                              )));

        }

        #endregion

        #region Contains(this GridOperatorStatus, Id, Status)

        /// <summary>
        /// Check if the given enumeration of grid operators and their current status
        /// contains the given pair of grid operator identification and status.
        /// </summary>
        /// <param name="GridOperatorStatus">An enumeration of grid operators and their current status.</param>
        /// <param name="Id">A grid operator identification.</param>
        /// <param name="Status">A grid operator status.</param>
        public static Boolean Contains(this IEnumerable<GridOperatorStatus>  GridOperatorStatus,
                                       GridOperator_Id                       Id,
                                       GridOperatorStatusTypes               Status)
        {

            foreach (var status in GridOperatorStatus)
            {
                if (status.Id     == Id &&
                    status.Status == Status)
                {
                    return true;
                }
            }

            return false;

        }

        #endregion

    }


    /// <summary>
    /// The current status of a grid operator.
    /// </summary>
    public class GridOperatorStatus : AInternalData,
                                                 IEquatable <GridOperatorStatus>,
                                                 IComparable<GridOperatorStatus>
    {

        #region Properties

        /// <summary>
        /// The unique identification of the grid operator.
        /// </summary>
        public GridOperator_Id          Id           { get; }

        /// <summary>
        /// The current status of the grid operator.
        /// </summary>
        public GridOperatorStatusTypes  Status       { get; }

        /// <summary>
        /// The timestamp of the current status of the grid operator.
        /// </summary>
        public DateTimeOffset           Timestamp    { get; }

        /// <summary>
        /// The timestamped status of the grid operator.
        /// </summary>
        public Timestamped<GridOperatorStatusTypes> TimestampedStatus
            => new (Timestamp, Status);

        #endregion

        #region Constructor(s)

        #region GridOperatorStatus(Id, Status,            CustomData = null, InternalData = null)

        /// <summary>
        /// Create a new grid operator status.
        /// </summary>
        /// <param name="Id">The unique identification of the grid operator.</param>
        /// <param name="Status">The current timestamped status of the grid operator.</param>
        /// <param name="CustomData">An optional dictionary of customer-specific data.</param>
        public GridOperatorStatus(GridOperator_Id                       Id,
                                  Timestamped<GridOperatorStatusTypes>  Status,
                                  JObject?                              CustomData     = null,
                                  UserDefinedDictionary?                InternalData   = null)

            : base(CustomData,
                   InternalData,
                   org.GraphDefined.Vanaheimr.Illias.Timestamp.Now)

        {

            this.Id         = Id;
            this.Status     = Status.Value;
            this.Timestamp  = Status.Timestamp;

        }

        #endregion

        #region GridOperatorStatus(Id, Status, Timestamp, CustomData = null, InternalData = null)

        /// <summary>
        /// Create a new grid operator status.
        /// </summary>
        /// <param name="Id">The unique identification of the grid operator.</param>
        /// <param name="Status">The current status of the grid operator.</param>
        /// <param name="Timestamp">The timestamp of the status change of the grid operator.</param>
        /// <param name="CustomData">An optional dictionary of customer-specific data.</param>
        public GridOperatorStatus(GridOperator_Id          Id,
                                             GridOperatorStatusTypes  Status,
                                             DateTime                            Timestamp,
                                             JObject?                            CustomData     = null,
                                             UserDefinedDictionary?              InternalData   = null)

            : base(CustomData,
                   InternalData,
                   org.GraphDefined.Vanaheimr.Illias.Timestamp.Now)

        {

            this.Id         = Id;
            this.Status     = Status;
            this.Timestamp  = Timestamp;

        }

        #endregion

        #endregion


        #region (static) Snapshot(GridOperator)

        /// <summary>
        /// Take a snapshot of the current grid operator status.
        /// </summary>
        /// <param name="GridOperator">A grid operator.</param>
        public static GridOperatorStatus Snapshot(GridOperator GridOperator)

            => new (GridOperator.Id,
                    GridOperator.Status);

        #endregion


        #region Operator overloading

        #region Operator == (GridOperatorStatus1, GridOperatorStatus2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="GridOperatorStatus1">A grid operator status.</param>
        /// <param name="GridOperatorStatus2">Another grid operator status.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator == (GridOperatorStatus GridOperatorStatus1,
                                           GridOperatorStatus GridOperatorStatus2)
        {

            // If both are null, or both are same instance, return true.
            if (ReferenceEquals(GridOperatorStatus1, GridOperatorStatus2))
                return true;

            // If one is null, but not both, return false.
            if (GridOperatorStatus1 is null || GridOperatorStatus2 is null)
                return false;

            return GridOperatorStatus1.Equals(GridOperatorStatus2);

        }

        #endregion

        #region Operator != (GridOperatorStatus1, GridOperatorStatus2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="GridOperatorStatus1">A grid operator status.</param>
        /// <param name="GridOperatorStatus2">Another grid operator status.</param>
        /// <returns>False if both match; True otherwise.</returns>
        public static Boolean operator != (GridOperatorStatus GridOperatorStatus1,
                                           GridOperatorStatus GridOperatorStatus2)

            => !(GridOperatorStatus1 == GridOperatorStatus2);

        #endregion

        #region Operator <  (GridOperatorStatus1, GridOperatorStatus2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="GridOperatorStatus1">A grid operator status.</param>
        /// <param name="GridOperatorStatus2">Another grid operator status.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator < (GridOperatorStatus GridOperatorStatus1,
                                          GridOperatorStatus GridOperatorStatus2)
        {

            if (GridOperatorStatus1 is null)
                throw new ArgumentNullException(nameof(GridOperatorStatus1), "The given GridOperatorStatus1 must not be null!");

            return GridOperatorStatus1.CompareTo(GridOperatorStatus2) < 0;

        }

        #endregion

        #region Operator <= (GridOperatorStatus1, GridOperatorStatus2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="GridOperatorStatus1">A grid operator status.</param>
        /// <param name="GridOperatorStatus2">Another grid operator status.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator <= (GridOperatorStatus GridOperatorStatus1,
                                           GridOperatorStatus GridOperatorStatus2)

            => !(GridOperatorStatus1 > GridOperatorStatus2);

        #endregion

        #region Operator >  (GridOperatorStatus1, GridOperatorStatus2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="GridOperatorStatus1">A grid operator status.</param>
        /// <param name="GridOperatorStatus2">Another grid operator status.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator > (GridOperatorStatus GridOperatorStatus1,
                                          GridOperatorStatus GridOperatorStatus2)
        {

            if (GridOperatorStatus1 is null)
                throw new ArgumentNullException(nameof(GridOperatorStatus1), "The given GridOperatorStatus1 must not be null!");

            return GridOperatorStatus1.CompareTo(GridOperatorStatus2) > 0;

        }

        #endregion

        #region Operator >= (GridOperatorStatus1, GridOperatorStatus2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="GridOperatorStatus1">A grid operator status.</param>
        /// <param name="GridOperatorStatus2">Another grid operator status.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator >= (GridOperatorStatus GridOperatorStatus1,
                                           GridOperatorStatus GridOperatorStatus2)

            => !(GridOperatorStatus1 < GridOperatorStatus2);

        #endregion

        #endregion

        #region IComparable<GridOperatorStatus> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        public Int32 CompareTo(Object? Object)

            => Object is GridOperatorStatus chargingStationOperatorStatus
                   ? CompareTo(chargingStationOperatorStatus)
                   : throw new ArgumentException("The given object is not a grid operator status!",
                                                 nameof(Object));

        #endregion

        #region CompareTo(GridOperatorStatus)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="GridOperatorStatus">An object to compare with.</param>
        public Int32 CompareTo(GridOperatorStatus? GridOperatorStatus)
        {

            if (GridOperatorStatus is null)
                throw new ArgumentNullException(nameof(GridOperatorStatus), "The given grid operator status must not be null!");

            var c = Id.       CompareTo(GridOperatorStatus.Id);

            if (c == 0)
                c = Status.   CompareTo(GridOperatorStatus.Status);

            if (c == 0)
                c = Timestamp.CompareTo(GridOperatorStatus.Timestamp);

            return c;

        }

        #endregion

        #endregion

        #region IEquatable<GridOperatorStatus> Members

        #region Equals(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public override Boolean Equals(Object? Object)

            => Object is GridOperatorStatus chargingStationOperatorStatus &&
                   Equals(chargingStationOperatorStatus);

        #endregion

        #region Equals(GridOperatorStatus)

        /// <summary>
        /// Compares two GridOperator identifications for equality.
        /// </summary>
        /// <param name="GridOperatorStatus">A grid operator identification to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public Boolean Equals(GridOperatorStatus? GridOperatorStatus)

            => GridOperatorStatus is not null              &&
               Id.       Equals(GridOperatorStatus.Id)     &&
               Status.   Equals(GridOperatorStatus.Status) &&
               Timestamp.Equals(GridOperatorStatus.Timestamp);

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

                return Id.       GetHashCode() * 5 ^
                       Status.   GetHashCode() * 3 ^
                       Timestamp.GetHashCode();

            }
        }

        #endregion

        #region (override) ToString()

        /// <summary>
        /// Return a text representation of this object.
        /// </summary>
        public override String ToString()

            => String.Concat(Id, " -> ",
                             Status,
                             " since ",
                             Timestamp.ToISO8601());

        #endregion

    }

}
