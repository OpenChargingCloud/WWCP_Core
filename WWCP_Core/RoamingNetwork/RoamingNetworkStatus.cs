/*
 * Copyright (c) 2014-2022 GraphDefined GmbH <achim.friedland@graphdefined.com>
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

        #region ToJSON(this RoamingNetworkStatus, Skip = null, Take = null)

        public static JObject ToJSON(this IEnumerable<RoamingNetworkStatus>  RoamingNetworkStatus,
                                     UInt64?                                 Skip   = null,
                                     UInt64?                                 Take   = null)
        {

            #region Initial checks

            if (RoamingNetworkStatus is null || !RoamingNetworkStatus.Any())
                return new JObject();

            #endregion

            #region Maybe there are duplicate roaming network identifications in the enumeration... take the newest one!

            var filteredStatus = new Dictionary<RoamingNetwork_Id, RoamingNetworkStatus>();

            foreach (var status in RoamingNetworkStatus)
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
                                                               new JArray(kvp.Value.Timestamp.  ToIso8601(),
                                                                          kvp.Value.Status.ToString())
                                                              )));

        }

        #endregion

        #region Contains(this RoamingNetworkStatus, Id, Status)

        /// <summary>
        /// Check if the given enumeration of roaming networks and their current status
        /// contains the given pair of roaming network identification and status.
        /// </summary>
        /// <param name="RoamingNetworkStatus">An enumeration of roaming networks and their current status.</param>
        /// <param name="Id">A roaming network identification.</param>
        /// <param name="Status">A roaming network status.</param>
        public static Boolean Contains(this IEnumerable<RoamingNetworkStatus>  RoamingNetworkStatus,
                                       RoamingNetwork_Id                       Id,
                                       RoamingNetworkStatusTypes               Status)
        {

            foreach (var status in RoamingNetworkStatus)
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
    /// The current status of a roaming network.
    /// </summary>
    public class RoamingNetworkStatus : AInternalData,
                                                 IEquatable <RoamingNetworkStatus>,
                                                 IComparable<RoamingNetworkStatus>
    {

        #region Properties

        /// <summary>
        /// The unique identification of the roaming network.
        /// </summary>
        public RoamingNetwork_Id          Id           { get; }

        /// <summary>
        /// The current status of the roaming network.
        /// </summary>
        public RoamingNetworkStatusTypes  Status       { get; }

        /// <summary>
        /// The timestamp of the current status of the roaming network.
        /// </summary>
        public DateTime                   Timestamp    { get; }

        /// <summary>
        /// The timestamped status of the roaming network.
        /// </summary>
        public Timestamped<RoamingNetworkStatusTypes> TimestampedStatus
            => new (Timestamp, Status);

        #endregion

        #region Constructor(s)

        #region RoamingNetworkStatus(Id, Status,            CustomData = null, InternalData = null)

        /// <summary>
        /// Create a new roaming network status.
        /// </summary>
        /// <param name="Id">The unique identification of the roaming network.</param>
        /// <param name="Status">The current timestamped status of the roaming network.</param>
        /// <param name="CustomData">An optional dictionary of customer-specific data.</param>
        public RoamingNetworkStatus(RoamingNetwork_Id                       Id,
                                    Timestamped<RoamingNetworkStatusTypes>  Status,
                                    JObject?                                CustomData     = null,
                                    UserDefinedDictionary?                  InternalData   = null)

            : base(CustomData,
                   InternalData)

        {

            this.Id         = Id;
            this.Status     = Status.Value;
            this.Timestamp  = Status.Timestamp;

        }

        #endregion

        #region RoamingNetworkStatus(Id, Status, Timestamp, CustomData = null, InternalData = null)

        /// <summary>
        /// Create a new roaming network status.
        /// </summary>
        /// <param name="Id">The unique identification of the roaming network.</param>
        /// <param name="Status">The current status of the roaming network.</param>
        /// <param name="Timestamp">The timestamp of the status change of the roaming network.</param>
        /// <param name="CustomData">An optional dictionary of customer-specific data.</param>
        public RoamingNetworkStatus(RoamingNetwork_Id          Id,
                                    RoamingNetworkStatusTypes  Status,
                                    DateTime                   Timestamp,
                                    JObject?                   CustomData     = null,
                                    UserDefinedDictionary?     InternalData   = null)

            : base(CustomData,
                   InternalData)

        {

            this.Id         = Id;
            this.Status     = Status;
            this.Timestamp  = Timestamp;

        }

        #endregion

        #endregion


        #region (static) Snapshot(RoamingNetwork)

        /// <summary>
        /// Take a snapshot of the current roaming network status.
        /// </summary>
        /// <param name="RoamingNetwork">A roaming network.</param>
        public static RoamingNetworkStatus Snapshot(RoamingNetwork RoamingNetwork)

            => new (RoamingNetwork.Id,
                    RoamingNetwork.Status);

        #endregion


        #region Operator overloading

        #region Operator == (RoamingNetworkStatus1, RoamingNetworkStatus2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="RoamingNetworkStatus1">A roaming network status.</param>
        /// <param name="RoamingNetworkStatus2">Another roaming network status.</param>
        /// <returns>true|false</returns>
        public static Boolean operator == (RoamingNetworkStatus RoamingNetworkStatus1,
                                           RoamingNetworkStatus RoamingNetworkStatus2)
        {

            // If both are null, or both are same instance, return true.
            if (ReferenceEquals(RoamingNetworkStatus1, RoamingNetworkStatus2))
                return true;

            // If one is null, but not both, return false.
            if (RoamingNetworkStatus1 is null || RoamingNetworkStatus2 is null)
                return false;

            return RoamingNetworkStatus1.Equals(RoamingNetworkStatus2);

        }

        #endregion

        #region Operator != (RoamingNetworkStatus1, RoamingNetworkStatus2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="RoamingNetworkStatus1">A roaming network status.</param>
        /// <param name="RoamingNetworkStatus2">Another roaming network status.</param>
        /// <returns>true|false</returns>
        public static Boolean operator != (RoamingNetworkStatus RoamingNetworkStatus1,
                                           RoamingNetworkStatus RoamingNetworkStatus2)

            => !(RoamingNetworkStatus1 == RoamingNetworkStatus2);

        #endregion

        #region Operator <  (RoamingNetworkStatus1, RoamingNetworkStatus2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="RoamingNetworkStatus1">A roaming network status.</param>
        /// <param name="RoamingNetworkStatus2">Another roaming network status.</param>
        /// <returns>true|false</returns>
        public static Boolean operator < (RoamingNetworkStatus RoamingNetworkStatus1,
                                          RoamingNetworkStatus RoamingNetworkStatus2)
        {

            if (RoamingNetworkStatus1 is null)
                throw new ArgumentNullException(nameof(RoamingNetworkStatus1), "The given RoamingNetworkStatus1 must not be null!");

            return RoamingNetworkStatus1.CompareTo(RoamingNetworkStatus2) < 0;

        }

        #endregion

        #region Operator <= (RoamingNetworkStatus1, RoamingNetworkStatus2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="RoamingNetworkStatus1">A roaming network status.</param>
        /// <param name="RoamingNetworkStatus2">Another roaming network status.</param>
        /// <returns>true|false</returns>
        public static Boolean operator <= (RoamingNetworkStatus RoamingNetworkStatus1,
                                           RoamingNetworkStatus RoamingNetworkStatus2)

            => !(RoamingNetworkStatus1 > RoamingNetworkStatus2);

        #endregion

        #region Operator >  (RoamingNetworkStatus1, RoamingNetworkStatus2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="RoamingNetworkStatus1">A roaming network status.</param>
        /// <param name="RoamingNetworkStatus2">Another roaming network status.</param>
        /// <returns>true|false</returns>
        public static Boolean operator > (RoamingNetworkStatus RoamingNetworkStatus1,
                                          RoamingNetworkStatus RoamingNetworkStatus2)
        {

            if (RoamingNetworkStatus1 is null)
                throw new ArgumentNullException(nameof(RoamingNetworkStatus1), "The given RoamingNetworkStatus1 must not be null!");

            return RoamingNetworkStatus1.CompareTo(RoamingNetworkStatus2) > 0;

        }

        #endregion

        #region Operator >= (RoamingNetworkStatus1, RoamingNetworkStatus2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="RoamingNetworkStatus1">A roaming network status.</param>
        /// <param name="RoamingNetworkStatus2">Another roaming network status.</param>
        /// <returns>true|false</returns>
        public static Boolean operator >= (RoamingNetworkStatus RoamingNetworkStatus1,
                                           RoamingNetworkStatus RoamingNetworkStatus2)

            => !(RoamingNetworkStatus1 < RoamingNetworkStatus2);

        #endregion

        #endregion

        #region IComparable<RoamingNetworkStatus> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        public Int32 CompareTo(Object? Object)

            => Object is RoamingNetworkStatus chargingStationOperatorStatus
                   ? CompareTo(chargingStationOperatorStatus)
                   : throw new ArgumentException("The given object is not a roaming network status!",
                                                 nameof(Object));

        #endregion

        #region CompareTo(RoamingNetworkStatus)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="RoamingNetworkStatus">An object to compare with.</param>
        public Int32 CompareTo(RoamingNetworkStatus? RoamingNetworkStatus)
        {

            if (RoamingNetworkStatus is null)
                throw new ArgumentNullException(nameof(RoamingNetworkStatus), "The given roaming network status must not be null!");

            var c = Id.       CompareTo(RoamingNetworkStatus.Id);

            if (c == 0)
                c = Status.   CompareTo(RoamingNetworkStatus.Status);

            if (c == 0)
                c = Timestamp.CompareTo(RoamingNetworkStatus.Timestamp);

            return c;

        }

        #endregion

        #endregion

        #region IEquatable<RoamingNetworkStatus> Members

        #region Equals(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        /// <returns>true|false</returns>
        public override Boolean Equals(Object? Object)

            => Object is RoamingNetworkStatus chargingStationOperatorStatus &&
                   Equals(chargingStationOperatorStatus);

        #endregion

        #region Equals(RoamingNetworkStatus)

        /// <summary>
        /// Compares two RoamingNetwork identifications for equality.
        /// </summary>
        /// <param name="RoamingNetworkStatus">A roaming network identification to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public Boolean Equals(RoamingNetworkStatus? RoamingNetworkStatus)

            => RoamingNetworkStatus is not null              &&
               Id.       Equals(RoamingNetworkStatus.Id)     &&
               Status.   Equals(RoamingNetworkStatus.Status) &&
               Timestamp.Equals(RoamingNetworkStatus.Timestamp);

        #endregion

        #endregion

        #region (override) GetHashCode()

        /// <summary>
        /// Return the HashCode of this object.
        /// </summary>
        /// <returns>The HashCode of this object.</returns>
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
                             Timestamp.ToIso8601());

        #endregion

    }

}
