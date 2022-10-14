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
    /// Extension methods for the EVSE admin status.
    /// </summary>
    public static class EVSEAdminStatusExtensions
    {

        #region ToJSON(this EVSEAdminStatus, Skip = null, Take = null)

        public static JObject ToJSON(this IEnumerable<EVSEAdminStatus>  EVSEAdminStatus,
                                     UInt64?                            Skip  = null,
                                     UInt64?                            Take  = null)
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

                else if (filteredStatus[status.Id].Status.Timestamp >= status.Status.Timestamp)
                    filteredStatus[status.Id] = status;

            }

            #endregion


            return new JObject((Take.HasValue ? filteredStatus.OrderBy(status => status.Key).Skip(Skip).Take(Take)
                                              : filteredStatus.OrderBy(status => status.Key).Skip(Skip)).

                                   Select(kvp => new JProperty(kvp.Key.ToString(),
                                                               new JArray(kvp.Value.Status.Timestamp.ToIso8601(),
                                                                          kvp.Value.Status.Value.    ToString())
                                                              )));

        }

        #endregion

        #region Contains(this EVSEAdminStatus, Id, Status)

        /// <summary>
        /// Check if the given enumeration of EVSEs and their current admin status
        /// contains the given pair of EVSE identification and admin status.
        /// </summary>
        /// <param name="EVSEAdminStatus">An enumeration of EVSEs and their current admin status.</param>
        /// <param name="Id">A EVSE identification.</param>
        /// <param name="Status">A EVSE admin status.</param>
        public static Boolean Contains(this IEnumerable<EVSEAdminStatus>  EVSEAdminStatus,
                                       EVSE_Id                            Id,
                                       EVSEAdminStatusTypes               Status)
        {

            foreach (var adminStatus in EVSEAdminStatus)
            {
                if (adminStatus.Id     == Id &&
                    adminStatus.Status == Status)
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
    public class EVSEAdminStatus : AInternalData,
                                   IEquatable<EVSEAdminStatus>,
                                   IComparable<EVSEAdminStatus>
    {

        #region Properties

        /// <summary>
        /// The unique identification of the EVSE.
        /// </summary>
        public EVSE_Id                            Id        { get; }

        /// <summary>
        /// The current timestamped admin status of the EVSE.
        /// </summary>
        public Timestamped<EVSEAdminStatusTypes>  Status    { get; }

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new EVSE admin status.
        /// </summary>
        /// <param name="Id">The unique identification of the EVSE.</param>
        /// <param name="Status">The current timestamped admin status of the EVSE.</param>
        /// <param name="CustomData">An optional dictionary of customer-specific data.</param>
        public EVSEAdminStatus(EVSE_Id                            Id,
                               Timestamped<EVSEAdminStatusTypes>  Status,
                               JObject?                           CustomData     = null,
                               UserDefinedDictionary?             InternalData   = null)

            : base(CustomData,
                   InternalData)

        {

            this.Id      = Id;
            this.Status  = Status;

        }

        #endregion


        #region (static) Snapshot(EVSE)

        /// <summary>
        /// Take a snapshot of the current EVSE admin status.
        /// </summary>
        /// <param name="EVSE">An EVSE.</param>
        public static EVSEAdminStatus Snapshot(EVSE EVSE)

            => new EVSEAdminStatus(EVSE.Id,
                                   EVSE.AdminStatus);

        #endregion


        #region Operator overloading

        #region Operator == (EVSEAdminStatus1, EVSEAdminStatus2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EVSEAdminStatus1">An EVSE admin status.</param>
        /// <param name="EVSEAdminStatus2">Another EVSE admin status.</param>
        /// <returns>true|false</returns>
        public static Boolean operator == (EVSEAdminStatus EVSEAdminStatus1, EVSEAdminStatus EVSEAdminStatus2)
        {

            // If both are null, or both are same instance, return true.
            if (ReferenceEquals(EVSEAdminStatus1, EVSEAdminStatus2))
                return true;

            // If one is null, but not both, return false.
            if (((Object) EVSEAdminStatus1 == null) || ((Object) EVSEAdminStatus2 == null))
                return false;

            return EVSEAdminStatus1.Equals(EVSEAdminStatus2);

        }

        #endregion

        #region Operator != (EVSEAdminStatus1, EVSEAdminStatus2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EVSEAdminStatus1">An EVSE admin status.</param>
        /// <param name="EVSEAdminStatus2">Another EVSE admin status.</param>
        /// <returns>true|false</returns>
        public static Boolean operator != (EVSEAdminStatus EVSEAdminStatus1, EVSEAdminStatus EVSEAdminStatus2)
            => !(EVSEAdminStatus1 == EVSEAdminStatus2);

        #endregion

        #region Operator <  (EVSEAdminStatus1, EVSEAdminStatus2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EVSEAdminStatus1">An EVSE admin status.</param>
        /// <param name="EVSEAdminStatus2">Another EVSE admin status.</param>
        /// <returns>true|false</returns>
        public static Boolean operator < (EVSEAdminStatus EVSEAdminStatus1, EVSEAdminStatus EVSEAdminStatus2)
        {

            if ((Object) EVSEAdminStatus1 == null)
                throw new ArgumentNullException(nameof(EVSEAdminStatus1), "The given EVSEAdminStatus1 must not be null!");

            return EVSEAdminStatus1.CompareTo(EVSEAdminStatus2) < 0;

        }

        #endregion

        #region Operator <= (EVSEAdminStatus1, EVSEAdminStatus2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EVSEAdminStatus1">An EVSE admin status.</param>
        /// <param name="EVSEAdminStatus2">Another EVSE admin status.</param>
        /// <returns>true|false</returns>
        public static Boolean operator <= (EVSEAdminStatus EVSEAdminStatus1, EVSEAdminStatus EVSEAdminStatus2)
            => !(EVSEAdminStatus1 > EVSEAdminStatus2);

        #endregion

        #region Operator >  (EVSEAdminStatus1, EVSEAdminStatus2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EVSEAdminStatus1">An EVSE admin status.</param>
        /// <param name="EVSEAdminStatus2">Another EVSE admin status.</param>
        /// <returns>true|false</returns>
        public static Boolean operator > (EVSEAdminStatus EVSEAdminStatus1, EVSEAdminStatus EVSEAdminStatus2)
        {

            if ((Object) EVSEAdminStatus1 == null)
                throw new ArgumentNullException(nameof(EVSEAdminStatus1), "The given EVSEAdminStatus1 must not be null!");

            return EVSEAdminStatus1.CompareTo(EVSEAdminStatus2) > 0;

        }

        #endregion

        #region Operator >= (EVSEAdminStatus1, EVSEAdminStatus2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EVSEAdminStatus1">An EVSE admin status.</param>
        /// <param name="EVSEAdminStatus2">Another EVSE admin status.</param>
        /// <returns>true|false</returns>
        public static Boolean operator >= (EVSEAdminStatus EVSEAdminStatus1, EVSEAdminStatus EVSEAdminStatus2)
            => !(EVSEAdminStatus1 < EVSEAdminStatus2);

        #endregion

        #endregion

        #region IComparable<EVSEAdminStatus> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        public Int32 CompareTo(Object Object)
        {

            if (Object == null)
                throw new ArgumentNullException(nameof(Object), "The given object must not be null!");

            if (!(Object is EVSEAdminStatus))
                throw new ArgumentException("The given object is not a EVSEAdminStatus!",
                                            nameof(Object));

            return CompareTo((EVSEAdminStatus) Object);

        }

        #endregion

        #region CompareTo(EVSEAdminStatus)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EVSEAdminStatus">An object to compare with.</param>
        public Int32 CompareTo(EVSEAdminStatus EVSEAdminStatus)
        {

            if ((Object) EVSEAdminStatus == null)
                throw new ArgumentNullException(nameof(EVSEAdminStatus), "The given EVSEAdminStatus must not be null!");

            // Compare EVSE Ids
            var _Result = Id.CompareTo(EVSEAdminStatus.Id);

            // If equal: Compare EVSE status
            if (_Result == 0)
                _Result = Status.CompareTo(EVSEAdminStatus.Status);

            return _Result;

        }

        #endregion

        #endregion

        #region IEquatable<EVSEAdminStatus> Members

        #region Equals(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        /// <returns>true|false</returns>
        public override Boolean Equals(Object Object)
        {

            if (Object == null)
                return false;

            if (!(Object is EVSEAdminStatus))
                return false;

            return Equals((EVSEAdminStatus) Object);

        }

        #endregion

        #region Equals(EVSEAdminStatus)

        /// <summary>
        /// Compares two EVSE identifications for equality.
        /// </summary>
        /// <param name="EVSEAdminStatus">An EVSE identification to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public Boolean Equals(EVSEAdminStatus EVSEAdminStatus)
        {

            if ((Object) EVSEAdminStatus == null)
                return false;

            return Id.         Equals(EVSEAdminStatus.Id) &&
                   Status.Equals(EVSEAdminStatus.Status);

        }

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

                return Id.    GetHashCode() * 5 ^
                       Status.GetHashCode();

            }
        }

        #endregion

        #region (override) ToString()

        /// <summary>
        /// Return a text representation of this object.
        /// </summary>
        public override String ToString()

            => String.Concat(Id, " -> ",
                             Status.Value,
                             " since ",
                             Status.Timestamp.ToIso8601());

        #endregion

    }

}
