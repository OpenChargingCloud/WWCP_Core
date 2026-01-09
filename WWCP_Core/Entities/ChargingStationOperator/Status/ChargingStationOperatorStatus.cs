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
    /// Extension methods for the charging station operator status.
    /// </summary>
    public static class ChargingStationOperatorStatusExtensions
    {

        #region ToJSON(this ChargingStationOperatorStatus, Skip = null, Take = null)

        public static JObject ToJSON(this IEnumerable<ChargingStationOperatorStatus>  ChargingStationOperatorStatus,
                                     UInt64?                                          Skip   = null,
                                     UInt64?                                          Take   = null)
        {

            #region Initial checks

            if (ChargingStationOperatorStatus is null || !ChargingStationOperatorStatus.Any())
                return new JObject();

            #endregion

            #region Maybe there are duplicate charging station operator identifications in the enumeration... take the newest one!

            var filteredStatus = new Dictionary<ChargingStationOperator_Id, ChargingStationOperatorStatus>();

            foreach (var status in ChargingStationOperatorStatus)
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

        #region Contains(this ChargingStationOperatorStatus, Id, Status)

        /// <summary>
        /// Check if the given enumeration of charging station operators and their current status
        /// contains the given pair of charging station operator identification and status.
        /// </summary>
        /// <param name="ChargingStationOperatorStatus">An enumeration of charging station operators and their current status.</param>
        /// <param name="Id">A charging station operator identification.</param>
        /// <param name="Status">A charging station operator status.</param>
        public static Boolean Contains(this IEnumerable<ChargingStationOperatorStatus>  ChargingStationOperatorStatus,
                                       ChargingStationOperator_Id                       Id,
                                       ChargingStationOperatorStatusTypes               Status)
        {

            foreach (var status in ChargingStationOperatorStatus)
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
    /// The current status of a charging station operator.
    /// </summary>
    public class ChargingStationOperatorStatus : AInternalData,
                                                 IEquatable <ChargingStationOperatorStatus>,
                                                 IComparable<ChargingStationOperatorStatus>
    {

        #region Properties

        /// <summary>
        /// The unique identification of the charging station operator.
        /// </summary>
        public ChargingStationOperator_Id          Id           { get; }

        /// <summary>
        /// The current status of the charging station operator.
        /// </summary>
        public ChargingStationOperatorStatusTypes  Status       { get; }

        /// <summary>
        /// The timestamp of the current status of the charging station operator.
        /// </summary>
        public DateTimeOffset                      Timestamp    { get; }

        /// <summary>
        /// The timestamped status of the charging station operator.
        /// </summary>
        public Timestamped<ChargingStationOperatorStatusTypes> TimestampedStatus
            => new (Timestamp, Status);

        #endregion

        #region Constructor(s)

        #region ChargingStationOperatorStatus(Id, Status,            CustomData = null, InternalData = null)

        /// <summary>
        /// Create a new charging station operator status.
        /// </summary>
        /// <param name="Id">The unique identification of the charging station operator.</param>
        /// <param name="Status">The current timestamped status of the charging station operator.</param>
        /// <param name="CustomData">An optional dictionary of customer-specific data.</param>
        public ChargingStationOperatorStatus(ChargingStationOperator_Id                       Id,
                                             Timestamped<ChargingStationOperatorStatusTypes>  Status,
                                             JObject?                                         CustomData     = null,
                                             UserDefinedDictionary?                           InternalData   = null)

            : base(CustomData,
                   InternalData,
                   org.GraphDefined.Vanaheimr.Illias.Timestamp.Now)

        {

            this.Id         = Id;
            this.Status     = Status.Value;
            this.Timestamp  = Status.Timestamp;

        }

        #endregion

        #region ChargingStationOperatorStatus(Id, Status, Timestamp, CustomData = null, InternalData = null)

        /// <summary>
        /// Create a new charging station operator status.
        /// </summary>
        /// <param name="Id">The unique identification of the charging station operator.</param>
        /// <param name="Status">The current status of the charging station operator.</param>
        /// <param name="Timestamp">The timestamp of the status change of the charging station operator.</param>
        /// <param name="CustomData">An optional dictionary of customer-specific data.</param>
        public ChargingStationOperatorStatus(ChargingStationOperator_Id          Id,
                                             ChargingStationOperatorStatusTypes  Status,
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


        #region (static) Snapshot(ChargingStationOperator)

        /// <summary>
        /// Take a snapshot of the current charging station operator status.
        /// </summary>
        /// <param name="ChargingStationOperator">A charging station operator.</param>
        public static ChargingStationOperatorStatus Snapshot(ChargingStationOperator ChargingStationOperator)

            => new (ChargingStationOperator.Id,
                    ChargingStationOperator.Status);

        #endregion


        #region Operator overloading

        #region Operator == (ChargingStationOperatorStatus1, ChargingStationOperatorStatus2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingStationOperatorStatus1">A charging station operator status.</param>
        /// <param name="ChargingStationOperatorStatus2">Another charging station operator status.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator == (ChargingStationOperatorStatus ChargingStationOperatorStatus1,
                                           ChargingStationOperatorStatus ChargingStationOperatorStatus2)
        {

            // If both are null, or both are same instance, return true.
            if (ReferenceEquals(ChargingStationOperatorStatus1, ChargingStationOperatorStatus2))
                return true;

            // If one is null, but not both, return false.
            if (ChargingStationOperatorStatus1 is null || ChargingStationOperatorStatus2 is null)
                return false;

            return ChargingStationOperatorStatus1.Equals(ChargingStationOperatorStatus2);

        }

        #endregion

        #region Operator != (ChargingStationOperatorStatus1, ChargingStationOperatorStatus2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingStationOperatorStatus1">A charging station operator status.</param>
        /// <param name="ChargingStationOperatorStatus2">Another charging station operator status.</param>
        /// <returns>False if both match; True otherwise.</returns>
        public static Boolean operator != (ChargingStationOperatorStatus ChargingStationOperatorStatus1,
                                           ChargingStationOperatorStatus ChargingStationOperatorStatus2)

            => !(ChargingStationOperatorStatus1 == ChargingStationOperatorStatus2);

        #endregion

        #region Operator <  (ChargingStationOperatorStatus1, ChargingStationOperatorStatus2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingStationOperatorStatus1">A charging station operator status.</param>
        /// <param name="ChargingStationOperatorStatus2">Another charging station operator status.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator < (ChargingStationOperatorStatus ChargingStationOperatorStatus1,
                                          ChargingStationOperatorStatus ChargingStationOperatorStatus2)
        {

            if (ChargingStationOperatorStatus1 is null)
                throw new ArgumentNullException(nameof(ChargingStationOperatorStatus1), "The given ChargingStationOperatorStatus1 must not be null!");

            return ChargingStationOperatorStatus1.CompareTo(ChargingStationOperatorStatus2) < 0;

        }

        #endregion

        #region Operator <= (ChargingStationOperatorStatus1, ChargingStationOperatorStatus2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingStationOperatorStatus1">A charging station operator status.</param>
        /// <param name="ChargingStationOperatorStatus2">Another charging station operator status.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator <= (ChargingStationOperatorStatus ChargingStationOperatorStatus1,
                                           ChargingStationOperatorStatus ChargingStationOperatorStatus2)

            => !(ChargingStationOperatorStatus1 > ChargingStationOperatorStatus2);

        #endregion

        #region Operator >  (ChargingStationOperatorStatus1, ChargingStationOperatorStatus2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingStationOperatorStatus1">A charging station operator status.</param>
        /// <param name="ChargingStationOperatorStatus2">Another charging station operator status.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator > (ChargingStationOperatorStatus ChargingStationOperatorStatus1,
                                          ChargingStationOperatorStatus ChargingStationOperatorStatus2)
        {

            if (ChargingStationOperatorStatus1 is null)
                throw new ArgumentNullException(nameof(ChargingStationOperatorStatus1), "The given ChargingStationOperatorStatus1 must not be null!");

            return ChargingStationOperatorStatus1.CompareTo(ChargingStationOperatorStatus2) > 0;

        }

        #endregion

        #region Operator >= (ChargingStationOperatorStatus1, ChargingStationOperatorStatus2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingStationOperatorStatus1">A charging station operator status.</param>
        /// <param name="ChargingStationOperatorStatus2">Another charging station operator status.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator >= (ChargingStationOperatorStatus ChargingStationOperatorStatus1,
                                           ChargingStationOperatorStatus ChargingStationOperatorStatus2)

            => !(ChargingStationOperatorStatus1 < ChargingStationOperatorStatus2);

        #endregion

        #endregion

        #region IComparable<ChargingStationOperatorStatus> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        public Int32 CompareTo(Object? Object)

            => Object is ChargingStationOperatorStatus chargingStationOperatorStatus
                   ? CompareTo(chargingStationOperatorStatus)
                   : throw new ArgumentException("The given object is not a charging station operator status!",
                                                 nameof(Object));

        #endregion

        #region CompareTo(ChargingStationOperatorStatus)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingStationOperatorStatus">An object to compare with.</param>
        public Int32 CompareTo(ChargingStationOperatorStatus? ChargingStationOperatorStatus)
        {

            if (ChargingStationOperatorStatus is null)
                throw new ArgumentNullException(nameof(ChargingStationOperatorStatus), "The given charging station operator status must not be null!");

            var c = Id.       CompareTo(ChargingStationOperatorStatus.Id);

            if (c == 0)
                c = Status.   CompareTo(ChargingStationOperatorStatus.Status);

            if (c == 0)
                c = Timestamp.CompareTo(ChargingStationOperatorStatus.Timestamp);

            return c;

        }

        #endregion

        #endregion

        #region IEquatable<ChargingStationOperatorStatus> Members

        #region Equals(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public override Boolean Equals(Object? Object)

            => Object is ChargingStationOperatorStatus chargingStationOperatorStatus &&
                   Equals(chargingStationOperatorStatus);

        #endregion

        #region Equals(ChargingStationOperatorStatus)

        /// <summary>
        /// Compares two ChargingStationOperator identifications for equality.
        /// </summary>
        /// <param name="ChargingStationOperatorStatus">A charging station operator identification to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public Boolean Equals(ChargingStationOperatorStatus? ChargingStationOperatorStatus)

            => ChargingStationOperatorStatus is not null              &&
               Id.       Equals(ChargingStationOperatorStatus.Id)     &&
               Status.   Equals(ChargingStationOperatorStatus.Status) &&
               Timestamp.Equals(ChargingStationOperatorStatus.Timestamp);

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
