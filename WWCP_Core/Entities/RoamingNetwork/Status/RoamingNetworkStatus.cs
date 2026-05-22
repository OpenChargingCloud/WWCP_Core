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
    /// Extension methods for the roaming network status.
    /// </summary>
    public static class RoamingNetworkStatusExtensions
    {

        public static IEnumerable<IStatus<RoamingNetwork_Id, RoamingNetworkStatusType>> ToStatusList(this RoamingNetworkStatus[] StatusList)
            => StatusList.Select(status => status as IStatus<RoamingNetwork_Id, RoamingNetworkStatusType>);

        public static JObject ToJSON(this RoamingNetworkStatus[] StatusList)
            => StatusList.ToStatusList().ToJSON();

        public static JObject ToJSON(this IEnumerable<RoamingNetworkStatus>  StatusList,
                                     UInt64?                                 Skip   = null,
                                     UInt64?                                 Take   = null)

            => StatusList.ToJSON(
                   Skip,
                   Take
               );


    }


    /// <summary>
    /// The current status of a roaming network.
    /// </summary>
    public readonly struct RoamingNetworkStatus : IStatus<RoamingNetwork_Id, RoamingNetworkStatusType>,
                                                  IEquatable<RoamingNetworkStatus>,
                                                  IComparable<RoamingNetworkStatus>,
                                                  IComparable
    {

        #region Properties

        /// <summary>
        /// The unique identification of the roaming network.
        /// </summary>
        public RoamingNetwork_Id         Id           { get; }

        /// <summary>
        /// The current status of the roaming network.
        /// </summary>
        public RoamingNetworkStatusType  Status       { get; }

        /// <summary>
        /// The timestamp of the current status of the roaming network.
        /// </summary>
        public DateTimeOffset            Timestamp    { get; }

        /// <summary>
        /// An optional data source or context for this roaming network status.
        /// </summary>
        public Context?                  Context      { get; }

        #endregion

        #region Constructor(s)

        #region RoamingNetworkStatus(Id, Status,            Context = null)

        /// <summary>
        /// Create a new roaming network status.
        /// </summary>
        /// <param name="Id">The unique identification of the roaming network.</param>
        /// <param name="Status">The current timestamped status of the roaming network.</param>
        /// <param name="Context">An optional data source or context for the roaming network status.</param>
        public RoamingNetworkStatus(RoamingNetwork_Id                      Id,
                                    Timestamped<RoamingNetworkStatusType>  Status,
                                    Context?                               Context   = null)

            : this(Id,
                   Status.Value,
                   Status.Timestamp,
                   Context)

        { }

        #endregion

        #region RoamingNetworkStatus(Id, Status, Timestamp, Context = null)

        /// <summary>
        /// Create a new roaming network status.
        /// </summary>
        /// <param name="Id">The unique identification of the roaming network.</param>
        /// <param name="Status">The current status of the roaming network.</param>
        /// <param name="Timestamp">The timestamp of the status of the roaming network.</param>
        /// <param name="Context">An optional data source or context for the roaming network status.</param>
        public RoamingNetworkStatus(RoamingNetwork_Id         Id,
                                    RoamingNetworkStatusType  Status,
                                    DateTimeOffset            Timestamp,
                                    Context?                  Context   = null)
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

        #region Operator == (RoamingNetworkStatus1, RoamingNetworkStatus2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="RoamingNetworkStatus1">A roaming network status.</param>
        /// <param name="RoamingNetworkStatus2">Another roaming network status.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator == (RoamingNetworkStatus RoamingNetworkStatus1,
                                           RoamingNetworkStatus RoamingNetworkStatus2)

            => RoamingNetworkStatus1.Equals(RoamingNetworkStatus2);

        #endregion

        #region Operator != (RoamingNetworkStatus1, RoamingNetworkStatus2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="RoamingNetworkStatus1">A roaming network status.</param>
        /// <param name="RoamingNetworkStatus2">Another roaming network status.</param>
        /// <returns>False if both match; True otherwise.</returns>
        public static Boolean operator != (RoamingNetworkStatus RoamingNetworkStatus1,
                                           RoamingNetworkStatus RoamingNetworkStatus2)

            => !RoamingNetworkStatus1.Equals(RoamingNetworkStatus2);

        #endregion

        #region Operator <  (RoamingNetworkStatus1, RoamingNetworkStatus2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="RoamingNetworkStatus1">A roaming network status.</param>
        /// <param name="RoamingNetworkStatus2">Another roaming network status.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator < (RoamingNetworkStatus RoamingNetworkStatus1,
                                          RoamingNetworkStatus RoamingNetworkStatus2)

            => RoamingNetworkStatus1.CompareTo(RoamingNetworkStatus2) < 0;

        #endregion

        #region Operator <= (RoamingNetworkStatus1, RoamingNetworkStatus2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="RoamingNetworkStatus1">A roaming network status.</param>
        /// <param name="RoamingNetworkStatus2">Another roaming network status.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator <= (RoamingNetworkStatus RoamingNetworkStatus1,
                                           RoamingNetworkStatus RoamingNetworkStatus2)

            => RoamingNetworkStatus1.CompareTo(RoamingNetworkStatus2) <= 0;

        #endregion

        #region Operator >  (RoamingNetworkStatus1, RoamingNetworkStatus2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="RoamingNetworkStatus1">A roaming network status.</param>
        /// <param name="RoamingNetworkStatus2">Another roaming network status.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator > (RoamingNetworkStatus RoamingNetworkStatus1,
                                          RoamingNetworkStatus RoamingNetworkStatus2)

            => RoamingNetworkStatus1.CompareTo(RoamingNetworkStatus2) > 0;

        #endregion

        #region Operator >= (RoamingNetworkStatus1, RoamingNetworkStatus2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="RoamingNetworkStatus1">A roaming network status.</param>
        /// <param name="RoamingNetworkStatus2">Another roaming network status.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator >= (RoamingNetworkStatus RoamingNetworkStatus1,
                                           RoamingNetworkStatus RoamingNetworkStatus2)

            => RoamingNetworkStatus1.CompareTo(RoamingNetworkStatus2) >= 0;

        #endregion

        #endregion

        #region IComparable<RoamingNetworkStatus> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two roaming network status.
        /// </summary>
        /// <param name="Object">A roaming network status to compare with.</param>
        public Int32 CompareTo(Object? Object)

            => Object is RoamingNetworkStatus roamingNetworkStatus
                   ? CompareTo(roamingNetworkStatus)
                   : throw new ArgumentException("The given object is not a roaming network status!",
                                                 nameof(Object));

        #endregion

        #region CompareTo(RoamingNetworkStatus)

        /// <summary>
        /// Compares two roaming network status.
        /// </summary>
        /// <param name="RoamingNetworkStatus">A roaming network status to compare with.</param>
        public Int32 CompareTo(RoamingNetworkStatus RoamingNetworkStatus)
        {

            var c = Id.                   CompareTo(RoamingNetworkStatus.Id);

            if (c == 0)
                c = Status.               CompareTo(RoamingNetworkStatus.Status);

            if (c == 0)
                c = Timestamp.ToISO8601().CompareTo(RoamingNetworkStatus.Timestamp.ToISO8601());

            if (c == 0 && Context is not null && RoamingNetworkStatus.Context is not null)
                c = Context.              CompareTo(RoamingNetworkStatus.Context);

            return c;

        }

        #endregion

        #endregion

        #region IEquatable<RoamingNetworkStatus> Members

        #region Equals(Object)

        /// <summary>
        /// Compares two roaming network status for equality.
        /// </summary>
        /// <param name="Object">A roaming network status to compare with.</param>
        public override Boolean Equals(Object? Object)

            => Object is RoamingNetworkStatus roamingNetworkStatus &&
                   Equals(roamingNetworkStatus);

        #endregion

        #region Equals(RoamingNetworkStatus)

        /// <summary>
        /// Compares two roaming network status for equality.
        /// </summary>
        /// <param name="RoamingNetworkStatus">A roaming network status to compare with.</param>
        public Boolean Equals(RoamingNetworkStatus RoamingNetworkStatus)

            => Id.                   Equals(RoamingNetworkStatus.Id)                    &&
               Status.               Equals(RoamingNetworkStatus.Status)                &&
               Timestamp.ToISO8601().Equals(RoamingNetworkStatus.Timestamp.ToISO8601()) &&

             ((Context is null     && RoamingNetworkStatus.Context is null) ||
              (Context is not null && RoamingNetworkStatus.Context is not null && Context.Equals(RoamingNetworkStatus.Context)));

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
