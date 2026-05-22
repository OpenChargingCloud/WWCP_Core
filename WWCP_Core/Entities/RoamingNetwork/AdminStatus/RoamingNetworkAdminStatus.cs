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
    /// Extension methods for the roaming network admin status.
    /// </summary>
    public static class RoamingNetworkAdminStatusExtensions
    {

        public static IEnumerable<IStatus<RoamingNetwork_Id, RoamingNetworkAdminStatusType>> ToStatusList(this RoamingNetworkAdminStatus[] StatusList)
            => StatusList.Select(status => status as IStatus<RoamingNetwork_Id, RoamingNetworkAdminStatusType>);

        public static JObject ToJSON(this RoamingNetworkAdminStatus[] StatusList)
            => StatusList.ToStatusList().ToJSON();

        public static JObject ToJSON(this IEnumerable<RoamingNetworkAdminStatus>  StatusList,
                                     UInt64?                                      Skip   = null,
                                     UInt64?                                      Take   = null)

            => StatusList.ToJSON(
                   Skip,
                   Take
               );


    }


    /// <summary>
    /// The current admin status of a roaming network.
    /// </summary>
    public readonly struct RoamingNetworkAdminStatus : IStatus<RoamingNetwork_Id, RoamingNetworkAdminStatusType>,
                                                       IEquatable<RoamingNetworkAdminStatus>,
                                                       IComparable<RoamingNetworkAdminStatus>,
                                                       IComparable
    {

        #region Properties

        /// <summary>
        /// The unique identification of the roaming network.
        /// </summary>
        public RoamingNetwork_Id              Id           { get; }

        /// <summary>
        /// The current admin status of the roaming network.
        /// </summary>
        public RoamingNetworkAdminStatusType  Status       { get; }

        /// <summary>
        /// The timestamp of the current admin status of the roaming network.
        /// </summary>
        public DateTimeOffset                 Timestamp    { get; }

        /// <summary>
        /// An optional data source or context for this roaming network admin status.
        /// </summary>
        public Context?                       Context      { get; }

        #endregion

        #region Constructor(s)

        #region RoamingNetworkAdminStatus(Id, Status,            Context = null)

        /// <summary>
        /// Create a new roaming network admin status.
        /// </summary>
        /// <param name="Id">The unique identification of the roaming network.</param>
        /// <param name="Status">The current timestamped admin status of the roaming network.</param>
        /// <param name="Context">An optional data source or context for the roaming network admin status.</param>
        public RoamingNetworkAdminStatus(RoamingNetwork_Id                           Id,
                                         Timestamped<RoamingNetworkAdminStatusType>  Status,
                                         Context?                                    Context   = null)

            : this(Id,
                   Status.Value,
                   Status.Timestamp,
                   Context)

        { }

        #endregion

        #region RoamingNetworkAdminStatus(Id, Status, Timestamp, Context = null)

        /// <summary>
        /// Create a new roaming network admin status.
        /// </summary>
        /// <param name="Id">The unique identification of the roaming network.</param>
        /// <param name="Status">The current admin status of the roaming network.</param>
        /// <param name="Timestamp">The timestamp of the admin status of the roaming network.</param>
        /// <param name="Context">An optional data source or context for the roaming network admin status.</param>
        public RoamingNetworkAdminStatus(RoamingNetwork_Id              Id,
                                         RoamingNetworkAdminStatusType  Status,
                                         DateTimeOffset                 Timestamp,
                                         Context?                       Context   = null)
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

        #region Operator == (RoamingNetworkAdminStatus1, RoamingNetworkAdminStatus2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="RoamingNetworkAdminStatus1">A roaming network admin status.</param>
        /// <param name="RoamingNetworkAdminStatus2">Another roaming network admin status.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator == (RoamingNetworkAdminStatus RoamingNetworkAdminStatus1,
                                           RoamingNetworkAdminStatus RoamingNetworkAdminStatus2)

            => RoamingNetworkAdminStatus1.Equals(RoamingNetworkAdminStatus2);

        #endregion

        #region Operator != (RoamingNetworkAdminStatus1, RoamingNetworkAdminStatus2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="RoamingNetworkAdminStatus1">A roaming network admin status.</param>
        /// <param name="RoamingNetworkAdminStatus2">Another roaming network admin status.</param>
        /// <returns>False if both match; True otherwise.</returns>
        public static Boolean operator != (RoamingNetworkAdminStatus RoamingNetworkAdminStatus1,
                                           RoamingNetworkAdminStatus RoamingNetworkAdminStatus2)

            => !RoamingNetworkAdminStatus1.Equals(RoamingNetworkAdminStatus2);

        #endregion

        #region Operator <  (RoamingNetworkAdminStatus1, RoamingNetworkAdminStatus2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="RoamingNetworkAdminStatus1">A roaming network admin status.</param>
        /// <param name="RoamingNetworkAdminStatus2">Another roaming network admin status.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator < (RoamingNetworkAdminStatus RoamingNetworkAdminStatus1,
                                          RoamingNetworkAdminStatus RoamingNetworkAdminStatus2)

            => RoamingNetworkAdminStatus1.CompareTo(RoamingNetworkAdminStatus2) < 0;

        #endregion

        #region Operator <= (RoamingNetworkAdminStatus1, RoamingNetworkAdminStatus2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="RoamingNetworkAdminStatus1">A roaming network admin status.</param>
        /// <param name="RoamingNetworkAdminStatus2">Another roaming network admin status.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator <= (RoamingNetworkAdminStatus RoamingNetworkAdminStatus1,
                                           RoamingNetworkAdminStatus RoamingNetworkAdminStatus2)

            => RoamingNetworkAdminStatus1.CompareTo(RoamingNetworkAdminStatus2) <= 0;

        #endregion

        #region Operator >  (RoamingNetworkAdminStatus1, RoamingNetworkAdminStatus2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="RoamingNetworkAdminStatus1">A roaming network admin status.</param>
        /// <param name="RoamingNetworkAdminStatus2">Another roaming network admin status.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator > (RoamingNetworkAdminStatus RoamingNetworkAdminStatus1,
                                          RoamingNetworkAdminStatus RoamingNetworkAdminStatus2)

            => RoamingNetworkAdminStatus1.CompareTo(RoamingNetworkAdminStatus2) > 0;

        #endregion

        #region Operator >= (RoamingNetworkAdminStatus1, RoamingNetworkAdminStatus2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="RoamingNetworkAdminStatus1">A roaming network admin status.</param>
        /// <param name="RoamingNetworkAdminStatus2">Another roaming network admin status.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator >= (RoamingNetworkAdminStatus RoamingNetworkAdminStatus1,
                                           RoamingNetworkAdminStatus RoamingNetworkAdminStatus2)

            => RoamingNetworkAdminStatus1.CompareTo(RoamingNetworkAdminStatus2) >= 0;

        #endregion

        #endregion

        #region IComparable<RoamingNetworkAdminStatus> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two roaming network admin status.
        /// </summary>
        /// <param name="Object">A roaming network admin status to compare with.</param>
        public Int32 CompareTo(Object? Object)

            => Object is RoamingNetworkAdminStatus roamingNetworkAdminStatus
                   ? CompareTo(roamingNetworkAdminStatus)
                   : throw new ArgumentException("The given object is not a roaming network admin status!",
                                                 nameof(Object));

        #endregion

        #region CompareTo(RoamingNetworkAdminStatus)

        /// <summary>
        /// Compares two roaming network admin status.
        /// </summary>
        /// <param name="RoamingNetworkAdminStatus">A roaming network admin status to compare with.</param>
        public Int32 CompareTo(RoamingNetworkAdminStatus RoamingNetworkAdminStatus)
        {

            var c = Id.                   CompareTo(RoamingNetworkAdminStatus.Id);

            if (c == 0)
                c = Status.               CompareTo(RoamingNetworkAdminStatus.Status);

            if (c == 0)
                c = Timestamp.ToISO8601().CompareTo(RoamingNetworkAdminStatus.Timestamp.ToISO8601());

            if (c == 0 && Context is not null && RoamingNetworkAdminStatus.Context is not null)
                c = Context.              CompareTo(RoamingNetworkAdminStatus.Context);

            return c;

        }

        #endregion

        #endregion

        #region IEquatable<RoamingNetworkAdminStatus> Members

        #region Equals(Object)

        /// <summary>
        /// Compares two roaming network admin status for equality.
        /// </summary>
        /// <param name="Object">A roaming network admin status to compare with.</param>
        public override Boolean Equals(Object? Object)

            => Object is RoamingNetworkAdminStatus roamingNetworkAdminStatus &&
                   Equals(roamingNetworkAdminStatus);

        #endregion

        #region Equals(RoamingNetworkAdminStatus)

        /// <summary>
        /// Compares two roaming network admin status for equality.
        /// </summary>
        /// <param name="RoamingNetworkAdminStatus">A roaming network admin status to compare with.</param>
        public Boolean Equals(RoamingNetworkAdminStatus RoamingNetworkAdminStatus)

            => Id.                   Equals(RoamingNetworkAdminStatus.Id)                    &&
               Status.               Equals(RoamingNetworkAdminStatus.Status)                &&
               Timestamp.ToISO8601().Equals(RoamingNetworkAdminStatus.Timestamp.ToISO8601()) &&

             ((Context is null     && RoamingNetworkAdminStatus.Context is null) ||
              (Context is not null && RoamingNetworkAdminStatus.Context is not null && Context.Equals(RoamingNetworkAdminStatus.Context)));

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
