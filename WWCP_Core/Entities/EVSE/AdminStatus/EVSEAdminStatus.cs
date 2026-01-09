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
    /// Extension methods for the EVSE admin status.
    /// </summary>
    public static class EVSEAdminStatusExtensions
    {

        #region ToJSON(this EVSEAdminStatus, Skip = null, Take = null)

        public static JObject ToJSON(this IEnumerable<EVSEAdminStatus>  EVSEAdminStatus,
                                     UInt64?                            Skip   = null,
                                     UInt64?                            Take   = null)
        {

            #region Initial checks

            if (EVSEAdminStatus is null || !EVSEAdminStatus.Any())
                return new JObject();

            #endregion

            #region Maybe there are duplicate EVSE identifications in the enumeration... take the newest one!

            var filteredStatus = new Dictionary<EVSE_Id, EVSEAdminStatus>();

            foreach (var status in EVSEAdminStatus)
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

        #region Contains(this EVSEAdminStatus, Id, Status)

        /// <summary>
        /// Check if the given enumeration of EVSEs and their current admin status
        /// contains the given pair of EVSE identification and admin status.
        /// </summary>
        /// <param name="EVSEAdminStatus">An enumeration of EVSEs and their current admin status.</param>
        /// <param name="Id">An EVSE identification.</param>
        /// <param name="AdminStatus">An EVSE admin status.</param>
        public static Boolean Contains(this IEnumerable<EVSEAdminStatus>  EVSEAdminStatus,
                                       EVSE_Id                            Id,
                                       EVSEAdminStatusType               AdminStatus)
        {

            foreach (var status in EVSEAdminStatus)
            {
                if (status.Id     == Id &&
                    status.Status == AdminStatus)
                {
                    return true;
                }
            }

            return false;

        }

        #endregion

    }


    /// <summary>
    /// The current admin status of an EVSE.
    /// </summary>
    public readonly struct EVSEAdminStatus : IEquatable<EVSEAdminStatus>,
                                             IComparable<EVSEAdminStatus>,
                                             IComparable
    {

        #region Properties

        /// <summary>
        /// The unique identification of the EVSE.
        /// </summary>
        public EVSE_Id              Id           { get; }

        /// <summary>
        /// The current admin status of the EVSE.
        /// </summary>
        public EVSEAdminStatusType  Status       { get; }

        /// <summary>
        /// The timestamp of the current admin status of the EVSE.
        /// </summary>
        public DateTimeOffset       Timestamp    { get; }

        /// <summary>
        /// An optional data source or context for this EVSE admin status.
        /// </summary>
        public Context?             Context      { get; }

        /// <summary>
        /// The timestamped admin status of the EVSE.
        /// </summary>
        public Timestamped<EVSEAdminStatusType> TimestampedAdminStatus
            => new (Timestamp, Status);

        #endregion

        #region Constructor(s)

        #region EVSEAdminStatus(Id,            Status, Context = null)

        /// <summary>
        /// Create a new EVSE admin status.
        /// </summary>
        /// <param name="Id">The unique identification of the EVSE.</param>
        /// <param name="Status">The current timestamped admin status of the EVSE.</param>
        /// <param name="Context">An optional data source or context for the EVSE admin status.</param>
        public EVSEAdminStatus(EVSE_Id                           Id,
                               Timestamped<EVSEAdminStatusType>  Status,
                               Context?                          Context   = null)

            : this(Id,
                   Status.Value,
                   Status.Timestamp,
                   Context)

        { }

        #endregion

        #region EVSEAdminStatus(Id, Status, Timestamp, Context = null)

        /// <summary>
        /// Create a new EVSE admin status.
        /// </summary>
        /// <param name="Id">The unique identification of the EVSE.</param>
        /// <param name="Status">The current admin status of the EVSE.</param>
        /// <param name="Timestamp">The timestamp of the admin status of the EVSE.</param>
        /// <param name="Context">An optional data source or context for the EVSE admin status.</param>
        public EVSEAdminStatus(EVSE_Id              Id,
                               EVSEAdminStatusType  Status,
                               DateTimeOffset       Timestamp,
                               Context?             Context   = null)
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

        #region Operator == (EVSEAdminStatus1, EVSEAdminStatus2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EVSEAdminStatus1">An EVSE admin status.</param>
        /// <param name="EVSEAdminStatus2">Another EVSE admin status.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator == (EVSEAdminStatus EVSEAdminStatus1,
                                           EVSEAdminStatus EVSEAdminStatus2)

            => EVSEAdminStatus1.Equals(EVSEAdminStatus2);

        #endregion

        #region Operator != (EVSEAdminStatus1, EVSEAdminStatus2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EVSEAdminStatus1">An EVSE admin status.</param>
        /// <param name="EVSEAdminStatus2">Another EVSE admin status.</param>
        /// <returns>False if both match; True otherwise.</returns>
        public static Boolean operator != (EVSEAdminStatus EVSEAdminStatus1,
                                           EVSEAdminStatus EVSEAdminStatus2)

            => !EVSEAdminStatus1.Equals(EVSEAdminStatus2);

        #endregion

        #region Operator <  (EVSEAdminStatus1, EVSEAdminStatus2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EVSEAdminStatus1">An EVSE admin status.</param>
        /// <param name="EVSEAdminStatus2">Another EVSE admin status.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator < (EVSEAdminStatus EVSEAdminStatus1,
                                          EVSEAdminStatus EVSEAdminStatus2)

            => EVSEAdminStatus1.CompareTo(EVSEAdminStatus2) < 0;

        #endregion

        #region Operator <= (EVSEAdminStatus1, EVSEAdminStatus2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EVSEAdminStatus1">An EVSE admin status.</param>
        /// <param name="EVSEAdminStatus2">Another EVSE admin status.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator <= (EVSEAdminStatus EVSEAdminStatus1,
                                           EVSEAdminStatus EVSEAdminStatus2)

            => EVSEAdminStatus1.CompareTo(EVSEAdminStatus2) <= 0;

        #endregion

        #region Operator >  (EVSEAdminStatus1, EVSEAdminStatus2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EVSEAdminStatus1">An EVSE admin status.</param>
        /// <param name="EVSEAdminStatus2">Another EVSE admin status.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator > (EVSEAdminStatus EVSEAdminStatus1,
                                          EVSEAdminStatus EVSEAdminStatus2)

            => EVSEAdminStatus1.CompareTo(EVSEAdminStatus2) > 0;

        #endregion

        #region Operator >= (EVSEAdminStatus1, EVSEAdminStatus2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EVSEAdminStatus1">An EVSE admin status.</param>
        /// <param name="EVSEAdminStatus2">Another EVSE admin status.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator >= (EVSEAdminStatus EVSEAdminStatus1,
                                           EVSEAdminStatus EVSEAdminStatus2)

            => EVSEAdminStatus1.CompareTo(EVSEAdminStatus2) >= 0;

        #endregion

        #endregion

        #region IComparable<EVSEAdminStatus> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two EVSE admin status.
        /// </summary>
        /// <param name="Object">An EVSE admin status to compare with.</param>
        public Int32 CompareTo(Object? Object)

            => Object is EVSEAdminStatus evseAdminStatus
                   ? CompareTo(evseAdminStatus)
                   : throw new ArgumentException("The given object is not an EVSE admin status!",
                                                 nameof(Object));

        #endregion

        #region CompareTo(EVSEAdminStatus)

        /// <summary>
        /// Compares two EVSE admin status.
        /// </summary>
        /// <param name="EVSEAdminStatus">An EVSE admin status to compare with.</param>
        public Int32 CompareTo(EVSEAdminStatus EVSEAdminStatus)
        {

            var c = Id.                   CompareTo(EVSEAdminStatus.Id);

            if (c == 0)
                c = Status.               CompareTo(EVSEAdminStatus.Status);

            if (c == 0)
                c = Timestamp.ToISO8601().CompareTo(EVSEAdminStatus.Timestamp.ToISO8601());

            if (c == 0 && Context is not null && EVSEAdminStatus.Context is not null)
                c = Context.              CompareTo(EVSEAdminStatus.Context);

            return c;

        }

        #endregion

        #endregion

        #region IEquatable<EVSEAdminStatus> Members

        #region Equals(Object)

        /// <summary>
        /// Compares two EVSE admin status for equality.
        /// </summary>
        /// <param name="Object">An EVSE admin status to compare with.</param>
        public override Boolean Equals(Object? Object)

            => Object is EVSEAdminStatus evseAdminStatus &&
                   Equals(evseAdminStatus);

        #endregion

        #region Equals(EVSEAdminStatus)

        /// <summary>
        /// Compares two EVSE admin status for equality.
        /// </summary>
        /// <param name="EVSEAdminStatus">An EVSE admin status to compare with.</param>
        public Boolean Equals(EVSEAdminStatus EVSEAdminStatus)

            => Id.                   Equals(EVSEAdminStatus.Id)                    &&
               Status.               Equals(EVSEAdminStatus.Status)                &&
               Timestamp.ToISO8601().Equals(EVSEAdminStatus.Timestamp.ToISO8601()) &&

             ((Context is null     && EVSEAdminStatus.Context is null) ||
              (Context is not null && EVSEAdminStatus.Context is not null && Context.Equals(EVSEAdminStatus.Context)));

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
