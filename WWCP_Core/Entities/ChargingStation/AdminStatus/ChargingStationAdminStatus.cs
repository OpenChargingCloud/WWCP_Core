/*
 * Copyright (c) 2014-2024 GraphDefined GmbH <achim.friedland@graphdefined.com> <achim.friedland@graphdefined.com>
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

        #region ToJSON(this ChargingStationAdminStatus, Skip = null, Take = null)

        public static JObject ToJSON(this IEnumerable<ChargingStationAdminStatus>  ChargingStationAdminStatus,
                                     UInt64?                                       Skip   = null,
                                     UInt64?                                       Take   = null)
        {

            #region Initial checks

            if (ChargingStationAdminStatus is null || !ChargingStationAdminStatus.Any())
                return new JObject();

            #endregion

            #region Maybe there are duplicate charging station identifications in the enumeration... take the newest one!

            var filteredStatus = new Dictionary<ChargingStation_Id, ChargingStationAdminStatus>();

            foreach (var status in ChargingStationAdminStatus)
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
                                                                          kvp.Value.AdminStatus.ToString())
                                                              )));

        }

        #endregion

        #region Contains(this ChargingStationAdminStatus, Id, Status)

        /// <summary>
        /// Check if the given enumeration of charging stations and their current admin status
        /// contains the given pair of charging station identification and admin status.
        /// </summary>
        /// <param name="ChargingStationAdminStatus">An enumeration of charging stations and their current admin status.</param>
        /// <param name="Id">A charging station identification.</param>
        /// <param name="AdminStatus">A charging station admin status.</param>
        public static Boolean Contains(this IEnumerable<ChargingStationAdminStatus>  ChargingStationAdminStatus,
                                       ChargingStation_Id                            Id,
                                       ChargingStationAdminStatusTypes               AdminStatus)
        {

            foreach (var status in ChargingStationAdminStatus)
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
    /// The current admin status of a charging station.
    /// </summary>
    public class ChargingStationAdminStatus : AInternalData,
                                                      IEquatable<ChargingStationAdminStatus>,
                                                      IComparable<ChargingStationAdminStatus>
    {

        #region Properties

        /// <summary>
        /// The unique identification of the charging station.
        /// </summary>
        public ChargingStation_Id               Id             { get; }

        /// <summary>
        /// The current admin status of the charging station.
        /// </summary>
        public ChargingStationAdminStatusTypes  AdminStatus    { get; }

        /// <summary>
        /// The timestamp of the current admin status of the charging station.
        /// </summary>
        public DateTime                         Timestamp      { get; }

        /// <summary>
        /// The timestamped admin status of the charging station.
        /// </summary>
        public Timestamped<ChargingStationAdminStatusTypes> TimestampedAdminStatus
            => new (Timestamp, AdminStatus);

        #endregion

        #region Constructor(s)

        #region ChargingStationAdminStatus(Id, AdminStatus,            CustomData = null, InternalData = null)

        /// <summary>
        /// Create a new charging station admin status.
        /// </summary>
        /// <param name="Id">The unique identification of the charging station.</param>
        /// <param name="AdminStatus">The current timestamped adminstatus of the charging station.</param>
        /// <param name="CustomData">An optional dictionary of customer-specific data.</param>
        public ChargingStationAdminStatus(ChargingStation_Id                            Id,
                                          Timestamped<ChargingStationAdminStatusTypes>  AdminStatus,
                                          JObject?                                      CustomData     = null,
                                          UserDefinedDictionary?                        InternalData   = null)

            : base(CustomData,
                   InternalData,
                   org.GraphDefined.Vanaheimr.Illias.Timestamp.Now)

        {

            this.Id           = Id;
            this.AdminStatus  = AdminStatus.Value;
            this.Timestamp    = AdminStatus.Timestamp;

        }

        #endregion

        #region ChargingStationAdminStatus(Id, AdminStatus, Timestamp, CustomData = null, InternalData = null)

        /// <summary>
        /// Create a new charging station admin status.
        /// </summary>
        /// <param name="Id">The unique identification of the charging station.</param>
        /// <param name="Status">The current admin status of the charging station.</param>
        /// <param name="Timestamp">The timestamp of the status change of the charging station.</param>
        /// <param name="CustomData">An optional dictionary of customer-specific data.</param>
        public ChargingStationAdminStatus(ChargingStation_Id               Id,
                                          ChargingStationAdminStatusTypes  AdminStatus,
                                          DateTime                         Timestamp,
                                          JObject?                         CustomData     = null,
                                          UserDefinedDictionary?           InternalData   = null)

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


        #region (static) Snapshot(ChargingStation)

        /// <summary>
        /// Take a snapshot of the current charging station admin status.
        /// </summary>
        /// <param name="ChargingStation">A charging station.</param>
        public static ChargingStationAdminStatus Snapshot(ChargingStation ChargingStation)

            => new (ChargingStation.Id,
                    ChargingStation.AdminStatus);

        #endregion


        #region Operator overloading

        #region Operator == (ChargingStationAdminStatus1, ChargingStationAdminStatus2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingStationAdminStatus1">A charging station admin status.</param>
        /// <param name="ChargingStationAdminStatus2">Another charging station admin status.</param>
        /// <returns>true|false</returns>
        public static Boolean operator == (ChargingStationAdminStatus ChargingStationAdminStatus1,
                                           ChargingStationAdminStatus ChargingStationAdminStatus2)
        {

            // If both are null, or both are same instance, return true.
            if (ReferenceEquals(ChargingStationAdminStatus1, ChargingStationAdminStatus2))
                return true;

            // If one is null, but not both, return false.
            if (ChargingStationAdminStatus1 is null || ChargingStationAdminStatus2 is null)
                return false;

            return ChargingStationAdminStatus1.Equals(ChargingStationAdminStatus2);

        }

        #endregion

        #region Operator != (ChargingStationAdminStatus1, ChargingStationAdminStatus2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingStationAdminStatus1">A charging station admin status.</param>
        /// <param name="ChargingStationAdminStatus2">Another charging station admin status.</param>
        /// <returns>true|false</returns>
        public static Boolean operator != (ChargingStationAdminStatus ChargingStationAdminStatus1,
                                           ChargingStationAdminStatus ChargingStationAdminStatus2)

            => !(ChargingStationAdminStatus1 == ChargingStationAdminStatus2);

        #endregion

        #region Operator <  (ChargingStationAdminStatus1, ChargingStationAdminStatus2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingStationAdminStatus1">A charging station admin status.</param>
        /// <param name="ChargingStationAdminStatus2">Another charging station admin status.</param>
        /// <returns>true|false</returns>
        public static Boolean operator < (ChargingStationAdminStatus ChargingStationAdminStatus1,
                                          ChargingStationAdminStatus ChargingStationAdminStatus2)
        {

            if (ChargingStationAdminStatus1 is null)
                throw new ArgumentNullException(nameof(ChargingStationAdminStatus1), "The given ChargingStationAdminStatus1 must not be null!");

            return ChargingStationAdminStatus1.CompareTo(ChargingStationAdminStatus2) < 0;

        }

        #endregion

        #region Operator <= (ChargingStationAdminStatus1, ChargingStationAdminStatus2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingStationAdminStatus1">A charging station admin status.</param>
        /// <param name="ChargingStationAdminStatus2">Another charging station admin status.</param>
        /// <returns>true|false</returns>
        public static Boolean operator <= (ChargingStationAdminStatus ChargingStationAdminStatus1,
                                           ChargingStationAdminStatus ChargingStationAdminStatus2)

            => !(ChargingStationAdminStatus1 > ChargingStationAdminStatus2);

        #endregion

        #region Operator >  (ChargingStationAdminStatus1, ChargingStationAdminStatus2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingStationAdminStatus1">A charging station admin status.</param>
        /// <param name="ChargingStationAdminStatus2">Another charging station admin status.</param>
        /// <returns>true|false</returns>
        public static Boolean operator > (ChargingStationAdminStatus ChargingStationAdminStatus1,
                                          ChargingStationAdminStatus ChargingStationAdminStatus2)
        {

            if (ChargingStationAdminStatus1 is null)
                throw new ArgumentNullException(nameof(ChargingStationAdminStatus1), "The given ChargingStationAdminStatus1 must not be null!");

            return ChargingStationAdminStatus1.CompareTo(ChargingStationAdminStatus2) > 0;

        }

        #endregion

        #region Operator >= (ChargingStationAdminStatus1, ChargingStationAdminStatus2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingStationAdminStatus1">A charging station admin status.</param>
        /// <param name="ChargingStationAdminStatus2">Another charging station admin status.</param>
        /// <returns>true|false</returns>
        public static Boolean operator >= (ChargingStationAdminStatus ChargingStationAdminStatus1,
                                           ChargingStationAdminStatus ChargingStationAdminStatus2)

            => !(ChargingStationAdminStatus1 < ChargingStationAdminStatus2);

        #endregion

        #endregion

        #region IComparable<ChargingStationAdminStatus> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        public Int32 CompareTo(Object? Object)

            => Object is ChargingStationAdminStatus chargingStationAdminStatus
                   ? CompareTo(chargingStationAdminStatus)
                   : throw new ArgumentException("The given object is not a charging station admin status!",
                                                 nameof(Object));

        #endregion

        #region CompareTo(ChargingStationAdminStatus)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingStationAdminStatus">An object to compare with.</param>
        public Int32 CompareTo(ChargingStationAdminStatus? ChargingStationAdminStatus)
        {

            if (ChargingStationAdminStatus is null)
                throw new ArgumentNullException(nameof(ChargingStationAdminStatus), "The given charging station admin status must not be null!");

            var c = Id.         CompareTo(ChargingStationAdminStatus.Id);

            if (c == 0)
                c = AdminStatus.CompareTo(ChargingStationAdminStatus.AdminStatus);

            if (c == 0)
                c = Timestamp.  CompareTo(ChargingStationAdminStatus.Timestamp);

            return c;

        }

        #endregion

        #endregion

        #region IEquatable<ChargingStationAdminStatus> Members

        #region Equals(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        /// <returns>true|false</returns>
        public override Boolean Equals(Object? Object)

            => Object is ChargingStationAdminStatus chargingStationAdminStatus &&
                   Equals(chargingStationAdminStatus);

        #endregion

        #region Equals(ChargingStationAdminStatus)

        /// <summary>
        /// Compares two ChargingStation identifications for equality.
        /// </summary>
        /// <param name="ChargingStationAdminStatus">A charging station identification to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public Boolean Equals(ChargingStationAdminStatus? ChargingStationAdminStatus)

            => ChargingStationAdminStatus is not null                     &&
               Id.         Equals(ChargingStationAdminStatus.Id)          &&
               AdminStatus.Equals(ChargingStationAdminStatus.AdminStatus) &&
               Timestamp.  Equals(ChargingStationAdminStatus.Timestamp);

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
                             Timestamp.ToIso8601());

        #endregion

    }

}
