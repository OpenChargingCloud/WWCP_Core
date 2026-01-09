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
    /// Extension methods for the energy meter status.
    /// </summary>
    public static class EnergyMeterStatusExtensions
    {

        #region ToJSON(this EnergyMeterStatus, Skip = null, Take = null)

        public static JObject ToJSON(this IEnumerable<EnergyMeterStatus>  EnergyMeterStatus,
                                     UInt64?                                  Skip   = null,
                                     UInt64?                                  Take   = null)
        {

            #region Initial checks

            if (EnergyMeterStatus is null || !EnergyMeterStatus.Any())
                return new JObject();

            #endregion

            #region Maybe there are duplicate energy meter identifications in the enumeration... take the newest one!

            var filteredStatus = new Dictionary<EnergyMeter_Id, EnergyMeterStatus>();

            foreach (var status in EnergyMeterStatus)
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
                                                                          kvp.Value.Status.ToString())
                                                              )));

        }

        #endregion

        #region Contains(this EnergyMeterStatus, Id, Status)

        /// <summary>
        /// Check if the given enumeration of energy meters and their current status
        /// contains the given pair of energy meter identification and status.
        /// </summary>
        /// <param name="EnergyMeterStatus">An enumeration of energy meters and their current status.</param>
        /// <param name="Id">A energy meter identification.</param>
        /// <param name="Status">A energy meter status.</param>
        public static Boolean Contains(this IEnumerable<EnergyMeterStatus>  EnergyMeterStatus,
                                       EnergyMeter_Id                       Id,
                                       EnergyMeterStatusTypes               Status)
        {

            foreach (var status in EnergyMeterStatus)
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
    /// The current status of an energy meter.
    /// </summary>
    public class EnergyMeterStatus : AInternalData,
                                     IEquatable <EnergyMeterStatus>,
                                     IComparable<EnergyMeterStatus>
    {

        #region Properties

        /// <summary>
        /// The unique identification of the energy meter.
        /// </summary>
        public EnergyMeter_Id          Id           { get; }

        /// <summary>
        /// The current status of the energy meter.
        /// </summary>
        public EnergyMeterStatusTypes  Status       { get; }

        /// <summary>
        /// The timestamp of the current status of the energy meter.
        /// </summary>
        public DateTimeOffset          Timestamp    { get; }

        /// <summary>
        /// The timestamped status of the energy meter.
        /// </summary>
        public Timestamped<EnergyMeterStatusTypes> TimestampedStatus
            => new (Timestamp, Status);

        #endregion

        #region Constructor(s)

        #region EnergyMeterStatus(Id, Status,            CustomData = null, InternalData = null)

        /// <summary>
        /// Create a new energy meter status.
        /// </summary>
        /// <param name="Id">The unique identification of the energy meter.</param>
        /// <param name="Status">The current timestamped status of the energy meter.</param>
        /// <param name="CustomData">An optional dictionary of customer-specific data.</param>
        public EnergyMeterStatus(EnergyMeter_Id                       Id,
                                 Timestamped<EnergyMeterStatusTypes>  Status,
                                 JObject?                             CustomData     = null,
                                 UserDefinedDictionary?               InternalData   = null)

            : base(CustomData,
                   InternalData,
                   org.GraphDefined.Vanaheimr.Illias.Timestamp.Now)

        {

            this.Id         = Id;
            this.Status     = Status.Value;
            this.Timestamp  = Status.Timestamp;

        }

        #endregion

        #region EnergyMeterStatus(Id, Status, Timestamp, CustomData = null, InternalData = null)

        /// <summary>
        /// Create a new energy meter status.
        /// </summary>
        /// <param name="Id">The unique identification of the energy meter.</param>
        /// <param name="Status">The current status of the energy meter.</param>
        /// <param name="Timestamp">The timestamp of the status change of the energy meter.</param>
        /// <param name="CustomData">An optional dictionary of customer-specific data.</param>
        public EnergyMeterStatus(EnergyMeter_Id          Id,
                                     EnergyMeterStatusTypes  Status,
                                     DateTime                    Timestamp,
                                     JObject?                    CustomData     = null,
                                     UserDefinedDictionary?      InternalData   = null)

            : base(CustomData,
                   InternalData,
                   org.GraphDefined.Vanaheimr.Illias.Timestamp.Now)

        {

            this.Id         = Id;
            this.Status     = Status;
            this.Timestamp  = Timestamp;

        }

        #endregion

        #endregion


        #region (static) Snapshot(EnergyMeter)

        /// <summary>
        /// Take a snapshot of the current energy meter status.
        /// </summary>
        /// <param name="EnergyMeter">A energy meter.</param>
        public static EnergyMeterStatus Snapshot(EnergyMeter EnergyMeter)

            => new (EnergyMeter.Id,
                    EnergyMeter.Status);

        #endregion


        #region Operator overloading

        #region Operator == (EnergyMeterStatus1, EnergyMeterStatus2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EnergyMeterStatus1">A energy meter status.</param>
        /// <param name="EnergyMeterStatus2">Another energy meter status.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator == (EnergyMeterStatus EnergyMeterStatus1,
                                           EnergyMeterStatus EnergyMeterStatus2)
        {

            // If both are null, or both are same instance, return true.
            if (ReferenceEquals(EnergyMeterStatus1, EnergyMeterStatus2))
                return true;

            // If one is null, but not both, return false.
            if (EnergyMeterStatus1 is null || EnergyMeterStatus2 is null)
                return false;

            return EnergyMeterStatus1.Equals(EnergyMeterStatus2);

        }

        #endregion

        #region Operator != (EnergyMeterStatus1, EnergyMeterStatus2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EnergyMeterStatus1">A energy meter status.</param>
        /// <param name="EnergyMeterStatus2">Another energy meter status.</param>
        /// <returns>False if both match; True otherwise.</returns>
        public static Boolean operator != (EnergyMeterStatus EnergyMeterStatus1,
                                           EnergyMeterStatus EnergyMeterStatus2)

            => !(EnergyMeterStatus1 == EnergyMeterStatus2);

        #endregion

        #region Operator <  (EnergyMeterStatus1, EnergyMeterStatus2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EnergyMeterStatus1">A energy meter status.</param>
        /// <param name="EnergyMeterStatus2">Another energy meter status.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator < (EnergyMeterStatus EnergyMeterStatus1,
                                          EnergyMeterStatus EnergyMeterStatus2)
        {

            if (EnergyMeterStatus1 is null)
                throw new ArgumentNullException(nameof(EnergyMeterStatus1), "The given EnergyMeterStatus1 must not be null!");

            return EnergyMeterStatus1.CompareTo(EnergyMeterStatus2) < 0;

        }

        #endregion

        #region Operator <= (EnergyMeterStatus1, EnergyMeterStatus2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EnergyMeterStatus1">A energy meter status.</param>
        /// <param name="EnergyMeterStatus2">Another energy meter status.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator <= (EnergyMeterStatus EnergyMeterStatus1,
                                           EnergyMeterStatus EnergyMeterStatus2)

            => !(EnergyMeterStatus1 > EnergyMeterStatus2);

        #endregion

        #region Operator >  (EnergyMeterStatus1, EnergyMeterStatus2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EnergyMeterStatus1">A energy meter status.</param>
        /// <param name="EnergyMeterStatus2">Another energy meter status.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator > (EnergyMeterStatus EnergyMeterStatus1,
                                          EnergyMeterStatus EnergyMeterStatus2)
        {

            if (EnergyMeterStatus1 is null)
                throw new ArgumentNullException(nameof(EnergyMeterStatus1), "The given EnergyMeterStatus1 must not be null!");

            return EnergyMeterStatus1.CompareTo(EnergyMeterStatus2) > 0;

        }

        #endregion

        #region Operator >= (EnergyMeterStatus1, EnergyMeterStatus2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EnergyMeterStatus1">A energy meter status.</param>
        /// <param name="EnergyMeterStatus2">Another energy meter status.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator >= (EnergyMeterStatus EnergyMeterStatus1,
                                           EnergyMeterStatus EnergyMeterStatus2)

            => !(EnergyMeterStatus1 < EnergyMeterStatus2);

        #endregion

        #endregion

        #region IComparable<EnergyMeterStatus> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        public Int32 CompareTo(Object? Object)

            => Object is EnergyMeterStatus energyMeterStatus
                   ? CompareTo(energyMeterStatus)
                   : throw new ArgumentException("The given object is not an energy meter status!",
                                                 nameof(Object));

        #endregion

        #region CompareTo(EnergyMeterStatus)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EnergyMeterStatus">An object to compare with.</param>
        public Int32 CompareTo(EnergyMeterStatus? EnergyMeterStatus)
        {

            if (EnergyMeterStatus is null)
                throw new ArgumentNullException(nameof(EnergyMeterStatus), "The given energy meter status must not be null!");

            var c = Id.       CompareTo(EnergyMeterStatus.Id);

            if (c == 0)
                c = Status.   CompareTo(EnergyMeterStatus.Status);

            if (c == 0)
                c = Timestamp.CompareTo(EnergyMeterStatus.Timestamp);

            return c;

        }

        #endregion

        #endregion

        #region IEquatable<EnergyMeterStatus> Members

        #region Equals(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public override Boolean Equals(Object? Object)

            => Object is EnergyMeterStatus energyMeterStatus &&
                   Equals(energyMeterStatus);

        #endregion

        #region Equals(EnergyMeterStatus)

        /// <summary>
        /// Compares two EnergyMeter identifications for equality.
        /// </summary>
        /// <param name="EnergyMeterStatus">A energy meter identification to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public Boolean Equals(EnergyMeterStatus? EnergyMeterStatus)

            => EnergyMeterStatus is not null              &&
               Id.       Equals(EnergyMeterStatus.Id)     &&
               Status.   Equals(EnergyMeterStatus.Status) &&
               Timestamp.Equals(EnergyMeterStatus.Timestamp);

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
                             Timestamp.ToISO8601());

        #endregion

    }

}
