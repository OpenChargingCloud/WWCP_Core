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
    /// Extension methods for the charging station status.
    /// </summary>
    public static class ChargingStationStatusExtensions
    {

        #region ToJSON(this ChargingStationStatus, Skip = null, Take = null)

        public static JObject ToJSON(this IEnumerable<ChargingStationStatus>  ChargingStationStatus,
                                     UInt64?                                  Skip   = null,
                                     UInt64?                                  Take   = null)
        {

            #region Initial checks

            if (ChargingStationStatus is null || !ChargingStationStatus.Any())
                return new JObject();

            #endregion

            #region Maybe there are duplicate charging station identifications in the enumeration... take the newest one!

            var filteredStatus = new Dictionary<ChargingStation_Id, ChargingStationStatus>();

            foreach (var status in ChargingStationStatus)
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

        #region Contains(this ChargingStationStatus, Id, Status)

        /// <summary>
        /// Check if the given enumeration of charging stations and their current status
        /// contains the given pair of charging station identification and status.
        /// </summary>
        /// <param name="ChargingStationStatus">An enumeration of charging stations and their current status.</param>
        /// <param name="Id">A charging station identification.</param>
        /// <param name="Status">A charging station status.</param>
        public static Boolean Contains(this IEnumerable<ChargingStationStatus>  ChargingStationStatus,
                                       ChargingStation_Id                       Id,
                                       ChargingStationStatusTypes               Status)
        {

            foreach (var status in ChargingStationStatus)
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
    /// The current status of a charging station.
    /// </summary>
    public class ChargingStationStatus : AInternalData,
                                                 IEquatable <ChargingStationStatus>,
                                                 IComparable<ChargingStationStatus>
    {

        #region Properties

        /// <summary>
        /// The unique identification of the charging station.
        /// </summary>
        public ChargingStation_Id          Id           { get; }

        /// <summary>
        /// The current status of the charging station.
        /// </summary>
        public ChargingStationStatusTypes  Status       { get; }

        /// <summary>
        /// The timestamp of the current status of the charging station.
        /// </summary>
        public DateTime                    Timestamp    { get; }

        /// <summary>
        /// The timestamped status of the charging station.
        /// </summary>
        public Timestamped<ChargingStationStatusTypes> TimestampedStatus
            => new (Timestamp, Status);

        #endregion

        #region Constructor(s)

        #region ChargingStationStatus(Id, Status,            CustomData = null, InternalData = null)

        /// <summary>
        /// Create a new charging station status.
        /// </summary>
        /// <param name="Id">The unique identification of the charging station.</param>
        /// <param name="Status">The current timestamped status of the charging station.</param>
        /// <param name="CustomData">An optional dictionary of customer-specific data.</param>
        public ChargingStationStatus(ChargingStation_Id                       Id,
                                     Timestamped<ChargingStationStatusTypes>  Status,
                                     JObject?                                 CustomData     = null,
                                     UserDefinedDictionary?                   InternalData   = null)

            : base(CustomData,
                   InternalData,
                   org.GraphDefined.Vanaheimr.Illias.Timestamp.Now)

        {

            this.Id         = Id;
            this.Status     = Status.Value;
            this.Timestamp  = Status.Timestamp;

        }

        #endregion

        #region ChargingStationStatus(Id, Status, Timestamp, CustomData = null, InternalData = null)

        /// <summary>
        /// Create a new charging station status.
        /// </summary>
        /// <param name="Id">The unique identification of the charging station.</param>
        /// <param name="Status">The current status of the charging station.</param>
        /// <param name="Timestamp">The timestamp of the status change of the charging station.</param>
        /// <param name="CustomData">An optional dictionary of customer-specific data.</param>
        public ChargingStationStatus(ChargingStation_Id          Id,
                                     ChargingStationStatusTypes  Status,
                                     DateTime                    Timestamp,
                                     JObject?                    CustomData     = null,
                                     UserDefinedDictionary?      InternalData   = null)

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


        #region (static) Snapshot(ChargingStation)

        /// <summary>
        /// Take a snapshot of the current charging station status.
        /// </summary>
        /// <param name="ChargingStation">A charging station.</param>
        public static ChargingStationStatus Snapshot(ChargingStation ChargingStation)

            => new (ChargingStation.Id,
                    ChargingStation.Status);

        #endregion


        #region Operator overloading

        #region Operator == (ChargingStationStatus1, ChargingStationStatus2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingStationStatus1">A charging station status.</param>
        /// <param name="ChargingStationStatus2">Another charging station status.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator == (ChargingStationStatus ChargingStationStatus1,
                                           ChargingStationStatus ChargingStationStatus2)
        {

            // If both are null, or both are same instance, return true.
            if (ReferenceEquals(ChargingStationStatus1, ChargingStationStatus2))
                return true;

            // If one is null, but not both, return false.
            if (ChargingStationStatus1 is null || ChargingStationStatus2 is null)
                return false;

            return ChargingStationStatus1.Equals(ChargingStationStatus2);

        }

        #endregion

        #region Operator != (ChargingStationStatus1, ChargingStationStatus2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingStationStatus1">A charging station status.</param>
        /// <param name="ChargingStationStatus2">Another charging station status.</param>
        /// <returns>False if both match; True otherwise.</returns>
        public static Boolean operator != (ChargingStationStatus ChargingStationStatus1,
                                           ChargingStationStatus ChargingStationStatus2)

            => !(ChargingStationStatus1 == ChargingStationStatus2);

        #endregion

        #region Operator <  (ChargingStationStatus1, ChargingStationStatus2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingStationStatus1">A charging station status.</param>
        /// <param name="ChargingStationStatus2">Another charging station status.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator < (ChargingStationStatus ChargingStationStatus1,
                                          ChargingStationStatus ChargingStationStatus2)
        {

            if (ChargingStationStatus1 is null)
                throw new ArgumentNullException(nameof(ChargingStationStatus1), "The given ChargingStationStatus1 must not be null!");

            return ChargingStationStatus1.CompareTo(ChargingStationStatus2) < 0;

        }

        #endregion

        #region Operator <= (ChargingStationStatus1, ChargingStationStatus2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingStationStatus1">A charging station status.</param>
        /// <param name="ChargingStationStatus2">Another charging station status.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator <= (ChargingStationStatus ChargingStationStatus1,
                                           ChargingStationStatus ChargingStationStatus2)

            => !(ChargingStationStatus1 > ChargingStationStatus2);

        #endregion

        #region Operator >  (ChargingStationStatus1, ChargingStationStatus2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingStationStatus1">A charging station status.</param>
        /// <param name="ChargingStationStatus2">Another charging station status.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator > (ChargingStationStatus ChargingStationStatus1,
                                          ChargingStationStatus ChargingStationStatus2)
        {

            if (ChargingStationStatus1 is null)
                throw new ArgumentNullException(nameof(ChargingStationStatus1), "The given ChargingStationStatus1 must not be null!");

            return ChargingStationStatus1.CompareTo(ChargingStationStatus2) > 0;

        }

        #endregion

        #region Operator >= (ChargingStationStatus1, ChargingStationStatus2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingStationStatus1">A charging station status.</param>
        /// <param name="ChargingStationStatus2">Another charging station status.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator >= (ChargingStationStatus ChargingStationStatus1,
                                           ChargingStationStatus ChargingStationStatus2)

            => !(ChargingStationStatus1 < ChargingStationStatus2);

        #endregion

        #endregion

        #region IComparable<ChargingStationStatus> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        public Int32 CompareTo(Object? Object)

            => Object is ChargingStationStatus chargingStationStatus
                   ? CompareTo(chargingStationStatus)
                   : throw new ArgumentException("The given object is not a charging station status!",
                                                 nameof(Object));

        #endregion

        #region CompareTo(ChargingStationStatus)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingStationStatus">An object to compare with.</param>
        public Int32 CompareTo(ChargingStationStatus? ChargingStationStatus)
        {

            if (ChargingStationStatus is null)
                throw new ArgumentNullException(nameof(ChargingStationStatus), "The given charging station status must not be null!");

            var c = Id.       CompareTo(ChargingStationStatus.Id);

            if (c == 0)
                c = Status.   CompareTo(ChargingStationStatus.Status);

            if (c == 0)
                c = Timestamp.CompareTo(ChargingStationStatus.Timestamp);

            return c;

        }

        #endregion

        #endregion

        #region IEquatable<ChargingStationStatus> Members

        #region Equals(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public override Boolean Equals(Object? Object)

            => Object is ChargingStationStatus chargingStationStatus &&
                   Equals(chargingStationStatus);

        #endregion

        #region Equals(ChargingStationStatus)

        /// <summary>
        /// Compares two ChargingStation identifications for equality.
        /// </summary>
        /// <param name="ChargingStationStatus">A charging station identification to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public Boolean Equals(ChargingStationStatus? ChargingStationStatus)

            => ChargingStationStatus is not null              &&
               Id.       Equals(ChargingStationStatus.Id)     &&
               Status.   Equals(ChargingStationStatus.Status) &&
               Timestamp.Equals(ChargingStationStatus.Timestamp);

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
