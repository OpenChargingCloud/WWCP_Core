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
    /// Extension methods for the charging station status.
    /// </summary>
    public static class ChargingStationStatusExtensions
    {

        public static IEnumerable<IStatus<ChargingStation_Id, ChargingStationStatusType>> ToStatusList(this ChargingStationStatus[] StatusList)
            => StatusList.Select(status => status as IStatus<ChargingStation_Id, ChargingStationStatusType>);

        public static JObject ToJSON(this ChargingStationStatus[] StatusList)
            => StatusList.ToStatusList().ToJSON();


        public static JObject ToJSON(this IEnumerable<ChargingStationStatus>  StatusList,
                                     UInt64?                                  Skip   = null,
                                     UInt64?                                  Take   = null)

            => StatusList.ToJSON(
                   Skip,
                   Take
               );


    }


    /// <summary>
    /// The current status of a charging station.
    /// </summary>
    public readonly struct ChargingStationStatus : IStatus<ChargingStation_Id, ChargingStationStatusType>,
                                                   IEquatable <ChargingStationStatus>,
                                                   IComparable<ChargingStationStatus>,
                                                   IComparable
    {

        #region Properties

        /// <summary>
        /// The unique identification of the charging station.
        /// </summary>
        public ChargingStation_Id         Id           { get; }

        /// <summary>
        /// The current status of the charging station.
        /// </summary>
        public ChargingStationStatusType  Status       { get; }

        /// <summary>
        /// The timestamp of the current status of the charging station.
        /// </summary>
        public DateTimeOffset             Timestamp    { get; }

        /// <summary>
        /// An optional data source or context for this charging station status.
        /// </summary>
        public Context?                   Context      { get; }

        #endregion

        #region Constructor(s)

        #region ChargingStationStatus(Id, Status,            Context = null)

        /// <summary>
        /// Create a new charging station status.
        /// </summary>
        /// <param name="Id">The unique identification of the charging station.</param>
        /// <param name="Status">The current timestamped status of the charging station.</param>
        /// <param name="Context">An optional data source or context for the charging station status.</param>
        public ChargingStationStatus(ChargingStation_Id                      Id,
                                     Timestamped<ChargingStationStatusType>  Status,
                                     Context?                                Context   = null)

            : this(Id,
                   Status.Value,
                   Status.Timestamp,
                   Context)

        { }

        #endregion

        #region ChargingStationStatus(Id, Status, Timestamp, Context = null)

        /// <summary>
        /// Create a new charging station status.
        /// </summary>
        /// <param name="Id">The unique identification of the charging station.</param>
        /// <param name="Status">The current status of the charging station.</param>
        /// <param name="Timestamp">The timestamp of the status change of the charging station.</param>
        /// <param name="Context">An optional data source or context for the charging station status.</param>
        public ChargingStationStatus(ChargingStation_Id         Id,
                                     ChargingStationStatusType  Status,
                                     DateTimeOffset             Timestamp,
                                     Context?                   Context   = null)
        {

            this.Id         = Id;
            this.Status     = Status;
            this.Timestamp  = Timestamp;
            this.Context    = Context;

            unchecked
            {

                hashCode = Id.       GetHashCode() * 7 ^
                           Status.   GetHashCode() * 5 ^
                           Timestamp.GetHashCode() * 3 ^
                          (Context?. GetHashCode() ?? 0);

            }

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

            => ChargingStationStatus1.Equals(ChargingStationStatus2);

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

            => !ChargingStationStatus1.Equals(ChargingStationStatus2);

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

            => ChargingStationStatus1.CompareTo(ChargingStationStatus2) < 0;

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

            => ChargingStationStatus1.CompareTo(ChargingStationStatus2) <= 0;

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

            => ChargingStationStatus1.CompareTo(ChargingStationStatus2) > 0;

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

            => ChargingStationStatus1.CompareTo(ChargingStationStatus2) >= 0;

        #endregion

        #endregion

        #region IComparable<ChargingStationStatus> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two charging station status.
        /// </summary>
        /// <param name="Object">A charging station status to compare with.</param>
        public Int32 CompareTo(Object? Object)

            => Object is ChargingStationStatus chargingStationStatus
                   ? CompareTo(chargingStationStatus)
                   : throw new ArgumentException("The given object is not a charging station status!",
                                                 nameof(Object));

        #endregion

        #region CompareTo(ChargingStationStatus)

        /// <summary>
        /// Compares two charging station status.
        /// </summary>
        /// <param name="ChargingStationStatus">A charging station status to compare with.</param>
        public Int32 CompareTo(ChargingStationStatus ChargingStationStatus)
        {

            var c = Id.                   CompareTo(ChargingStationStatus.Id);

            if (c == 0)
                c = Status.               CompareTo(ChargingStationStatus.Status);

            if (c == 0)
                c = Timestamp.ToISO8601().CompareTo(ChargingStationStatus.Timestamp.ToISO8601());

            if (c == 0 && Context is not null && ChargingStationStatus.Context is not null)
                c = Context.              CompareTo(ChargingStationStatus.Context);

            return c;

        }

        #endregion

        #endregion

        #region IEquatable<ChargingStationStatus> Members

        #region Equals(Object)

        /// <summary>
        /// Compares two charging station status for equality.
        /// </summary>
        /// <param name="Object">A charging station status to compare with.</param>
        public override Boolean Equals(Object? Object)

            => Object is ChargingStationStatus chargingStationStatus &&
                   Equals(chargingStationStatus);

        #endregion

        #region Equals(ChargingStationStatus)

        /// <summary>
        /// Compares two charging station status for equality.
        /// </summary>
        /// <param name="ChargingStationStatus">A charging station status to compare with.</param>
        public Boolean Equals(ChargingStationStatus ChargingStationStatus)

            => Id.                   Equals(ChargingStationStatus.Id)                    &&
               Status.               Equals(ChargingStationStatus.Status)                &&
               Timestamp.ToISO8601().Equals(ChargingStationStatus.Timestamp.ToISO8601()) &&

             ((Context is null     && ChargingStationStatus.Context is null) ||
              (Context is not null && ChargingStationStatus.Context is not null && Context.Equals(ChargingStationStatus.Context)));

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

            => $"{Id} -> '{Status}' since {Timestamp.ToISO8601()}";

        #endregion

    }

}
