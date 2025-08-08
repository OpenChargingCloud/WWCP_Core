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
    /// Extension methods for the charging pool status.
    /// </summary>
    public static class ChargingPoolStatusExtensions
    {

        #region ToJSON(this ChargingPoolStatus, Skip = null, Take = null)

        public static JObject ToJSON(this IEnumerable<ChargingPoolStatus>  ChargingPoolStatus,
                                     UInt64?                               Skip  = null,
                                     UInt64?                               Take  = null)
        {

            #region Initial checks

            if (ChargingPoolStatus is null || !ChargingPoolStatus.Any())
                return new JObject();

            #endregion

            #region Maybe there are duplicate charging pool identifications in the enumeration... take the newest one!

            var filteredStatus = new Dictionary<ChargingPool_Id, ChargingPoolStatus>();

            foreach (var status in ChargingPoolStatus)
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
                                                               new JArray(kvp.Value.Timestamp.ToISO8601(),
                                                                          kvp.Value.Status.   ToString())
                                                              )));

        }

        #endregion

        #region Contains(this ChargingPoolStatus, Id, Status)

        /// <summary>
        /// Check if the given enumeration of charging pools and their current status
        /// contains the given pair of charging pool identification and status.
        /// </summary>
        /// <param name="ChargingPoolStatus">An enumeration of charging pools and their current status.</param>
        /// <param name="Id">A charging pool identification.</param>
        /// <param name="Status">A charging pool status.</param>
        public static Boolean Contains(this IEnumerable<ChargingPoolStatus>  ChargingPoolStatus,
                                       ChargingPool_Id                       Id,
                                       ChargingPoolStatusType               Status)
        {

            foreach (var status in ChargingPoolStatus)
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
    /// The current status of a charging pool.
    /// </summary>
    public class ChargingPoolStatus : AInternalData,
                                      IEquatable <ChargingPoolStatus>,
                                      IComparable<ChargingPoolStatus>
    {

        #region Properties

        /// <summary>
        /// The unique identification of the charging pool.
        /// </summary>
        public ChargingPool_Id          Id           { get; }

        /// <summary>
        /// The current status of the charging pool.
        /// </summary>
        public ChargingPoolStatusType  Status       { get; }

        /// <summary>
        /// The timestamp of the current status of the charging pool.
        /// </summary>
        public DateTimeOffset          Timestamp    { get; }

        /// <summary>
        /// The timestamped status of the charging pool.
        /// </summary>
        public Timestamped<ChargingPoolStatusType> TimestampedStatus
            => new (Timestamp, Status);

        #endregion

        #region Constructor(s)

        #region ChargingPoolStatus(Id, Status,            CustomData = null, InternalData = null)

        /// <summary>
        /// Create a new charging pool status.
        /// </summary>
        /// <param name="Id">The unique identification of the charging pool.</param>
        /// <param name="Status">The current timestamped status of the charging pool.</param>
        /// <param name="CustomData">An optional dictionary of customer-specific data.</param>
        public ChargingPoolStatus(ChargingPool_Id                       Id,
                                  Timestamped<ChargingPoolStatusType>  Status,
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

        #region ChargingPoolStatus(Id, Status, Timestamp, CustomData = null, InternalData = null)

        /// <summary>
        /// Create a new charging pool status.
        /// </summary>
        /// <param name="Id">The unique identification of the charging pool.</param>
        /// <param name="Status">The current status of the charging pool.</param>
        /// <param name="Timestamp">The timestamp of the status change of the charging pool.</param>
        /// <param name="CustomData">An optional dictionary of customer-specific data.</param>
        public ChargingPoolStatus(ChargingPool_Id          Id,
                                  ChargingPoolStatusType  Status,
                                  DateTime                 Timestamp,
                                  JObject?                 CustomData     = null,
                                  UserDefinedDictionary?   InternalData   = null)

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


        #region (static) Snapshot(ChargingPool)

        /// <summary>
        /// Take a snapshot of the current charging pool status.
        /// </summary>
        /// <param name="ChargingPool">A charging pool.</param>
        public static ChargingPoolStatus Snapshot(ChargingPool ChargingPool)

            => new (ChargingPool.Id,
                    ChargingPool.Status);

        #endregion


        #region Operator overloading

        #region Operator == (ChargingPoolStatus1, ChargingPoolStatus2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingPoolStatus1">A charging pool status.</param>
        /// <param name="ChargingPoolStatus2">Another charging pool status.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator == (ChargingPoolStatus ChargingPoolStatus1,
                                           ChargingPoolStatus ChargingPoolStatus2)
        {

            // If both are null, or both are same instance, return true.
            if (ReferenceEquals(ChargingPoolStatus1, ChargingPoolStatus2))
                return true;

            // If one is null, but not both, return false.
            if (ChargingPoolStatus1 is null || ChargingPoolStatus2 is null)
                return false;

            return ChargingPoolStatus1.Equals(ChargingPoolStatus2);

        }

        #endregion

        #region Operator != (ChargingPoolStatus1, ChargingPoolStatus2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingPoolStatus1">A charging pool status.</param>
        /// <param name="ChargingPoolStatus2">Another charging pool status.</param>
        /// <returns>False if both match; True otherwise.</returns>
        public static Boolean operator != (ChargingPoolStatus ChargingPoolStatus1,
                                           ChargingPoolStatus ChargingPoolStatus2)

            => !(ChargingPoolStatus1 == ChargingPoolStatus2);

        #endregion

        #region Operator <  (ChargingPoolStatus1, ChargingPoolStatus2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingPoolStatus1">A charging pool status.</param>
        /// <param name="ChargingPoolStatus2">Another charging pool status.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator < (ChargingPoolStatus ChargingPoolStatus1,
                                          ChargingPoolStatus ChargingPoolStatus2)
        {

            if (ChargingPoolStatus1 is null)
                throw new ArgumentNullException(nameof(ChargingPoolStatus1), "The given ChargingPoolStatus1 must not be null!");

            return ChargingPoolStatus1.CompareTo(ChargingPoolStatus2) < 0;

        }

        #endregion

        #region Operator <= (ChargingPoolStatus1, ChargingPoolStatus2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingPoolStatus1">A charging pool status.</param>
        /// <param name="ChargingPoolStatus2">Another charging pool status.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator <= (ChargingPoolStatus ChargingPoolStatus1,
                                           ChargingPoolStatus ChargingPoolStatus2)

            => !(ChargingPoolStatus1 > ChargingPoolStatus2);

        #endregion

        #region Operator >  (ChargingPoolStatus1, ChargingPoolStatus2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingPoolStatus1">A charging pool status.</param>
        /// <param name="ChargingPoolStatus2">Another charging pool status.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator > (ChargingPoolStatus ChargingPoolStatus1,
                                          ChargingPoolStatus ChargingPoolStatus2)
        {

            if (ChargingPoolStatus1 is null)
                throw new ArgumentNullException(nameof(ChargingPoolStatus1), "The given ChargingPoolStatus1 must not be null!");

            return ChargingPoolStatus1.CompareTo(ChargingPoolStatus2) > 0;

        }

        #endregion

        #region Operator >= (ChargingPoolStatus1, ChargingPoolStatus2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingPoolStatus1">A charging pool status.</param>
        /// <param name="ChargingPoolStatus2">Another charging pool status.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator >= (ChargingPoolStatus ChargingPoolStatus1,
                                           ChargingPoolStatus ChargingPoolStatus2)

            => !(ChargingPoolStatus1 < ChargingPoolStatus2);

        #endregion

        #endregion

        #region IComparable<ChargingPoolStatus> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        public Int32 CompareTo(Object? Object)

            => Object is ChargingPoolStatus chargingPoolStatus
                   ? CompareTo(chargingPoolStatus)
                   : throw new ArgumentException("The given object is not a charging pool status!",
                                                 nameof(Object));

        #endregion

        #region CompareTo(ChargingPoolStatus)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingPoolStatus">An object to compare with.</param>
        public Int32 CompareTo(ChargingPoolStatus? ChargingPoolStatus)
        {

            if (ChargingPoolStatus is null)
                throw new ArgumentNullException(nameof(ChargingPoolStatus), "The given charging pool status must not be null!");

            var c = Id.       CompareTo(ChargingPoolStatus.Id);

            if (c == 0)
                c = Status.   CompareTo(ChargingPoolStatus.Status);

            if (c == 0)
                c = Timestamp.CompareTo(ChargingPoolStatus.Timestamp);

            return c;

        }

        #endregion

        #endregion

        #region IEquatable<ChargingPoolStatus> Members

        #region Equals(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public override Boolean Equals(Object? Object)

            => Object is ChargingPoolStatus chargingPoolStatus &&
                   Equals(chargingPoolStatus);

        #endregion

        #region Equals(ChargingPoolStatus)

        /// <summary>
        /// Compares two ChargingPool identifications for equality.
        /// </summary>
        /// <param name="ChargingPoolStatus">A charging pool identification to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public Boolean Equals(ChargingPoolStatus? ChargingPoolStatus)

            => ChargingPoolStatus is not null              &&
               Id.       Equals(ChargingPoolStatus.Id)     &&
               Status.   Equals(ChargingPoolStatus.Status) &&
               Timestamp.Equals(ChargingPoolStatus.Timestamp);

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
