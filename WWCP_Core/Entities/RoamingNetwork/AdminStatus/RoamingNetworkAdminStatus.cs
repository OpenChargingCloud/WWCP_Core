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

        #region ToJSON(this RoamingNetworkAdminStatus, Skip = null, Take = null)

        public static JObject ToJSON(this IEnumerable<RoamingNetworkAdminStatus>  RoamingNetworkAdminStatus,
                                     UInt64?                                      Skip   = null,
                                     UInt64?                                      Take   = null)
        {

            #region Initial checks

            if (RoamingNetworkAdminStatus is null || !RoamingNetworkAdminStatus.Any())
                return new JObject();

            #endregion

            #region Maybe there are duplicate roaming network identifications in the enumeration... take the newest one!

            var filteredStatus = new Dictionary<RoamingNetwork_Id, RoamingNetworkAdminStatus>();

            foreach (var status in RoamingNetworkAdminStatus)
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
                                                                          kvp.Value.AdminStatus.ToString())
                                                              )));

        }

        #endregion

        #region Contains(this RoamingNetworkAdminStatus, Id, Status)

        /// <summary>
        /// Check if the given enumeration of roaming networks and their current admin status
        /// contains the given pair of roaming network identification and admin status.
        /// </summary>
        /// <param name="RoamingNetworkAdminStatus">An enumeration of roaming networks and their current admin status.</param>
        /// <param name="Id">A roaming network identification.</param>
        /// <param name="AdminStatus">A roaming network admin status.</param>
        public static Boolean Contains(this IEnumerable<RoamingNetworkAdminStatus>  RoamingNetworkAdminStatus,
                                       RoamingNetwork_Id                            Id,
                                       RoamingNetworkAdminStatusTypes               AdminStatus)
        {

            foreach (var status in RoamingNetworkAdminStatus)
            {
                if (status.Id          == Id &&
                    status.AdminStatus == AdminStatus)
                {
                    return true;
                }
            }

            return false;

        }

        #endregion

    }


    /// <summary>
    /// The current admin status of a roaming network.
    /// </summary>
    public class RoamingNetworkAdminStatus : AInternalData,
                                             IEquatable<RoamingNetworkAdminStatus>,
                                             IComparable<RoamingNetworkAdminStatus>
    {

        #region Properties

        /// <summary>
        /// The unique identification of the roaming network.
        /// </summary>
        public RoamingNetwork_Id               Id             { get; }

        /// <summary>
        /// The current admin status of the roaming network.
        /// </summary>
        public RoamingNetworkAdminStatusTypes  AdminStatus    { get; }

        /// <summary>
        /// The timestamp of the current admin status of the roaming network.
        /// </summary>
        public DateTimeOffset                  Timestamp      { get; }

        /// <summary>
        /// The timestamped admin status of the roaming network.
        /// </summary>
        public Timestamped<RoamingNetworkAdminStatusTypes> TimestampedAdminStatus
            => new (Timestamp, AdminStatus);

        #endregion

        #region Constructor(s)

        #region RoamingNetworkAdminStatus(Id, AdminStatus,            CustomData = null, InternalData = null)

        /// <summary>
        /// Create a new roaming network admin status.
        /// </summary>
        /// <param name="Id">The unique identification of the roaming network.</param>
        /// <param name="AdminStatus">The current timestamped adminstatus of the roaming network.</param>
        /// <param name="CustomData">An optional dictionary of customer-specific data.</param>
        public RoamingNetworkAdminStatus(RoamingNetwork_Id                            Id,
                                         Timestamped<RoamingNetworkAdminStatusTypes>  AdminStatus,
                                         JObject?                                     CustomData     = null,
                                         UserDefinedDictionary?                       InternalData   = null)

            : base(CustomData,
                   InternalData,
                   org.GraphDefined.Vanaheimr.Illias.Timestamp.Now)

        {

            this.Id           = Id;
            this.AdminStatus  = AdminStatus.Value;
            this.Timestamp    = AdminStatus.Timestamp;

        }

        #endregion

        #region RoamingNetworkAdminStatus(Id, AdminStatus, Timestamp, CustomData = null, InternalData = null)

        /// <summary>
        /// Create a new roaming network admin status.
        /// </summary>
        /// <param name="Id">The unique identification of the roaming network.</param>
        /// <param name="Status">The current admin status of the roaming network.</param>
        /// <param name="Timestamp">The timestamp of the status change of the roaming network.</param>
        /// <param name="CustomData">An optional dictionary of customer-specific data.</param>
        public RoamingNetworkAdminStatus(RoamingNetwork_Id               Id,
                                         RoamingNetworkAdminStatusTypes  AdminStatus,
                                         DateTime                        Timestamp,
                                         JObject?                        CustomData     = null,
                                         UserDefinedDictionary?          InternalData   = null)

            : base(CustomData,
                   InternalData,
                   org.GraphDefined.Vanaheimr.Illias.Timestamp.Now)

        {

            this.Id           = Id;
            this.AdminStatus  = AdminStatus;
            this.Timestamp    = Timestamp;

        }

        #endregion

        #endregion


        #region (static) Snapshot(RoamingNetwork)

        /// <summary>
        /// Take a snapshot of the current roaming network admin status.
        /// </summary>
        /// <param name="RoamingNetwork">A roaming network.</param>
        public static RoamingNetworkAdminStatus Snapshot(RoamingNetwork RoamingNetwork)

            => new (RoamingNetwork.Id,
                    RoamingNetwork.AdminStatus);

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
        {

            // If both are null, or both are same instance, return true.
            if (ReferenceEquals(RoamingNetworkAdminStatus1, RoamingNetworkAdminStatus2))
                return true;

            // If one is null, but not both, return false.
            if (RoamingNetworkAdminStatus1 is null || RoamingNetworkAdminStatus2 is null)
                return false;

            return RoamingNetworkAdminStatus1.Equals(RoamingNetworkAdminStatus2);

        }

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

            => !(RoamingNetworkAdminStatus1 == RoamingNetworkAdminStatus2);

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
        {

            if (RoamingNetworkAdminStatus1 is null)
                throw new ArgumentNullException(nameof(RoamingNetworkAdminStatus1), "The given RoamingNetworkAdminStatus1 must not be null!");

            return RoamingNetworkAdminStatus1.CompareTo(RoamingNetworkAdminStatus2) < 0;

        }

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

            => !(RoamingNetworkAdminStatus1 > RoamingNetworkAdminStatus2);

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
        {

            if (RoamingNetworkAdminStatus1 is null)
                throw new ArgumentNullException(nameof(RoamingNetworkAdminStatus1), "The given RoamingNetworkAdminStatus1 must not be null!");

            return RoamingNetworkAdminStatus1.CompareTo(RoamingNetworkAdminStatus2) > 0;

        }

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

            => !(RoamingNetworkAdminStatus1 < RoamingNetworkAdminStatus2);

        #endregion

        #endregion

        #region IComparable<RoamingNetworkAdminStatus> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        public Int32 CompareTo(Object? Object)

            => Object is RoamingNetworkAdminStatus chargingStationOperatorAdminStatus
                   ? CompareTo(chargingStationOperatorAdminStatus)
                   : throw new ArgumentException("The given object is not a roaming network admin status!",
                                                 nameof(Object));

        #endregion

        #region CompareTo(RoamingNetworkAdminStatus)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="RoamingNetworkAdminStatus">An object to compare with.</param>
        public Int32 CompareTo(RoamingNetworkAdminStatus? RoamingNetworkAdminStatus)
        {

            if (RoamingNetworkAdminStatus is null)
                throw new ArgumentNullException(nameof(RoamingNetworkAdminStatus), "The given roaming network admin status must not be null!");

            var c = Id.         CompareTo(RoamingNetworkAdminStatus.Id);

            if (c == 0)
                c = AdminStatus.CompareTo(RoamingNetworkAdminStatus.AdminStatus);

            if (c == 0)
                c = Timestamp.  CompareTo(RoamingNetworkAdminStatus.Timestamp);

            return c;

        }

        #endregion

        #endregion

        #region IEquatable<RoamingNetworkAdminStatus> Members

        #region Equals(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public override Boolean Equals(Object? Object)

            => Object is RoamingNetworkAdminStatus chargingStationOperatorAdminStatus &&
                   Equals(chargingStationOperatorAdminStatus);

        #endregion

        #region Equals(RoamingNetworkAdminStatus)

        /// <summary>
        /// Compares two RoamingNetwork identifications for equality.
        /// </summary>
        /// <param name="RoamingNetworkAdminStatus">A roaming network identification to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public Boolean Equals(RoamingNetworkAdminStatus? RoamingNetworkAdminStatus)

            => RoamingNetworkAdminStatus is not null                     &&
               Id.         Equals(RoamingNetworkAdminStatus.Id)          &&
               AdminStatus.Equals(RoamingNetworkAdminStatus.AdminStatus) &&
               Timestamp.  Equals(RoamingNetworkAdminStatus.Timestamp);

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

                return Id.         GetHashCode() * 5 ^
                       AdminStatus.GetHashCode() * 3 ^
                       Timestamp.  GetHashCode();

            }
        }

        #endregion

        #region (override) ToString()

        /// <summary>
        /// Return a text representation of this object.
        /// </summary>
        public override String ToString()

            => String.Concat(Id, " -> ",
                             AdminStatus,
                             " since ",
                             Timestamp.ToISO8601());

        #endregion

    }

}
