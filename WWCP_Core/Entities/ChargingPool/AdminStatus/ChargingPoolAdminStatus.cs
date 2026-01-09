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

        #region ToJSON(this ChargingPoolAdminStatus, Skip = null, Take = null)

        public static JObject ToJSON(this IEnumerable<ChargingPoolAdminStatus>  ChargingPoolAdminStatus,
                                     UInt64?                                    Skip  = null,
                                     UInt64?                                    Take  = null)
        {

            #region Initial checks

            if (ChargingPoolAdminStatus is null || !ChargingPoolAdminStatus.Any())
                return new JObject();

            #endregion

            #region Maybe there are duplicate charging pool identifications in the enumeration... take the newest one!

            var filteredStatus = new Dictionary<ChargingPool_Id, ChargingPoolAdminStatus>();

            foreach (var status in ChargingPoolAdminStatus)
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

        #region Contains(this ChargingPoolAdminStatus, Id, Status)

        /// <summary>
        /// Check if the given enumeration of charging pools and their current admin status
        /// contains the given pair of charging pool identification and admin status.
        /// </summary>
        /// <param name="ChargingPoolAdminStatus">An enumeration of charging pools and their current admin status.</param>
        /// <param name="Id">A charging pool identification.</param>
        /// <param name="Status">A charging pool admin status.</param>
        public static Boolean Contains(this IEnumerable<ChargingPoolAdminStatus>  ChargingPoolAdminStatus,
                                       ChargingPool_Id                            Id,
                                       ChargingPoolAdminStatusType               Status)
        {

            foreach (var adminStatus in ChargingPoolAdminStatus)
            {
                if (adminStatus.Id          == Id &&
                    adminStatus.AdminStatus == Status)
                {
                    return true;
                }
            }

            return false;

        }

        #endregion

    }


    /// <summary>
    /// The current admin status of a charging pool.
    /// </summary>
    public class ChargingPoolAdminStatus : AInternalData,
                                           IEquatable <ChargingPoolAdminStatus>,
                                           IComparable<ChargingPoolAdminStatus>
    {

        #region Properties

        /// <summary>
        /// The unique identification of the charging pool.
        /// </summary>
        public ChargingPool_Id               Id             { get; }

        /// <summary>
        /// The current admin status of the charging pool.
        /// </summary>
        public ChargingPoolAdminStatusType  AdminStatus    { get; }

        /// <summary>
        /// The timestamp of the current admin status of the charging pool.
        /// </summary>
        public DateTimeOffset               Timestamp      { get; }

        /// <summary>
        /// The timestamped admin status of the charging pool.
        /// </summary>
        public Timestamped<ChargingPoolAdminStatusType> TimestampedAdminStatus
            => new (Timestamp, AdminStatus);

        #endregion

        #region Constructor(s)

        #region ChargingPoolAdminStatus(Id, AdminStatus,            CustomData = null, InternalData = null)

        /// <summary>
        /// Create a new charging pool admin status.
        /// </summary>
        /// <param name="Id">The unique identification of the charging pool.</param>
        /// <param name="AdminStatus">The current timestamped adminstatus of the charging pool.</param>
        /// <param name="CustomData">An optional dictionary of customer-specific data.</param>
        public ChargingPoolAdminStatus(ChargingPool_Id                           Id,
                                       Timestamped<ChargingPoolAdminStatusType>  AdminStatus,
                                       JObject?                                  CustomData     = null,
                                       UserDefinedDictionary?                    InternalData   = null)

            : base(CustomData,
                   InternalData,
                   org.GraphDefined.Vanaheimr.Illias.Timestamp.Now)

        {

            this.Id           = Id;
            this.AdminStatus  = AdminStatus.Value;
            this.Timestamp    = AdminStatus.Timestamp;

        }

        #endregion

        #region ChargingPoolAdminStatus(Id, AdminStatus, Timestamp, CustomData = null, InternalData = null)

        /// <summary>
        /// Create a new charging pool admin status.
        /// </summary>
        /// <param name="Id">The unique identification of the charging pool.</param>
        /// <param name="Status">The current admin status of the charging pool.</param>
        /// <param name="Timestamp">The timestamp of the status change of the charging pool.</param>
        /// <param name="CustomData">An optional dictionary of customer-specific data.</param>
        public ChargingPoolAdminStatus(ChargingPool_Id               Id,
                                       ChargingPoolAdminStatusType  AdminStatus,
                                       DateTime                      Timestamp,
                                       JObject?                      CustomData     = null,
                                       UserDefinedDictionary?        InternalData   = null)

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


        #region (static) Snapshot(ChargingPool)

        /// <summary>
        /// Take a snapshot of the current charging pool admin status.
        /// </summary>
        /// <param name="ChargingPool">A charging pool.</param>
        public static ChargingPoolAdminStatus Snapshot(ChargingPool ChargingPool)

            => new (ChargingPool.Id,
                    ChargingPool.AdminStatus);

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
        {

            // If both are null, or both are same instance, return true.
            if (ReferenceEquals(ChargingPoolAdminStatus1, ChargingPoolAdminStatus2))
                return true;

            // If one is null, but not both, return false.
            if (ChargingPoolAdminStatus1 is null || ChargingPoolAdminStatus2 is null)
                return false;

            return ChargingPoolAdminStatus1.Equals(ChargingPoolAdminStatus2);

        }

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

            => !(ChargingPoolAdminStatus1 == ChargingPoolAdminStatus2);

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
        {

            if (ChargingPoolAdminStatus1 is null)
                throw new ArgumentNullException(nameof(ChargingPoolAdminStatus1), "The given ChargingPoolAdminStatus1 must not be null!");

            return ChargingPoolAdminStatus1.CompareTo(ChargingPoolAdminStatus2) < 0;

        }

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

            => !(ChargingPoolAdminStatus1 > ChargingPoolAdminStatus2);

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
        {

            if (ChargingPoolAdminStatus1 is null)
                throw new ArgumentNullException(nameof(ChargingPoolAdminStatus1), "The given ChargingPoolAdminStatus1 must not be null!");

            return ChargingPoolAdminStatus1.CompareTo(ChargingPoolAdminStatus2) > 0;

        }

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

            => !(ChargingPoolAdminStatus1 < ChargingPoolAdminStatus2);

        #endregion

        #endregion

        #region IComparable<ChargingPoolAdminStatus> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        public Int32 CompareTo(Object? Object)

            => Object is ChargingPoolAdminStatus chargingPoolAdminStatus
                   ? CompareTo(chargingPoolAdminStatus)
                   : throw new ArgumentException("The given object is not a charging pool admin status!",
                                                 nameof(Object));

        #endregion

        #region CompareTo(ChargingPoolAdminStatus)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingPoolAdminStatus">An object to compare with.</param>
        public Int32 CompareTo(ChargingPoolAdminStatus? ChargingPoolAdminStatus)
        {

            if (ChargingPoolAdminStatus is null)
                throw new ArgumentNullException(nameof(ChargingPoolAdminStatus), "The given charging pool admin status must not be null!");

            var c = Id.         CompareTo(ChargingPoolAdminStatus.Id);

            if (c == 0)
                c = AdminStatus.CompareTo(ChargingPoolAdminStatus.AdminStatus);

            if (c == 0)
                c = Timestamp.  CompareTo(ChargingPoolAdminStatus.Timestamp);

            return c;

        }

        #endregion

        #endregion

        #region IEquatable<ChargingPoolAdminStatus> Members

        #region Equals(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public override Boolean Equals(Object? Object)

            => Object is ChargingPoolAdminStatus chargingPoolAdminStatus &&
                   Equals(chargingPoolAdminStatus);

        #endregion

        #region Equals(ChargingPoolAdminStatus)

        /// <summary>
        /// Compares two ChargingPool identifications for equality.
        /// </summary>
        /// <param name="ChargingPoolAdminStatus">A charging pool identification to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public Boolean Equals(ChargingPoolAdminStatus? ChargingPoolAdminStatus)

            => ChargingPoolAdminStatus is not null                     &&
               Id.         Equals(ChargingPoolAdminStatus.Id)          &&
               AdminStatus.Equals(ChargingPoolAdminStatus.AdminStatus) &&
               Timestamp.  Equals(ChargingPoolAdminStatus.Timestamp);

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
