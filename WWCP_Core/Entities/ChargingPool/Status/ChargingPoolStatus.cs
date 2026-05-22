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
    /// Extension methods for the charging pool status.
    /// </summary>
    public static class ChargingPoolStatusExtensions
    {

        public static IEnumerable<IStatus<ChargingPool_Id, ChargingPoolStatusType>> ToStatusList(this ChargingPoolStatus[] StatusList)
            => StatusList.Select(status => status as IStatus<ChargingPool_Id, ChargingPoolStatusType>);

        public static JObject ToJSON(this ChargingPoolStatus[] StatusList)
            => StatusList.ToStatusList().ToJSON();

        public static JObject ToJSON(this IEnumerable<ChargingPoolStatus>  StatusList,
                                     UInt64?                               Skip   = null,
                                     UInt64?                               Take   = null)

            => StatusList.ToJSON(
                   Skip,
                   Take
               );

    }


    /// <summary>
    /// The current status of a charging pool.
    /// </summary>
    public readonly struct ChargingPoolStatus : IStatus<ChargingPool_Id, ChargingPoolStatusType>,
                                                   IEquatable <ChargingPoolStatus>,
                                                   IComparable<ChargingPoolStatus>,
                                                   IComparable
    {

        #region Properties

        /// <summary>
        /// The unique identification of the charging pool.
        /// </summary>
        public ChargingPool_Id         Id           { get; }

        /// <summary>
        /// The current status of the charging pool.
        /// </summary>
        public ChargingPoolStatusType  Status       { get; }

        /// <summary>
        /// The timestamp of the current status of the charging pool.
        /// </summary>
        public DateTimeOffset          Timestamp    { get; }

        /// <summary>
        /// An optional data source or context for this charging pool status.
        /// </summary>
        public Context?                Context      { get; }

        #endregion

        #region Constructor(s)

        #region ChargingPoolStatus(Id, Status,            Context = null)

        /// <summary>
        /// Create a new charging pool status.
        /// </summary>
        /// <param name="Id">The unique identification of the charging pool.</param>
        /// <param name="Status">The current timestamped status of the charging pool.</param>
        /// <param name="Context">An optional data source or context for the charging pool status.</param>
        public ChargingPoolStatus(ChargingPool_Id                      Id,
                                  Timestamped<ChargingPoolStatusType>  Status,
                                  Context?                             Context   = null)

            : this(Id,
                   Status.Value,
                   Status.Timestamp,
                   Context)

        { }

        #endregion

        #region ChargingPoolStatus(Id, Status, Timestamp, Context = null)

        /// <summary>
        /// Create a new charging pool status.
        /// </summary>
        /// <param name="Id">The unique identification of the charging pool.</param>
        /// <param name="Status">The current status of the charging pool.</param>
        /// <param name="Timestamp">The timestamp of the status change of the charging pool.</param>
        /// <param name="Context">An optional data source or context for the charging pool status.</param>
        public ChargingPoolStatus(ChargingPool_Id         Id,
                                  ChargingPoolStatusType  Status,
                                  DateTimeOffset          Timestamp,
                                  Context?                Context   = null)
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

            => ChargingPoolStatus1.Equals(ChargingPoolStatus2);

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

            => !ChargingPoolStatus1.Equals(ChargingPoolStatus2);

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

            => ChargingPoolStatus1.CompareTo(ChargingPoolStatus2) < 0;

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

            => ChargingPoolStatus1.CompareTo(ChargingPoolStatus2) <= 0;

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

            => ChargingPoolStatus1.CompareTo(ChargingPoolStatus2) > 0;

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

            => ChargingPoolStatus1.CompareTo(ChargingPoolStatus2) >= 0;

        #endregion

        #endregion

        #region IComparable<ChargingPoolStatus> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two charging pool status.
        /// </summary>
        /// <param name="Object">A charging pool status to compare with.</param>
        public Int32 CompareTo(Object? Object)

            => Object is ChargingPoolStatus chargingPoolStatus
                   ? CompareTo(chargingPoolStatus)
                   : throw new ArgumentException("The given object is not a charging pool status!",
                                                 nameof(Object));

        #endregion

        #region CompareTo(ChargingPoolStatus)

        /// <summary>
        /// Compares two charging pool status.
        /// </summary>
        /// <param name="ChargingPoolStatus">A charging pool status to compare with.</param>
        public Int32 CompareTo(ChargingPoolStatus ChargingPoolStatus)
        {

            var c = Id.                   CompareTo(ChargingPoolStatus.Id);

            if (c == 0)
                c = Status.               CompareTo(ChargingPoolStatus.Status);

            if (c == 0)
                c = Timestamp.ToISO8601().CompareTo(ChargingPoolStatus.Timestamp.ToISO8601());

            if (c == 0 && Context is not null && ChargingPoolStatus.Context is not null)
                c = Context.              CompareTo(ChargingPoolStatus.Context);

            return c;

        }

        #endregion

        #endregion

        #region IEquatable<ChargingPoolStatus> Members

        #region Equals(Object)

        /// <summary>
        /// Compares two charging pool status for equality.
        /// </summary>
        /// <param name="Object">A charging pool status to compare with.</param>
        public override Boolean Equals(Object? Object)

            => Object is ChargingPoolStatus chargingPoolStatus &&
                   Equals(chargingPoolStatus);

        #endregion

        #region Equals(ChargingPoolStatus)

        /// <summary>
        /// Compares two charging pool status for equality.
        /// </summary>
        /// <param name="ChargingPoolStatus">A charging pool status to compare with.</param>
        public Boolean Equals(ChargingPoolStatus ChargingPoolStatus)

            => Id.                   Equals(ChargingPoolStatus.Id)                    &&
               Status.               Equals(ChargingPoolStatus.Status)                &&
               Timestamp.ToISO8601().Equals(ChargingPoolStatus.Timestamp.ToISO8601()) &&

             ((Context is null     && ChargingPoolStatus.Context is null) ||
              (Context is not null && ChargingPoolStatus.Context is not null && Context.Equals(ChargingPoolStatus.Context)));

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
