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
    /// Extension methods for the charging pool admin status.
    /// </summary>
    public static class ChargingPoolAdminStatusExtensions
    {

        public static IEnumerable<IStatus<ChargingPool_Id, ChargingPoolAdminStatusType>> ToStatusList(this ChargingPoolAdminStatus[] StatusList)
            => StatusList.Select(status => status as IStatus<ChargingPool_Id, ChargingPoolAdminStatusType>);

        public static JObject ToJSON(this ChargingPoolAdminStatus[] StatusList,
                                     UInt64?                        Skip   = null,
                                     UInt64?                        Take   = null)

            => StatusList.ToStatusList().ToJSON(Skip, Take);


        public static JObject ToJSON(this IEnumerable<ChargingPoolAdminStatus>  StatusList,
                                     UInt64?                                    Skip   = null,
                                     UInt64?                                    Take   = null)

            => StatusList.ToJSON(
                   Skip,
                   Take
               );


    }


    /// <summary>
    /// The current admin status of a charging pool.
    /// </summary>
    public readonly struct ChargingPoolAdminStatus : IStatus<ChargingPool_Id, ChargingPoolAdminStatusType>,
                                                     IEquatable<ChargingPoolAdminStatus>,
                                                     IComparable<ChargingPoolAdminStatus>,
                                                     IComparable
    {

        #region Properties

        /// <summary>
        /// The unique identification of the charging pool.
        /// </summary>
        public ChargingPool_Id              Id           { get; }

        /// <summary>
        /// The current admin status of the charging pool.
        /// </summary>
        public ChargingPoolAdminStatusType  Status       { get; }

        /// <summary>
        /// The timestamp of the current admin status of the charging pool.
        /// </summary>
        public DateTimeOffset               Timestamp    { get; }

        /// <summary>
        /// An optional data source or context for this charging pool admin status.
        /// </summary>
        public Context?                     Context      { get; }

        #endregion

        #region Constructor(s)

        #region ChargingPoolAdminStatus(Id, Status,            Context = null)

        /// <summary>
        /// Create a new charging pool admin status.
        /// </summary>
        /// <param name="Id">The unique identification of the charging pool.</param>
        /// <param name="Status">The current timestamped admin status of the charging pool.</param>
        /// <param name="Context">An optional data source or context for the charging pool admin status.</param>
        public ChargingPoolAdminStatus(ChargingPool_Id                           Id,
                                       Timestamped<ChargingPoolAdminStatusType>  Status,
                                       Context?                                  Context   = null)

            : this(Id,
                   Status.Value,
                   Status.Timestamp,
                   Context)

        { }

        #endregion

        #region ChargingPoolAdminStatus(Id, Status, Timestamp, Context = null)

        /// <summary>
        /// Create a new charging pool admin status.
        /// </summary>
        /// <param name="Id">The unique identification of the charging pool.</param>
        /// <param name="Status">The current admin status of the charging pool.</param>
        /// <param name="Timestamp">The timestamp of the admin status of the charging pool.</param>
        /// <param name="Context">An optional data source or context for the charging pool admin status.</param>
        public ChargingPoolAdminStatus(ChargingPool_Id              Id,
                                       ChargingPoolAdminStatusType  Status,
                                       DateTimeOffset               Timestamp,
                                       Context?                     Context   = null)
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

        #region Operator == (ChargingPoolAdminStatus1, ChargingPoolAdminStatus2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingPoolAdminStatus1">A charging pool admin status.</param>
        /// <param name="ChargingPoolAdminStatus2">Another charging pool admin status.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator == (ChargingPoolAdminStatus ChargingPoolAdminStatus1,
                                           ChargingPoolAdminStatus ChargingPoolAdminStatus2)

            => ChargingPoolAdminStatus1.Equals(ChargingPoolAdminStatus2);

        #endregion

        #region Operator != (ChargingPoolAdminStatus1, ChargingPoolAdminStatus2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingPoolAdminStatus1">A charging pool admin status.</param>
        /// <param name="ChargingPoolAdminStatus2">Another charging pool admin status.</param>
        /// <returns>False if both match; True otherwise.</returns>
        public static Boolean operator != (ChargingPoolAdminStatus ChargingPoolAdminStatus1,
                                           ChargingPoolAdminStatus ChargingPoolAdminStatus2)

            => !ChargingPoolAdminStatus1.Equals(ChargingPoolAdminStatus2);

        #endregion

        #region Operator <  (ChargingPoolAdminStatus1, ChargingPoolAdminStatus2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingPoolAdminStatus1">A charging pool admin status.</param>
        /// <param name="ChargingPoolAdminStatus2">Another charging pool admin status.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator < (ChargingPoolAdminStatus ChargingPoolAdminStatus1,
                                          ChargingPoolAdminStatus ChargingPoolAdminStatus2)

            => ChargingPoolAdminStatus1.CompareTo(ChargingPoolAdminStatus2) < 0;

        #endregion

        #region Operator <= (ChargingPoolAdminStatus1, ChargingPoolAdminStatus2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingPoolAdminStatus1">A charging pool admin status.</param>
        /// <param name="ChargingPoolAdminStatus2">Another charging pool admin status.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator <= (ChargingPoolAdminStatus ChargingPoolAdminStatus1,
                                           ChargingPoolAdminStatus ChargingPoolAdminStatus2)

            => ChargingPoolAdminStatus1.CompareTo(ChargingPoolAdminStatus2) <= 0;

        #endregion

        #region Operator >  (ChargingPoolAdminStatus1, ChargingPoolAdminStatus2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingPoolAdminStatus1">A charging pool admin status.</param>
        /// <param name="ChargingPoolAdminStatus2">Another charging pool admin status.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator > (ChargingPoolAdminStatus ChargingPoolAdminStatus1,
                                          ChargingPoolAdminStatus ChargingPoolAdminStatus2)

            => ChargingPoolAdminStatus1.CompareTo(ChargingPoolAdminStatus2) > 0;

        #endregion

        #region Operator >= (ChargingPoolAdminStatus1, ChargingPoolAdminStatus2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingPoolAdminStatus1">A charging pool admin status.</param>
        /// <param name="ChargingPoolAdminStatus2">Another charging pool admin status.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator >= (ChargingPoolAdminStatus ChargingPoolAdminStatus1,
                                           ChargingPoolAdminStatus ChargingPoolAdminStatus2)

            => ChargingPoolAdminStatus1.CompareTo(ChargingPoolAdminStatus2) >= 0;

        #endregion

        #endregion

        #region IComparable<ChargingPoolAdminStatus> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two charging pool admin status.
        /// </summary>
        /// <param name="Object">A charging pool admin status to compare with.</param>
        public Int32 CompareTo(Object? Object)

            => Object is ChargingPoolAdminStatus chargingPoolAdminStatus
                   ? CompareTo(chargingPoolAdminStatus)
                   : throw new ArgumentException("The given object is not a charging pool admin status!",
                                                 nameof(Object));

        #endregion

        #region CompareTo(ChargingPoolAdminStatus)

        /// <summary>
        /// Compares two charging pool admin status.
        /// </summary>
        /// <param name="ChargingPoolAdminStatus">A charging pool admin status to compare with.</param>
        public Int32 CompareTo(ChargingPoolAdminStatus ChargingPoolAdminStatus)
        {

            var c = Id.                   CompareTo(ChargingPoolAdminStatus.Id);

            if (c == 0)
                c = Status.               CompareTo(ChargingPoolAdminStatus.Status);

            if (c == 0)
                c = Timestamp.ToISO8601().CompareTo(ChargingPoolAdminStatus.Timestamp.ToISO8601());

            if (c == 0 && Context is not null && ChargingPoolAdminStatus.Context is not null)
                c = Context.              CompareTo(ChargingPoolAdminStatus.Context);

            return c;

        }

        #endregion

        #endregion

        #region IEquatable<ChargingPoolAdminStatus> Members

        #region Equals(Object)

        /// <summary>
        /// Compares two charging pool admin status for equality.
        /// </summary>
        /// <param name="Object">A charging pool admin status to compare with.</param>
        public override Boolean Equals(Object? Object)

            => Object is ChargingPoolAdminStatus chargingPoolAdminStatus &&
                   Equals(chargingPoolAdminStatus);

        #endregion

        #region Equals(ChargingPoolAdminStatus)

        /// <summary>
        /// Compares two charging pool admin status for equality.
        /// </summary>
        /// <param name="ChargingPoolAdminStatus">A charging pool admin status to compare with.</param>
        public Boolean Equals(ChargingPoolAdminStatus ChargingPoolAdminStatus)

            => Id.                   Equals(ChargingPoolAdminStatus.Id)                    &&
               Status.               Equals(ChargingPoolAdminStatus.Status)                &&
               Timestamp.ToISO8601().Equals(ChargingPoolAdminStatus.Timestamp.ToISO8601()) &&

             ((Context is null     && ChargingPoolAdminStatus.Context is null) ||
              (Context is not null && ChargingPoolAdminStatus.Context is not null && Context.Equals(ChargingPoolAdminStatus.Context)));

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
