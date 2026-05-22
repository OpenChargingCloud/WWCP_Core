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
    /// Extension methods for the charging station admin status.
    /// </summary>
    public static class ChargingStationAdminStatusExtensions
    {

        public static IEnumerable<IStatus<ChargingStation_Id, ChargingStationAdminStatusType>> ToStatusList(this ChargingStationAdminStatus[] StatusList)
            => StatusList.Select(status => status as IStatus<ChargingStation_Id, ChargingStationAdminStatusType>);

        public static JObject ToJSON(this ChargingStationAdminStatus[] StatusList)
            => StatusList.ToStatusList().ToJSON();

        public static JObject ToJSON(this IEnumerable<ChargingStationAdminStatus>  StatusList,
                                     UInt64?                                       Skip   = null,
                                     UInt64?                                       Take   = null)

            => StatusList.ToJSON(
                   Skip,
                   Take
               );


    }


    /// <summary>
    /// The current admin status of a charging station.
    /// </summary>
    public readonly struct ChargingStationAdminStatus : IStatus<ChargingStation_Id, ChargingStationAdminStatusType>,
                                                        IEquatable<ChargingStationAdminStatus>,
                                                        IComparable<ChargingStationAdminStatus>,
                                                        IComparable
    {

        #region Properties

        /// <summary>
        /// The unique identification of the charging station.
        /// </summary>
        public ChargingStation_Id              Id           { get; }

        /// <summary>
        /// The current admin status of the charging station.
        /// </summary>
        public ChargingStationAdminStatusType  Status       { get; }

        /// <summary>
        /// The timestamp of the current admin status of the charging station.
        /// </summary>
        public DateTimeOffset                  Timestamp    { get; }

        /// <summary>
        /// An optional data source or context for this charging station admin status.
        /// </summary>
        public Context?                        Context      { get; }

        #endregion

        #region Constructor(s)

        #region ChargingStationAdminStatus(Id, Status,            Context = null)

        /// <summary>
        /// Create a new charging station admin status.
        /// </summary>
        /// <param name="Id">The unique identification of the charging station.</param>
        /// <param name="Status">The current timestamped admin status of the charging station.</param>
        /// <param name="Context">An optional data source or context for the charging station admin status.</param>
        public ChargingStationAdminStatus(ChargingStation_Id                           Id,
                                          Timestamped<ChargingStationAdminStatusType>  Status,
                                          Context?                                     Context   = null)

            : this(Id,
                   Status.Value,
                   Status.Timestamp,
                   Context)

        { }

        #endregion

        #region ChargingStationAdminStatus(Id, Status, Timestamp, Context = null)

        /// <summary>
        /// Create a new charging station admin status.
        /// </summary>
        /// <param name="Id">The unique identification of the charging station.</param>
        /// <param name="Status">The current admin status of the charging station.</param>
        /// <param name="Timestamp">The timestamp of the admin status of the charging station.</param>
        /// <param name="Context">An optional data source or context for the charging station admin status.</param>
        public ChargingStationAdminStatus(ChargingStation_Id              Id,
                                          ChargingStationAdminStatusType  Status,
                                          DateTimeOffset                  Timestamp,
                                          Context?                        Context   = null)
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


        #region Operator overloading

        #region Operator == (ChargingStationAdminStatus1, ChargingStationAdminStatus2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingStationAdminStatus1">A charging station admin status.</param>
        /// <param name="ChargingStationAdminStatus2">Another charging station admin status.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator == (ChargingStationAdminStatus ChargingStationAdminStatus1,
                                           ChargingStationAdminStatus ChargingStationAdminStatus2)

            => ChargingStationAdminStatus1.Equals(ChargingStationAdminStatus2);

        #endregion

        #region Operator != (ChargingStationAdminStatus1, ChargingStationAdminStatus2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingStationAdminStatus1">A charging station admin status.</param>
        /// <param name="ChargingStationAdminStatus2">Another charging station admin status.</param>
        /// <returns>False if both match; True otherwise.</returns>
        public static Boolean operator != (ChargingStationAdminStatus ChargingStationAdminStatus1,
                                           ChargingStationAdminStatus ChargingStationAdminStatus2)

            => !ChargingStationAdminStatus1.Equals(ChargingStationAdminStatus2);

        #endregion

        #region Operator <  (ChargingStationAdminStatus1, ChargingStationAdminStatus2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingStationAdminStatus1">A charging station admin status.</param>
        /// <param name="ChargingStationAdminStatus2">Another charging station admin status.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator < (ChargingStationAdminStatus ChargingStationAdminStatus1,
                                          ChargingStationAdminStatus ChargingStationAdminStatus2)

            => ChargingStationAdminStatus1.CompareTo(ChargingStationAdminStatus2) < 0;

        #endregion

        #region Operator <= (ChargingStationAdminStatus1, ChargingStationAdminStatus2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingStationAdminStatus1">A charging station admin status.</param>
        /// <param name="ChargingStationAdminStatus2">Another charging station admin status.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator <= (ChargingStationAdminStatus ChargingStationAdminStatus1,
                                           ChargingStationAdminStatus ChargingStationAdminStatus2)

            => ChargingStationAdminStatus1.CompareTo(ChargingStationAdminStatus2) <= 0;

        #endregion

        #region Operator >  (ChargingStationAdminStatus1, ChargingStationAdminStatus2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingStationAdminStatus1">A charging station admin status.</param>
        /// <param name="ChargingStationAdminStatus2">Another charging station admin status.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator > (ChargingStationAdminStatus ChargingStationAdminStatus1,
                                          ChargingStationAdminStatus ChargingStationAdminStatus2)

            => ChargingStationAdminStatus1.CompareTo(ChargingStationAdminStatus2) > 0;

        #endregion

        #region Operator >= (ChargingStationAdminStatus1, ChargingStationAdminStatus2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingStationAdminStatus1">A charging station admin status.</param>
        /// <param name="ChargingStationAdminStatus2">Another charging station admin status.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator >= (ChargingStationAdminStatus ChargingStationAdminStatus1,
                                           ChargingStationAdminStatus ChargingStationAdminStatus2)

            => ChargingStationAdminStatus1.CompareTo(ChargingStationAdminStatus2) >= 0;

        #endregion

        #endregion

        #region IComparable<ChargingStationAdminStatus> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two charging station admin status.
        /// </summary>
        /// <param name="Object">A charging station admin status to compare with.</param>
        public Int32 CompareTo(Object? Object)

            => Object is ChargingStationAdminStatus chargingStationAdminStatus
                   ? CompareTo(chargingStationAdminStatus)
                   : throw new ArgumentException("The given object is not a charging station admin status!",
                                                 nameof(Object));

        #endregion

        #region CompareTo(ChargingStationAdminStatus)

        /// <summary>
        /// Compares two charging station admin status.
        /// </summary>
        /// <param name="ChargingStationAdminStatus">A charging station admin status to compare with.</param>
        public Int32 CompareTo(ChargingStationAdminStatus ChargingStationAdminStatus)
        {

            var c = Id.                   CompareTo(ChargingStationAdminStatus.Id);

            if (c == 0)
                c = Status.               CompareTo(ChargingStationAdminStatus.Status);

            if (c == 0)
                c = Timestamp.ToISO8601().CompareTo(ChargingStationAdminStatus.Timestamp.ToISO8601());

            if (c == 0 && Context is not null && ChargingStationAdminStatus.Context is not null)
                c = Context.              CompareTo(ChargingStationAdminStatus.Context);

            return c;

        }

        #endregion

        #endregion

        #region IEquatable<ChargingStationAdminStatus> Members

        #region Equals(Object)

        /// <summary>
        /// Compares two charging station admin status for equality.
        /// </summary>
        /// <param name="Object">A charging station admin status to compare with.</param>
        public override Boolean Equals(Object? Object)

            => Object is ChargingStationAdminStatus chargingStationAdminStatus &&
                   Equals(chargingStationAdminStatus);

        #endregion

        #region Equals(ChargingStationAdminStatus)

        /// <summary>
        /// Compares two charging station admin status for equality.
        /// </summary>
        /// <param name="ChargingStationAdminStatus">A charging station admin status to compare with.</param>
        public Boolean Equals(ChargingStationAdminStatus ChargingStationAdminStatus)

            => Id.                   Equals(ChargingStationAdminStatus.Id)                    &&
               Status.               Equals(ChargingStationAdminStatus.Status)                &&
               Timestamp.ToISO8601().Equals(ChargingStationAdminStatus.Timestamp.ToISO8601()) &&

             ((Context is null     && ChargingStationAdminStatus.Context is null) ||
              (Context is not null && ChargingStationAdminStatus.Context is not null && Context.Equals(ChargingStationAdminStatus.Context)));

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
