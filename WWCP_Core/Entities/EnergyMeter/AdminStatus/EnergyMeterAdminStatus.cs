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
    /// Extension methods for the energy meter admin status.
    /// </summary>
    public static class EnergyMeterAdminStatusExtensions
    {

        #region ToJSON(this EnergyMeterAdminStatus, Skip = null, Take = null)

        public static JObject ToJSON(this IEnumerable<EnergyMeterAdminStatus>  EnergyMeterAdminStatus,
                                     UInt64?                                       Skip   = null,
                                     UInt64?                                       Take   = null)
        {

            #region Initial checks

            if (EnergyMeterAdminStatus is null || !EnergyMeterAdminStatus.Any())
                return new JObject();

            #endregion

            #region Maybe there are duplicate energy meter identifications in the enumeration... take the newest one!

            var filteredStatus = new Dictionary<EnergyMeter_Id, EnergyMeterAdminStatus>();

            foreach (var status in EnergyMeterAdminStatus)
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

        #region Contains(this EnergyMeterAdminStatus, Id, Status)

        /// <summary>
        /// Check if the given enumeration of energy meters and their current admin status
        /// contains the given pair of energy meter identification and admin status.
        /// </summary>
        /// <param name="EnergyMeterAdminStatus">An enumeration of energy meters and their current admin status.</param>
        /// <param name="Id">A energy meter identification.</param>
        /// <param name="AdminStatus">A energy meter admin status.</param>
        public static Boolean Contains(this IEnumerable<EnergyMeterAdminStatus>  EnergyMeterAdminStatus,
                                       EnergyMeter_Id                            Id,
                                       EnergyMeterAdminStatusTypes               AdminStatus)
        {

            foreach (var status in EnergyMeterAdminStatus)
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
    /// The current admin status of a energy meter.
    /// </summary>
    public class EnergyMeterAdminStatus : AInternalData,
                                                      IEquatable<EnergyMeterAdminStatus>,
                                                      IComparable<EnergyMeterAdminStatus>
    {

        #region Properties

        /// <summary>
        /// The unique identification of the energy meter.
        /// </summary>
        public EnergyMeter_Id               Id             { get; }

        /// <summary>
        /// The current admin status of the energy meter.
        /// </summary>
        public EnergyMeterAdminStatusTypes  AdminStatus    { get; }

        /// <summary>
        /// The timestamp of the current admin status of the energy meter.
        /// </summary>
        public DateTimeOffset               Timestamp      { get; }

        /// <summary>
        /// The timestamped admin status of the energy meter.
        /// </summary>
        public Timestamped<EnergyMeterAdminStatusTypes> TimestampedAdminStatus
            => new (Timestamp, AdminStatus);

        #endregion

        #region Constructor(s)

        #region EnergyMeterAdminStatus(Id, AdminStatus,            CustomData = null, InternalData = null)

        /// <summary>
        /// Create a new energy meter admin status.
        /// </summary>
        /// <param name="Id">The unique identification of the energy meter.</param>
        /// <param name="AdminStatus">The current timestamped adminstatus of the energy meter.</param>
        /// <param name="CustomData">An optional dictionary of customer-specific data.</param>
        public EnergyMeterAdminStatus(EnergyMeter_Id                            Id,
                                      Timestamped<EnergyMeterAdminStatusTypes>  AdminStatus,
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

        #region EnergyMeterAdminStatus(Id, AdminStatus, Timestamp, CustomData = null, InternalData = null)

        /// <summary>
        /// Create a new energy meter admin status.
        /// </summary>
        /// <param name="Id">The unique identification of the energy meter.</param>
        /// <param name="Status">The current admin status of the energy meter.</param>
        /// <param name="Timestamp">The timestamp of the status change of the energy meter.</param>
        /// <param name="CustomData">An optional dictionary of customer-specific data.</param>
        public EnergyMeterAdminStatus(EnergyMeter_Id               Id,
                                          EnergyMeterAdminStatusTypes  AdminStatus,
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


        #region (static) Snapshot(EnergyMeter)

        /// <summary>
        /// Take a snapshot of the current energy meter admin status.
        /// </summary>
        /// <param name="EnergyMeter">A energy meter.</param>
        public static EnergyMeterAdminStatus Snapshot(EnergyMeter EnergyMeter)

            => new (EnergyMeter.Id,
                    EnergyMeter.AdminStatus);

        #endregion


        #region Operator overloading

        #region Operator == (EnergyMeterAdminStatus1, EnergyMeterAdminStatus2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EnergyMeterAdminStatus1">A energy meter admin status.</param>
        /// <param name="EnergyMeterAdminStatus2">Another energy meter admin status.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator == (EnergyMeterAdminStatus EnergyMeterAdminStatus1,
                                           EnergyMeterAdminStatus EnergyMeterAdminStatus2)
        {

            // If both are null, or both are same instance, return true.
            if (ReferenceEquals(EnergyMeterAdminStatus1, EnergyMeterAdminStatus2))
                return true;

            // If one is null, but not both, return false.
            if (EnergyMeterAdminStatus1 is null || EnergyMeterAdminStatus2 is null)
                return false;

            return EnergyMeterAdminStatus1.Equals(EnergyMeterAdminStatus2);

        }

        #endregion

        #region Operator != (EnergyMeterAdminStatus1, EnergyMeterAdminStatus2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EnergyMeterAdminStatus1">A energy meter admin status.</param>
        /// <param name="EnergyMeterAdminStatus2">Another energy meter admin status.</param>
        /// <returns>False if both match; True otherwise.</returns>
        public static Boolean operator != (EnergyMeterAdminStatus EnergyMeterAdminStatus1,
                                           EnergyMeterAdminStatus EnergyMeterAdminStatus2)

            => !(EnergyMeterAdminStatus1 == EnergyMeterAdminStatus2);

        #endregion

        #region Operator <  (EnergyMeterAdminStatus1, EnergyMeterAdminStatus2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EnergyMeterAdminStatus1">A energy meter admin status.</param>
        /// <param name="EnergyMeterAdminStatus2">Another energy meter admin status.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator < (EnergyMeterAdminStatus EnergyMeterAdminStatus1,
                                          EnergyMeterAdminStatus EnergyMeterAdminStatus2)
        {

            if (EnergyMeterAdminStatus1 is null)
                throw new ArgumentNullException(nameof(EnergyMeterAdminStatus1), "The given EnergyMeterAdminStatus1 must not be null!");

            return EnergyMeterAdminStatus1.CompareTo(EnergyMeterAdminStatus2) < 0;

        }

        #endregion

        #region Operator <= (EnergyMeterAdminStatus1, EnergyMeterAdminStatus2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EnergyMeterAdminStatus1">A energy meter admin status.</param>
        /// <param name="EnergyMeterAdminStatus2">Another energy meter admin status.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator <= (EnergyMeterAdminStatus EnergyMeterAdminStatus1,
                                           EnergyMeterAdminStatus EnergyMeterAdminStatus2)

            => !(EnergyMeterAdminStatus1 > EnergyMeterAdminStatus2);

        #endregion

        #region Operator >  (EnergyMeterAdminStatus1, EnergyMeterAdminStatus2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EnergyMeterAdminStatus1">A energy meter admin status.</param>
        /// <param name="EnergyMeterAdminStatus2">Another energy meter admin status.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator > (EnergyMeterAdminStatus EnergyMeterAdminStatus1,
                                          EnergyMeterAdminStatus EnergyMeterAdminStatus2)
        {

            if (EnergyMeterAdminStatus1 is null)
                throw new ArgumentNullException(nameof(EnergyMeterAdminStatus1), "The given EnergyMeterAdminStatus1 must not be null!");

            return EnergyMeterAdminStatus1.CompareTo(EnergyMeterAdminStatus2) > 0;

        }

        #endregion

        #region Operator >= (EnergyMeterAdminStatus1, EnergyMeterAdminStatus2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EnergyMeterAdminStatus1">A energy meter admin status.</param>
        /// <param name="EnergyMeterAdminStatus2">Another energy meter admin status.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator >= (EnergyMeterAdminStatus EnergyMeterAdminStatus1,
                                           EnergyMeterAdminStatus EnergyMeterAdminStatus2)

            => !(EnergyMeterAdminStatus1 < EnergyMeterAdminStatus2);

        #endregion

        #endregion

        #region IComparable<EnergyMeterAdminStatus> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        public Int32 CompareTo(Object? Object)

            => Object is EnergyMeterAdminStatus energyMeterAdminStatus
                   ? CompareTo(energyMeterAdminStatus)
                   : throw new ArgumentException("The given object is not a energy meter admin status!",
                                                 nameof(Object));

        #endregion

        #region CompareTo(EnergyMeterAdminStatus)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EnergyMeterAdminStatus">An object to compare with.</param>
        public Int32 CompareTo(EnergyMeterAdminStatus? EnergyMeterAdminStatus)
        {

            if (EnergyMeterAdminStatus is null)
                throw new ArgumentNullException(nameof(EnergyMeterAdminStatus), "The given energy meter admin status must not be null!");

            var c = Id.         CompareTo(EnergyMeterAdminStatus.Id);

            if (c == 0)
                c = AdminStatus.CompareTo(EnergyMeterAdminStatus.AdminStatus);

            if (c == 0)
                c = Timestamp.  CompareTo(EnergyMeterAdminStatus.Timestamp);

            return c;

        }

        #endregion

        #endregion

        #region IEquatable<EnergyMeterAdminStatus> Members

        #region Equals(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public override Boolean Equals(Object? Object)

            => Object is EnergyMeterAdminStatus energyMeterAdminStatus &&
                   Equals(energyMeterAdminStatus);

        #endregion

        #region Equals(EnergyMeterAdminStatus)

        /// <summary>
        /// Compares two EnergyMeter identifications for equality.
        /// </summary>
        /// <param name="EnergyMeterAdminStatus">A energy meter identification to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public Boolean Equals(EnergyMeterAdminStatus? EnergyMeterAdminStatus)

            => EnergyMeterAdminStatus is not null                     &&
               Id.         Equals(EnergyMeterAdminStatus.Id)          &&
               AdminStatus.Equals(EnergyMeterAdminStatus.AdminStatus) &&
               Timestamp.  Equals(EnergyMeterAdminStatus.Timestamp);

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
