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
    /// Extension methods for the charging station operator admin status.
    /// </summary>
    public static class ChargingStationOperatorAdminStatusExtensions
    {

        #region ToJSON(this ChargingStationOperatorAdminStatus, Skip = null, Take = null)

        public static JObject ToJSON(this IEnumerable<ChargingStationOperatorAdminStatus>  ChargingStationOperatorAdminStatus,
                                     UInt64?                                               Skip   = null,
                                     UInt64?                                               Take   = null)
        {

            #region Initial checks

            if (ChargingStationOperatorAdminStatus is null || !ChargingStationOperatorAdminStatus.Any())
                return new JObject();

            #endregion

            #region Maybe there are duplicate ChargingStationOperatorAdmin identifications in the enumeration... take the newest one!

            var filteredStatus = new Dictionary<ChargingStationOperator_Id, ChargingStationOperatorAdminStatus>();

            foreach (var status in ChargingStationOperatorAdminStatus)
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

        #region Contains(this ChargingStationOperatorAdminStatus, Id, Status)

        /// <summary>
        /// Check if the given enumeration of charging station operators and their current admin status
        /// contains the given pair of charging station operator identification and admin status.
        /// </summary>
        /// <param name="ChargingStationOperatorAdminStatus">An enumeration of charging station operators and their current admin status.</param>
        /// <param name="Id">A charging station operator identification.</param>
        /// <param name="AdminStatus">A charging station operator admin status.</param>
        public static Boolean Contains(this IEnumerable<ChargingStationOperatorAdminStatus>  ChargingStationOperatorAdminStatus,
                                       ChargingStationOperator_Id                            Id,
                                       ChargingStationOperatorAdminStatusTypes               AdminStatus)
        {

            foreach (var status in ChargingStationOperatorAdminStatus)
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
    /// The current admin status of a charging station operator.
    /// </summary>
    public class ChargingStationOperatorAdminStatus : AInternalData,
                                                      IEquatable<ChargingStationOperatorAdminStatus>,
                                                      IComparable<ChargingStationOperatorAdminStatus>
    {

        #region Properties

        /// <summary>
        /// The unique identification of the charging station operator.
        /// </summary>
        public ChargingStationOperator_Id               Id             { get; }

        /// <summary>
        /// The current admin status of the charging station operator.
        /// </summary>
        public ChargingStationOperatorAdminStatusTypes  AdminStatus    { get; }

        /// <summary>
        /// The timestamp of the current admin status of the charging station operator.
        /// </summary>
        public DateTime                                 Timestamp      { get; }

        /// <summary>
        /// The timestamped admin status of the charging station operator.
        /// </summary>
        public Timestamped<ChargingStationOperatorAdminStatusTypes> TimestampedAdminStatus
            => new (Timestamp, AdminStatus);

        #endregion

        #region Constructor(s)

        #region ChargingStationOperatorStatus(Id, AdminStatus,            CustomData = null, InternalData = null)

        /// <summary>
        /// Create a new charging station operator admin status.
        /// </summary>
        /// <param name="Id">The unique identification of the charging station operator.</param>
        /// <param name="AdminStatus">The current timestamped adminstatus of the charging station operator.</param>
        /// <param name="CustomData">An optional dictionary of customer-specific data.</param>
        public ChargingStationOperatorAdminStatus(ChargingStationOperator_Id                            Id,
                                                  Timestamped<ChargingStationOperatorAdminStatusTypes>  AdminStatus,
                                                  JObject?                                              CustomData     = null,
                                                  UserDefinedDictionary?                                InternalData   = null)

            : base(CustomData,
                   InternalData)

        {

            this.Id           = Id;
            this.AdminStatus  = AdminStatus.Value;
            this.Timestamp    = AdminStatus.Timestamp;

        }

        #endregion

        #region ChargingStationOperatorStatus(Id, AdminStatus, Timestamp, CustomData = null, InternalData = null)

        /// <summary>
        /// Create a new charging station operator admin status.
        /// </summary>
        /// <param name="Id">The unique identification of the charging station operator.</param>
        /// <param name="Status">The current admin status of the charging station operator.</param>
        /// <param name="Timestamp">The timestamp of the status change of the charging station operator.</param>
        /// <param name="CustomData">An optional dictionary of customer-specific data.</param>
        public ChargingStationOperatorAdminStatus(ChargingStationOperator_Id               Id,
                                                  ChargingStationOperatorAdminStatusTypes  AdminStatus,
                                                  DateTime                                 Timestamp,
                                                  JObject?                                 CustomData     = null,
                                                  UserDefinedDictionary?                   InternalData   = null)

            : base(CustomData,
                   InternalData)

        {

            this.Id           = Id;
            this.AdminStatus  = AdminStatus;
            this.Timestamp    = Timestamp;

        }

        #endregion

        #endregion


        #region (static) Snapshot(ChargingStationOperator)

        /// <summary>
        /// Take a snapshot of the current charging station operator admin status.
        /// </summary>
        /// <param name="ChargingStationOperator">A charging station operator.</param>
        public static ChargingStationOperatorAdminStatus Snapshot(ChargingStationOperator ChargingStationOperator)

            => new (ChargingStationOperator.Id,
                    ChargingStationOperator.AdminStatus);

        #endregion


        #region Operator overloading

        #region Operator == (ChargingStationOperatorAdminStatus1, ChargingStationOperatorAdminStatus2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingStationOperatorAdminStatus1">A charging station operator admin status.</param>
        /// <param name="ChargingStationOperatorAdminStatus2">Another charging station operator admin status.</param>
        /// <returns>true|false</returns>
        public static Boolean operator == (ChargingStationOperatorAdminStatus ChargingStationOperatorAdminStatus1,
                                           ChargingStationOperatorAdminStatus ChargingStationOperatorAdminStatus2)
        {

            // If both are null, or both are same instance, return true.
            if (ReferenceEquals(ChargingStationOperatorAdminStatus1, ChargingStationOperatorAdminStatus2))
                return true;

            // If one is null, but not both, return false.
            if (ChargingStationOperatorAdminStatus1 is null || ChargingStationOperatorAdminStatus2 is null)
                return false;

            return ChargingStationOperatorAdminStatus1.Equals(ChargingStationOperatorAdminStatus2);

        }

        #endregion

        #region Operator != (ChargingStationOperatorAdminStatus1, ChargingStationOperatorAdminStatus2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingStationOperatorAdminStatus1">A charging station operator admin status.</param>
        /// <param name="ChargingStationOperatorAdminStatus2">Another charging station operator admin status.</param>
        /// <returns>true|false</returns>
        public static Boolean operator != (ChargingStationOperatorAdminStatus ChargingStationOperatorAdminStatus1,
                                           ChargingStationOperatorAdminStatus ChargingStationOperatorAdminStatus2)

            => !(ChargingStationOperatorAdminStatus1 == ChargingStationOperatorAdminStatus2);

        #endregion

        #region Operator <  (ChargingStationOperatorAdminStatus1, ChargingStationOperatorAdminStatus2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingStationOperatorAdminStatus1">A charging station operator admin status.</param>
        /// <param name="ChargingStationOperatorAdminStatus2">Another charging station operator admin status.</param>
        /// <returns>true|false</returns>
        public static Boolean operator < (ChargingStationOperatorAdminStatus ChargingStationOperatorAdminStatus1,
                                          ChargingStationOperatorAdminStatus ChargingStationOperatorAdminStatus2)
        {

            if (ChargingStationOperatorAdminStatus1 is null)
                throw new ArgumentNullException(nameof(ChargingStationOperatorAdminStatus1), "The given ChargingStationOperatorAdminStatus1 must not be null!");

            return ChargingStationOperatorAdminStatus1.CompareTo(ChargingStationOperatorAdminStatus2) < 0;

        }

        #endregion

        #region Operator <= (ChargingStationOperatorAdminStatus1, ChargingStationOperatorAdminStatus2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingStationOperatorAdminStatus1">A charging station operator admin status.</param>
        /// <param name="ChargingStationOperatorAdminStatus2">Another charging station operator admin status.</param>
        /// <returns>true|false</returns>
        public static Boolean operator <= (ChargingStationOperatorAdminStatus ChargingStationOperatorAdminStatus1,
                                           ChargingStationOperatorAdminStatus ChargingStationOperatorAdminStatus2)

            => !(ChargingStationOperatorAdminStatus1 > ChargingStationOperatorAdminStatus2);

        #endregion

        #region Operator >  (ChargingStationOperatorAdminStatus1, ChargingStationOperatorAdminStatus2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingStationOperatorAdminStatus1">A charging station operator admin status.</param>
        /// <param name="ChargingStationOperatorAdminStatus2">Another charging station operator admin status.</param>
        /// <returns>true|false</returns>
        public static Boolean operator > (ChargingStationOperatorAdminStatus ChargingStationOperatorAdminStatus1,
                                          ChargingStationOperatorAdminStatus ChargingStationOperatorAdminStatus2)
        {

            if (ChargingStationOperatorAdminStatus1 is null)
                throw new ArgumentNullException(nameof(ChargingStationOperatorAdminStatus1), "The given ChargingStationOperatorAdminStatus1 must not be null!");

            return ChargingStationOperatorAdminStatus1.CompareTo(ChargingStationOperatorAdminStatus2) > 0;

        }

        #endregion

        #region Operator >= (ChargingStationOperatorAdminStatus1, ChargingStationOperatorAdminStatus2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingStationOperatorAdminStatus1">A charging station operator admin status.</param>
        /// <param name="ChargingStationOperatorAdminStatus2">Another charging station operator admin status.</param>
        /// <returns>true|false</returns>
        public static Boolean operator >= (ChargingStationOperatorAdminStatus ChargingStationOperatorAdminStatus1,
                                           ChargingStationOperatorAdminStatus ChargingStationOperatorAdminStatus2)

            => !(ChargingStationOperatorAdminStatus1 < ChargingStationOperatorAdminStatus2);

        #endregion

        #endregion

        #region IComparable<ChargingStationOperatorAdminStatus> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        public Int32 CompareTo(Object? Object)

            => Object is ChargingStationOperatorAdminStatus chargingStationOperatorAdminStatus
                   ? CompareTo(chargingStationOperatorAdminStatus)
                   : throw new ArgumentException("The given object is not a charging station operator admin status!",
                                                 nameof(Object));

        #endregion

        #region CompareTo(ChargingStationOperatorAdminStatus)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingStationOperatorAdminStatus">An object to compare with.</param>
        public Int32 CompareTo(ChargingStationOperatorAdminStatus? ChargingStationOperatorAdminStatus)
        {

            if (ChargingStationOperatorAdminStatus is null)
                throw new ArgumentNullException(nameof(ChargingStationOperatorAdminStatus), "The given charging station operator admin status must not be null!");

            var c = Id.         CompareTo(ChargingStationOperatorAdminStatus.Id);

            if (c == 0)
                c = AdminStatus.CompareTo(ChargingStationOperatorAdminStatus.AdminStatus);

            if (c == 0)
                c = Timestamp.  CompareTo(ChargingStationOperatorAdminStatus.Timestamp);

            return c;

        }

        #endregion

        #endregion

        #region IEquatable<ChargingStationOperatorAdminStatus> Members

        #region Equals(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        /// <returns>true|false</returns>
        public override Boolean Equals(Object? Object)

            => Object is ChargingStationOperatorAdminStatus chargingStationOperatorAdminStatus &&
                   Equals(chargingStationOperatorAdminStatus);

        #endregion

        #region Equals(ChargingStationOperatorAdminStatus)

        /// <summary>
        /// Compares two ChargingStationOperator identifications for equality.
        /// </summary>
        /// <param name="ChargingStationOperatorAdminStatus">A charging station operator identification to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public Boolean Equals(ChargingStationOperatorAdminStatus? ChargingStationOperatorAdminStatus)

            => ChargingStationOperatorAdminStatus is not null                     &&
               Id.         Equals(ChargingStationOperatorAdminStatus.Id)          &&
               AdminStatus.Equals(ChargingStationOperatorAdminStatus.AdminStatus) &&
               Timestamp.  Equals(ChargingStationOperatorAdminStatus.Timestamp);

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
